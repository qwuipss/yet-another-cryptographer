using System.Text;

namespace AegisCryptographer.Configuration;

public interface IConfigurationProvider
{
    Encoding Encoding { get; }
}