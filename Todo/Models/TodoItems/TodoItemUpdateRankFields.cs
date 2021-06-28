using System.ComponentModel.DataAnnotations;
using Todo.Data.Entities;

namespace Todo.Models.TodoItems
{
    public class TodoItemUpdateRankFields
    {
        public int TodoItemId { get; set; }
        public int Rank { get; set; }

        public TodoItemUpdateRankFields() { }
    }
}