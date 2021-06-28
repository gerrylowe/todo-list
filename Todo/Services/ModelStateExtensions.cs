using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Services
{
    public static class ModelStateExtensions
    {
        public static string FormatErrorsAsHtml(this ModelStateDictionary modelState)
        {
            var errors = new StringBuilder();
            foreach (var key in modelState.Keys)
            {
                foreach (var error in modelState[key].Errors)
                {
                    errors.Append(error.ErrorMessage);
                    errors.Append("<br/>");
                }
            }

            return errors.ToString();
        }

        public static object FormatErrorsForJson(this ModelStateDictionary modelState)
        {
            var errors = new StringBuilder();
            foreach (var key in modelState.Keys)
            {
                foreach (var error in modelState[key].Errors)
                {
                    errors.Append(error.ErrorMessage);
                }
            }

            return new { ErrorMessage = errors.ToString() };
        }
    }
}
