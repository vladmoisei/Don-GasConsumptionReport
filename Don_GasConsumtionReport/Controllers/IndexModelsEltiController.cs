using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Don_GasConsumtionReport;
using System.IO;
using OfficeOpenXml;

namespace Don_GasConsumtionReport.Controllers
{
    public class IndexModelsEltiController : Controller
    {
        private readonly RaportareDbContext _context;

        public IndexModelsEltiController(RaportareDbContext context)
        {
            _context = context;
        }

        // GET: IndexModelsElti
        public async Task<IActionResult> Index()
        {
            List<IndexModel> listaDeAfisat = await _context.IndexModels.ToListAsync();
            return View(listaDeAfisat.Where(model => model.PlcName == "PlcElti" && Auxiliar.IsCurrentMonth(Auxiliar.ReturnareDataFromString(model.Data))));
        }

        // Functie exportare data to excel file
        public async Task<IActionResult> ExportToExcelAsync(string dataFrom, string dataTo)
        {
            //return Content(dataFrom + "<==>" + dataTo);
            List<IndexModel> listaSql = await _context.IndexModels.ToListAsync();
            IEnumerable<IndexModel> listaExcel = listaSql.Where(model => model.PlcName == "PlcElti");

            // Extrage datele cuprinse intre limitele date de operator
            IEnumerable<IndexModel> listaDeAfisat = listaExcel.Where(model => Auxiliar.IsDateBetween(model.Data, dataFrom, dataTo));

            var stream = new MemoryStream();

            using (var pck = new ExcelPackage(stream))
            {
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Elti");
                ws.Cells["A1:Z1"].Style.Font.Bold = true;

                ws.Cells["A1"].Value = "Id";
                ws.Cells["B1"].Value = "Data";
                ws.Cells["C1"].Value = "Nume Plc";
                ws.Cells["D1"].Value = "Index gaz";
                ws.Cells["E1"].Value = "Consum gaz";

                int rowStart = 2;
                foreach (var elem in listaDeAfisat)
                {
                    ws.Cells[string.Format("A{0}", rowStart)].Value = elem.Id;
                    ws.Cells[string.Format("B{0}", rowStart)].Value = elem.Data;
                    ws.Cells[string.Format("C{0}", rowStart)].Value = elem.PlcName;
                    ws.Cells[string.Format("D{0}", rowStart)].Value = elem.IndexValue;
                    ws.Cells[string.Format("E{0}", rowStart)].Value = elem.GazValue;
                    rowStart++;
                }

                ws.Cells["A:AZ"].AutoFitColumns();

                pck.Save();
            }
            stream.Position = 0;
            string excelName = "RaportGazElti.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);

        }


        // GET: IndexModelsCuptor/Details/5
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

        // GET: IndexModelsElti/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: IndexModelsElti/Create
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

        // GET: IndexModelsElti/Edit/5
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

        // POST: IndexModelsElti/Edit/5
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

        // GET: IndexModelsElti/Delete/5
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

        // POST: IndexModelsElti/Delete/5
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
