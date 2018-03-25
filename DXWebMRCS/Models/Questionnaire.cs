using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DXWebMRCS.Models
{
    public class Questionnaire
    {
        [Key]
        public int QID { get; set; }
        public int UID { get; set; }
        public int LID { get; set; }
        [Required]
        [Display(Name = @"1. Сургалт ойлгомжтой, сургагч багш сэдвийн талаар мэдлэг сайтай байсан.
        The training was easy to understand an instructors were knowledgeable")]
        public int c1a1 { get; set; }
        [Required]
        [Display(Name = @"2. Сургалтын чанар сайн байсан
        The quality of teaching was good")]
        public int c1a2 { get; set; }
        [Required]
        [Display(Name = @"3. Сургалтанд зөв материал ба болон дуу дүрс хэрэглэгдсэн
        Good training aids and audio-visual aids were used")]
        public int c1a3 { get; set; }
        [Required]
        [Display(Name = @"4. Сургалтанд оролцогчдыг илүү идэвхтэй оролцохыг уриалж байсан.
        Class participation and interaction were encouraged")]
        public int c1a4 { get; set; }
        [Required]
        [Display(Name = @"5. Оролцогчдод асуулт тавихад хангалттай хугацаа өгөгдөж байсан
        Adequate time was provided for attendee questions")]
        public int c1a5 { get; set; }
        [Required]
        [Display(Name = @"6. Сургалтын талаар санал, хүсэлт. Feedback of training")]
        public string c1a6 { get; set; }
        [Required]
        [Display(Name = @"1. Сургалт ойлгомжтой, сургагч багш сэдвийн талаар мэдлэг сайтай байсан. The training was easy to understand an instructors were knowledgeable")]
        public int c2a1 { get; set; }
        [Required]
        [Display(Name = @"2. Сургалтын чанар сайн байсан
        The quality of teaching was good")]
        public int c2a2 { get; set; }
        [Required]
        [Display(Name = @"3. Сургалтанд зөв материал ба болон дуу дүрс хэрэглэгдсэн
        Good training aids and audio-visual aids were used")]
        public int c2a3 { get; set; }
        [Required]
        [Display(Name = @"4. Сургалтанд оролцогчдыг илүү идэвхтэй оролцохыг уриалж байсан.
        Class participation and interaction were encouraged")]
        public int c2a4 { get; set; }
        [Required]
        [Display(Name = @"5. Оролцогчдод асуулт тавихад хангалттай хугацаа өгөгдөж байсан
        Adequate time was provided for attendee questions")]
        public int c2a5 { get; set; }
        [Required]
        [Display(Name = @"1. Сургалт ойлгомжтой, сургагч багш сэдвийн талаар мэдлэг сайтай байсан.
        The training was easy to understand an instructors were knowledgeable")]
        public int c3a1 { get; set; }
        [Required]
        [Display(Name = @"2. Сургалтын чанар сайн байсан
        The quality of teaching was good")]
        public int c3a2 { get; set; }
        [Required]
        [Display(Name = @"3. Сургалтанд зөв материал болон дуу дүрс хэрэглэгдсэн
        Good training aids and audio-visual aids were used")]
        public int c3a3 { get; set; }
        [Required]
        [Display(Name = @"4. Сургалтанд оролцогчдыг илүү идэвхтэй оролцохыг уриалж байсан.
        Class participation and interaction were encouraged")]
        public int c3a4 { get; set; }
        [Required]
        [Display(Name = @"5. Оролцогчдод асуулт тавихад хангалттай хугацаа өгөгдөж байсан
      Adequate time was provided for attendee questions")]
        public int c3a5 { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Асуумж бөглөсөн хугацаа")]
        public System.DateTime DateEnd { get; set; }
        public virtual userid userid { get; set; }
        public virtual lessonid lessonid { get; set; }

    }
}