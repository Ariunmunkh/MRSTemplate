using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DXWebMRCS.Models
{
    public class Project
    {
        [Key]
        public int ProjectID { get; set; }
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
        [Display(Name = "Эхлэх огноо")]
        public DateTime? BeginDate { get; set; }
        [Display(Name = "Дуусах огноо")]
        public DateTime? EndDate { get; set; }
        [Display(Name = "Төрөл")]
        //Гамшигийн менежмент;->DisasterManagement
        //Нийгмийн оролцоог дэмжих;->SupportSocialParticipation
        //Эрүүл мэндийн хөтөлбөр;->HealthProgram
        //Хүүхэд, залуучуудын хөтөлбөр;->ChildAndYouthProgram
        public string Type { get; set; }
    }
}