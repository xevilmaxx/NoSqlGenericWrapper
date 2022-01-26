using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;


/// <summary>
/// ORIGIN:
/// https://github.com/mlockett42/litedb-async
/// </summary>

//If you wish to use ThreadPool instead of 1 single thread you may consider using:
//https://github.com/amibar/SmartThreadPool
//Which is good, but you wont be able to use async keyword
//And you will need to change some interfaces
//I assume i won't need more than 1 separated thread on each microservice, so for now i won't use it
//This has also a bit of advantage as i dont allow programmers to overkill host system with multiple pools on microservices
//at most 1 additional thread for each microservice in ParkO ecosystem is more than enough
//Otherwise you may use ThreadPool library externally where it's really needed without compromising current interfaces
//For now this implements 1 separated thread with queue, so slow tasks may be delegated to background thread, but one at time
//This class will be created only on first Async request, if you don't use it, this will be never allocated
namespace LibRtDb.GenericNoSql
{
    /// <summary>
    /// You can enqueue different sync tasks here, in order to archive async ways
    /// <para/>
    /// This is singleton, becouse:
    /// <para/>
    /// 1. We want to access to it from different places without passing it around
    /// <para/>
    /// 2. Once created typically we don't want to re-create it again
    /// <para/>
    /// 3. We won't create it at all if you dont use Async functionality on runtime
    /// </summary>
    public class GenericAsyncEngine
    {

        #region Singleton
        private static readonly Lazy<GenericAsyncEngine> lazy =
        new Lazy<GenericAsyncEngine>(() => new GenericAsyncEngine());

        public static GenericAsyncEngine Instance { get { return lazy.Value; } }
        #endregion

        private readonly Thread _backgroundThread;
        private readonly SemaphoreSlim _newTaskArrived = new SemaphoreSlim(initialCount: 0, maxCount: int.MaxValue);
        private readonly CancellationTokenSource _shouldTerminate = new CancellationTokenSource();
        private readonly ConcurrentQueue<Action> _queue = new ConcurrentQueue<Action>();

        public GenericAsyncEngine()
        {
            _backgroundThread = new Thread(BackgroundLoop);
            _backgroundThread.Start();
        }

        /// <summary>
        /// This is not an infinite loop polling,
        /// it stops when no requests are arriving thanks to .Wait(terminationToken)
        /// So it's good enough
        /// </summary>
        private void BackgroundLoop()
        {
            var terminationToken = _shouldTerminate.Token;

            try
            {
                while (!terminationToken.IsCancellationRequested)
                {
                    _newTaskArrived.Wait(terminationToken);

                    if (!_queue.TryDequeue(out var function)) continue;

                    function();
                }
            }
            catch (OperationCanceledException) when (terminationToken.IsCancellationRequested)
            {
                // it's OK, we're exiting
            }
        }

        /// <summary>
        /// Enqueue task with some return type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="function"></param>
        /// <returns></returns>
        public Task<T> EnqueueAsync<T>(Func<T> function)
        {
            var tcs = new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);

            void Function()
            {
                try
                {
                    tcs.SetResult(function());
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            }

            _queue.Enqueue(Function);
            _newTaskArrived.Release();
            return tcs.Task;
        }

        /// <summary>
        /// Enqueue task with VOID return type
        /// </summary>
        /// <param name="function"></param>
        /// <returns></returns>
        public Task EnqueueAsync(Action function)
        {
            var tcs = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

            void Function()
            {
                try
                {
                    function();
                    tcs.SetResult();
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            }

            _queue.Enqueue(Function);
            _newTaskArrived.Release();
            return tcs.Task;
        }

        /// <summary>
        /// Release resources in appropriate way
        /// </summary>
        /// <param name="disposing"></param>
        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                using (_shouldTerminate)
                using (_newTaskArrived)
                {
                    _shouldTerminate.Cancel();

                    // give the thread 5 seconds to exit... must not block forever here
                    _backgroundThread.Join(TimeSpan.FromSeconds(5));
                }
            }
        }

        /// <summary>
        /// Release resources in appropriate way
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
