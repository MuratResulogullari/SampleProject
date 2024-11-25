using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);


var cache = builder.AddRedis("cache")
                      .WithRedisInsight()
                      .WithDataVolume(isReadOnly: false);


var rabbitmq = builder.AddRabbitMQ("messaging")
                      .WithManagementPlugin()
                      .WithDataVolume(isReadOnly: false);

var postgres = builder.AddPostgres("postgres")
                       .WithDataVolume(isReadOnly: false)
                       .WithPgAdmin();

var catalogDB = postgres.AddDatabase("CatalogDB");

var orderServiceApi = builder.AddProject<Projects.SampleProject_OrderService_API>("order-service-api");
var paymentServiceApi = builder.AddProject<Projects.SampleProject_PaymentService_API>("payment-service-api");
var catalogServiceApi =  builder.AddProject<Projects.SampleProject_CatalogService_API>("catalog-service-api")
       .WaitFor(cache)
       .WithReference(cache)
       .WaitFor(rabbitmq)
       .WithReference(rabbitmq)
       .WaitFor(catalogDB)
       .WithReference(catalogDB)
       .WithReference(orderServiceApi)
       .WithReference(paymentServiceApi);

paymentServiceApi
       .WaitFor(cache)
       .WithReference(cache)
       .WaitFor(rabbitmq)
       .WithReference(rabbitmq)
       .WithReference(orderServiceApi);
orderServiceApi
       .WaitFor(cache)
       .WithReference(cache)
       .WaitFor(rabbitmq)
       .WithReference(rabbitmq)
       .WithReference(paymentServiceApi);



builder.Build().Run();
