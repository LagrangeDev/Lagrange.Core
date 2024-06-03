#!/bin/sh

USER=user

usermod -o -u ${UID} ${USER} > /dev/null
groupmod -o -g ${GID} ${USER} > /dev/null
usermod -g ${GID} ${USER} > /dev/null

chown -R ${UID}:${GID} /app > /dev/null

exec su-exec $USER /app/bin/Lagrange.OneBot "$@"