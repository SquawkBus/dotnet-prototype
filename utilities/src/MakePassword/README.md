# MakePassword

## Overview

This is a utility program to set a password in a password file for
use with `SquawkBus.Extensions.PasswordFileAuthenticator`.

## Usage

Provide the location of the password file and the username.

```bash
MakePassword $ dotnet run -- passwords.json tom
Enter password: *******
```