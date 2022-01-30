using DotNetDapper.Dtos;
using DotNetDapper.Filters;
using DotNetDapper.Middleware;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using WebAPICoreDapper.Models;

namespace Dapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ValidateModel]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly string _connectionString;
        public ProductController(IConfiguration configuration,
              ILogger<ProductController> logger)
        {
            _logger = logger;
            _connectionString = configuration.GetConnectionString("DbConnectionString");
        }

        /// <summary>
        /// Get Product By Id (Tìm sản phẩm với ID)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> Get(int id)
        {
            _logger.LogInformation($"GetProductById Id = {id}");

            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
                paramaters.Add("@id", id);
                var result = await conn.QueryAsync<Product>("Get_Product_ById", paramaters, null, null, System.Data.CommandType.StoredProcedure);

                return Ok(new ApiOkResponse(result.SingleOrDefault()));
            }
        }

        // GET: api/Product/5
        [HttpGet("paging")]
        public async Task<IActionResult> GetPaging(string keyWord, int categoryId, int pageIndex, int pageSize)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
                paramaters.Add("@keyWord", keyWord);
                paramaters.Add("@categoryId", categoryId);
                paramaters.Add("@pageIndex", pageIndex);
                paramaters.Add("@pageSize", pageSize);
                paramaters.Add("@totalRow", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);

                var result = await conn.QueryAsync<Product>("Get_Product_AllPaging", paramaters, null, null, System.Data.CommandType.StoredProcedure);

                int totalRow = paramaters.Get<int>("@totalRow");
                var response = new PagedResult<Product>()
                {
                    Items = result.ToList(),
                    TotalRow = totalRow,
                    PageSize = pageSize,
                    PageIndex = pageIndex
                };

                return Ok(new ApiOkResponse(response));
            }
        }

        /// <summary>
        ///  Create Product (Thêm sản phẩm)
        /// </summary>
        /// <remarks>
        /// { "id": 0,"sku": "string","name": "string","price": 0,"discount_Price": 0, "is_Active": true,"image_Url": "string","view_Count": 0, "created_At": "2022-01-30T05:54:00.676Z"}
        /// </remarks>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Product product)
        {
            int newId = 0;
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
                paramaters.Add("@sku", product.Sku);
                paramaters.Add("@price", product.Price);
                paramaters.Add("@isActive", product.IsActive);
                paramaters.Add("@imageUrl", product.ImageUrl);
                paramaters.Add("@name", product.Name);
                paramaters.Add("@id", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
                var result = await conn.ExecuteAsync("Create_Product", paramaters, null, null, System.Data.CommandType.StoredProcedure);

                newId = paramaters.Get<int>("@id");
            }
            return Ok(new ApiOkResponse(newId));
        }

        /// <summary>
        ///  Update Info Product (Cập nhật thông tin sản phẩm)
        /// </summary>
        /// <remarks>
        /// { "id": 0,"sku": "string","name": "string","price": 0,"discount_Price": 0, "is_Active": true,"image_Url": "string","view_Count": 0, "created_At": "2022-01-30T05:54:00.676Z"}
        /// </remarks>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Product product)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
                paramaters.Add("@id", id);
                paramaters.Add("@sku", product.Sku);
                paramaters.Add("@price", product.Price);
                paramaters.Add("@isActive", product.IsActive);
                paramaters.Add("@imageUrl", product.ImageUrl);
                paramaters.Add("@name", product.Name);
                var result = await conn.ExecuteAsync("Update_Product", paramaters, null, null, System.Data.CommandType.StoredProcedure);

                return Ok(new ApiOkResponse(result));
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
                paramaters.Add("@id", id);
                var result = await conn.ExecuteAsync("Delete_Product_ById", paramaters, null, null, System.Data.CommandType.StoredProcedure);

                return Ok(new ApiOkResponse(result));
            }
        }
    }
}