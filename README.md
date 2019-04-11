# GitLab.Api.Extender
The main purpose of that api is to simplify CI/CD builds that have to be access gitlab to add tags, etc... Current gitlab api requires to pass project id to most of the endpoinds and that forces to store it in builds parameters. Usually builds already have http link to gitlab repo as parameter and this api allows to access gitlab using repo link only.

## Run using pre-built docker image
You can quickly run a container with a pre-built [Docker image](https://hub.docker.com/r/phoenixsystemsag/gitlab-api-extender) using [docker-compose.yaml](https://github.com/phoenix-systems/GitLab.Api.Extender/blob/master/docker-compose.yaml). Update the following params before start:
```
- App__Access__Key:  Key to secure api endpoints
- App__GitLab__Url: Url to gitlab api, like: https://example.com/api
- App__GitLab__Token: Token that have access to gitlab projects
```

## Build docker image
Pull code from repository and run the following command:
```
docker build --pull -t gitlab-api-extender .
```
