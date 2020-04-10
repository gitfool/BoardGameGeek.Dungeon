FROM mcr.microsoft.com/dotnet/core/runtime:3.1.3-bionic

ARG configuration=Release

WORKDIR /app
COPY bin/$configuration/ .

ENTRYPOINT [ "dotnet", "BoardGameGeek.Dungeon.dll" ]
