FROM darakeon/nginx-netcore
MAINTAINER Dara Keon

RUN /var/cert/cert-https.sh Stories meak-stories.com meak@darakeon.com

COPY Site /var/mebrak

RUN dotnet publish /var/mebrak/Presentation/Presentation.csproj -o /var/www
RUN apt remove -y dotnet-sdk-5.0
RUN rm -r /var/mebrak

RUN curl -L https://github.com/darakeon/meak/archive/main.zip > main.zip

RUN apt install -y unzip
RUN mkdir /var/data/
RUN unzip main.zip -d /var/data/
RUN mv /var/data/meak-main/_* /var/data
RUN rm -r /var/data/meak-main/
RUN rm main.zip
RUN apt remove -y unzip

RUN service nginx restart

WORKDIR /var/www

CMD service nginx start && ./Presentation
