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
        Task<List<EmployeeDto>> GetAllAsync();
        Task<EmployeeDto> GetByIdAsync(int employeeId);
        Task<List<EmployeeDto>> SearchByNameAsync(string name);
        Task<bool> DeleteAsync(int employeeUserId);
        Task<List<EmployeeAttendance>> GetEmployeeMonthlyAttendanceReport(string dateInfo);
        Task<List<RegistrationDetail>> GetEmployeeSixMonthRegistrationReport();



    }
}
