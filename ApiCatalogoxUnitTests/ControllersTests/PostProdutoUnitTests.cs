using APICatalogo.Controllers;
using APICatalogo.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalogoxUnitTests.UnitTests;

public class PostProdutoUnitTests : IClassFixture<ProdutosUnitTestController>
{
    private readonly ProdutosController _controller;

    public PostProdutoUnitTests(ProdutosUnitTestController controller)
    {
        _controller = new ProdutosController(controller.context);
    }

    [Fact]
    public async Task PostProduto_Return_CreatedStatusCode()
    {
        var novoProduto = new Produto
        {
            Nome = "Novo Produto",
            Descricao = "Descrição do Novo Produto",
            Preco = 10.99m,
            ImagemUrl = "imagemfake1.jpg",
            CategoriaId = 4,
        };

        var data = await _controller.Post(novoProduto);

        var createdResult = data.Result.Should().BeOfType<CreatedAtRouteResult>();
        createdResult.Subject.StatusCode.Should().Be(201);
    }

    [Fact]
    public async Task PostProduto_Return_BadRequest()
    {
        Produto novoProduto = null;

        var data = await _controller.Post(novoProduto);

        var createdResult = data.Result.Should().BeOfType<BadRequestResult>();
        createdResult.Subject.StatusCode.Should().Be(400);
    }
}

