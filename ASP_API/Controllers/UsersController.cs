using ASP_API.Data;
using ASP_API.Data.Entities;
using ASP_API.Data.Entities.Identity;
using ASP_API.Helpers;
using ASP_API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using static ASP_API.Models.UserViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ASP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppEFContext _appEFContext;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly UserManager<UserEntity> _userManager;
        public UsersController(AppEFContext appEFContext, IConfiguration configuration, IMapper mapper, UserManager<UserEntity> userManager)
        {
            _appEFContext = appEFContext;
            _configuration = configuration;
            _mapper = mapper;
            _userManager = userManager;
        }
        // GET: api/<UsersController>
        [HttpGet("list")]
        public async Task<IActionResult> Get()
        {
            var users = await _appEFContext.Users.ToListAsync();
            var result = users.Select(x => _mapper.Map<UserItemViewModel>(x)).ToList();
            for(int i = 0; i < result.Count; i++)
            {
                result[i].Role = _userManager.GetRolesAsync(users[i]).Result.FirstOrDefault();
            }

            return Ok(result);
        }
        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _appEFContext.Users.SingleOrDefaultAsync(x => x.Id == id);
            //Console.WriteLine(user.UserRoles.FirstOrDefault().Role.Name);
            var result = _mapper.Map<UserItemViewModel>(user);
            result.Role = _userManager.GetRolesAsync(user).Result.FirstOrDefault();

  
            return Ok(result);
    
        }

        // POST api/<UsersController>
        [HttpPost("create")]
        public async Task<IActionResult> Post([FromForm] CreateUserViewModel model)
        {
            String imageName = string.Empty;
            if (model.Image != null)
            {
                var fileExp = Path.GetExtension(model.Image.FileName);
                var dirSave = Path.Combine(Directory.GetCurrentDirectory(), "images");
                imageName = Path.GetRandomFileName() + fileExp;
                //using (var steam = System.IO.File.Create(Path.Combine(dirSave, imageName)))
                //{
                //    await model.Image.CopyToAsync(steam);
                //}
                using (var ms = new MemoryStream())
                {
                    await model.Image.CopyToAsync(ms);
                    var bmp = new Bitmap(Image.FromStream(ms));
                    string[] sizes = ((string)_configuration.GetValue<string>("ImageSizes")).Split(" ");
                    foreach (var s in sizes)
                    {
                        int size = Convert.ToInt32(s);
                        var saveImage = ImageWorker.CompressImage(bmp, size, size, false);
                        saveImage.Save(Path.Combine(dirSave, s + "_" + imageName));
                    }
                }
            }
            var user = new UserEntity()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Image = imageName,
                UserName = model.Email
            };
          
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded && model.Role != null && model.Role != "None")
            {
                result = await _userManager.AddToRoleAsync(user, model.Role);
                return Ok();
            }
            return BadRequest();
        }

        // PUT api/<UsersController>/5
        [HttpPut("edit")]
        public async Task<IActionResult> Put([FromForm] EditUserViewModel model)
        {
            var user = await _appEFContext.Users.SingleOrDefaultAsync(x => x.Id == model.Id);
            if (user == null)
                return NotFound();

            String imageNewName = string.Empty;
            if (model.Image != null)
            {
                var imageOld = user.Image;  //старе фото
                var fileExp = Path.GetExtension(model.Image.FileName);
                var dirSave = Path.Combine(Directory.GetCurrentDirectory(), "images");
                imageNewName = Path.GetRandomFileName() + fileExp;
                using (var ms = new MemoryStream())
                {
                    await model.Image.CopyToAsync(ms);
                    var bmp = new Bitmap(Image.FromStream(ms));
                    string[] sizes = ((string)_configuration.GetValue<string>("ImageSizes")).Split(" ");
                    foreach (var s in sizes)
                    {
                        int size = Convert.ToInt32(s);
                        var saveImage = ImageWorker.CompressImage(bmp, size, size, false);
                        saveImage.Save(Path.Combine(dirSave, s + "_" + imageNewName));
                        //видаляю старі фото
                        var imgDelete = Path.Combine(dirSave, s + "_" + imageOld);
                        if (System.IO.File.Exists(imgDelete))
                        {
                            System.IO.File.Delete(imgDelete);
                        }
                    }
                }
            }
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.Image = string.IsNullOrEmpty(imageNewName) ? user.Image : imageNewName;
          
            var role = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
            var remove = await _userManager.RemoveFromRoleAsync(user, role);
            if(remove.Succeeded)
            {
                var result = await _userManager.AddToRoleAsync(user, model.Role);
                if(result.Succeeded)
                {
                    await _appEFContext.SaveChangesAsync();
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return BadRequest();
            }
           

            return Ok(user);
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _appEFContext.Users.SingleOrDefaultAsync(x => x.Id == id);
            if (user == null)
                return NotFound();

            var dirSave = Path.Combine(Directory.GetCurrentDirectory(), "images");
            string[] sizes = ((string)_configuration.GetValue<string>("ImageSizes")).Split(" ");
            foreach (var s in sizes)
            {
                var imgDelete = Path.Combine(dirSave, s + "_" + user.Image);
                if (System.IO.File.Exists(imgDelete))
                {
                    System.IO.File.Delete(imgDelete);
                }
            }
            _appEFContext.Users.Remove(user);
            await _appEFContext.SaveChangesAsync();
            return Ok();
        }
    }
}
