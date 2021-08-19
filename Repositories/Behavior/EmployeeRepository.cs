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

        public async Task<List<EmployeeDto>> GetAllAsync()  //get all employees
        {
            return await (from u in _dbContext.Users
                          join e in _dbContext.Employees on u.UserId equals e.UserId
                          where u.isDeleted == false
                          orderby u.UserName
                          select new EmployeeDto
                          {
                              UserId = e.UserId,
                              EmployeeId = e.EmployeeId,
                              AddressId = u.AddressId,
                              CellNumber = u.CellNumber,
                              Email = u.Email,
                              IdNumber = u.IdNumber,
                              Initials = u.Initials,
                              Password = u.Password,
                              Surname = u.Surname,
                              UserName = u.UserName,
                              UserTypeId = u.UserTypeId,
                              CommenceDate = e.CommencementDate,
                              TerminationDate = e.TerminationDate,
                              TerminationReason = e.TerminationReason,
                              Image = e.Image,
                              Document = e.Document

                          }).ToListAsync();
        }

        public async Task<EmployeeDto> GetByIdAsync(int employeeId)  //get employee by ID
        {
            return await (from u in _dbContext.Users
                          join e in _dbContext.Employees on u.UserId equals e.UserId
                          where u.isDeleted == false && e.EmployeeId == employeeId
                          orderby u.UserName
                          select new EmployeeDto
                          {
                              UserId = e.UserId,
                              EmployeeId = e.EmployeeId,
                              AddressId = u.AddressId,
                              CellNumber = u.CellNumber,
                              Email = u.Email,
                              IdNumber = u.IdNumber,
                              Initials = u.Initials,
                              Password = u.Password,
                              Surname = u.Surname,
                              UserName = u.UserName,
                              UserTypeId = u.UserTypeId,
                              CommenceDate = e.CommencementDate,
                              TerminationDate = e.TerminationDate,
                              TerminationReason = e.TerminationReason,
                              Image = e.Image,
                              Document = e.Document
                          }).SingleOrDefaultAsync();
        }

        public async Task<List<EmployeeDto>> SearchByNameAsync(string name)  //search employee name
        {
            return await (from u in _dbContext.Users
                          join e in _dbContext.Employees on u.UserId equals e.UserId
                          where u.isDeleted == false && (string.IsNullOrEmpty(name) || u.UserName.StartsWith(name))
                          orderby u.UserName
                          select new EmployeeDto
                          {
                              UserId = e.UserId,
                              EmployeeId = e.EmployeeId,
                              AddressId = u.AddressId,
                              CellNumber = u.CellNumber,
                              Email = u.Email,
                              IdNumber = u.IdNumber,
                              Initials = u.Initials,
                              Password = u.Password,
                              Surname = u.Surname,
                              UserName = u.UserName,
                              UserTypeId = u.UserTypeId,
                              CommenceDate = e.CommencementDate,
                              TerminationDate = e.TerminationDate,
                              TerminationReason = e.TerminationReason,
                              Image = e.Image,
                              Document = e.Document
                          }).ToListAsync();
        }

        public async Task<List<EmployeeAttendance>> GetEmployeeMonthlyAttendanceReport(string dateInfo)  //emp attendance report
        {
            var date = DateTime.Parse(dateInfo);

            var employees = await (from e in _dbContext.Employees
                                   join u in _dbContext.Users on e.UserId equals u.UserId
                                   join et in _dbContext.EmployeeTeams on e.EmployeeId equals et.EmployeeId
                                   join t in _dbContext.Teams on et.TeamId equals t.TeamId
                                   where u.isDeleted == false
                                   select new
                                   {
                                       Initials = u.Initials,
                                       Surname = u.Surname,
                                       EmployeeId = e.EmployeeId,
                                       UserName = u.UserName,
                                       UserId = u.UserId,
                                       TeamId = et.TeamId,
                                       TeamName = t.TeamName
                                   }).ToListAsync();
            var attendance = await (from a in _dbContext.AttendanceEntities
                                    join ah in _dbContext.AttendanceHistoryEntities on a.AttendanceHistoryId equals ah.AttendanceHistoryId
                                    where ah.Date.Date.Month == date.Date.Month && ah.Date.Date.Year == date.Date.Year
                                    select new
                                    {
                                        EmployeeId = a.EmployeeId,
                                        Present = a.Present,
                                        Absent = a.Absent,
                                    }).ToListAsync();

            List<EmployeeAttendance> attendanceHistory = new List<EmployeeAttendance>();

            foreach (var employee in employees)
            {
                int daysCount = 0;
                foreach (var att in attendance)
                {
                    if (att.Present == true && att.EmployeeId == employee.EmployeeId)
                    {
                        daysCount += 1;
                    }
                }
                attendanceHistory.Add(new EmployeeAttendance
                {
                    initial = employee.Initials,
                    surname = employee.Surname,
                    Attendance = daysCount,
                    TeamName = employee.TeamName
                });
            }
            return await Task.FromResult(attendanceHistory);
        }

        public async Task<List<RegistrationDetail>> GetEmployeeSixMonthRegistrationReport() // six month registration report
        {

            var mons = new Dictionary<int, int>();
            for (int i = 0; i < 6; i++)
            {
                if ((DateTime.Now.Date.Month - i) >= 0)
                    mons.Add(DateTime.Now.Date.Month - i, DateTime.Now.Date.Year);
                else
                    mons.Add(12 - i, DateTime.Now.Date.Year - 1);

            }
            var months = await _dbContext.Months.Where(month => mons.Keys.Contains(month.MonthId)).ToListAsync();

            List<RegistrationDetail> registeredEmployee = new List<RegistrationDetail>();

            foreach (var month in mons)
            {
                var emp = await (from e in _dbContext.Employees
                                 join u in _dbContext.Users on e.UserId equals u.UserId
                                 where (e.CommencementDate.Value.Date.Month == month.Key && e.CommencementDate.Value.Date.Year == month.Value)
                                 select new RegisteredEmployeeDto
                                 {
                                     Initials = u.Initials,
                                     Surname = u.Surname,
                                     Email = u.Email,
                                     IdNumber = u.IdNumber
                                 }).ToListAsync();
                registeredEmployee.Add(new RegistrationDetail { Employees = emp ?? new List<RegisteredEmployeeDto>(), MonthId = month.Key, MonthName = months.Where(m => m.MonthId == month.Key).SingleOrDefault().MonthName, Year = month.Value.ToString() });

            }


            return await Task.FromResult(registeredEmployee);
        }

        public Task<UpdateEmployeeDto> Update(UpdateEmployeeDto employee)
        {
            var user = (from u in _dbContext.Users where u.UserId == employee.UserId && u.isDeleted == false select u).SingleOrDefault();
            var emp = (from e in _dbContext.Employees where e.UserId == employee.UserId && user.isDeleted == false select e).SingleOrDefault();

            user.Email = employee.Email;
            user.Surname = employee.Surname;
            user.Initials = employee.Initials;

            _dbContext.Update(user);
            _dbContext.SaveChanges();

            emp.Image = employee.Image;
            _dbContext.Update(emp);
            _dbContext.SaveChanges();

            return Task.FromResult(employee);
        }


    }






}

