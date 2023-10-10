using BlazorServerApp.Helper;
using BlazorServerApp.Injectables;
using MediatR;

namespace BlazorServerApp.Extensions;

public static class IServiceCollectionExtension
{
    public static void InitMediatR(this IServiceCollection self)
    {
        self.AddMediatR(AssemblyHelper.GetAllAssemblies().ToArray());
        //self.AddTransient(typeof(IPipelineBehavior<,>), typeof(MediatRLoggingBehavior<,>));
    }

    public static void InitScrutor(this IServiceCollection self)
    {
        self.Scan(scan => scan
                            .FromAssemblies(AssemblyHelper.GetAllAssemblies())
                            .AddClasses(classes => classes.AssignableTo<ITransientService>())
                            .AsImplementedInterfaces()
                            .WithTransientLifetime()
                            .AddClasses(classes => classes.AssignableTo<IScopedService>())
                            .AsImplementedInterfaces()
                            .WithScopedLifetime()
                            .AddClasses(classes => classes.AssignableTo<ISingletonService>())
                            .AsImplementedInterfaces()
                            .WithSingletonLifetime()
                            );
    }
}
