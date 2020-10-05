using AddItemsDynamically.Data;
using AddItemsDynamically.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using SIPVS.Data;
using System.Xml;
using System.Xml.Schema;
using System.Collections.Generic;

namespace AddItemsDynamically.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderService _context;


        public OrdersController(IOrderService context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            return View(_context.FindAll());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var result = _context.FindOne(id);
            Console.Write(result);
            if (result != default)
                return View(result);
            else
                return NotFound();
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            var model = new Order();
            model.Items.Add(new OrderItem());
            return View(model);
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,IsUrgent,Items")] Order order)
        {
            if (ModelState.IsValid)
            {
                order.Created = DateTime.UtcNow;
                _context.Insert(order);
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        [HttpPost]
        public async Task<ActionResult> AddOrderItem([Bind("Items")] Order order)
        {
            order.Items.Add(new OrderItem());
            return PartialView("OrderItems", order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int id)
        {

            var order = _context.FindOne(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,IsUrgent,Created,Items")] Order order)
        {
            Console.WriteLine(order.ToString());
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
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
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int id)
        {

            var order = _context.FindOne(id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = _context.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ExportXML(int id)
        {
            var result = _context.FindOne(id);
            System.Xml.Serialization.XmlSerializer writer =
            new System.Xml.Serialization.XmlSerializer(typeof(Order));
            System.Xml.Serialization.XmlSerializerNamespaces ns = new System.Xml.Serialization.XmlSerializerNamespaces();
            ns.Add("orderxml", "http://www.my.example.com/xml/order");
            var stream = new MemoryStream();
            writer.Serialize(stream, result, ns);
            return File(stream.ToArray(), "application/xml", "export.xml");
        }
        public Stream ExportXMLstream(int id)
        {
            var result = _context.FindOne(id);
            System.Xml.Serialization.XmlSerializer writer =
            new System.Xml.Serialization.XmlSerializer(typeof(Order));
            var stream = new MemoryStream();
            writer.Serialize(stream, result);

            return stream;
        }

        public async Task<IActionResult> ValidateXML(int id)
        {
            XmlReaderSettings xmlSettings = new XmlReaderSettings();
            xmlSettings.Schemas.Add("http://www.my.example.com/xml/orders", "order.xsd");
            xmlSettings.ValidationType = ValidationType.Schema;
            xmlSettings.ValidationEventHandler += new ValidationEventHandler(SettingsValidationEventHandler);
            XmlReader orders = XmlReader.Create("export.xml", xmlSettings);
            return View(orders);
        }
        public static void SettingsValidationEventHandler(object sender, ValidationEventArgs e)
        {
            if (e.Severity == XmlSeverityType.Warning)
            {

                Console.Write("WARNING: ");
                Console.WriteLine(e.Message);
            }
            else if (e.Severity == XmlSeverityType.Error)
            {
                Console.Write("ERROR: ");
                Console.WriteLine(e.Message);
            }
        }

        [HttpPost]
        [Route("[controller]/fileUpload/{id}")]
        public async Task<IActionResult> OnPostUploadAsync(List<IFormFile> files, int id)
        {
            long size = files.Sum(f => f.Length);

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var filePath = Path.GetTempFileName();

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            // Process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return Ok(new { count = files.Count, size });
        }


        private bool OrderExists(int id)
        {
            var result = _context.FindOne(id);
            if (result.Id != null)
                return true;
            else
                return false;

        }
    }
}
