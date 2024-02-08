using AutoMapper;
using FRN.Application._1._1_Interface;
using FRN.Domain._2._1_Interface;
using FRN.Domain._2._2_Entity;
using System.Data.SqlClient;

namespace FRN.Application._1._2_AppService
{
    public class ProdutoAppService: BaseService, IProdutoAppService 
    {
        private readonly IProductRepository _produtoRepository;

        public ProdutoAppService(IMapper mapper,
                                 IProductRepository produtoRepository,
                                 IDomainNotificationHandler notificator) : base (notificator, mapper)
        {
            _produtoRepository = produtoRepository;
        }        
       
        public IEnumerable<Product> Get()
        {
            try
            {
                var result = _produtoRepository.Get();
                if (result == null)
                {
                    NotifyError("Não foram encontrados dados na Pesquisa");
                    return null;
                }
                return _mapper.Map<IEnumerable<Product>>(result);
            }
            catch (Exception ex)
            {
                if(ex is SqlException)
                {
                    NotifyError(ex.Message.ToString());
                    return null;
                }
                else
                {
                    NotifyError(ex.Message.ToString());
                    return null;
                }
            }
        }

        public IEnumerable<Product> GetByCod(int codigo)
        {
            try
            {
                var result = _produtoRepository.GetByCod(codigo);
                if (result == null)
                {
                    NotifyError("Não foram encontrados dados na Pesquisa");
                    return null;
                }
                return _mapper.Map<IEnumerable<Product>>(result);
            }
            catch (Exception ex)
            {
                if (ex is SqlException)
                {
                    NotifyError(ex.Message.ToString());
                    return null;
                }
                else
                {
                    NotifyError(ex.Message.ToString());
                    return null;
                }
            }
        }

        public IEnumerable<Product> GetByFilter(string descricao, string dt_fabricacao, string dt_validade)
        {
            try
            {
                var result = _produtoRepository.GetByFilter(descricao, dt_fabricacao, dt_validade);
                if (result == null)
                {
                    NotifyError("Não foram encontrados dados na Pesquisa");
                    return null;
                }
                return _mapper.Map<IEnumerable<Product>>(result);
            }
            catch (Exception ex)
            {
                if (ex is SqlException)
                {
                    NotifyError(ex.Message.ToString());
                    return null;
                }
                else
                {
                    NotifyError(ex.Message.ToString());
                    return null;
                }
            }
        }     

        public void Post(Product product)
        {
            if(product.dt_fabricacao >= product.dt_validade)
            {
                NotifyError("Data de validade não pode ser maior ou igual que a data de fabricação");
            }
            else 
            {
                try
                {
                    _produtoRepository.Post(product);
                }
                catch (Exception ex)
                {
                    if (ex is SqlException)
                    {
                        NotifyError(ex.Message.ToString());
                    }
                    else
                    {
                        NotifyError(ex.Message.ToString());
                    }
                }

            }
            
            
        }

        public void Inactivate(Product product)
        {
            try
            {
                _produtoRepository.Inactivate(product);
            }
            catch (Exception ex)
            {
                if (ex is SqlException)
                {
                    NotifyError(ex.Message.ToString());
                }
                else
                {
                    NotifyError(ex.Message.ToString());
                }
            }
        }

        public void Put(Product product)
        {
            if (product.dt_fabricacao >= product.dt_validade)
            {
                NotifyError("Data de validade não pode ser maior ou igual que a data de fabricação");
            }
            else
            {
                try
                {
                    _produtoRepository.Put(product);
                }
                catch (Exception ex)
                {
                    if (ex is SqlException)
                    {
                        NotifyError(ex.Message.ToString());
                    }
                    else
                    {
                        NotifyError(ex.Message.ToString());
                    }
                }
            }
             
        }
    }
}
