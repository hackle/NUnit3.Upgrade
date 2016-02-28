using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using System;
using Microsoft.CodeAnalysis.Formatting;

namespace NUnit3.Upgrade.UnitTest
{
    internal class ExpectedExceptionConverter
    {
        private const string exceptionName = "ExpectedException";

        public ExpectedExceptionConverter()
        {
        }

        internal string Convert(string input)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(input);

            var rootNode = CSharpSyntaxTree.Create(syntaxTree.GetRoot() as CSharpSyntaxNode).GetRoot();
            var classes = rootNode.DescendantNodes().OfType<ClassDeclarationSyntax>();

            foreach (var c in classes)
            {
                var methods = c.Members.OfType<MethodDeclarationSyntax>();

                foreach (var m in methods)
                {
                    var attributeList = m.AttributeLists
                        .FirstOrDefault(al => al.Attributes.Any(a => a.Name.ToString() == ExpectedExceptionConverter.exceptionName));

                    var attribute = attributeList.Attributes
                        .FirstOrDefault(a => a.Name.ToString() == ExpectedExceptionConverter.exceptionName);

                    var exceptionName = GetExceptionName(attribute);

                    var oldBody = m.Body.AddStatements();

                    var invocationStatement = SyntaxFactory.ParseStatement($"Assert.Throws<{exceptionName}>(()=>{m.Body.ToFullString().TrimEnd(Environment.NewLine.ToCharArray())});");

                    var newMethod = m.WithAttributeLists(m.AttributeLists.Remove(attributeList))
                        .WithBody(m.Body.WithStatements(new SyntaxList<StatementSyntax>()).AddStatements(invocationStatement));
                    
                    rootNode = rootNode.ReplaceNode(m, newMethod);
                }
            }

            return rootNode.NormalizeWhitespace().ToFullString();
        }

        private string GetExceptionName(AttributeSyntax attribute)
        {
            var typeofExpression = attribute.ArgumentList.Arguments.Select(ar => ar.Expression as TypeOfExpressionSyntax)
                .FirstOrDefault(t => null != t) as TypeOfExpressionSyntax;

            return ((IdentifierNameSyntax)typeofExpression.Type).Identifier.Text;
        }
    }
}