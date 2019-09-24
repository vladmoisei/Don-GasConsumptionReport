using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Don_GasConsumtionReport;

namespace Don_GasConsumtionReport.Controllers
{
    public class IndexModelsGaddaF4 : Controller
    {
        private readonly RaportareDbContext _context;

        public IndexModelsGaddaF4(RaportareDbContext context)
        {
            _context = context;
        }

        // GET: IndexModelsGaddaF4
        public async Task<IActionResult> Index()
        {
            List<IndexModel> listaDeAfisat = await _context.IndexModels.ToListAsync();
            return View(listaDeAfisat.Where(model => model.PlcName == "PlcGaddaF4" && Auxiliar.IsCurrentMonth(Auxiliar.ReturnareDataFromString(model.Data))));
            //return View(await _context.IndexModels.ToListAsync());
        }

        // GET: IndexModelsGaddaF4/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var indexModel = await _context.IndexModels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (indexModel == null)
            {
                return NotFound();
            }

            return View(indexModel);
        }

        // GET: IndexModelsGaddaF4/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: IndexModelsGaddaF4/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Data,PlcName,IndexValue,GazValue")] IndexModel indexModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(indexModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(indexModel);
        }

        // GET: IndexModelsGaddaF4/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var indexModel = await _context.IndexModels.FindAsync(id);
            if (indexModel == null)
            {
                return NotFound();
            }
            return View(indexModel);
        }

        // POST: IndexModelsGaddaF4/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Data,PlcName,IndexValue,GazValue")] IndexModel indexModel)
        {
            if (id != indexModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(indexModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IndexModelExists(indexModel.Id))
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
            return View(indexModel);
        }

        // GET: IndexModelsGaddaF4/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var indexModel = await _context.IndexModels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (indexModel == null)
            {
                return NotFound();
            }

            return View(indexModel);
        }

        // POST: IndexModelsGaddaF4/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var indexModel = await _context.IndexModels.FindAsync(id);
            _context.IndexModels.Remove(indexModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IndexModelExists(int id)
        {
            return _context.IndexModels.Any(e => e.Id == id);
        }
    }
}
