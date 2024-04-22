#!/bin/bash

# Create a `secrets` folder
mkdir secrets
cd secrets
mkdir docker
cd ..

# Create a `db_root_password.txt` file
echo "rootpass" > secrets/docker/db_root_password.txt

# Create a `secrets.json` file with the following content
echo '{
  "ConnectionString": {
    "DefaultConnection": "server=172.20.0.10;port=3306;uid=root;pwd=rootpass;database=Hosys"
  },
  "Security": {
    "Argon2": {
      "Salt": "string",
      "Secret": "string",
      "AssociatedData": "string"
    },
    "Jwt": {
      "Secret": "string",
      "Issuer": "string",
      "Audience": "string",
      "ExpireIn": 30
    }
  }
}' > src/Infrastructure/Hosys.WebAPI/secrets.json
