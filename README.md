# .NET Core API for ORM(Entity)

[Document](http://wiki.webnori.com/pages/viewpage.action?pageId=12583404)


## Mysql UP
	//로컬에 DB를 세팅해서 사용할시 이용합니다.
	
    //최초에 한번(네트워크셋팅)
    docker network create --driver=bridge --subnet=172.19.0.0/16 devnet
 
    docker network inspect devnet
    
    //Mysql 실행
    TestInfra>docker-compose up -d


## Local Cluster

- node1: dotnet run --configuration Release --project accountapi --akkaip 127.0.0.1 --akkaport 5100  --akkaseed 127.0.0.1:5100 --port 5000
- node2: dotnet run --configuration Release --project accountapi --akkaip 127.0.0.1 --akkaport 5101  --akkaseed 127.0.0.1:5100 --port 5001
- node3: dotnet run --configuration Release --project accountapi --akkaip 127.0.0.1 --akkaport 5102  --akkaseed 127.0.0.1:5100 --port 5002

