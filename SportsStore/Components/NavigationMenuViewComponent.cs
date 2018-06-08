using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Components
{
    /*
     * ViewComponent lässt sich als über eine Razorseite aufrufen.
     * Diese Komponente wird in das Layout eingebunden da diese Komponente
     * das Inhaltsverzeichnis beinhaltet das überall im SportsStore zu sehen
     * sein soll. Die Razor-Layoutseite greift mit @await Component.InvokeAsync("NavigationMenu");
     * auf den hier geschriebenen Code zurück.
     */
    public class NavigationMenuViewComponent : ViewComponent
    {
        private IProductRepository repository;

        public NavigationMenuViewComponent(IProductRepository repo)
        {
            repository = repo;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedCategory = RouteData?.Values["category"];
            return View(repository.Products
                .Select(x => x.Category)
                .Distinct()
                .OrderBy(x => x));
        }
    }
}
