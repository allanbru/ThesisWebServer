using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThesisWebServer
{
    public struct SslCertSubject
    {
        public string? CommonName { get; set; }
    }

    public struct SslCertIssuer
    {
        public string? CountryName { get; set; }
        public string? StateOrProvinceName { get; set; }
        public string? LocalityName { get; set; }
        public string? OrganizationName { get; set; }
        public string? CommonName { get; set; }
    }

    public struct SslCert
    {
        public SslCertSubject? Subject { get; set; }
        public SslCertIssuer? Issuer { get; set; }
        public int? Version { get; set; }
        public string? SerialNumber { get; set; }
        public string? NotBefore { get; set; }
        public string? NotAfter { get; set; }
        public Dictionary<string, string> SubjectAltName { get; set; }
        public string? OCSP { get; set; }
        public string? CaIssuers { get; set; }
    }
}
