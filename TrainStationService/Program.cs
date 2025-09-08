using TrainStationService.DI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTrainStationServices(builder.Configuration);

var app = builder.Build();

app.MapGrpcService<TrainStationService.Grpc.TrainCarGrpcService>();
app.Run();