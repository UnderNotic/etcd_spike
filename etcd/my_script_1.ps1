.\etcd --name infra0 --initial-advertise-peer-urls http://localhost:2380 --listen-peer-urls http://localhost:2380 --listen-client-urls http://localhost:2381 --advertise-client-urls http://localhost:2381 --initial-cluster-token cluster --initial-cluster infra0=http://localhost:2380,infra1=http://localhost:2390 --initial-cluster-state new