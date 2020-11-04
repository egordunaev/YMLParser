FROM microsoft/mssql-server-linux:latest

# Creating work directory
RUN mkdir -p /usr/work
WORKDIR /usr/work

# Copying scripts into working directory
COPY . /usr/work/

# Granting permissions for the import-data.sh script to be executable
RUN chmod +x /usr/work/import-data.sh

EXPOSE 1433

CMD /bin/bash ./entrypoint.sh