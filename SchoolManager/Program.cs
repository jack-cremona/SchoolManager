using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Scalar.AspNetCore;
using SchoolManager.Data;
using SchoolManager.DTO;
using System.Text.Json;

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

            string? connectionString = builder.Configuration.GetConnectionString("Default");
            builder.Services.AddSqlServer<SchoolDbContext>(connectionString);

            builder.Services.AddSingleton<Mapper>(); //servizio per la mappatura tra entity e dto, è stateless quindi va bene come singleton

            //builder.Services.AddSingleton<IMapper, Mapper>(); //alternativa, se volessi usare l'interfaccia invece della classe concreta, in questo caso non è necessario perché non ci sono dipendenze da iniettare, ma è una buona pratica per mantenere il codice più flessibile e testabile

            var app = builder.Build();


            //controlla che il db esista e allineato alle migrazioni
            using (var scope = app.Services.CreateScope())
            {
                var ctx = scope.ServiceProvider.GetService<SchoolDbContext>();
                if (ctx != null)
                {
                    ctx.Database.Migrate();
                    //primo metodo
                    //ctx.Database.ExecuteSqlRaw("truncate table [nome tabella]"); //se volessi svuotare una tabella, ad esempio per testare la creazione di nuovi record
                    //secondo metodo
                    ctx.Courses.RemoveRange(ctx.Courses); //esempio di come eliminare tutti i record di una tabella usando EF Core, in questo caso elimino tutti i corsi
                    ctx.SaveChanges();

                    string json = File.ReadAllText("courses.json"); //leggo il file json che contiene i dati dei corsi, in questo caso è un file di esempio che ho creato, ma in un caso reale potrebbe essere un file caricato dall'utente o scaricato da un servizio esterno
                    List<CourseJson>? data = JsonSerializer.Deserialize<List<CourseJson>>(json);
                    if(data != null)
                    {
                        data = data.Where(c => !string.IsNullOrEmpty(c.title)).ToList(); //filtro i dati per eliminare quelli che non hanno un titolo, in questo caso è un controllo di base per evitare di inserire record con dati mancanti o non validi
                        List<Course> courses = data.ConvertAll(d => new Course { Title = d.title }); //converto i dati dal formato json al formato entity, in questo caso creo una lista di corsi a partire dai dati del file json, è importante notare che sto usando l'operatore di null-forgiving (!) per indicare che il titolo non sarà null, in questo caso è una semplificazione perché ho già filtrato i dati per eliminare quelli senza titolo
                        ctx.AddRange(courses); //aggiungo i corsi al contesto, in questo caso sto aggiungendo tutti i corsi alla volta usando il metodo AddRange, che è più efficiente rispetto ad aggiungere i corsi uno alla volta con il metodo Add
                        ctx.SaveChanges(); //salvo le modifiche al database, in questo caso sto salvando tutti i corsi che ho aggiunto al contesto, è importante notare che sto salvando le modifiche solo alla fine, dopo aver aggiunto tutti i corsi, per evitare di fare più operazioni di salvataggio che potrebbero essere inefficienti


                    }
                }
            }


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


            public class CourseJson
            {
                public string? title { get; set; }
                //public string? description { get; set; }
            }
    }
}