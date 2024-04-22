namespace SoupAndDungeon.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Players
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string ScorePoints { get; set; }
    }
}
