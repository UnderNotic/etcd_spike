using Etcd.Client;
using System;
using System.Configuration;
using System.Threading;

namespace Node
{
    public class HeartBeat : IDisposable
    {
        private string NODE_TYPE { get; } = ConfigurationManager.AppSettings["NodeType"];
        private IDisposable timer;
        private EtcdClient etcdClient { get; } = new EtcdClient();
        private TimeSpan interval { get; } = TimeSpan.FromSeconds(5);
        private TimeSpan TTL { get; } = TimeSpan.FromSeconds(10);

        public HeartBeat(string url)
        {
            Run(url);
        }

        private async void Run(string url)
        {

            var leaseId = await etcdClient.PutWithLease($"heartbeat|{NODE_TYPE}|{url}", url, (int)TTL.TotalSeconds);
            this.timer = new Timer(_ => etcdClient.KeepAliveLease(leaseId), null, TimeSpan.Zero, interval);
        }

        public void Dispose()
        {
            timer.Dispose();
            etcdClient.Dispose();
        }
    }
}
