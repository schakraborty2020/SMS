using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Business
{
    public class LoginObj
    {
        public Int64? StudioID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Permalink { get; set; }
        public string Host { get; set; }
        public string IP { get; set; }
        public long AppNameID { get; set; }
        public string DeviceID { get; set; }
        public long DeviceTypeID { get; set; }
        public string UserGUID { get; set; }
    }

    public class UserAuthenticationResponse
    {
        public int statuscode { get; set; }
        public string message { get; set; }
        public int count { get; set; }
        public Int64 user_id { get; set; }
        public Int64 studio_id { get; set; }
        public string full_name { get; set; }
        public string api_key { get; set; }
        public string api_token { get; set; }
    }

    public class UserApiTokenResponse
    {
        public int statuscode { get; set; }
        public string message { get; set; }
        public int count { get; set; }
        public string api_token { get; set; }
    }

    public class UserJwt
    {
        public string token_String { get; set; }
        public DateTime expiry { get; set; }
    }

    public class UserApiTokenObj
    {
        public Int64 user_id { get; set; }
        public string api_token { get; set; }
        public DateTime token_expiry { get; set; }
    }

    public class UserObj
    {
        public Int64 user_id { get; set; }
    }
}