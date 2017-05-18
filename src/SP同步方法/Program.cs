using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SP同步方法
{
    public class Program
    {
        public static SerialPort sp = new SerialPort();
        /// <summary>
        /// 2秒超时时间
        /// </summary>
        public static int timeout = 2;
        /// <summary>
        /// 是否超时
        /// </summary>
        public static bool isOverTime;
        /// <summary>
        /// 计时器
        /// </summary>
        public static System.Timers.Timer timer = new System.Timers.Timer();

        public static void Main(string[] args)
        {
            sp.PortName = "COM3";
            sp.Open();

            timer.AutoReset = false;
            timer.Interval = 1000 * timeout;
            timer.Enabled = true;
            timer.Elapsed += (o, e) =>
            {
                isOverTime = true;
            };

            // 同步方式读取2个字节
            var a = SyncReadBytes(2);
            if (a == null)
            {
                Console.WriteLine("a未接收到数据");
            }
            else
            {
                foreach (var v in a)
                {
                    Console.Write($"0x{v,-2}");
                }
            }

            Console.WriteLine();

            // 同步方式读取5个字节
            var b = SyncReadBytes(5);
            if (b == null)
            {
                Console.WriteLine("b未接收到数据");
            }
            else
            {
                foreach (var v in b)
                {
                    Console.Write($"0x{v,-2}");
                }
            }
            Console.WriteLine();


            Console.WriteLine("over");
            Console.Read();

        }


        /// <summary>
        /// 同步方式 读取字节
        /// </summary>
        /// <param name="v"></param>
        private static Byte[] SyncReadBytes(int bufferSize) 
        {
            isOverTime = false;
            // 开始计时
            timer.Start();

            while (!isOverTime)
            {
                if (sp.BytesToRead >= bufferSize)
                {
                    var bytes = new Byte[bufferSize];
                    sp.Read(bytes, 0, bufferSize);
                    // 停止计时
                    timer.Stop();
                    return bytes;
                }
                Thread.Sleep(500);
            }

            return null;
        }



    }
}
