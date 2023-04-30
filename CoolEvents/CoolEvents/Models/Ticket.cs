﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CoolEvents.Models
{
    [Table("Ticket")]
    public partial class Ticket
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int EventId { get; set; }

        [ForeignKey("EventId")]
        [InverseProperty("Tickets")]
        public virtual Event Event { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("Tickets")]
        public virtual User User { get; set; }
    }
}