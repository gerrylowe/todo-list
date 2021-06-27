using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Todo.Data.Entities;
using Todo.Services;
using Xunit;

namespace Todo.Tests
{
    public class WhenRelevantTodoListsAreRetrievedForAUser
    {
        [Fact]
        public void TheyContainListsOwnedByUserAndListsContainingItemsForWhichUserIsResponsibleParty()
        {
            // Arrange.
            var alice = new IdentityUser("alice@example.com");
            var bob = new IdentityUser("bob@example.com");
            var todoList1 = new TestTodoListBuilder(alice, "list-name-1").Build();      // List owned by user.
            var todoList2 = new TestTodoListBuilder(bob, "list-name-2").Build();        // List owned by someone else.
            var todoList3 = new TestTodoListBuilder(bob, "list-name-3")
                                    .WithItem("Item 3.1", Importance.Medium)
                                    .WithItem("Item 3.2", Importance.Low, alice)
                                    .WithItem("Item 3.3", Importance.High).Build();     // List containing items for which user is responsible party.
            var todoList4 = new TestTodoListBuilder(alice, "list-name-4")
                                    .WithItem("Item 4.1", Importance.Low, bob).Build(); // List owned by user, but all items have other responsible party.
            var todoLists = new List<TodoList> { todoList1, todoList2, todoList3, todoList4 }.AsQueryable();

            // Act.
            var listsRelevantToAlice = todoLists.ApplyRelevantTodoListFilter(alice.Id).ToList();

            // Assert.
            Assert.Equal(3, listsRelevantToAlice.Count());
            Assert.Equal("list-name-1", listsRelevantToAlice[0].Title);
            Assert.Equal("list-name-3", listsRelevantToAlice[1].Title);
            Assert.Equal("list-name-4", listsRelevantToAlice[2].Title);
        }
    }
}
