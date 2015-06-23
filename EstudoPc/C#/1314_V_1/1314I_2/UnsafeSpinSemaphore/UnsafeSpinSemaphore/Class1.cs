using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace UnsafeSpinSemaphore
{
    public class UnsafeSpinSemaphore
    {
        private readonly int maximum;
        private volatile int count;
       public UnsafeSpinSemaphore(int i, int m)
        {
           int lastValue=count;
            if (i < 0 || m <= 0)
                throw new ArgumentException("i/m");
            Interlocked.CompareExchange(ref count, i, lastValue);
           maximum = m;
        }

        public void Acquire{
         
             
            SpinWait sw = new SpinWait();
                while (count == 0)
                    sw.SpinOnce();
                count -= 1;
        }
public void Release(int rs) {
if (rs < 0 || rs + count > maximum)
throw new ArgumentException("rs");
count += rs;
}
}
    }

