#!/bin/bash

cd ${SQUAWKBUS_ROOT}/src/SquawkBus.Distributor
dotnet run -- ${SQUAWKBUS_ROOT}/examples/server-config/password-file/appsettings.json
