using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RouterLab
{
    public partial class RouterStandart
    {
        public int RouterStandartId { get; set; }
        public int RouterId { get; set; }
        public int StandartId { get; set; }
        [Display(Name = "Назва моделі роутера")]

        public virtual Router Router { get; set; }
        [Display(Name = "Стандарт роботи роутера")]
        public virtual Standart Standart { get; set; }
    }
}
