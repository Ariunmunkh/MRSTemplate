﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DXWebMRCS.Models
{
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

    public class Tag
    {
        [Key]
        public int TagID { get; set; }

        [Display(Name = "Гарчиг Монгол")]
        public string NameMon { get; set; }

        [Display(Name = "Гарчиг Англи")]
        public string NameEng { get; set; }
    }

    public class TagDetail
    {
        [Key]
        public int TagDetailID { get; set; }

        public string Source { get; set; }

        public int SourceID { get; set; }

        public int TagID { get; set; }

    }
}