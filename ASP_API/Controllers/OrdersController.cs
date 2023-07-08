using ASP_API.Data;
using ASP_API.Data.Entities;
using ASP_API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using static ASP_API.Models.UserViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ASP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly AppEFContext _appEFContext;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public OrdersController(AppEFContext appEFContext, IConfiguration configuration, IMapper mapper)
        {
            _appEFContext = appEFContext;
            _configuration = configuration;
            _mapper = mapper;
        }

        // GET: api/<OrdersController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var orders = await _appEFContext.Orders
                .Include(x => x.DeliveryInfo)
                .Include(x => x.Products)
                .ThenInclude(x => x.Product)
                .Include(x => x.User)
                .ToListAsync();



            var result = orders.Select(x => new OrderViewItemModel
            {
                Id = x.Id,
                Status = x.Status,
                DataCreated = x.DataCreated,
                Cost = x.Cost,
                DiscountCost = x.DiscountCost,
                UserId = x.UserId,
                User = _mapper.Map<UserItemViewModel>(x.User),
                Payment_Type = x.Payment_Type,
                Payment_Status = x.Payment_Status,
                Recipient_Full_Name = x.Recipient_Full_Name,
                Recipient_Phone = x.Recipient_Phone,
                DeliveryInfoId = x.DeliveryInfoId,
                DeliveryInfo = x.DeliveryInfo,
                
                Products = x.Products.Select(y => new ProductOrderViewModel
                {
                    Id = y.Id,
                    Product = _mapper.Map<ProductItemViewModel>(y.Product),
                    Quantity = y.Quantity
                }).ToList()
            }).ToList();

            return Ok(result);
        }

        // GET api/<OrdersController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _appEFContext.Orders.Where(x => x.Id == id).SingleOrDefaultAsync();

            if (result != null)
                return Ok(result);
            else
                return NotFound();
        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetByUserId(int id)
        {
            var result = await _appEFContext.Orders.Where(x => x.UserId == id).ToListAsync();

            if (result.Count > 0)
                return Ok(result);
            else
                return NotFound();
        }

        // POST api/<OrdersController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OrderCreateItemModel model)
        {
            if(model.Products.Count > 0)
            {
                var products = new List<ProductEntity>();
                double cost = 0;

                var delInfo = new DeliveryInfoEntity
                {
                   DeliveryTypeId = model.DeliveryTypeId,
                   Oblast = model.Oblast,
                   Rayon = model.Rayon,
                   City = model.City,
                   Street = model.Street,
                   NumberOfHouse = model.NumberOfHouse
                };

                await _appEFContext.AddAsync(delInfo);
                await _appEFContext.SaveChangesAsync();

                foreach (var productTmp in model.Products)
                {
                    var product = await _appEFContext.Products.Where(x => x.Id == productTmp.ProductId).SingleOrDefaultAsync();
                    products.Add(product);
                    if (product.DiscountPrice != null)
                        cost += Convert.ToDouble(product.DiscountPrice) * productTmp.Quantity;
                    else
                        cost += product.Price * productTmp.Quantity;

                    
                }

                var order = new OrderEntity
                {
                    Status = "Створено",
                    DataCreated = DateTime.Now,
                    Cost = cost,
                    DiscountCost = null,
                    UserId = model.UserId,
                    Payment_Type = model.Payment_Type,
                    Payment_Status = false,
                    Recipient_Full_Name = model.Recipient_Full_Name,
                    Recipient_Phone = model.Recipient_Phone,
                    DeliveryInfoId = delInfo.Id 
                };
                await _appEFContext.AddAsync(order);
                await _appEFContext.SaveChangesAsync();

                foreach (var productTmp in model.Products)
                {

                    var prOrder = new ProductOrderEntity
                    {
                        ProductId = productTmp.ProductId,
                        Quantity = productTmp.Quantity,
                        OrderId = order.Id,
                       
                    };
                    await _appEFContext.AddAsync(prOrder);
                    await _appEFContext.SaveChangesAsync();
                }

                

                return Ok(order);
            }

            return BadRequest("Немає продуктів!");
        }

        // PUT api/<OrdersController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] string value)
        {
            return Ok();
        }

        // DELETE api/<OrdersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
