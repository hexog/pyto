using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Pyto.Data;
using Pyto.Data.Todo;
using Pyto.Data.Users;
using Pyto.Services;
using Pyto.Services.Authentication;
using Pyto.Services.Common;
using Pyto.Services.TodoList;
using TodoRepository = Pyto.Data.Todo.TodoRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
	o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Name = "Authorization",
		Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
		In = ParameterLocation.Header,
		Type = SecuritySchemeType.ApiKey,
		Scheme = "Bearer",
	});

	o.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Name = "Bearer",
				In = ParameterLocation.Header,
				Reference = new OpenApiReference
				{
					Id = "Bearer",
					Type = ReferenceType.SecurityScheme,
				},
			},
			new List<string>()
		},
	});
});

builder.Services.AddDbContext<ApplicationDbContext>(
	o => o.UseSqlite(builder.Configuration.GetConnectionString("Sqlite")));
builder.Services.AddIdentity<UserDbo, IdentityRole<Guid>>()
   .AddEntityFrameworkStores<ApplicationDbContext>()
   .AddDefaultTokenProviders();

builder.Services
   .AddControllers()
   .AddJsonOptions(o =>
	{
		o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
	});

builder.Services.AddAuthorization();
builder.Services.AddAuthentication()
   .AddJwtBearer(o =>
	{
		o.SaveToken = true;
		if (builder.Environment.IsDevelopment())
		{
			o.RequireHttpsMetadata = false;
		}

		o.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidAudience = builder.Configuration["Authentication:JWT:ValidAudience"],
			ValidIssuer = builder.Configuration["Authentication:JWT:ValidIssuer"],
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Authentication:JWT:Key"])),
		};
	});

AddApplicationServices(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

DataInitializer.InitializeRoles(app.Services).Wait();

app.Run();

void AddApplicationServices(IServiceCollection serviceCollection)
{
	// Repositories
	serviceCollection
	   .AddScoped<IRefreshTokenRepository, RefreshTokenRepository>()
	   .AddScoped<ITodoRepository, TodoRepository>();

	// Processes
	serviceCollection
	   .AddScoped<RefreshTokenDeleteProcess>()
	   .AddHostedService<TimedProcessRunner<RefreshTokenDeleteProcess>>();

	// Services
	serviceCollection
	   .AddScoped<IAuthenticationService, AuthenticationService>()
	   .AddTransient<ITodoListServiceFactory, TodoListServiceFactory>();
}
