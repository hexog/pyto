FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 9000

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Pyto/Pyto.csproj", "Pyto/"]
RUN dotnet restore "Pyto/Pyto.csproj"
COPY . .
WORKDIR /src/Pyto
RUN dotnet build "Pyto.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Pyto.csproj" -c Release -o /app/publish

FROM node:14-alpine AS client-build
WORKDIR /src/Pyto.Client
COPY Pyto.Client .
RUN yarn
RUN yarn build --outDir /app/publish/wwwroot

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=client-build /app/publish/wwwroot wwwroot
ENTRYPOINT ["dotnet", "Pyto.dll"]
