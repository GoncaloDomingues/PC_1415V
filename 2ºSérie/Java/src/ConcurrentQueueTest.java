/**
 * Created by Goncalo Domingues on 20/06/2015.
 */

import org.junit.Assert;
import org.junit.Before;
import org.junit.Test;

public class ConcurrentQueueTest {

    ConcurrentQueue<Integer> empty;
    ConcurrentQueue<Integer> list;

    @Before
    public void doList(){
        empty = new ConcurrentQueue<>();
        list = new ConcurrentQueue<>();

        for(int i=0;i<10;i++)
            list.put(i);
    }

    @Test
    public void emptyTake(){
        Assert.assertNull(empty.tryTake());
    }

    @Test
    public void emptyPut(){
        empty.put(new Integer(10));
        Assert.assertEquals(new Integer(10), empty.tryTake());

    }

    @Test
    public void listTake(){
        Assert.assertEquals(new Integer(0),list.tryTake());
    }

}
