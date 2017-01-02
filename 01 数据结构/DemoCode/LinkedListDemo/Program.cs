using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedListDemo {
      class Program {
        static void Main(string[] args) {
            ListTest();
            LinkedListTest();
            //LinkedListTest2();


            //ListTest();
            //LinkedListTest();
            //LinkedListTest2();
            Console.ReadLine();
        }

        private static void ListTest() {
            Console.WriteLine("===== List 测试 =====");
            List<int> list = new List<int>();
            for (int i = 0; i < 100; i++) {
                list.Add(i);
            }

            Test("数据较少时", () => {
                for (int i = 0; i < 1000; i++) {
                    list.Insert(0, i);
                }
            });

            list.Clear();
            for (int i = 0; i < 100000; i++) {
                list.Add(i);
            }
            Test("数据较多时", () => {
                for (int i = 0; i < 1000; i++) {
                    list.Insert(0, i);
                }
            });

            Test("添加到末尾", () => {
                for (int i = 0; i < 1000; i++) {
                    list.Add(i);
                }
            });

            Test("检索数据  ", () => {
                for (int i = 0; i < 1000; i++) {
                    list.Contains(32323209);
                }
            });

            Test("枚举数据  ", () => {
                for (int i = 0; i < 1000; i++) {
                    foreach (var item in list) {
                        var pp = item;
                    }
                }
            });
        }

        private static void LinkedListTest() {
            Console.WriteLine("===== LinkedList 测试 =====");
            LinkedList<int> list = new LinkedList<int>();
            for (int i = 0; i < 100; i++) {
                list.AddLast(i);
            }

            Test("数据较少时", () => {
                for (int i = 0; i < 1000; i++) {
                    list.AddFirst(i);
                }
            });

            list.Clear();
            for (int i = 0; i < 100000; i++) {
                list.AddLast(i);
            }
            Test("数据较多时", () => {
                for (int i = 0; i < 1000; i++) {
                    list.AddFirst(i);
                }
            });

            Test("添加到末尾", () => {
                for (int i = 0; i < 1000; i++) {
                    list.AddLast(i);
                }
            });

            Test("检索数据  ", () => {
                for (int i = 0; i < 1000; i++) {
                    list.Contains(32323209);
                }
            });

            Test("枚举数据  ", () => {
                for (int i = 0; i < 1000; i++) {
                    foreach (var item in list) {
                        var pp = item;
                    }
                }
            });
        }


        private static void LinkedListTest2() {
            Console.WriteLine("===== LinkedList2 测试 =====");
            LinkedList<int> list = new LinkedList<int>();
            Test("直接添加", () => {
                for (int i = 0; i < 1000; i++) {
                    list.AddFirst(i);
                }
            });
        }

        static void Test(string name, Action action) {
            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            action();
            watch.Stop();
            Console.WriteLine(name + " 耗时：" + watch.Elapsed);
        }
    }
}
