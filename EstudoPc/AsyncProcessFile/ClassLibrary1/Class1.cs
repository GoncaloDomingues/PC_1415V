using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
namespace UltimoTeste
{
    public class AsyncProcessFile
    {
        public AsyncProcessFile(string filePath)
        { }
        public IAsyncResult BeginProcess(AsyncCallback callback, object state)
        {
            return null;
        }
        public long EndProcess(IAsyncResult asyncResult)
        {
            return 1;
        }
        public Task<long> ProcessAsync();
    }

    public static class AsyncProcessFiles
    {

        public static IAsyncResult BeginProcessFiles(string[] filePaths, Func<long> initial, Func<long, long, long> reducer, AsyncCallback callback, object state)
        {
            var gar = new GenericAsyncResult<long>(callback, state);
            AsyncCallback onBeginProcess = null;
            int count = filePaths.Length;

            AsyncProcessFile f = null;

            long initialLong = initial();

            onBeginProcess = (ar) =>
            {
                initialLong = reducer(initialLong, f.EndProcess(ar));

                if (Interlocked.Decrement(ref count) == 0)
                    gar.OnComplete(initialLong, null);
            };

            foreach (string file in filePaths)
            {
                AsyncProcessFile af = new AsyncProcessFile(file);
                f = af;
                af.BeginProcess(onBeginProcess, state);
            }
            return gar;
        }

        public static long EndProcessFiles(IAsyncResult asyncResult)
        {
            return ((GenericAsyncResult<long>)asyncResult).Result;
        }


        public static async Task<long> ProcessFilesAsync(string[] filePaths, Func<long> initial, Func<long, long, long> reducer)
        {
            long initialLong = initial();

            foreach (string file in filePaths)
            {
                AsyncProcessFile af = new AsyncProcessFile(file);

                initialLong =  reducer(initialLong, await af.ProcessAsync());
              
            }
            return initialLong;
        }
    }
}
