FROM ghcr.io/mazharenko/dotnet-interactive-docker:main

RUN dotnet nuget add source https://api.nuget.org/v3/index.json -n nuget.org

COPY --chown=1000 ./docs ${HOME}/docs/

WORKDIR ${HOME}/docs/