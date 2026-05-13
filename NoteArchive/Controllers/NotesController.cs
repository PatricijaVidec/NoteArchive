using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NoteArchive.Data;
using NoteArchive.Models;
using System.Security.Claims;
using System.IO.Compression;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace NoteArchive.Controllers
{
    public class NotesController : Controller
    {
        private readonly NoteArchiveContext _context;

        public NotesController(NoteArchiveContext context)
        {
            _context = context;
        }

        // GET: Notes
        public async Task<IActionResult> Index(string searchString)
        {
            var notes = _context.Notes
                .Include(n => n.Images)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                notes = notes.Where(n =>
                    n.Title.Contains(searchString) ||
                    n.Author.Contains(searchString) ||
                    n.Genre.Contains(searchString) ||
                    n.Performer.Contains(searchString));
            }

            return View(await notes.ToListAsync());
        }
        
        // GET: Notes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Notes
            .Include(n => n.Images)
            .FirstOrDefaultAsync(m => m.ID == id);

            if (note == null)
            {
                return NotFound();
            }

            return View(note);
        }

        // GET: Notes/Create
        
        public IActionResult Create()
        {
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Notes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Note note,List<IFormFile> files)
        {
            note.UserID = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            ModelState.Remove("UserID");

            if (ModelState.IsValid)
            {
                _context.Notes.Add(note);

                await _context.SaveChangesAsync();

                if (files != null && files.Count > 0)
                {
                    foreach (var file in files)
                    {
                        var fileName =
                            Guid.NewGuid().ToString()
                            + Path.GetExtension(file.FileName);

                        var uploadPath = Path.Combine(
                            Directory.GetCurrentDirectory(),
                            "wwwroot/uploads",
                            fileName);

                        using (var stream =
                            new FileStream(uploadPath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        var image = new NoteImage
                        {
                            ImagePath = "/uploads/" + fileName,
                            NoteID = note.ID
                        };

                        _context.NoteImages.Add(image);
                    }

                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            return View(note);
        }
        // GET: Notes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Notes.FindAsync(id);
            if (note == null)
            {
                return NotFound();
            }
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", note.UserID);
            return View(note);
        }

        // POST: Notes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,Author,Genre,Performer,PublicationDate")] Note note)
        {
            if (id != note.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(note);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NoteExists(note.ID))
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
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", note.UserID);
            return View(note);
        }

        public async Task<IActionResult> DownloadImages(int id)
        {
            var note = await _context.Notes
                .Include(n => n.Images)
                .FirstOrDefaultAsync(n => n.ID == id);

            if (note == null)
            {
                return NotFound();
            }

            using var memoryStream = new MemoryStream();

            using (var zip = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                foreach (var image in note.Images)
                {
                    var path = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot",
                        image.ImagePath.TrimStart('/'));

                    if (System.IO.File.Exists(path))
                    {
                        var entry = zip.CreateEntry(Path.GetFileName(path));

                        using var entryStream = entry.Open();
                        using var fileStream = System.IO.File.OpenRead(path);

                        await fileStream.CopyToAsync(entryStream);
                    }
                }
            }

            memoryStream.Position = 0;

            return File(
                memoryStream.ToArray(),
                "application/zip",
                $"{note.Title}.zip");
        }

        // GET: Notes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var note = await _context.Notes
                .Include(n => n.User)
                .FirstOrDefaultAsync(m => m.ID == id);
                 
            if (note == null)
            {
                return NotFound();
            }

            if (note.UserID != User.FindFirstValue(ClaimTypes.NameIdentifier)
                && !User.IsInRole("Administrator"))
            {
                return Forbid();
            }

            return View(note);
        }

        // POST: Notes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var note = await _context.Notes.FindAsync(id);

            if (note == null)
            {
                return NotFound();
            }

            if (note.UserID != User.FindFirstValue(ClaimTypes.NameIdentifier)
                && !User.IsInRole("Administrator"))
            {
                return Forbid();
            }

            _context.Notes.Remove(note);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool NoteExists(int id)
        {
            return _context.Notes.Any(e => e.ID == id);
        }
    }
}
