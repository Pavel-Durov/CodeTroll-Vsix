using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Threading;
using System.Threading.Tasks;

namespace CodeTroll.CodeFix
{
    class InterfaceNamingConventionCodeFix
    {
        public static string Title => CodeFixTitle.ToString();

        public static readonly LocalizableString CodeFixTitle = new LocalizableResourceString(nameof(Resources.InterfaceNamingConventionCodeFixTitle), Resources.ResourceManager, typeof(Resources));

        internal async Task<Document> RenameInterface(Document document, InterfaceDeclarationSyntax interfaceDeclarationSyntax, CancellationToken cancellationToken)
        {
            var oldRoot = await document.GetSyntaxRootAsync(cancellationToken);

            var newName = string.Empty;

            var interfaceName = interfaceDeclarationSyntax.Identifier.Text;
            if (!interfaceName.ToLower().StartsWith("i") && !interfaceName.Contains(InterfaceExplicitNamingConventionCodeFix.INTERFACE_NAMING_CONVENTION_CODE_FIX_POSTFIX))
            {
                newName = $"I{interfaceName}";
            }
            var newRoot = oldRoot.ReplaceNode(interfaceDeclarationSyntax, SyntaxFactory.InterfaceDeclaration(newName));

            return document.WithSyntaxRoot(newRoot);

        }

    }
}
