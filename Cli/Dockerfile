# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/runtime:7.0.12-jammy

LABEL org.opencontainers.image.source=https://github.com/gitfool/BoardGameGeek.Dungeon

# Install packages
RUN <<EOF
    set -ex
    apt-get update
    apt-get install --no-install-recommends -y curl sudo vim
    rm -rf /var/lib/apt/lists/*
EOF

# Add non-root user
RUN <<EOF
    set -ex
    groupadd --gid 1000 user
    useradd --uid 1000 --gid 1000 --shell /bin/bash -m user
    echo "user ALL=(ALL:ALL) NOPASSWD: ALL" > /etc/sudoers.d/user
    chmod 0440 /etc/sudoers.d/user
EOF

USER user

RUN <<EOF
    set -ex
    echo "alias l='ls -aF'" >> ~/.bash_aliases
    echo "alias ll='ls -ahlF'" >> ~/.bash_aliases
    echo "alias ls='ls --color=auto --group-directories-first'" >> ~/.bash_aliases
EOF

# Install dungeon
ENV DOTNET_HostBuilder__ReloadConfigOnChange=false

WORKDIR /home/user/dungeon

COPY --link --chown=1000:1000 bin/Release/publish .

RUN <<EOF
    set -ex
    echo "alias d='cd ~/dungeon && dotnet bgg-dungeon.dll'" >> ~/.bash_aliases
    find . -type f -regextype posix-egrep -iregex '.*\.(dll|json|yaml)$' -exec chmod -x {} \;
EOF

ENTRYPOINT [ "dotnet", "bgg-dungeon.dll" ]
