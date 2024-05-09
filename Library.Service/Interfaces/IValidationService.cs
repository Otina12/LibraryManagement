namespace Library.Service.Interfaces;

//general validation service for all models
public interface IValidationService
{
    bool BirthdayIsValid(int day, int month, int year);
}
