.\etcd --name infra1 --initial-advertise-peer-urls http://localhost:2390 --listen-peer-urls http://localhost:2390 --listen-client-urls http://localhost:2391 --advertise-client-urls http://localhost:2391 --initial-cluster-token cluster --initial-cluster infra0=http://localhost:2380,infra1=http://localhost:2390 --initial-cluster-state new