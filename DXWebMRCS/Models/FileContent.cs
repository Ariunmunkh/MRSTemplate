using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DXWebMRCS.Models
{
    public class FileContent
    {
        public int Id { get; set; }
        public string TitleEng { get; set; }
        public string TitleMon { get; set; }
        public string DescriptionEng { get; set; }
        public string DescriptionMon { get; set; }
        public string Image { get; set; }
        public string FilePath { get; set; }
        [NotMapped]
        private string _Path;
        public string Path
        {
            set { _Path = value; }
            get { return @"~\Content\Uploadfile"; }
        }
    }
}