using System;
using ServiceStack.ServiceHost;

namespace MobileVehicleInspection.Api.Services
{
    public class MustBeLocalAttribute : Attribute, IHasRequestFilter
    {
        public void RequestFilter(IHttpRequest req, IHttpResponse res, object requestDto)
        {
            if (req.RemoteIp != "127.0.0.1" && req.RemoteIp != "::1")
                throw new UnauthorizedAccessException("Service not allowed");

        }

        public IHasRequestFilter Copy()
        {
            return this;
        }

        public int Priority 
        {
            get { return 0; }
        }
    }
}