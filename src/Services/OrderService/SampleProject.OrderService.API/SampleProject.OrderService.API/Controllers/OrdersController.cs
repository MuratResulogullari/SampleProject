using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SampleProject.OrderService.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class OrdersController : ControllerBase
  {
    public OrdersController() { }


    [HttpGet("get-order-ids")]
    public async Task<IActionResult> GetOrderIdsAsync()
    {
      Task.CompletedTask.Wait();
      return Ok(new List<OrderIdResponse> 
      {
        new OrderIdResponse{Id=Guid.NewGuid().ToString()},
        new OrderIdResponse{Id=Guid.NewGuid().ToString()},
        new OrderIdResponse{Id=Guid.NewGuid().ToString()},
        new OrderIdResponse{Id=Guid.NewGuid().ToString()},
        new OrderIdResponse{Id=Guid.NewGuid().ToString()},
        new OrderIdResponse{Id=Guid.NewGuid().ToString()}
      });

    }
    private class OrderIdResponse
    {
      public string Id { get; set; }
    }
  }
}
