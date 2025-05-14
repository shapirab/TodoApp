using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Data.DataModels.Entities;

namespace TodoApp.Data.Services.Interfaces
{
    public interface ITodoService
    {
        Task<ICollection<TodoEntity>> GetAllTodosAsync();
        Task<(ICollection<TodoEntity>, PaginationMetaData paginationMetaData)> GetAllTodosAsync
            (bool? isCompleted, DateTime? createdAt, DateTime? completedAt, 
                string? searchQuery, string? sort, int pageNumber, int pageSize);
        Task<TodoEntity?> GetTodoByIdAsync(int id);
        Task AddTodoAsync(TodoEntity entity);
        Task DeleteTodoAsync(int id);
        Task<bool> SaveChangesAsync();
    }
}
