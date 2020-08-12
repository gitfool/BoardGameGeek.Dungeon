FROM mcr.microsoft.com/dotnet/core/runtime:3.1.7-focal

ARG configuration=Release

WORKDIR /app
COPY bin/$configuration/ .

ENTRYPOINT [ "dotnet", "BoardGameGeek.Dungeon.dll" ]
