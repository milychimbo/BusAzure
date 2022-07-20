using Inventory.Api.Events.Impl;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Api.Events.Handlers
{
    public class ProductStockEventHandler : IHandler<IEnumerable<ProductStockEvent>>
    {
        public void Execute(IEnumerable<ProductStockEvent> @event)
        {
            // Always store connection strings securely. 
            string connectionString = "Server=tcp:servidorbus1.database.windows.net;"
                + "Database=stock;User ID=echimbo@servidorbus1.database.windows.net;Password=Psswrd18052000;"
                + "Trusted_Connection=False;Encrypt=True;";


            List<ProductStockEvent> items = @event.ToList();
            for (int i = 0; i < items.Count; i++)
            {
                int id = items[i].ProductId;
                int cantidad = items[i].Quantity;
                decimal unidad = items[i].UnitPrice;
                // Best practice is to scope the SqlConnection to a "using" block
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // Connect to the database
                    conn.Open();

                    // Read rows
                    string sql = "INSERT INTO [dbo].[registros] ([productId],[quantity],[unitPrice]) VALUES (" + id.ToString() + "," + cantidad.ToString() + "," + unidad.ToString() + ")";
                    SqlCommand selectCommand = new SqlCommand(sql, conn);
                    SqlDataReader results = selectCommand.ExecuteReader();


                }
            }

            // Do something awesome with your event
        }

        Task IHandler<IEnumerable<ProductStockEvent>>.Execute(IEnumerable<ProductStockEvent> @event)
        {
            throw new System.NotImplementedException();
        }
    }
}
