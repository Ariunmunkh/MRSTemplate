using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DXWebMRCS.Models
{
    public class SafeUser
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Та гүйцэд бөглөнө үү.")]
        public string SafeType { get; set; }
        public string DepartmentType { get; set; }
        [Required(ErrorMessage = "Та гүйцэд бөглөнө үү.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Та гүйцэд бөглөнө үү.")]
        public string FirstName { get; set; }
        public string Email { get; set; }
        [Required(ErrorMessage = "Та гүйцэд бөглөнө үү.")]
        public DateTime DateOfBirth { get; set; }
        [Required(ErrorMessage = "Та гүйцэд бөглөнө үү.")]
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public string WorkPhone { get; set; }
        public string OtherPhone { get; set; }
        [Required(ErrorMessage = "Та гүйцэд бөглөнө үү.")]
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string StateProvince { get; set; }
        public string District { get; set; }
        [Required(ErrorMessage = "Та гүйцэд бөглөнө үү.")]
        public string NowCountry { get; set; }
        public string NowAddress { get; set; }
        public string HomeAddress2 { get; set; }
        public string NowStateProvince { get; set; }
        public string NowDistrict { get; set; }
        [Display(Name = "Би аюулгүй, сайн байна.")]
        public bool SafeMail1 { get; set; }
        [Display(Name = "Би болон манай гэр бүл аюулгүй, сайн байгаа.")]
        public bool SafeMail2 { get; set; }
        [Display(Name = "Одоогоор түр орогнох байранд байрлаж байна.")]
        public bool SafeMail3 { get; set; }
        [Display(Name = "Одоогоор гэртээ байна.")]
        public bool SafeMail4 { get; set; }
        [Display(Name = "Одоогоор найз'\'хамаатан'\'хөршийнхөө гэрт байрлаж байна.")]
        public bool SafeMail5 { get; set; }
        [Display(Name = "Одоогоор зочид буудалд байрлаж байна.")]
        public bool SafeMail6 { get; set; }
        [Display(Name = "Боломж олдохоор залгана аа.")]
        public bool SafeMail7 { get; set; }
        [Display(Name = "Боломж олдмогц цахим шуудангаар захидал илгээнэ ээ.")]
        public bool SafeMail8 { get; set; }
        [Display(Name = "Боломж олдмогц шуудангаар захидал илгээнэ ээ.")]
        public bool SafeMail9 { get; set; }
        [Display(Name = "Би аюулгүй байгаа ба нүүлгэн шилжүүлэх нөхцөл байдалд байна.")]
        public bool SafeMail10 { get; set; }
        [Display(Name = "Би нүүлгэн шилжүүлэх ажиллагаанд хамрагдаж, одоо аюулгүй байна.")]
        public bool SafeMail11 { get; set; }
        [Display(Name = "Намайг түр орогнох байр руу нүүлгэн шилжүүлсэн.")]
        public bool SafeMail12 { get; set; }
        [Display(Name = "Би найз'\'хамаатныхаа гэр рүү нүүн шилжсэн байдалтай байна.")]
        public bool SafeMail13 { get; set; }
        [Display(Name = "Би одоогоор гэртээ аюулгүй байна.")]
        public bool SafeMail14 { get; set; }
        public string OtherNews { get; set; }
    }
}