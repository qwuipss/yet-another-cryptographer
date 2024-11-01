namespace Yacr.Commands.Resolvers;

public interface ICommandResolver
{
    ICommand Resolve(string? input);
}