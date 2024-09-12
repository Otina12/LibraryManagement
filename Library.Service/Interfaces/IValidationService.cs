namespace Library.Service.Interfaces;

//general validation service for all models

/// <summary>
/// Provides methods to validate various models and return Success result with object or Failure result with corresponding error.
/// </summary>
public interface IValidationService
{
    bool BirthdayIsValid(int day, int month, int year);
}
