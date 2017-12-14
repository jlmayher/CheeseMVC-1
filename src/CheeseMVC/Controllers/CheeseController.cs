using Microsoft.AspNetCore.Mvc;
using CheeseMVC.Models;
using System.Collections.Generic;
using CheeseMVC.ViewModels;
using CheeseMVC.Data;
using System.Linq;

namespace CheeseMVC.Controllers
{
    public class CheeseController : Controller
    {
        private CheeseDbContext cheeseDbContext;

        public CheeseController(CheeseDbContext context)
        {
            this.cheeseDbContext = context;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            List<Cheese> cheeses = this.cheeseDbContext.Cheeses.ToList();

            return View(cheeses);
        }

        public IActionResult Add()
        {
            AddCheeseViewModel addCheeseViewModel = new AddCheeseViewModel();
            return View(addCheeseViewModel);
        }

        [HttpPost]
        public IActionResult Add(AddCheeseViewModel addCheeseViewModel)
        {
            if (ModelState.IsValid)
            {
                // Add the new cheese to my existing cheeses
                Cheese newCheese = new Cheese
                {
                    Name = addCheeseViewModel.Name,
                    Description = addCheeseViewModel.Description,
                    Type = addCheeseViewModel.Type
                };

                this.cheeseDbContext.Cheeses.Add(newCheese);
                this.cheeseDbContext.SaveChanges();

                return Redirect("/Cheese");
            }

            return View(addCheeseViewModel);
        }

        public IActionResult Remove()
        {
            ViewBag.title = "Remove Cheeses";
            ViewBag.cheeses = this.cheeseDbContext.Cheeses.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Remove(int[] cheeseIds)
        {
            foreach (int cheeseId in cheeseIds)
            {
                //var cheese = this.cheeseDbContext.Cheeses.Find(new { ID = cheeseId });
                var cheese = this.cheeseDbContext.Cheeses.Single<Cheese>(c => c.ID == cheeseId);
                this.cheeseDbContext.Cheeses.Remove(cheese);
                this.cheeseDbContext.SaveChanges();
            }

            return Redirect("/");
        }
    }
}
