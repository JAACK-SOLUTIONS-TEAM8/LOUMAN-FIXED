using Louman.AppDbContexts;
using Louman.Models.DTOs.Employee;
using Louman.Models.DTOs.Team;
using Louman.Models.Entities;
using Louman.Repositories.Abstraction;
using Louman.Utilities;
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
        public async Task<EmployeeDto> Add(UserEmployee ep)
        {
            var employee = ep.Employee;
            if (employee.UserId == 0)
            {
                var user = new UserEntity
                {
                    UserName = employee.UserName,
                    AddressId = employee.AddressId,
                    CellNumber = employee.CellNumber,
                    Email = employee.Email,
                    IdNumber = employee.IdNumber,
                    Name = employee.Initials,
                    Password = Hashing.GenerateSha512String(employee.Password),
                    Surname = employee.Surname,
                    UserTypeId = employee.UserTypeId,
                    isDeleted = false,
                    EmailConfirmationCode = null,
                    TokenExpirationTime = null,

                };
                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();



                

                var newEmployee = new EmployeeEntity
                {
                    UserId = user.UserId,
                    CommencementDate = employee.CommenceDate.HasValue ? employee.CommenceDate.Value : null,
                    TerminationDate = employee.TerminationDate.HasValue ? employee.TerminationDate.Value : null,
                    TerminationReason = employee.TerminationReason ?? null,
                    Image = employee.Image,
                    EmployeeDocument = employee.Document
                };

                _dbContext.Employees.Add(newEmployee);
                _dbContext.SaveChanges();
                var auditEntity = new AuditEntity
                {
                    Date = DateTime.Now,
                    UserId = ep.UserId,
                    Operation = $"Employee :{employee.Initials} ${employee.Surname} is added to the system"
                };

                await _dbContext.Audits.AddAsync(auditEntity);
                await _dbContext.SaveChangesAsync();

                return await Task.FromResult(new EmployeeDto
                {
                    EmployeeId = newEmployee.EmployeeId,
                    UserId = user.UserId,
                    AddressId = user.AddressId,
                    CellNumber = user.CellNumber,
                    Email = user.Email,
                    IdNumber = user.IdNumber,
                    Initials = user.Name,
                    Password = Hashing.GenerateSha512String(employee.Password),
                    Surname = user.Surname,
                    UserName = user.UserName,
                    UserTypeId = user.UserTypeId,
                    CommenceDate = newEmployee.CommencementDate ?? null,
                    TerminationReason = newEmployee.TerminationReason ?? null,
                    TerminationDate = newEmployee.TerminationDate ?? null,
                    Image = employee.Image,
                    Document = employee.Document
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
                    user.Password = Hashing.GenerateSha512String(employee.Password);
                    user.Surname = employee.Surname;
                    user.UserTypeId = employee.UserTypeId;
                    user.Name = employee.Initials;
                    user.IdNumber = employee.IdNumber;
                    user.AddressId = employee.AddressId;

                    _dbContext.Update(user);
                    _dbContext.SaveChanges();

                    var emp = (from e in _dbContext.Employees where e.EmployeeId == employee.EmployeeId select e).SingleOrDefault();

                    emp.CommencementDate = employee.CommenceDate.HasValue ? employee.CommenceDate.Value : null;
                    emp.TerminationDate = employee.TerminationDate.HasValue ? employee.TerminationDate.Value : null;
                    emp.TerminationReason = employee.TerminationReason ?? null;
                    emp.Image = employee.Image;
                    emp.EmployeeDocument = employee.Document;


                    _dbContext.Update(emp);
                    await _dbContext.SaveChangesAsync();

                    var auditEntity = new AuditEntity
                    {
                        Date = DateTime.Now,
                        UserId = ep.UserId,
                        Operation = $"Employee :{employee.Initials} ${employee.Surname} is added to the system"
                    };

                    await _dbContext.Audits.AddAsync(auditEntity);
                    await _dbContext.SaveChangesAsync();

                    return await Task.FromResult(new EmployeeDto
                    {
                        EmployeeId = emp.EmployeeId,
                        UserId = user.UserId,
                        AddressId = user.AddressId,
                        CellNumber = user.CellNumber,
                        Email = user.Email,
                        IdNumber = user.IdNumber,
                        Initials = user.Name,
                        Password = Hashing.GenerateSha512String(employee.Password),
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

        public  Task<List<EmployeeDto>> GetAllAsync()  //get all employees
        {
            var employees= (from u in _dbContext.Users
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
                              Initials = u.Name,
                              Password = u.Password,
                              Surname = u.Surname,
                              UserName = u.UserName,
                              UserTypeId = u.UserTypeId,
                              CommenceDate = e.CommencementDate,
                              TerminationDate = e.TerminationDate,
                              TerminationReason = e.TerminationReason,
                              Image = e.Image,
                              Document = e.EmployeeDocument

                          }).ToList();
            return Task.FromResult(employees);
        }

        public async Task<EmployeeDto> GetByIdAsync(int employeeId)  //get employee by ID
        {

            var team = await (from et in _dbContext.EmployeeTeams
                              join t in _dbContext.Teams on et.TeamId equals t.TeamId
                              where et.EmployeeId == employeeId
                              select
new
{
 TeamId = t.TeamId,
 TeamName = t.TeamName
}).SingleOrDefaultAsync();

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
                              Initials = u.Name,
                              Password = u.Password,
                              Surname = u.Surname,
                              UserName = u.UserName,
                              UserTypeId = u.UserTypeId,
                              CommenceDate = e.CommencementDate,
                              TerminationDate = e.TerminationDate,
                              TerminationReason = e.TerminationReason,
                              Image = e.Image,
                              Document = e.EmployeeDocument
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
                              Initials = u.Name,
                              Password = u.Password,
                              Surname = u.Surname,
                              UserName = u.UserName,
                              UserTypeId = u.UserTypeId,
                              CommenceDate = e.CommencementDate,
                              TerminationDate = e.TerminationDate,
                              TerminationReason = e.TerminationReason,
                              Image = e.Image,
                              Document = e.EmployeeDocument
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
                                       Initials = u.Name,
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
                                     Initials = u.Name,
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
            user.Name = employee.Initials;

            _dbContext.Update(user);
            _dbContext.SaveChanges();

            emp.Image = employee.Image;
            _dbContext.Update(emp);
            _dbContext.SaveChanges();

            return Task.FromResult(employee);
        }

        public async Task<EmployeeDto> GetByUserIdAsync(int userId)
        {
            var team = await (from et in _dbContext.EmployeeTeams
                              join t in _dbContext.Teams on et.TeamId equals t.TeamId
                              join e in _dbContext.Employees on et.EmployeeId equals e.EmployeeId
                              where e.UserId == userId
                              select
new
{
    TeamId = t.TeamId,
    TeamName = t.TeamName
}).SingleOrDefaultAsync();

            return await (from u in _dbContext.Users
                          join e in _dbContext.Employees on u.UserId equals e.UserId
                          where u.isDeleted == false && e.UserId == userId
                          orderby u.UserName
                          select new EmployeeDto
                          {
                              UserId = e.UserId,
                              EmployeeId = e.EmployeeId,
                              AddressId = u.AddressId,
                              CellNumber = u.CellNumber,
                              Email = u.Email,
                              IdNumber = u.IdNumber,
                              Initials = u.Name,
                              Password = u.Password,
                              Surname = u.Surname,
                              UserName = u.UserName,
                              UserTypeId = u.UserTypeId,
                              CommenceDate = e.CommencementDate,
                              TerminationDate = e.TerminationDate,
                              TerminationReason = e.TerminationReason,
                              Image = e.Image,
                              Document = e.EmployeeDocument,
                              TeamId = team != null ? team.TeamId : 0,
                              TeamName = team != null ? team.TeamName : null
                          }).SingleOrDefaultAsync();
        }




    }



}

