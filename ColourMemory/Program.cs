using ColourMemory.Core;
using ColourMemory.Services;
using ColourMemory.UI;
using Microsoft.Extensions.DependencyInjection;

internal class Program
{
    static void Main(string[] args)
    {
        // DI setup
        var services = new ServiceCollection();
        ConfigureServices(services);
        var serviceProvider = services.BuildServiceProvider();

        var ui = serviceProvider.GetRequiredService<ConsoleUI>();
        ui.PrintGameLoop();      
    }

    static void ConfigureServices(ServiceCollection services)
    {
        services.AddSingleton<IRandomProvider, StandardRandom>();
        services.AddTransient<GameLogic>();
        services.AddTransient<ConsoleUI>();
    }
}