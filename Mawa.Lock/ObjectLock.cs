using System;
using System.Threading.Tasks;

namespace Mawa.Lock
{
    public class ObjectLock : IDisposable
    {
        #region Initial
        public readonly object opening_Lock;
        private bool is_Open = false;
        private bool is_inUsing = false;
        public ObjectLock()
        {
            opening_Lock = new object();
        }


        #endregion

        #region Core
  
        /********* self Info ***********/
        private int sum_error = 0;

        /// <summary>
        /// this  will be open until the lock close
        /// </summary>
        public void open_lock()
        {
            lock (opening_Lock)
            {
                while (this.is_Open == true)
                {

                }
                this.is_Open = true;
                this.is_inUsing = true;
            }
        }

        public Task open_lock_Async()
        {
            return Task.Run(() =>
            {
                lock (opening_Lock)
                {
                    while (this.is_Open == true)
                    {

                    }
                    this.is_Open = true;
                    this.is_inUsing = true;
                }
            });
        }

        public void close_lock()
        {
            //lock (opening_Lock)
            {
                if (this.is_inUsing && this.is_Open)
                {

                }
                else
                {
                    sum_error++;
                }
                this.is_Open = false;
                this.is_inUsing = false;
            }
        }


        #endregion

        #region Execute Action Inside Lock

        /// <summary>
        /// this will be opening while the task in action
        /// will close after action end executed
        /// </summary>
        /// <remarks>This wouldn't throw execption</remarks>
        /// <param name="action">the execution code</param>
        public void ExecuteInsideLock(Action action)
        {
            lock (opening_Lock)
            {
                action.Invoke();
            }
        }

        public void ExecuteInsideLock<TArgs>(Action<TArgs> action, TArgs args)
        {
            lock (opening_Lock)
            {
                action.Invoke(args);
            }
        }

        public TResult ExecuteInsideLock<TResult>(Func<TResult> predicates)
        {
            lock (opening_Lock)
            {
                return predicates.Invoke();
            }
        }

        /// <summary>
        /// This will execute code inside lock with try & catch Exception from code if is.
        /// </summary>
        /// <param name="action">the implementation code</param>
        /// <returns>The exception from executed code if any or null if successed implemented</returns>
        public Exception TryExecuteInsideLock(Action action)
        {
            Exception temp_exception = null;
            open_lock();
            try
            {
                action();
            }
            catch(Exception ex)
            {
                temp_exception = ex;
            }
            finally
            {
                close_lock();
            }
            return temp_exception;
        }

        public void TryExecuteInsideLock<TResult>(Func<TResult> predicates, out TResult Result, out Exception exception_result)
            where TResult : class
        {
            exception_result = null;
            Result = null;
            open_lock();
            try
            {
                Result = predicates();
            }
            catch (Exception ex)
            {
                exception_result = ex;
            }
            finally
            {
                close_lock();
            }
        }

        #endregion


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                is_Open = false;
                is_inUsing = false;

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ObjectLock_Script() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }
        ~ObjectLock()
        {
            Dispose(false);
        }
        #endregion
    }
}
