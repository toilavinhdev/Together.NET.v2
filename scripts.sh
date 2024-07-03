dotnet publish -c Release -o Publish

npm run build --configuration=prod

sudo rm -r /var/www/Together.NET.v2/Server
scp -r ~/Workspace/Together.NET.v2/Together.Server/Together.API/Publish/* root@14.225.211.153:/var/www/Together.NET.v2/Server

sudo rm -r /var/www/Together.NET.v2/Client
scp -r ~/Workspace/Together.NET.v2/Together.Client/dist/together/* root@14.225.211.153:/var/www/Together.NET.v2/Client

sudo service kestrel-together-api reload

sudo chmod -R 755 /var/www/Together.NET.v2

sudo service nginx reload 

sudo journalctl -fu kestrel-together-api.service