package ex1;

import org.junit.Assert;
import org.junit.Test;

public class ConcurrentQueueTest {
	
	@Test
	public void trytakeFromEmptyListTest(){
		final ConcurrentQueue<Integer> buffer = new ConcurrentQueue<>();
		Integer i = buffer.tryTake();
		Assert.assertNull(i);
	}
	
	@Test
	public void testSingleProductionAndConsuptionOneValue() throws InterruptedException{
		final ConcurrentQueue<Integer> buffer = new ConcurrentQueue<>();
		final int VALUE = 5;
		
		Thread producer = new Thread(){
			@Override
			public void run() {
				buffer.put(VALUE);
			}
		};
		
		Thread consumer = new Thread() {
			@Override
			public void run() {
				Integer val = buffer.tryTake();
				Assert.assertNotNull(val);
				Assert.assertEquals(VALUE, val.intValue());
			}
		};
		producer.start();
		consumer.start();
		
		producer.join();
		consumer.join();
	}

}
