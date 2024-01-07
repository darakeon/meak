FROM darakeon/netcore-libman as builder
LABEL maintainer="Dara Keon <laboon@darakeon.com>"
RUN maintain

COPY site /var/mebrak
RUN cd /var/mebrak/Presentation/ \
	&& dotnet publish Presentation.csproj -o /var/www \
	&& rm -r /var/mebrak \
	&& cd /var/www \
	&& libman restore


FROM darakeon/netcore-server

COPY --from=builder /var/www /var/www

ENV ASPNETCORE_URLS=http://+:2703
EXPOSE 2703

COPY stories /var/data

WORKDIR /var/www

CMD ./Presentation
