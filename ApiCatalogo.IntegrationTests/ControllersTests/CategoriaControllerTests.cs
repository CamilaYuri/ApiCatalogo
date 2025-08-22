using ApiCatalogo.IntegrationTests.Factories;
using ApiCatalogo.IntegrationTests.Fixtures;
using APICatalogo.Models;
using FluentAssertions;
using System.Net.Http.Json;

namespace ApiCatalogo.IntegrationTests.ControllersTests
{
    [Collection("Database")]
    public class CategoriaControllerTests : IClassFixture<DbFixture>
    {
        private readonly HttpClient _client;
        private readonly DbFixture _dbFixture;

        public CategoriaControllerTests(DbFixture dbFixture)
        {
            _dbFixture = dbFixture;
            var factory = new ApiCatalogoFactory(_dbFixture);
            _client = factory.CreateClient();
        }

        private async Task DeleteCategoriaAsync(int id)
        {
            var categoria = await _dbFixture.AppDbContext.Categorias.FindAsync(id);
            if (categoria != null)
            {
                _dbFixture.AppDbContext.Categorias.Remove(categoria);
                await _dbFixture.AppDbContext.SaveChangesAsync();
            }
        }

        [Fact]
        public async Task GetCategoriasProdutos_Returns_CategoriasWithProdutos()
        {
            var response = await _client.GetAsync("/categorias/produtos");
            response.EnsureSuccessStatusCode();

            var categorias = await response.Content.ReadFromJsonAsync<IEnumerable<Categoria>>();
            categorias.Should().NotBeNull();
            categorias.Should().HaveCountGreaterThan(0);
        }

        [Fact]
        public async Task Get_ReturnsAllCategorias()
        {
            var response = await _client.GetAsync("/categorias");
            response.EnsureSuccessStatusCode();

            var categorias = await response.Content.ReadFromJsonAsync<IEnumerable<Categoria>>();
            categorias.Should().NotBeNull();
            categorias.Should().HaveCountGreaterThan(0);
        }

        [Fact]
        public async Task Get_ById_ReturnsCategoria()
        {
            var novo = new Categoria { Nome = "CategoriaTesteTemp", ImagemUrl = "temp.jpg" };
            var post = await _client.PostAsJsonAsync("/categorias", novo);
            post.EnsureSuccessStatusCode();
            var created = await post.Content.ReadFromJsonAsync<Categoria>();

            try
            {
                var response = await _client.GetAsync($"/categorias/{created.CategoriaId}");
                response.EnsureSuccessStatusCode();

                var categoria = await response.Content.ReadFromJsonAsync<Categoria>();
                categoria.Should().NotBeNull();
                categoria.CategoriaId.Should().Be(created.CategoriaId);
            }
            finally
            {
                await DeleteCategoriaAsync(created.CategoriaId);
            }
        }

        [Fact]
        public async Task Post_ValidCategoria_ReturnsCreatedCategoria()
        {
            var novo = new Categoria { Nome = "NovaCategoriaTemp", ImagemUrl = "nova.jpg" };
            var post = await _client.PostAsJsonAsync("/categorias", novo);
            post.EnsureSuccessStatusCode();
            var created = await post.Content.ReadFromJsonAsync<Categoria>();

            try
            {
                created.Should().NotBeNull();
                created.Nome.Should().Be(novo.Nome);
                created.ImagemUrl.Should().Be(novo.ImagemUrl);
            }
            finally
            {
                await DeleteCategoriaAsync(created.CategoriaId);
            }
        }

        [Fact]
        public async Task Put_ValidCategoria_ReturnsUpdatedCategoria()
        {
            var novo = new Categoria { Nome = "ParaAtualizarTemp", ImagemUrl = "img.jpg" };
            var post = await _client.PostAsJsonAsync("/categorias", novo);
            post.EnsureSuccessStatusCode();
            var created = await post.Content.ReadFromJsonAsync<Categoria>();

            try
            {
                created.Nome = "AtualizadaTemp";
                var put = await _client.PutAsJsonAsync($"/categorias/{created.CategoriaId}", created);
                put.EnsureSuccessStatusCode();

                var updated = await put.Content.ReadFromJsonAsync<Categoria>();
                updated.Should().NotBeNull();
                updated.Nome.Should().Be("AtualizadaTemp");
            }
            finally
            {
                await DeleteCategoriaAsync(created.CategoriaId);
            }
        }

        [Fact]
        public async Task Delete_ExistingCategoria_ReturnsOk()
        {
            var novo = new Categoria { Nome = "ParaDeletarTemp", ImagemUrl = "img.jpg" };
            var post = await _client.PostAsJsonAsync("/categorias", novo);
            post.EnsureSuccessStatusCode();
            var created = await post.Content.ReadFromJsonAsync<Categoria>();

            try
            {
                var delete = await _client.DeleteAsync($"/categorias/{created.CategoriaId}");
                delete.EnsureSuccessStatusCode();
            }
            finally
            {
                await DeleteCategoriaAsync(created.CategoriaId);
            }
        }
    }
}
