using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace CodePraxis.Tinsl.Specialized
{
    using System.Collections;
    using System.Collections.Specialized;
    using System.Configuration;

    public class ProviderResolver<T> : IResolver<T>
    {

        public ProviderResolver()
        {
        }

        public ProviderResolver(string sectionName)
        {
            SectionName = sectionName;
        }

        public string SectionName
        {
            get;
            set;
        }

        public R Resolve<R>(object identifier = null) where R : T
        {
            Type type = typeof(R);
            if (identifier != null && !(identifier is string))
            {
                throw new ArgumentException("Provider identifier must be a string", "identifier");
            }

            Configuration config =
                ConfigurationManager.OpenExeConfiguration(
                ConfigurationUserLevel.None);

            ProtectedConfigurationSection pSection =
                (ProtectedConfigurationSection)config.GetSection(SectionName);

            ProviderSettingsCollection providerSettings = pSection.Providers;

            List<ProviderSettings> validProviders = new List<ProviderSettings>();

            foreach (ProviderSettings pSettings in providerSettings)
            {
                if (identifier == null || pSettings.Name.Equals((string)identifier))
                {
                    Type providerType = Type.GetType(pSettings.Type);

                    if (type.IsAssignableFrom(providerType))
                    {
                        validProviders.Add(pSettings);
                    }
                }
            }

            if (validProviders.Count > 1)
            {
                throw new ResolverException("More than one matching provider found.", type);
            }
            if (validProviders.Count == 0)
            {
                throw new ResolverException("No matching provider found.", type);
            }

            ProviderSettings provider = validProviders.First();
            Type selectedProviderType = Type.GetType(provider.Type);

            object instance = Activator.CreateInstance(selectedProviderType);

            NameValueCollection parameters = provider.Parameters;
            foreach (string key in parameters.Keys)
            {
                PropertyInfo propertyInfo = selectedProviderType.GetProperty(key,
                    BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase );

                if (propertyInfo != null && propertyInfo.PropertyType.Equals(typeof(string)))
                {
                    propertyInfo.SetValue(instance, parameters[key]);
                }
            }
            return (R)instance;

        }
    }
}
