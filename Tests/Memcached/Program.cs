#if DEBUG
using System;
using System.Threading;
using ReusableLibrary.Memcached.Tests.Infrastructure;

namespace ReusableLibrary.Memcached.Tests
{
    public static class Program
    {
        internal static void Main()
        {
            DecrementTest();
            ////IncrementTest();
            ////GetProfiling();            
        }

        private static void DecrementTest()
        {
            using (var context = new CacheClientContext())
            {
                var test = new CacheClientTest.Text();
                test.SetFixture(context);
                var key = DomainModelFactory.RandomKey();
                for (int i = 0; i < 100; i++)
                {
                    Console.WriteLine("{0}. Counter = {1}", i, 
                        test.Cache.Increment(key, -1L, 10L, TimeSpan.FromSeconds(15)));
                    Thread.Sleep(1000);
                }
            }
        }

        private static void IncrementTest()
        {
            using (var context = new CacheClientContext())
            {
                var test = new CacheClientTest.Binary();
                test.SetFixture(context);
                test.Increment(DomainModelFactory.IncrementKeys[0]);
                test.Increment_NoInitial();
            }
        }

        private static void GetProfiling()
        {
            for (int i = 0; i < 100; i++)
            {
                var test = new CacheClientProfilingTest.Text();
                test.GetString(8);
            }
        }
    }
}

#endif