using ApiCatalogo.IntegrationTests.Factories;
using APICatalogo.Models;
using FluentAssertions;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using APICatalogo.Context;

namespace ApiCatalogo.IntegrationTests.ControllersTests
{
    public class CategoriaControllerTests : IClassFixture<ApiCatalogoFactory>
    {
        private readonly HttpClient _client;
        private readonly ApiCatalogoFactory _factory;

        public CategoriaControllerTests(ApiCatalogoFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        private async Task DeleteCategoriaAsync(int id)
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var appDb = scopedServices.GetRequiredService<AppDbContext>();

                var categoria = await appDb.Categorias.FindAsync(id);
                if (categoria != null)
                {
                    appDb.Categorias.Remove(categoria);
                    await appDb.SaveChangesAsync();
                }
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
