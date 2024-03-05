#!/bin/sh

USER=user

usermod -g ${GID} ${USER}
umask ${UMASK} > /dev/null

chown -R ${UID}:${GID} /root/bin
chown -R ${UID}:${GID} /root/bin/Lagrange.OneBot
chown -R ${UID}:${GID} /root/data

chmod +x /root/bin/Lagrange.OneBot

su-exec $USER /root/bin/Lagrange.OneBot "$@"