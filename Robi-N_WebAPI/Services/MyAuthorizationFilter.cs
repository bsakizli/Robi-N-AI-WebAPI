﻿using Hangfire.Dashboard;

namespace Robi_N_WebAPI.Services
{
    public class MyAuthorizationFilter : IDashboardAuthorizationFilter
    {

        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            // Allow all authenticated users to see the Dashboard (potentially dangerous).
            return httpContext.User.Identity?.IsAuthenticated ?? false;
        }
    }
}
