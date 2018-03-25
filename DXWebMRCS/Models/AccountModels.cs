using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace DXWebMRCS.Models
{

    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("DefaultConnection")
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Menu>().ToTable("Menu");
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<SafeUser> SafeUsers { get; set; }
        public DbSet<SliderPhoto> SliderPhotos { get; set; }
        public DbSet<Branch> Branchs { get; set; }
        public DbSet<Training> Trainings { get; set; }
        public DbSet<TrainingRequest> TrainingRequests { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<Gallery> Galleries { get; set; }
        public DbSet<TagDetail> TagDetails { get; set; }
        public DbSet<FileContent> FileContents { get; set; }

        public DbSet<Elearn> Elearn { get; set; }
        public DbSet<eService> eService { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Magazine> Magazine { get; set; }

        public System.Data.Entity.DbSet<DXWebMRCS.Models.videos> videos { get; set; }
    }
    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string CityTown { get; set; }
        public string StateProvince { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }

        //what is your preferred way of contact?
        public string orderField1 { get; set; }//phone;email;sms;

        //would you like to join our mailing list and receive news and updates from red cross?
        public string orderField2 { get; set; }//yes;no

        //have you worked or volunteered with Red Cross before?
        public string orderField3 { get; set; }//no;volunteered;worked

        //which areas do you have interest in volunteering?
        public bool orderField41 { get; set; }//disaster managerment/preparedness
        public bool orderField42 { get; set; }//social care and inclusion
        public bool orderField43 { get; set; }//health promotion
        public bool orderField44 { get; set; }//youth movement
        public bool orderField45 { get; set; }//other (please specify)
        public string orderField451 { get; set; }//other (please specify) -> note field

        //Tell us a bit about yourself (your study, experience, what you'd like to do as a volunteer)
        public string orderField5 { get; set; }//note field

        public DateTime BirthOfDay { get; set; }
        public int Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string AvatarPath { get; set; }
        public int? BranchId { get; set; }
    }

    #region Change password ViewModel
    public class ChangePasswordSendEmailModel
    {
        [Required(ErrorMessage = "Та мэйл хаягаа оруулна уу.")]
        [RegularExpression("\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*", ErrorMessage = "Please enter a valid email address.")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        public string Email { get; set; }
    }

    public class ChangePasswordModel
    {

        public string email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
    #endregion

    #region Login and register ViewModel
    public class LoginModel
    {
        [Required(ErrorMessage = "Та мэйл хаягаа оруулна уу.")]
        [RegularExpression("\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*", ErrorMessage = "Please enter a valid email address.")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Та нууц үгээ оруулна уу.")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        bool rememberMe;
        [Display(Name = "Remember me?")]
        public bool RememberMe
        {
            get { return rememberMe; }
            set { rememberMe = value; }
        }
    }

    public class ExternalLoginConfirmationViewModel
    {
        [Required(ErrorMessage = "Та нэр ээ оруулна уу.")]
        [Display(Name = "Нэр")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Та мэйл хаягаа оруулна уу.")]
        [Display(Name = "Мэйл")]
        public string Email { get; set; }
    }

    public class RegisterModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Та овог нэрээ оруулна уу.")]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Та өөрийн нэрээ оруулна уу.")]
        [Display(Name = "User name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Та төрсөн он сар өдөрөө оруулна уу.")]
        [Display(Name = "Birth Of Day")]
        public DateTime BirthOfDay { get; set; }

        [Required(ErrorMessage = "Та хүйсээ сонгоно уу.")]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Та утасны дугаараа оруулна уу.")]
        [Display(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Та мэйл хаягаа оруулна уу.")]
        [RegularExpression("\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*", ErrorMessage = "Please enter a valid email address.")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Та нууц үгээ оруулна уу.")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string EditPassword { get; set; }

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string CityTown { get; set; }
        public string StateProvince { get; set; }
        public string Country { get; set; }
        public string orderField1 { get; set; }//phone;email;sms;

        //would you like to join our mailing list and receive news and updates from red cross?
        public string orderField2 { get; set; }//yes;no

        //have you worked or volunteered with Red Cross before?
        public string orderField3 { get; set; }//no;volunteered;worked

        //which areas do you have interest in volunteering?
        public bool orderField41 { get; set; }//disaster managerment/preparedness
        public bool orderField42 { get; set; }//social care and inclusion
        public bool orderField43 { get; set; }//health promotion
        public bool orderField44 { get; set; }//youth movement
        public bool orderField45 { get; set; }//other (please specify)
        public string orderField451 { get; set; }//other (please specify) -> note field

        //Tell us a bit about yourself (your study, experience, what you'd like to do as a volunteer)
        public string orderField5 { get; set; }//note field
    }

    public class EditRegisterModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Та овог нэрээ оруулна уу.")]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Та өөрийн нэрээ оруулна уу.")]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Та төрсөн он сар өдөрөө оруулна уу.")]
        [Display(Name = "Birth Of Day")]
        public DateTime BirthOfDay { get; set; }

        public string Gender { get; set; }

        [Required(ErrorMessage = "Та утасны дугаараа оруулна уу.")]
        [Display(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Та мэйл хаягаа оруулна уу.")]
        [RegularExpression("\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*", ErrorMessage = "Please enter a valid email address.")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        public string AvatarPath { get; set; }
        public string EditPassword { get; set; }
    }
    #endregion

    #region Branch and Training ViewModel
    public class BranchRegisterModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [RegularExpression("\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*", ErrorMessage = "Please enter a valid email address.")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        [Required]
        [Display(Name = "Салбар сонгох")]
        public int BranchId { get; set; }
    }

    public class TrainingModel
    {
        public int TrainingID { get; set; }
        public string NameMon { get; set; }
        public string NameEng { get; set; }
        public string ContentMon { get; set; }
        public string ContentEng { get; set; }
        public string Where { get; set; }
        public DateTime When { get; set; }
        public decimal Duration { get; set; }

        public string Image { get; set; }
        /*
         0-Бүртгэж байна
         1-Хүлээгдэж байна
         2-Заралсан
         3-Явагдаж байна
         4-Дууссан
         5-Цуцалсан*/
        public decimal Status { get; set; }
        public int? RequestID { get; set; }
        public int? UserID { get; set; }
        /*
         * 0-Хүсэлт илгээсэн
         * 1-Хүлээн авсан
         * 9-Цуцалсан
        */
        public int? RequestStatus { get; set; }
        public int? type { get; set; }
    }

    public class BranchViewModel
    {
        public int BranchID { get; set; }
        public string NameMon { get; set; }
        public string NameEng { get; set; }
        public string Logo { get; set; }
        public string Image { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public int menuID { get; set; }
        public News news { get; set; }
        public PagedList.IPagedList<DXWebMRCS.Models.News> newsList { get; set; }
    }

    #endregion

    public class EnumValue
    {
        public string Text { get; set; }
        public int ValueId { get; set; }
    }


    public class GalleryModel
    {
        public int GalleryID { get; set; }
        public string TitleMon { get; set; }
        public string TitleEng { get; set; }
        public string Image { get; set; }
        public int TagID { get; set; }
    }

}