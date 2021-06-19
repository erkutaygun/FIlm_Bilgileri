using System.Threading.Tasks;
using filmarsiv.webui.EmailServices;
using filmarsiv.webui.Extensions;
using filmarsiv.webui.Identity;
using filmarsiv.webui.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace filmarsiv.webui.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class AccountController:Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private IEmailSender _emailSender;


        public AccountController(UserManager<User> userManager,SignInManager<User> signInManager,IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }
        public IActionResult Login(string ReturnUrl=null){
            return View(new LoginModel(){
                ReturnUrl = ReturnUrl
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
         public async Task<IActionResult> Login(LoginModel model){
            if(!ModelState.IsValid){
                return View(model);
            }
            
            var user = await _userManager.FindByNameAsync(model.UserName);
            if(user==null){
                ModelState.AddModelError("","Bu kullanici adi ile daha once hesap olusturulmamis");
                return View(model);
            }

            if(!await _userManager.IsEmailConfirmedAsync(user)){
                ModelState.AddModelError("","Lutfen email hesabinizi gelen link ile hesabinizi onaylayiniz.");
                return View(model);
            }
            var result = await _signInManager.PasswordSignInAsync(user,model.Password,true,false);

            if(result.Succeeded){
                return Redirect(model.ReturnUrl??"~/");
            }
            ModelState.AddModelError("","Girilen kullanici adi veya parola yanlis.");
            return View(model);
        }
        public IActionResult Register(){
            return View();
        }
         [HttpPost]
         [ValidateAntiForgeryToken]
         public async Task<IActionResult> Register(RegisterModel model){
            if(!ModelState.IsValid){
                return View();
            }

            var user = new User(){
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user,model.Password);
            if(result.Succeeded){
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var url = Url.Action("ConfirmEmail","Account",new {
                    userId = user.Id,
                    token = code
                });
                await _emailSender.SendEmailAsync(model.Email,"Hesabinizi onaylayiniz",$"Lutfen email hesabinizi onaylamak icin linke <a href='https://localhost:44325{url}'>tiklayiniz</a>");
                return RedirectToAction("Login","Account");
         }
            ModelState.AddModelError("","Bilinmeyen bir hata oldu");
            return View(model);
        }
        
         public async Task<IActionResult> Logout(){
            await _signInManager.SignOutAsync();
            TempData.Put("message",new AlertMessage(){
                Title="Oturum kapatildi",
                Message = "Hesabiniz guvenli bir sekilde kapatildi.",
                AlertType = "warning"
            });
            return Redirect("~/");
         }
         public async Task<IActionResult> ConfirmEmail(string userId,string token){
            if(userId==null || token==null){
                TempData.Put("message",new AlertMessage(){
                    Title="Gecersiz token",
                    Message = "Gecersiz token",
                    AlertType = "danger"
                });
                return View();
            }
            var user = await _userManager.FindByIdAsync(userId);
            if(user!=null){
                var result = await _userManager.ConfirmEmailAsync(user,token);
                if(result.Succeeded){
                    TempData.Put("message",new AlertMessage(){
                        Title="Hesabiniz onaylandi",
                        Message = "Hesabiniz onaylandi",
                        AlertType = "success"
                });
                    return View();
                    }
            }
             TempData.Put("message",new AlertMessage(){
                    Title="Hesabiniz onaylanmadi",
                    Message = "Hesabiniz onaylanmadi",
                    AlertType = "warning"
                });
            return View();
          }
         public IActionResult ForgotPassword(){
             return View();
         }
         [HttpPost]
         public async Task<IActionResult> ForgotPassword(string Email){
                if(string.IsNullOrEmpty(Email)){
                    return View();
                }
                var user = await _userManager.FindByEmailAsync(Email);

                if(user==null){
                    return View();
                }
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);

                var url = Url.Action("ResetPassword","Account",new{
                    userId = user.Id,
                    token = code
                });
                await _emailSender.SendEmailAsync(Email,"Reset Password",$"Parolazini yenilemek icin linke <a href='https://localhost:44325{url}'>tiklayiniz</a>");
                return View();
         }
          public IActionResult ResetPassword(string userId,string token){
            if(userId==null || token==null){
                return RedirectToAction("Home","Index");
            }
            var model = new ResetPasswordModel {Token = token};
            return View();
         }
         [HttpPost]
         public async Task<IActionResult> ResetPassword(ResetPasswordModel model){
             if(!ModelState.IsValid){
                 return View(model);
             }
             var user = await _userManager.FindByEmailAsync(model.Email);
             if(user==null){
                 return RedirectToAction("Home","Index");
             }
             var result = await _userManager.ResetPasswordAsync(user,model.Token,model.Password);

             if(result.Succeeded){
                 return RedirectToAction("Login","Account");
             }
             

             return View(model);
         }
    }
}