using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SP同步方法
{
    /// <summary>
    /// async 与串口交互
    /// </summary>
    public partial class Program
    {

        public static async Task<byte[]> GetResponse(int bufferSize, CancellationTokenSource cancelTokenSource)
        {
            var taskResult = await Task.Run(() =>
            {
                while (!cancelTokenSource.Token.IsCancellationRequested)
                {
                    if (sp.BytesToRead >= bufferSize)
                    {
                        var bytes = new Byte[bufferSize];
                        sp.Read(bytes, 0, bufferSize);
                        cancelTokenSource.Cancel();
                        return bytes;
                    }
                    Thread.Sleep(500);
                }
                return null;
            }, cancelTokenSource.Token);

            return taskResult;
        }

    }
}