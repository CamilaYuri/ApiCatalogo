using APICatalogo.Controllers;
using APICatalogo.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalogoxUnitTests.UnitTests;

public class PutProdutoUnitTests : IClassFixture<ProdutosUnitTestController>
{
    private readonly ProdutosController _controller;

    public PutProdutoUnitTests(ProdutosUnitTestController controller)
    {
        _controller = new ProdutosController(controller.context);
    }

    [Fact]
    public async Task PutProduto_Return_OkResult()
    {
        var produtoId = 6;

        var produto = new Produto
        {
            ProdutoId = produtoId,
            Nome = "Produto Atualizado",
            Descricao = "Descrição do Novo Atualizado",
            ImagemUrl = "imagem1.jpg",
            CategoriaId = 2,
        };

        var result = await _controller.Put(produtoId, produto) as ActionResult<Produto>;

        result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task PutProduto_Return_BadRequestResult()
    {
        var produtoId = 1000;

        var produto = new Produto
        {
            ProdutoId = 99,
            Nome = "Produto Atualizado",
            Descricao = "Descrição do Novo Atualizado",
            ImagemUrl = "imagem1.jpg",
            CategoriaId = 2,
        };

        var data = await _controller.Put(produtoId, produto) as ActionResult<Produto>;

        data.Result.Should().BeOfType<BadRequestResult>().Which.StatusCode.Should().Be(400);
    }
}

