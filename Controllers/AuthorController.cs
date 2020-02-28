using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MVC.Models;
using System.Data;
using System.Threading.Tasks;

namespace MVC.Controllers
{
    public class AuthorController : Controller
    {
        public async Task<IActionResult> Index()
        {
            using (EFCoreWebDemoContext context = new EFCoreWebDemoContext())
            {
                System.Collections.Generic.List<Author> model = await context.Authors.AsNoTracking().ToListAsync();
                return View(model);
            }
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("FirstName, LastName")] Author author)
        {
            using (EFCoreWebDemoContext context = new EFCoreWebDemoContext())
            {
                //context.Add(author);
                SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter { ParameterName = "@AuthorId", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output},
                new SqlParameter { ParameterName = "@FirstName", SqlDbType = SqlDbType.VarChar, Size = 50, Direction = ParameterDirection.Input, SqlValue = author.FirstName},
                new SqlParameter { ParameterName = "@LastName", SqlDbType = SqlDbType.VarChar, Size = 75, Direction = ParameterDirection.Input, SqlValue = author.LastName}
                };
                try
                {
                    context.Database.ExecuteSqlRaw("Exec AuthorSave @AuthorId OUT, @FirstName, @LastName", parameters);
                }
                catch (System.Exception e)
                {
                    //log error
                    ModelState.TryAddModelError("AddingAuthor", e.Message);
                }
                object returnvalue = parameters[0].Value;
                if (returnvalue != null && int.TryParse(returnvalue.ToString(), out int AuthorId) && AuthorId > 0)
                {
                    //Log success message;
                }
                //await context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
        }
    }
}
