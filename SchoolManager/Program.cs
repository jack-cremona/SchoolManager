using Microsoft.AspNetCore.Mvc.Infrastructure;
using Scalar.AspNetCore;
using SchoolManager.Data;
using SchoolManager.DTO;

namespace SchoolManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddSqlServer<SchoolDbContext>(builder.Configuration.GetConnectionString("Default"));

            builder.Services.AddSingleton<Mapper>(); //servizio per la mappatura tra entity e dto, è stateless quindi va bene come singleton

            //builder.Services.AddSingleton<IMapper, Mapper>(); //alternativa, se volessi usare l'interfaccia invece della classe concreta, in questo caso non è necessario perché non ci sono dipendenze da iniettare, ma è una buona pratica per mantenere il codice più flessibile e testabile

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.MapScalarApiReference();

            app.Run();
        }
    }
}
