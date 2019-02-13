FROM microsoft/dotnet:2.2.2-runtime-bionic

ARG configuration=Debug

COPY bin/$configuration/publish/ /app/

ENTRYPOINT [ "dotnet", "/app/BoardGameGeek.Dungeon.dll" ]
