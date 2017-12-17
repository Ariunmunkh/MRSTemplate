using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DXWebMRCS.Models
{
    public class Menu
    {
        public Menu()
        {
            //this.Menus1 = new HashSet<Menu>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MenuID { get; set; }
        [Display(Name = "Нэр (Монгол)")]
        public string NameMon { get; set; }
        [Display(Name = "Нэр (Англи)")]
        public string NameEng { get; set; }
        [Display(Name = "Үндсэн цэс сонгох")]
        public string NavigateUrl { get; set; }

        [Display(Name = "Төрөл")]
        public string MenuType { get; set; }

        [Display(Name = "Зураг")]
        public string Image { get; set; }

        public int? ParentId { get; set; }

        [Display(Name = "Салбар")]
        public int? BranchID { get; set; }

    }
}