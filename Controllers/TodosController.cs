using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Database;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodosController : ControllerBase
    {
        private readonly TodoApiContext _context;

        public TodosController(TodoApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<TodoItem>> GetTodoItems()
        {
            var result = await _context.TodoItems.ToListAsync();
            return Ok(new { items = result });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(int id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null) return NotFound();
            return Ok(new { todoItem = todoItem });
        }

        [HttpPost]
        public async Task<IActionResult> PostTodoItem([FromForm] TodoItem todoItem)
        {
            //_context.TodoItems.Add(todoItem);
            // await _context.SaveChangesAsync();

            if (ModelState.IsValid)
            {
                await _context.TodoItems.AddAsync(todoItem);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);//v1
                // return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);//v2
            }
            return new JsonResult("Something went wrong") { StatusCode = 500 };
        }

        [HttpPut]
        public async Task<IActionResult> PutTodoItem(int id, [FromForm] TodoItem todoItem)
        {
            var existItem = await _context.TodoItems.FindAsync(id);
            if (existItem == null) return BadRequest();

            existItem.Title = todoItem.Title;
            existItem.Description = todoItem.Description;

            //implement the changes on the database level
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTodoItem(int id)
        {
            var item = await _context.TodoItems.FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) NotFound();

            _context.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}