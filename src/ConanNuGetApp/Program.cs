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

            // We are no longer processing these nuspec files as we are making our own bringing Full and Core support into the same package
            //"Microsoft.Net.Compilers",
            //"Microsoft.NETCore.Compilers"
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
            UpdateConanDescription(description);
            if (description != null)
            {
                description.Value += AddDescription;
            }

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

            // Special case
            // we need to move the file in the nuspec from
            //
            if (id == "Microsoft.Net.Compilers" || id == "Microsoft.NETCore.Compilers")
            {
                // <file src="$additionalFilesPath$/Microsoft.NETCore.Compilers.props" target="build" />
                var files = SelectElement(package, "files");
                foreach (var file in files.Descendants().Where(x => x.Name.LocalName == "file"))
                {
                    var srcAttribute = file.Attribute("src").Value;
                    var targetAttribute = file.Attribute("target");
                    if (srcAttribute.StartsWith("$additionalFilesPath$/Microsoft.") && srcAttribute.EndsWith(".props"))
                    {
                        targetAttribute.Value = targetAttribute.Value + "/" + ReplacePrefix(Path.GetFileName(srcAttribute));
                        break;
                    }
                }
            }


            metadata.Add(new XElement(XName.Get("iconUrl", metadata.Name.NamespaceName), "https://raw.githubusercontent.com/conan-roslyn/Conan/master/img/conan.png"));
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
                element.Value = element.Value.Replace("https://aka.ms/roslyn-packages", "https://github.com/conan-roslyn/Conan");
                element.Value = element.Value.Replace(PrefixPackage, NewPrefixPackage);
            }
        }

        private static XElement SelectElement(XElement parent, string name)
        {
            return parent.Descendants().FirstOrDefault(x => x.Name.LocalName == name);
        }
    }
}
