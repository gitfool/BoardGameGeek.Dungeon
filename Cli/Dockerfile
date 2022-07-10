# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/runtime-deps:6.0.6-jammy

LABEL org.opencontainers.image.source=https://github.com/gitfool/BoardGameGeek.Dungeon

# Install packages
RUN <<EOF
    set -ex
    apt-get update
    apt-get install --no-install-recommends -y curl sudo vim
    rm -rf /var/lib/apt/lists/*
EOF

# Install libssl 1.1; workaround dotnet 5.0 runtime dependency removed from ubuntu jammy
# renovate: datasource=repology depName=libssl1.1 packageName=ubuntu_20_04/openssl versioning=loose
RUN <<EOF
    set -ex
    version=1.1.1f-1ubuntu2.15
    curl -fsSL https://launchpad.net/ubuntu/+archive/primary/+files/libssl1.1_${version}_amd64.deb -o libssl1.1.deb
    DEBIAN_FRONTEND=noninteractive dpkg -i libssl1.1.deb
    rm -f libssl1.1.deb
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

COPY --chown=user:user bin/Release/linux-x64/publish .

RUN <<EOF
    set -ex
    echo "alias d='cd ~/dungeon && ./bgg-dungeon'" >> ~/.bash_aliases
    chmod -x config/*.yaml
EOF

ENTRYPOINT [ "./bgg-dungeon" ]
