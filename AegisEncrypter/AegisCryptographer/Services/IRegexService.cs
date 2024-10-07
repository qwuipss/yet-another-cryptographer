using AegisCryptographer.Collections;
using AegisCryptographer.Commands.Resolvers;

namespace AegisCryptographer.Services;

public interface IRegexService
{
    string? GetQuotesStringWithEscapedQuotes(string str);
    IEnumerable<string> SplitCommandArgumentsString(string str);
    ISplitExecutionStringInfo SplitExecutionStringInfo(string str);
}