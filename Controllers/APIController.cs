using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static YCTest.Models.YC;
namespace Juro_Sales.Controllers
{
    [Route("[controller]")]
    public class APIController : Controller
    {
        private readonly YCDbContext _db;
        public APIController(YCDbContext dbComtext)
        {
            _db = dbComtext;
        }
        #region SalesAPI
        [HttpGet("Saless")]
        public IActionResult SalesRead(int page = 1) => Ok(_db.Sales.Skip((page - 1) * 10).Take(10));

        [HttpGet("Sales")]
        public IActionResult SalesReadDetail(string SalesCode) => Ok(_db.Sales.Find(SalesCode));
        [HttpPost("Sales")]
        public IActionResult SalesCreate(Sales data) => CreateData(data);
        [HttpPut("Sales")]
        public IActionResult SalesUpdate(Sales data) => Update(data);
        [HttpDelete("Sales")]
        public IActionResult SalesDelete(int id) => Delete(id, _db.Sales, "Sales");
        #endregion

        #region HousesAPI
        [HttpGet("Houses")]
        public IActionResult HouseRead(int page = 1) => Ok(_db.House.Skip((page - 1) * 10).Take(10));

        [HttpGet("House")]
        public IActionResult HouseReadDetail(string HouseCode) => Ok(_db.House.Find(HouseCode));
        [HttpPost("House")]
        public IActionResult HouseCreate(House data) => CreateData(data);
        [HttpPut("House")]
        public IActionResult HouseUpdate(House data) => Update(data);
        [HttpDelete("House")]
        public IActionResult HouseDelete(int id) => Delete(id, _db.House, "House");
        #endregion

        #region 泛型Function
        private IActionResult CreateData<T>(T entity) where T : class
        {
            try
            {
                _db.Set<T>().Add(entity);
                _db.SaveChanges();
                return Created("Create Ok!", entity);
            }
            catch(Exception ex) {
                return BadRequest(ex.Message);
            }
        }
        private IActionResult Update<T>(T entity) where T : class
        {
            _db.Entry(entity).State = EntityState.Modified;

            try
            {
                _db.SaveChanges();
            }

            catch (DbUpdateConcurrencyException ex)
            {
                return Conflict($"已被其他狀態更改：{ex.Message}");
            }

            catch (Exception ex)
            {
                return BadRequest($"錯誤：{ex.Message}");
            }

            return NoContent();
        }
        private IActionResult Delete<T>(int id, DbSet<T> dbSet, string entityName) where T : class
        {
            var data = dbSet.Find(id);

            if (data == null)
            {
                return NotFound($"{entityName} 不存在 {id}");
            }
            dbSet.Remove(data);
            try
            {
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                return BadRequest($"錯誤：{ex.Message}");
            }

            return NoContent();
        }
        #endregion
    }
}

