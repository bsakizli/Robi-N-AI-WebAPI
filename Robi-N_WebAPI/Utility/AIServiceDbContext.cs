﻿using Microsoft.EntityFrameworkCore;
using Robi_N_WebAPI.Model;
using Robi_N_WebAPI.Utility.Tables;

namespace Robi_N_WebAPI.Utility
{
    public class AIServiceDbContext : DbContext
    {
        public AIServiceDbContext(DbContextOptions<AIServiceDbContext> options) : base(options) { }

        public DbSet<ApiUsers> RBN_AI_SERVICE_USERS { get; set; }  
        public DbSet<RBN_AI_SERVICE_ROLES_MAP> RBN_AI_SERVICE_ROLES_MAP { get; set; }  
        public DbSet<RBN_AI_SERVICE_ROLE> RBN_AI_SERVICE_ROLE { get; set; }  
    }
}
