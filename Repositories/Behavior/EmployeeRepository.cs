using Louman.AppDbContexts;
using Louman.Models.DTOs.Employee;
using Louman.Models.DTOs.Team;
using Louman.Models.Entities;
using Louman.Repositories.Abstraction;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _dbContext;

        public EmployeeRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<EmployeeDto> Add(EmployeeDto employee)
        {
            if (employee.UserId == 0)
            {
                var user = new UserEntity
                {
                    UserName = employee.UserName,
                    AddressId = employee.AddressId,
                    CellNumber = employee.CellNumber,
                    Email = employee.Email,
                    IdNumber = employee.IdNumber,
                    Initials = employee.Initials,
                    Password = employee.Password,
                    Surname = employee.Surname,
                    UserTypeId = employee.UserTypeId,
                    isDeleted = false
                };
                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();

                var newEmployee = new EmployeeEntity
                {
                    UserId = user.UserId,
                     CommencementDate=employee.CommenceDate.HasValue ? employee.CommenceDate.Value : null,
                     TerminationDate =employee.TerminationDate.HasValue?employee.TerminationDate.Value:null,
                     TerminationReason=employee.TerminationReason??null,
                     Image=employee.Image,
                     Document=employee.Document
                };

                _dbContext.Employees.Add(newEmployee);
                _dbContext.SaveChanges();
                return await Task.FromResult( new EmployeeDto
                {
                    EmployeeId = newEmployee.EmployeeId,
                    UserId = user.UserId,
                    AddressId = user.AddressId,
                    CellNumber = user.CellNumber,
                    Email = user.Email,
                    IdNumber = user.IdNumber,
                    Initials = user.Initials,
                    Password = user.Password,
                    Surname = user.Surname,
                    UserName = user.UserName,
                    UserTypeId = user.UserTypeId,
                    CommenceDate = newEmployee.CommencementDate?? null,
                    TerminationReason =newEmployee.TerminationReason??null,
                    TerminationDate=newEmployee.TerminationDate??null,
                    //added upload documents/image
                    Image = employee.Image,
                    Document=employee.Document
                    

                });

            }
            else
            {

                var user = (from u in _dbContext.Users where u.UserId == employee.UserId && u.isDeleted == false select u).SingleOrDefault();
                if (user != null)
                {
                    user.UserName = employee.UserName;
                    user.CellNumber = employee.CellNumber;
                    user.Email = employee.Email;
                    user.Password = employee.Password;
                    user.Surname = employee.Surname;
                    user.UserTypeId = employee.UserTypeId;
                    user.Initials = employee.Initials;
                    user.IdNumber = employee.IdNumber;
                    user.AddressId = employee.AddressId;

                    _dbContext.Update(user);
                    _dbContext.SaveChanges();

                    var emp = (from e in _dbContext.Employees where e.EmployeeId == employee.EmployeeId select e).SingleOrDefault();

                    emp.CommencementDate = employee.CommenceDate.HasValue ? employee.CommenceDate.Value : null;
                    emp.TerminationDate = employee.TerminationDate.HasValue ? employee.TerminationDate.Value : null;
                    emp.TerminationReason = employee.TerminationReason ?? null;
                    emp.Image = employee.Image;
                    emp.Document = employee.Document;


                    _dbContext.Update(emp);
                    await _dbContext.SaveChangesAsync();

                    return await Task.FromResult(new EmployeeDto
                    {
                        EmployeeId = emp.EmployeeId,
                        UserId = user.UserId,
                        AddressId = user.AddressId,
                        CellNumber = user.CellNumber,
                        Email = user.Email,
                        IdNumber = user.IdNumber,
                        Initials = user.Initials,
                        Password = user.Password,
                        Surname = user.Surname,
                        UserName = user.UserName,
                        UserTypeId = user.UserTypeId,
                        CommenceDate = employee.CommenceDate,
                        TerminationDate = employee.TerminationDate,
                        TerminationReason = employee.TerminationReason,
                        Image = employee.Image,
                        Document = employee.Document

                    });
                }
            }
            return await Task.FromResult(new EmployeeDto());

        }
        public async Task<bool> DeleteAsync(int employeeUserId)  //delete employee
        {
            var user = _dbContext.Users.Find(employeeUserId);
            if (user != null)
            {
                user.isDeleted = true;
                _dbContext.Users.Update(user);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }


    }

        


    }

