namespace DXWebMRCS.Migrations
{
    using DXWebMRCS.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Security;
    using WebMatrix.WebData;

    internal sealed class Configuration : DbMigrationsConfiguration<DXWebMRCS.Models.UsersContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DXWebMRCS.Models.UsersContext context)
        {
            //this.AddUserAndRoles();
            context.Branchs.AddOrUpdate(x => x.NameMon,
        new Branch() { NameMon = "Архангай", NameEng = "Arkhangai" },
        new Branch() { NameMon = "Багануур", NameEng = "Baganuur" },
        new Branch() { NameMon = "Багахангай", NameEng = "Bagakhangai" },
        new Branch() { NameMon = "Баян-Өлгий", NameEng = "Bayan-Ulgii" },
        new Branch() { NameMon = "Баянгол", NameEng = "Bayangol" },
        new Branch() { NameMon = "Баянзүрх", NameEng = "Bayanzurkh" },
        new Branch() { NameMon = "Баянхонгор", NameEng = "Bayankhongor" },
        new Branch() { NameMon = "Булган", NameEng = "Bulgan" },
        new Branch() { NameMon = "Говь-Алтай", NameEng = "Govi-Altai" },
        new Branch() { NameMon = "Говьсүмбэр", NameEng = "Govisumber" },
        new Branch() { NameMon = "Дархан", NameEng = "Darkhan" },
        new Branch() { NameMon = "Дорноговь", NameEng = "Dornogovi" },
        new Branch() { NameMon = "Дорнод", NameEng = "Dornod" },
        new Branch() { NameMon = "Дундговь", NameEng = "Dundgovi" },
        new Branch() { NameMon = "Мандал", NameEng = "Mandal" },
        new Branch() { NameMon = "Завхан", NameEng = "Zavkhan" },
        new Branch() { NameMon = "Налайх", NameEng = "Nalaikh" },
        new Branch() { NameMon = "Онцгой байдлын ерөнхий газар", NameEng = "NEMA" },
        new Branch() { NameMon = "Орхон", NameEng = "Orkhon" },
        new Branch() { NameMon = "Сонгинохайрхан", NameEng = "Songinokhairkhan" },
        new Branch() { NameMon = "Сэлэнгэ", NameEng = "Selenge" },
        new Branch() { NameMon = "Сүхбаатар", NameEng = "Sukhbaatar" },
        new Branch() { NameMon = "Сүхбаатар дүүрэг", NameEng = "Sukhbaatar district" },
        new Branch() { NameMon = "Төв", NameEng = "Tuv" },
        new Branch() { NameMon = "Төмөр зам", NameEng = "Tumur zam" },
        new Branch() { NameMon = "Увс", NameEng = "Uvs" },
        new Branch() { NameMon = "Хан-Уул дүүрэг", NameEng = "Khan-Uul" },
        new Branch() { NameMon = "Хил хамгаалах ерөнхий газар", NameEng = "Khil hamgaalah" },
        new Branch() { NameMon = "Ховд", NameEng = "Khovd" },
        new Branch() { NameMon = "Хэнтий", NameEng = "Khentii" },
        new Branch() { NameMon = "Хөвсгөл", NameEng = "Khentii" },
        new Branch() { NameMon = "Чингэлтэй дүүрэг", NameEng = "Chingeltei" },
        new Branch() { NameMon = "Өвөрхангай", NameEng = "Uvurkhangai" },
        new Branch() { NameMon = "Өмнөговь", NameEng = "Umnugovi" }
        );

            context.Tag.AddOrUpdate(x => x.NameMon,
        new Tag() { NameMon = "Байгууллагын хөгжил", NameEng = "Organization Development" },
        new Tag() { NameMon = "Гамшгийн менежментийн хөтөлбөр", NameEng = "Disaster Management programme" },
        new Tag() { NameMon = "Нийгмийн оролцоог дэмжих хөтөлбөр", NameEng = "Social Inclusion, Development programme" },
        new Tag() { NameMon = "Нийгмийн эрүүл мэндийг дэмжих хөтөлбөр", NameEng = "Public Health Promotion programme" },
        new Tag() { NameMon = "Хүүхэд, залуучуудын хөтөлбөр", NameEng = "Red Cross Youth programme" }
        );
        }
    }
}
