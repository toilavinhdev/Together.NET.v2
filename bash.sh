# without docker on ubuntu

dotnet publish -c Release -o Publish

npm run build:prod

sudo rm -r /var/www/Together.NET.v2/publish

sudo rm -r /var/www/Together.NET.v2/html

scp -r Together.Server/Together.API/Publish/* root@14.225.211.153:/var/www/Together.NET.v2/publish

scp -r Together.Client/dist/together.net/* root@14.225.211.153:/var/www/Together.NET.v2/html

sudo chmod -R 755 /var/www/Together.NET.v2

sudo service kestrel-togethernet reload

sudo service nginx reload 

sudo journalctl -fu kestrel-togethernet.service

# using docker

npm run build:prod

sudo mkdir -p /usr/local/src/Together.NET.v2

scp .dockerignore Together.NET.v2.sln redis.conf root@14.225.211.153:/usr/local/src/Together.NET.v2 

scp -r Together.Server/* root@14.225.211.153:/usr/local/src/Together.NET.v2/Together.Server 

scp -r  Together.Docker/docker-compose.prod.yml root@14.225.211.153:/usr/local/src/Together.NET.v2/Together.Docker 

scp -r Together.Proxy/nginx.production-docker.conf root@14.225.211.153:/usr/local/src/Together.NET.v2/Together.Proxy 

scp -r Together.Client/dist/together.net* root@14.225.211.153:/usr/local/src/Together.NET.v2/Together.Client

cd /usr/local/src/Together.NET.v2 && docker-compose -f ./Together.Docker/docker-compose.prod.yml up -d

cd /usr/local/src/ && git pull https://github.com/toilavinhdev/Together.NET.v2