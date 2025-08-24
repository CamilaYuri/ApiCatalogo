using APICatalogo.Controllers;
using APICatalogo.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalogoxUnitTests.UnitTests;

public class DeleteProdutoUnitTests : IClassFixture<ProdutosUnitTestController>
{
    private readonly ProdutosController _controller;

    public DeleteProdutoUnitTests(ProdutosUnitTestController controller)
    {
        _controller = new ProdutosController(controller.Context);
    }

    [Fact]
    public async Task DeleteProduto_Return_OkResult()
    {
        var novoProduto = new Produto
        {
            ProdutoId = 10,
            Nome = "Novo Produto",
            Descricao = "Descrição do Novo Produto",
            Preco = 10.99m,
            ImagemUrl = "imagemfake1.jpg",
            CategoriaId = 4,
        };

        var data = await _controller.Post(novoProduto);

        var createdResult = data.Result.Should().BeOfType<CreatedAtRouteResult>();
        createdResult.Subject.StatusCode.Should().Be(201);

        var result = await _controller.Delete(novoProduto.ProdutoId);

        result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task DeleteProduto_Return_NotFound()
    {
        var produtoId = 1000;

        var result = await _controller.Delete(produtoId) as ActionResult<Produto>;

        result.Should().NotBeNull();
        result.Result.Should().BeOfType<NotFoundObjectResult>();
    }
}

