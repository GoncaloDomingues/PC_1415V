package ex2;

import java.io.InvalidObjectException;
import java.util.concurrent.atomic.AtomicReference;

import org.hamcrest.core.IsAnything;

/*

Usando os mecanismos nonblocking, discutidas nas aulas teóricas, implemente uma versão
optimizada do sincronizador FutureHolder<T> (cuja especificação consta no exercício 1 da
primeira série de exercícios). As optimizações devem incidir sobre as situações em que o
método getValue não bloqueia a t hread invocante e quando o método s etValue não tem que
libertar nenhuma thread bloqueada pelo método getValue

*/
public class FutureHolderNonBlocking<T> {
	
	private final Object lockObj = new Object();
	private volatile boolean hasValue;
	private AtomicReference<T> obj;
	private T val;
	
	public FutureHolderNonBlocking() {
		hasValue = false;
		obj = new AtomicReference<T>();
	}
	
	public void setValue(T value) throws InvalidObjectException {
		if (value == null)
			throw new IllegalArgumentException("Null value parameter!");
		if (isValueAvailable())
			throw new InvalidObjectException("Thread already got exclusive lock!");
		if (obj.compareAndSet(value, val)) { //erro aqui...
			hasValue = true;
			synchronized(lockObj) {
				lockObj.notifyAll();
			}
		}
		
	}
	
	public T getValue(int timeout) throws InterruptedException {
		if (isValueAvailable()) {
			return val;
		}
		synchronized (lockObj) {
			try {
				lockObj.wait((int)timeout);
				hasValue = false;
				return val;
			} catch(InterruptedException e) {
				if (isValueAvailable()) {
					Thread.currentThread().interrupt();
					return val;
				}
				throw e;
			}
		}
	}

	
	private boolean isValueAvailable() {
		return hasValue;
	}
}
