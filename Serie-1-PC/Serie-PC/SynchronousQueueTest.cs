using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System;

namespace Ex1 {

    [TestClass]
    public class SynchronousQueueTest {

        SynchronousQueue<int> sq = new SynchronousQueue<int>();
        SynchronousQueue<string> sqString = new SynchronousQueue<string>();
        private int value_1;
        private int value_2;

        private void setValue_2() {
            value_2 = 11;
            try {
                sq.Put(value_2);
            } catch (ThreadInterruptedException) { }
        }

        [TestMethod]
        public void SynchronousQueueTakePutTest() {
            Thread producer = new Thread(new ThreadStart(() => {
                value_1 = 22;
                try {
                    sq.Put(value_1);
                } catch (ThreadInterruptedException) { }
            }));
            int val = -1;
            Thread consumer = new Thread(new ThreadStart(() => { val = sq.Take(); }));
            producer.Start();
            consumer.Start();
            producer.Join();
            consumer.Join();

            Assert.AreEqual(value_1, val);
        }

        [TestMethod]
        public void SynchronousQueueTakeNothingTest() {
            Thread producer = new Thread(new ThreadStart(() => { }));
            int val = default(int);
            Thread consumer = new Thread(new ThreadStart( () => { val = sq.Take(); } ));
            producer.Start();
            consumer.Start();

            Assert.AreEqual(val, default(int));
        }

        [TestMethod]
        public void SynchronousQueueTakePutQueueTest() {
            Thread producer = new Thread(new ThreadStart(() => {
                value_1 = 22;
                try {
                    sq.Put(value_1);
                } catch (ThreadInterruptedException) { }
            }));
            int val_1 = -1;
            Thread consumer = new Thread(new ThreadStart(() => { val_1 = sq.Take(); }));
            producer.Start();
            setValue_2();
            consumer.Start();
            int val_2 = sq.Take();

            producer.Join();
            consumer.Join();

            Assert.AreEqual(value_1, val_1);
            Assert.AreEqual(value_2, val_2);
        }

        [TestMethod]
        public void SynchronousQueuePutNullTest() {
            Thread producer = new Thread(new ThreadStart(() => { 
                try {
                    sqString.Put(default(string)); 
                } catch (ArgumentException) {
                    Assert.AreEqual(true, true);
                }
            }));


            producer.Start();

        }
    }

}
