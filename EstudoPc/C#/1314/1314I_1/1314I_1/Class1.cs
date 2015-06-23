using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _1314I_1
{ /*    public class UnsafeSpinCompletion
    {
        const int ALL = int.MaxValue;
        private int state = 0;
        public void Wait()
        {
            if (state == ALL)
                return;
            SpinWait sw = new SpinWait();
            while (state == 0)
                sw.SpinOnce();
            if (state != ALL)
                state--;
        }
        public void Complete()
        {
            if (state != ALL)
                state++;
        }
        public void CompleteAll()
        {
            state = ALL;
        }
    }*/

#pragma warning disable 420
    public class UnsafeSpinCompletion
    {
        const int ALL = int.MaxValue;
        private volatile int state = 0;
        public void Wait()
        {
            if (state == ALL)
                return;

            SpinWait sw = new SpinWait();
            while (state == 0)
                sw.SpinOnce();

            if (state != ALL)
                Interlocked.Decrement(ref state);
        }
        public void Complete()
        {
            if (state != ALL)
                Interlocked.Increment(ref state);
        }
        public void CompleteAll()
        {
            state = ALL;
        }
    }
}
