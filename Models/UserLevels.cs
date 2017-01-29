using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Levels
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ExpNeeded { get; set; }

        public string LevelName { get; set; }

        public string LevelEffects { get; set; }

        public string EffectName { get; set; }

        public string LevelColor { get; set; }

    }
    public class LevelEffects
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string EffectUrl { get; set; }
    }
}