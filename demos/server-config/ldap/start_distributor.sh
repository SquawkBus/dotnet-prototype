#!/bin/bash

cd ${SQUAWKBUS_ROOT}/src/SquawkBus.Distributor
dotnet run -- ${SQUAWKBUS_ROOT}/demos/server-config/ldap/appsettings.json
