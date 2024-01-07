MAKEFLAGS += --no-print-directory

build:
	@docker build .. -t darakeon/mebrak -f mebrak.dockerfile --progress=plain

push: build
	@docker push darakeon/mebrak

run: build
	@docker run --name mebrak -p 2703:2703 -d --rm darakeon/mebrak
	@sleep 3
	@docker logs mebrak

enter:
	@docker exec -it mebrak bash

stop:
	@docker stop mebrak

logs:
	@docker logs mebrak
