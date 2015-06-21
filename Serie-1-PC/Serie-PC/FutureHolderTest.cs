using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;


namespace Ex1 {

    [TestClass]
    public class FutureHolderTest {

        [TestMethod]
        public void FutureHolderInvalidOperationExceptionTest() {
            FutureHolder<string> h = new FutureHolder<string>();
            string testMsg = "This is a test";

            Thread producer = new Thread(new ThreadStart(() => { h.SetValue(testMsg); } ));
            Thread consumer = new Thread(new ThreadStart(() => {
                try {
                    h.SetValue("This is the same test");
                } catch (InvalidOperationException) {
                    Assert.AreEqual(h.GetValue(0), testMsg);
                }
            }));

            producer.Start();
            consumer.Start();
        }

        [TestMethod]
        public void FutureHolderArgumentExceptionTest() {
            FutureHolder<string> h = new FutureHolder<string>();
            Thread producer = new Thread(new ThreadStart(() => {
                try {
                    h.SetValue(null);
                } catch (ArgumentException) {
                    Assert.AreEqual(true, true);
                }
            }));
            producer.Start();
        }

        [TestMethod]
        public void FutureHolderSetValueTest() {
            FutureHolder<string> h = new FutureHolder<string>();
            string testMsg = "This is a test";
            Thread producer = new Thread(new ThreadStart(() => { h.SetValue(testMsg); }));
            Thread consumer = new Thread(new ThreadStart(() => { Assert.AreEqual(h.GetValue(5), testMsg); }));

            producer.Start();
            consumer.Start();
        }

        [TestMethod]
        public void FutureHolderTimeoutTest() {
            FutureHolder<string> h = new FutureHolder<string>();
            Thread producer = new Thread(new ThreadStart(() => {
                Assert.AreEqual(h.GetValue(5), default(string));
                Assert.AreEqual(h.GetValue(0), default(string));
            }));
            producer.Start();
        }
    }

}
