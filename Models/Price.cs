using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RouterLab
{
    public partial class Price
    {
        public Price()
        {
            Router = new HashSet<Router>();
        }
        [Key]
        public int PriceId { get; set; }
        [Required(ErrorMessage = "Потрібно заповнити поле")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Довжина значення від 2 до 50 символів")]
        [Display(Name = "Ціна")]
        public string Price1 { get; set; }

        public virtual ICollection<Router> Router { get; set; }
    }
}
