using System.Data;

namespace FRN.Domain._2._1_Interface.DapperInterface
{
    public interface IDatabaseFactory
    {
        IDbConnection GetDbConnection { get; }
    }
}
