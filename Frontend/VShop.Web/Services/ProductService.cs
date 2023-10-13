using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VShop.Web.Models;
using VShop.Web.Services.Contracts;

namespace VShop.Web.Services;

public class ProductService : IProductService
{
    private readonly IHttpClientFactory clientFactory;
    private const string apiEndpoint = "/api/products/";
    private ProductViewModel productVM;
    private IEnumerable<ProductViewModel> productsVM;
    private readonly JsonSerializerOptions jsonSerializerOptions;


    public ProductService(IHttpClientFactory clientFactory)
    {
        this.clientFactory = clientFactory;
        jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<IEnumerable<ProductViewModel>> GetAllProducts(string token)
    {
        HttpClient client = clientFactory.CreateClient(name: HttpClientFactoryName.ProductApi);
        PutTokenInHeaderAuthorization(token, client);

        using (HttpResponseMessage response = await client.GetAsync(requestUri: apiEndpoint))
        {
            if (!response.IsSuccessStatusCode)
                return null;

            var apiResponse = await response.Content.ReadAsStreamAsync();
            productsVM = await JsonSerializer
                        .DeserializeAsync<IEnumerable<ProductViewModel>>(utf8Json: apiResponse, options: jsonSerializerOptions);
        }

        return productsVM;
    }

    public async Task<ProductViewModel> FindProductById(int id, string token)
    {
        HttpClient client = clientFactory.CreateClient(name: HttpClientFactoryName.ProductApi);
        PutTokenInHeaderAuthorization(token, client);

        using (HttpResponseMessage response = await client.GetAsync(requestUri: apiEndpoint + id))
        {
            if (!response.IsSuccessStatusCode)
                return null;

            var apiResponse = await response.Content.ReadAsStreamAsync();
            productVM = await JsonSerializer
                        .DeserializeAsync<ProductViewModel>(utf8Json: apiResponse, options: jsonSerializerOptions);
        }

        return productVM;
    }

    public async Task<ProductViewModel> CreateProduct(ProductViewModel productViewModel, string token)
    {
        HttpClient client = clientFactory.CreateClient(name: HttpClientFactoryName.ProductApi);
        PutTokenInHeaderAuthorization(token, client);

        StringContent content = new StringContent(
            content: JsonSerializer.Serialize(value: productViewModel),
            encoding: Encoding.UTF8,
            mediaType: "application/json"
        );

        using (HttpResponseMessage response = await client.PostAsync(requestUri: apiEndpoint, content: content))
        {
            if (!response.IsSuccessStatusCode) return null;

            var apiResponse = await response.Content.ReadAsStreamAsync();
            productVM = await JsonSerializer
                        .DeserializeAsync<ProductViewModel>(utf8Json: apiResponse, options: jsonSerializerOptions);
        }

        return productVM;
    }

    public async Task<ProductViewModel> UpdateProduct(ProductViewModel productViewModel, string token)
    {
        HttpClient client = clientFactory.CreateClient(name: HttpClientFactoryName.ProductApi);
        PutTokenInHeaderAuthorization(token, client);
        ProductViewModel productUpdated = new ProductViewModel();

        using (HttpResponseMessage response = await client.PutAsJsonAsync(requestUri: apiEndpoint, value: productViewModel))
        {
            if (!response.IsSuccessStatusCode) return null;

            var apiResponse = await response.Content.ReadAsStreamAsync();
            productUpdated = await JsonSerializer
                        .DeserializeAsync<ProductViewModel>(utf8Json: apiResponse, options: jsonSerializerOptions);
        }

        return productUpdated;
    }

    public async Task<bool> DeleteProductById(int id, string token)
    {
        HttpClient client = clientFactory.CreateClient(name: HttpClientFactoryName.ProductApi);
        PutTokenInHeaderAuthorization(token, client);

        using (HttpResponseMessage response = await client.DeleteAsync(requestUri: apiEndpoint + id))
        {
            if (response.IsSuccessStatusCode)
                return true;
        }

        return false;
    }

    private static void PutTokenInHeaderAuthorization(string token, HttpClient client)
    {
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(scheme: "Bearer", parameter: token);
    }
}
