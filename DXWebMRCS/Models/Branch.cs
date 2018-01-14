using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DXWebMRCS.Models
{
    public class Branch
    {
        //Салбар аймаг 
        public Branch()
        {
            //this.Branchs1 = new HashSet<Branch>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BranchID { get; set; }

        [Display(Name = "Нэр (Монгол)")]
        public string NameMon { get; set; }

        [Display(Name = "Нэр (Англи)")]
        public string NameEng { get; set; }

        [Display(Name = "Лого")]
        public string Logo { get; set; }

        [Display(Name = "Зураг")]
        public string Image { get; set; }

        [Display(Name = "Имайл")]
        public string email { get; set; }

        [Display(Name = "Утас")]
        public string phone { get; set; }

        [Display(Name = "Хаяг")]
        public string address { get; set; }

    }
}