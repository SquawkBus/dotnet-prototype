#!/bin/bash

cd ${SQUAWKBUS_ROOT}/src/SquawkBus.Distributor
dotnet run -- ${SQUARKBUS_ROOT}/demos/server-config/jwt/appsettings.json
