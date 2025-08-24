using APICatalogo.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace ApiCatalogo.IntegrationTests.Factories
{
    public class ApiCatalogoFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test");

            builder.ConfigureServices(services =>
            {
                var descriptorApp = services.SingleOrDefault(d =>
                    d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                if (descriptorApp != null) services.Remove(descriptorApp);

                var descriptorIdentity = services.SingleOrDefault(d =>
                    d.ServiceType == typeof(DbContextOptions<IdentityContext>));
                if (descriptorIdentity != null) services.Remove(descriptorIdentity);

                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                services.AddDbContext<IdentityContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                var sp = services.BuildServiceProvider();
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var appDb = scopedServices.GetRequiredService<AppDbContext>();
                    var identityDb = scopedServices.GetRequiredService<IdentityContext>();

                    appDb.Database.EnsureCreated();
                    identityDb.Database.EnsureCreated();

                    if (!appDb.Categorias.Any())
                    {
                        appDb.Categorias.Add(new APICatalogo.Models.Categoria { CategoriaId = 1, Nome = "Categoria Teste 1", ImagemUrl = "imagem_1.jpg" });
                        appDb.SaveChanges();
                    }
                }
            });
        }
    }
}