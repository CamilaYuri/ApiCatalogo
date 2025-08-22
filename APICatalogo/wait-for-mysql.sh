#!/bin/sh

host="$1"
shift
cmd="$@"

until mysql -h "$host" -u root -pSecretPassword01 -e "SELECT 1"; do
  >&2 echo "MySQL ainda não está pronto - aguardando..."
  sleep 2
done

>&2 echo "MySQL pronto - iniciando API"

exec "$@"
