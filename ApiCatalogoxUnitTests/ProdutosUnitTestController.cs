using APICatalogo.Context;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogoxUnitTests
{
    public class ProdutosUnitTestController
    {
        public AppDbContext context;
        public static DbContextOptions<AppDbContext> dbContextOptions { get; }

        public static string connectionString = "Server=localhost;Database=CatalogoDB;Uid=root;Pwd=Nanis162415!";

        static ProdutosUnitTestController()
        {
            dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                .Options;
        }
        
        public ProdutosUnitTestController()
        {
            context = new AppDbContext(dbContextOptions);
        }
    }
}
