﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AspNetMVC_API.Models.ViewModels
{
    public class StudentViewModel
    {
        //baş harfleri küçük harfe çevirdik json datalar küçük harfli olduğundan.(id,name.....)
        public int id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Adınız en az 2 en çık 50 karakter uzunluğunda olmalıdır!")]
        public string name { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Soyadınız en az 2 en çık 50 karakter uzunluğunda olmalıdır!")]
        public string surname { get; set; }
        [Required]
        public DateTime registerdate { get; set; } = DateTime.Now;
    }
}