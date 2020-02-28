using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC.Models;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Controllers
{
    public class BookController : Controller
    {
        public async Task<IActionResult> Index()
        {
            using (EFCoreWebDemoContext context = new EFCoreWebDemoContext())
            {
                System.Collections.Generic.List<Author> model = await context.Authors.Include(a => a.Books).AsNoTracking().ToListAsync();
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            using (EFCoreWebDemoContext context = new EFCoreWebDemoContext())
            {
                System.Collections.Generic.List<SelectListItem> authors = await context.Authors.Select(a => new SelectListItem
                {
                    Value = a.AuthorId.ToString(),
                    Text = $"{a.FirstName} {a.LastName}"
                }).ToListAsync();
                ViewBag.Authors = authors;
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Title, AuthorId")] Book book)
        {
            using (EFCoreWebDemoContext context = new EFCoreWebDemoContext())
            {
                context.Books.Add(book);
                await context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
        }
    }
}