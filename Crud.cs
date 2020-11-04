using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using YMLParser;

namespace YMLParser
{
    /// <summary>
    /// Controller for basic CRUD operations.
    /// </summary>
    public class CrudController
    {
        /// <summary>
        /// Establishes connection to SQL base on Docker container.
        /// </summary>
        /// <returns></returns>
        private SqlConnection GetConnection()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["DockerDB"].ConnectionString);
        }
        /// <summary>
        /// Creates new entry in database given shop data.
        /// </summary>
        /// <param name="shopData">Shop data</param>
        public void Create(ShopDto shopData)
        {
            using(var connection = GetConnection())
            {
                connection.Open();
                using(var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO shopInfo(id, name, shopId) VALUES (@id, @name, @shopId)";
                    cmd.Parameters.AddWithValue("@id", shopData.id);
                    cmd.Parameters.AddWithValue("@name", shopData.name);
                    cmd.Parameters.AddWithValue("@shopId", shopData.shopId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        /// <summary>
        /// Updates database entry given shop data.
        /// </summary>
        /// <param name="shopData">Shop data</param>
        public void Update(ShopDto shopData)
        {
            using(var connection = GetConnection())
            {
                connection.Open();
                using(var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "UPDATE shopInfo SET name=@name, shopId=@shopId WHERE id=@id";
                    cmd.Parameters.AddWithValue("@id", shopData.id);
                    cmd.Parameters.AddWithValue("@name", shopData.name);
                    cmd.Parameters.AddWithValue("@shopId", shopData.shopId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        /// <summary>
        /// Deletes database entry of given id
        /// </summary>
        /// <param name="id">Elements id</param>
        public void Delete(int id)
        {
            using(var connection = GetConnection())
            {
                connection.Open();
                using(var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM shopInfo WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id",id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        /// <summary>
        /// Gets offer by given id
        /// </summary>
        /// <param name="id">Offer id</param>
        /// <returns>ShopDto object, if there's no entries of given id returns null</returns>
        public ShopDto Get(int id)
        {
            using(var connection = GetConnection())
            {
                connection.Open();
                using(var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT id, name, shopId FROM shopInfo WHERE id=@id";
                    cmd.Parameters.AddWithValue("@id", id);
                    using(var datareader = cmd.ExecuteReader())
                    {
                        return !datareader.Read() ? null : LoadShopInfo(datareader);
                    }
                }
            }
        }
        /// <summary>
        /// Gets up to 50 offers of given shopID
        /// </summary>
        /// <param name="shopId">Shop id</param>
        /// <returns>List of ShopDto objects</returns>
        public List<ShopDto> GetMany(int shopId)
        {
            List<ShopDto> shopInfos = new List<ShopDto>();
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT TOP 50 id, name, shopId FROM shopInfo WHERE shopId=@shopId";
                    cmd.Parameters.AddWithValue("@shopId", shopId);
                    using(var datareader = cmd.ExecuteReader())
                    {
                        while(datareader.Read())
                        {
                            shopInfos.Add(LoadShopInfo(datareader));
                        }
                    }
                }
            }
            return shopInfos;
        }
        /// <summary>
        /// Converts data from SQL data reader to ShopDto object
        /// </summary>
        /// <param name="reader">SQLDataReader object</param>
        /// <returns>ShopDto object</returns>
        private static ShopDto LoadShopInfo(SqlDataReader reader)
        {
            return new ShopDto
            {
                id = reader.GetInt32(reader.GetOrdinal("id")),
                name = reader.GetString(reader.GetOrdinal("name")),
                shopId = reader.GetInt32(reader.GetOrdinal("shopId"))
            };
        }
    }
}