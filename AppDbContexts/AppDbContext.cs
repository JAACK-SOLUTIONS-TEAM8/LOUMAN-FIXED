using Louman.Models.DTOs;
using Louman.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.AppDbContexts
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<AddressEntity> Addresses { get; set; }
        public DbSet<AdminEntity> Admins { get; set; }
        public DbSet<AttendanceEntity> AttendanceEntities { get; set; }
        public DbSet<AttendanceHistoryEntity> AttendanceHistoryEntities { get; set; }
        public DbSet<ClientEntity> Clients { get; set; }
        public DbSet<DayEntity> Days { get; set; }
        public DbSet<DiscountEntity> Discounts { get; set; }
        public DbSet<EmployeeEntity> Employees { get; set; }
        public DbSet<EmployeeTeamEntity> EmployeeTeams { get; set; }
        public DbSet<EnquiryEntity> Enquiries { get; set; }
        public DbSet<EnquiryResponseEntity> EnquiryResponses { get; set; }
        public DbSet<EnquiryResponseStatusEntity> EnquiryResponseStatus{ get; set; }
        public DbSet<EnquiryTypeEntity> EnquiryTypes { get; set; }
        public DbSet<InvoiceEntity> Invoices{ get; set; }
        public DbSet<LocationEntity> Locations { get; set; }
        public DbSet<MeetingEntity> Meetings{ get; set; }
        public DbSet<MeetingStatusEntity> MeetingStatus { get; set; }
        public DbSet<OrderEntity> Orders{ get; set; }
        public DbSet<OrderLineEntity> OrderLines{ get; set; }
        public DbSet<OrderStatusEntity> OrderStatus { get; set; }
        public DbSet<PaymentEntity> Payments { get; set; }
        public DbSet<PaymentStatusEntity> PaymentStatus { get; set; }
        public DbSet<PaymentTypeEntity> PaymentTypes { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<ProductSizeEntity> ProductSizes { get; set; }
        public DbSet<ProductTypeEntity> ProductTypes{ get; set; }
        public DbSet<SlotEntity> Slots{ get; set; }
        public DbSet<StockEntity> Stocks { get; set; }
        public DbSet<TeamEntity> Teams { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<UserTypeEntity> UserTypes { get; set; }

        public DbSet<BookedSlotEntity> BookedSlots { get; set; }
        public DbSet<DeliveryTypeEntity> DeliveryTypes { get; set; }
        public DbSet<OrderBillEntity> OrderBills { get; set; }
        public DbSet<CardDetailEntity> CardDetails { get; set; }
        public DbSet<AuditEntity> Audits { get; set; }
        public DbSet<TeamDaysEntity> TeamDays { get; set; }
        public DbSet<MonthEntity> Months { get; set; }

        public DbSet<TimerConfigEntity> Timer { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
