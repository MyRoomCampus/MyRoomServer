#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
RUN dotnet tool install --global dotnet-ef
ARG APP_DBCONN
ARG BuildConfig
ARG ASPNETCORE_ENVIRONMENT
WORKDIR /src
COPY ["MyRoomServer/MyRoomServer.csproj", "MyRoomServer/"]
RUN dotnet restore "MyRoomServer/MyRoomServer.csproj"
COPY . .
WORKDIR "/src/MyRoomServer"
RUN dotnet build "MyRoomServer.csproj" -c ${BuildConfig} -o /app/build && \
	/root/.dotnet/tools/dotnet-ef database update --configuration ${BuildConfig}

FROM build AS publish
ARG BuildConfig
ARG ASPNETCORE_ENVIRONMENT
RUN dotnet publish "MyRoomServer.csproj" -c ${BuildConfig} -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyRoomServer.dll"]