using FluentValidation;
using MSAF.App.Functions.SmokeTest;

namespace MSAF.App.Functions.Functions.API.SmokeTest
{
    public class SmoketestRequestValidator : AbstractValidator<SmokeTestValidatorRequestModel>
    {
        public SmoketestRequestValidator()
        {
            RuleFor(x => x.StringVal).NotEmpty().MinimumLength(1);
            RuleFor(x => x.IntVal).GreaterThanOrEqualTo(10);
            RuleFor(x => x.DateVal).GreaterThan(DateTimeOffset.Now);
        }
    }
}
