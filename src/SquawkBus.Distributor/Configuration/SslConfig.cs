using System;
using System.Security.Cryptography.X509Certificates;

namespace SquawkBus.Distributor.Configuration
{
    public class SslConfig
    {
        public bool IsEnabled { get; set; }
        public string? CertFile { get; set; }
        public string? KeyFile { get; set; }
        public string? StoreLocation { get; set; }
        public string? SubjectName { get; set; }

        public X509Certificate2? ToCertificate()
        {
            if (!IsEnabled)
                return null;

            if (StoreLocation == null || string.IsNullOrWhiteSpace(StoreLocation))
            {
                if (CertFile == null || KeyFile == null)
                    throw new ApplicationException("Invalid SSL configuration");

                var certFile = Environment.ExpandEnvironmentVariables(CertFile);
                var keyFile = Environment.ExpandEnvironmentVariables(KeyFile);

                return X509Certificate2.CreateFromPemFile(certFile, keyFile);
            }
            else
            {
                var storeLocation = Enum.Parse<StoreLocation>(StoreLocation);
                if (SubjectName == null)
                    throw new ApplicationException("No subject name in config");

                X509Store store = new X509Store(storeLocation);
                try
                {
                    store.Open(OpenFlags.ReadOnly);

                    var currentCerts = store.Certificates.Find(X509FindType.FindByTimeValid, DateTime.Now, false);
                    var signingCert = currentCerts.Find(X509FindType.FindBySubjectName, SubjectName, false);
                    if (signingCert.Count == 0)
                        throw new ApplicationException("No certificate");
                    return signingCert[0];
                }
                finally
                {
                    store.Close();
                }
            }
        }
    }
}