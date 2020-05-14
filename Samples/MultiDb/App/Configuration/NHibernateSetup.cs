namespace SharpArch.MultiDatabase.Configuration
{
    using Autofac;
    using Domain.PersistenceSupport;
    using FluentNHibernate.Cfg.Db;
    using LogDb;
    using MainDb;
    using Microsoft.Extensions.DependencyInjection;
    using NHibernate.Configuration;
    using NHibernate.MultiDb;
    using Persistence;


    public static class NHibernateSetup
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            services.AddSingleton<ISessionFactoryRegistry>(sp =>
            {
                var sr = new SessionFactoryRegistry();

                sr.Add(Databases.Log, new NHibernateSessionFactoryBuilder()
                    .AddMappingAssemblies(new[] {typeof(LogDbPersistenceModelGenerator).Assembly})
                    .UsePersistenceConfigurer(
                        MsSqlConfiguration.MsSql2012.ConnectionString("Server=localhost,2433;Database=Log;User Id=sa;Password=Password12!;")
                    )
                    .UseAutoPersistenceModel(new MainDbPersistenceModelGenerator().Generate())
                );

                sr.Add(Databases.Main, new NHibernateSessionFactoryBuilder()
                    .AddMappingAssemblies(new[] {typeof(MainDbPersistenceModelGenerator).Assembly})
                    .UsePersistenceConfigurer(
                        MsSqlConfiguration.MsSql2012.ConnectionString("Server=localhost,2433;Database=Log;User Id=sa;Password=Password12!;")
                    )
                    .UseAutoPersistenceModel(new MainDbPersistenceModelGenerator().Generate())
                );

                return sr;
            });

            services.AddScoped<ISessionRegistry>(sp => new SessionRegistry(sp.GetRequiredService<ISessionFactoryRegistry>()));

            services.AddSingleton<IDatabaseIdentifierProvider>(new DefaultDatabaseIdentifierProvider(true));

            return services;
        }

        public static void AddRepositories(this ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(TaggedNHibernateRepository<,>))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}
