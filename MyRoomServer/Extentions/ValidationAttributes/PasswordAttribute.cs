using System.ComponentModel.DataAnnotations;

namespace MyRoomServer.Extentions.Validations
{
    public sealed class PasswordAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            var password = value?.ToString();
            if (password == null)
            {
                return false;
            }
            return password.Length >= 6 && password.Length <= 20;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"The field {name} must be set and with the minimum length of 6, the maximum length of 20.";
        }
    }
}
