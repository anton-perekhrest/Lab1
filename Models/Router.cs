using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RouterLab
{
    public partial class Router
    {
        public Router()
        {
            RouterStandart = new HashSet<RouterStandart>();
        }

        public int RouterId { get; set; }
        [Required(ErrorMessage = "Потрібно заповнити поле")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Довжина значення від 2 до 50 символів")]
        [Display(Name = "Назва моделі роутера")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Потрібно заповнити поле")]
        [Range(1997, 2020, ErrorMessage = "Введіть від 1997 до поточного")]
        [Display(Name = "Рік випуску моделі")]
        public int Year { get; set; }
        
        public int PriceId { get; set; }
        public int DiapasonId { get; set; }
        public int SpeedId { get; set; }
        [Display(Name = "Режим роботи роутера")]
        public virtual Diapason Diapason { get; set; }
        [Display(Name = "Ціна")]
        public virtual Price Price { get; set; }
        [Display(Name = "Швидкість")]
        public virtual Speed Speed { get; set; }
        public virtual ICollection<RouterStandart> RouterStandart { get; set; }
    }
}
