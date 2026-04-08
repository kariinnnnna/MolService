using Microsoft.EntityFrameworkCore;
using MolServiceContracts.BusinessLogicContracts;
using MolServiceContracts.StorageContracts;
using MolServiceDatabaseImplement.Implements;
using MolServiceDatabaseImplement;
using MolServiceBusinessLogic.Implements;
using MolServiceBusinessLogic.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MOLServiceDatabase>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        x =>
        {
            x.MigrationsAssembly("MolServiceDatabaseImplement");
            x.MigrationsHistoryTable("__EFMigrationsHistory_MolService");
        }));

builder.Services.AddScoped<IClassroomStorage, ClassroomStorage>();
builder.Services.AddScoped<IEquipmentMovementHistoryStorage, EquipmentMovementHistoryStorage>();
builder.Services.AddScoped<IMaterialResponsiblePersonStorage, MaterialResponsiblePersonStorage>();
builder.Services.AddScoped<IMaterialTechnicalValueGroupStorage, MaterialTechnicalValueGroupStorage>();
builder.Services.AddScoped<IMaterialTechnicalValueStorage, MaterialTechnicalValueStorage>();
builder.Services.AddScoped<IMaterialTechnicalValueRecordStorage, MaterialTechnicalValueRecordStorage>();
builder.Services.AddScoped<ISoftwareStorage, SoftwareStorage>();
builder.Services.AddScoped<ISoftwareRecordStorage, SoftwareRecordStorage>();

builder.Services.AddScoped<IClassroomLogic, ClassroomLogic>();
builder.Services.AddScoped<IEquipmentMovementHistoryLogic, EquipmentMovementHistoryLogic>();
builder.Services.AddScoped<IMaterialResponsiblePersonLogic, MaterialResponsiblePersonLogic>();
builder.Services.AddScoped<IMaterialTechnicalValueGroupLogic, MaterialTechnicalValueGroupLogic>();
builder.Services.AddScoped<IMaterialTechnicalValueLogic, MaterialTechnicalValueLogic>();
builder.Services.AddScoped<IMaterialTechnicalValueRecordLogic, MaterialTechnicalValueRecordLogic>();
builder.Services.AddScoped<ISoftwareLogic, SoftwareLogic>();
builder.Services.AddScoped<ISoftwareRecordLogic, SoftwareRecordLogic>();

builder.Services.AddHttpClient<OneCApiService>();
builder.Services.AddTransient<IOneCImportLogic, OneCImportLogic>();
builder.Services.AddHttpClient<CoreApiService>();
builder.Services.AddTransient<ICoreClassroomImportLogic, CoreClassroomImportLogic>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();