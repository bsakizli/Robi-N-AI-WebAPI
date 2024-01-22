using EmptorUtility;
using ExtendedXmlSerializer.ExtensionModel;
using Hangfire;
using Hangfire.Dashboard;
using HangfireBasicAuthenticationFilter;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Robi_N_WebAPI.Authentication.basic;
using Robi_N_WebAPI.Model;
using Robi_N_WebAPI.Services;
using Robi_N_WebAPI.Utility;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using Swashbuckle.Examples;
using Swashbuckle.Swagger;
using System.Collections;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using Robi_N_WebAPI.BackgroundJob.Schedules;
using Google.Api;
using DocumentFormat.OpenXml.InkML;
using Hangfire.SqlServer;

var builder = WebApplication.CreateBuilder(args);




// Add services to the container.

builder.Services.AddAuthentication(opt =>
{
    //default config
    opt.DefaultScheme = "UNKNOWN";
    opt.DefaultChallengeScheme = "UNKNOWN";
})
.AddJwtBearer("keycloak", options =>
{
    // first auth method (validate keycloak token with publicKey)
    options.RequireHttpsMetadata = false;

    var publicKey = new ReadOnlySpan<byte>(Convert.FromBase64String(builder.Configuration["Jwt:Key"]));
    var rsa = RSA.Create();
    int readByte = 0;
    rsa.ImportSubjectPublicKeyInfo(publicKey, out readByte);

   


	options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? String.Empty))

        //ValidateIssuer = true,
        //ValidateAudience = true,
        //ValidateLifetime = true,
        //ValidateIssuerSigningKey = true,
        //ValidIssuer = builder.Configuration["Jwt:Issuer"],
        //ValidAudience = builder.Configuration["Jwt:Audience"],
        //ValidAlgorithms = new List<string>() { "RS256" },
        //AuthenticationType = "JWT",
        ////IssuerSigningKey = new RsaSecurityKey(rsa)
        //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? String.Empty))
    };

    //options.Events = new JwtBearerEvents()
    //{
    //    OnMessageReceived = context =>
    //    {
    //        var Token = context.Request.Headers["Authorization"].ToString();
    //        context.Token = Token;
    //        return Task.CompletedTask;
    //    },
    //    OnAuthenticationFailed = context =>
    //    {
    //        return Task.CompletedTask;
    //    },
    //    OnChallenge = context =>
    //    {
    //        return Task.CompletedTask;
    //    },
    //    OnTokenValidated = context =>
    //    {
    //        return Task.CompletedTask;
    //    }
    //};

    options.Validate();

})
.AddJwtBearer("Bearer", options =>
{
    //secound auth method (validate jwt token with securityKey ) 
    options.RequireHttpsMetadata = false;

    var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(builder.Configuration["Jwt:Key"]));

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        // ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        AuthenticationType = "JWT",
        IssuerSigningKey = securityKey,

    };

    options.Events = new JwtBearerEvents()
    {
        OnMessageReceived = context =>
        {
            //var Token = context.Request.Headers["Authorization"].ToString();
            //context.Token = Token;
            return Task.CompletedTask;
        },
        OnAuthenticationFailed = context =>
        {
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            return Task.CompletedTask;
        }
    };

    options.Validate();
})
.AddScheme<AuthenticationSchemeOptions, basicAuthenticationHandler>("basic", null)//therd auth method (validate basic auth)
.AddPolicyScheme("UNKNOWN", "UNKNOWN", options =>
{
    options.ForwardDefaultSelector = context =>
    {

        string authorization = context.Request.Headers[HeaderNames.Authorization];
        {

            if (authorization != null)
            {
                var tt = authorization.Contains("Bearer");

                if (authorization.Contains("Bearer"))
                {
                    return "Bearer";
                }
            }

            //var jwtHandler = new JwtSecurityTokenHandler();
            //if (jwtHandler.CanReadToken(authorization))
            //{
            //    var issuer = jwtHandler.ReadJwtToken(authorization).Issuer;
            //    if (issuer == builder.Configuration["Jwt:Issuer"])
            //    {
            //        return "Bearer";
            //    }

            //}
        }

        return "basic";
    };
});


builder.Services.AddSwaggerGen(c =>
{



    c.SwaggerDoc("v2", new OpenApiInfo()
    {
        Contact = new OpenApiContact
        {
            Name = "Barış Sakızlı",
            Email = "baris.sakizli@bdh.com.tr"
        },
        TermsOfService = new Uri("https://www.bdh.com.tr/"),
        Title = "Robi-N AI API",
        Version = "v2",
        Description = "AI-powered custom solution services"
    });

    c.EnableAnnotations();



    c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
    {
        Description = "Basic auth added to authorization header",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "basic",
        Type = SecuritySchemeType.Http
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "basic" }
                },
                new string[] { }
        }
    });




    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
         new OpenApiSecurityScheme
         {
           Reference = new OpenApiReference
           {
             Type = ReferenceType.SecurityScheme,
             Id = "Bearer"
           }
          },
          new string[] { }
        }
    });

    //c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    //{
    //    Description = "Please insert JWT with pas Bearer into field",
    //    Name = "Authorization",
    //    In = ParameterLocation.Header,
    //    Scheme = "Bearer",
    //    Type = SecuritySchemeType.ApiKey,
    //    BearerFormat = "JWT"
    //});
    //c.AddSecurityRequirement(new OpenApiSecurityRequirement
    //{
    //    {
    //        new OpenApiSecurityScheme
    //        {
    //            Reference = new OpenApiReference{Type = ReferenceType.SecurityScheme, Id = "Bearer"}
    //        },
    //        new string[] { }
    //    }
    //});

});

