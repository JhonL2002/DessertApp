using System.Globalization;

namespace DessertApp.Infraestructure.ConfigurationServices
{
    public static class CultureConfigurator
    {
        public static void ConfigureCulture()
        {
            //Create English base culture (en-US)
            var customCulture = new CultureInfo("en-US");

            //Customize numeric format
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            customCulture.NumberFormat.NumberGroupSeparator = ",";

            //Apply configuration as global level
            CultureInfo.DefaultThreadCurrentCulture = customCulture;
            CultureInfo.DefaultThreadCurrentUICulture = customCulture;

        }
    }
}
