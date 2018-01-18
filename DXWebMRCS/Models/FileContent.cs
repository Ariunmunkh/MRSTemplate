using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DXWebMRCS.Models
{
    public class FileContent
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Та Англи гарчигаа оруулна уу.")]
        [Display(Name = "Гарчиг Англи")]
        public string TitleEng { get; set; }
        [Required(ErrorMessage = "Та Монгол гарчигаа оруулна уу.")]
        [Display(Name = "Гарчиг Монгол")]
        public string TitleMon { get; set; }
        [Required(ErrorMessage = "Та Англи тайлбараа оруулна уу.")]
        [Display(Name = "Тайлбар Англи")]
        public string DescriptionEng { get; set; }
        [Required(ErrorMessage = "Та Монгол тайлбараа оруулна уу.")]
        [Display(Name = "Тайлбар Монгол")]
        public string DescriptionMon { get; set; }
        [Required(ErrorMessage = "Та зурагаа оруулна уу.")]
        [Display(Name = "Зураг")]
        public string Image { get; set; }
        public string FilePath { get; set; }
        [NotMapped]
        [Required]
        [DataType(DataType.Upload)]
        [Display(Name = "Файл оруулна уу.")]
        public HttpPostedFileBase File { get; set; }

    }
}