//builder.Services.AddAuthentication("basicAuthentication")
//.AddScheme<AuthenticationSchemeOptions, basicAuthenticationHandler>("basicAuthentication", null);

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
//{
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        ValidIssuer = builder.Configuration["Jwt:Issuer"],
//        ValidAudience = builder.Configuration["Jwt:Audience"],
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? String.Empty))
//    };
//}).AddScheme<AuthenticationSchemeOptions, basicAuthenticationHandler>("basicAuthentication", null);




builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddRouting(options => options.LowercaseUrls = true);

//builder.Services.AddControllers();



//Hangfire
//Hangfire
builder.Services.AddHangfire(x => x.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHangfireServer();


builder.Services.AddControllers(options =>
{
    options.RespectBrowserAcceptHeader = true;
    options.InputFormatters.Add(new XmlSerializerInputFormatter(options));
    options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
});

builder.Services.AddControllers().AddXmlSerializerFormatters();
builder.Services.AddMvc().AddXmlSerializerFormatters();

builder.Services.AddMvc().AddXmlDataContractSerializerFormatters();


//builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<AIServiceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);



//builder.Services.AddDbContext<EmptorDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("EmptorConnection"))
//);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

#region Test



//builder.Services.AddSwaggerGen();
//builder.Services.AddSwaggerGen(options =>
//{
//    options.SwaggerDoc("v2", new Microsoft.OpenApi.Models.OpenApiInfo
//    {
//        Title = "Robi-N AI API",
//        Version = "v2",
//        Description = "AI-powered custom solution services",
//    });

//    options.AddSecurityDefinition("basic", new OpenApiSecurityScheme
//    {
//        Description = "basic auth added to authorization header",
//        Name = "Authorization",
//        In = ParameterLocation.Header,
//        Scheme = "basic",
//        Type = SecuritySchemeType.Http
//    });
//    options.AddSecurityRequirement(new OpenApiSecurityRequirement
//    {
//        {
//            new OpenApiSecurityScheme
//            {
//                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "basic" }
//            },
//            new string[] { }
//        }
//    });

//    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//    {
//        In = ParameterLocation.Header,
//        Scheme = "Bearer",
//        BearerFormat = "JWT",
//        Description = "Please insert JWT with Bearer into field",
//        Name = "Authorization",
//        Type = SecuritySchemeType.ApiKey
//    });


//    //options.AddSecurityDefinition("pas", new OpenApiSecurityScheme
//    //{
//    //    Description = "Please insert JWT with pas Bearer into field",
//    //    Name = "Authorization",
//    //    In = ParameterLocation.Header,
//    //    Scheme = "pas",
//    //    Type = SecuritySchemeType.ApiKey,
//    //    BearerFormat = "JWT"
//    //});
//    options.AddSecurityRequirement(new OpenApiSecurityRequirement
//    {
//        {
//            new OpenApiSecurityScheme
//            {
//                Reference = new OpenApiReference{Type = ReferenceType.SecurityScheme, Id = "Bearer"}
//            },
//            new string[] { }
//        }
//    });






//    //options.AddSecurityDefinition("keycloak", new OpenApiSecurityScheme
//    //{
//    //    Description = "Please insert JWT with keycloak Bearer into field",
//    //    Name = "Authorization",
//    //    In = ParameterLocation.Header,
//    //    Scheme = "keycloak",
//    //    Type = SecuritySchemeType.ApiKey,
//    //    BearerFormat = "JWT"
//    //});
//    //options.AddSecurityRequirement(new OpenApiSecurityRequirement
//    //{
//    //    {
//    //        new OpenApiSecurityScheme
//    //        {
//    //            Reference = new OpenApiReference{Type = ReferenceType.SecurityScheme, Id = "keycloak"}
//    //        },
//    //        new string[] { }
//    //    }
//    //});


//    //options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//    //{
//    //    In = ParameterLocation.Header,
//    //    Scheme = "Bearer",
//    //    BearerFormat = "JWT",
//    //    Description = "Please insert JWT with Bearer into field",
//    //    Name = "Authorization",
//    //    Type = SecuritySchemeType.ApiKey
//    //});

//    //options.AddSecurityRequirement(new OpenApiSecurityRequirement {
//    //{
//    // new OpenApiSecurityScheme
//    // {
//    //   Reference = new OpenApiReference
//    //   {
//    //     Type = ReferenceType.SecurityScheme,
//    //     Id = "Bearer"
//    //   }
//    //  },
//    //  new string[] { }
//    //}
//});


//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v2", new OpenApiInfo() { Version = "V2", Title = "Sepehr Customer Club" });

//    c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
//    {
//        Description = "basic auth added to authorization header",
//        Name = "Authorization",
//        In = ParameterLocation.Header,
//        Scheme = "basic",
//        Type = SecuritySchemeType.Http
//    });
//    c.AddSecurityRequirement(new OpenApiSecurityRequirement
//{
//    {
//        new OpenApiSecurityScheme
//        {
//            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "basic" }
//        },
//        new string[] { }
//    }
//});

//    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//    {
//        In = ParameterLocation.Header,
//        Scheme = "Bearer",
//        BearerFormat = "JWT",
//        Description = "Please insert JWT with Bearer into field",
//        Name = "Authorization",
//        Type = SecuritySchemeType.ApiKey
//    });
//    c.AddSecurityRequirement(new OpenApiSecurityRequirement
//{
//    {
//        new OpenApiSecurityScheme
//        {
//            Reference = new OpenApiReference{Type = ReferenceType.SecurityScheme, Id = "Bearer"}
//        },
//        new string[] { }
//    }
//});
//});



//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo { Title = "basicAuth", Version = "v1" });
//    c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
//    {
//        Name = "Authorization",
//        Type = SecuritySchemeType.Http,
//        Scheme = "basic",
//        In = ParameterLocation.Header,
//        Description = "basic Authorization header using the Bearer scheme."
//    });
//    c.AddSecurityRequirement(new OpenApiSecurityRequirement
//                {
//                    {
//                          new OpenApiSecurityScheme
//                            {
//                                Reference = new OpenApiReference
//                                {
//                                    Type = ReferenceType.SecurityScheme,
//                                    Id = "basic"
//                                }
//                            },
//                            new string[] {}
//                    }
//                });
//});

//Log.Logger = new LoggerConfiguration()
//    .ReadFrom.Configuration(builder.Configuration)
//    .CreateLogger();


#endregion



Log.Logger = new LoggerConfiguration()
    .WriteTo
    .MSSqlServer(
        connectionString: builder.Configuration["ConnectionStrings:DefaultConnection"] /*"Server=localhost;Database=LogDb;Integrated Security=SSPI;"*/,
        sinkOptions: new MSSqlServerSinkOptions { TableName = "Logs" })
    .CreateLogger();


builder.Host.UseSerilog();


builder.Services.AddFastReport();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddCors(p => p.AddPolicy("corspolicy", build =>
{
    build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
})
);

//builder.Services.AddCors();

//builder.Services.AddControllersWithViews(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true).AddRazorRuntimeCompilation();
builder.Services.AddControllersWithViews(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseSerilogRequestLogging();

app.UseStatusCodePages(async context =>
{
    if (context.HttpContext.Response.StatusCode == 401)
    {

        context.HttpContext.Response.ContentType = "application/json";
        await context.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(new
        {
            status = 401,
            description = "Unauthorized credentials were used."
        }));

        //JsonResult result = new JsonResult(new { msg = "Some example message." });

        // await context.HttpContext.Response.WriteAsync("Custom Unauthorized request");
    }

    if (context.HttpContext.Response.StatusCode == 403)
    {

        context.HttpContext.Response.ContentType = "application/json";
        await context.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(new
        {
            status = 403,
            displayMessage = "Yetkisiz İşlem",
            description = "Unauthorized transaction."
        }));

        //JsonResult result = new JsonResult(new { msg = "Some example message." });

        // await context.HttpContext.Response.WriteAsync("Custom Unauthorized request");
    }
});

