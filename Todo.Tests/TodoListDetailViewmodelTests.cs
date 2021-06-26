using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Todo.EntityModelMappers.TodoLists;
using Todo.Models.TodoItems;
using Todo.Models.TodoLists;
using Xunit;

namespace Todo.Tests
{
    public class WhenTodoListDetailViewmodelIsCreated
    {
        [Fact]
        public void ListItemsAreOrderedByImportance()
        {
            // Arrange.
            var todoList = new TestTodoListBuilder(new IdentityUser("alice@example.com"), "list-name")
                                    .WithItem("Item 1", Data.Entities.Importance.Medium)
                                    .WithItem("Item 2", Data.Entities.Importance.Low)
                                    .WithItem("Item 3", Data.Entities.Importance.High)
                                    .WithItem("Item 4", Data.Entities.Importance.Low)
                                    .WithItem("Item 5", Data.Entities.Importance.Medium)
                                    .WithItem("Item 6", Data.Entities.Importance.High)
                                    .Build();

            // Act.
            var viewModel = TodoListDetailViewmodelFactory.Create(todoList);

            // Assert.
            var orderedItems = viewModel.Items.ToList();
            Assert.Equal(orderedItems.Count, todoList.Items.Count);
            Assert.Equal("Item 3", orderedItems[0].Title);
            Assert.Equal("Item 6", orderedItems[1].Title);
            Assert.Equal("Item 1", orderedItems[2].Title);
            Assert.Equal("Item 5", orderedItems[3].Title);
            Assert.Equal("Item 2", orderedItems[4].Title);
            Assert.Equal("Item 4", orderedItems[5].Title);
        }
    }
}
