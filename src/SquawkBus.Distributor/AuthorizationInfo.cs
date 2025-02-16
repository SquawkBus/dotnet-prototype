using System.Collections.Generic;

namespace SquawkBus.Distributor
{
    public class AuthorizationInfo
    {
        public AuthorizationInfo(bool isAuthorizationRequired, ISet<int>? entitlements)
        {
            IsAuthorizationRequired = isAuthorizationRequired;
            Entitlements = entitlements ?? new HashSet<int>();
        }

        public bool IsAuthorizationRequired { get; }
        public ISet<int> Entitlements { get; }
    }
}
