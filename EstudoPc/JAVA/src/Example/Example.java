package Example;

/**
 * Created by Goncalo Domingues on 22/06/2015.
 */
public class Example{
    public static void main(String[]args){
        Counter counter=new Counter();

        Thread threadA=new CounterThread(counter);
        Thread threadB = new CounterThread(counter);

        threadA.start();
        threadB.start();
    }
}
