using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace filmarsiv.entity
{
    public class Movie
    {
        public int MovieId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Years { get; set; }
        public string ImdbRating { get; set; }
        public string RunTime { get; set; }
        public string Language { get; set; }
        public string ImageUrl { get; set; }
        public string Details { get; set; }
        public bool IsHome { get; set; }
        public bool IsApproved { get; set;}

        public List<MovieCategory> MovieCategories { get; set; }
    }
}