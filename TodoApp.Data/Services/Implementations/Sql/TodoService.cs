using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Data.DataModels.Entities;
using TodoApp.Data.DbContexts;
using TodoApp.Data.Services.Interfaces;

namespace TodoApp.Data.Services.Implementations.Sql
{
    public class TodoService(AppDbContext db) : ITodoService
    {
        public async Task AddTodoAsync(TodoEntity entity)
        {
            await db.Todos.AddAsync(entity);
        }

        public async Task DeleteTodoAsync(int id)
        {
            TodoEntity? todoEntity = await db.Todos.FindAsync(id);
            if(todoEntity != null)
            {
                db.Todos.Remove(todoEntity);
            }
        }

        public async Task<ICollection<TodoEntity>> GetAllTodosAsync()
        {
            return await db.Todos.OrderBy(todo => todo.CreatedAt).ToListAsync();
        }

        public async Task<(ICollection<TodoEntity>, PaginationMetaData paginationMetaData)> GetAllTodosAsync
            (bool? isCompleted, DateTime? createdAt, DateTime? completedAt, string? searchQuery,
                string? sort, int pageNumber, int pageSize)
        {
            IQueryable<TodoEntity> collection = db.Todos;

            if (isCompleted != null)
            {
                collection = collection.Where(todo => todo.IsCompleted == isCompleted);
            }
            if (createdAt != null)
            {
                collection = collection.Where(todo => todo.CreatedAt == createdAt);
            }
            if (completedAt != null)
            {
                collection = collection.Where(todo => todo.CompletedAt == completedAt);
            }
            if (!string.IsNullOrEmpty(searchQuery))
            {
                collection = collection.Where(todo =>
                    (todo.Heading != null && todo.Heading.Contains(searchQuery)) ||
                    (todo.Description != null && todo.Description.Contains(searchQuery)));
            }

            int totalCount = await collection.CountAsync();
            PaginationMetaData paginationMetaData = new PaginationMetaData(totalCount, pageNumber, pageSize);

            var collectionToReturn = await collection
                    .OrderBy(todo => todo.CreatedAt)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

            return (collectionToReturn, paginationMetaData);
        }

        public async Task<TodoEntity?> GetTodoByIdAsync(int id)
        {
            return await db.Todos.FindAsync(id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await db.SaveChangesAsync() > 0;
        }
    }
}
