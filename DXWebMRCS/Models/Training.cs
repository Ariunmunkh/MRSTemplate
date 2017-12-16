using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DXWebMRCS.Models
{
    public class Training
    {
        //Явагдах сургалт
        public Training()
        {
            //this.Trainings1 = new HashSet<Training>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TrainingID { get; set; }

        [Display(Name = "Нэр (Монгол)")]
        public string NameMon { get; set; }

        [Display(Name = "Нэр (Англи)")]
        public string NameEng { get; set; }

        [Display(Name = "Агуулга (Монгол)")]
        public string ContentMon { get; set; }

        [Display(Name = "Агуулга (Англи)")]
        public string ContentEng { get; set; }

        
        [Display(Name = "Хаана")]
        public string Where { get; set; }

        
        [Display(Name = "Хэзээ")]
        public DateTime When { get; set; }

        
        [Display(Name = "Үргэлжлэх хугацаа")]
        public decimal Duration { get; set; }

        /*
         0-Бүртгэж байна
         1-Хүлээгдэж байна
         2-Заралсан
         3-Явагдаж байна
         4-Дууссан
         5-Цуцалсан*/
        [Display(Name = "Төлөв")]
        public decimal Status { get; set; }
       			

    }
}