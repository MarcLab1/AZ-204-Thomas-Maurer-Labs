using System;
using StackExchange.Redis;

namespace RedisCashe
{
    class Program
    {   private static string CacheConnection = "redisthomas000.redis.cache.windows.net:6380,password=23432=,ssl=True,abortConnect=False";
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            IDatabase cache = lazyConnection.Value.GetDatabase();

            Console.WriteLine("Reading cache : " + cache.StringGet("Session333").ToString());
            Console.WriteLine("Writing cache : " + cache.StringSet("Session333", "Something to Redis" + DateTime.Now()));
            Console.WriteLine("Reading cache : " + cache.StringGet("Session333").ToString());

            cache.KeyExpire("Session333", DateTime.Now.AddMinutes(1));

            lazyConnection.Value.Dispose();

            Console.WriteLine("Press Any Key");
            Console.ReadLine();
        }

        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            return ConnectionMultiplexer.Connect(CacheConnection);
        });

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }
    }
}