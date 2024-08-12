# without docker on ubuntu server

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

find ./Together.Server -type d -name "bin" -o -name "obj"
find ./Together.Server -type d -name "bin" -exec rm -r {} + &&
  find ./Together.Server -type d -name "obj" -exec rm -r {} +

sudo mkdir -p /usr/local/src/Together.NET.v2

scp .dockerignore Together.NET.v2.sln redis.conf root@14.225.211.153:/usr/local/src/Together.NET.v2 

sudo mkdir -p /usr/local/src/Together.NET.v2/Together.Server &&
  scp -r Together.Server/* root@14.225.211.153:/usr/local/src/Together.NET.v2/Together.Server 

sudo mkdir -p /usr/local/src/Together.NET.v2/Together.Docker &&
  scp -r Together.Docker/docker-compose.prod.yml root@14.225.211.153:/usr/local/src/Together.NET.v2/Together.Docker
  
sudo mkdir -p /usr/local/src/Together.NET.v2/Together.Proxy && 
  scp -r Together.Proxy/nginx.production-docker.conf root@14.225.211.153:/usr/local/src/Together.NET.v2/Together.Proxy 

sudo mkdir -p /usr/local/src/Together.NET.v2/Together.Client && 
  scp -r Together.Client/dist/together.net/* root@14.225.211.153:/usr/local/src/Together.NET.v2/Together.Client/dist/together.net

cd /usr/local/src/Together.NET.v2 && sudo chmod -R 755 . && docker-compose -f ./Together.Docker/docker-compose.prod.yml up -d