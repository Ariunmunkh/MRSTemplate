using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DXWebMRCS.Models
{
    public class TrainingRequest
    {
        public int ID { get; set; }

        public int TrainingID { get; set; }
      
        public int UserID { get; set; }
        /*
         * 0-Хүсэлт илгээсэн
         * 1-Хүлээн авсан
         * 9-Цуцалсан
        */
        public int Status { get; set; }
    }
}