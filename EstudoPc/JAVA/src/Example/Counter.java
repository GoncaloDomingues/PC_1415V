package Example;

/**
 * Created by Goncalo Domingues on 22/06/2015.
 */
public class Counter {

    long count=0;

    public synchronized void add(long value){
        this.count+=count;
    }
}
