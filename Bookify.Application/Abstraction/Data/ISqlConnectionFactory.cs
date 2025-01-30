using System.Data;

namespace Bookify.Application.Abstraction.Data;

public interface ISqlConnectionFactory
{

    IDbConnection CreateConnection();
}