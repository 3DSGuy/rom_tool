using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace _3DSExplorer.Utils
{
    public static class TitleDatabase
    {
        private const string DatabaseFileName = "title.db";
        public static readonly string FilePath;
        private static readonly bool Loaded;
        private static readonly Dictionary<string, Dictionary<string, string>> Contents = new Dictionary<string, Dictionary<string, string>>();

        static TitleDatabase()
        {
            Dictionary<string, string> currentSection = null;
            Loaded = false;
            FilePath = Path.GetDirectoryName(Application.ExecutablePath) + "/" + DatabaseFileName;
            if (!File.Exists(FilePath)) return;
            var reader = new StreamReader(FilePath);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                line = line.Trim();
                if (line == "") continue;
                if (line[0] == ';') continue;
                if (line.StartsWith("[") && line.EndsWith("]") && line.Length > 2)
                {
                    var currentSectionName = line.Substring(1, line.Length - 2);
                    if (Contents.ContainsKey(currentSectionName))
                        currentSection = Contents[currentSectionName];
                    else
                    {
                        currentSection = new Dictionary<string, string>();
                        Contents[currentSectionName] = currentSection;
                    }
                }
                else if (currentSection != null && line.Contains("="))
                {
                    var eqPos = line.IndexOf('=');
                    currentSection[line.Substring(0, eqPos).Trim()] = line.Substring(eqPos + 1).Trim();
                }
            }
            reader.Close();
            Loaded = true;
        }

        public static string Get(string group, string key)
        {
            if (Loaded & Contents.ContainsKey(group) && Contents[group].ContainsKey(key))
                return Contents[group][key];
            return key + "- N/A";
        }

    }

    public class TitleInfo
    {
        public string Type;
        public string Title;
        public string Region;
        public string Developer;
        public string ProductCode;

        public TitleInfo()
        {
            const string unknown = "<Unknown>";
            Type = unknown;
            Title = unknown;
            Region = unknown;
            Developer = unknown;
            ProductCode = unknown;
        }

        public override string ToString()
        {
            return Title;
        }

        public static TitleInfo Resolve(char[] productChars, char[] developerChars)
        {
            var info = new TitleInfo();
            //CTR-P-#$$%
            //0123456789
            info.ProductCode = new string(productChars);

            if (productChars != null)
            {
                var type = productChars[6];
                switch (type)
                {
                    case 'A':
                        info.Type = "Application";
                        break;
                    case 'C':
                        info.Type = "CTR";
                        break;
                    case 'H':
                        info.Type = "Dev";
                        break;
                }
                var regionCode = productChars[9];
                switch (regionCode)
                {
                    case 'E':
                        info.Region = "USA";
                        break;
                    case 'P':
                        info.Region = "EUR";
                        break;
                    case 'J':
                        info.Region = "JPN";
                        break;
                    case 'F':
                        info.Region = "FRA";
                        break;
                    case 'Z':
                        info.Region = "ENG";
                        break;
                }
                var productCode = productChars[7].ToString() + productChars[8];
                info.Title = type == 'A' ? TitleDatabase.Get("ProductCodes", productCode) : productCode;
                
            }
            if (developerChars != null)
            {
                var developerCode = developerChars[0].ToString() + developerChars[1];
                info.Developer = TitleDatabase.Get("MakerCodes", developerCode);
            }
            return info;
        }
    }
}
