FROM mcr.microsoft.com/dotnet/runtime-deps:6.0.4-focal

LABEL org.opencontainers.image.source=https://github.com/gitfool/BoardGameGeek.Dungeon

# Install packages
RUN apt-get update \
    && apt-get install --no-install-recommends -y sudo vim \
    && rm -rf /var/lib/apt/lists/*

# Add non-root user
RUN groupadd --gid 1000 user \
    && useradd --uid 1000 --gid 1000 --shell /bin/bash -m user \
    && echo "user ALL=(ALL:ALL) NOPASSWD: ALL" > /etc/sudoers.d/user \
    && chmod 0440 /etc/sudoers.d/user

USER user

RUN echo "alias l='ls -aF'" >> ~/.bash_aliases \
    && echo "alias ll='ls -ahlF'" >> ~/.bash_aliases \
    && echo "alias ls='ls --color=auto --group-directories-first'" >> ~/.bash_aliases

# Install dungeon
ENV DOTNET_HostBuilder__ReloadConfigOnChange=false

WORKDIR /home/user/dungeon

COPY --chown=user:user bin/Release/linux-x64/publish .

RUN echo "alias d='cd ~/dungeon && ./bgg-dungeon'" >> ~/.bash_aliases \
    && chmod -x config/*.yaml

ENTRYPOINT [ "./bgg-dungeon" ]
