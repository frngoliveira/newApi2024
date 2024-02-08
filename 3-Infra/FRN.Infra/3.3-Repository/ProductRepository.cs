using Dapper;
using FRN.Domain._2._1_Interface;
using FRN.Domain._2._2_Entity;
using FRN.Infra._3._1_Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Text;


namespace FRN.Infra._3._3_Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IDomainNotificationHandler _notificator;
        public ProductRepository(FRNContext context,
                                 IConfiguration configuration,
                                 IDomainNotificationHandler notificator) : base(context)
        {
            _configuration = configuration;
            _notificator = notificator;
        }

        public IEnumerable<Product> Get()
        {
            StringBuilder errorMessage = new StringBuilder();
            string query = $@"
                    SELECT 
	                    p.cod_produto,
	                    p.descricao, 
	                    p.situacao_produto,
	                    p.dt_fabricacao,
	                    p.dt_validade,
	                    f.descricao_fornecedor,
                        f.cod_fornecedor
                    FROM 
	                    tbl_produto p
                    INNER JOIN 
	                    tbl_fornecedor f
                    ON 
	                    f.cod_fornecedor = p.cod_fornecedor	
                    WHERE 
	                    p.situacao_produto > 0";

            try
            {
                using (SqlConnection cnx = new SqlConnection(
                    _context.Database.GetDbConnection().ConnectionString))
                {
                    var dados = cnx.Query<Product>(query);
                    return dados;
                }
            }
            catch (SqlException Sqlex)
            {
                for (int i = 0; i < Sqlex.Errors.Count; i++)
                {
                    _notificator.Handle(Sqlex.Errors[i].Message);
                }
                return null;
            }
        }

        public IEnumerable<Product> GetByCod(int codigo)
        {
            StringBuilder errorMessage = new StringBuilder();
            string query = $@"
                    SELECT 
	                    p.cod_produto,
	                    p.descricao, 
	                    p.situacao_produto,
	                    p.dt_fabricacao,
	                    p.dt_validade,
	                    f.descricao_fornecedor,
                        f.cod_fornecedor
                    FROM 
	                    tbl_produto p
                    INNER JOIN 
	                    tbl_fornecedor f
                    ON 
	                    f.cod_fornecedor = p.cod_fornecedor	
                    WHERE 
	                    p.cod_produto = {codigo}";

            try
            {
                using (SqlConnection cnx = new SqlConnection(
                    _context.Database.GetDbConnection().ConnectionString))
                {
                    var dados = cnx.Query<Product>(query);
                    return dados;
                }
            }
            catch (SqlException Sqlex)
            {
                for (int i = 0; i < Sqlex.Errors.Count; i++)
                {
                    _notificator.Handle(Sqlex.Errors[i].Message);
                }
                return null;
            }
        }       

        public IEnumerable<Product> GetByFilter(string descricao, string dt_fabricacao, string dt_validade)
        {
            StringBuilder errorMessage = new StringBuilder();
            string query = $@"
                    SELECT 
	                    p.cod_produto, 
	                    p.descricao,
	                    p.situacao_produto,
	                    p.dt_fabricacao,
	                    p.dt_validade,

	                    f.cod_fornecedor,
	                    f.descricao_fornecedor
                    FROM
	                    tbl_produto p WITH(NOLOCK)
                    INNER JOIN
	                    tbl_fornecedor f 
                    ON  
	                    p.cod_fornecedor = f.cod_fornecedor";

            if (!string.IsNullOrWhiteSpace(descricao)) 
            {
                query = query + $@" WHERE p.descricao LIKE '%{descricao}%'";
            }

            if (!string.IsNullOrWhiteSpace(dt_fabricacao))
            {
                query = query + $@" AND p.dt_fabricacao BETWEEN '{dt_fabricacao}' AND '{dt_fabricacao}'";
            }

            if (!string.IsNullOrWhiteSpace(dt_validade))
            {
                query = query + $@" AND p.dt_validade BETWEEN '{dt_validade}' AND '{dt_validade}'";
            }

            try
            {
                using (SqlConnection cnx = new SqlConnection(
                    _context.Database.GetDbConnection().ConnectionString))
                {
                    var dados = cnx.Query<Product>(query);
                    return dados;
                }
            }
            catch (SqlException Sqlex)
            {
                for (int i = 0; i < Sqlex.Errors.Count; i++)
                {
                    _notificator.Handle(Sqlex.Errors[i].Message);
                }
                return null;
            }
        }

        public void Post(Product product)
        {
            StringBuilder errorMessage = new StringBuilder();

            string query = $@"INSERT INTO tbl_produto (descricao, situacao_produto, dt_fabricacao, dt_validade, cod_fornecedor)
                                VALUES 
                   ('{product.descricao}', '{product.situacao_produto}', '{product.dt_fabricacao}', '{product.dt_validade}', '{product.cod_fornecedor}') ";           

            using (SqlConnection cnx = new SqlConnection(
                _context.Database.GetDbConnection().ConnectionString))
            {
                cnx.Open();
                var transacao = cnx.BeginTransaction();

                try
                {
                    cnx.Execute(query, transaction: transacao);
                    transacao.Commit();
                }
                catch (SqlException Sqlex)
                {
                    transacao.Rollback();
                    cnx.Close();

                    for (int i = 0; i < Sqlex.Errors.Count; i++)
                    {
                        errorMessage.Append("Mensagem: " + Sqlex.Errors[i].Message);
                    }
                    throw new Exception(errorMessage.ToString());
                }
                catch(Exception ex)
                {
                    transacao.Rollback();
                    cnx.Close();
                    throw new Exception(ex.Message.ToString());
                }
                cnx.Close();
            }
        }

        public void Put(Product product)
        {
            StringBuilder errorMessage = new StringBuilder();
            
            string query = $@"
                UPDATE 
                    tbl_produto
                SET
                    descricao = '{product.descricao}',
                    situacao_produto = '{product.situacao_produto}',
                    dt_fabricacao = '{product.dt_fabricacao}',
                    dt_validade = '{product.dt_validade}',
                    cod_fornecedor = {product.cod_fornecedor}
                WHERE Cod_produto = {product.cod_produto}";

            using (SqlConnection cnx = new SqlConnection(
                _context.Database.GetDbConnection().ConnectionString))
            {
                cnx.Open();
                var transacao = cnx.BeginTransaction();

                try
                {
                    cnx.Execute(query, transaction: transacao);
                    transacao.Commit();
                }
                catch (SqlException Sqlex)
                {
                    transacao.Rollback();
                    cnx.Close();

                    for (int i = 0; i < Sqlex.Errors.Count; i++)
                    {
                        errorMessage.Append("Mensagem: " + Sqlex.Errors[i].Message);
                    }
                    throw new Exception(errorMessage.ToString());
                }
                catch (Exception ex)
                {
                    transacao.Rollback();
                    cnx.Close();
                    throw new Exception(ex.Message.ToString());
                }
                cnx.Close();
            }
        }

        public void Inactivate(Product product)
        {
            StringBuilder errorMessage = new StringBuilder();

            string query = $@"
                UPDATE 
                    tbl_produto
                SET                    
                    situacao_produto = '{product.situacao_produto}'
                WHERE 
                    cod_produto = '{product.cod_produto}'";

            using (SqlConnection cnx = new SqlConnection(
                _context.Database.GetDbConnection().ConnectionString))
            {
                cnx.Open();
                var transacao = cnx.BeginTransaction();

                try
                {
                    cnx.Execute(query, transaction: transacao);
                    transacao.Commit();
                }
                catch (SqlException Sqlex)
                {
                    transacao.Rollback();
                    cnx.Close();

                    for (int i = 0; i < Sqlex.Errors.Count; i++)
                    {
                        errorMessage.Append("Mensagem: " + Sqlex.Errors[i].Message);
                    }
                    throw new Exception(errorMessage.ToString());
                }
                catch (Exception ex)
                {
                    transacao.Rollback();
                    cnx.Close();
                    throw new Exception(ex.Message.ToString());
                }
                cnx.Close();
            }
        }
    }
}
