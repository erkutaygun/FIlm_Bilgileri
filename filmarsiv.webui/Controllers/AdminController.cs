using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using filmarsiv.business.Abstract;
using filmarsiv.entity;
using filmarsiv.webui.Extensions;
using filmarsiv.webui.Identity;
using filmarsiv.webui.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace filmarsiv.webui.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {

        private IMovieService _movieService;
        private ICategoryService _categoryService;
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<User> _userManager;

        public AdminController(IMovieService movieService, ICategoryService categoryService, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _movieService = movieService;
            _categoryService = categoryService;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task<IActionResult> UserEdit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var selectedRoles = await _userManager.GetRolesAsync(user);
                var roles = _roleManager.Roles.Select(i => i.Name);

                ViewBag.Roles = roles;
                return View(new UserDetailsModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed,
                    SelectedRoles = selectedRoles
                });
            }
            return Redirect("~/admin/user/list");
        }
        [HttpPost]
        public async Task<IActionResult> UserEdit(UserDetailsModel model, string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user != null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.UserName = model.UserName;
                    user.Email = model.Email;
                    user.EmailConfirmed = model.EmailConfirmed;

                    var result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        var userRoles = await _userManager.GetRolesAsync(user);
                        selectedRoles = selectedRoles ?? new string[] { };
                        await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles).ToArray<string>());
                        await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles).ToArray<string>());

                        return Redirect("/admin/user/list");
                    }
                }
                return Redirect("/admin/user/list");
            }
            return View(model);
        }
        public IActionResult UserList(){
            return View(_userManager.Users);
        }
        public async Task<IActionResult> RoleEdit(string id){
            
            var role = await _roleManager.FindByIdAsync(id);

            var members = new List<User>();
            var nonmembers = new List<User>();

            foreach (var user in _userManager.Users.ToList())
            {  
               var list = await _userManager.IsInRoleAsync(user,role.Name).ConfigureAwait(true)
                                ?members:nonmembers;
                list.Add(user);
            }
            var model = new RoleDetails(){
                Role = role,
                Memebers = members,
                NonMemebers = nonmembers
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> RoleEdit(RoleEditModel model){
            
            if(ModelState.IsValid){
                foreach (var userId in model.IdsToAdd ?? new string[]{})
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if(user!=null){
                        var result = await _userManager.AddToRoleAsync(user,model.RoleName);
                        if(!result.Succeeded){
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("",error.Description);
                            }
                        }
                    }
                }

                foreach (var userId in model.IdsToDelete ?? new string[]{})
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if(user!=null){
                        var result = await _userManager.RemoveFromRoleAsync(user,model.RoleName);
                        if(!result.Succeeded){
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("",error.Description);
                            }
                        }
                    }
                }
            }
            return Redirect("/admin/role/"+model.RoleId);
        }
        public IActionResult RoleList(){
            return View(_roleManager.Roles);
        }
        
        public IActionResult RoleCreate(){
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RoleCreate(RoleModel model)
        { 
            if(ModelState.IsValid){
                var result = await _roleManager.CreateAsync(new IdentityRole(model.Name));
                if(result.Succeeded){
                    return RedirectToAction("RoleList");
                }else{
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("",error.Description);
                    }
                }
            }
            return View(model);
        }

        public IActionResult MovieList(){
            return View(new MovieListViewModel(){
                Movies = _movieService.GetAll()
            });
        }
         public IActionResult CategoryList(){
            return View(new CategoryListViewModel(){
                Categories = _categoryService.GetAll()
            });
        }
        [HttpGet]
        public IActionResult MovieCreate(){
            return View();
        }
        [HttpPost]
        public IActionResult MovieCreate(MovieModel model){
            if(ModelState.IsValid){
                var entity = new Movie(){
                    Name = model.Name,
                    Url = model.Url,
                    Years = model.Years,
                    ImdbRating = model.ImdbRating,
                    RunTime = model.RunTime,
                    Language = model.Language,
                    ImageUrl = model.ImageUrl,
                    Details = model.Details
                    
                };
                if(_movieService.Create(entity)){
                    TempData.Put("message",new AlertMessage(){
                        Title="kayit eklendi.",
                        Message = "kayit eklendi.",
                        AlertType = "success"
                    });
                    return RedirectToAction("MovieList");
                }
                TempData.Put("message",new AlertMessage(){
                    Title="hata",
                    Message =_movieService.ErroMessage,
                    AlertType = "danger"
                });
                return View(model);
            }
            return View(model);
        }
         [HttpGet]
        public IActionResult CategoryCreate(){
                return View();
        }
        [HttpPost]
        public IActionResult CategoryCreate(CategoryModel model){
            if(ModelState.IsValid){
                var entity = new Category(){
                    Name = model.Name,
                    Url = model.Url,
                
                };
               _categoryService.Create(entity);
                TempData.Put("message",new AlertMessage(){
                    Title="Kategori eklendi",
                    Message = "Kategori eklendi",
                    AlertType = "success"
                });

                return RedirectToAction("CategoryList");
            }
            return View(model);
            
        }
        [HttpGet]
        public IActionResult MovieEdit(int? id){
               
               if(id==null){
                   return NotFound();
               }
                var entity = _movieService.GetByIdWithCategories((int)id);

                if(entity==null){
                    return NotFound();
                }
                var model = new MovieModel(){
                    MovieId = entity.MovieId,
                    Name = entity.Name,
                    Url = entity.Url,
                    Years = entity.Years,
                    ImdbRating = entity.ImdbRating,
                    RunTime = entity.RunTime,
                    Language = entity.Language,
                    ImageUrl = entity.ImageUrl,     
                    Details = entity.Details,
                    IsHome = entity.IsHome,
                    IsApproved = entity.IsApproved,
                    SelectedCategories = entity.MovieCategories.Select(i=>i.Category).ToList()
                };
                ViewBag.Categories = _categoryService.GetAll();
                return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> MovieEdit(MovieModel model,int[] categoryIds,IFormFile file){
            if(ModelState.IsValid){
                    var entity = _movieService.GetById(model.MovieId);
                        if(entity==null){
                            return NotFound();
                        }
                        entity.Name=model.Name;
                        entity.Url=model.Url;
                        entity.Years=model.Years;
                        entity.ImdbRating=model.ImdbRating;
                        entity.RunTime=model.RunTime;
                        entity.Language=model.Language;
                        entity.Details=model.Details;
                        entity.IsHome = model.IsHome;
                        entity.IsApproved = model.IsApproved;

                        if(file!=null){
                            var extention = Path.GetExtension(file.FileName);
                            var randomName = string.Format($"{Guid.NewGuid()}{extention}");
                            entity.ImageUrl = randomName;
                            var path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot\\img",randomName);

                            using(var stream = new FileStream(path,FileMode.Create)){
                               await file.CopyToAsync(stream);
                            }
                        }

                        if(_movieService.Update(entity,categoryIds)){
                             TempData.Put("message",new AlertMessage(){
                                Title="kayit guncellendi",
                                Message = "kayit guncellendi.",
                                AlertType = "success"
                            });
                            return RedirectToAction("MovieList");
                        }
                         TempData.Put("message",new AlertMessage(){
                            Title="hata",
                            Message = _movieService.ErroMessage,
                            AlertType = "danger"
                        });
                }
                ViewBag.Categories = _categoryService.GetAll();
                return View(model);
        }
        public IActionResult DeleteMovie(int movieId)
        {
            var entity = _movieService.GetById(movieId);

            if (entity != null)
            {
                _movieService.Delete(entity);
            }
            TempData.Put("message", new AlertMessage()
            {
                Title = "urun silindi",
                Message = $"{entity.Name} isimli urun silindi",
                AlertType = "danger"
            });
            return RedirectToAction("MovieList");
        }

        [HttpGet]
        public IActionResult CategoryEdit(int? id){
               
               if(id==null){
                   return NotFound();
               }
                var entity = _categoryService.GetByIdWithMovies((int)id);

                if(entity==null){
                    return NotFound();
                }
                var model = new CategoryModel(){
                    CategoryId = entity.CategoryId,
                    Name = entity.Name,
                    Url = entity.Url,
                    Movies = entity.MovieCategories.Select(m=>m.Movie).ToList()
                };
                return View(model);

            }
        [HttpPost]
        public IActionResult CategoryEdit(CategoryModel model){
            if(ModelState.IsValid){
                var entity = _categoryService.GetById(model.CategoryId);
                  if(entity==null){
                    return NotFound();
                }
                entity.Name = model.Name;
                entity.Url = model.Url;

                _categoryService.Update(entity);
                TempData.Put("message",new AlertMessage(){
                    Title="kategori guncellendi",
                    Message = $"{entity.Name} isimli kategori guncellendi",
                    AlertType = "success"
                });
                return RedirectToAction("CategoryList");
                }
            return View(model);
        }
 
        public IActionResult DeleteCategory(int categoryId){
            var entity = _categoryService.GetById(categoryId);

            if(entity!=null){
                _categoryService.Delete(entity);
            }
            TempData.Put("message",new AlertMessage(){
                    Title="kategori silindi",
                    Message = $"{entity.Name} isimli kategori silindi",
                    AlertType = "danger"
            });
            return RedirectToAction("CategoryList");
        }
        [HttpPost]
        public IActionResult DeleteFromCategory(int movieId,int categoryId)
        {
            _categoryService.DeleteFromCategory(movieId,categoryId);
            return Redirect("/admin/categories/"+categoryId);
        }
        public IActionResult AccessDenied(){
            return View();
        }
    }
}