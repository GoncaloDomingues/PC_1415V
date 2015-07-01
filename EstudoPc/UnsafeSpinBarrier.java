package ex1;

import java.util.concurrent.atomic.AtomicInteger;

public class UnsafeSpinBarrier {
	
	public volatile AtomicInteger toArrive;
	
	/*
	 * os tipos primitivos n�o s�o thread-safe, pois as escritas
	 * podem n�o ser at�micas.
	 * Quando � resolu��o da Barrier os m�todos addPartner 
	 * e removePartner est�o errados: em ambos � necess�rio 
	 * garantir atomicidade do teste com a altera��o do estado,
	 * o que n�o est� a fazer. (nota: n�o fazer toArrive.get() == 0)
	 */
	
	public UnsafeSpinBarrier(int partners) {
		if (partners <= 0)
			throw new IllegalArgumentException();
		toArrive = new AtomicInteger(partners);
	}
	
	public void signalAndWait() {
		if (toArrive.compareAndSet(0, toArrive.get()))
			throw new IllegalStateException();
		
		if (toArrive.decrementAndGet() > 0)
			do {
				Thread.currentThread().yield();
			} while (!(toArrive.compareAndSet(0, toArrive.get())));
	}
	
	public void addPartner() {
		if (toArrive.compareAndSet(0, toArrive.get()))
			throw new IllegalStateException();
		toArrive.incrementAndGet();
	}
	
	public void removePartner() {
		toArrive.decrementAndGet();
		if (toArrive.compareAndSet(0, toArrive.get()))
			throw new IllegalStateException();
	}
	
}
