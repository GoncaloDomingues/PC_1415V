using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex1 {
    
    public class Rw_Semaphore {

        private readonly object lockObj = new object();
        private bool readerNwriter; //true = reader; false = writer;

        public void DownRead() {

        }

        public void DownWrite() {

        }

        public void UpRead() {

        }

        public void UpWrite() {

        }

        public void DowngradeWriter() {
            lock (lockObj) {

            }
        }

    }

}
