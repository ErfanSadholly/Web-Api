using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Web_Api.Account_Models;
using Web_Api.AppData;
using Web_Api.DbModels;
using Web_Api.Jwt;
using Web_Api.Validations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
	options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		In = ParameterLocation.Header,
		Description = "Please enter a valid JWT token with Bearer prefix",
		Name = "Authorization",
		Type = SecuritySchemeType.ApiKey,
		BearerFormat = "JWT"
	});

	options.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			new string[] {}
		}
	});
});

//--------------------------------------------------------------------------------------------------------

builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//----------------------------------------------------------------------------------------------------

builder.Services.AddIdentity<User, Role>(Options =>
{
	Options.Password.RequiredLength = 5;
	Options.Password.RequireNonAlphanumeric = false;
	Options.Password.RequiredUniqueChars = 3;
	Options.Password.RequireDigit = true;
	Options.Password.RequireLowercase = true;
	Options.Password.RequireUppercase = true;

	Options.Tokens.AuthenticatorTokenProvider = "JwtBearer";

	Options.User.RequireUniqueEmail = true;

}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

//-----------------------------------------------------------------------------------------------------
// Add Secret.json
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());
builder.Configuration.AddJsonFile("Secret.json", optional: false, reloadOnChange: true);
builder.Services.Configure<JwtModel>(builder.Configuration.GetSection("JwtModel"));

Microsoft.AspNetCore.Authentication.AuthenticationBuilder authenticationBuilder = builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
	var jwtModel = builder.Configuration.GetSection("JwtModel").Get<JwtModel>();

	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,
		ValidIssuer = jwtModel.Issuer,
		ValidAudience = jwtModel.Audience,
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtModel.SecretKey)),
		ClockSkew = TimeSpan.Zero
	};
});

builder.Services.AddValidatorsFromAssemblyContaining<PhoneBookDTOValidation>();
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
