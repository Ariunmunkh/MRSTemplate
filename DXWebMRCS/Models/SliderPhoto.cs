﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DXWebMRCS.Models
{
    public class SliderPhoto
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Зураг")]
        public string ImagePath { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}