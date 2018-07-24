FROM microsoft/dotnet:2.1-sdk
COPY . /root/
WORKDIR /root/
CMD ASPNETCORE_URLS=http://*:$PORT dotnet MVCDemo.dll