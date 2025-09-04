using Barin.Framework.Application.Infrastructure;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace Barin.Framework.Repository.Dapper;

public abstract class UnitOfWork : IUnitOfWork
{
    private bool _disposed;

    protected DbConnection Connection { get; private set; }
    protected DbTransaction Transaction { get; private set; }
    public Guid UnitOfWorkId { get; private set; }

    protected UnitOfWork(string connectionString)
    {
        UnitOfWorkId = Guid.NewGuid();
        Connection = new SqlConnection(connectionString);

        if (Connection.State == ConnectionState.Closed)
            Connection.OpenAsync().GetAwaiter().GetResult();

        Transaction = Connection.BeginTransactionAsync().GetAwaiter().GetResult();
    }

    public async Task CommitAsync()
    {
        try
        {
            await Transaction.CommitAsync();
        }
        catch
        {
            await Transaction.RollbackAsync();
            throw;
        }
        finally
        {
            await Transaction.DisposeAsync();
            Transaction = await Connection.BeginTransactionAsync();

            ResetRepositories();
        }
    }

    protected abstract void ResetRepositories();

    public async ValueTask DisposeAsync()
    {
        await DisposeAsync(true);
        GC.SuppressFinalize(this);
    }

    private async Task DisposeAsync(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
        {
            if (Transaction != null)
                await Transaction.DisposeAsync();

            if (Connection != null)
                await Connection.DisposeAsync();
        }

        _disposed = true;
    }
}