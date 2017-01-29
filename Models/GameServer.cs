using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Tick
    {
        public int Id { get; set; }
        public float TickReading { get; set; }
        public DateTime DateRead { get; set; }
    }
}