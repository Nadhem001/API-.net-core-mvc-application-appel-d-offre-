using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using ang_net.Models;
using ang_net.ModelViews;
using ang_net.services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SendGrid.Helpers.Mail;

namespace ang_net.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CrosPolicy")]
    public class AccountController : ControllerBase
    {
        private readonly ang_netContext _db;
        private readonly UserManager<applicationUser> _manager;
        private readonly SignInManager<applicationUser> _signInManager;

        public AccountController(ang_netContext db, UserManager<applicationUser> manager,
            SignInManager<applicationUser> signInManager)
        {
            _db = db;
            _manager = manager;
            _signInManager = signInManager;
        }
        [HttpGet]
        [Route("GetUse")]
        public async Task<ActionResult<IEnumerable<applicationUser>>> GetUse()
        {
            
            
            return await _db.Users.ToListAsync();

            // return BadRequest("1");
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegistreModel model)
        {
            if (model == null)
            {
                return NotFound();
            }
            if(ModelState.IsValid)
            {
                if (EmailExistes(model.Email))
                {
                    return BadRequest("Email is used");
                }
                if (!IsEmailValid(model.Email))
                    {
                    return BadRequest("Email is not valid");
                }
                if (userNameExistes(model.UserName))
                {
                    return BadRequest("userName is used");
                }
                var user = new applicationUser
                {
                    Email = model.Email,
                    UserName=model.UserName
                };


                var result = await _manager.CreateAsync(user,model.Password);
               if (result.Succeeded)
                {
                    //localhost:49689/api/account/RegistrationConfirm?ID=545435&Token=541298875465
                    var token = await _manager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmLink = Url.Action("RegistrationConfirm","Account",new {ID=user.Id,Token=HttpUtility.UrlEncode(token)}, Request.Scheme);
                    var txt = "please confirm your Registration at our site ";
                    var link = "<a href=\""+confirmLink+"\" >Confirm registration </a> ";
                    var title = "Registration Confirm";
                    if(await SendGridAPI.Execute(user.Email,user.UserName,txt,link,title))
                    {
                        return Ok("regestrition complite");
                    }
                    return Ok(confirmLink);
                }
                else
                {
                    return BadRequest(result.Errors);
                }
                
            }
            return StatusCode(StatusCodes.Status400BadRequest);

        }

        private bool userNameExistes(string userName)
        {
            return _db.Users.Any(x => x.UserName == userName);
        }

        private bool EmailExistes(string email)
        {
            return _db.Users.Any(x => x.Email == email);
        }
        private bool IsEmailValid(string email)
        {
            Regex em = new Regex(@"\b[\w\.-]+@[\w\.-]+\.\w{2,4}\b");
            if(em.IsMatch(email))
            {
                return true;
            }
            return false;
        }
               [HttpGet]
               [Route("RegistrationConfirm")]
            public async Task <IActionResult> RegistrationConfirm (string ID,String Token)
            {
                if(string.IsNullOrEmpty(ID)|| string.IsNullOrEmpty(Token)) 
                return NotFound("1");

                var user = await _manager.FindByIdAsync(ID);
                if (user == null)
                    return NotFound("2");

                var result = await _manager.ConfirmEmailAsync(user, HttpUtility.UrlDecode(Token));
                if (result.Succeeded)
                {
                    return Ok("Registration Success");

                }
                else
                {
                    return BadRequest(result.Errors);

                }
            }

        /**********************************************************/

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (model == null)
                return NotFound();

            var user = await _manager.FindByEmailAsync(model.Email);
            if (user == null)
                return NotFound("2");
            if(!user.EmailConfirmed)
            {
                return Unauthorized("email is not confirmed yet !!!");
            }
            var result = await _signInManager.PasswordSignInAsync(user,model.Password,model.RememberMe,true);
            if (result.Succeeded)
            {
                return Ok("Login Success");

            }
            else if(result.IsLockedOut)
            {
                return Unauthorized("user account is locked");

            }
            return StatusCode(StatusCodes.Status204NoContent);
        }

    }
}