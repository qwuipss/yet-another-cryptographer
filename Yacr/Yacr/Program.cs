using SimpleInjector;
using Yacr.Commands.Execution;
using Yacr.Commands.Flags;
using Yacr.Commands.Resolvers;
using Yacr.Configuration;
using Yacr.Cryptography.Algorithms;
using Yacr.Debug;
using Yacr.IO;
using Yacr.Runners;
using Yacr.Services;

namespace Yacr;

// ReSharper disable once ClassNeverInstantiated.Global
public class Program
{
    public static void Main(string[] args)
    {
        var container = new Container();

        container.Register<IConfigurationProvider, ConfigurationProvider>(Lifestyle.Singleton);
        container.Register<IDebugLogFactory, DebugLogFactory>(Lifestyle.Singleton);
        container.Register<IWriter, Writer>(Lifestyle.Singleton);
        container.Register<IReader, Reader>(Lifestyle.Singleton);
        container.Register<ICommandResolver, CommandResolver>(Lifestyle.Singleton);
        container.Register<ICommandFlagsResolver, CommandFlagsResolver>(Lifestyle.Singleton);
        container.Register<ICryptoAlgorithmResolver, CryptoAlgorithmResolver>(Lifestyle.Singleton);
        container.Register<ICommandExecutor, CommandExecutor>(Lifestyle.Singleton);
        container.Register<IRegexService, RegexService>(Lifestyle.Singleton);
        container.Register<ICryptoService, CryptoService>(Lifestyle.Singleton);
        container.Register<LoopRunner>(Lifestyle.Singleton);

        if (args.Length is 0)
            container.GetInstance<LoopRunner>().Run();
        else
            throw new NotImplementedException(); // todo cli runner
    }
}