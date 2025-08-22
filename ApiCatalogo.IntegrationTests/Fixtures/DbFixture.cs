using APICatalogo.Context;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.IntegrationTests.Fixtures
{
    public class DbFixture : IDisposable
    {
        public readonly AppDbContext AppDbContext;
        public readonly IdentityContext IdentityContext;
        public readonly string ConnectionString;
        private bool _disposed;

        public DbFixture()
        {
            ConnectionString = "server=localhost;port=3308;database=catalogodb;user=root;password=SecretPassword01";

            var appOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString),
                    b => b.MigrationsAssembly("APICatalogo"))
                .Options;
            AppDbContext = new AppDbContext(appOptions);

            var identityOptions = new DbContextOptionsBuilder<IdentityContext>()
                .UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString),
                    b => b.MigrationsAssembly("APICatalogo"))
                .Options;
            IdentityContext = new IdentityContext(identityOptions);
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                AppDbContext.Dispose();
                IdentityContext.Dispose();
                _disposed = true;
            }
        }
    }
}
