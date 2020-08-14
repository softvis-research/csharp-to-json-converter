﻿using System.Collections.Generic;
using System.Linq;
using csharp_to_json_converter.model;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace csharp_to_json_converter.utils.analyzers
{
    public class ConstructorAnalyzer : AbstractAnalyzer
    {
        internal ConstructorAnalyzer(SyntaxTree syntaxTree, SemanticModel semanticModel) : base(syntaxTree,
            semanticModel)
        {
        }

        public void Analyze(ClassDeclarationSyntax classDeclarationSyntax, ClassModel classModel)
        {
            List<ConstructorDeclarationSyntax> constructorDeclarationSyntaxes = classDeclarationSyntax
                .DescendantNodes()
                .OfType<ConstructorDeclarationSyntax>()
                .ToList();

            foreach (ConstructorDeclarationSyntax constructorDeclarationSyntax in constructorDeclarationSyntaxes)
            {
                ConstructorModel constructorModel = new ConstructorModel();

                constructorModel.Name = constructorDeclarationSyntax.Identifier.Text;

                IMethodSymbol methodSymbol =
                    SemanticModel.GetDeclaredSymbol(constructorDeclarationSyntax) as IMethodSymbol;

                constructorModel.Static = methodSymbol.IsStatic;
                constructorModel.Abstract = methodSymbol.IsAbstract;
                constructorModel.Sealed = methodSymbol.IsSealed;
                constructorModel.Async = methodSymbol.IsAsync;
                constructorModel.Override = methodSymbol.IsOverride;
                constructorModel.Virtual = methodSymbol.IsVirtual;
                constructorModel.Accessibility = methodSymbol.DeclaredAccessibility.ToString();

                classModel.Constructors.Add(constructorModel);
            }
        }
    }
}