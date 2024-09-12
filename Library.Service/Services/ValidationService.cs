using Library.Model.Interfaces;
using Library.Service.Interfaces;

namespace Library.Service.Services
{
    /// <inheritdoc />
    public class ValidationService : IValidationService
    {

        public ValidationService()
        {
        }

        public bool BirthdayIsValid(int year, int month, int day)
        {
            if (year < 1850 || year > DateTime.Today.Year) // validate year
                return false;

            if (month < 1 || month > 12) // validate month
                return false;

            if (day < 1 || day > DateTime.DaysInMonth(year, month)) // validate day (including leap years)
                return false;

            return true; // otherwise valid
        }
    }
}
