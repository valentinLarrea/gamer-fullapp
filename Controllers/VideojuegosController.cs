using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GAMER.Data;
using GAMER.Models;
using Microsoft.AspNetCore.Authorization;



namespace GAMER.Controllers
{
    public class VideojuegosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VideojuegosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Videojuegos

        public async Task<IActionResult> Index(string nombreBuscado, int? generoId, int? plataformaId)
        {

            ViewData["Busqueda"] = nombreBuscado;

            var videojuegos = _context.Videojuego
            .Include(v => v.Desarrollador)
            .Include(v => v.Genero)
            .Include(v => v.VideojuegoPlataformas)
            .ThenInclude(vp => vp.Plataforma)
            .AsQueryable();

            if (!string.IsNullOrEmpty(nombreBuscado))
            {
                videojuegos = videojuegos.Where(v => v.Titulo.Contains(nombreBuscado));
            }

            if (generoId.HasValue)
            {
                videojuegos = videojuegos.Where(v => v.Genero.Id == generoId.Value);
            }

            if (plataformaId.HasValue)
            {
                videojuegos = videojuegos.Where(v => v.VideojuegoPlataformas.Any(vp => vp.Plataforma.Id == plataformaId.Value));
            }

            var generos = await _context.Genero.ToListAsync();
            var plataformas = await _context.Plataforma.ToListAsync();

            ViewData["GeneroId"] = new SelectList(generos, "Id", "Descripcion");
            ViewData["PlataformaId"] = new SelectList(plataformas, "Id", "Tipo");

            return View(await videojuegos.ToListAsync());
        }

        // GET: Videojuegos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var videojuego = await _context.Videojuego
                .Include(v => v.Desarrollador)
                .Include(v => v.Genero)
                .Include(v => v.VideojuegoPlataformas) 
            .ThenInclude(vp => vp.Plataforma)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (videojuego == null)
            {
                return NotFound();
            }

            return View(videojuego);
        }

        // GET: Videojuegos/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["DesarrolladorId"] = new SelectList(_context.Desarrollador, "Id", "Nombre");
            ViewData["GeneroId"] = new SelectList(_context.Genero, "Id", "Descripcion");
            ViewData["PlataformaId"] = new SelectList(_context.Plataforma, "Id", "Tipo");
            return View();
        }

        // POST: Videojuegos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Titulo,Sinopsis,Imagen,GeneroId,DesarrolladorId")] Videojuego videojuego, int[]? PlataformaIds, IFormFile? imagen)
        {
            if (ModelState.IsValid)
            {
                if (imagen != null && imagen.Length > 0)
                {
                    var extensionesPermitidas = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var extensionArchivo = Path.GetExtension(imagen.FileName).ToLowerInvariant();

                    if (!extensionesPermitidas.Contains(extensionArchivo))
                    {
                        ModelState.AddModelError("Imagen", "Solo se permiten archivos de imagen.");
                        return View(videojuego);
                    }

                    var nombreArchivo = Path.GetFileName(imagen.FileName);
                    var rutaGuardado = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagenes", nombreArchivo);

                    using (var stream = new FileStream(rutaGuardado, FileMode.Create))
                    {
                        await imagen.CopyToAsync(stream);
                    }

                    videojuego.Imagen = $"/imagenes/{nombreArchivo}";
                }

                _context.Add(videojuego);
                await _context.SaveChangesAsync();

                if (PlataformaIds != null)
                {
                    foreach (var plataformaId in PlataformaIds)
                    {
                        _context.Add(new VideojuegoPlataforma
                        {
                            VideojuegoId = videojuego.Id,
                            PlataformaId = plataformaId
                        });
                    }
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DesarrolladorId"] = new SelectList(_context.Desarrollador, "Id", "Nombre", videojuego.DesarrolladorId);
            ViewData["GeneroId"] = new SelectList(_context.Genero, "Id", "Descripcion", videojuego.GeneroId);
            ViewData["PlataformaId"] = new SelectList(_context.Plataforma, "Id", "Tipo", PlataformaIds);
            return View(videojuego);
        }

        // GET: Videojuegos/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id, int[]? PlataformaIds)
        {
            if (id == null)
            {
                return NotFound();
            }

            var videojuego = await _context.Videojuego.FindAsync(id);
            if (videojuego == null)
            {
                return NotFound();
            }
            ViewData["DesarrolladorId"] = new SelectList(_context.Desarrollador, "Id", "Nombre", videojuego.DesarrolladorId);
            ViewData["GeneroId"] = new SelectList(_context.Genero, "Id", "Descripcion", videojuego.GeneroId);
            ViewData["PlataformaId"] = new SelectList(_context.Plataforma, "Id", "Tipo", PlataformaIds);
            return View(videojuego);
        }

        // POST: Videojuegos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Sinopsis,Imagen,GeneroId,DesarrolladorId")] Videojuego videojuego, int[]? PlataformaIds, IFormFile? imagen)
        {
            if (id != videojuego.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (imagen != null && imagen.Length > 0)
                    {
                        var extensionesPermitidas = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                        var extensionArchivo = Path.GetExtension(imagen.FileName).ToLowerInvariant();

                        if (!extensionesPermitidas.Contains(extensionArchivo))
                        {
                            ModelState.AddModelError("Imagen", "Solo se permiten archivos de imagen.");
                            return View(videojuego);
                        }
                        var nombreArchivo = Path.GetFileName(imagen.FileName);
                        var rutaGuardado = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagenes", nombreArchivo);

                        using (var stream = new FileStream(rutaGuardado, FileMode.Create))
                        {
                            await imagen.CopyToAsync(stream);
                        }

                        videojuego.Imagen = $"/imagenes/{nombreArchivo}";
                    }
                    _context.Update(videojuego);
                    await _context.SaveChangesAsync();

                    if (PlataformaIds != null)
                    {
                        var plataformasExistentes = _context.VideojuegoPlataformas
                            .Where(vp => vp.VideojuegoId == videojuego.Id).ToList();
                        _context.VideojuegoPlataformas.RemoveRange(plataformasExistentes);
                        await _context.SaveChangesAsync();

                        foreach (var plataformaId in PlataformaIds)
                        {
                            _context.Add(new VideojuegoPlataforma
                            {
                                VideojuegoId = videojuego.Id,
                                PlataformaId = plataformaId
                            });
                        }
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VideojuegoExists(videojuego.Id))
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
            ViewData["DesarrolladorId"] = new SelectList(_context.Desarrollador, "Id", "Nombre", videojuego.DesarrolladorId);
            ViewData["GeneroId"] = new SelectList(_context.Genero, "Id", "Descripcion", videojuego.GeneroId);
            ViewData["PlataformaId"] = new SelectList(_context.Plataforma, "Id", "Tipo", PlataformaIds);
            return View(videojuego);
        }

        // GET: Videojuegos/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var videojuego = await _context.Videojuego
                .Include(v => v.Desarrollador)
                .Include(v => v.Genero)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (videojuego == null)
            {
                return NotFound();
            }

            return View(videojuego);
        }

        // POST: Videojuegos/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var videojuego = await _context.Videojuego.FindAsync(id);
            if (videojuego != null)
            {
                _context.Videojuego.Remove(videojuego);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VideojuegoExists(int id)
        {
            return _context.Videojuego.Any(e => e.Id == id);
        }
    }
}
