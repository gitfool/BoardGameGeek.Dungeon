FROM mcr.microsoft.com/dotnet/core/runtime:3.1.5-focal

ARG configuration=Release

WORKDIR /app
COPY bin/$configuration/ .

ENTRYPOINT [ "dotnet", "BoardGameGeek.Dungeon.dll" ]
