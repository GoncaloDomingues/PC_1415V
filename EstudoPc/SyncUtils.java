
public class SyncUtils {

   public static long adjustTimeout(long startTime, long timeout) {
       if(timeout != 0) {
           long now = System.currentTimeMillis();
           long elapsed = (now == startTime) ? 1 : now - startTime;
            
           if(elapsed >= timeout)
               timeout = 0;
           else{
               timeout -= elapsed;
               startTime = now;
           }
       }
       return timeout;
   }

}
