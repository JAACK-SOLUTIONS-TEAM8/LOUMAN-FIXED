using Louman.Models.DTOs.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Repositories.Abstraction
{
    public interface IEmployeeRepository
    {
        Task<EmployeeDto> Add(EmployeeDto employee);
        Task<bool> DeleteAsync(int employeeUserId);
        


    }
}
