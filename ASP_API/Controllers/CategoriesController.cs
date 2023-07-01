using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using ASP_API.Data;
using ASP_API.Data.Entities;
using ASP_API.Helpers;
using ASP_API.Models;
using AutoMapper;


namespace ASP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly AppEFContext _appEFContext;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public CategoriesController(AppEFContext appEFContext, IConfiguration configuration, IMapper mapper)
        {
            _appEFContext = appEFContext;
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpGet("list")]
        public async Task<IActionResult> List()
        {
            var result = await _appEFContext.Categories
                 .Select(x => _mapper.Map<CategoryItemViewModel>(x))
                 .ToListAsync();

            return Ok(result);
        }
        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _appEFContext.Categories
                 .Where(s => s.Id == id)
                 .Select(x => _mapper.Map<CategoryItemViewModel>(x))
                 .ToListAsync();

            if (result.Count > 0)
            {
                return Ok(result[0]);
            }
            else
            { return NotFound(); }
        }
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] CategoryCreateViewModel model)
        {
            String imageName = string.Empty;
            if(model.Image != null)
            {
                var fileExp = Path.GetExtension(model.Image.FileName);
                var dirSave = Path.Combine(Directory.GetCurrentDirectory(), "images");
                imageName = Path.GetRandomFileName()+fileExp;
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
                        var saveImage = ImageWorker.CompressImage(bmp, size, size, false, true);
                        saveImage.Save(Path.Combine(dirSave, s + "_" + imageName));
                    }
                }
            }

            var category = _mapper.Map<CategoryEntity>(model);
            category.Image = imageName;
            await _appEFContext.AddAsync(category);
            await _appEFContext.SaveChangesAsync();
            return Ok(category);
        }

        [HttpPut("edit")]
        public async Task<IActionResult> Edit([FromForm] CategoryEditViewModel model)
        {
            var category = await _appEFContext.Categories.SingleOrDefaultAsync(x => x.Id == model.Id);
            if (category == null)
                return NotFound();

            String imageNewName = string.Empty;
            if (model.Image != null)
            {
                var imageOld = category.Image;  //старе фото
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
            category.Name = model.Name;
            category.Priority = model.Priority;
            category.Description = model.Description;
            category.Status = model.Status;
            category.ParentId = model.ParentId == 0 ? null : model.ParentId;
            category.Image = string.IsNullOrEmpty(imageNewName) ? category.Image : imageNewName;
            await _appEFContext.SaveChangesAsync();
            return Ok(category);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            var category = await _appEFContext.Categories.SingleOrDefaultAsync(x => x.Id == id);
            if (category == null)
                return NotFound();

            var dirSave = Path.Combine(Directory.GetCurrentDirectory(), "images");
            string[] sizes = ((string)_configuration.GetValue<string>("ImageSizes")).Split(" ");
            foreach (var s in sizes)
            {
                var imgDelete = Path.Combine(dirSave, s + "_" + category.Image);
                if (System.IO.File.Exists(imgDelete))
                {
                    System.IO.File.Delete(imgDelete);
                }
            }
            _appEFContext.Categories.Remove(category);
            await _appEFContext.SaveChangesAsync();
            return Ok();
        }
    }
}
