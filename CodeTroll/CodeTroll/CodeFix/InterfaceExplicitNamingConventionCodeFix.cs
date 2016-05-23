using Microsoft.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Threading;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Formatting;
using System.Linq;

namespace CodeTroll.CodeFix
{
    class InterfaceExplicitNamingConventionCodeFix
    {
        public static string Title => CodeFixTitle.ToString();

        public static readonly LocalizableString CodeFixTitle = new LocalizableResourceString(nameof(Resources.InterfaceExplicitNamingConventionCodeFixTitle), Resources.ResourceManager, typeof(Resources));

        public const string INTERFACE_NAMING_CONVENTION_CODE_FIX_POSTFIX = "ExplicitInterfaceDeclaration";

        internal async Task<Document> RenameInterface(Document document, InterfaceDeclarationSyntax interfaceDeclarationSyntax, CancellationToken cancellationToken)
        {
            var oldRoot = await document.GetSyntaxRootAsync(cancellationToken);

            var newName = string.Empty;

            var interfaceName = interfaceDeclarationSyntax.Identifier.Text;

            if (!interfaceName.Contains(INTERFACE_NAMING_CONVENTION_CODE_FIX_POSTFIX))
            {
                var firstChar = interfaceName.ToCharArray().FirstOrDefault();

                var temp  = interfaceName.Substring(1, interfaceName.Length -1);
                newName = $"I{temp}{INTERFACE_NAMING_CONVENTION_CODE_FIX_POSTFIX}";
            }
            

            var newRoot = oldRoot.ReplaceNode(interfaceDeclarationSyntax, SyntaxFactory.InterfaceDeclaration(newName));

            return document.WithSyntaxRoot(newRoot);

        }
    }
}
