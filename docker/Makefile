MAKEFLAGS += --no-print-directory

build:
	@docker build .. -t darakeon/mebrak -f mebrak.dockerfile

push: build
	@docker push darakeon/mebrak

run: build
	@docker run --name mebrak -p 2703:2703 -d --rm darakeon/mebrak

enter: build
	@docker run --name mebrak -it --rm darakeon/mebrak bash
