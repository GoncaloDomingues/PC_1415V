package ex2;

import java.io.InvalidObjectException;

import junit.framework.Assert;

import org.junit.Test;

public class FutureHolderNonBlockingTest {
	
	
	@Test
	public void futureHolderInvalidOperationExceptionTest() throws InterruptedException {
		FutureHolderNonBlocking<String> fh = new FutureHolderNonBlocking<String>();
		String testMsg = "this is a test";
		
		Thread producer = new Thread(){
			@Override
			public void run() {
				try {
					fh.setValue(testMsg);
				} catch (InvalidObjectException e) {  }
			}
		};
		
		Thread consumer = new Thread(){
			@Override
			public void run() {
				try {
					fh.setValue("This is the same test");
				} catch (InvalidObjectException e) { 
					try {
						Assert.assertEquals(fh.getValue(0), testMsg);
					} catch (InterruptedException e1) {  }
				}
			}
		};
		
		
		producer.start();
		consumer.start();
		
		producer.join();
		consumer.join();
	}

	@Test
	public void futureHolderArgumentExceptionTest() throws InterruptedException {
		FutureHolderNonBlocking<String> fh = new FutureHolderNonBlocking<String>();
		Thread producer = new Thread(){
			@Override
			public void run() {
				try {
					fh.setValue(null);
				} catch (IllegalArgumentException | InvalidObjectException e) { 
					Assert.assertEquals(true, true);
				}
			}
		};
        
		producer.start();
		
		producer.join();
        
	}

	@Test
	public void futureHolderSetValueTest() throws InterruptedException {
		FutureHolderNonBlocking<String> fh = new FutureHolderNonBlocking<String>();
        String testMsg = "This is a test";
        
        Thread producer = new Thread(){
			@Override
			public void run() {
				try {
					fh.setValue(testMsg);
				} catch (IllegalArgumentException | InvalidObjectException e) { 
					Assert.assertEquals(true, true);
				}
			}
		};
        
		 Thread consumer = new Thread(){
				@Override
				public void run() {
					try {
						String test = fh.getValue(5);
						Assert.assertEquals(test, testMsg);
					} catch (IllegalArgumentException | InterruptedException e) { 
						Assert.assertEquals(true, true);
					}
				}
		};
		
        producer.start();
        consumer.start();
        
        producer.join();
        consumer.join();
	}
	
	@Test
	public void futureHolderTimeoutTest() throws InterruptedException {
		FutureHolderNonBlocking<String> fh = new FutureHolderNonBlocking<String>();
        
		Thread producer = new Thread(){
			@Override
			public void run() {
				try {
					Assert.assertEquals(fh.getValue(5), null);
		            Assert.assertEquals(fh.getValue(0), null);
				} catch (IllegalArgumentException | InterruptedException e) { 
					Assert.assertEquals(true, true);
				}
			}
		};

        producer.start();
        
        producer.join();
	}
}
