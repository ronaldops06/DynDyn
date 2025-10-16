using System.Data;

namespace Worker.Service.Data.Repository;

public abstract class BaseRepository
{
    protected readonly IDbConnection _connection;

    protected BaseRepository(IDbConnection connection)
    {
        _connection = connection;
    }
}