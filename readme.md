# Conan  [![Build status](https://ci.appveyor.com/api/projects/status/1llcx8sa97sp7k71?svg=true)](https://ci.appveyor.com/project/xoofx/Conan)

<img align="right" width="256px" height="256px" src="img/conan.png">

Conan is a _lightweight_ fork of the [.NET Compiler Platform ("Roslyn")](https://github.com/dotnet/roslyn/) by adding a **compiler plugin infrastructure**. These plugins can be deployed and installed as regular Diagnostic Analyzers.

## Notice

> This repository is under construction, not officially released and still not usable

## NuGet Packages

| Roslyn | Conan    |
| ------- | --------
| Microsoft.CodeAnalysis.Common | Conan.CodeAnalysis [![NuGet](https://img.shields.io/nuget/v/Conan.CodeAnalysis.svg)](https://www.nuget.org/packages/Conan.CodeAnalysis/)
| Microsoft.CodeAnalysis.CSharp | Conan.CodeAnalysis.Common [![NuGet](https://img.shields.io/nuget/v/Conan.CodeAnalysis.Common.svg)](https://www.nuget.org/packages/Conan.CodeAnalysis.Common/)
| Microsoft.CodeAnalysis.CSharp.Workspaces | Conan.CodeAnalysis.CSharp [![NuGet](https://img.shields.io/nuget/v/Conan.CodeAnalysis.CSharp.svg)](https://www.nuget.org/packages/Conan.CodeAnalysis.CSharp/)
| Microsoft.CodeAnalysis | Conan.CodeAnalysis.CSharp.Workspaces [![NuGet](https://img.shields.io/nuget/v/Conan.CodeAnalysis.CSharp.Workspaces.svg)](https://www.nuget.org/packages/Conan.CodeAnalysis.CSharp.Workspaces/)
| Microsoft.CodeAnalysis.VisualBasic | Conan.CodeAnalysis.VisualBasic [![NuGet](https://img.shields.io/nuget/v/Conan.CodeAnalysis.VisualBasic.svg)](https://www.nuget.org/packages/Conan.CodeAnalysis.VisualBasic/)
| Microsoft.CodeAnalysis.VisualBasic.Workspaces | Conan.CodeAnalysis.VisualBasic.Workspaces [![NuGet](https://img.shields.io/nuget/v/Conan.CodeAnalysis.VisualBasic.Workspaces.svg)](https://www.nuget.org/packages/Conan.CodeAnalysis.VisualBasic.Workspaces/)
| Microsoft.CodeAnalysis.Workspaces.Common | Conan.CodeAnalysis.Workspaces.Common [![NuGet](https://img.shields.io/nuget/v/Conan.CodeAnalysis.Workspaces.Common.svg)](https://www.nuget.org/packages/Conan.CodeAnalysis.Workspaces.Common/)
| Microsoft.Net.Compilers | Conan.Net.Compilers [![NuGet](https://img.shields.io/nuget/v/Conan.Net.Compilers.svg)](https://www.nuget.org/packages/Conan.Net.Compilers/)
| Microsoft.NETCore.Compilers | Conan.NETCore.Compilers [![NuGet](https://img.shields.io/nuget/v/Conan.NETCore.Compilers.svg)](https://www.nuget.org/packages/Conan.NETCore.Compilers/)

## Credits

All the people involved behind the [.NET Compiler Platform ("Roslyn")](https://github.com/dotnet/roslyn/). Conan is a very little addition to the huge work already done there.

The logo is called Lion created by [Jennifer Keana](https://thenounproject.com/jkeana7/) from the Noun Project

## Licensing

Same license than Roslyn: [Apache-2.0](roslyn/License.txt)

## Author

Alexandre MUTEL aka [xoofx](http://xoofx.com)
