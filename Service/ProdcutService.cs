using Microsoft.Azure.Cosmos;
using TailwindApi.Models;
using TailwindApi.Service.IService;

namespace TailwindApi.Service
{
    public class ProdcutService : IProductService
    {

        //private readonly Container _container;

        private readonly CosmosClient _client;
        private readonly Database database;
        private readonly Container _container;

        public ProdcutService(CosmosClient client)
        {
            _client = client;
            database = _client.GetDatabase("tailwind");
            _container = _client.GetContainer(database.Id, "products");
        }

        //public ProdcutService(CosmosClient cosmosClient, string databaseName, string containerName)
        //{
        //    _container = cosmosClient.GetContainer(databaseName,containerName);
        //}
        public async Task<string> CreateProductAsync(Product product)
        {
            var response = await _container.CreateItemAsync(product);
            return response.Resource.Id;
        }

        public async Task<string> DeleteProductAsync(string productId)
        {

            var response = await _container.DeleteItemAsync<Product>(productId, new PartitionKey(productId));
            return response.ActivityId;
        }

        public async Task<Product> GetProductByIdAsync(string id)
        {
            var response = await _container.ReadItemAsync<Product>(id, new PartitionKey(id));
            return response;
        }

        public async Task<IList<Product>> GetProductsAsync()
        {
            var sqlQuery = "SELECT * FROM c";
            QueryDefinition queryDefinition = new QueryDefinition(sqlQuery);
            FeedIterator<Product> queryResultSetIterator = _container.GetItemQueryIterator<Product>(queryDefinition);
            List<Product> products = new List<Product>();
            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<Product> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (Product product in currentResultSet)
                {
                    products.Add(product);
                }
            }
            return products;
        }

        public async Task<string> UpdateProductAsync(Product product)
        {
            var response = await _container.UpsertItemAsync(product, new PartitionKey(product.Id));
            return response.Resource.Id;
        }
    }
}
