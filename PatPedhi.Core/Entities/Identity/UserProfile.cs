using System;
using System.Collections.Generic;
using System.Text;

namespace PatPedhi.Core.Entities.Identity
{
    public class UserProfile : BaseEntity
    {
        public string first_name { get; set; }
        public string middle_name { get; set; }
        public string last_name { get; set; }
        public DateTime date_of_birth { get; set; }
        public bool is_active { get; set; }
        public bool is_approved { get; set; }
        public string profile_photo_url { get; set; }
        public string signature_photo_url { get; set; }
    }
}
