#!/bin/sh

USER_ID=${UID:-0}

if [ "$USER_ID" -ne 0 ]; then
    chown -R $USER_ID /app/data
    useradd --shell /bin/sh -u $USER_ID -o -c "" -m user
    usermod -a -G root user
    export HOME=/home/user
    exec /usr/sbin/gosu user /app/bin/Lagrange.OneBot "$@"
else
    exec /app/bin/Lagrange.OneBot "$@"
fi