using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Business
{
    public class Studio
    {
        public Int64? studio_id { get; set; }
        public string api_key { get; set; }
        public string lg_logo_name { get; set; }
        public string md_logo_name { get; set; }
        public string sm_logo_name { get; set; }
        public string fav_icon_name { get; set; }
    }

    public class GeneralSetting
    {
        public Int64? studio_id { get; set; }
        public string studio_name { get; set; }
        public string studio_title { get; set; }
    }

    public class StudioStartupObj
    {
        public Int64? studio_id{ get; set; }
        public string web_app_url { get; set; }
    }
    public class StudioStartupInfo
    {
        public int? statuscode { get; set; }
        public string message { get; set; }
        public int? count { get; set; }
        public Int64? studio_id { get; set; }
        public string studio_title { get; set; }
        public string api_key { get; set; }
        public string lg_logo_name { get; set; }
        public string md_logo_name { get; set; }
        public string sm_logo_name { get; set; }
        public string fav_icon_name { get; set; }
    }

    public class StudioConfigObj
    {
        public Int64? studio_id { get; set; }
    }

    public class StudioConfig
    {
        public int? statuscode { get; set; }
        public string message { get; set; }
        public int? count { get; set; }
        public Studio studio { get; set; }
        public GeneralSetting general_setting { get; set; }
    }
}