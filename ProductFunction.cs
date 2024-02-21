using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TailwindApi.Models;
using TailwindApi.Service.IService;

namespace TailwindApi
{
    public class ProductFunction
    {
        private readonly ILogger<ProductFunction> _logger;
        private readonly IProductService _service;

        public ProductFunction(ILogger<ProductFunction> logger, IProductService service)
        {
            _logger = logger;
            _service = service;
        }

        [Function(nameof(CreateProduct))]
        public async Task<IActionResult> CreateProduct([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "product")] HttpRequest req)
        {
            _logger.LogInformation("Create new product.");

            var requestData = await new StreamReader(req.Body).ReadToEndAsync();
            Product? data = JsonConvert.DeserializeObject<Product>(requestData);
            if (data == null) 
            {
                return new BadRequestResult();
            }
            string response = await _service.CreateProductAsync(data);
            return new OkObjectResult(response);

        }

        [Function(nameof(GetProducts))]
        public async Task<IActionResult> GetProducts([HttpTrigger(AuthorizationLevel.Function, "get", Route = "products")] HttpRequest req)
        {
            _logger.LogInformation("Get all product.");

            var response = await _service.GetProductsAsync();

            return new OkObjectResult(response);
        }

        [Function(nameof(GetProductById))]
        public async Task<IActionResult> GetProductById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "product/{id}")] HttpRequest req,string id)
        {
            _logger.LogInformation("Get Product by Id.");
            var response = await _service.GetProductByIdAsync(id);
            return new OkObjectResult(response);
        }

        [Function(nameof(UpdateProduct))]
        public async Task<IActionResult> UpdateProduct([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "product")] HttpRequest req)
        {
            _logger.LogInformation("Update product.");
            var requestData = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<Product>(requestData);
            var response = await _service.UpdateProductAsync(data);
            return new OkObjectResult(response);
        }

        [Function(nameof(DeleteProduct))]
        public async Task<IActionResult> DeleteProduct([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "product/{id}")] HttpRequest req,string id)
        {
            _logger.LogInformation("Delete product by Id.");
            var response= await _service.DeleteProductAsync(id);
            return new OkObjectResult(response);
        }
    }
}
