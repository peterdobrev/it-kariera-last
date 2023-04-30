using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoolEvents.Models;
using System.Drawing;

namespace CoolEvents.Controllers
{
    public class EventsController : Controller
    {
        private readonly CoolEventsDBContext _context;

        public EventsController(CoolEventsDBContext context)
        {
            _context = context;
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            if(HttpContext.Session.GetString("IsAuthenticated") != "true")
            {
                return RedirectToAction("Login","Users");
            }

              return _context.Events != null ? 
                          View(await _context.Events.ToListAsync()) :
                          Problem("Entity set 'CoolEventsDBContext.Events'  is null.");
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Events == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,ImageFile,Date")] Event @event)
        {
            if (!@event.IsValidImage())
            {
                ModelState.AddModelError(nameof(@event.ImageFile), "The file must be a valid image type (JPEG, PNG or GIF) and less than 2MB in size.");
            }

            if (ModelState.IsValid)
            {
                byte[] imageData = null;
                if (@event.ImageFile != null)
                {
                    using var stream = new MemoryStream();
                    await @event.ImageFile.CopyToAsync(stream);
                    imageData = stream.ToArray();
                }

                @event.Image = imageData;

                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(@event);
        }


        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Events == null)
            {
                return NotFound();
            }

            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,ImageFile,Date")] Event @event)
        {
            if (id != @event.Id)
            {
                return NotFound();
            }

            if (!@event.IsValidImage())
            {
                ModelState.AddModelError(nameof(@event.ImageFile), "The file must be a valid image type (JPEG, PNG or GIF) and less than 2MB in size.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var eventToUpdate = await _context.Events.FindAsync(id);

                    if (eventToUpdate == null)
                    {
                        return NotFound();
                    }

                    if (@event.ImageFile != null)
                    {
                        using var stream = new MemoryStream();
                        await @event.ImageFile.CopyToAsync(stream);
                        eventToUpdate.Image = stream.ToArray();
                    }

                    eventToUpdate.Name = @event.Name;
                    eventToUpdate.Description = @event.Description;
                    eventToUpdate.Date = @event.Date;

                    _context.Update(eventToUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.Id))
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

            return View(@event);
        }


        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Events == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Events == null)
            {
                return Problem("Entity set 'CoolEventsDBContext.Events'  is null.");
            }
            var @event = await _context.Events.FindAsync(id);
            if (@event != null)
            {
                _context.Events.Remove(@event);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
          return (_context.Events?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public IActionResult GetImage(int id)
        {
            var eventItem = _context.Events.Find(id);
            if (eventItem != null && eventItem.Image != null)
            {
                return File(eventItem.Image, "image/jpeg");
            }
            else
            {
                return NotFound();
            }
        }

    }
}
