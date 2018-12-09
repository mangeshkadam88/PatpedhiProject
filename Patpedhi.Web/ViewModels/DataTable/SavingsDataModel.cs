using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Patpedhi.Web.ViewModels.DataTable
{
    public class SavingsDataModel
    {
        public Guid saving_id { get; set; }
        public string amount_string { get; set; }
        public string modified_date_string { get; set; }
        public string is_approved_string { get; set; }
        public string approved_by { get; set; }
        public string approved_on { get; set; }
        public string is_active_string { get; set; }
        public Guid user_id { get; set; }
    }
}
