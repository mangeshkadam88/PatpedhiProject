using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Patpedhi.Web.ViewModels.DataTable
{
    public class UsersDataModel
    {
        public Guid user_id { get; set; }
        public string first_name { get; set; }
        public string middle_name { get; set; }
        public string last_name { get; set; }
        public string date_of_birth_string { get; set; }
        public string role_string { get; set; }
        public string is_approved_string { get; set; }
        public string is_active_string { get; set; }
    }
}
