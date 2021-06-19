using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using filmarsiv.entity;

namespace filmarsiv.webui.Models
{
    public class MovieModel{
        public int MovieId { get; set; }
        [Required(ErrorMessage="Film adi zorunludur.")]
        public string Name { get; set; }
        [Required(ErrorMessage="Film url zorunludur.")]
        public string Url { get; set; }
        [Required(ErrorMessage="Film yili zorunludur.")]
        public string Years { get; set; }
        [Required(ErrorMessage="Film Imdb rating zorunludur.")]
        public string ImdbRating { get; set; }
        [Required(ErrorMessage="Film runtime zorunludur.")]
        public string RunTime { get; set; }
        [Required(ErrorMessage="Film dili zorunludur.")]
        public string Language { get; set; }
        [Required(ErrorMessage="Film resmi zorunludur.")]
        public string ImageUrl { get; set; }
        [Required(ErrorMessage="Film detay zorunludur.")]
        public string Details { get; set; }
        public bool IsHome { get; set; }
        public bool IsApproved { get; set; }
        public List<Category> SelectedCategories { get; set; }
    }
}