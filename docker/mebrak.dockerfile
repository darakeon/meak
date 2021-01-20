FROM darakeon/netcore
COPY Site /var/mebrak

RUN dotnet publish /var/mebrak/Presentation/Presentation.csproj -o /var/www
RUN apt remove -y dotnet-sdk-5.0
RUN rm -r /var/mebrak

RUN curl -L https://github.com/darakeon/meak/archive/main.zip > /var/www/main.zip
RUN apt remove -y curl

RUN apt install -y unzip
RUN mkdir /var/data/
RUN unzip /var/www/main.zip -d /var/data/
RUN mv /var/data/meak-main/_* /var/data
RUN rm -r /var/data/meak-main/
RUN apt remove -y unzip

ENV ASPNETCORE_URLS=http://+:80;https://+:443
EXPOSE 80
EXPOSE 443
WORKDIR /var/www
CMD ./Presentation
