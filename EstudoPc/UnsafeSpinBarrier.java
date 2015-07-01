package ex1;

import java.util.concurrent.atomic.AtomicInteger;

public class UnsafeSpinBarrier {
	
	public volatile AtomicInteger toArrive;
	
	/*
	 * os tipos primitivos não são thread-safe, pois as escritas
	 * podem não ser atómicas.
	 * Quando à resolução da Barrier os métodos addPartner 
	 * e removePartner estão errados: em ambos é necessário 
	 * garantir atomicidade do teste com a alteração do estado,
	 * o que não está a fazer. (nota: não fazer toArrive.get() == 0)
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
