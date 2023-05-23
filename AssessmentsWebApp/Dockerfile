#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.


FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["AssessmentsWebApp/AssessmentsWebApp.csproj", "AssessmentsWebApp/"]
RUN dotnet restore "AssessmentsWebApp/AssessmentsWebApp.csproj"
COPY . .
WORKDIR "/src/AssessmentsWebApp"
RUN dotnet build "AssessmentsWebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AssessmentsWebApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AssessmentsWebApp.dll"]