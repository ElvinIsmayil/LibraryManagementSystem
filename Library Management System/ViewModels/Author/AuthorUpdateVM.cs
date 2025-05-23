﻿using Library_Management_System.Enums;
using Library_Management_System.ViewModels.AuthorContact;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Library_Management_System.ViewModels.Author
{
    public class AuthorUpdateVM 
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Surname { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }

        [Required]
        public Gender Gender { get; set; } 

        [StringLength(500)]
        public string? Biography { get; set; }

        public string? ImageUrl { get; set; }

        public IFormFile? Image { get; set; }

        [Required]
        public AuthorContactUpdateVM AuthorContactUpdateVM { get; set; }

        public List<SelectListItem>? GenderList { get; set; }



    }
}
