using System;

namespace Mawa.Lock
{
    public class LockForLast : IDisposable
    {

        private bool is_Open = false;
        private bool is_inUsing = false;
        private object opening_Lock;
        /********* self Info ***********/
        private int sum_error = 0;

        ObjectLock lastId_lock;
        string _last_id = string.Empty;
        string _running_id = string.Empty;
        string GetId()
        {
            lastId_lock.open_lock();
            string id = Randoms.RandomId.ProcessLockerId();
            _last_id = id;
            lastId_lock.close_lock();
            return id;
        }

        public bool IsThereWaiter
        {
            get
            {
                if (string.IsNullOrEmpty(_last_id))
                    return false;

                if (_running_id.Equals(_last_id))
                    return false;
                else
                    return true;
            }
        }


        public bool isContinue()
        {
            string waiter_id = GetId();
            lock (opening_Lock)
            {
                while (this.is_Open == true)
                {
                    if (_last_id != waiter_id)
                        return false;
                }
                // as temp
                if (_last_id != waiter_id)
                    return false;
                this.is_Open = true;
                this.is_inUsing = true;
                _running_id = waiter_id;
            }
            return true;
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
                    throw new Exception();
                }
                this.is_Open = false;
                this.is_inUsing = false;
            }
        }

        public LockForLast()
        {
            lastId_lock = new ObjectLock();
            opening_Lock = new object();
        }

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
                lastId_lock.Dispose();
                lastId_lock = null;
                opening_Lock = null;

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
        ~LockForLast()
        {
            Dispose(false);
        }
        #endregion
    }
}
