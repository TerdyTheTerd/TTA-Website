using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class UserStats
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; } //Primary Key

        public string NickName { get; set; }
        public string Prefix { get; set; }
        public string ForumsGroup { get; set; }
        public string TagGroup { get; set; }
        public string FactionsName { get; set; }
        public int PointsEarned { get; set; }
        public int TotalPost { get; set; }
        public int MinutesPlayed { get; set; }
        public bool IsPremium { get; set; }
        public int PointsGiven { get; set; }
        public int ProfileViews { get; set; }
        public string Quote { get; set; }
        public string Bio { get; set; }
        public string ProfilePicture { get; set; }
        public string ProfileBanner { get; set; }
        public bool IsOnline { get; set; }
        public string Location { get; set; }
        public string ProfileLevel { get; set; } //Deprecated, rather than try and keep this is sync with the levels table, create a method that will return current level and xp since xp will need to be calculated anyways
        public int ProfileExp { get; set; }
        public int Gender { get; set; }
        public int Age { get; set; }
        public string NameEffect { get; set; }
        public int StorageAvailable { get; set; }
        public int StorageUsed { get; set; }
        public DateTime JoinDate { get; set; }

        public virtual ApplicationUser User { get; set; }
        [ForeignKey("User")]
        public string DisplayName { get; set; }

        public string ApplicationUserId { get; set; }

    }
}