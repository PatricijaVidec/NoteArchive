using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NoteArchive.Data;
using NoteArchive.Models;

namespace NoteArchive.Controllers
{
    public class NoteImagesController : Controller
    {
        private readonly NoteArchiveContext _context;

        public NoteImagesController(NoteArchiveContext context)
        {
            _context = context;
        }

        // GET: NoteImages
        public async Task<IActionResult> Index()
        {
            var noteArchiveContext = _context.NoteImages.Include(n => n.Note);
            return View(await noteArchiveContext.ToListAsync());
        }

        // GET: NoteImages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var noteImage = await _context.NoteImages
                .Include(n => n.Note)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (noteImage == null)
            {
                return NotFound();
            }

            return View(noteImage);
        }

        // GET: NoteImages/Create
        public IActionResult Create()
        {
            ViewData["NoteID"] = new SelectList(_context.Notes, "ID", "ID");
            return View();
        }

        // POST: NoteImages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ImagePath,NoteID")] NoteImage noteImage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(noteImage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["NoteID"] = new SelectList(_context.Notes, "ID", "ID", noteImage.NoteID);
            return View(noteImage);
        }

        // GET: NoteImages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var noteImage = await _context.NoteImages.FindAsync(id);
            if (noteImage == null)
            {
                return NotFound();
            }
            ViewData["NoteID"] = new SelectList(_context.Notes, "ID", "ID", noteImage.NoteID);
            return View(noteImage);
        }

        // POST: NoteImages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ImagePath,NoteID")] NoteImage noteImage)
        {
            if (id != noteImage.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(noteImage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NoteImageExists(noteImage.ID))
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
            ViewData["NoteID"] = new SelectList(_context.Notes, "ID", "ID", noteImage.NoteID);
            return View(noteImage);
        }

        // GET: NoteImages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var noteImage = await _context.NoteImages
                .Include(n => n.Note)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (noteImage == null)
            {
                return NotFound();
            }

            return View(noteImage);
        }

        // POST: NoteImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var noteImage = await _context.NoteImages.FindAsync(id);
            if (noteImage != null)
            {
                _context.NoteImages.Remove(noteImage);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NoteImageExists(int id)
        {
            return _context.NoteImages.Any(e => e.ID == id);
        }
    }
}
