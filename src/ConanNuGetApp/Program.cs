using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace ConanNuGetApp
{
    /// <summary>
    /// Converts the Roslyn nuspec in Conan nuspecs
    /// </summary>
    class Program
    {
        private const string RoslynNuGetFolder = @"..\..\..\..\roslyn\src\NuGet";
        private const string PrefixPackage = "Microsoft.";
        private const string NewPrefixPackage = "Conan.";
        private const string AddDescription = @"
      Conan is a lightweight fork of the NET Compiler Platform (""Roslyn"") by adding a compiler plugin infrastructure.
    ";

        /// <summary>
        /// We are only processing the following top level nuspec with their transitive dependencies
        /// </summary>
        private static readonly string[] RootPackages = new string[]
        {
            "Microsoft.CodeAnalysis",
            "Microsoft.Net.Compilers",
            "Microsoft.NETCore.Compilers"
        };

        static void Main(string[] args)
        {
            var folder = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(typeof(Program).Assembly.Location), RoslynNuGetFolder));

            var dictionary = new Dictionary<string, string>();
            foreach (var package in RootPackages)
            {
                ProcessPackage(folder, package, dictionary);
            }
        }

        private static string ProcessPackage(string folder, string id, Dictionary<string, string> processed)
        {
            string result;
            if (processed.TryGetValue(id, out result))
            {
                return result;
            }

            var nuspecFile = Path.Combine(folder, id + ".nuspec");

            // Early exit if it is an external dependency
            if (!File.Exists(nuspecFile))
            {
                processed.Add(id, id);
                return id;
            }

            var newId = ReplacePrefix(id);
            processed.Add(id, newId);

            var xd = XDocument.Load(nuspecFile);
            var package = xd.Root;
            var metadata = SelectElement(package, "metadata");
            var idNode = SelectElement(metadata, "id");

            var summary = SelectElement(metadata, "summary");
            UpdateConanDescription(summary);
            if (summary != null)
            {
                summary.Value += AddDescription;
            }
            var description = SelectElement(metadata, "description");
            if (description != null)
            {
                description.Value += AddDescription;
            }
            UpdateConanDescription(description);

            idNode.Value = ReplacePrefix(idNode.Value);

            var dependencies = SelectElement(metadata, "dependencies");
            if (dependencies != null)
            {
                foreach (XElement dependency in dependencies.Descendants().Where(x => x.Name.LocalName == "dependency"))
                {
                    var idAttribute = dependency.Attribute("id");
                    var dependencyId = idAttribute?.Value;
                    if (dependencyId != null && dependencyId.StartsWith(PrefixPackage))
                    {
                        idAttribute.Value = ProcessPackage(folder, dependencyId, processed);
                    }
                }
            }

            metadata.Add(new XElement(XName.Get("iconUrl", metadata.Name.NamespaceName), "https://raw.githubusercontent.com/xoofx/Conan/master/img/conan.png"));
            var newNuspecFile = Path.Combine(folder, newId + ".nuspec");
            xd.Save(newNuspecFile);

            return newId;
        }

        private static string ReplacePrefix(string prefix)
        {
            if (prefix.StartsWith(PrefixPackage))
            {
                prefix = NewPrefixPackage + prefix.Substring(PrefixPackage.Length);
            }

            return prefix;
        }

        private static void UpdateConanDescription(XElement element)
        {
            if (element != null)
            {
                element.Value = element.Value.Replace("(\"Roslyn\")", "(\"Roslyn + Conan\")");
                element.Value = element.Value.Replace("https://aka.ms/roslyn-packages", "https://github.com/xoofx/Conan");
            }
        }

        private static XElement SelectElement(XElement parent, string name)
        {
            return parent.Descendants().FirstOrDefault(x => x.Name.LocalName == name);
        }
    }
}
