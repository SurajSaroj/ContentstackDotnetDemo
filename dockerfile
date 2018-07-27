FROM microsoft/dotnet:2.1-sdk
WORKDIR /root/

COPY *.csproj ./
COPY . ./

RUN dotnet restore
RUN dotnet publish -c Release -o out

COPY /out/ /root/

CMD ASPNETCORE_URLS=http://*:$PORT dotnet MVCDemo.dll
