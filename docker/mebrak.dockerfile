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

COPY --from=builder /root/.dotnet/corefx/cryptography/x509stores/my/*.pfx  /var/https/certificate.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/var/https/certificate.pfx

ENV ASPNETCORE_URLS=https://+:2703
EXPOSE 2703

COPY stories /var/data

WORKDIR /var/www

CMD ./Presentation
