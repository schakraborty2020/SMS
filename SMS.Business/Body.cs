using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Business
{
    public class SetBodyObj
    {
        public Int64 studio_id { get; set; }
        public Int64 user_id { get; set; }
        public SetBodyObj.SetRestriction set_restrictions { get; set; }
        public SetBodyObj.Body body { get; set; }
        public SetBodyObj.BodyCustomProp body_custom_prop { get; set; }
        public List<SetBodyObj.Address> addresses { get; set; }
        public List<SetBodyObj.Address> phones { get; set; }
        public List<SetBodyObj.Address> emails { get; set; }
        public List<SetBodyObj.Address> financials { get; set; }
        public class SetRestriction
        {
            public bool is_set_custom_prop { get; set; }
            public bool is_set_address { get; set; }
            public bool is_set_phone { get; set; }
            public bool is_set_email { get; set; }
            public bool is_set_financial { get; set; }
        }
        public class Body
        {
            public Int64 body_id { get; set; }
            public Int64? body_typ_lu_id { get; set; }
            public Int64? parent_body_id { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
        }
        public class BodyCustomProp
        {
            public string work_place_name { get; set; }
        }
        public class Address
        {
            public string address1 { get; set; }
            public string address2 { get; set; }
            public Int64? country_lu_id { get; set; }
            public Int64? state_lu_id { get; set; }
            public string city { get; set; }
            public string zip { get; set; }
        }
        public class Phone
        {
            public string formatted_number { get; set; }
        }
        public class Email
        {
            public string email_address { get; set; }
        }
        public class Financial
        {
            public string bank_name { get; set; }
        }
    }
    public class BodyListObj
    {
        public Int64 studio_id { get; set; }
        public Int64? site_list_lu_id { get; set; }
        public List<BodyType> body_types { get; set; }
        public List<BodyType> body_sub_types { get; set; }

        public class BodyType
        {
            public Int64 body_typ_lu_id { get; set; }
        }
        public class BodySubType
        {
            public Int64 body_sub_typ_lu_id { get; set; }
        }
    }
}