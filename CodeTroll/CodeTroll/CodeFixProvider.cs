using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CodeTroll.CodeFix;

namespace CodeTroll
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(CodeTrollCodeFixProvider)), Shared]
    public class CodeTrollCodeFixProvider : CodeFixProvider
    {
        public CodeTrollCodeFixProvider()
        {
            _lowercaseNameStartLocalMemberCodeFix = new AddSealedKeywordStartLocalMemberCodeFix();
            _interfaceExplicitNamingConventionCodeFix = new InterfaceExplicitNamingConventionCodeFix();
            _interfaceNamingConventionCodeFix = new InterfaceNamingConventionCodeFix();
        }

        #region CodeFixes

        readonly AddSealedKeywordStartLocalMemberCodeFix _lowercaseNameStartLocalMemberCodeFix;
        readonly InterfaceExplicitNamingConventionCodeFix _interfaceExplicitNamingConventionCodeFix;
        readonly InterfaceNamingConventionCodeFix _interfaceNamingConventionCodeFix;

        #endregion

        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(CodeTrollAnalyzer.DiagnosticId); }
        }

        public sealed override FixAllProvider GetFixAllProvider()
        {
            return WellKnownFixAllProviders.BatchFixer;
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            var diagnostic = context.Diagnostics.First();

            var diagnosticSpan = diagnostic.Location.SourceSpan;
            var declaration = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType<TypeDeclarationSyntax>().First();


            var localDeclarationStatementSyntax = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType<LocalDeclarationStatementSyntax>().FirstOrDefault();
            var interfaceDeclarationSyntax = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType<InterfaceDeclarationSyntax>().FirstOrDefault();


            if (localDeclarationStatementSyntax != null)
            {
                context.RegisterCodeFix(
                        CodeAction.Create(
                            title: AddSealedKeywordStartLocalMemberCodeFix.Title,
                            createChangedDocument: c => _lowercaseNameStartLocalMemberCodeFix.RenameLowerCaseLocalMember(context.Document, localDeclarationStatementSyntax, c),
                            equivalenceKey: AddSealedKeywordStartLocalMemberCodeFix.Title),
                        diagnostic);
            }

            if (interfaceDeclarationSyntax != null)
            {
                var interfaceName = interfaceDeclarationSyntax.Identifier.Text;
                if (!interfaceName.Contains(InterfaceExplicitNamingConventionCodeFix.INTERFACE_NAMING_CONVENTION_CODE_FIX_POSTFIX))
                {
                    context.RegisterCodeFix(
                    CodeAction.Create(
                        title: InterfaceExplicitNamingConventionCodeFix.Title,
                        createChangedDocument: c => _interfaceExplicitNamingConventionCodeFix.RenameInterface(context.Document, interfaceDeclarationSyntax, c),
                        equivalenceKey: InterfaceExplicitNamingConventionCodeFix.Title),
                    diagnostic);
                }


                if (!interfaceName.ToLower().StartsWith("i") && !interfaceName.Contains(InterfaceExplicitNamingConventionCodeFix.INTERFACE_NAMING_CONVENTION_CODE_FIX_POSTFIX))
                {
                    context.RegisterCodeFix(
                    CodeAction.Create(
                        title: InterfaceNamingConventionCodeFix.Title,
                        createChangedDocument: c => _interfaceNamingConventionCodeFix.RenameInterface(context.Document, interfaceDeclarationSyntax, c),
                        equivalenceKey: InterfaceNamingConventionCodeFix.Title),
                    diagnostic);
                }
            }

        }
    }
}
