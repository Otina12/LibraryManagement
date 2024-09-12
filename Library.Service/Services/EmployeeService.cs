﻿using Library.Model.Abstractions;
using Library.Model.Abstractions.Errors;
using Library.Model.Interfaces;
using Library.Model.Models;
using Library.Service.Dtos.Employee.Get;
using Library.Service.Dtos.Employee.Post;
using Library.Service.Helpers.Extensions;
using Library.Service.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Library.Service.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<Employee> _userManager;

    public EmployeeService(IUnitOfWork unitOfWork, UserManager<Employee> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync()
    {
        var employees = await _unitOfWork.Employees.GetAll(trackChanges: false);
        var employeeDtos = employees.Select(x => x.MapToEmployeeDto());
        return employeeDtos;
    }

    public async Task<Result<EmployeeDto>> GetEmployeeByIdAsync(string employeeId)
    {
        var employee = await _unitOfWork.Employees.GetById(new Guid(employeeId));

        if (employee is null)
        {
            return Result.Failure<EmployeeDto>(Error<Employee>.NotFound);
        }

        var employeeDto = employee.MapToEmployeeDto();
        return Result.Success(employeeDto);
    }

    public async Task<Result<IEnumerable<string>>> GetAllRolesOfEmployee(string employeeId)
    {
        var employee = await _unitOfWork.Employees.GetById(new Guid(employeeId));

        if (employee is null)
        {
            return Result.Failure<IEnumerable<string>>(Error<Employee>.NotFound);
        }

        var roles = await _userManager.GetRolesAsync(employee);

        return Result.Success<IEnumerable<string>>(roles);
    }

    public async Task<Result> UpdateEmployeeAsync(string employeeId, UpdateEmployeeDto updateEmployeeDto)
    {
        var employee = await _unitOfWork.Employees.GetById(new Guid(employeeId));

        if (employee is null)
        {
            return Result.Failure(Error<Employee>.NotFound);
        }

        employee.UpdateDate = DateTime.UtcNow;
        employee.Name = updateEmployeeDto.Name;
        employee.Surname = updateEmployeeDto.Surname;
        employee.UserName = updateEmployeeDto.Username;
        employee.Email = updateEmployeeDto.Email;
        employee.PhoneNumber = updateEmployeeDto.PhoneNumber;

        _unitOfWork.Employees.Update(employee);
        return Result.Success();
    }

    public async Task<Result> DeactivateEmployeeAsync(string employeeId)
    {
        var employee = await _unitOfWork.Employees.GetById(new Guid(employeeId));

        if (employee is null)
        {
            return Result.Failure(Error<Employee>.NotFound);
        }

        employee.DeleteDate = DateTime.UtcNow;
        _unitOfWork.Employees.Update(employee);

        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> ReactivateEmployeeAsync(string employeeId)
    {
        var employee = await _unitOfWork.Employees.GetById(new Guid(employeeId));

        if (employee is null)
        {
            return Result.Failure(Error<Employee>.NotFound);
        }

        employee.DeleteDate = null;
        _unitOfWork.Employees.Update(employee);

        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> AddRolesAsync(string employeeId, string[] roles)
    {
        var employee = await _unitOfWork.Employees.GetById(new Guid(employeeId));

        if (employee is null)
        {
            return Result.Failure(Error<Employee>.NotFound);
        }

        return await AddRolesAsync(employee, roles);
    }


    public async Task<Result> AddRolesAsync(Employee employee, string[] roles)
    {
        foreach (var role in roles)
        {
            if (!await _userManager.IsInRoleAsync(employee, role))
            {
                await _userManager.AddToRoleAsync(employee, role);
            }
        }

        return Result.Success();
    }

    public async Task<Result> RemoveRolesAsync(string employeeId, string[] roles)
    {
        var employee = await _unitOfWork.Employees.GetById(new Guid(employeeId));

        if (employee is null)
        {
            return Result.Failure(Error<Employee>.NotFound);
        }

        return await RemoveRolesAsync(employee, roles);
    }

    public async Task<Result> RemoveRolesAsync(Employee employee, string[] roles)
    {
        foreach (var role in roles)
        {
            if (await _userManager.IsInRoleAsync(employee, role))
            {
                _unitOfWork.Detach(employee);
                await _userManager.RemoveFromRoleAsync(employee, role);
            }
        }

        return Result.Success();
    }

    public async Task<Result> UpdateRolesAsync(string employeeId, string[] newRoles)
    {
        var employee = await _unitOfWork.Employees.GetById(new Guid(employeeId));

        if (employee is null)
        {
            return Result.Failure(Error<Employee>.NotFound);
        }

        _unitOfWork.Detach(employee); // bug fix needed
        var oldRoles = await _userManager.GetRolesAsync(employee);

        return await UpdateRolesAsync(employee, oldRoles.ToArray(), newRoles);
    }

    private async Task<Result> UpdateRolesAsync(Employee employee, string[] oldRoles, string[] newRoles)
    {
        // these are old roles that do not appear in new roles, meaning we need to delete them
        var rolesToRemove = oldRoles.Except(newRoles).ToArray();

        // new roles that were not in the old roles
        var rolesToAdd = newRoles.Except(oldRoles).ToArray();

        await RemoveRolesAsync(employee, rolesToRemove);
        await AddRolesAsync(employee, rolesToAdd);

        return Result.Success();
    }
}
