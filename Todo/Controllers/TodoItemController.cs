﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todo.Data;
using Todo.Data.Entities;
using Todo.EntityModelMappers.TodoItems;
using Todo.Models.TodoItems;
using Todo.Services;

namespace Todo.Controllers
{
    [Authorize]
    public class TodoItemController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public TodoItemController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Create(int todoListId)
        {
            var todoList = dbContext.SingleTodoList(todoListId);
            var fields = TodoItemCreateFieldsFactory.Create(todoList, User.Id());
            return View(fields);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TodoItemCreateFields fields)
        {
            if (!ModelState.IsValid) { return View(fields); }

            var item = new TodoItem(fields.TodoListId, fields.ResponsiblePartyId, fields.Title, fields.Importance, 0);

            await dbContext.AddAsync(item);
            await dbContext.SaveChangesAsync();

            return RedirectToListDetail(fields.TodoListId);
        }

        [HttpPost]
        public async Task<IActionResult> DirectCreate(TodoItemCreateFields fields)
        {
            if (!ModelState.IsValid) { return Content(ModelState.FormatErrorsAsHtml(), "text/html"); }

            var item = new TodoItem(fields.TodoListId, fields.ResponsiblePartyId, fields.Title, fields.Importance, 0);

            await dbContext.AddAsync(item);
            await dbContext.SaveChangesAsync();

            await dbContext.Entry(item).Reference(i => i.ResponsibleParty).LoadAsync();
            return PartialView("_TodoItemPartial", TodoItemSummaryViewmodelFactory.Create(item));
        }

        [HttpGet]
        public IActionResult Edit(int todoItemId)
        {
            var todoItem = dbContext.SingleTodoItem(todoItemId);
            var fields = TodoItemEditFieldsFactory.Create(todoItem);
            return View(fields);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TodoItemEditFields fields)
        {
            if (!ModelState.IsValid) { return View(fields); }

            var todoItem = dbContext.SingleTodoItem(fields.TodoItemId);

            TodoItemEditFieldsFactory.Update(fields, todoItem);

            dbContext.Update(todoItem);
            await dbContext.SaveChangesAsync();

            return RedirectToListDetail(todoItem.TodoListId);
        }

        private RedirectToActionResult RedirectToListDetail(int fieldsTodoListId)
        {
            return RedirectToAction("Detail", "TodoList", new {todoListId = fieldsTodoListId});
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRank(TodoItemUpdateRankFields fields)
        {
            if (!ModelState.IsValid) { return Json(ModelState.FormatErrorsForJson()); }

            var todoItem = dbContext.SingleTodoItem(fields.TodoItemId);
            todoItem.Rank = fields.Rank;

            dbContext.Update(todoItem);
            await dbContext.SaveChangesAsync();

            await dbContext.Entry(todoItem).Reference(i => i.ResponsibleParty).LoadAsync();
            return Json(TodoItemSummaryViewmodelFactory.Create(todoItem));
        }
    }
}