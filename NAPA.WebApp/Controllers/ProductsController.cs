using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NAPA.AppService;
using NAPA.Database;
using NAPA.Models;
using NAPA.ViewModels;

namespace NAPA.WebApp.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly IConfiguration configuration;

        public ProductsController(ApplicationContext context, IConfiguration _configuration)
        {
            _context = context;
            configuration = _configuration;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            InitializeTotalPrice();
            return View(await _context.Products.ToListAsync());
        }
        public void InitializeTotalPrice()
        {
            var products = _context.Products.ToList();
            double vat = double.Parse(configuration["VAT"]);
            foreach (var product in products)
            {
                product.TotalPrice = VAT.GetTotalPrice(product.Price, product.Quantity, vat);
            }
            _context.SaveChanges();
        }
        // GET: Products/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateProductVM viewModel)
        {
            if (ModelState.IsValid)
            {
                double vat = double.Parse(configuration["VAT"]);
                double price = double.Parse(viewModel.Price.Replace(".", ","));
                int quantity = viewModel.Quantity;
                Product product = new Product
                {
                    Name = viewModel.Name,
                    Price = price,
                    Quantity = quantity,
                    TotalPrice = VAT.GetTotalPrice(price, quantity, vat),
                    LastChangedDate = DateTime.Now.ToString("dd.MM.yyyy HH:mm")
                };
                _context.Add(product);
                int user_id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                await _context.SaveChangesAsync(user_id);
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            EditProductVM viewModel = new EditProductVM()
            {
                Id = id ?? 1,
                Quantity = product.Quantity,
                Price = product.Price.ToString(),
                Name = product.Name
            };
            return View(viewModel);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, EditProductVM viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    double vat = double.Parse(configuration["VAT"]);
                    double price = double.Parse(viewModel.Price.Replace(".", ","));
                    int quantity = viewModel.Quantity;
                    Product product = new Product
                    {
                        Id = id,
                        Name = viewModel.Name,
                        Price = price,
                        Quantity = viewModel.Quantity,
                        TotalPrice = VAT.GetTotalPrice(price, quantity, vat),
                        LastChangedDate = DateTime.Now.ToString("dd.MM.yyyy HH:mm")
                    };
                    _context.Update(product);
                    int user_id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    await _context.SaveChangesAsync(user_id);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(viewModel.Id))
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
            return View(viewModel);
        }


        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Bazada malumot yo'q");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            int user_id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await _context.SaveChangesAsync(user_id);
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
