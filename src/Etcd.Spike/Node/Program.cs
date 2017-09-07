using Etcd.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Node
{
    class Program
    {
        static void Main(string[] args)
        {
            Run();
        }

        static async void Run()
        {
            var client = new EtcdClient();
            try
            {
                var result = await client.Put("message", "MyValue");
                var resp = await client.Get("message");
                Console.WriteLine(resp);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw ex;
            }
        }
    }
}
