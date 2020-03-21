FROM mcr.microsoft.com/dotnet/core/runtime:3.1.2-bionic

ARG configuration=Debug

COPY bin/$configuration/publish/ /app/

ENTRYPOINT [ "dotnet", "/app/BoardGameGeek.Dungeon.dll" ]
