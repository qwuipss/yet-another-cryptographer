using Yacr.Collections;
using Yacr.Commands.Resolvers;

namespace Yacr.Services;

public interface IRegexService
{
    string? GetQuotesStringWithEscapedQuotes(string str);
    IEnumerable<string> SplitCommandArgumentsString(string str);
    ISplitExecutionStringInfo SplitExecutionStringInfo(string str);
}