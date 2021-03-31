FROM darakeon/netcore
MAINTAINER Dara Keon <laboon@darakeon.com>
RUN maintain

COPY site /var/mebrak
RUN dotnet publish /var/mebrak/Presentation/Presentation.csproj -o /var/www
RUN apt remove -y dotnet-sdk-5.0
RUN rm -r /var/mebrak

ENV ASPNETCORE_URLS=http://+:2703;https://+:2709
EXPOSE 2709

COPY stories /var/data

WORKDIR /var/www

CMD ./Presentation
