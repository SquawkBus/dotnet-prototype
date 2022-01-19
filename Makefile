DISTRIBUTOR_VERSION=1.0.0-alpha1
AUTHENTICATION_VERSION=1.0.0-alpha1
MESSAGES_VERSION=1.0.0-alpha1
ADAPTERS_VERSION=1.0.0-alpha1
JWT_AUTHENTICATION_VERSION=1.0.0-alpha1
LDAP_AUTHENTICATION_VERSION=1.0.0-alpha1
PWD_AUTHENTICATION_VERSION=1.0.0-alpha1
MKPASSWD_VERSION=1.0.0-alpha1

CORE_SRC=src
EXTENSIONS_SRC=extensions/src
UTILS_SRC=utilities/src

DIST_WIN10_X64=distributor-${DISTRIBUTOR_VERSION}-win10-x64
DIST_LINUX_X64=distributor-${DISTRIBUTOR_VERSION}-linux-x64
DIST_OSX_X64=distributor-${DISTRIBUTOR_VERSION}-osx-x64

MKPASSWD_WIN10_X64=MakePassword-${MKPASSWD_VERSION}-win10-x64-sc
MKPASSWD_LINUX_X64=MakePassword-${MKPASSWD_VERSION}-linux-x64-sc
MKPASSWD_OSX_X64=MakePassword-${MKPASSWD_VERSION}-osx-x64-sc

.PHONY: all dist dotnet-build publish clean

all:
	@echo "targets: dist clean"

dist: publish copy-extensions

dotnet-build:
	dotnet build

.PHONY: publish-dist publish-mkpasswd

publish: publish-dist publish-mkpasswd

.PHONY: publish-dist-win10-x64 publish-dist-linux-x64 publish-dist-osx-x64

publish-dist: publish-dist-win10-x64 publish-dist-linux-x64 publish-dist-osx-x64

publish-dist-win10-x64:
	dotnet publish ${CORE_SRC}/SquawkBus.Distributor \
		-r win10-x64 \
		--self-contained true \
		-p:PublishSingleFile=true \
		-o build/${DIST_WIN10_X64_SC}
	cp ${CORE_SRC}/SquawkBus.Distributor/appsettings.json build/${DIST_WIN10_X64_SC}
	cp scripts/distributor.bat build/${DIST_WIN10_X64_SC}
	cd build && zip -r ${DIST_WIN10_X64_SC}.zip ${DIST_WIN10_X64_SC}
	rm -r build/${DIST_WIN10_X64_SC}

publish-dist-linux-x64:
	dotnet publish ${CORE_SRC}/SquawkBus.Distributor \
		-r linux-x64 \
		--self-contained true \
		-p:PublishSingleFile=true \
		-o build/${DIST_LINUX_X64_SC}
	cp ${CORE_SRC}/SquawkBus.Distributor/appsettings.json build/${DIST_LINUX_X64_SC}
	cp scripts/distributor.sh build/${DIST_LINUX_X64_SC}
	cd build && tar cvzf ${DIST_LINUX_X64_SC}.tar.gz ${DIST_LINUX_X64_SC}
	rm -r build/${DIST_LINUX_X64_SC}

publish-dist-osx-x64:
	dotnet publish ${CORE_SRC}/SquawkBus.Distributor \
		-r osx-x64 \
		--self-contained true \
		-p:PublishSingleFile=true \
		-o build/${DIST_OSX_X64_SC}
	cp ${CORE_SRC}/SquawkBus.Distributor/appsettings.json build/${DIST_OSX_X64_SC}
	cp scripts/distributor.sh build/${DIST_OSX_X64_SC}
	cd build && tar cvzf ${DIST_OSX_X64_SC}.tar.gz ${DIST_OSX_X64_SC}
	rm -r build/${DIST_OSX_X64_SC}

.PHONY: publish-mkpasswd-win10-x64 publish-mkpasswd-linux-x64 publish-mkpasswd-osx-x64

publish-mkpasswd: publish-mkpasswd-win10-x64 publish-mkpasswd-linux-x64 publish-mkpasswd-osx-x64

publish-mkpasswd-win10-x64:
	dotnet publish ${UTILS_SRC}/MakePassword \
		-r win10-x64 \
		--self-contained true \
		-p:PublishSingleFile=true \
		-o build/${MKPASSWD_WIN10_X64}
	cp scripts/mkpasswd.bat build/${MKPASSWD_WIN10_X64}
	cd build && zip -r ${MKPASSWD_WIN10_X64}.zip ${MKPASSWD_WIN10_X64}
	rm -r build/${MKPASSWD_WIN10_X64}

publish-mkpasswd-linux-x64:
	dotnet publish ${UTILS_SRC}/MakePassword \
		-r linux-x64 \
		--self-contained true \
		-p:PublishSingleFile=true \
		-o build/${MKPASSWD_LINUX_X64}
	cp scripts/mkpasswd.sh build/${MKPASSWD_LINUX_X64}
	cd build && tar cvzf ${MKPASSWD_LINUX_X64}.tar.gz ${MKPASSWD_LINUX_X64}
	rm -r build/${MKPASSWD_LINUX_X64}