app.UseSwagger();
//app.UseSwaggerUI();
//app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v2/swagger.json", "PlaceInfo Services"));

app.UseSwaggerUI(c =>
{
    string swaggerJsonPath = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "..";
    c.SwaggerEndpoint($"{swaggerJsonPath}/swagger/v2/swagger.json", "Robin Web API");
});


app.UseCors("corspolicy");

app.UseStaticFiles();
app.UseRouting();
//app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.UseHangfireServer(new BackgroundJobServerOptions());

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    DashboardTitle = "Robi-N Zamanlayıcı",
    IsReadOnlyFunc = (DashboardContext context) => true,
    //Authorization = new[] { new MyAuthorizationFilter() }
    Authorization = new[]
        {
                new HangfireCustomBasicAuthenticationFilter{
                    User = "bdh",
                    Pass = "Bdhpass1!"
                }
	}
});

GlobalConfiguration.Configuration
				.UseColouredConsoleLogProvider()
				.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
				.UseSimpleAssemblyNameTypeSerializer()
				.UseRecommendedSerializerSettings()
				.UseResultsInContinuations()
				.UseSqlServerStorage(@builder.Configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
				{
				
					CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
					QueuePollInterval = TimeSpan.Zero,
					SlidingInvisibilityTimeout = TimeSpan.FromMinutes(1),
					UseRecommendedIsolationLevel = true,
					UsePageLocksOnDequeue = true,
					DisableGlobalLocks = true,
					EnableHeavyMigrations = true
				});



RecurringJobs.AutomaticTicketClosedOperation();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseFastReport();
app.Run();
