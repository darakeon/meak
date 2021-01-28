@echo off

set dir=%~dp0
set push=%~1

docker context use default > NUL

docker stop mebrak > NUL 2> NUL
docker rm mebrak > NUL 2> NUL

docker build .. -t darakeon/mebrak -f "%dir%mebrak.dockerfile"

if "%push%" neq "" (
	docker %push% darakeon/mebrak
)
