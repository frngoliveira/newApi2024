using FRN.Domain._2._2_Entity;

namespace FRN.Domain._2._1_Interface
{
    public interface IProductRepository
    {
        IEnumerable<Product> Get();
        void Post(Product product);
        void Put(Product product);
        void Inactivate(Product product);
        IEnumerable<Product> GetByCod(int codigo);
        IEnumerable<Product> GetByFilter(string descricao, string dt_fabricacao, string dt_validade);
    }
}
