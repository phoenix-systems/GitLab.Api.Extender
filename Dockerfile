FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.sln .
COPY GitLab.Api.Extender/*.csproj ./GitLab.Api.Extender/
RUN dotnet restore

# copy everything else and build app
COPY GitLab.Api.Extender/. ./GitLab.Api.Extender/
WORKDIR /app/GitLab.Api.Extender
RUN dotnet publish -c Release -o out


FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS runtime
WORKDIR /app
COPY --from=build /app/GitLab.Api.Extender/out ./
ENTRYPOINT ["dotnet", "GitLab.Api.Extender.dll"]