build:
	@docker build .. -t darakeon/mebrak -f mebrak.dockerfile

push: build
	@docker push darakeon/mebrak

run: build
	@docker run --name mebrak --rm darakeon/mebrak

enter: build
	@docker run --name mebrak -it --rm darakeon/mebrak bash