publish-mkpasswd-osx-x64:
	dotnet publish ${UTILS_SRC}/MakePassword \
		-r osx-x64 \
		--self-contained true \
		-p:PublishSingleFile=true \
		-o build/${MKPASSWD_OSX_X64}
	cp scripts/mkpasswd.sh build/${MKPASSWD_OSX_X64}
	cd build && tar cvzf ${MKPASSWD_OSX_X64}.tar.gz ${MKPASSWD_OSX_X64}
	rm -r build/${MKPASSWD_OSX_X64}

.PHONY: copy-extensions copy-extension-jwtauthentication copy-extension-ldapauthentication copy-extension-pwdauthentication

copy-extensions: dotnet-build copy-extension-jwtauthentication copy-extension-ldapauthentication copy-extension-pwdauthentication

copy-extension-jwtauthentication: build
	cp -r ${EXTENSIONS_SRC}/SquawkBus.Extension.JwtAuthentication/bin/Debug/netstandard2.1 build/SquawkBus.Extension.JwtAuthentication-${JWT_AUTHENTICATION_VERSION}
	cd build && zip -r SquawkBus.Extension.JwtAuthentication-${JWT_AUTHENTICATION_VERSION}.zip SquawkBus.Extension.JwtAuthentication-${JWT_AUTHENTICATION_VERSION}
	cd build && tar czvf SquawkBus.Extension.JwtAuthentication-${JWT_AUTHENTICATION_VERSION}.tar.gz SquawkBus.Extension.JwtAuthentication-${JWT_AUTHENTICATION_VERSION}
	rm -r build/SquawkBus.Extension.JwtAuthentication-${JWT_AUTHENTICATION_VERSION}

copy-extension-ldapauthentication: build
	cp -r ${EXTENSIONS_SRC}/SquawkBus.Extension.LdapAuthentication/bin/Debug/netstandard2.1 build/SquawkBus.Extension.LdapAuthentication-${LDAP_AUTHENTICATION_VERSION}
	cd build && zip -r SquawkBus.Extension.LdapAuthentication-${LDAP_AUTHENTICATION_VERSION}.zip SquawkBus.Extension.LdapAuthentication-${LDAP_AUTHENTICATION_VERSION}
	cd build && tar czvf SquawkBus.Extension.LdapAuthentication-${LDAP_AUTHENTICATION_VERSION}.tar.gz SquawkBus.Extension.LdapAuthentication-${LDAP_AUTHENTICATION_VERSION}
	rm -r build/SquawkBus.Extension.LdapAuthentication-${LDAP_AUTHENTICATION_VERSION}

copy-extension-pwdauthentication: build
	cp -r ${EXTENSIONS_SRC}/SquawkBus.Extension.PasswordFileAuthentication/bin/Debug/netstandard2.1 build/SquawkBus.Extension.PasswordFileAuthentication-${PWD_AUTHENTICATION_VERSION}
	cd build && zip -r SquawkBus.Extension.PasswordFileAuthentication-${PWD_AUTHENTICATION_VERSION}.zip SquawkBus.Extension.PasswordFileAuthentication-${PWD_AUTHENTICATION_VERSION}
	cd build && tar czvf SquawkBus.Extension.PasswordFileAuthentication-${PWD_AUTHENTICATION_VERSION}.tar.gz SquawkBus.Extension.PasswordFileAuthentication-${PWD_AUTHENTICATION_VERSION}
	rm -r build/SquawkBus.Extension.PasswordFileAuthentication-${PWD_AUTHENTICATION_VERSION}

build:
	mkdir build

push-authentication:
	dotnet pack ${CORE_SRC}/SquawkBus.Authentication
	dotnet nuget push ${CORE_SRC}/SquawkBus.Authentication/bin/Debug/SquawkBus.Authentication.${AUTHENTICATION_VERSION}.nupkg \
		--api-key ${NUGET_API_KEY} \
		--source https://api.nuget.org/v3/index.json

push-messages:
	dotnet pack ${CORE_SRC}/SquawkBus.Messages
	dotnet nuget push ${CORE_SRC}/SquawkBus.Messages/bin/Debug/SquawkBus.Messages.${MESSAGES_VERSION}.nupkg \
		--api-key ${NUGET_API_KEY} \
		--source https://api.nuget.org/v3/index.json

push-adapters:
	dotnet pack ${CORE_SRC}/SquawkBus.Adapters
	dotnet nuget push ${CORE_SRC}/SquawkBus.Adapters/bin/Debug/SquawkBus.Adapters.${ADAPTERS_VERSION}.nupkg \
		--api-key ${NUGET_API_KEY} \
		--source https://api.nuget.org/v3/index.json

clean:
	-rm -r build
	find . -type d -name bin -exec rm -rf {} \;	
	find . -type d -name obj -exec rm -rf {} \;	
