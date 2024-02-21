using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TailwindApi.Models;

namespace TailwindApi.Service.IService
{
    public interface IProductService
    {
        Task<IList<Product>> GetProductsAsync();
        Task<string> CreateProductAsync(Product product);
        Task<string> UpdateProductAsync(Product product);
        Task<string> DeleteProductAsync(string productId);
        Task<Product> GetProductByIdAsync(string productId);
    }
}
