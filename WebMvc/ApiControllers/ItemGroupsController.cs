using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebMvc.DAL.Models;
using WebMvc.DAL;
using WebMvc.Models.Requests;
using AutoMapper;
using WebMvc.Models.Requests.ItemGroup;

namespace WebMvc.ApiControllers;
[Route("api/[controller]")]
[ApiController]
public class ItemGroupsController : Controller
{
    private readonly MainDbContext _dbContext;
    private readonly IMapper _mapper;

    public ItemGroupsController(MainDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _dbContext.ItemGroups.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var itemGroup = await _dbContext.ItemGroups.AsNoTracking().ToListAsync();
        return View(itemGroup);
    }

    [HttpGet("Create")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromForm] CreateItemGroupRequest request)
    {
        var newItemGroup = _mapper.Map<ItemGroup>(request);
        _dbContext.ItemGroups.Add(newItemGroup);

        await _dbContext.SaveChangesAsync();

        return Ok();
    }

    [HttpPatch("Update")]
    public async Task<IActionResult> Update([FromForm]UpdateGroupRequest request)
    {
        var updatedItemGroup = _mapper.Map<ItemGroup>(request);


        _dbContext.ItemGroups.Update(updatedItemGroup);

        await _dbContext.SaveChangesAsync();

        return View();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        _dbContext.ItemGroups.Remove(new ItemGroup { Id = id });

        await _dbContext.SaveChangesAsync();

        return Ok();
    }
}