using System;
using System.Net;
using System.Text;
using MobileVehicleInspection.Contracts;

namespace MobileVehicleInspection.Api.Library
{
    public class DanishTransportAuthorityScraper : IVehicleInspectionLookup
    {
        private readonly ScraperSettings _settings;
        private readonly DanishTransportAuthorityResponseParser _parser;

        public DanishTransportAuthorityScraper(ScraperSettings settings, DanishTransportAuthorityResponseParser parser)
        {
            _settings = settings;
            _parser = parser;
        }

        public Vehicle ByRegistration(RegistrationNumber registration)
        {
            var client = new WebClient {Encoding = Encoding.UTF8};
            var url = new Uri(String.Format(_settings.UrlTemplate, registration));
            string result = null;
            try
            {
                result = client.DownloadString(url);
            }
            catch (WebException wex)
            {
                throw new InvalidOperationException(
                    string.Format("Unable to retrieve data from {0}, {1}, HTTP {2}", url, wex.Status.ToString(),
                                  wex.Response == null ? -1 : (int) ((HttpWebResponse) wex.Response).StatusCode), wex);
            }

            return _parser.Parse(result);
        }
    }
}