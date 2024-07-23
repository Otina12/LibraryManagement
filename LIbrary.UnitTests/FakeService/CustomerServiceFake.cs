using Library.Model.Abstractions;
using Library.Model.Models;
using Library.Service.Dtos;
using Library.Service.Dtos.Customers.Get;
using Library.Service.Dtos.Customers.Post;
using Library.Service.Interfaces;
using Library.UnitTests.MockModels;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.UnitTests.FakeService
{
    public class CustomerServiceFake : ICustomerService
    {
        private readonly List<Customer> _customers;
        private readonly Mock<ICustomerService> _mockService;
        private readonly CustomerFaker _customerFaker;

        public CustomerServiceFake()
        {
            _customers = new List<Customer>();
            _mockService = new Mock<ICustomerService>();
            _customerFaker = new CustomerFaker();
            SetupMockMethods();
        }

        private void SetupMockMethods()
        {
            _mockService.Setup(s => s.Create(It.IsAny<CreateCustomerDto>()))
                .Returns((CreateCustomerDto dto) => Task.FromResult(CreateCustomer(dto)));

            _mockService.Setup(s => s.Deactivate(It.IsAny<string>()))
                .Returns((string id) => Task.FromResult(DeactivateCustomer(id)));

            _mockService.Setup(s => s.GetAllFilteredCustomers(It.IsAny<EntityFiltersDto<CustomerDto>>()))
                .Returns((EntityFiltersDto<CustomerDto> filters) => GetFilteredCustomers(filters));

            _mockService.Setup(s => s.GetCustomerById(It.IsAny<string>()))
                .Returns((string id) => Task.FromResult(GetCustomer(id)));

            _mockService.Setup(s => s.Reactivate(It.IsAny<string>()))
                .Returns((string id) => Task.FromResult(ReactivateCustomer(id)));

            _mockService.Setup(s => s.Update(It.IsAny<CustomerDto>()))
                .Returns((CustomerDto dto) => Task.FromResult(UpdateCustomer(dto)));
        }

        public Task<Result> Create(CreateCustomerDto createCustomerDto)
        {
            return _mockService.Object.Create(createCustomerDto);
        }

        public Task<Result<Customer>> Deactivate(string stringId)
        {
            return _mockService.Object.Deactivate(stringId);
        }

        public EntityFiltersDto<CustomerDto> GetAllFilteredCustomers(EntityFiltersDto<CustomerDto> customerFilters)
        {
            return _mockService.Object.GetAllFilteredCustomers(customerFilters);
        }

        public Task<Result<CustomerDto>> GetCustomerById(string Id)
        {
            return _mockService.Object.GetCustomerById(Id);
        }

        public Task<Result<Customer>> Reactivate(string stringId)
        {
            return _mockService.Object.Reactivate(stringId);
        }

        public Task<Result> Update(CustomerDto customerDto)
        {
            return _mockService.Object.Update(customerDto);
        }

        private Result CreateCustomer(CreateCustomerDto dto)
        {
            var customer = _customerFaker.Generate();
            customer.Name = dto.Name;
            customer.Surname = dto.Surname;
            customer.Email = dto.Email;
            customer.PhoneNumber = dto.PhoneNumber;
            customer.Address = dto.Address;
            _customers.Add(customer);
            return Result.Success();
        }

        private Result<Customer> DeactivateCustomer(string id)
        {
            var customer = _customers.FirstOrDefault(c => c.Id == id);
            if (customer == null)
                return Result.Failure(Error<Customer>.NotFound);

            _customers.Remove(customer);
            return Result.Success(customer);
        }

        private EntityFiltersDto<CustomerDto> GetFilteredCustomers(EntityFiltersDto<CustomerDto> filters)
        {
            var query = _customers.AsQueryable();

            // fake apply filters

            var filteredCustomers = query.Select(c => new CustomerDto(
                c.Id,
                c.Name,
                c.Surname,
                c.Email,
                c.PhoneNumber,
                c.Address,
                DateTime.Now,
                false
            )).ToList();

            return new EntityFiltersDto<CustomerDto>
            {
                Entities = filteredCustomers,
                TotalItems = filteredCustomers.Count
            };
        }

        private Result<CustomerDto> GetCustomer(string id)
        {
            var customer = _customers.FirstOrDefault(c => c.Id == id);
            if (customer == null)
                return Result.Failure(Error<CustomerDto>.NotFound);

            var dto = new CustomerDto(
                customer.Id,
                customer.Name,
                customer.Surname,
                customer.Email,
                customer.PhoneNumber,
                customer.Address,
                DateTime.UtcNow,
                false
            );

            return Result.Success(dto);
        }

        private Result<Customer> ReactivateCustomer(string id)
        {
            var customer = _customers.FirstOrDefault(c => c.Id == id);
            if (customer == null)
                return Result.Failure(Error<Customer>.NotFound);

            return Result.Success(customer);
        }

        private Result UpdateCustomer(CustomerDto dto)
        {
            var customer = _customers.FirstOrDefault(c => c.Id == dto.Id);
            if (customer == null)
                return Result.Failure(Error<CustomerDto>.NotFound);

            customer.Name = dto.Name;
            customer.Surname = dto.Surname;
            customer.Email = dto.Email;
            customer.PhoneNumber = dto.PhoneNumber;
            customer.Address = dto.Address;

            return Result.Success();
        }

        public Task<Result<Customer>> Reactivate(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Result<Customer>> Deactivate(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}