using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UnsafeSpinReadWriteLock
{
    #pragma warning disable 420
    public class UnsafeSpinReadWriteLock
    {
        private volatile int state; // 0 means free, -1 means writing, and > 0 means reading
        public void LockRead()
        {
            SpinWait sw = new SpinWait();
            while (state < 0)
                sw.SpinOnce();
            Interlocked.Increment(ref state);
        }
        public void LockWrite()
        {
            SpinWait sw = new SpinWait();
            while (state != 0)
                sw.SpinOnce();

            int lastState = state;
            Interlocked.CompareExchange(ref state, -1, lastState);
        }
        public void UnlockRead() { Interlocked.Decrement(ref state); }
        public void UnlockWrite() { 
            int lastState = state;
            Interlocked.CompareExchange(ref state, 0, lastState);

        }
    }

}
