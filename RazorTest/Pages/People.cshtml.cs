using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorTest.Models;

namespace RazorTest.Pages
{
    public class PeopleModel : PageModel
    {
        private readonly MyDbContext _context;

        public List<Person> People { get; set; } = new List<Person>();

        [BindProperty]
        public Person NewPerson { get; set; }

        public PeopleModel(MyDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            People = _context.People.ToList();
        }

        public IActionResult OnPost()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }
                _context.People.Add(NewPerson);
                _context.SaveChanges();
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                ModelState.AddModelError(string.Empty, "An error occurred while adding the person.");
                return RedirectToPage();
            }
        }
    }
}
