using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace XamarinXamlPreviewCS.Libs
{
    public static class ResourceLoader
    {
        static internal string[] Names { get; set; }
        static internal Assembly Assembly { get; set; }

        public static System.IO.Stream GetObject(string resourceName)
        {
            if (ResourceLoader.Assembly == null)
            {
                ResourceLoader.Assembly = typeof(ResourceLoader).GetTypeInfo().Assembly;
                ResourceLoader.Names = ResourceLoader.Assembly.GetManifestResourceNames();
            }
            try
            {
                string path = ResourceLoader.Names.First(x => x.EndsWith(resourceName, StringComparison.CurrentCultureIgnoreCase));
                return ResourceLoader.Assembly.GetManifestResourceStream(path);
            }
            catch
            {
                return null;
            }
        }

        public static string GetString(string resourceName)
        {
            using (var st = ResourceLoader.GetObject(resourceName))
            {
                if (st != null)
                {
                    using (var sr = new System.IO.StreamReader(st))
                    {
                        return sr.ReadToEnd();
                    }
                }
                else
                {
                    return "";
                }
            }
        }
    }
}
