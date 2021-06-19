using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using filmarsiv.entity;

namespace filmarsiv.webui.Models
{
    public class CategoryModel{
        public int CategoryId { get; set; }

        [Required(ErrorMessage="Kategori adi zorunludur.")]
        [StringLength(100,MinimumLength=5,ErrorMessage="Kategori icin 5-100 arasinda deger giriniz.")]
        public string Name { get; set; }
        [Required(ErrorMessage="Url zorunludur.")]
        [StringLength(100,MinimumLength=5,ErrorMessage="Url icin 5-100 arasinda deger giriniz.")]
        public string Url { get; set;}
        public List<Movie> Movies { get; set; }
    }
}