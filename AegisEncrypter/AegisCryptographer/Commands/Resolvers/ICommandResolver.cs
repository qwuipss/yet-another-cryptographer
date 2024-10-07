namespace AegisCryptographer.Commands.Resolvers;

public interface ICommandResolver
{
    ICommand Resolve();
}