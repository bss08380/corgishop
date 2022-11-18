FROM mcr.microsoft.com/mssql/server:2022-latest

USER root

COPY create-corgishop.sql create-corgishop.sql
COPY import-data.sh import-data.sh
COPY entrypoint.sh entrypoint.sh

RUN chmod +x entrypoint.sh

CMD /bin/bash ./entrypoint.sh