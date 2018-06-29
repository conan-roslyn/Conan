# Conan  [![Build status](https://ci.appveyor.com/api/projects/status/i5ladyqmy7kp2m98?svg=true)](https://ci.appveyor.com/project/xoofx/conan)

<img align="right" width="256px" height="256px" src="img/conan.png">

Conan is a _lightweight_ fork of the [.NET Compiler Platform ("Roslyn")](https://github.com/dotnet/roslyn/) by adding a **compiler plugin infrastructure**. These plugins can be deployed and installed as regular Diagnostic Analyzers.

## Notice

> This repository is under construction, a documentation will follow in the coming weeks

## Usage

### How to develop a Conan compiler plugin?

1. Create a `netstandard1.3` library
2. Add the latest `Conan.CodeAnalysis` package (`alpha5+`)
3. Create a new class an inherit from `CompilationRewriter`. See [`HelloWorld` plugin example](https://github.com/conan-roslyn/Conan.Plugin.HelloWorld)
   ```C#
       [DiagnosticAnalyzer(LanguageNames.CSharp)]
       public class MyCompilationRewriter : CompilationRewriter
       {
           public override Compilation Rewrite(CompilationRewriterContext context)
           {
               var compilation = context.Compilation;
   
               // Transform compilation
               ...
   
               return compilation;
           }
       }
   ```    
4. If you want to create a NuGet package for this plugin, you can add a reference to the NuGet package [AnalyzerPack](https://github.com/xoofx/AnalyzerPack) and it will transform automatically your package into a Diagnostic Analyzer NuGet package (when doing a dotnet/msbuild Pack)

### How to use this plugin in your project?

1. Add the package `Conan.Net.Compilers` to your project: This will make the Conan compiler as the default CSharp/VB compiler and replace the default Roslyn compiler (This package works for both Full framework and Core framework unlike the Roslyn packages)
2. Add your plugin you developed either by:
   - Adding directly a reference to it into your csproj. This is what is used by the HelloWorld package:
     ```xml
       <ItemGroup>
           <Analyzer Include="..\Conan.Plugin.HelloWorld\bin\$(Configuration)\netstandard1.3\Conan.Plugin.HelloWorld.dll" />
       </ItemGroup>
     ```
   - Adding a reference to the NuGet package of your plugin (that has been through `AnalyzerPack`)
3. If you compile your project, the plugin will be automatically loaded and executed, check the logs!

## NuGet Packages

Their are 2 fundamental root packages in Conan:

- [**Conan.Net.Compilers**](https://www.nuget.org/packages/Conan.Net.Compilers): This is the Conan compiler that is replacing the default Roslyn compiler, working with both .NET full framework and .NET Core projects. This compiler will be responsible to load your Conan plugins (as Diagnostic Analyzers)
- [**Conan.CodeAnalysis**](https://www.nuget.org/packages/Conan.CodeAnalysis/): This is the root package for developing a Conan compiler plugin that you should reference from your plugin compiler library

| Roslyn | Conan    | NuGet
| ------- | -------- | --------
| Microsoft.Net.Compilers<br>Microsoft.NETCore.Compilers | [**Conan.Net.Compilers**](https://www.nuget.org/packages/Conan.Net.Compilers) | [![NuGet](https://img.shields.io/nuget/v/Conan.Net.Compilers.svg)](https://www.nuget.org/packages/Conan.Net.Compilers/)
| Microsoft.CodeAnalysis | [**Conan.CodeAnalysis**](https://www.nuget.org/packages/Conan.CodeAnalysis/) | [![NuGet](https://img.shields.io/nuget/v/Conan.CodeAnalysis.svg)](https://www.nuget.org/packages/Conan.CodeAnalysis/)
| • Microsoft.CodeAnalysis.Common | • [Conan.CodeAnalysis.Common](https://www.nuget.org/packages/Conan.CodeAnalysis.Common/) | [![NuGet](https://img.shields.io/nuget/v/Conan.CodeAnalysis.Common.svg)](https://www.nuget.org/packages/Conan.CodeAnalysis.Common/)
| • Microsoft.CodeAnalysis.CSharp | • [Conan.CodeAnalysis.CSharp](https://www.nuget.org/packages/Conan.CodeAnalysis.CSharp/) | [![NuGet](https://img.shields.io/nuget/v/Conan.CodeAnalysis.CSharp.svg)](https://www.nuget.org/packages/Conan.CodeAnalysis.CSharp/)
| • Microsoft.CodeAnalysis.CSharp.Workspaces | • [Conan.CodeAnalysis.CSharp.Workspaces](https://www.nuget.org/packages/Conan.CodeAnalysis.CSharp.Workspaces/) | [![NuGet](https://img.shields.io/nuget/v/Conan.CodeAnalysis.CSharp.Workspaces.svg)](https://www.nuget.org/packages/Conan.CodeAnalysis.CSharp.Workspaces/)
| • Microsoft.CodeAnalysis.VisualBasic | • [Conan.CodeAnalysis.VisualBasic](https://www.nuget.org/packages/Conan.CodeAnalysis.VisualBasic/) | [![NuGet](https://img.shields.io/nuget/v/Conan.CodeAnalysis.VisualBasic.svg)](https://www.nuget.org/packages/Conan.CodeAnalysis.VisualBasic/)
| • Microsoft.CodeAnalysis.VisualBasic.Workspaces | • [Conan.CodeAnalysis.VisualBasic.Workspaces](https://www.nuget.org/packages/Conan.CodeAnalysis.VisualBasic.Workspaces/) | [![NuGet](https://img.shields.io/nuget/v/Conan.CodeAnalysis.VisualBasic.Workspaces.svg)](https://www.nuget.org/packages/Conan.CodeAnalysis.VisualBasic.Workspaces/)
| • Microsoft.CodeAnalysis.Workspaces.Common | • [Conan.CodeAnalysis.Workspaces.Common](https://www.nuget.org/packages/Conan.CodeAnalysis.Workspaces.Common/) | [![NuGet](https://img.shields.io/nuget/v/Conan.CodeAnalysis.Workspaces.Common.svg)](https://www.nuget.org/packages/Conan.CodeAnalysis.Workspaces.Common/)

## Credits

All the people involved behind the [.NET Compiler Platform ("Roslyn")](https://github.com/dotnet/roslyn/). Conan is a very little addition to the huge work already done there.

The logo is called Lion created by [Jennifer Keana](https://thenounproject.com/jkeana7/) from the Noun Project

## Licensing

Same license than Roslyn: [Apache-2.0](roslyn/License.txt)

## Author

Alexandre MUTEL aka [xoofx](http://xoofx.com)
