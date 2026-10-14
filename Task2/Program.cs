using Microsoft.EntityFrameworkCore;

namespace Task2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new() { Title = "Task2 API", Version = "v1" });
                options.CustomSchemaIds(type => type.FullName);
            });
            builder.Services.AddDbContext<Data.AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
                    ?? "Server=(localdb)\\MSSQLLocalDB;Database=Task2Products;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"));

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<Data.AppDbContext>();
                dbContext.Database.EnsureCreated();
            }

            app.UseSwagger(options => options.SerializeAsV2 = true);
            app.UseSwaggerUI(options =>
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Task2 v1"));
            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
