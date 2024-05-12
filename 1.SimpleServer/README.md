```
docker build -t npr211-server-image . 
docker run -it --rm -p 5864:80 --name npr211_container npr211-server-image
docker run -d --restart=always --name npr211_container -p 5864:80 npr211-server-image
docker ps -a
docker stop npr211_container
docker rm npr211_container

docker images --all
docker rmi npr211-server-image

docker login
docker tag npr211-server-image:latest novakvova/npr211-server-image:latest
docker push novakvova/npr211-server-image:latest

docker pull novakvova/npr211-server-image:latest
docker ps -a
docker run -d --restart=always --name npr211_container -p 5864:80 novakvova/npr211-server-image


docker pull novakvova/npr211-server-image:latest
docker images --all
docker ps -a
docker stop npr211_container
docker rm npr211_container
docker run -d --restart=always --name npr211_container -p 5864:80 novakvova/npr211-server-image
```

```nginx options /etc/nginx/sites-available/default
server {
    server_name   npr211.itstep.click *.npr211.itstep.click;
    location / {
       proxy_pass         http://localhost:5864;
       proxy_http_version 1.1;
       proxy_set_header   Upgrade $http_upgrade;
       proxy_set_header   Connection keep-alive;
       proxy_set_header   Host $host;
       proxy_cache_bypass $http_upgrade;
       proxy_set_header   X-Forwarded-For $proxy_add_x_forwarded_for;
       proxy_set_header   X-Forwarded-Proto $scheme;
    }
}

sudo systemctl restart nginx
certbot
```