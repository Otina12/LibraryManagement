using AutoMapper;
using Library.Model.Exceptions;
using Library.Model.Interfaces;
using Library.Model.Models;
using Library.Service.Dtos;
using Library.Service.Extensions;
using Library.Service.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Library.Service.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<Employee> _userManager;



    public EmployeeService(IUnitOfWork unitOfWork, UserManager<Employee> userManager, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync()
    {
        var employees = await _unitOfWork.Employees.GetAll();
        var employeeDtos = employees.Select(x => x.MapToEmployeeDto());
        return employeeDtos;
    }

    public async Task<EmployeeDto?> GetEmployeeByIdAsync(string employeeId)
    {
        var employee = await _unitOfWork.Employees.GetById(new Guid(employeeId));
        return employee?.MapToEmployeeDto();
    }

    public async Task<IEnumerable<string>> GetAllRolesOfEmployee(string employeeId)
    {
        var employee = await _unitOfWork.Employees.GetById(new Guid(employeeId));

        if (employee is null)
        {
            throw new EmployeeNotFoundException($"Employee with Id {employeeId} was not found.");
        }

        return await _userManager.GetRolesAsync(employee);
    }

    public async Task UpdateEmployeeAsync(string employeeId, UpdateEmployeeDto updateEmployeeDto)
    {
        var employee = await _unitOfWork.Employees.GetById(new Guid(employeeId));
        if (employee is null)
            throw new EmployeeNotFoundException("Employee was not found");

        employee.UpdateDate = DateTime.UtcNow;
        employee.Name = updateEmployeeDto.Name;
        employee.Surname = updateEmployeeDto.Surname;
        employee.UserName = updateEmployeeDto.Username;
        employee.Email = updateEmployeeDto.Email;
        employee.PhoneNumber = updateEmployeeDto.PhoneNumber;

        _unitOfWork.Employees.Update(employee);
    }

    public async Task DeleteEmployeeAsync(string employeeId)
    {
        var employee = await _unitOfWork.Employees.GetById(new Guid(employeeId));
        if (employee is null)
            throw new EmployeeNotFoundException("Employee was not found");

        employee.DeleteDate = DateTime.UtcNow;
        _unitOfWork.Employees.Update(employee);
    }
}
