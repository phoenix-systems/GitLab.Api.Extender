# GitLab.Api.Extender
The main purpose of that api is to simplify CI/CD builds that have to be access gitlab to add tags, etc... Current gitlab api requires to pass project id to most of the endpoinds and that forces to store it in builds parameters. Usually builds already have http link to gitlab repo (like http://git.example.com/app/app-client.git) as parameter and this api allows to access gitlab using repo link only.

## Features
- Get Poject Id by http repository url
- Add Tag by http repository url

## How to use
All endpoints are documented in swagger. It must be available here: http://domain-or-ip:80/swagger/

## Run using pre-built docker image
You can quickly run a container with a pre-built [docker image](https://hub.docker.com/r/phoenixsystemsag/gitlab-api-extender) using [docker-compose.yaml](https://github.com/phoenix-systems/GitLab.Api.Extender/blob/master/docker-compose.yaml).

1. Update the following environment params in docker-compose.yaml
```
- App__Access__Key:  Key to secure api endpoints
- App__GitLab__Url: Url to gitlab api, like: https://example.com/api
- App__GitLab__Token: Token that has access to gitlab projects
```

2. Start docker-compose
```
docker-compose up -d
```

3. Open swagger: http://domain-or-ip:80/swagger/

## Build docker image
Pull code from repository and run the following command:
```
docker build --pull -t gitlab-api-extender .
```
