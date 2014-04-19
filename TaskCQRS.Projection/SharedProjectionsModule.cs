using System.Configuration;
using Autofac;
using Autofac.Core;

using MongoDB.Driver;

namespace TaskCQRS.Projection
{
    public class SharedProjectionsModule : Module
    {
        private readonly string connectionString;

        public SharedProjectionsModule(string connectionName)
        {
            //Assert.ArgumentNotNullOrEmpty(connectionName, "connectionStringName");
            var mongoConnectionStringSetting = ConfigurationManager.ConnectionStrings[connectionName];

           // Assert.IsNotNull(mongoConnectionStringSetting, "Mongo connection string is not set.");
            connectionString = mongoConnectionStringSetting.ConnectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.Register(ctx =>
            {
                var urlBuilder = new MongoUrlBuilder(connectionString);
                var client = new MongoClient(connectionString);
                var server = client.GetServer();

                //builder.RegisterProjection<TasksProjection, ITasksProjection>().WithParameter(ResolvedParameter.ForNamed<MongoDatabase>("SharedProjectionsModule"));
                //builder.

                return server.GetDatabase(urlBuilder.DatabaseName);
            }).Named<MongoDatabase>("SharedProjectionsModule");
        }
    }
}
