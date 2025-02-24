# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/runtime:9.0.2-noble

LABEL org.opencontainers.image.source=https://github.com/gitfool/BoardGameGeek.Dungeon

# Install packages
RUN <<EOF
    set -ex
    apt-get update
    apt-get install --no-install-recommends -y curl sudo vim
    rm -rf /var/lib/apt/lists/*
EOF

# Modify non-root user
RUN <<EOF
    set -ex
    echo "ubuntu ALL=(ALL:ALL) NOPASSWD: ALL" > /etc/sudoers.d/ubuntu
    chmod 0440 /etc/sudoers.d/ubuntu
EOF

USER ubuntu

RUN <<EOF
    set -ex
    echo "alias l='ls -aF'" >> ~/.bash_aliases
    echo "alias ll='ls -ahlF'" >> ~/.bash_aliases
    echo "alias ls='ls --color=auto --group-directories-first'" >> ~/.bash_aliases
EOF

# Install dungeon
ENV DOTNET_HostBuilder__ReloadConfigOnChange=false

COPY --link --chown=1000:1000 bin/Release/publish /app

RUN <<EOF
    set -ex
    echo "alias d='dotnet /app/bgg-dungeon.dll'" >> ~/.bash_aliases
    find /app -type f -regextype posix-egrep -iregex '.*\.(dll|json|yaml)$' -exec chmod -x {} \;
EOF

ENTRYPOINT [ "dotnet", "/app/bgg-dungeon.dll" ]
