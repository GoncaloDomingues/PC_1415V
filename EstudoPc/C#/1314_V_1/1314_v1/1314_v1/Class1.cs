using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _1314_v1
{ /*public class UnsafeSpinSemaphore
    {
        private int permits;
        public UnsafeSpinSemaphore(int initial)
        {
            if (initial < 0) throw new InvalidOperationException();
            permits = initial;
        }
        public void Acquire(int permitsToAcquire)
        {
            if (permits >= permitsToAcquire)
            {
                permits -= permitsToAcquire;
                return;
            }
            SpinWait sw = new SpinWait();
            do
            {
                sw.SpinOnce();
            } while (permits < permitsToAcquire);
            permits -= permitsToAcquire;
        }
        public void Release(int permitsToRelease)
        {
            if (permits + permitsToRelease < permits) throw new InvalidOperationException();
            permits += permitsToRelease;
        }
    }*/

    #pragma warning disable 420
    public class UnsafeSpinSemaphore
    {
        private volatile int permits;
        public UnsafeSpinSemaphore(int initial)
        {
            if (initial < 0) throw new InvalidOperationException();
            int lastValue = initial;
            Interlocked.CompareExchange(ref permits, initial, lastValue);
        }
        public void Acquire(int permitsToAcquire)
        {
            int lastValue = permits;
            permitsToAcquire=0-permitsToAcquire;

            if (lastValue >= permitsToAcquire)
            {
                Interlocked.Add(ref permits, permitsToAcquire);
             
                return;
            
            }
            SpinWait sw = new SpinWait();
            do
            {
                sw.SpinOnce();
            } while (permits < permitsToAcquire);

            Interlocked.Add(ref permits, permitsToAcquire);
        }
        public void Release(int permitsToRelease)
        {
            int lastValue = permits;
            if (lastValue + permitsToRelease < permits) throw new InvalidOperationException();

            Interlocked.Add(ref permits, permitsToRelease);
        }

    }
}
