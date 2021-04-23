# LC_MicroService_test
Asp.Net Core Mvc+CoreWebApi+Consul+Ocelot+IdentityServer4

查看consul集群状态(最少要三个节点才能组件集群)：
docker exec -t node1 consul members   ##查看节点
docker exec -t node1 consul operator raft list-peers   ##查看主从信息

--1、部署consul集群--》通过nginx实现consul集群
a)创建consul节点
consul node1：
docker run -d --name=node1 --restart=always -e 'CONSUL_LOCAL_CONFIG={"skip_leave_on_interrupt":true}' -p 8300:8300 -p 8301:8301 -p 8301:8301/udp -p 8302:8302/udp -p 8302:8302 -p 8400:8400 -p 8500:8500 -p 8600:8600 -h node1 consul agent -server -bind=0.0.0.0 -bootstrap-expect=3 -node=node1 -data-dir=/tmp/data-dir -client 0.0.0.0 -ui

consul node2：
docker run -d --name=node2 --restart=always -e 'CONSUL_LOCAL_CONFIG={"skip_leave_on_interrupt":true}' -p 9300:8300 -p 9301:8301 -p 9301:8301/udp -p 9302:8302/udp -p 9302:8302 -p 9400:8400 -p 9500:8500 -p 9600:8600 -h node2 consul agent -server -bind=0.0.0.0 -join=47.92.132.209 -node-id=$(uuidgen | awk '{print tolower($0)}') -node=node2 -data-dir=/tmp/data-dir -client 0.0.0.0 -ui

consul node3：
docker run -d --name=node3 --restart=always -e 'CONSUL_LOCAL_CONFIG={"skip_leave_on_interrupt":true}' -p 10300:8300 -p 10301:8301 -p 10301:8301/udp -p 10302:8302/udp -p 10302:8302 -p 10400:8400 -p 10500:8500 -p 10600:8600 -h node3 consul agent -server -bind=0.0.0.0 -join=47.92.132.209 -node-id=$(uuidgen | awk '{print tolower($0)}') -node=node3 -data-dir=/tmp/data-dir -client 0.0.0.0 -ui

consul node4--》client：
docker run -d --name=node4 --restart=always -e 'CONSUL_LOCAL_CONFIG={"leave_on_terminate":true}' -p 11300:8300 -p 11301:8301 -p 11301:8301/udp -p 11302:8302/udp -p 11302:8302 -p 11400:8400 -p 11500:8500 -p 11600:8600 -h node4 consul agent -bind=0.0.0.0 -retry-join=47.92.132.209 -node-id=$(uuidgen | awk '{print tolower($0)}') -node=node4 -client 0.0.0.0 -ui

b)nginx配置consul集群
docker run -d -p 8089:80 -v /home/0421/config/consulnginx/:/var/log/nginx/ -v /home/0421/config/consulnginx/nginx.conf:/etc/nginx/nginx.conf --name consulnginx nginx

--2、部署webapi注册到consul，通过docker-compose.yml批量构建部署
a)cd到docker-compose.yml存放路径，然后执行命令：docker-compose up

--3、部署ocelot--》通过nginx实现ocelot集群
a)cd到docker-compose.yml存放路径，然后执行命令：docker-compose up
b)nginx配置ocelot集群---------已经在docker-compose.yml里面配置了，所以这里就不需要了。

--4、部署IdentityServer4鉴权中心
a)cd到Dockerfile所在路径，然后执行命令：
docker build -t ids4.0422 -f Dockerfile .    ##创建镜像
docker run -itd -p 7200:80 ids4.0422        ##启动容器实例



![image](https://user-images.githubusercontent.com/26539681/115856742-719e9a00-a45f-11eb-8a9d-516de89b5ae0.png)
![image](https://user-images.githubusercontent.com/26539681/115855555-f2f52d00-a45d-11eb-9afc-d5f47c5b1ef4.png)
![image](https://user-images.githubusercontent.com/26539681/115858295-559bf800-a461-11eb-9b49-a70e63c95da0.png)
![image](https://user-images.githubusercontent.com/26539681/115856165-bbd34b80-a45e-11eb-8d99-007a9ac0d7df.png)
![image](https://user-images.githubusercontent.com/26539681/115857359-2cc73300-a460-11eb-91f5-beebd372a342.png)
![image](https://user-images.githubusercontent.com/26539681/115857447-49fc0180-a460-11eb-94f1-e4f5d83faee7.png)
![image](https://user-images.githubusercontent.com/26539681/115857469-51230f80-a460-11eb-8493-26d99528d948.png)
![image](https://user-images.githubusercontent.com/26539681/115858056-11a8f300-a461-11eb-9808-f9bf491d2384.png)
