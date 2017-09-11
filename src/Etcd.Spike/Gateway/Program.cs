using Microsoft.Owin.Hosting;
using System;

namespace Gateway
{
    class Program
    {
        static void Main(string[] args)
        {
            Run();
        }

        static async void Run()
        {
            var url = $"http://+:3000";
            using (WebApp.Start<Startup>(url))
            {
                Console.WriteLine("Running on {0}", url);
                Console.WriteLine("Press enter to exit");
                Console.ReadLine();
            }
        }
    }
}
