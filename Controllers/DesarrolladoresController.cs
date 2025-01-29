using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GAMER.Data;
using GAMER.Models;
using OfficeOpenXml;
using Microsoft.AspNetCore.Authorization;

namespace GAMER.Controllers
{
    public class DesarrolladoresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DesarrolladoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Desarrolladores
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Desarrollador.ToListAsync());
        }

        // GET: Desarrolladores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var desarrollador = await _context.Desarrollador
                .FirstOrDefaultAsync(m => m.Id == id);
            if (desarrollador == null)
            {
                return NotFound();
            }

            return View(desarrollador);
        }

        [HttpGet]
        public IActionResult ImportExcel()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ImportExcel(IFormFile archivoExcel)
        {
            if (archivoExcel != null && archivoExcel.Length > 0)
            {
                using (var stream = new MemoryStream())
                {
                    await archivoExcel.CopyToAsync(stream);

                    using (var package = new ExcelPackage(stream))
                    {
                        var cantHojas = package.Workbook.Worksheets[0];
                        var cantFilas = cantHojas.Dimension.Rows;

                        List<Desarrollador> desarrolladores = new List<Desarrollador>();
                        for (int fila = 1; fila <= cantFilas; fila++)
                        {
                            var nombre = cantHojas.Cells[fila, 1].Text.Trim();
                            var pais = cantHojas.Cells[fila, 2].Text.Trim();
                            if (!string.IsNullOrEmpty(nombre) || !string.IsNullOrEmpty(pais))
                            {
                                desarrolladores.Add(new Desarrollador
                                {
                                    Nombre = nombre,
                                    Pais = pais
                                });
                            }
                        }
                        if (ModelState.IsValid)
                        {
                            _context.Desarrollador.AddRange(desarrolladores);
                            await _context.SaveChangesAsync();
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            Console.WriteLine("No se encontraron desarrolladores");
                        }
                    }
                }
            }

            return View();
        }

        // GET: Desarrolladores/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Desarrolladores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Pais")] Desarrollador desarrollador)
        {
            if (ModelState.IsValid)
            {
                _context.Add(desarrollador);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(desarrollador);
        }

        // GET: Desarrolladores/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var desarrollador = await _context.Desarrollador.FindAsync(id);
            if (desarrollador == null)
            {
                return NotFound();
            }
            return View(desarrollador);
        }

        // POST: Desarrolladores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Pais")] Desarrollador desarrollador)
        {
            if (id != desarrollador.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(desarrollador);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DesarrolladorExists(desarrollador.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(desarrollador);
        }

        // GET: Desarrolladores/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var desarrollador = await _context.Desarrollador
                .FirstOrDefaultAsync(m => m.Id == id);
            if (desarrollador == null)
            {
                return NotFound();
            }

            return View(desarrollador);
        }

        // POST: Desarrolladores/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var desarrollador = await _context.Desarrollador.FindAsync(id);
            if (desarrollador != null)
            {
                _context.Desarrollador.Remove(desarrollador);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DesarrolladorExists(int id)
        {
            return _context.Desarrollador.Any(e => e.Id == id);
        }
    }
}
