using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BikeDistributor.WebApp.Models;

namespace BikeDistributor.WebApp.Controllers
{
    [RoutePrefix("api/store")]
    public class StoreController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetBikes()
        {
            return Ok(
                new ApplicationDbContext().BikeModels.ToList()
            );
        }

        [Authorize]
        [Route("cart")]
        [HttpPost]
        public IHttpActionResult AddItemToCart()
        {
            return Created(new Uri("/api/store/cart/0"), new object());
        }

        [Authorize]
        [Route("cart")]
        [HttpGet]
        public IHttpActionResult GetItemsInCart()
        {
            return Ok(
                new
                {
                    company = "Contoso",
                    lines = new List<Line>
                    {
                        new Line(new Bike("Test", "test", 1000m), 10)
                    },
                    tax = .0725m
                }
                );
        }
    }
}
