using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using PharmacyAPI.Data;
using PharmacyAPI.Repository;
using PharmacyAPI.Mappers;

var builder = WebApplication.CreateBuilder(args);

// إعداد الاتصال بقاعدة البيانات
builder.Services.AddDbContext<PharmacyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));// APPSETING  ملاحظة سجلتاسم القاعة بالطرية 2 والتي هي في 

//  AutoMapperفي حال لزمت
builder.Services.AddAutoMapper(typeof(PharmacyMappingProfile));

// ✅ إعداد Serilog
Log.Logger = new LoggerConfiguration()//هذه الابع اسطر خاصة بدرس ال serilog//هذه الاربع اسطر ثابتة وعي عبفهم الكومبايل وين دلوغ
    .MinimumLevel.Debug()//اريد تدبغلي على الملف المطلوب يوميا ملاحظة التتبيغ نوع من الاخطاء
    .WriteTo.Console()//هون يعني دلوغ على الكونسول
    .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)//هون يعني دلوغ على الملف المرفق بالمشروع استطيع تغير المسار ايضا لمسار اخر
    .CreateLogger();

builder.Logging.AddConsole();
builder.Host.UseSerilog();

//  إعداد JWT Authentication
var key = Encoding.ASCII.GetBytes(builder.Configuration["JwtSettings:Key"]);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"]
    };
});

//  إضافة الخدمات 
builder.Services.AddScoped<IPharmacistRepository, PharmacistRepository>();
builder.Services.AddScoped<IMedicineRepository, MedicineRepository>();
builder.Services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();


// إضافة التحكم في النسخ 
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
