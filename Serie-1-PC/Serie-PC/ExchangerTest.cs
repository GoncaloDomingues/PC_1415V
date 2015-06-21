using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ex1 {

    [TestClass]
    public class ExchangerTest {

        [TestMethod]
        public void ExchangerMessageExchangeTest() {
            Exchanger<string> ex = new Exchanger<string>();

            string msg1 = "This is message 1!";
            string msg2 = "Ohhh and this message2!";
            int timeout = 5000;
            
            Thread producer = new Thread(new ThreadStart( () => {
                string val = null;
                try {
                    val = ex.Exchange(msg1, timeout);
                } catch (ThreadInterruptedException) {

                }
                Assert.IsNotNull(val);
                Assert.AreEqual(msg2, val);
            }));

            Thread consumer = new Thread(new ThreadStart(() => {
                string ret = null;
                try {
                    ret = ex.Exchange(msg2, timeout);
                } catch (ThreadInterruptedException) {

                }
                Assert.IsNotNull(ret);
                Assert.AreEqual(msg1, ret);    
            }));

            producer.Start();
            consumer.Start();

            producer.Join();
            consumer.Join();
        }

    }

}
