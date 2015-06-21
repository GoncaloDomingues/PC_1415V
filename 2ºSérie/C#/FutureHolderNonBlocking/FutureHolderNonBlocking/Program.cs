using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FutureHolderNonBlocking
{
    class FutureHolderNonBlocking<T> where T : class
    {

        private volatile bool _hasValue = false;
        private T _newValue = default(T);




        public void SetValue(T value)
        {
            if (IsValueAvailable())
                return;

            if ((Interlocked.CompareExchange(ref _newValue, value, default(T))) == default(T))
            {
                _hasValue = true;
                lock (this)
                {
                    Monitor.PulseAll(this);
                }
            }
        }

        public T GetValue(long timeout)
        {


            if (IsValueAvailable())
            {
                return _newValue;

            }

            lock (this)
            {
                try
                {
                    Monitor.Wait(this, (int)timeout);
                    return _newValue;
                }
                catch (ThreadInterruptedException)
                {
                    if (IsValueAvailable())
                    {
                        Thread.CurrentThread.Interrupt();
                        return _newValue;
                    }
                    throw;
                }

            }

        }

        public bool IsValueAvailable()
        {
            return _hasValue;
        }

    }
}
