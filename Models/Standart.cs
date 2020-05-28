using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RouterLab
{
    public partial class Standart
    {
        public Standart()
        {
            RouterStandart = new HashSet<RouterStandart>();
        }

        public int StandartId { get; set; }
        [Required(ErrorMessage = "Потрібно заповнити поле")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Довжина значення від 2 до 50 символів")]
        [Display(Name = "Стандарт роботи роутера")]
        public string Standart1 { get; set; }

        public virtual ICollection<RouterStandart> RouterStandart { get; set; }
    }
}
