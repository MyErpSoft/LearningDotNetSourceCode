using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartitionerDemo {
    class Program {
        const int count = 10000000;

        static void Main(string[] args) {
            Demo1();
            Demo2();
            Demo3();
            Demo4();
            Demo5();

            Console.ReadLine();
        }

        static void Demo1() {
            //比如创建1000万个随机的字符串任务，最笨的单线程做法是：
            
            Test("单线程创建随机字符串", () => {
                string[] items = new string[count];
                Random random = new Random();
                for (int i = 0; i < count; i++) {
                    items[i] = CreateRandomString(random);
                }
            });            
        }

        static void Demo2() {
            //多个分区：
            Test("多个分区创建随机字符串", () => {
                string[] items = new string[count];
                var pars = Partitioner.Create(0,count);

                List<Task> tasks = new List<Task>();
                foreach (var item in pars.GetDynamicPartitions()) {
                    tasks.Add(Task.Run(() => {
                        Random random = new Random();
                        //Console.Write("ThreadId:" + System.Threading.Thread.CurrentThread.ManagedThreadId.ToString());
                        for (int i = item.Item1; i < item.Item2; i++) {
                            items[i] = CreateRandomString(random);
                        }
                    }));
                }
                Task.WaitAll(tasks.ToArray());
            });
        }

        static void Demo3() {
            Random random = new Random();
            var stringItems = from p in Enumerable.Range(0, count).AsParallel() select CreateRandomString(random);
            Test("Linq语法", () => {
                string[] items = stringItems.ToArray();
            });
        }

        static void Demo4() {
            Random random = new Random();
            string[] items = new string[count];
            Test("Parallel.For语法", () => Parallel.For(0, count, (i) => items[i] = CreateRandomString(random)));
        }

        static void Demo5() {
            //缓冲区不支持并发，但此案例中并行的作业中没有资源的争用：
            Test("多个分区优化算法", () => {
                string[] items = new string[count];
                var pars = Partitioner.Create(0, count);

                List<Task> tasks = new List<Task>();
                foreach (var item in pars.GetDynamicPartitions()) {
                    tasks.Add(Task.Run(() => {
                        RandomStringBuilder random = new Program.RandomStringBuilder();
                        //Console.Write("ThreadId:" + System.Threading.Thread.CurrentThread.ManagedThreadId.ToString());
                        for (int i = item.Item1; i < item.Item2; i++) {
                            items[i] = random.CreateRandomString();
                        }
                    }));
                }
                Task.WaitAll(tasks.ToArray());
            });
        }

        private static int avalue = (int)'a';
        private static string CreateRandomString(Random random) {
            var len = random.Next(1, 15);
            char[] chars = new char[len];
            for (int i = 0; i < len; i++) {
                chars[i] = (char)(avalue + random.Next(0, 26));
            }
            return new string(chars);
        }

        static void Test(string caption ,Action action) {
            Console.Write("正在执行 " + caption + "...");
            Stopwatch watch = new Stopwatch();
            watch.Start();
            action();
            watch.Stop();
            Console.WriteLine("耗时：" + watch.Elapsed.ToString());
        }

        private sealed class RandomStringBuilder {
            private Random _random = new Random();
            private char[] _buffer = new char[15];
            public string CreateRandomString() {
                var len = _random.Next(1, 15);
                for (int i = 0; i < len; i++) {
                    _buffer[i] = (char)(avalue + _random.Next(0, 26));
                }
                return new string(_buffer, 0, len);
            }
        }
    }
}
