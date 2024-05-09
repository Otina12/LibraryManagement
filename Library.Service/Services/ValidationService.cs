using Library.Service.Interfaces;

namespace Library.Service.Services
{
    public class ValidationService : IValidationService
    {
        public bool BirthdayIsValid(int year, int month, int day)
        {
            if(year <= 1850 || year >= DateTime.Today.Year || // validate once more
               month < 0|| month > 12 || day < 1 || day > 31)
                return false;

            if (month == 2) // special case for february 
                return day <= 29;

            if(day == 31) // check if day 31 was inputed for month with 30 days
                return !(month == 4 || // April
                    month == 6 ||      // June
                    month == 9 ||      // September
                    month == 11);      // November

            return true; // otherwise valid
        }
    }
}
