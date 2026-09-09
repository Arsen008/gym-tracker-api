using Microsoft.AspNetCore.Mvc;
using SachkovTech.Domain;
using SachkovTech.Infrastructure;

namespace SachkovTech.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ToDoItemsController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public ToDoItemsController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var items = _dbContext.ToDoItems.ToList();
        return Ok(items);
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetById([FromRoute] Guid id)
    {
        var item = _dbContext.ToDoItems.FirstOrDefault(x => x.Id == ToDoItemId.Create(id));
        if (item == null)
        {
            return NotFound($"Task with id {id} was not found.");
        }

        return Ok(item);
    }

    [HttpPost]
    public IActionResult Create([FromBody] string title)
    {
        var result = ToDoItem.Create(title);
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        var newItem = result.Value;
        _dbContext.ToDoItems.Add(newItem);
        _dbContext.SaveChanges();

        return Ok(newItem);
    }

    [HttpPut("{id:guid}")]
    public IActionResult Update([FromRoute] Guid id, [FromBody] string newTitle)
    {
        var item = _dbContext.ToDoItems.FirstOrDefault(x => x.Id == ToDoItemId.Create(id));
        if (item == null)
        {
            return NotFound($"Task with id {id} was not found.");
        }

    
        var result = item.UpdateTitle(newTitle);
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        _dbContext.SaveChanges();
        return Ok(item);
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete([FromRoute] Guid id)
    {
        var item = _dbContext.ToDoItems.FirstOrDefault(x => x.Id == ToDoItemId.Create(id));
        if (item == null)
        {
            return NotFound($"Task with id {id} was not found.");
        }

        _dbContext.ToDoItems.Remove(item);
        _dbContext.SaveChanges();

        return NoContent();  
    }
}