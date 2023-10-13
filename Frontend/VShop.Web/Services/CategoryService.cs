using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using VShop.Web.Models;
using VShop.Web.Services.Contracts;

namespace VShop.Web.Services;

public class CategoryService : ICategoryService
{
    private readonly IHttpClientFactory clientFactory;
    private readonly JsonSerializerOptions jsonSerializerOptions;
    private const string apiEndpoint = "/api/categories/";

    public CategoryService(IHttpClientFactory httpClientFactory)
    {
        clientFactory = httpClientFactory;
        jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<IEnumerable<CategoryViewModel>> GetAllCategories(string token)
    {
        var client = clientFactory.CreateClient(name: HttpClientFactoryName.ProductApi);
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(scheme: "Bearer", parameter: token);

        using (HttpResponseMessage response = await client.GetAsync(requestUri: apiEndpoint))
        {
            if (response.IsSuccessStatusCode)
            {
                Stream apiResponse = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<IEnumerable<CategoryViewModel>>(utf8Json: apiResponse, options: jsonSerializerOptions);
            }
        }
        return null;
    }
}
