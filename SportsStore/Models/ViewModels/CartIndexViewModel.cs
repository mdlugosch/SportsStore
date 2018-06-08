using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models.ViewModels
{
    public class CartIndexViewModel
    {
        // Hält ein Cart-Objekt
        public Cart Cart { get; set; }
        // ReturnUrl für den Continue-Button
        public string ReturnUrl { get; set; }
    }
}
