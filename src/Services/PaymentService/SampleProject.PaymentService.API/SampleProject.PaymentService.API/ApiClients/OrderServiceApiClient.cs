using System.Collections.Generic;

namespace SampleProject.PaymentService.API.ApiClients
{
  public class OrderServiceApiClient(HttpClient httpClient)
  {
    public async Task<List<OrderIdResponse>> GetOrderIdsAsync(CancellationToken cancellationToken = default)
    {
      var result = new List<OrderIdResponse>();
      var response = await httpClient.GetAsync("/api/Orders/get-order-ids", cancellationToken);
      if (response.IsSuccessStatusCode)
      {
        var content = await response.Content.ReadAsStringAsync();
        result = System.Text.Json.JsonSerializer.Deserialize<List<OrderIdResponse>>(content);
      }
      return result;
    }
  }
  public class OrderIdResponse
  {
    public string id { get; set; }
  }
}
