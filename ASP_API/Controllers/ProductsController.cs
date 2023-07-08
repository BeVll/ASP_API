using ASP_API.Data;
using ASP_API.Data.Entities;
using ASP_API.Helpers;
using ASP_API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ASP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppEFContext _appContext;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public ProductsController(AppEFContext appEFContext, IConfiguration configuration, IMapper mapper)
        {
            _appContext = appEFContext;
            _configuration = configuration;
            _mapper = mapper;

        }

        // GET: api/<ProductsController>
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var model = await _appContext.Products
                .Include(x => x.Category)
                .Include(x => x.ProductImages)
                .Select(x => new ProductItemViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Price = x.Price,
                    DiscountPrice = x.DiscountPrice,
                    Status = x.Status,
                    CategoryId = x.CategoryId,
                    CategoryName = x.Category.Name,
                    Images =
                        x.ProductImages
                        .Select(x =>
                            new ProductImageItemViewModel { Id = x.Id, Path = x.Path })
                        .ToList(),
                })
                .ToListAsync();
            return Ok(model);
        }


        // GET api/<ProductsController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var model = await _appContext.Products
                .Include(x => x.Category)
                .Include(x => x.ProductImages)
                .Select(x => new ProductItemViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Price = x.Price,
                    DiscountPrice = x.DiscountPrice,
                    Status = x.Status,
                    CategoryId = x.CategoryId,
                    CategoryName = x.Category.Name,
                    Images =
                        x.ProductImages
                        .Select(x =>
                            new ProductImageItemViewModel { Id = x.Id, Path = x.Path })
                        .ToList(),
                })
                .Where(s => s.Id == id)
                .FirstOrDefaultAsync();
            return Ok(model);
        }

        // POST api/<ProductsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] ProductCreateViewModel model)
        {
            if (model.Name != null)
            {
                var product = new ProductEntity
                {
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    DiscountPrice = model.DiscountPrice,
                    DateCreated = DateTime.UtcNow,
                    CategoryId = model.CategoryId,
                    Priority = model.Priority,
                    Status = model.Status

                };
                _appContext.Add(product);
                _appContext.SaveChanges();
                foreach (var idImg in model.ImagesID)
                {
                    var image = await _appContext.ProductImages.SingleOrDefaultAsync(x => x.Id == idImg);
                    image.ProductId = product.Id;
                }
                _appContext.SaveChanges();
                return Ok(product);
            }
            return BadRequest(404);
        }

        [HttpPost("uploadImage")]
        public async Task<IActionResult> UploadImage([FromForm] ProductUploadImageViewModel model)
        {

            string imageName = string.Empty;
            if (model.Image != null)
            {
                var fileExp = Path.GetExtension(model.Image.FileName);
                var dirSave = Path.Combine(Directory.GetCurrentDirectory(), "images");
                imageName = Path.GetRandomFileName() + fileExp;
                using (var ms = new MemoryStream())
                {
                    await model.Image.CopyToAsync(ms);
                    var bmp = new Bitmap(System.Drawing.Image.FromStream(ms));
                    string[] sizes = ((string)_configuration.GetValue<string>("ImageSizes")).Split(" ");
                    foreach (var s in sizes)
                    {
                        int size = Convert.ToInt32(s);
                        var saveImage = ImageWorker.CompressImage(bmp, size, size, false);
                        saveImage.Save(Path.Combine(dirSave, s + "_" + imageName));
                    }
                }
                var entity = new ImageProduct();
                entity.Path = imageName;
                entity.DateCreated = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                _appContext.ProductImages.Add(entity);
                _appContext.SaveChanges();
                return Ok(_mapper.Map<ProductImageItemViewModel>(entity));

            }
            return BadRequest();
        }
        // PUT api/<ProductsController>/5
        [HttpPut("edit")]
        public async Task<IActionResult> Put([FromForm] ProductEditViewModel model)
        {
            //var product = await _appContext.Products.SingleOrDefaultAsync(x => x.Id == model.Id);
            //if (product == null)
            //    return NotFound();

            //product.Name = model.Name;
            //product.Priority = model.Priority;
            //product.Description = model.Description;
            //product.Status = model.Status;
            //product.Price = model.Price;
            //product.DiscountPrice = model.DiscountPrice;
            //product.CategoryId = model.CategoryId;


            //await _appContext.SaveChangesAsync();
            //return Ok(product);
            return BadRequest("Not working!");
        }

        // DELETE api/<ProductsController>/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _appContext.Products.Include(x => x.ProductImages).SingleOrDefaultAsync(x => x.Id == id);
            if (product == null)
                return NotFound();

            foreach(var img in product.ProductImages)
            {
                var image = await _appContext.ProductImages.SingleOrDefaultAsync(x => x.Id == img.Id);
                if (image == null)
                    return NotFound();

                var dirSave = Path.Combine("images");
                string[] sizes = ((string)_configuration.GetValue<string>("ImageSizes")).Split(" ");
                foreach (var s in sizes)
                {
                    var imgDelete = Path.Combine(dirSave, s + "_" + image.Path);
                    if (System.IO.File.Exists(imgDelete))
                    {
                        System.IO.File.Delete(imgDelete);
                    }
                }
                _appContext.ProductImages.Remove(image);
                await _appContext.SaveChangesAsync();
            }

            _appContext.Products.Remove(product);
            await _appContext.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("deleteImage/{id}")]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var image = await _appContext.ProductImages.SingleOrDefaultAsync(x => x.Id == id);
            if (image == null)
                return NotFound();

            var dirSave = Path.Combine("images");
            string[] sizes = ((string)_configuration.GetValue<string>("ImageSizes")).Split(" ");
            foreach (var s in sizes)
            {
                var imgDelete = Path.Combine(dirSave, s + "_" + image.Path);
                if (System.IO.File.Exists(imgDelete))
                {
                    System.IO.File.Delete(imgDelete);
                }
            }
            _appContext.ProductImages.Remove(image);
            await _appContext.SaveChangesAsync();
            return Ok();
        }

    }

}
