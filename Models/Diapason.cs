using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RouterLab
{
    public partial class Diapason
    {
        public Diapason()
        {
            Router = new HashSet<Router>();
        }

        public int DiapasonId { get; set; }
        [Required(ErrorMessage = "Потрібно заповнити поле")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Довжина значення від 2 до 50 символів")]
        [Display(Name = "Режим роботи роутера")]
        public string Diapason1 { get; set; }

        public virtual ICollection<Router> Router { get; set; }
    }
}
