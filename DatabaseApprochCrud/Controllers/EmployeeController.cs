using DatabaseApprochCrud.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DatabaseApprochCrud.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeDbContect employeeDb;

        public EmployeeController(EmployeeDbContect employeeDb)
        {
            this.employeeDb = employeeDb;
        }

        

        public IActionResult Index()
        {
            if(HttpContext.Session.GetString("UserSession") != null)
            {
                var employeeData = employeeDb.Employees.ToList();
                return View(employeeData);
            }
            else
            {
                return RedirectToAction("Login");
            }
            
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Employee emp, string[] Hobbies)
        {
            
            try
            {
                emp.Hobbies = string.Join(",", Hobbies);
                await employeeDb.Employees.AddAsync(emp);
                await employeeDb.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error: {ex.Message}");
            }
            return View(emp);
        }

        
        public async Task<IActionResult> Edit(int id, string[] Hobbies)
        {
            if(id == null || employeeDb.Employees == null)
            {
                return NotFound();
            }
            var empData = await employeeDb.Employees.FindAsync(id);
            ViewBag.HobbiesList = empData.Hobbies?.Split(',') ?? Array.Empty<string>();

            if (empData == null)
            {  
                return NotFound();
            }
            return View(empData);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string[] Hobbies, Employee emp)
        {
            if (ModelState.IsValid)
            {
                var empData = await employeeDb.Employees.FindAsync(emp.Id);
                if (empData != null)
                {
                    empData.FirstName = emp.FirstName;
                    empData.LastName = emp.LastName;
                    empData.Email = emp.Email;
                    empData.Password = emp.Password;
                    empData.Gender = emp.Gender;
                    empData.City = emp.City;
                    empData.Country = emp.Country;
                    empData.Hobbies = string.Join(",", Hobbies);

                    await employeeDb.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return NotFound();
            }
            return View(emp);
        }

        public IActionResult Login()
        {
            if(HttpContext.Session.GetString("UserSession") != null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(Employee emp)
        {
            var myUser = employeeDb.Employees.Where(x=> x.Email == emp.Email & x.Password == emp.Password).FirstOrDefault();
            if (myUser != null)
            {
                HttpContext.Session.SetString("UserSession",emp.Email);
                return RedirectToAction("Index");
            }
            
            return View();
        }

        public IActionResult Logout()
        {
            if(HttpContext.Session.GetString("UserSession") != null)
            {
                HttpContext.Session.Remove("UserSession");
                return RedirectToAction("Login");
            }
            return View();
        }
    }
}
