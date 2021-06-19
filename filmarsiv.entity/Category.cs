using System.Collections.Generic;

namespace filmarsiv.entity
{
    public class Category
    {
        public int CategoryId { get; set; }

        public string Name { get; set; }
        public string Url { get; set;}
        public List<MovieCategory> MovieCategories { get; set; }
    }
}