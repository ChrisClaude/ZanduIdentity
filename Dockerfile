FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["ZanduIdentity/ZanduIdentity.csproj", "ZanduIdentity/"]
RUN dotnet restore "ZanduIdentity/ZanduIdentity.csproj"
COPY . .
WORKDIR "/src/ZanduIdentity"
RUN dotnet build "ZanduIdentity.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ZanduIdentity.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ZanduIdentity.dll"]
