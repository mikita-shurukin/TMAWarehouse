using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebMvc.DAL;
using WebMvc.DAL.Models;
using WebMvc.Models;
using WebMvc.Models.Requests;
using WebMvc.Models.Responses;

namespace WebMvc.ApiControllers;
[Route("api/[controller]")]
[ApiController]
public class WarehouseController : Controller
{
    private readonly MainDbContext _dbContext;
    private readonly IMapper _mapper;

    public WarehouseController(MainDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    [HttpGet("Items/{warehouseId}")]
    public async Task<IActionResult> GetItems(int warehouseId, string sortOrder = "")
    {
        ViewBag.WarehouseId = warehouseId;

        var query = _dbContext.InventoryItems
            .AsNoTracking()
            .Where(x => x.WarehouseId == warehouseId)
            .Include(x => x.Item)                      
            .ThenInclude(x => x.Group)
            .Include(x => x.Warehouse)
            .Select(x => new WarehouseItemResponse
            {
                ItemId = x.Item.Id,
                GroupId = x.Item.GroupId,
                GroupName = x.Item.Group.Name,
                Price = x.Item.Price,
                UnitOfMeasurement = x.UnitOfMeasurement,
                Quantity = x.Quantity,
                StorageLocation = x.Warehouse.StorageLocation,
                ContactPerson = x.Warehouse.ContactPerson,
                PhotoUrl = x.Item.PhotoUrl
            });


        if (!string.IsNullOrEmpty(sortOrder))
        {
            query = sortOrder switch
            {
                "asc" => query.OrderBy(x => x.Price),
                "desc" => query.OrderByDescending(x => x.Price),
                _ => query 
            };
        }

        var items = await query.ToListAsync();

        return View(items);
    }


    [HttpGet("AddItems")]
    public IActionResult AddItems()
    {
        return View();
    }

    [HttpPost("AddItems")]
    public async Task<IActionResult> AddItems(AddItemsRequest request)
    {

        if (!await _dbContext.Warehouses.AsNoTracking().AnyAsync(x => x.Id == request.WarehouseId))
            return NotFound($"Warehouse with id={request.WarehouseId} was not found");

        var itemIds = request.ItemPositions.Select(x => x.ItemId).Distinct().ToList();

        if (await _dbContext.Items.AsNoTracking().Where(x => itemIds.Contains(x.Id)).CountAsync() != itemIds.Count)
            return NotFound($"Items with some id from request was not found");

        if (request.ItemPositions.Any(x => x.Quantity <= 0))
            return BadRequest("Quantity has to be set more than zero");


        var itemPositions = request.ItemPositions.GroupBy(x => x.ItemId).Select(x => new ItemPosition
        {
            UnitOfMeasurement = x.First().UnitOfMeasurement,
            ItemId = x.Key,
            Quantity = x.Select(a => a.Quantity).Aggregate((a, b) => a + b)
        });

        foreach (var itemPosition in itemPositions)
        {
            var existingInventoryItem = await _dbContext.InventoryItems
                .AsNoTracking()
                .Where(x => x.WarehouseId == request.WarehouseId)
                .FirstOrDefaultAsync(x => x.ItemId == itemPosition.ItemId);

            if (existingInventoryItem != null)
            {
                existingInventoryItem.Quantity += itemPosition.Quantity;
            }
            else
            {
                _dbContext.InventoryItems.Add(new InventoryItem
                {
                    ItemId = itemPosition.ItemId,
                    Quantity = itemPosition.Quantity,
                    UnitOfMeasurement = itemPosition.UnitOfMeasurement,
                    WarehouseId = request.WarehouseId
                });
            }
        }

        await _dbContext.SaveChangesAsync();

        return Ok();
    }



    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _dbContext.Warehouses.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var warehouse = await _dbContext.Warehouses.AsNoTracking().ToListAsync();
        return View("GetAll",warehouse);
    }

    [HttpGet("Create")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromForm]CreateWarehouseRequest request)
    {

        var warehouseMap = _mapper.Map<Warehouse>(request);
        _dbContext.Warehouses.Add(warehouseMap);

        await _dbContext.SaveChangesAsync();

        return Ok();
    }

    [HttpPatch]
    public async Task<IActionResult> Update([FromBody] UpdateWarehouseRequest request)
    {
        //TODO: Add validation
        var warehouse = _mapper.Map<Warehouse>(request);
        _dbContext.Warehouses.Update(warehouse);

        await _dbContext.SaveChangesAsync();

        return Ok();
    }



    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        _dbContext.Warehouses.Remove(new Warehouse { Id = id });

        await _dbContext.SaveChangesAsync();

        return Ok();
    }
}