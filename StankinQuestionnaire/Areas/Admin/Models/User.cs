using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web;
using StankinQuestionnaire.Model;
using System.ComponentModel.DataAnnotations;

namespace StankinQuestionnaire.Areas.Admin.Models
{
    public class UserViewModel
    {
        public long UserID { get; set; }
        public string UserName { get; set; }
        public string SubvisionName { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
    }

    public class UserEditModel
    {
        public long ID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public long? SubvisionID { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsChecker { get; set; }
        public IEnumerable<SelectListItem> Subdivisions { get; set; }
    }

    public class UsersFromFile
    {
        //[Required, FileExtensions(Extensions = "csv",
        //   ErrorMessage = "Формат файла должен быть csv")]
        [Display(Name = "Файл")]
        public HttpPostedFileBase File { get; set; }
        [Display(Name = "Кодировка")]
        public EncodeType EncodeType { get; set; }
    }

    public enum EncodeType
    {
        UTF8,
        UTF32,
        ASCII
    }

    public class Rating
    {
        public int Year { get; set; }
        public string CategoryName { get; set; }
        public int Point { get; set; }
    }
}