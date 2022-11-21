using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonSpied.Class
{
    public class ApiResponse
    {
        public String http_status_code;
        public String visible_sources;
        public String available_sources;
        public Int32 persons_count;
        public String search_id;
        public Query query;
        public AvailableData available_data;
        public Person person;

        #region Query Class
        public class Query
        {
            public List<EmailsQuery> emails; 
        }

        public class EmailsQuery
        {
            public String address;
            public String address_md5;
        }
        #endregion

        public class AvailableData
        {
            public PremiumAvailableData premium;
        }

        public class PremiumAvailableData
        {
            public Int32 jobs;
            public Int32 addresses;
            public Int32 phones;
            public Int32 mobile_phones;
            public Int32 landline_phones;
            public Int32 languages;
            public Int32 user_ids;
            public Int32 social_profiles;
            public Int32 names;
            public Int32 genders;
            public Int32 emails;
        }

        #region Person Class
        public class Person
        {
            public String id;
            public String match;
            public String search_pointer;
            public List<NamesPerson> names;
            public List<EmailsPerson> emails;
            public List<PhonesPerson> phones;
            public GenderPerson gender;
            public List<LanguagesPerson> languages;
            public List<AddressesPerson> addresses;
            public List<JobsPerson> jobs;
            public List<UserIdsPerson> user_ids;
            public List<UrlsPerson> urls;
        }

        public class NamesPerson
        {
            public DateTime valid_since;//"2008-10-08",
            //public DateTime last_seen;
            public String first;
            public String middle;
            public String last;
            public String display;
        }

        public class EmailsPerson
        {
            public DateTime valid_since; //2014-12-01
            public DateTime last_seen; //2018-01-24
            public Boolean email_provider; //false,
            public String address;
            public String address_md5;
            public String type;
        }

        public class PhonesPerson
        {
            public DateTime valid_since; //2018-01-24
            public DateTime last_seen; //2018-01-24
            public String type;
            public Int32 country_code;
            public Int32 number;
            public String display;
            public String display_international;
        }

        public class GenderPerson
        {
            public DateTime valid_since;
            public DateTime last_seen;
            public String content;
        }

        public class LanguagesPerson
        {
            public String language;
            public String display;
        }

        public class AddressesPerson
        {
            public DateTime valid_since;
            public String country;
            public String city;
            public String street;
            public Int32 zip_code;
            public String display;
        }

        public class JobsPerson
        {
            public DateTime valid_since;
            public String organization;
            public String display;
        }
        
        public class UserIdsPerson
        {
            public String content;
        }
        
        public class UrlsPerson
        {
            public String source_id;
            public String domain;
            public String name;
            public String category;
            public String url;
        }
        
        #endregion
    }
}
