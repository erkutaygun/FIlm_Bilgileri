using System.Collections.Generic;
using filmarsiv.entity;

namespace filmarsiv.webui.Models
{
        public class MovieDetailModel
    {
        public Movie Movie { get; set; }
        public List<Category> Categories { get; set;}
    }
}