FROM mcr.microsoft.com/dotnet/core/runtime:3.1.3-bionic

ARG configuration=Debug

WORKDIR /app
COPY bin/$configuration/publish/ .

ENTRYPOINT [ "dotnet", "BoardGameGeek.Dungeon.dll" ]
