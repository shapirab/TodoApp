using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Data.DataModels.Dto.TodoDtos
{
    public class TodoToUpdateDto
    {
        public string? Heading { get; set; }
        public string? Description { get; set; }
        public bool? IsCompleted { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}
