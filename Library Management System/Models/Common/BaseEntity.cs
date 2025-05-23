﻿namespace Library_Management_System.Models.Common
{
    public abstract class BaseEntity
    {
        public int Id { get; set; } 
        public bool IsDeleted { get; set; } 
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }
    }
}
