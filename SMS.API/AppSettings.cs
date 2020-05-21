using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMS.API
{
    public class AppSettings
    {
        public class Jwt
        {
            public string Key { get; set; }
            public string Issuer { get; set; }
            public string ExpiryInMinutes { get; set; }
        }
    }
}