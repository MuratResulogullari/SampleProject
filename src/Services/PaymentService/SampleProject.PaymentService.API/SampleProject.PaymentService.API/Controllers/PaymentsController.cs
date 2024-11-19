using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SampleProject.PaymentService.API.ApiClients;
using System.Net.Http;
using System.Threading;

namespace SampleProject.PaymentService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentsController : ControllerBase
{
  private readonly OrderServiceApiClient _orderServiceApiClient;
  private readonly HttpClient _httpClient;
  public PaymentsController(OrderServiceApiClient orderServiceApiClient, HttpClient httpClient)
  {
    _orderServiceApiClient = orderServiceApiClient;
    _httpClient = httpClient;
  }

  [HttpGet("get-orders-httpclient")]
  public async Task<IActionResult> GetOrdersHttpClientAsync()
  {
    var result = new List<OrderIdResponse>();

    _httpClient.BaseAddress =new Uri("http://localhost:5129");
    var response = await _httpClient.GetAsync("/api/Orders/get-order-ids");
    if (response.IsSuccessStatusCode)
    {
      var content = await response.Content.ReadAsStringAsync();
      result = System.Text.Json.JsonSerializer.Deserialize<List<OrderIdResponse>>(content);
    }
    return Ok(result);
  } 
  [HttpGet("get-orders-aspire")]
  public async Task<IActionResult> GetOrdersAspireAsync()
  {
    var result = await _orderServiceApiClient.GetOrderIdsAsync(); 
    return Ok(result);
  }
}