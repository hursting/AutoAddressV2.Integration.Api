
using AutoAddressV2.Integration.Api.Middleware;

WebApplication.CreateBuilder(args)
    .RegisterServices()
    .Build()
    .ConfigurePipeline()
    .Run();