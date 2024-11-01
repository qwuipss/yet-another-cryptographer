using System.Text;

namespace Yacr.Configuration;

public class ConfigurationProvider : IConfigurationProvider
{
    public Encoding Encoding { get; } = Encoding.UTF8;
}