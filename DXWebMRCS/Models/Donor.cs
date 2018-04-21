using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DXWebMRCS.Models
{
    public class Donor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DID { get; set; }

        [Display(Name = "Хүний нэр (Монгол)")]
        public string Pnamemon { get; set; }
        [Display(Name = "Хүний нэр (Англи)")]
        public string Pnameeng { get; set; }
        [Display(Name = "Хэлсэн ишлэл (Монгол)")]
        public string ParagraphMon { get; set; }
        [Display(Name = "Хэлсэн ишлэл (Англи)")]
        public string ParagraphEng { get; set; }
        [Display(Name = "Албан тушаал (Монгол)")]
        public string PositionMon { get; set; }
        [Display(Name = "Албан тушаал (Англи)")]
        public string PositionEng { get; set; }
        [Display(Name = "Зураг 100x100")]
        public string Image { get; set; }
    }
}