using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PuppeteerSharp;
using System.Linq;
using PuppeteerSharp.Input;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Data.SqlClient;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Company.Function
{
    public class GeneratePdf
    {
        private readonly AppInfo appInfo;

        public GeneratePdf(AppInfo browserInfo)
        {
            this.appInfo = browserInfo;
        }

        [FunctionName("GeneratePdf")]
        public async Task Run([QueueTrigger("orders")] string myQueueItem, ILogger log)
        {
            log.LogInformation($"Browser path: {appInfo.BrowserExecutablePath}");

            int orderId = int.Parse(myQueueItem);
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
                ExecutablePath = appInfo.BrowserExecutablePath
            });
            var page = await browser.NewPageAsync();
            await page.GoToAsync($"http://localhost:{appInfo.RazorPagesServerPort}/");

            string orderDetailsData = GetOrderDetailsData(orderId);
            string orderData = GetOrderData(orderId);
            await page.TypeAsync("#items-box", orderDetailsData);
            await page.TypeAsync("#customerInfo-box", orderData);
            await Task.WhenAll(
                page.WaitForNavigationAsync(), 
                page.ClickAsync("#submit-button"));
            var stream = await page.PdfStreamAsync();
            await browser.CloseAsync();

            string storageConnectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            BlobContainerClient container = new BlobContainerClient(storageConnectionString, "invoices");
            container.CreateIfNotExists(Azure.Storage.Blobs.Models.PublicAccessType.None);
            var blockBlobClient = container.GetBlobClient(orderId.ToString() + ".pdf");
            blockBlobClient.Upload(stream, new BlobHttpHeaders { ContentType = "application/pdf" });
            
            string sasUri = blockBlobClient.GenerateSasUri(permissions: Azure.Storage.Sas.BlobSasPermissions.Read, expiresOn: DateTime.Now.AddYears(1)).AbsoluteUri;

            var sqlConnectionString = Environment.GetEnvironmentVariable("DefaultConnection");
            using (var connection = new SqlConnection(sqlConnectionString))
            {
                string sql = "Update Orders  Set Processed = -1, InvoiceURL = @InvoiceURL where OrderId = @OrderId";
                var affectedRows = connection.Execute(sql,new {OrderId = orderId, InvoiceURL = sasUri});
            }
        }

        private string GetOrderDetailsData(int orderId)
        {
            var sqlConnectionString = Environment.GetEnvironmentVariable("DefaultConnection");

            string sqlOrderDetails = "select OrderDetails.ProductId, Products.Title as [Name], Products.Price as [UnitPrice], OrderDetails.Quantity from OrderDetails INNER JOIN Products ON OrderDetails.ProductId = Products.ProductId where OrderDetails.OrderId = @OrderID";
            
            using (var connection = new SqlConnection(sqlConnectionString))
            {
                var orderDetails = connection.Query<OrderDetail>(sqlOrderDetails, new {OrderID = orderId}).ToList();

                var data = orderDetails.Select(_ => new {
                    productId = _.ProductId.ToString(),
                    name = _.Name,
                    quantity = _.Quantity,
                    unitPrice = Decimal.Parse(_.UnitPrice)
                });

                return JsonConvert.SerializeObject(data);
            }
        }

        private string GetOrderData(int orderId)
        {
            var sqlConnectionString = Environment.GetEnvironmentVariable("DefaultConnection");

            string sqlOrderDetails = "select * from [Orders] where OrderId = @OrderID";
            
            using (var connection = new SqlConnection(sqlConnectionString))
            {
                var orderDetails = connection.QueryFirstOrDefault<Order>(sqlOrderDetails, new {OrderID = orderId});
                return JsonConvert.SerializeObject(orderDetails);
            }
        }

        public class OrderDetail
        {
            public int ProductId { get; set; }
            public string Name { get; set; }
            public string UnitPrice { get; set; }
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
}

