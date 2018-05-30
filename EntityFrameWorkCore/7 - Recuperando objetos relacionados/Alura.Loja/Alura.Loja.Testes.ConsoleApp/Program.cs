using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alura.Loja.Testes.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            

            using (var contexto = new LojaContext())
            {
                // Um para um
                var cliente = contexto
                    .Clientes
                    .Include(c => c.EnderecoDeEntrega)
                    .FirstOrDefault();

                Console.WriteLine($"Endereço de entrega: {cliente.EnderecoDeEntrega.Logadouro}");

                //Um para muitos explicito
                var produto = contexto
                    .Produtos
                    .Where(p => p.Id == 6002)
                    .FirstOrDefault();

                contexto.Entry(produto)
                    .Collection(p => p.Compras)
                    .Query()
                    .Where(c => c.Preco > 10)
                    .Load();

                Console.WriteLine($"Mostrando as compras do produto {produto.Nome}");
                foreach (var item in produto.Compras)
                {
                    Console.WriteLine("\t" + item);
                }


            }

        }

        private static void CompraPaoUmPraMuitos()
        {
            using (var contexto = new LojaContext())
            {
                var pao = contexto
                    .Produtos
                    .Where(p => p.Id == 6002)
                    .FirstOrDefault();

                var compra = new Compra();
                compra.Quantidade = 50;
                compra.Produto = pao;
                compra.Preco = pao.PrecoUnitario * compra.Quantidade;

                var compra2 = new Compra();
                compra2.Quantidade = 100;
                compra2.Produto = pao;
                compra2.Preco = pao.PrecoUnitario * compra2.Quantidade;

                var compra3 = new Compra();
                compra3.Quantidade = 100;
                compra3.Produto = pao;
                compra3.Preco = pao.PrecoUnitario * compra3.Quantidade;

                contexto.Compras.AddRange(compra, compra2, compra3);



                contexto.SaveChanges();


            }
        }

        private static void ExibeProdutosDaPromocao()
        {
            //Muitos Para Muitos
            using (var contexto2 = new LojaContext())
            {
                var promacao = contexto2
                    .Promocaos
                    .Include(p => p.Produtos)
                    .ThenInclude(pp => pp.Produto)
                    .FirstOrDefault();

                Console.WriteLine("\n Mostrando os produtos da promoção");
                foreach (var item in promacao.Produtos)
                {
                    Console.WriteLine(item.Produto);
                }
            }
        }

        private static void IncluirPromocao()
        {
            using (var contexto = new LojaContext())
            {
                var promocao = new Promocao()
                {
                    Descrição = "Queima Total Janiero 2017",
                    DataInicio = new DateTime(2017, 1, 1),
                    DataTermino = new DateTime(207, 1, 31)
                };

                var produtos = contexto
                    .Produtos
                    .Where(p => p.Categoria == "Bebidas")
                    .ToList();

                foreach (var item in produtos)
                {
                    promocao.IncluirProduto(item);
                }

                contexto.Promocaos.Add(promocao);

                ExibeEntries(contexto.ChangeTracker.Entries());

                contexto.SaveChanges();

            }
        }

        private static void UmParaUm()
        {
            var fulano = new Cliente();
            fulano.Nome = "Fulaninho de tal";
            fulano.EnderecoDeEntrega = new Endereco()
            {
                Numero = 12,
                Logadouro = "Rua dos Inválidos",
                Complemento = "sobrado",
                Bairro = "Centro",
                Cidade = "Cidade"

            };

            using (var contexto = new LojaContext())
            {
                contexto.Clientes.Add(fulano);
                contexto.SaveChanges();
            }
        }

        private  static void MuitosParaMuitos()
        {
            var p1 = new Produto() { Nome = "Suco de Laranja", Categoria = "Bebidas", PrecoUnitario = 8.79, Unidade = "Litros" };
        var p2 = new Produto() { Nome = "Café", Categoria = "Bebidas", PrecoUnitario = 12.45, Unidade = "Gramas" };
        var p3 = new Produto() { Nome = "Macarrão", Categoria = "Alimentos", PrecoUnitario = 4.23, Unidade = "Gramas" };


        var promocaoPascoa = new Promocao();

        promocaoPascoa.Descrição = "Páscoa Feliz";
            promocaoPascoa.DataInicio = DateTime.Now;
            promocaoPascoa.DataTermino = DateTime.Now.AddMonths(3);

            promocaoPascoa.IncluirProduto(p1);
            promocaoPascoa.IncluirProduto(p2);
            promocaoPascoa.IncluirProduto(p3);


            using (var contexto = new LojaContext())
            {
                //contexto.Promocaos.Add(promocaoPascoa);
                var promocao = contexto.Promocaos.Find(1);


        contexto.Promocaos.Remove(promocao);

                
                contexto.SaveChanges();
            }
}
        private static void ExibeEntries(IEnumerable<EntityEntry> enumerable)
        {
            foreach (var e in enumerable)
            {
                Console.WriteLine(e.Entity.ToString() + " - " + e.State);
            }
        }
    }
}
