using APICatalogo.Controllers;
using APICatalogo.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogoxUnitTests.UnitTests;

public class PutProdutoUnitTests : IClassFixture<ProdutosUnitTestController>
{
    private readonly ProdutosController _controller;
    private readonly ProdutosUnitTestController _fixture;

    public PutProdutoUnitTests(ProdutosUnitTestController controller)
    {
        _fixture = controller;
        _controller = new ProdutosController(_fixture.Context);
    }

    [Fact]
    public async Task PutProduto_Return_OkResult()
    {
        var produtoId = 3;

        var produtoExistente = await _fixture.Context.Produtos.FindAsync(produtoId);
        produtoExistente.Should().NotBeNull();

        _fixture.Context.Entry(produtoExistente).State = EntityState.Detached;

        var produtoAtualizado = new Produto
        {
            ProdutoId = produtoId,
            Nome = "Produto Atualizado",
            Descricao = "Descrição do Novo Atualizado",
            ImagemUrl = "imagem1.jpg",
            Preco = 49.90m,
            CategoriaId = 3
        };

        var result = await _controller.Put(produtoId, produtoAtualizado) as ActionResult<Produto>;

        result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();

        var produtoVerificado = await _fixture.Context.Produtos.FindAsync(produtoId);
        produtoVerificado.Should().NotBeNull();
        produtoVerificado.Nome.Should().Be("Produto Atualizado");
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
