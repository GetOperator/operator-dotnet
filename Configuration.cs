using System.Configuration;

// Before compiling this application,
// remember to reference the System.Configuration assembly in your project.

// Define a custom section. This class is used to
// populate the configuration file.
// It creates the following custom section:
//  <OperatorConfigSection name="Contoso" url="http://www.contoso.com" port="8080" />.

namespace Operator
{
  public sealed class ConfigSection : ConfigurationSection
  {

    public ConfigSection()
    {
    }

    [ConfigurationProperty("DSN",
        IsRequired = true)]
    // [RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
    public string Dsn
    {
      get
      {
        return (string)this["DSN"];
      }
      set
      {
        this["DSN"] = value;
      }
    }
  }

}