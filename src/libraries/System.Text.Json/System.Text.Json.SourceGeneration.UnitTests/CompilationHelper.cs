﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;
using System.Text.Json.Serialization;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace System.Text.Json.SourceGeneration.UnitTests
{
    public class CompilationHelper
    {
        public static Compilation CreateCompilation(string source, MetadataReference[] additionalReferences = null)
        {
            // Bypass System.Runtime error.
            Assembly systemRuntimeAssembly = Assembly.Load("System.Runtime, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
            string systemRuntimeAssemblyPath = systemRuntimeAssembly.Location;

            List<MetadataReference> references = new List<MetadataReference> {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Attribute).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(JsonSerializableAttribute).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(JsonSerializerOptions).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Type).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(KeyValuePair).Assembly.Location),
                MetadataReference.CreateFromFile(systemRuntimeAssemblyPath),
            };

            // Add additional references as needed.
            if(additionalReferences != null)
            {
                foreach(MetadataReference reference in additionalReferences)
                {
                    references.Add(reference);
                }
            }

            return CSharpCompilation.Create(
                "TestAssembly",
                syntaxTrees: new[] { CSharpSyntaxTree.ParseText(source) },
                references: references.ToArray(),
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
            );
        }

        private static GeneratorDriver CreateDriver(Compilation compilation, params ISourceGenerator[] generators)
            => new CSharpGeneratorDriver(
                new CSharpParseOptions(kind: SourceCodeKind.Regular, documentationMode: DocumentationMode.Parse),
                ImmutableArray.Create(generators),
                ImmutableArray<AdditionalText>.Empty);

        public static Compilation RunGenerators(Compilation compilation, out ImmutableArray<Diagnostic> diagnostics, params ISourceGenerator[] generators)
        {
            CreateDriver(compilation, generators).RunFullGeneration(compilation, out Compilation outCompilation, out diagnostics);
            return outCompilation;
        }
    }
}
