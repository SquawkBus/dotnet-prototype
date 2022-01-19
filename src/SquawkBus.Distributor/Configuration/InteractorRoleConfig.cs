using System;
using System.Collections.Generic;
using System.Linq;

using SquawkBus.Distributor.Roles;

namespace SquawkBus.Distributor.Configuration
{
    public class InteractorRoleConfig
    {
        public List<Role>? Allow { get; set; }
        public List<Role>? Deny { get; set; }

        public InteractorRole ToInteractorRole(string host, string user)
        {
            var expandedHost = Environment.ExpandEnvironmentVariables(host);

            return new InteractorRole(
              expandedHost,
              user,
              Allow?.Aggregate(Role.None, (aggregate, role) => aggregate | role) ?? Role.None,
              Deny?.Aggregate(Role.None, (aggregate, role) => aggregate | role) ?? Role.None);
        }
    }
}
