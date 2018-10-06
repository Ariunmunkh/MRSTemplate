using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sub.Models
{
    public class BranchViewModel
    {
        public int BranchID { get; set; }
        public string NameMon { get; set; }
        public string NameEng { get; set; }
        public string Logo { get; set; }
        public string Image { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public int menuID { get; set; }
        public News news { get; set; }
        public PagedList.IPagedList<News> newsList { get; set; }
    }

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
    public class News
    {
        [Key]
        public int CID { get; set; }
        [Required]
        [Display(Name = "Гарчиг Монгол")]
        public string TitleMon { get; set; }
        [Display(Name = "Гарчиг Англи")]
        public string TitleEng { get; set; }
        [Display(Name = "Агуулга Монгол")]
        [AllowHtml]
        public string BodyMon { get; set; }
        [AllowHtml]
        [Display(Name = "Агуулга Англи")]
        public string BodyEng { get; set; }
        [Display(Name = "Зураг")]
        public string Image { get; set; }
        public string ImageMedium { get; set; }

        [Display(Name = "Цэс")]
        public int? MenuID { get; set; }

        [Display(Name = "Салбар")]
        public int? BranchID { get; set; }

        [Display(Name = "Төрөл")]
        public string ContentType { get; set; }
        [Display(Name = "Өөрчилсөн огноо")]
        public System.DateTime Date { get; set; }

        [Display(Name = "Таг")]
        public string Tags { get; set; }
    }

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

        [Display(Name = "Эрэмбэ")]
        public int? OrderNum { get; set; }

        [Display(Name = "Баганы дугаар")]//олон баганаар үед утга авна.
        public int? ColumnNum { get; set; }
    }

    public class BranchPost
    {
        public int menuID { get; set; }
    }
}