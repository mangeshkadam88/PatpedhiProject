using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Patpedhi.Web.ViewModels
{
    public class SavingsModel
    {
        public Guid SavingId { get; set; }
        public Guid UserId { get; set; }

        [Required]
        [StringLength(50)]
        [DataType(DataType.Currency)]
        [Display(Name = "Amount")]
        public string Amount { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Approved")]
        public bool IsApproved { get; set; }

        public bool IsActive { get; set; }
    }
}
