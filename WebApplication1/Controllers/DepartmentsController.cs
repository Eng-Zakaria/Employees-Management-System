using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DepartmentsController
            (ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetIndexView()
        {
            return View("Index", _context.Departments);
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View(_context.Departments);
        }
        [HttpGet]
        public IActionResult GetDetailsView(int id)
        {
            Department dept = _context.Departments.Include(e=>e.Employees).FirstOrDefault(d => d.Id == id);
            if (dept == null)
            {
                return NotFound();
            }
            return View("Details", dept);
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            Department dept = _context.Departments.Include(e=>e.Employees).FirstOrDefault(d => d.Id == id);
            if (dept == null)
            {
                return NotFound();
            }
            return View(dept);
        }

        [HttpGet]
        public IActionResult GetCreateView()
        {
            return View("Create");
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult AddDepartment([Bind
                    ("Id,Name,Description")]Department dept)
        {
            if (ModelState.IsValid == true)
            {
                _context.Departments.Add(dept);
                _context.SaveChanges();

                return RedirectToAction("GetIndexView");
            }
            else
            {
                return View("Create");
            }
        }
        public IActionResult GetEditView(int id)
        {
            var dept = _context.Departments.FirstOrDefault(d => d.Id == id);
            if (dept == null)
            {
                return NotFound();
            }

            return View("Edit", dept);
        }
        [ValidateAntiForgeryToken] // prevents  CSRF Attacks
        [HttpPost]
        public IActionResult EditCurrent(int id, [Bind("Id,FullName,Description")] Department dept)
        {
            if (dept.Id == id)
            {
                return BadRequest();
            }



            if (ModelState.IsValid == true)
            {
                _context.Departments.Update(dept);
                _context.SaveChanges();
                return RedirectToAction("GetIndexView");

            }
            else
            {
                return View("Edit");
            }

        }
        public IActionResult GetDeleteView(int id)
        {
            var dept = _context.Departments.FirstOrDefault(d => d.Id == id);
            if (dept == null)
            {
                return NotFound();
            }

            return View("Delete", dept);
        }
        [HttpPost]
        public IActionResult DeleteCurrent(int id)
        {
            Department dept = _context.Departments.FirstOrDefault(d => d.Id == id);

            _context.Departments.Remove(dept);
            _context.SaveChanges();

            return RedirectToAction("GetIndexView");
        }
    }
}
