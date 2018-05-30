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
            //GravarUsandoAdoNet();
            //GravarUsandoEntity();
            //RecuperaProdutos();
            //ExcluirProdutos();
            //RecuperaProdutos();
            AtualizarProduto();

        }

        private static void AtualizarProduto()
        {
            // inclui um produto
            GravarUsandoEntity();
            RecuperaProdutos();

            // atualiza o produto
            using (var repo = new ProdutoDAOEntity())
            {
                Produto primeiro = repo.Produtos().First();
                primeiro.Nome = "Cassiono Royale - Editado";
                repo.Atualizar(primeiro);
                
            }
            RecuperaProdutos();
        }

        private static void ExcluirProdutos()
        {
            using (var repo = new ProdutoDAOEntity())

            {
                IList<Produto> produtos = repo.Produtos();
                foreach (var item in produtos)
                {
                    repo.Remover(item);
                }
                
            }
        }

        private static void RecuperaProdutos()
        {
            using (var repo = new ProdutoDAOEntity())

            {
                IList<Produto> produtos = repo.Produtos();
                Console.WriteLine("foram encontrados {0} produto(s)", produtos.Count);
                foreach (var item in produtos)
                {
                    Console.WriteLine(item.Nome);
                }
            }
        }

        private static void GravarUsandoEntity()
        {
            Produto p = new Produto();
            p.Nome = "Harry Potter e a Ordem da Fênix";
            p.Categoria = "Livros";
            p.Preco = 19.89;

            using (var repo = new ProdutoDAOEntity())

            {
                repo.Adicionar(p); // Usando a variavel contexto para chamar a prop Produtos + Add
                
            }
        }

        private static void GravarUsandoAdoNet()
        {
            Produto p = new Produto();
            p.Nome = "Harry Potter e a Ordem da Fênix";
            p.Categoria = "Livros";
            p.Preco = 19.89;

            using (var repo = new ProdutoDAO())
            {
                repo.Adicionar(p);
            }
        }
    }
}
