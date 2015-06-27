﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cyclo.Models
{
    public class Category
    {
        public int ID { get; set; }
        [DisplayName("Отдел")]
        public String name { get; set; }
    }
    public class SubCategory
    {
        public int ID { get; set; }
        public Category parent { get; set; }
        [DisplayName("Направление деятельности")]
        public String name { get; set; }
    }
    public class Event
    {
        public int ID { get; set; }
        public SubCategory subCategory { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Дата начала")]
        public DateTime startDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Дата завершения")]
        public DateTime endDate { get; set; }
        [DisplayName("Описание события")]
        [DataType(DataType.Html)]
        [AllowHtml]
        [UIHint("tinymce_full")]
        public String description { get; set; }
        [DisplayName("Название события")]
        public String name { get; set; }
    }
    public class Job
    {
        public int ID { get; set; }
        public Event inEvent {get;set;}
        [DisplayName("Постановка задачи")]
        [DataType(DataType.Html)]
        [AllowHtml]
        [UIHint("tinymce_full")]
        public String description { get; set; }
        [DisplayName("Отчет о проделанной работе")]
        [DataType(DataType.Html)]
        [AllowHtml]
        [UIHint("tinymce_full")]
        public String report { get; set; }
        public int userID { get; set; }
    }
    public class CycloDBContext : DbContext
    {
        public DbSet<Category> categories { get; set; }
        public DbSet<SubCategory> subCategories { get; set; }
        public DbSet<Event> events { get; set; }
        public DbSet<Job> Jobs { get; set; }
    }
}