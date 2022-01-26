namespace LibRtDb.Locking
{
    /// <summary>
    /// Used for most Heavy operations in system to avoid system halt under many microservices pressure
    /// </summary>
    public static class GlobalMutexLocks
    {
                
        //GLOBAL MUTEX ACROSS PROCESSES
        //Global is really necessary, otherwise on linux it wont work properly
        //Please avoid manually duplicated Names !!!
        //Those Locs are supposed to be used in Syncronous scenario, don't use them in Tasks or Async methods
        public static GlobalMutex MartenDb = new GlobalMutex("MartenMutex");
        public static GlobalMutex GrpcServer = new GlobalMutex("GrpcServer");
        public static GlobalMutex GrpcClient = new GlobalMutex("GrpcClient");

    }
}
