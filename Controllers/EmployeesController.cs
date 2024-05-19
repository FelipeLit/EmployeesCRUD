using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using PruebaDesempeno.Data;
using PruebaDesempeno.Models;
using System.IO;

using Microsoft.EntityFrameworkCore;

namespace PruebaDesempeno.Controllers
{
    public class EmployeesController : Controller
    {
        public readonly BaseContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public EmployeesController(BaseContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index(string search)
        {
            var employee = from employees in _context.Employees select employees;

            if(!String.IsNullOrEmpty(search))
            {
                employee = employee.Where(x => x.Names.Contains(search) 
                || x.LastNames.Contains(search)
                || x.Gender.Contains(search)
                || x.Address.Contains(search)
                || x.CivilStatus.Contains(search)
                || x.Languages.Contains(search)
                || x.Email.Contains(search)
                || x.Phone.Contains(search)
                || x.AcademicTitle.Contains(search)
                ||x.Area.Contains(search));
            }

            return View(employee.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> CreateEmploye(Employees e, IFormFile imagen,IFormFile cv)
        {
            var routeProfile = Path.Combine(_hostEnvironment.WebRootPath, "images", imagen.FileName); 
            var routeCv = Path.Combine(_hostEnvironment.WebRootPath, "documents", cv.FileName);
            
            using (var stream = new FileStream(routeProfile, FileMode.Create)){
                await imagen.CopyToAsync(stream);
            }

            using (var stream = new FileStream(routeCv, FileMode.Create)){
                await cv.CopyToAsync(stream);
            }

            e.ProfilePicture = "/images/"+imagen.FileName;
            e.Cv = "/documents/"+cv.FileName;
           

            _context.Employees.Add(e);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public  async Task<IActionResult> Details(int? id)
        {
            return View(await _context.Employees.FindAsync(id));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            return View(await _context.Employees.FindAsync(id));
        }

        public async Task<IActionResult> EditJobs(Employees e, IFormFile archivo)
        {
            string nombreArchivo = archivo.FileName;
            e.ProfilePicture = nombreArchivo;
            e.Cv = nombreArchivo;
            _context.Employees.Update(e);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var employe = await _context.Employees.FindAsync(id);
            _context.Employees.Remove(employe);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}