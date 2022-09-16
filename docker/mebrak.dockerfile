FROM darakeon/netcore-libman
LABEL maintainer="Dara Keon <laboon@darakeon.com>"
RUN maintain

COPY site /var/mebrak
RUN cd /var/mebrak/Presentation/ \
	&& libman restore \
	&& dotnet publish Presentation.csproj -o /var/www \
	&& apt-get remove -y dotnet-sdk-6.0 \
	&& maintain \
	&& rm -r /var/mebrak

ENV ASPNETCORE_URLS=http://+:2703;https://+:2709
EXPOSE 2709

COPY stories /var/data

WORKDIR /var/www

CMD ./Presentation
