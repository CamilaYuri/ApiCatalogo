using ApiCatalogo.IntegrationTests.Fixtures;
using APICatalogo.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ApiCatalogo.IntegrationTests.Factories
{
    public class ApiCatalogoFactory : WebApplicationFactory<Program>
    {
        private readonly DbFixture _dbFixture;

        public ApiCatalogoFactory(DbFixture dbFixture)
        {
            _dbFixture = dbFixture;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test");

            builder.ConfigureServices(services =>
            {
                var descriptorApp = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                if (descriptorApp != null) services.Remove(descriptorApp);

                var descriptorIdentity = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<IdentityContext>));
                if (descriptorIdentity != null) services.Remove(descriptorIdentity);

                services.AddDbContext<AppDbContext>(options =>
                    options.UseMySql(
                        _dbFixture.ConnectionString,
                        ServerVersion.AutoDetect(_dbFixture.ConnectionString),
                        b => b.MigrationsAssembly("APICatalogo")
                    )
                );

                services.AddDbContext<IdentityContext>(options =>
                    options.UseMySql(
                        _dbFixture.ConnectionString,
                        ServerVersion.AutoDetect(_dbFixture.ConnectionString),
                        b => b.MigrationsAssembly("APICatalogo")
                    )
                );
            });
        }
    }
}
