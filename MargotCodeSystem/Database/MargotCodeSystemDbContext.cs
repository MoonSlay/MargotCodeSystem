﻿using MargotCodeSystem.Database.DbModels;
using MargotCodeSystem.Database.DbModels.ResidentModels;
using MargotCodeSystem.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MargotCodeSystem.Database
{
    public class MargotCodeSystemDbContext : IdentityDbContext
    {
        public MargotCodeSystemDbContext()
        {

        }

        public MargotCodeSystemDbContext(DbContextOptions<MargotCodeSystemDbContext> options): base (options)
        {

        }

        public DbSet<ApplicationUser> Tbl_ApplicationUsers { get; set; }
        public DbSet<HouseOccupantModel> Tbl_HouseOccupants { get; set; }
        public DbSet<ResidentModel> Tbl_Residents { get; set; }
        public DbSet<PetModel> Tbl_Pets { get; set; }
        public DbSet<MedsModel> Tbl_Meds { get; set; }
        public DbSet<EmployeeModel> Tbl_Employee { get; set; }

        public DbSet<DashboardModel> Tbl_Dashboard { get; set; }
        public DbSet<HouseOccupantGroupModel> Tbl_HouseGroup { get; set; }

    }
}
