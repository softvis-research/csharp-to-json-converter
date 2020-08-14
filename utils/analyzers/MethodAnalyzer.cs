﻿using System.Collections.Generic;
using System.Linq;
using csharp_to_json_converter.model;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace csharp_to_json_converter.utils.analyzers
{
    public class MethodAnalyzer : AbstractAnalyzer
    {
        private readonly InvocationAnalyzer _invocationAnalyzer;
        private readonly ParameterAnalyzer _parameterAnalyzer;

        internal MethodAnalyzer(SyntaxTree syntaxTree, SemanticModel semanticModel) : base(syntaxTree, semanticModel)
        {
            _invocationAnalyzer = new InvocationAnalyzer(SyntaxTree, SemanticModel);
            _parameterAnalyzer = new ParameterAnalyzer(SyntaxTree, SemanticModel);
        }

        public void Analyze(ClassDeclarationSyntax classDeclarationSyntax, ClassModel classModel)
        {
            List<MethodDeclarationSyntax> methodDeclarationSyntaxes = classDeclarationSyntax
                .DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
                .ToList();

            foreach (MethodDeclarationSyntax methodDeclarationSyntax in methodDeclarationSyntaxes)
            {
                MethodModel methodModel = new MethodModel();

                methodModel.Name = methodDeclarationSyntax.Identifier.Text;

                IMethodSymbol methodSymbol = SemanticModel.GetDeclaredSymbol(methodDeclarationSyntax) as IMethodSymbol;

                methodModel.Fqn = methodSymbol.ToString();
                methodModel.Static = methodSymbol.IsStatic;
                methodModel.Abstract = methodSymbol.IsAbstract;
                methodModel.Sealed = methodSymbol.IsSealed;
                methodModel.Async = methodSymbol.IsAsync;
                methodModel.Override = methodSymbol.IsOverride;
                methodModel.Virtual = methodSymbol.IsVirtual;
                methodModel.Accessibility = methodSymbol.DeclaredAccessibility.ToString();
                methodModel.ReturnType = methodSymbol.ReturnType.ToString();
                methodModel.FirstLineNumber =
                    methodDeclarationSyntax.GetLocation().GetLineSpan().StartLinePosition.Line + 1;
                methodModel.LastLineNumber =
                    methodDeclarationSyntax.GetLocation().GetLineSpan().EndLinePosition.Line + 1;

                _invocationAnalyzer.Analyze(methodDeclarationSyntax, methodModel);
                _parameterAnalyzer.Analyze(methodDeclarationSyntax, methodModel);

                classModel.Methods.Add(methodModel);
            }
        }
    }
}