FROM darakeon/nginx-netcore
MAINTAINER Dara Keon

RUN /var/cert/cert-https.sh Stories meak-stories.com meak@darakeon.com

COPY site /var/mebrak

RUN dotnet publish /var/mebrak/Presentation/Presentation.csproj -o /var/www
RUN apt remove -y dotnet-sdk-5.0
RUN rm -r /var/mebrak

COPY stories /var/data

RUN service nginx restart

WORKDIR /var/www

CMD service nginx start && ./Presentation
