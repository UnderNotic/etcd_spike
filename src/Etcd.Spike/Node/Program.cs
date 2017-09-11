using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;

namespace Node
{
    class Program
    {
        static void Main(string[] args)
        {
            var url = $"http://+:{GetAvailablePort(8080)}";
            var heartBeat = new HeartBeat(url);
            using (WebApp.Start<Startup>(url))
            {
                Console.WriteLine($"Running {ConfigurationManager.AppSettings["NodeType"]} on {url}");
                Console.WriteLine("Press enter to exit");
                Console.ReadLine();
            }
            heartBeat.Dispose();
        }

        public static int GetAvailablePort(int startingPort)
        {
            IPEndPoint[] endPoints;
            List<int> portArray = new List<int>();

            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();

            TcpConnectionInformation[] connections = properties.GetActiveTcpConnections();
            portArray.AddRange(from n in connections
                               where n.LocalEndPoint.Port >= startingPort
                               select n.LocalEndPoint.Port);

            endPoints = properties.GetActiveTcpListeners();
            portArray.AddRange(from n in endPoints
                               where n.Port >= startingPort
                               select n.Port);

            endPoints = properties.GetActiveUdpListeners();
            portArray.AddRange(from n in endPoints
                               where n.Port >= startingPort
                               select n.Port);

            portArray.Sort();

            for (int i = startingPort; i < UInt16.MaxValue; i++)
                if (!portArray.Contains(i))
                    return i;

            return 0;
        }
    }
}

