namespace SoupAndDungeon.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Items
    {
        public int Id { get; set; }

        [Required]
        public string ItemName { get; set; }

        [Required]
        public string Image { get; set; }

        public string ItemDescription { get; set; }
    }
}
