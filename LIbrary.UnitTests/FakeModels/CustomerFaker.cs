using Bogus;
using Library.Model.Models;

namespace Library.UnitTests.MockModels
{
    public class CustomerFaker : Faker<Customer>
    {
        public CustomerFaker()
        {
            RuleFor(c => c.Id, f => f.Random.AlphaNumeric(11));
            RuleFor(c => c.Name, f => f.Name.FirstName());
            RuleFor(c => c.Surname, f => f.Name.LastName());
            RuleFor(c => c.Email, (f, c) => f.Internet.Email(c.Name, c.Surname));
            RuleFor(c => c.PhoneNumber, f => f.Phone.PhoneNumber());
            RuleFor(c => c.Address, f => f.Address.FullAddress());
        }
    }
}