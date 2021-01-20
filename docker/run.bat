@echo off

cd ..
copy docker\mebrak.dockerfile mebrak.dockerfile > NUL
docker build . -f mebrak.dockerfile -t darakeon/mebrak
del mebrak.dockerfile > NUL
cd docker

docker rm -f mebrak > NUL 2> NUL
docker run --name mebrak -p 80:80 -p 443:443 -it darakeon/mebrak
docker push darakeon/mebrak
