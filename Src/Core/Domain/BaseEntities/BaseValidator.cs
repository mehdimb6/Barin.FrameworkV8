using Barin.Framework.Domain.ModelContracts;
using FluentValidation;

namespace Barin.Framework.Domain.BaseEntities;

public class BaseValidator<TKey, TOut> : AbstractValidator<TKey> where TKey : IModel<TOut>, IValidatorModel
{
    public BaseValidator()
    {
        if (typeof(TOut) == typeof(Guid))
            RuleFor(entity => entity.Id).NotEmpty().WithMessage("شناسه نباید خالی باشد.");
    }
}
