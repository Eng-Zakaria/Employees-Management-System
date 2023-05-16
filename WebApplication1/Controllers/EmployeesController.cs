using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using WebApplication1.Data;
using WebApplication1.Models;



namespace WebApplication1.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EmployeesController
            (ApplicationDbContext context,
                IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [Route("[controller]/List")]
        [HttpGet]
        public IActionResult GetIndexView(string? search,
            string? sortType, string? sortOrder, int pageSize = 10,
            int pageNumber = 1)
        {
            
            IQueryable<Employee> emps = _context.Employees.AsQueryable();

            if (string.IsNullOrWhiteSpace(search) == false)
            {
                search = search.Trim();
                emps = _context.Employees.Where(e => e.
                FullName.Contains(search));
            }


            if (!string.IsNullOrWhiteSpace(sortType)
                && !string.IsNullOrWhiteSpace(sortOrder))
            {
                if(sortType == "FullName")
                {
                    if(sortOrder == "asc")
                        emps = emps.OrderBy(e=>e.FullName);
                    else if(sortOrder == "desc")
                        emps = emps.OrderByDescending(e=>e.FullName);
                }

                if (sortType == "Position")
                {
                    if (sortOrder == "asc")
                        emps = emps.OrderBy(e => e.Position);
                    else if (sortOrder == "desc")
                        emps = emps.OrderByDescending(e => e.Position);
                }

                if (sortType == "Salary")
                {
                    if (sortOrder == "asc")
                        emps = emps.OrderBy(e => e.Salary);
                    else if (sortOrder == "desc")
                        emps = emps.OrderByDescending(e => e.Salary);
                }

            }

            emps = emps.Skip(pageSize * (pageNumber -1)).Take(pageSize);

            ViewBag.CurrentSearch = search;
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;


            return View("Index", emps.ToList());
        }

        [HttpGet]
        

        [HttpGet]
        public IActionResult GetDetailsView(int id)
        {
            Employee emp = _context.Employees.Include(e=>e.Department).FirstOrDefault(e => e.Id == id);
            if (emp == null)
            {
                return NotFound();
            }
            return View("Details", emp);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            Employee emp = _context.Employees.Include(e=>e.Department).FirstOrDefault(e => e.Id == id);
            if (emp == null)
            {
                return NotFound();
            }

            return View(emp);
        }

        [HttpGet]
        public IActionResult GetCreateView()
        {
            ViewBag.AllDepartments = _context.Departments.ToList();
            return View("Create");
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult AddEmployee([Bind
        ("Id,FullName,Position,Salary,Bonus" +
         ",PhoneNo,ConfirmEmail,Email,ConfirmPassword,Password," +
         "HiringDateTime,AttendanceTime,BirthDate" +
         ",LeavingTime,CreatedAt,LastUpdatedAt,DepartmentId")]
         Employee emp, IFormFile? imageFile)
        {
            if (ModelState.IsValid == true)
            {
                if (imageFile == null)
                {
                    emp.ImageUrl = "\\images\\No_Image.png";
                }
                else
                {
                    string imgExtension = Path.GetExtension(imageFile.FileName);
                    Guid imgGuid = Guid.NewGuid();
                    string imgName = imgGuid + imgExtension;
                    string imgUrl = "\\images\\" + imgName;

                    emp.ImageUrl = imgUrl;
                    string imgPath = _webHostEnvironment.WebRootPath + imgUrl;
                    FileStream imgStream = new FileStream(imgPath, FileMode.Create); 
                    imageFile.CopyTo(imgStream);
                    imgStream.Dispose();
                
                }


                emp.LastUpdatedAt = emp.CreatedAt = DateTime.Now;

                _context.Employees.Add(emp);
                _context.SaveChanges();
                return RedirectToAction("GetIndexView");
            }
            else 
            {
                ViewBag.AllDepartments = _context.Departments.ToList();
                return View("Create"); 
            } 
        }

        public IActionResult GetEditView(int id)
        {
            Employee emp = _context.Employees.FirstOrDefault(e => e.Id == id);
            if (emp == null) 
            {
                return NotFound(); 
            }

            ViewBag.AllDepartments = _context.Departments.ToList();
            return View("Edit", emp);
        }

        [ValidateAntiForgeryToken] // prevents  CSRF Attacks
        [HttpPost]
        public IActionResult EditCurrent(int id,[Bind(
         "Id,FullName,Position,Salary,Bonus,PhoneNo,ConfirmEmail," +
         "Email,ConfirmPassword,Password,HiringDateTime,AttendenceTime," +
         "BirthDate,LeavingTime,CreatedAt,LastUpdatedAt,DepartmentId,ImageUrl"
         )]Employee emp, IFormFile ? imageFile)
        {
            if(emp.Id != id)
            {
               return BadRequest();
            }

            if (ModelState.IsValid == true)
            {
                if (imageFile != null)
                {
                    if (emp.ImageUrl != "\\images\\No_Image.png")
                    {
                        string oldPath = _webHostEnvironment.WebRootPath + emp.ImageUrl;
                        if (System.IO.File.Exists(oldPath))
                        {
                            System.IO.File.Delete(oldPath);
                        }
                        
                        string imgExtension = Path.GetExtension(imageFile.FileName);
                        Guid imgGuid = Guid.NewGuid();
                        string imgName = imgGuid + imgExtension;
                        string imgUrl = "\\images" + imgName;
                        
                        emp.ImageUrl = imgUrl;
                        string imgPath = _webHostEnvironment.WebRootPath + imgUrl;
                        FileStream imgStream = new FileStream(imgPath, FileMode.Create);
                        imageFile.CopyTo(imgStream);
                        imgStream.Dispose();

                    }
                }

                emp.LastUpdatedAt = DateTime.Now;
                _context.Employees.Update(emp);
                _context.SaveChanges();

                ViewBag.AllDepartments = _context.Departments.ToList();
                return RedirectToAction("GetIndexView");
            }
            else
            {
                ViewBag.AllDepartments = _context.Departments.ToList();
                return View("Edit");
            }

        }
        
        public IActionResult GetDeleteView(int id)
        {
            var emp = _context.Employees.FirstOrDefault(e => e.Id == id);
            if (emp == null)
            {
                return NotFound();
            }

            ViewBag.AllDepartments = _context.Departments.ToList();
            return View("Delete", emp);
        }

        [HttpPost]
        public IActionResult DeleteCurrent(int id)
        {

            Employee emp = _context.Employees.FirstOrDefault(e => e.Id == id);

            if (emp.ImageUrl != "\\images\\No_Image.png")
            {
                string imgPath = _webHostEnvironment.WebRootPath + emp.ImageUrl;
                
                if (System.IO.File.Exists(imgPath))
                {
                    System.IO.File.Delete(imgPath);
                }
                
                

            }

            _context.Employees.Remove(emp);
            _context.SaveChanges();

            ViewBag.AllDepartments = _context.Departments.ToList();
            return RedirectToAction("GetIndexView");
        }
    }
}
