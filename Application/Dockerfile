FROM mcr.microsoft.com/dotnet/runtime-deps:5.0.4-focal

ARG configuration=Release

WORKDIR /app
COPY bin/$configuration/linux-x64/publish/ .

ENTRYPOINT [ "./BoardGameGeek.Dungeon" ]
