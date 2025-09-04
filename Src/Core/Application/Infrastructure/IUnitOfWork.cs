namespace Barin.Framework.Application.Infrastructure;

/// <summary>
/// یک واحد کاری شامل چندین ریپازیتوری است . 
/// کار اصلی این کلاس مدیریت تراکنش ها با بانک اطلاعاتی است
/// </summary>
public interface IUnitOfWork : IAsyncDisposable
{
    /// <summary>
    /// با فراخوانی این متد تمامی تراکنش جاری که در جریان است اعمال میشود
    /// در صورتی که یکی از دستورات تراکنش با خطا مواجه شود کل تغییرات لغو (رول بک) می شود 
    /// </summary>
    Task CommitAsync();
}
