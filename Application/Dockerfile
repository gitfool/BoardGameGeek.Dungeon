FROM mcr.microsoft.com/dotnet/core/runtime:2.2.6-bionic

ARG configuration=Debug

COPY bin/$configuration/publish/ /app/

ENTRYPOINT [ "dotnet", "/app/BoardGameGeek.Dungeon.dll" ]
