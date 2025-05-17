# Fluent Code Generation Tool

A lightweight and flexible code generation tool designed for .NET projects.
Originally focused on CRUD scaffolding, it has evolved into a general-purpose C# code generator.
The tool uses reflection and Roslyn to process types and generate partial models or proxies.

## ✨ Features

* 🔍 Reflection-based type discovery
* 🧠 Semantic analysis via Roslyn
* ⚙️ Flexible pipeline for code transformations
* 📁 Generates `.g.cs` files for partial class extension
* 🧼 Automatically injects required `using` directives
* 🚀 Designed for integration with CI/CD or build-time code generation

## 💡 Use Cases

* Generating proxy models
* Creating DTOs or API contracts
* Automating repetitive boilerplate
* Preparing source code for other generators

## 📦 Installation

```bash
dotnet add package FluentCodeGenTool
```

Or use as a CLI tool:

```bash
dotnet tool install --global FluentCodeGenTool
```

## 🛠️ Usage

```bash
FluentCodeGenTool <AssemblyPath> <OutputDirectory>
```

Example:

```bash
FluentCodeGenTool ./MyProject/bin/Debug/net6.0/MyProject.dll ./Generated
```

## 📚 How It Works

1. Scans the provided assembly for types containing a nested `Configurator` class.
2. Executes code generation pipelines based on `IModelGenerationConfigurator`.
3. Generates new `.g.cs` files into the specified output directory.
4. Injects proper `using` directives for all referenced types, including generated ones.

## 🧩 Extension Points

You can customize behavior by implementing:

* `IModelGenerationConfigurator`
* Custom steps in `GenerationPipeline`

This allows powerful modular generation tailored to your project's needs.

## 📄 License

MIT License — see [LICENSE](./LICENSE) for details.
