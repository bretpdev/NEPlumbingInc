namespace NEPlumbingInc.Validation;

public sealed class RequiredPhoneNumberAttribute : ValidationAttribute
{
    public int MinDigits { get; init; } = 10;
    public int MaxDigits { get; init; } = 15;

    public RequiredPhoneNumberAttribute()
    {
        ErrorMessage = "Please enter a valid phone number";
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not string phone || string.IsNullOrWhiteSpace(phone))
        {
            return new ValidationResult(ErrorMessage);
        }

        var digitCount = 0;

        foreach (var ch in phone)
        {
            if (char.IsDigit(ch))
            {
                digitCount++;
                continue;
            }

            if (ch is ' ' or '(' or ')' or '-' or '.' or '+')
            {
                continue;
            }

            return new ValidationResult(ErrorMessage);
        }

        if (digitCount < MinDigits || digitCount > MaxDigits)
        {
            return new ValidationResult(ErrorMessage);
        }

        return ValidationResult.Success;
    }
}
