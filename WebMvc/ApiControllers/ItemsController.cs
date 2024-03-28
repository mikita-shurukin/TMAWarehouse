using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebMvc.DAL;
using WebMvc.DAL.Models;
using WebMvc.Models.Requests;
using WebMvc.Models.Requests.ItemGroup;

namespace WebMvc.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : Controller
    {
        private readonly MainDbContext _dbContext;
        private readonly IMapper _mapper;

        public ItemsController(MainDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _dbContext.Items.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var items = await _dbContext.Items.AsNoTracking().ToListAsync();
            return View("Get", items);
        }




        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromForm] CreateItemRequest request)
        {

            var item = _mapper.Map<Item>(request);
            _dbContext.Items.Add(item);

            await _dbContext.SaveChangesAsync();

            return Ok();
        }



        [HttpPatch]
        public async Task<IActionResult> Update([FromBody] UpdateItemRequest request)
        {

            var item = _mapper.Map<Item>(request);
            _dbContext.Items.Update(item);

            await _dbContext.SaveChangesAsync();

            return Ok();
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var itemToDelete = await _dbContext.Items.FindAsync(id);

            if (itemToDelete == null)
            {
                return NotFound();
            }

            _dbContext.Items.Remove(itemToDelete); 

            await _dbContext.SaveChangesAsync(); 

            return NoContent(); 
        }

    }
}
