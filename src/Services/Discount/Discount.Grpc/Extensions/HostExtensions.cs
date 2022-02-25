﻿using Npgsql;

namespace DIscount.Grpc.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase<TContext>(this IHost host, int? retry = 0)
        {
            int retryForAvailability = retry.Value;

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var configuration = services.GetRequiredService<IConfiguration>();
                var logger = services.GetRequiredService<ILogger<TContext>>();

                try
                {
                    logger.LogInformation("Migrating postgresql database...");

                    using var connection = new NpgsqlConnection
                        (configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

                    connection.Open();

                    using var command = new NpgsqlCommand()
                    {
                        Connection = connection
                    };

                    command.CommandText = "DROP TABLE IF EXISTS Coupon";
                    command.ExecuteNonQuery();

                    command.CommandText = @"create table Coupon(
	                                            ID SERIAL PRIMARY KEY NOT NULL,
	                                            ProductName varchar(24) not null,
	                                            Description text,
	                                            Amount int)";
                    command.ExecuteNonQuery();

                    command.CommandText = "insert into Coupon (ProductNAme, Description, Amount) values ('Iphone X', 'IPhone Discount', 150)";
                    command.ExecuteNonQuery();

                    logger.LogInformation("Migrated postgresql database!");
                }
                catch (NpgsqlException ex)
                {

                    logger.LogError("An error occurred while migrating the postgresql database");

                    if (retryForAvailability < 50)
                    {
                        retryForAvailability++;
                        System.Threading.Thread.Sleep(1000);
                        MigrateDatabase<TContext>(host, retryForAvailability);
                    }
                }
            }

            return host;
        }
    }
}
