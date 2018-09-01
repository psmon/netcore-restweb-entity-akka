FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["accountapi/accountapi.csproj", "accountapi/"]
RUN dotnet restore "accountapi/accountapi.csproj"
COPY . .
WORKDIR "/src/accountapi"
RUN dotnet build "accountapi.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "accountapi.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "accountapi.dll"]