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
        T _newValue;
        private readonly object toLock = new object();

    
        public void SetValue(T value)
        {
            lock (toLock)
            {
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

                try
                {
                    Monitor.Wait(toLock);
                }
                catch (ThreadInterruptedException)
                {
                    throw new ThreadInterruptedException();
                }

                if (IsValueAvailable())
                {
                    Monitor.Pulse(toLock);
                    return _newValue;
                }

                int timeFinal = (int)timeout;


                if (SyncUtils.AdjustTimeout(ref _lastTime, ref timeFinal) == 0)
                {
                    Monitor.Pulse(this);
                    return default(T);
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