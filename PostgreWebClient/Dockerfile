FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["PostgreWebClient/PostgreWebClient.csproj", "PostgreWebClient/"]
RUN dotnet restore "PostgreWebClient/PostgreWebClient.csproj"
COPY . .
WORKDIR "/src/PostgreWebClient"
RUN dotnet build "PostgreWebClient.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PostgreWebClient.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PostgreWebClient.dll"]
