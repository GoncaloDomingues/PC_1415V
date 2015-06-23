using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class UnsafeCountdownLatch
    {
        private int count;
        private readonly ManualResetEvent waitEvent = new ManualResetEvent(false);
        public UnsafeCountdownLatch(int c)
        {
            if (c <= 0) throw new InvalidOperationException();
                int lastValue=count;
                Interlocked.CompareExchange(ref count, c, lastValue);

        }
        public bool Wait(int timeout)
        {
            if (count == 0) return true;
            waitEvent.WaitOne(timeout);
            return count == 0;
        }
        public void Signal()
        {
            if (count == 0) throw new InvalidOperationException();
            if (--count == 0) waitEvent.Set();
        }
        public void AddCount()
        {
            if (count == 0 || count + 1 < 0) throw new InvalidOperationException();
            count++;
        }
    }
}
