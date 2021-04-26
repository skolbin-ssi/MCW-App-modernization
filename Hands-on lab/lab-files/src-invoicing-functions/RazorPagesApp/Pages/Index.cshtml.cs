using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace RazorPagesApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IEnumerable<InvoiceItem> Items { get; set; }
        public Order orderInformation { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnPost()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            var itemsJson = Request.Form["items"].First();
            var customerJson = Request.Form["customerInfo"].First();
            
            Items = JsonSerializer.Deserialize<IEnumerable<InvoiceItem>>(itemsJson, options);
            orderInformation = JsonSerializer.Deserialize<Order>(customerJson);
        }
    }

    public class InvoiceItem
    {
        public string ProductId { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }

    public class Order
        {
            public int OrderID { get; set; }
            public string OrderDate { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string PostalCode { get; set; }
            public string Country { get; set; }
        }
}
