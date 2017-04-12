﻿namespace LMDB.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;

    public class Genre
    {
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; }

        public virtual ICollection<Movie> Movies { get; set; }
    }
}
