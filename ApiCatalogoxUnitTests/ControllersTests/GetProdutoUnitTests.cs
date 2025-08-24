using APICatalogo.Context;
using APICatalogo.Controllers;
using APICatalogo.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalogoxUnitTests.UnitTests;

public class GetProdutoUnitTests : IClassFixture<ProdutosUnitTestController>
{
    private readonly ProdutosController _controller;

    public GetProdutoUnitTests(ProdutosUnitTestController controller)
    {
        _controller = new ProdutosController(controller.Context);
    }

    [Fact]
    public async Task GetProdutoById_OkResult()
    {
        var produtoId = 2;

        var data = await _controller.Get(produtoId);

        data.Result.Should().BeOfType<OkObjectResult>()
            .Which.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task GetProdutoById_NotFound()
    {
        var produtoId = 999;

        var data = await _controller.Get(produtoId);

        data.Result.Should().BeOfType<NotFoundObjectResult>()
            .Which.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task GetProdutoById_BadRequest()
    {
        var produtoId = -1;

        var data = await _controller.Get(produtoId);

        data.Result.Should().BeOfType<BadRequestObjectResult>()
            .Which.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task GetProdutos_Return_ListOfProduto()
    {
        var data = await _controller.Get();

        data.Result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeAssignableTo<IEnumerable<Produto>>()
            .And.NotBeNull();
    }
}

