 using System.Data;

namespace Barin.Framework.Repository.Dapper;

/// <summary>
/// این ریپازیتوری مدیریت تراکنشهای جاری را بر عهده
/// دارد و ارتباط با دیتابیس را مدیریت می کند
/// </summary>
public abstract class RepositoryBase
{
    protected IDbTransaction Transaction { get; private set; }
    protected IDbConnection Connection => Transaction.Connection;

    protected RepositoryBase(IDbTransaction transaction)
    {
        Transaction = transaction;
    }
}
