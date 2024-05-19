using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using PruebaDesempeno.Data;
using PruebaDesempeno.Models;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace PruebaDesempeno.Controllers
{
    public class JobsController : Controller
    {
        public readonly BaseContext _context;
        public  IWebHostEnvironment _hostEnvironment;

        public JobsController(BaseContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index(string search)
        {
            var jobs = from job in _context.Jobs select job;

            if(!String.IsNullOrEmpty(search))
            {
                jobs = jobs.Where(x => x.NameCompany.Contains(search) 
                || x.OfferTitle.Contains(search)
                || x.Description.Contains(search)
                || x.Status.Contains(search)
                || x.Country.Contains(search)
                || x.Languages.Contains(search));
            }

            return View(jobs.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> CreateJobs(Jobs j, IFormFile archivo)
        {
            var routeImage = Path.Combine(_hostEnvironment.WebRootPath, "images",archivo.FileName);

            using(var stream = new FileStream(routeImage, FileMode.Create))
            {
                await archivo.CopyToAsync(stream);
            }
            
           
            j.LogoCompany = "/images/"+archivo.FileName;

            _context.Jobs.Add(j);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public  async Task<IActionResult> Details(int? id)
        {
            return View(await _context.Jobs.FindAsync(id));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            return View(await _context.Jobs.FindAsync(id));
        }

        public async Task<IActionResult> EditJobs(Jobs j, IFormFile archivo)
        {
            string nombreArchivo = archivo.FileName;
            j.LogoCompany = nombreArchivo;
            _context.Jobs.Update(j);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var job = await _context.Jobs.FindAsync(id);
            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}