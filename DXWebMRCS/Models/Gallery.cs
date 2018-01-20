using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DXWebMRCS.Models
{
    public class Gallery
    {
        public Gallery()
        {
            //this.Gallerys1 = new HashSet<Gallery>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GalleryID { get; set; }

        [Display(Name = "Нэр (Монгол)")]
        public string TitleMon { get; set; }

        [Display(Name = "Нэр (Англи)")]
        public string TitleEng { get; set; }

        [Display(Name = "Зураг")]
        public string Image { get; set; }

        [Display(Name = "Таг")]
        public string Tags { get; set; }

    }

}