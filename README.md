# GitLab.Api.Extender
The main purpose of that api is to simplify CI/CD builds that have to be access gitlab to add tags, etc... Current gitlab api requires to pass project id to most of the endpoinds and that forces to store it in builds parameters. Usually builds already have http link to gitlab repo as parameter and this api allows to access gitlab using repo link only.
