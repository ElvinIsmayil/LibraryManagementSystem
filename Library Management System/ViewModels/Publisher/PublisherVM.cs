﻿using System.ComponentModel.DataAnnotations;

namespace Library_Management_System.ViewModels.Publisher
{
    public class PublisherVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Adress { get; set; }
        public double Rating { get; set; }
    }
}
