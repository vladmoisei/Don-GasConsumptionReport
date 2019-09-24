﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Don_GasConsumtionReport;

namespace Don_GasConsumtionReport.Controllers
{
    public class IndexModelsGaddaF2 : Controller
    {
        private readonly RaportareDbContext _context;

        public IndexModelsGaddaF2(RaportareDbContext context)
        {
            _context = context;
        }

        // GET: IndexModelsGaddaF2
        public async Task<IActionResult> Index()
        {
            List<IndexModel> listaDeAfisat = await _context.IndexModels.ToListAsync();
            return View(listaDeAfisat.Where(model => model.PlcName == "PlcGaddaF2" && Auxiliar.IsCurrentMonth(Auxiliar.ReturnareDataFromString(model.Data))));
        }

        // GET: IndexModelsGaddaF2/Details/5
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

        // GET: IndexModelsGaddaF2/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: IndexModelsGaddaF2/Create
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

        // GET: IndexModelsGaddaF2/Edit/5
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

        // POST: IndexModelsGaddaF2/Edit/5
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

        // GET: IndexModelsGaddaF2/Delete/5
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

        // POST: IndexModelsGaddaF2/Delete/5
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
