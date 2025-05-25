using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Data.DataModels.Dto.TodoDtos;
using TodoApp.Data.DataModels.Entities;
using TodoApp.Data.Services;
using TodoApp.Data.Services.Interfaces;

namespace TodoApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController(ITodoService todoService, IMapper mapper) : ControllerBase
    {
        private int maxPageSize = 20;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoReturnDto>>> GetAllTodosAsync
            (bool? isCompleted, DateTime? createdAt, DateTime? completedAt, string? searchQuery,
                string? sort, int pageNumber = 1, int pageSize = 10)
        {
            if (pageSize > maxPageSize)
            {
                pageSize = maxPageSize;
            }
            var (todos, paginationMetaData) = await todoService
                .GetAllTodosAsync(isCompleted, createdAt, completedAt, searchQuery, sort, pageNumber, pageSize);

            Response.Headers.Append("X-Pagination", System.Text.Json.JsonSerializer.Serialize(paginationMetaData));
            return Ok(mapper.Map<ICollection<TodoReturnDto>>(todos));
        }

        [HttpGet("{id:int}", Name = "GetTodo")]
        public async Task<ActionResult<TodoReturnDto>> GetTodoByIdAsync(int id)
        {
            TodoEntity? todo = await todoService.GetTodoByIdAsync(id);
            if (todo == null)
            {
                return NotFound("Todo with the provided id was not found");
            }
            return Ok(mapper.Map<TodoReturnDto>(todo));
        }

        [HttpPost]
        public async Task<ActionResult<TodoReturnDto>> AddTodoAsync(TodoAddDto todo)
        {
            if (todo == null)
            {
                return BadRequest("Todo cannot be null");
            }

            TodoEntity todoEntity = mapper.Map<TodoEntity>(todo);

            await todoService.AddTodoAsync(todoEntity);
            await todoService.SaveChangesAsync();

            TodoReturnDto todoToReturn = mapper.Map<TodoReturnDto>(todoEntity);
            return CreatedAtRoute("GetTodo", new
            {
                id = todoToReturn.Id
            }, todoToReturn);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<bool>> DeleteTodoAsync(int id)
        {
            TodoEntity? todo = await todoService.GetTodoByIdAsync(id);
            if (todo == null)
            {
                return NotFound("Todo with the provided id was not found");
            }

            await todoService.DeleteTodoAsync(id);
            return Ok(await todoService.SaveChangesAsync());
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<TodoReturnDto>> UpdateTodoAsync(int id, TodoToUpdateDto todo)
        {
            if (todo == null)
            {
                return BadRequest("Todo cannot be null");
            }
            TodoEntity? todoEntity = await todoService.GetTodoByIdAsync(id);
            if (todoEntity == null)
            {
                return NotFound("Todo with the provided id was not found");
            }

            mapper.Map(todo, todoEntity);
            if (await todoService.SaveChangesAsync())
            {
                return Ok(mapper.Map<TodoReturnDto>(todoEntity));
            }
            return BadRequest("Failed to update todo");
        }
    }
}
