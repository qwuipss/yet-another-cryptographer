using System.Text;

namespace Yacr.Configuration;

public interface IConfigurationProvider
{
    Encoding Encoding { get; }
}