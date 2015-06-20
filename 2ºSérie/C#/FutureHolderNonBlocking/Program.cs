using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FutureHolderNonBlocking
{
    class FutureHolderNonBlocking<T> where T:class
    {

        private volatile int _hasValue;
        private T _newValue=default(T);

        public FutureHolderNonBlocking()
        {
            if(_newValue==default(T))
                 _hasValue = 0;
        }
        public void SetValue(T value)
        {
            if (IsValueAvailable())
                return;

            T actualValue = _newValue;
            Interlocked.CompareExchange(ref _newValue, value,actualValue);
        }

        public T GetValue(long timeout)
        {
            if (timeout == 0)
                return default(T);

            int initialTime = (timeout != Timeout.Infinite) ? Environment.TickCount : 0;
             do
                {

                    if (IsValueAvailable())
                    {
                        int currentHasValue = _hasValue;
                        Interlocked.CompareExchange(ref _hasValue,0, currentHasValue);
                        return _newValue;

                    }
                } while (timeout > 0);
            
            return default(T);
        }

        public bool IsValueAvailable()
        {
            return _hasValue==1;
        }

    }
}
