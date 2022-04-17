using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HRMS.Models
{
    public partial class HRMSContext : DbContext
    {
        public HRMSContext()
        {
        }

        public HRMSContext(DbContextOptions<HRMSContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Attendance> Attendances { get; set; } = null!;
        public virtual DbSet<Employee> Employees { get; set; } = null!;
        public virtual DbSet<Group> Groups { get; set; } = null!;
        public virtual DbSet<Hliday> Hlidays { get; set; } = null!;
        public virtual DbSet<Setting> Settings { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Attendance>(entity =>
            {
                entity.HasKey(e => new { e.EmpId, e.Day });

                entity.ToTable("attendances");

                entity.Property(e => e.EmpId)
                    .HasMaxLength(100)
                    .HasColumnName("emp_id");

                entity.Property(e => e.Day)
                    .HasColumnType("date")
                    .HasColumnName("day");

                entity.Property(e => e.AttendingTime).HasColumnName("attendingTime");

                entity.Property(e => e.LeavingTime).HasColumnName("leavingTime");

                entity.HasOne(d => d.Emp)
                    .WithMany(p => p.Attendances)
                    .HasForeignKey(d => d.EmpId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_attendance_For_Employee");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(100)
                    .HasColumnName("id");

                entity.Property(e => e.Address)
                    .HasMaxLength(100)
                    .HasColumnName("address");

                entity.Property(e => e.ArrivalTime).HasColumnName("arrivalTime");

                entity.Property(e => e.ContractDate)
                    .HasColumnType("date")
                    .HasColumnName("contractDate");

                entity.Property(e => e.DOb)
                    .HasColumnType("date")
                    .HasColumnName("dOB");

                entity.Property(e => e.Extrahour).HasColumnName("extrahour");

                entity.Property(e => e.FullName)
                    .HasMaxLength(90)
                    .HasColumnName("fullName");

                entity.Property(e => e.Gender).HasColumnName("gender");

                entity.Property(e => e.LeavingTime).HasColumnName("leavingTime");

                entity.Property(e => e.NationalId)
                    .HasMaxLength(14)
                    .HasColumnName("nationalId")
                    .IsFixedLength();

                entity.Property(e => e.Nationality)
                    .HasMaxLength(30)
                    .HasColumnName("nationality");

                entity.Property(e => e.Phone)
                    .HasMaxLength(11)
                    .HasColumnName("phone")
                    .IsFixedLength();

                entity.Property(e => e.Salary).HasColumnType("money");
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.CreateAttendance).HasColumnName("createAttendance");

                entity.Property(e => e.CreateEmp).HasColumnName("createEmp");

                entity.Property(e => e.CreateSettings).HasColumnName("createSettings");

                entity.Property(e => e.DeleteAttendance).HasColumnName("deleteAttendance");

                entity.Property(e => e.DeleteEmp).HasColumnName("deleteEmp");

                entity.Property(e => e.DeleteSettings).HasColumnName("deleteSettings");

                entity.Property(e => e.GroupName)
                    .HasMaxLength(35)
                    .HasColumnName("groupName");

                entity.Property(e => e.ReadAttendance).HasColumnName("readAttendance");

                entity.Property(e => e.ReadEmp).HasColumnName("readEmp");

                entity.Property(e => e.ReadReport).HasColumnName("readReport");

                entity.Property(e => e.ReadSettings).HasColumnName("readSettings");

                entity.Property(e => e.UpdateAttendance).HasColumnName("updateAttendance");

                entity.Property(e => e.UpdateEmp).HasColumnName("updateEmp");

                entity.Property(e => e.UpdateSettings).HasColumnName("updateSettings");
            });

            modelBuilder.Entity<Hliday>(entity =>
            {
                entity.HasKey(e => new { e.Name, e.Date })
                    .HasName("PK_hlidays_1");

                entity.ToTable("hlidays");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");
            });

            modelBuilder.Entity<Setting>(entity =>
            {
                entity.HasKey(e => new { e.WeekEnd1, e.PenaltyHour })
                    .HasName("PK_Settings_1");

                entity.Property(e => e.WeekEnd1)
                    .HasMaxLength(10)
                    .HasColumnName("weekEnd1");

                entity.Property(e => e.PenaltyHour).HasColumnName("penaltyHour");

                entity.Property(e => e.ExtraHour).HasColumnName("extraHour");

                entity.Property(e => e.WeekEnd2)
                    .HasMaxLength(10)
                    .HasColumnName("weekEnd2");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Email);

                entity.Property(e => e.Email)
                    .HasMaxLength(80)
                    .HasColumnName("email");

                entity.Property(e => e.EmpId)
                    .HasMaxLength(100)
                    .HasColumnName("empId");

                entity.Property(e => e.GrpId).HasColumnName("grpId");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .HasColumnName("password");

                entity.Property(e => e.UserName)
                    .HasMaxLength(55)
                    .HasColumnName("userName");

                entity.HasOne(d => d.Emp)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.EmpId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_isEmployee");

                entity.HasOne(d => d.Grp)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.GrpId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_in_Group");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
