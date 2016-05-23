using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Formatting;

namespace CodeTroll.CodeFix
{
    class AddSealedKeywordStartLocalMemberCodeFix
    {
        public static string Title => MessageFormat.ToString();

        public static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.SealYourLocalMembers), Resources.ResourceManager, typeof(Resources));


        public async Task<Document> RenameLowerCaseLocalMember(Document document, LocalDeclarationStatementSyntax localDeclaration, CancellationToken cancellationToken)
        {
            var firstToken = localDeclaration.GetFirstToken();

            var leadingTrivia = firstToken.LeadingTrivia;
            var trimmedLocal = localDeclaration.ReplaceToken(
                firstToken, firstToken.WithLeadingTrivia(SyntaxTriviaList.Empty));

            var constToken = SyntaxFactory.Token(leadingTrivia, SyntaxKind.SealedKeyword, SyntaxFactory.TriviaList(SyntaxFactory.ElasticMarker));

            var newModifiers = trimmedLocal.Modifiers.Insert(0, constToken);

            var newLocal = trimmedLocal.WithModifiers(newModifiers);

            var formattedLocal = newLocal.WithAdditionalAnnotations(Formatter.Annotation);

            var oldRoot = await document.GetSyntaxRootAsync(cancellationToken);
            var newRoot = oldRoot.ReplaceNode(localDeclaration, formattedLocal);

            return document.WithSyntaxRoot(newRoot);
        }
    }
}
