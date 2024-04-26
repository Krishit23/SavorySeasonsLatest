using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SavorySeasons.Data;
using SavorySeasons.Entities;
using SavorySeasons.Models;

namespace SavorySeasons.Controllers
{
    public class OrderController : ControllerBase
    {
        private readonly SavorySeasonsDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(SavorySeasonsDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("api/GetOrders")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Order>>> GetAllOrders()
        {

            var orders = await _dbContext.Orders.Include(o => o.ApplicationUser).ToListAsync();

            return Ok(orders);
        }

        [HttpPost]
        [Route("api/addOrder")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Post([FromBody] CreateOrderDto orderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var checkUser = await _userManager.FindByEmailFromClaimsPrinciple(User);
               
                var order = new Order
                {
                    DishName = orderDto.DishName,
                    Price = orderDto.Price,
                    userId = checkUser.Id,
                    Quantity = orderDto.Quantity,
                };

                _dbContext.Orders.Add(order);
                await _dbContext.SaveChangesAsync();
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
