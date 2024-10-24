using System.Text;

namespace AegisCryptographer.Configuration;

public class ConfigurationProvider : IConfigurationProvider
{
    public Encoding Encoding { get; } = Encoding.UTF8;
}