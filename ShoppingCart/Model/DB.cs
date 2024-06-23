using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Model
{
    public class DB
    {
        MySqlConnection connection = new MySqlConnection("server=localhost; port=3308; username=root; password=root; database=shoppingcart; charset=utf8");

        public void openConnection()
        {
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
        }

        public void closeConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }

        public MySqlConnection getConnection()
        {
            return connection;
        }

        public void addProduct(Product product)
        {
            // Добавление продукта в базу данных
        }

        public void addHistoryElement(Purchase purchase)
        {
            // Добавление покупки в историю в базе данных
        }
    }
}