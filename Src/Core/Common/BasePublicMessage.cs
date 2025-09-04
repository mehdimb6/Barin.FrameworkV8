namespace Barin.Framework.Common;

public class BasePublicMessage
{
    protected BasePublicMessage()
    {
    }

    public const string ErrorSystem = "بروز مشکل فنی در انجام عملیات";
    public const string SuccessAction = "عملیات با موفقیت انجام شد";
    public const string UnsuccessAction = "عملیاتی انجام نشد";
    public const string SuccessGet = "اطلاعات با موفقیت دریافت شد";
    public const string SuccessAdd = "{0} با موفقیت ثبت شد";
    public const string SuccessEdit = "{0} با موفقیت اصلاح شد";
    public const string SuccessDelete = "{0} با موفقیت حذف شد";
    public const string NotExist = "موردی یافت نشد";
}