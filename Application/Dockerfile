FROM mcr.microsoft.com/dotnet/runtime:5.0.0-rc.1-focal

ARG configuration=Release

WORKDIR /app
COPY bin/$configuration/ .

ENTRYPOINT [ "dotnet", "BoardGameGeek.Dungeon.dll" ]
