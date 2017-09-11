using Etcd.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Mvccpb.Event.Types;

namespace Gateway
{
    public class ServiceDiscovery : IDisposable
    {
        private EtcdClient etcdClient { get; } = new EtcdClient();
        public IDictionary<string, IList<DiscoverableService>> availableNodes { get; private set; } = new Dictionary<string, IList<DiscoverableService>>();
        private EtcdWatcher watcher;

        public ServiceDiscovery()
        {
            Run();
        }

        private async Task Run()
        {
            availableNodes = RangeServicesToDictionary(await etcdClient.GetRange("heartbeat"));

            watcher = await etcdClient.WatchRange("heartbeat");
            watcher.Subscribe(events =>
                {
                    foreach (var e in events)
                    {
                        var service = DiscoverableService.CreateFromEtcdKey(e.Key);
                        switch (e.Type)
                        {
                            case EventType.Put:
                                IList<DiscoverableService> values;
                                if (availableNodes.TryGetValue(service.Type, out values))
                                {
                                    values.Add(service);
                                }
                                else
                                {
                                    availableNodes.Add(service.Type, new List<DiscoverableService> { service });
                                }
                                break;
                            case EventType.Delete:
                                var valuesForKey = availableNodes[service.Type];
                                var isDeleted = valuesForKey.Remove(service);
                                if (!isDeleted)
                                {
                                    throw new Exception("This can not happen");
                                }
                                if (!valuesForKey.Any())
                                {
                                    availableNodes.Remove(e.Key);
                                }
                                break;
                        }
                    }
                    Console.WriteLine($"Available nodes {availableNodes.Aggregate(string.Empty, (acc, item) => $"{item.Key} - {item.Value.Aggregate(string.Empty, (a, i) => $"{i.Address} {a}")} {acc}")}");

                });
        }

        private Dictionary<string, IList<DiscoverableService>> RangeServicesToDictionary(IDictionary<string, string> resp)
        {
            return resp.Select(kvp => DiscoverableService.CreateFromEtcdKey(kvp.Key)).Aggregate(new Dictionary<string, IList<DiscoverableService>>(), (acc, item) =>
            {
                IList<DiscoverableService> list;
                if (acc.TryGetValue(item.Type, out list))
                {
                    list.Add(item);
                }
                else
                {
                    acc.Add(item.Type, new List<DiscoverableService> { item });
                }
                return acc;
            });
        }

        public void Dispose()
        {
            watcher.Dispose();
            etcdClient.Dispose();
        }
    }
}
