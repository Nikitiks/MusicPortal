using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Text.Json;

namespace MusicPortal.Services
{
    public class JsonStringLocalizer : IStringLocalizer
    {
        private readonly Dictionary<string, string> _localizations;

        public JsonStringLocalizer()
        {
            _localizations = new Dictionary<string, string>();
            LoadLocalizations();
        }

        private void LoadLocalizations()
        {
            try
            {
                var culture = CultureInfo.CurrentCulture.Name;
                var basePath = Directory.GetCurrentDirectory();
                var resourcesPath = Path.Combine(basePath, "Resources");

                if (Directory.Exists(resourcesPath))
                {
                    var jsonFiles = Directory.GetFiles(resourcesPath, $"*.{culture}.json", SearchOption.AllDirectories);

                    foreach (var file in jsonFiles)
                    {
                        try
                        {
                            var jsonContent = File.ReadAllText(file);
                            var localizations = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonContent);

                            if (localizations != null)
                            {
                                foreach (var localization in localizations)
                                {
                                    _localizations[localization.Key] = localization.Value;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error loading localization file {file}: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in LoadLocalizations: {ex.Message}");
            }
        }

        public LocalizedString this[string name]
        {
            get
            {
                if (_localizations.TryGetValue(name, out var value))
                {
                    return new LocalizedString(name, value);
                }
                return new LocalizedString(name, name, resourceNotFound: true);
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var format = this[name];
                if (!format.ResourceNotFound)
                {
                    return new LocalizedString(name, string.Format(format.Value, arguments), false);
                }
                return new LocalizedString(name, name, true);
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            return _localizations.Select(x => new LocalizedString(x.Key, x.Value));
        }
    }

    public class JsonStringLocalizerFactory : IStringLocalizerFactory
    {
        public IStringLocalizer Create(Type resourceSource)
        {
            return new JsonStringLocalizer();
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            return new JsonStringLocalizer();
        }
    }
}