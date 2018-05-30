using System;
using Microsoft.EntityFrameworkCore; // Usar isso para funcionar o Entity


namespace Alura.Loja.Testes.ConsoleApp
{
    internal class LojaContext : DbContext //Usar a API do Entity
    {
        public DbSet<Produto> Produtos { get; set; } //Qual classe irá persistida pelo Entity
        //Nome da propriedade "Produtos" mesma da tabela do banco de dados
        public DbSet<Compra> Compras { get; set; }
        public DbSet<Promocao> Promocaos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<PromocaoProduto>()
                .HasKey(pp => new { pp.PromocaoId, pp.ProdutoId });

            modelBuilder
                .Entity<Endereco>()
                .ToTable("Enderecos");

            modelBuilder
                .Entity<Endereco>()
                .Property<int>("ClienteId");

            modelBuilder
                .Entity<Endereco>()
                .HasKey("ClienteId");

        }


        public LojaContext() // Está vazio pois vai ser opcional, assim o código continua compilando 
        { }

        public LojaContext(DbContextOptions<LojaContext> options) : base(options) // Outros providers
        {

        }

        //Definir o banco de dados que vou usar
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=LojaDB;Trusted_Connection=true;");
                /* Nome do servidor: Server=(localdb)\MSSQLLocalDB;
                 * Nome do Banco de dados: Database=LojaDB;
                 * Conexão sergura: Trusted_Connection=true;              
                 */
            }

        }
    }
}