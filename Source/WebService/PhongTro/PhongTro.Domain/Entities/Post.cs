﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhongTro.Domain.Entities
{
    public class Post
    {
        [Key]
        public Guid PostID { get; set; }

        [Required]
        [MaxLength(200)]
        public string Address { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double Price { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int NumberLodgers { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        public DateTime PostDate { get; set; } = DateTime.Now;
        public DateTime? LastUpdate { get; set; }
        public float TotalPoint { get; set; } = 0;
        public float NumberReviewer { get; set; } = 0;
        public Guid PhongTroUserID { get; set; }

        public virtual PhongTroUser PhongTroUser { get; set; }

        public virtual ICollection<BoardingHouseImage> Images { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<FavouritePost> FavouritePosts { get; set; }
    }
}