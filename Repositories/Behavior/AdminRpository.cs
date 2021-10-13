using Louman.AppDbContexts;
using Louman.Models.DTOs;
using Louman.Models.DTOs.Timer;
using Louman.Models.DTOs.User;
using Louman.Models.Entities;
using Louman.Repositories.Abstraction;
using Louman.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Repositories
{
    public class AdminRpository : IAdminRepository
    {
        private readonly AppDbContext _dbContext;

        public AdminRpository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public AdminDto Add(UpsertAdminDto admin)
        {
            if(admin.AdminId==0)
            {
                var user = new UserEntity
                {
                    UserName = admin.UserName,
                    AddressId = null,
                    CellNumber = admin.CellNumber,
                    Email = admin.Email,
                    IdNumber = admin.IdNumber,
                    Name = admin.Initials,
                    Password = Hashing.GenerateSha512String(admin.Password),
                    Surname = admin.Surname,
                    UserTypeId = admin.UserTypeId,
                    isDeleted = false,
                    EmailConfirmationCode = null,
                    TokenExpirationTime = null,
                };
                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();

                var newAdmin = new AdminEntity
                {
                    UserId = user.UserId
                };

                _dbContext.Admins.Add(newAdmin);
                _dbContext.SaveChanges();
                return new AdminDto
                {
                    AdminId = newAdmin.AdminId,
                    UserId = user.UserId,
                    AddressId = user.AddressId,
                    CellNumber = user.CellNumber,
                    Email = user.Email,
                    IdNumber = user.IdNumber,
                    Initials = user.Name,
                    Password = Hashing.GenerateSha512String(admin.Password),
                    Surname = user.Surname,
                    UserName = user.UserName,
                    UserTypeId = user.UserTypeId
                };

            }
            else
            {

                var user = (from u in _dbContext.Users where u.UserId == admin.AdminUserId && u.isDeleted == false select u).SingleOrDefault();
                if(user!=null)
                {
                    user.UserName = admin.UserName;
                    user.CellNumber = admin.CellNumber;
                    user.Email = admin.Email;
                    user.Surname = admin.Surname;
                    user.UserTypeId = admin.UserTypeId;
                    user.Name = admin.Initials;
                    _dbContext.Update(user);
                    _dbContext.SaveChanges();

                    return new AdminDto
                    {
                        AdminId = admin.AdminId,
                        UserId = user.UserId,
                        AddressId = user.AddressId,
                        CellNumber = user.CellNumber,
                        Email = user.Email,
                        IdNumber = user.IdNumber,
                        Initials = user.Name,
                        Password = " ",
                        Surname = user.Surname,
                        UserName = user.UserName,
                        UserTypeId = user.UserTypeId
                    };
                }
            }
            return new AdminDto();
            
        }
        public bool Delete(int adminUserId)
        {
            var user = _dbContext.Users.Find(adminUserId);
            if (user != null)
            {
                user.isDeleted = true;
                _dbContext.Users.Update(user);
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }
        public List<AdminDto> GetAll()
        {
            return (from u in _dbContext.Users
                    join a in _dbContext.Admins on u.UserId equals a.UserId
                    where u.isDeleted == false
                    orderby u.UserName
                    select new AdminDto
                    {
                        UserId = a.UserId,
                        AdminId = a.AdminId,
                        AddressId = u.AddressId,
                        CellNumber = u.CellNumber,
                        Email = u.Email,
                        IdNumber = u.IdNumber,
                        Initials = u.Name,
                        Password = u.Password,
                        Surname = u.Surname,
                        UserName = u.UserName,
                        UserTypeId = u.UserTypeId

                    }).ToList();

        }

        public AdminDto GetById(int id)
        {
            return (from u in _dbContext.Users
                    join a in _dbContext.Admins on u.UserId equals a.UserId
                    where u.isDeleted == false && a.AdminId == id
                    orderby u.UserName
                    select new AdminDto
                    {
                        UserId = a.UserId,
                        AdminId = a.AdminId,
                        AddressId = u.AddressId,
                        CellNumber = u.CellNumber,
                        Email = u.Email,
                        IdNumber = u.IdNumber,
                        Initials = u.Name,
                        Password = u.Password,
                        Surname = u.Surname,
                        UserName = u.UserName,
                        UserTypeId = u.UserTypeId

                    }).SingleOrDefault();
        }

        public  TimerConfigEntity GetTimerCongif()
        {
            return _dbContext.Timer.Where(t => t.Id == 1).SingleOrDefault();
        }

        public List<AdminDto> SearchByName(string name)
        {
            return (from u in _dbContext.Users
                    join a in _dbContext.Admins on u.UserId equals a.UserId
                    where u.isDeleted == false && (string.IsNullOrEmpty(name) || u.UserName.StartsWith(name))
                    orderby u.UserName
                    select new AdminDto
                    {
                        UserId = a.UserId,
                        AdminId = a.AdminId,
                        AddressId = u.AddressId,
                        CellNumber = u.CellNumber,
                        Email = u.Email,
                        IdNumber = u.IdNumber,
                        Initials = u.Name,
                        Password = u.Password,
                        Surname = u.Surname,
                        UserName = u.UserName,
                        UserTypeId = u.UserTypeId

                    }).ToList();
        }

        public bool SetTimerConfig(TimerConfigDto config)
        {
            var configEntity = _dbContext.Timer.Find(1);

            configEntity.LeftTime = config.LeftTime;
            configEntity.Notify = config.Notify;
            configEntity.StopTime = config.StopTime;
            _dbContext.Timer.Update(configEntity);
            _dbContext.SaveChanges();
            return true;
        }

        public async Task<RoleDto> AddRole(RoleDto role)
        {
            if (role.RoleId == 0)
            {
                var newRole = new RoleEntity
                {
                    RoleName = role.RoleName
                };
                _dbContext.Roles.Add(newRole);
                await _dbContext.SaveChangesAsync();


                return await Task.FromResult(new RoleDto
                {
                    RoleId = newRole.RoleId,
                    RoleName = newRole.RoleName
                });

            }
            else
            {

                var roleEntity = _dbContext.Roles.Find(role.RoleId);
                if (roleEntity != null)
                {
                    roleEntity.RoleName = role.RoleName;
                    _dbContext.Roles.Update(roleEntity);
                    await _dbContext.SaveChangesAsync();

                    return await Task.FromResult(new RoleDto
                    {
                        RoleId = role.RoleId,
                        RoleName = role.RoleName
                    });
                }
            }
            return new RoleDto();

        }
        public async Task<List<RoleDto>> GetAllRoles()
        {
            return await (from r in _dbContext.Roles
                          orderby r.RoleName
                          select new RoleDto
                          {
                              RoleId = r.RoleId,
                              RoleName = r.RoleName
                          }).ToListAsync();
        }
        public async Task<RoleDto> GetRoleById(int id)
        {
            return await (from r in _dbContext.Roles
                    where r.RoleId==id
                    orderby r.RoleName
                    select new RoleDto
                    {
                        RoleId=r.RoleId,
                        RoleName=r.RoleName

                    }).SingleOrDefaultAsync();
        }


    }
}
