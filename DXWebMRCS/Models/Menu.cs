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

        [Display(Name = "Агуулга (Монгол)")]
        [AllowHtml]
        public string BodyMon { get; set; }
        [AllowHtml]
        [Display(Name = "Агуулга (Англи)")]
        public string BodyEng { get; set; }
        [Display(Name = "Зураг")]
        public byte[] Image { get; set; }
        public System.DateTime Date { get; set; }


        //[ForeignKey("Menu1")]
        public int? ParentId { get; set; }

        //public virtual ICollection<Menu> Menus1 { get; set; }
        //public virtual Menu Menu1 { get; set; }
    }
}