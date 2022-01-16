using System;
using System.DirectoryServices.Protocols;
using System.IO;
using System.Net;
using System.Security;

using SquawkBus.Authentication;
using SquawkBus.Messages;

namespace SquawkBus.Extensions.LdapAuthentication
{
    public class LdapAuthenticator : IAuthenticator
    {
        public LdapAuthenticator(string[] args)
        {
            if (args.Length != 2)
                throw new ArgumentException("usage: <ldap-server> <ldap-port>");

            Server = Environment.ExpandEnvironmentVariables(args[0]);

            if (int.TryParse(args[1], out var port))
                Port = port;
            else
                throw new ArgumentException("Expected the second argument ldap-port to be an integer");
        }

        public string Method => @"LDAP";
        public string Server { get; }
        public int Port { get; }

        public AuthenticationResponse Authenticate(Stream stream)
        {
            var reader = new DataReader(stream);
            var connectionString = reader.ReadString();
            var connectionDetails = LdapConnectionDetails.Parse(connectionString);
            var identifier = new LdapDirectoryIdentifier(Server, Port);

            using (var conn = new LdapConnection(identifier))
            {
                try
                {
                    //conn.SessionOptions.SecureSocketLayer = true;
                    //conn.AuthType = (AuthType.)authType;
                    var credentials = new NetworkCredential(connectionDetails.Username, connectionDetails.Password);
                    conn.Bind(credentials);

                    return new AuthenticationResponse(
                        connectionDetails.Username,
                        Method,
                        connectionDetails.Impersonating,
                        connectionDetails.ForwardedFor,
                        connectionDetails.Application);
                }
                catch (LdapException)
                {
                    throw new SecurityException();
                }
            }
        }
    }
}