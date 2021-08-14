﻿using Louman.AppDbContexts;
using Louman.Models.DTOs;
using Louman.Models.Entities;
using Louman.Repositories.Abstraction;
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
                    Initials = admin.Initials,
                    Password = admin.Password,
                    Surname = admin.Surname,
                    UserTypeId = admin.UserTypeId,
                    isDeleted = false
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
                    Initials = user.Initials,
                    Password = user.Password,
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
                    user.Password = admin.Password;
                    user.Surname = admin.Surname;
                    user.UserTypeId = admin.UserTypeId;
                    user.Initials = admin.Initials;
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
                        Initials = user.Initials,
                        Password = user.Password,
                        Surname = user.Surname,
                        UserName = user.UserName,
                        UserTypeId = user.UserTypeId
                    };
                }
            }
            return new AdminDto();
            
        }

      

        
    }
}