using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Todo.Data.Entities;

namespace Todo.Tests
{
    /*
     * This class makes it easier for tests to create new TodoLists with TodoItems correctly hooked up
     */
    public class TestTodoListBuilder
    {
        private readonly string title;
        private readonly IdentityUser owner;
        private readonly List<(string Name, Importance Importance, int Rank, IdentityUser ResponsibleParty)> items = new List<(string, Importance, int, IdentityUser)>();

        public TestTodoListBuilder(IdentityUser owner, string title)
        {
            this.title = title;
            this.owner = owner;
        }

        public TestTodoListBuilder WithItem(string itemTitle, Importance importance, IdentityUser responsiblePartry = null, int rank = 0)
        {
            items.Add((itemTitle, importance, rank, (responsiblePartry == null) ? owner : responsiblePartry));
            return this;
        }

        public TodoList Build()
        {
            var todoList = new TodoList(owner, title);
            var todoItems = items.Select(itm => new TodoItem(todoList.TodoListId, itm.ResponsibleParty.Id, itm.Name, itm.Importance, itm.Rank).WithResponsibleParty(itm.ResponsibleParty));
            todoItems.ToList().ForEach(tlItm =>
            {
                todoList.Items.Add(tlItm);
                tlItm.TodoList = todoList;
            });
            return todoList;
        }
    }

    public static class TodoListItemExtensions
    {
        public static TodoItem WithResponsibleParty(this TodoItem todoItem, IdentityUser responsibleParty)
        {
            todoItem.ResponsiblePartyId = responsibleParty.Id;
            todoItem.ResponsibleParty = responsibleParty;

            return todoItem;
        }
    }
}