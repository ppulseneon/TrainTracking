using TrainCarGrpcService;
using TrainStationClient.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddGrpcClient<TrainCarService.TrainCarServiceClient>(options =>
{
    var grpcServiceUrl = builder.Configuration["GrpcSettings:TrainStationServiceUrl"]
                         ?? throw new InvalidOperationException("GrpcSettings:TrainStationServiceUrl is not configured.");
    options.Address = new Uri(grpcServiceUrl);
});

builder.Services.AddScoped<ITrainCarGrpcClientService, TrainCarGrpcClientService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();