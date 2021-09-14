FROM mcr.microsoft.com/dotnet/runtime-deps:5.0.10-focal

# Add non-root user
RUN groupadd --gid 1000 user \
    && useradd --uid 1000 --gid 1000 --shell /bin/bash -m user

USER user

# Install app
WORKDIR /home/user

COPY --chown=user:user bin/Release/linux-x64/publish .

ENTRYPOINT [ "./bgg" ]
