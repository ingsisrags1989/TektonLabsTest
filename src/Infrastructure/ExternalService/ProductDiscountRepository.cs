using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ExternalService
{
    public class ProductDiscountRepository : IProductDiscountRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ProductDiscountRepository(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<ProductDiscountEntity> GetDataAsync(int ProductId)
        {

            var url = _configuration["ExternalService:ProductDiscount"];
            var response = await _httpClient.GetAsync($"{url}/{ProductId}");
            response.EnsureSuccessStatusCode();
            var responseText = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ProductDiscountEntity>(responseText);
        }
    }
}
