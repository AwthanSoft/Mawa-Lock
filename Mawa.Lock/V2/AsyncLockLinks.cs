

//https://davidsekar.com/c-sharp/dont-lock-on-async-tasks
//How do we wait async ?
//System.Threading assembly comes with Semaphore and SemaphoreSlim that helps you to control concurrent access to the shared resource.

//Semaphore - Supports local semaphore for an application and System semaphore for synchronization across multiple process (IPC - inter-process communications).
//SemaphoreSlim - Just supports local semaphore that is at an application level

//For a standalone application or web applications, SemaphoreSlim is more that enough to handle all concurrency controls. With SemaphoreSlim, you can asynchronously await for the lock to be released.

//So in same scenario, now a thread from the Threadpool will be performing the shared write operation on that batchId,.. while other 9 threads with the need for shared access will be suspended till the first thread IO Completion and SemaphoreSlim release. 




//https://www.rocksolidknowledge.com/articles/locking-and-asyncawait




