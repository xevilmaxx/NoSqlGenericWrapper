using System;
using System.Threading;

namespace LibRtDb.Locking
{
    /// <summary>
    /// Custom class created to make cleaner code writing for acquirung and releasing locks
    /// <para />
    /// Also auto logs and warns about incorrect usage with Error level logs
    /// </summary>
    public class GlobalMutex
    {

        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Once generated in constructor this is never supposed to change
        /// </summary>
        private readonly Mutex MutexLock;

        /// <summary>
        /// A variable needed to avoid releasing or acquiring lock in wrong order, also avoids exceptions
        /// </summary>
        private bool IsPreviouslyAcquired = false;


        /// <summary>
        /// Mutex Name Without Global Prefix
        /// </summary>
        public readonly string MutexName;

        /// <summary>
        /// Mutex Name With Global Prefix
        /// </summary>
        //public readonly string FullMutexName;

        /// <summary>
        /// Global Prefix will be prepended automatically, just give a unique name
        /// </summary>
        /// <param name="MutexName"></param>
        public GlobalMutex(string MutexName)
        {
            try
            {

                //The most important part is: Global\\ which will ensure that lock has SYSTEM WIDE validity
                //Added additional naming prefix to ensure uniqueness if mutex in system and avoid accidental interference with
                //other possible locks
                var fullMutexName = $"Global\\GlobalMutexLocks_{MutexName}";

                //InitiallyOwned = false, helps to ensure that it's a GLOBAL Mutex, otherwise it might not work 
                MutexLock = new Mutex(false, fullMutexName);
                this.MutexName = fullMutexName;
                
                //this.MutexName = MutexName;
                //this.FullMutexName = fullMutexName;

                log.Trace($"Defined global Mutex -> {fullMutexName}");

            }
            catch (Exception ex)
            {
                log.Error(ex, $"!!!Try to execute as Administrator of PC to make it work!!! {MutexName}");
            }
        }

        /// <summary>
        /// Will ensure that anyone who will try to do that on same machine will need to wait until you release this lock
        /// <para />
        /// Place at the beginning of the method
        /// </summary>
        /// <returns></returns>
        public bool AcquireLock()
        {
            //Acquire GLOBAL mutex, so eventually others can continue
            try
            {

                if (IsPreviouslyAcquired == true)
                {
                    log.Error($"Lock previously Acquired, but not released! {MutexName}");
                    return false;
                }
                else
                {
                    if(MutexLock != null)
                    {
                        MutexLock.WaitOne();
                        IsPreviouslyAcquired = true;
                        log.Trace($"Lock Acquired! {MutexName}");
                        return true;
                    }
                    else
                    {
                        log.Warn($"Cannot Acquire Lock for {MutexName}, as it probably was never created properly, eventually execute with Administrative Previledge");
                        return false;
                    }
                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }

        /// <summary>
        /// Will release previously acquired lock globally, so that others can Acquire lock on resource
        /// <para />
        /// Place at the end of the method
        /// </summary>
        /// <returns></returns>
        public bool ReleaseLock()
        {
            //Release GLOBAL mutex, so eventually others can continue
            try
            {

                if (IsPreviouslyAcquired == true)
                {
                    if(MutexLock != null)
                    {
                        MutexLock.ReleaseMutex();
                        IsPreviouslyAcquired = false;
                        log.Trace($"Lock Released! {MutexName}");
                        return true;
                    }
                    else
                    {
                        //probably never reachable case due to the presence of bool protector, anyway will keep it here just in case
                        log.Warn($"Cannot Release Lock for {MutexName}, as it probably was never created properly, eventually execute with Administrative Previledge");
                        return false;
                    }                    
                }
                else
                {
                    log.Error($"Lock wasnt previously Acquired! {MutexName}");
                    return false;
                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }

    }
}
