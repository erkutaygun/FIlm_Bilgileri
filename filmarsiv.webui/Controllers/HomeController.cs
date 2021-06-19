using System;
using System.Collections.Generic;
using filmarsiv.business.Abstract;
using filmarsiv.data.Abstract;
using filmarsiv.webui.Models;
using Microsoft.AspNetCore.Mvc;

namespace filmarsiv.webui.Controllers
{
     // localhost:5001/home
    public class HomeController:Controller
    {
        // localhost:5000/home
        private IMovieService _movieService;

        public HomeController(IMovieService movieService){
            this._movieService=movieService;
        }  
        public IActionResult Index()
        {
            var movieViewModel = new MovieListViewModel()
            {
                Movies = _movieService.GetHomePageMovies()
            };

            return View(movieViewModel);
        }

        public IActionResult About()
        {
            return View();
        }

         public IActionResult Contact()
        {
            return View("MyView");
        }

    }
        
}
