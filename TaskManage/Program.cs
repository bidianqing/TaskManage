using TaskManage;

ThreadPool.GetMinThreads(out int minWorkerThreads, out int minCompletionPortThreads);
ThreadPool.GetMaxThreads(out int maxWorkerThreads, out int maxCompletionPortThreads);

Console.WriteLine($"最小工作线程数: {minWorkerThreads}");
Console.WriteLine($"最小I/O完成端口线程数: {minCompletionPortThreads}");
Console.WriteLine($"最大工作线程数: {maxWorkerThreads}");
Console.WriteLine($"最大I/O完成端口线程数: {maxCompletionPortThreads}");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSingleton<ITaskService, TaskService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
