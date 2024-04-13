using API.Data;
using API.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace API.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly string? _con;
        public CategoryRepository(IConfiguration configuration)
        {
            _con = configuration.GetConnectionString("MachineTestDB")?? 
            throw new InvalidOperationException("Connection string 'MachineTestDB' not found in configuration.");
        }
        public async Task<List<CategoryModel>> GetAllCategoriesAsync()
        {
            List<CategoryModel> categories = new List<CategoryModel>();
            using(var con= new SqlConnection(_con))
            {
                await con.OpenAsync();

                string query = "SELECT * FROM Category";
                using(var com= new SqlCommand(query,con))
                {
                    using(var reader = await com.ExecuteReaderAsync())
                    {
                        while(await reader.ReadAsync())
                        {
                            var category = new CategoryModel
                            {
                                CategoryId = Convert.ToInt32(reader["CategoryId"]),
                                CategoryName = reader["CategoryName"].ToString(),
                            };
                            categories.Add(category);
                        }
                    }
                }
            }
            return  categories;
        }
        public async Task<CategoryModel> GetCategoryByIdAsync(int CategoryId)
        {
            using(var con= new SqlConnection(_con))
            {
                await con.OpenAsync();

                string query = "SELECT categoryId, CategoryName FROM Category WHERE CategoryId = @CategoryId";
                using(var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@CategoryId", CategoryId);
                    using(var reader = await cmd.ExecuteReaderAsync())
                    {
                        if(await reader.ReadAsync())
                        {
                            var category = new CategoryModel
                            {
                                CategoryId = Convert.ToInt32(reader["CategoryId"]),
                                CategoryName = reader["CategoryName"].ToString()
                            };
                            return category;
                        }
                    }
                }
            }
            return null;
        }
        public async Task<int> AddCategoryAsync(CategoryModel categoryModel)
        {
            using(var con = new SqlConnection(_con))
            {
                await con.OpenAsync();

                string query = "INSERT INTO Category(Categoryname)VALUES(@CategoryName);SELECT SCOPE_IDENTITY();";
                
                using(var command = new SqlCommand(query,con))
                {
                    command.Parameters.AddWithValue("@CategoryName", categoryModel.CategoryName);
                    object result = await command.ExecuteScalarAsync();
                    return Convert.ToInt32(result);
                }
            }
        }
        public async Task UpdateCategoryAsync(int categoryId, CategoryModel categoryModel)
        {
            using (var connection = new SqlConnection(_con))
            {
                await connection.OpenAsync();

                string query = "UPDATE Category SET CategoryName = @CategoryName WHERE CategoryId = @CategoryId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CategoryName", categoryModel.CategoryName);
                    command.Parameters.AddWithValue("@CategoryId", categoryId);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
         public async Task DeleteCategoryAsync(int categoryId)
        {
            using (var connection = new SqlConnection(_con))
            {
                await connection.OpenAsync();

                string query = "DELETE FROM Category WHERE CategoryId = @CategoryId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CategoryId", categoryId);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}