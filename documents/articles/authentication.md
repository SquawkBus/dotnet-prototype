# Authentication

The distributor server has a pluggable authentication module. As you can see from the
default `appsettings.json` the default authenticator is the `NullAuthenticator`.

```json
{
  "distributor": {
    ...
    "authentication": {
      "assemblyPath": null,
      "assemblyName": "SquawkBus.Authentication",
      "typeName": "SquawkBus.Authentication.NullAuthenticator",
      "args": []
    },
    ...
  }
  ...
}
```

The client must connect with the appropriate client side authenticator. For the
`NullAuthenticator` this is the `NullClientAuthenticator` from the
`SquawkBus.Adapters` assembly. If no authenticator is specified
when the client is created the null authenticator is used by default.

```cs
var client = Client.Create(Dns.GetHostName(), 9001);
```

This is equivalent to the code above.

```cs
var authenticator = new NullClientAuthenticator();
var client = Client.Create(Dns.GetHostName(), 9001, authenticator: authenticator);
```

## Password File Authenticator

### Distributor

A simple password file authenticator is provided as an extension. The extension
can be downloaded from the 
[Releases](https://github.com/SquawkBus/SquawkBus/releases) 
page as `SqauwkBus.Extensions.PasswordFileAuthentication`. Unpack the
files into the folder "extensions" under the distributor. Also download the
`MakePassword` utility and unpack it into a `utilities` folder under the
distributor. The folder structure should look like as follows.

```
distributor
|   appsettings.json
|   distributor.bat
|   SquawkBus.Distributor.exe
|   SquawkBus.Distributor.pdb
|
+---extensions
|   +---SquawkBus.Extensions.PasswordFileAuthentication
|           SquawkBus.Extensions.PasswordFileAuthentication.deps.json
|           SquawkBus.Extensions.PasswordFileAuthentication.dll
|           SquawkBus.Extensions.PasswordFileAuthentication.pdb
|           Newtonsoft.Json.dll
|
+---utilities
    +---MakePassword
            MakePassword.exe
            MakePassword.pdb
            passwords.json
```

Now create copy the `appsettings.json` to `appsettings-pwd.json` and create a
script to run the distributor called `distributor-pwd.bat` with the following
contents.

```bat
REM Start the distributor

set SQUAWKBUS_HOME=%~dp0
set CONFIG_FILE=%SQUAWKBUS_HOME%\appsettings-pwd.json

SquawkBus.Distributor %CONFIG_FILE%
```

No change the `authentication` section of `appsettings-pwd.json` to be the
following.

```json
{
  "distributor": {
    ...
    "authentication": {
      "assemblyPath": "%SQUAWKBUS_HOME%/extensions/SquawkBus.Extensions.PasswordFileAuthentication/SquawkBus.Extensions.PasswordFileAuthentication.dll",
      "assemblyName": "SquawkBus.Extensions.PasswordFileAuthentication",
      "typeName": "SquawkBus.Extensions.PasswordFileAuthentication.PasswordFileAuthenticator",
      "args": [
        "%SQUAWKBUS_HOME%/utilities/MakePassword/passwords.json"
      ]
    },    ...
  }
  ...
}
```

For simplicity we we not set up the SSL configuration here, but as usernames
and passwords will be exchange this should also be configured.

The password file authenticator takes a single argument in the `args` property
which is the path to the passwords file. The `SQUAWKBUS_HOME` environment
variable was set up in the launch script. The password file is created using
the `MakePassword` utility and for simplicity we will keep the file in the
same folder. Make a password in the following manner.

```
MakePassword passwords.json rob
Enter Password: ********
```

Not start the distributor with the `distributor-pwd` script. You should see the
following:

```
info: SquawkBus.Distributor.Server[0]
      Starting server version 5.1.0.0
info: SquawkBus.Distributor.Server[0]
      Server started
info: SquawkBus.Distributor.Program[0]
      Waiting for SIGTERM/SIGINT on PID 21864
info: SquawkBus.Distributor.Acceptor[0]
      Listening on 0.0.0.0:9001 with SSL disabled with BASIC authentication
```

The `BASIC authentication` indicates the password file authenticator is being
used.

### Client

The client should use the `BasicClientAuthenticator` as follows.

```cs
var authenticator = new BasicClientAuthenticator(username, password);
var client = Client.Create(server, 9002, authenticator: authenticator);
```

Using the code found in [Getting Started - Windows](getting-started-windows.md)
change the client creation for the subscriber and publisher to use the code above
with the username and password that were created with `MakePassword`.

## LDAP Authenticator

There is an extension which can perform LDAP authentication.

### Distributor

Download the `SquawkBus.Extensions.LdapAuthentication` package and
unpack it in the same manner as before. Copy the `appsettings.json` to
`appsettings-ldap.json` and change the `authentication` section to the following.

```json
{
  "distributor": {
    ...
    "authentication": {
      "assemblyPath": "%SQUAWKBUS_HOME%/extensions/SquawkBus.Extensions.LdapAuthentication/SquawkBus.Extensions.LdapAuthentication.dll",
      "assemblyName": "SquawkBus.Extensions.LdapAuthentication",
      "typeName": "SquawkBus.Extensions.LdapAuthentication.LdapAuthenticator",
      "args": [
          "ldap.example.com",
          "636"
      ]
    },
    ...
  }
  ...
}
```

This extension takes two arguments in the `args` array property. The first is
the host of the LDAP service, and the second is the port.

### Client

The `BasicClientAuthenticator` class is used for the client, as the same
properties are required (username and password). This time pass in valid LDAP
credentials.

## JWT Authentication

The JWT authenticator uses JSON web tokens to perform authentication.

### Distributor

Download the `SquawkBus.Extensions.JwtAuthentication` package and
unpack it in the same manner as before. Copy the `appsettings.json` to
`appsettings-jwt.json` and change the `authentication` section to the following.

```json
{
  "distributor": {
    ...
    "authentication": {
      "assemblyPath": "%SQUAWKBUS_HOME%/extensions/SquawkBus.Extensions.JwtAuthentication/SquawkBus.Extensions.JwtAuthentication.dll",
      "assemblyName": "SquawkBus.Extensions.JwtAuthentication",
      "typeName": "SquawkBus.Extensions.JwtAuthenticator",
      "args": [
        "A secret of more than 15 characters"
      ]
    },
    ...
  }
  ...
}
```

This extension takes a single argument in the `args` array property, the secret
used to sign the token.

### Client

The client uses the `TokenClientAuthenticator` for authentication in the
following manner.

```cs
var authenticator = new TokenClientAuthenticator(jwtToken);
var client = Client.Create(server, 9002, authenticator: authenticator);
```
