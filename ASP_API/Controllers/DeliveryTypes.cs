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
    public class DeliveryTypesController : ControllerBase
    {
        private readonly AppEFContext _appEFContext;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public DeliveryTypesController(AppEFContext appEFContext, IConfiguration configuration, IMapper mapper)
        {
            _appEFContext = appEFContext;
            _configuration = configuration;
            _mapper = mapper;
        }

        // GET: api/<OrdersController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _appEFContext.DeliveryTypes.ToListAsync();
   
            return Ok(result);
        }

        // GET api/<OrdersController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _appEFContext.DeliveryTypes.Where(x => x.Id == id).SingleOrDefaultAsync();

            if (result != null)
                return Ok(result);
            else
                return NotFound();
        }

        // POST api/<OrdersController>
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] DeliveryTypeCreateItemModel model)
        {
            var type = _mapper.Map<DeliveryTypeEntity>(model);
            await _appEFContext.AddAsync(type);
            await _appEFContext.SaveChangesAsync();
            return Ok(type);
        }

        // PUT api/<OrdersController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromForm] DeliveryTypeEntity model)
        {
            var delType = await _appEFContext.DeliveryTypes.SingleOrDefaultAsync(x => x.Id == model.Id);
            if (delType == null)
                return NotFound();

            delType.Title = model.Title;
            delType.Description = model.Description;
            await _appEFContext.SaveChangesAsync();
            return Ok(delType);
        }

        // DELETE api/<OrdersController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var delType = await _appEFContext.DeliveryTypes.SingleOrDefaultAsync(x => x.Id == id);
            if (delType == null)
                return NotFound();

            _appEFContext.DeliveryTypes.Remove(delType);
            await _appEFContext.SaveChangesAsync();
            return Ok();
        }
    }
}
