[![Typing SVG](https://readme-typing-svg.demolab.com?font=Fira+Code&weight=500&size=46&pause=1000&color=EB7B59&center=true&vCenter=true&random=false&width=435&height=100&lines=HosysAPI)](https://git.io/typing-svg)

> Note: The application is still under development. Several or all of the features are not available or work incorrectly.

HosysAPI is an API that works in conjunction with the Hosys Front-End environment. This is an application responsible for containing open source tools created within the API and executing processes on the server side.

This application is designed to run in a Docker container, but does not take away the possibility of creating a fully integrated environment with the application.

---

# Getting Started 🔥
The application contains some dependencies that are not in this repository. Secrets are ignored by `.gitignore` and must be added manually.

## Secrets 🕵️

Before you start, there is a file `create_dependences.sh` in the root directory of the project which helps you to automatically create the dependencies of the secrets mentioned in the Linux operating system. Run the following commands to start it:

1. Give permission to the file on your machine:
```bash
chmod +x create_secrets.sh
```

2. Run the file:
```bash
./create_secrets.sh
```

First, there are the application secrets, which indicate the connection to the database, database passwords and the cryptographic security methods or hashes.
```json
{
  "ConnectionString": {
    "DefaultConnection": "server=172.20.0.10;port=3306;uid=root;pwd={Your_DB_Password};database=Hosys"
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
}
```
> Note: For the `ExpireIn` key, you need to specify the time in minutes that the token will expire.

### Linux secrets
For Linux, the `secrets.json` must be stored inside the root directory of the [`Hosys.WebAPI`](/src/Infrastructure/Hosys.WebAPI) component containing the following contents.

### Windows secrets
In Windows, secrets are stored outside the scope of the application. In this case, we should store the secrets in the directory `%appdata%\Microsoft\UserSecrets\fa878202-b992-4624-a584-5b73ab589ca4`. 

If the folder with the specified Guid does not exist, you must create it.

### Docker secrets
When using the `docker compose up` command to instantiate the project's dependencies in a Docker container, some secrets must be added.

By default, there should be a `secrets/docker` folder in the root of the project, with a file called `db_root_password.txt`, containing the password for your database that will be run in Docker.
```
/
├── secrets
│   └── docker
│       └── db_root_password.txt
└── src
```
- Example of `db_root_password.txt` content:
```txt
my_password_content_here123
```
