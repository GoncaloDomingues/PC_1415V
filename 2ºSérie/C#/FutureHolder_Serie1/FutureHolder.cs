using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _1ºSérie
{
   public class FutureHolder<T>
    {
        bool _hasValue = false; 
        T _newValue=default(T);
        private readonly object toLock = new object();

    
        public void SetValue(T value)
        {
            lock (toLock)
            {
                if(IsValueAvailable())
                    throw new InvalidOperationException();

                _newValue = value;
                _hasValue = true;
                Monitor.PulseAll(toLock);
            }
        }

        public T GetValue(long timeout)
        {
            lock (toLock)
            {
                int _lastTime = (timeout != Timeout.Infinite) ? Environment.TickCount : 0;
                
                 if (IsValueAvailable())
                {
                    return _newValue;
                }


                try
                {
                    Monitor.Wait(toLock,timeout);
                    return _newValue;
                }
                catch (ThreadInterruptedException)
                {
                    if(IsValueAvailable()){
                        Thread.CurrentThread.Interrupt();
                        return _newValue;
                    }
                    throw;
                }

                return default(T);
            }
        }

        public bool IsValueAvailable()
        {
            return _hasValue;
        }
    }
}