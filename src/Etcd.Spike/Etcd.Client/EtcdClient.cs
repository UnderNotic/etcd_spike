using Etcdserverpb;
using Google.Protobuf;
using Grpc.Core;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Etcd.Client
{
    public class EtcdClient
    {
        private DnsEndPoint[] etcdServerEndpoints = new[] { new DnsEndPoint("localhost", 2381), new DnsEndPoint("localhost", 2391) };
        private KV.KVClient kvClient;

        public EtcdClient()
        {
            Channel channel = new Channel(etcdServerEndpoints[0].Host, etcdServerEndpoints[0].Port, ChannelCredentials.Insecure);
            this.kvClient = new KV.KVClient(channel);
        }

        public async Task<PutResponse> Put(string key, string value)
        {
            return await kvClient.PutAsync(new PutRequest() { Key = ByteString.CopyFromUtf8(key), Value = ByteString.CopyFromUtf8(value) });
        }

        public async Task<string> Get(string key)
        {
            var resp = await kvClient.RangeAsync(new RangeRequest() { Key = ByteString.CopyFromUtf8(key) });
            return resp.Kvs[0].Value.ToStringUtf8();
        }
    }
}
