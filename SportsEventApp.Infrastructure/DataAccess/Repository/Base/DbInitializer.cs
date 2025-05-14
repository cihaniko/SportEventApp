using Dapper;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SportsEventApp.Infrastructure.DataAccess.Repository.Base
{
    public class DbInitializer : IDbInitializer
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IHostEnvironment _env;

        public DbInitializer(IDbConnectionFactory connectionFactory, IHostEnvironment env)
        {
            _connectionFactory = connectionFactory;
            _env = env;
        }

        public async Task InitializeAsync()
        {
            using var connection = _connectionFactory.CreateConnection();

            var exists = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'SportEvents';");

            if (exists > 0)
            {
                Console.WriteLine("✔️ Veritabanı zaten hazır.");
                return;
            }

            // init.sql dosyasını oku ve çalıştır
            var filePath = Path.Combine(_env.ContentRootPath, "init.sql");

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"❌ init.sql bulunamadı: {filePath}");
                return;
            }

            var script = await File.ReadAllTextAsync(filePath);
            if (string.IsNullOrWhiteSpace(script))
            {
                Console.WriteLine("❌ init.sql dosyası boş.");
                return;
            }

            var commands = Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);

            foreach (var command in commands)
            {
                var trimmed = command.Trim();
                if (!string.IsNullOrWhiteSpace(trimmed))
                {
                    await connection.ExecuteAsync(trimmed);
                }
            }

            Console.WriteLine("✅ init.sql başarıyla uygulandı.");

        }
    }
}
