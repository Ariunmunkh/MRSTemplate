﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public byte[] Image { get; set; }
        [Display(Name = "Төрөл")]
        public string ContentType { get; set; }
        public System.DateTime Date { get; set; }
    }
}