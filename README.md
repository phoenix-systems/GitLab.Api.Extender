# GitLab.Api.Extender
The main purpose of that api is to simplify CI/CD builds that have to be access gitlab to add tags, etc... Current gitlab api requires to pass project id to most of the endpoinds and that forces to store it in builds parameters. Usually builds already have http link to gitlab repo as parameter and this api allows to access gitlab using repo link only.

## Pre-built Docker Image
You can quickly run a container with a pre-built [Docker image](https://hub.docker.com/r/phoenixsystemsag/gitlab-api-extender) using [docker-compose.yaml](https://github.com/phoenix-systems/GitLab.Api.Extender/blob/master/docker-compose.yaml)

Update the following params before start:
```
      - "App__Access__Key=<api-access-key>"
      - "App__GitLab__Url=<gitlab-api-url>"
      - "App__GitLab__Token=<gitlab-token>"
```
