using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace ApiCatalogoxUnitTests
{
    public class ProdutosUnitTestController
    {
        public AppDbContext Context { get; }

        public ProdutosUnitTestController()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
                .Options;

            Context = new AppDbContext(options);

            Context.Database.EnsureCreated();
            SeedTestData();
        }

        private void SeedTestData()
        {
            Context.Categorias.AddRange(
                new Categoria { CategoriaId = 1, Nome = "Eletrônicos", ImagemUrl = "eletronicos.jpg" },
                new Categoria { CategoriaId = 2, Nome = "Livros", ImagemUrl = "livros.jpg" },
                new Categoria { CategoriaId = 3, Nome = "Brinquedos", ImagemUrl = "brinquedos.jpg" },
                new Categoria { CategoriaId = 4, Nome = "Moda", ImagemUrl = "moda.jpg" }
            );

            Context.Produtos.AddRange(
                new Produto
                {
                    ProdutoId = 1,
                    Nome = "Smartphone X",
                    Descricao = "Smartphone de última geração",
                    ImagemUrl = "smartphone.jpg",
                    Preco = 1999.99m,
                    CategoriaId = 1
                },
                new Produto
                {
                    ProdutoId = 2,
                    Nome = "Livro C# Avançado",
                    Descricao = "Livro completo sobre C#",
                    ImagemUrl = "livro_csharp.jpg",
                    Preco = 99.90m,
                    CategoriaId = 2
                },
                new Produto
                {
                    ProdutoId = 3,
                    Nome = "Carrinho de Brinquedo",
                    Descricao = "Carrinho divertido para crianças",
                    ImagemUrl = "carrinho.jpg",
                    Preco = 49.90m,
                    CategoriaId = 3
                },
                new Produto
                {
                    ProdutoId = 4,
                    Nome = "Vestido",
                    Descricao = "Vestido florido",
                    ImagemUrl = "vestido.jpg",
                    Preco = 69.90m,
                    CategoriaId = 4
                }
            );

            Context.SaveChanges();
        }
    }
}