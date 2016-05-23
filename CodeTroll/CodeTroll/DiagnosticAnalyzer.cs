using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using CodeTroll.Rules;
using CodeTroll.Infra;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using System;

namespace CodeTroll
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class CodeTrollAnalyzer : DiagnosticAnalyzer
    {
        public static string DiagnosticId = Const.DIAGNOSTIC_ID;

        #region Rules 

        static readonly CornyClassNamingConventionRule _classNamingConventionRule = new CornyClassNamingConventionRule();
        static readonly UsingsNamespacesRule _usingsNamespacesRule = new UsingsNamespacesRule();
        static readonly LowercaseNameStartLocalMemberRule _lowercaseNameStartLocalMemberRule = new LowercaseNameStartLocalMemberRule();
        static readonly EventNameStartsUppercaseRule _eventNameStartsUppercaseRule = new EventNameStartsUppercaseRule();
        static readonly InterfaceExplicitNamingConventionRule _interfaceExplicitNamingConventionRule = new InterfaceExplicitNamingConventionRule();
        static readonly InterfaceNamingConventionRule _interfaceNamingConventionRule = new InterfaceNamingConventionRule();

        #endregion

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get
            {
                return ImmutableArray
                    .Create(_classNamingConventionRule.Rule)
                    .Add(_usingsNamespacesRule.Rule)
                    .Add(_eventNameStartsUppercaseRule.Rule)
                    .Add(_interfaceExplicitNamingConventionRule.Rule)
                    .Add(_interfaceNamingConventionRule.Rule)
                    .Add(_lowercaseNameStartLocalMemberRule.Rule);
            }
        }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSymbolAction(CornyClassNamingConventionAction, SymbolKind.NamedType);
            context.RegisterSymbolAction(InterfaceExplicitNamingConventionAction, SymbolKind.NamedType);
            context.RegisterSymbolAction(InterfaceNamingConventionAction, SymbolKind.NamedType);
            context.RegisterSyntaxNodeAction(LocalMemberNAmeStartsWithLowerCaseAction, SyntaxKind.LocalDeclarationStatement);
            context.RegisterSymbolAction(EventNameConventionUppercaseStartAction, SymbolKind.Event);
        }


        private void InterfaceNamingConventionAction(SymbolAnalysisContext context)
        {
            var namedTypeSymbol = context.Symbol as INamedTypeSymbol;
            if (namedTypeSymbol == null)
                return;

            if (_interfaceNamingConventionRule.Match(namedTypeSymbol))
            {
                var diagnostic = Diagnostic.Create(_interfaceNamingConventionRule.Rule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);
                context.ReportDiagnostic(diagnostic);
            }
        }

        private void InterfaceExplicitNamingConventionAction(SymbolAnalysisContext context)
        {
            var namedTypeSymbol = context.Symbol as INamedTypeSymbol;
            if (namedTypeSymbol == null)
                return;

            if (_interfaceExplicitNamingConventionRule.Match(namedTypeSymbol))
            {
                var diagnostic = Diagnostic.Create(_interfaceExplicitNamingConventionRule.Rule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);
                context.ReportDiagnostic(diagnostic);
            }
        }

        private void EventNameConventionUppercaseStartAction(SymbolAnalysisContext context)
        {
            var @event = context.Symbol as IEventSymbol;
            if (@event != null)
            {
                if (_eventNameStartsUppercaseRule.Match(@event))
                {
                    context.ReportDiagnostic(Diagnostic.Create(_eventNameStartsUppercaseRule.Rule, @event.Locations.First()));
                }
            }
        }

        private void LocalMemberNAmeStartsWithLowerCaseAction(SyntaxNodeAnalysisContext context)
        {
            var localDeclaration = (LocalDeclarationStatementSyntax)context.Node;

            if (localDeclaration.Modifiers.Any(SyntaxKind.ConstKeyword))
            {
                return;
            }

            if (localDeclaration.Declaration.Variables.Any())
            {
                var first = localDeclaration.Declaration.Variables.First();
                if (_lowercaseNameStartLocalMemberRule.Match(first))
                {
                    context.ReportDiagnostic(Diagnostic.Create(_lowercaseNameStartLocalMemberRule.Rule, context.Node.GetLocation()));
                }
            }
        }

        private static void CornyClassNamingConventionAction(SymbolAnalysisContext context)
        {
            var namedTypeSymbol = context.Symbol as INamedTypeSymbol;
            if (namedTypeSymbol == null)
                return;

            if (_classNamingConventionRule.Match(namedTypeSymbol))
            {
                var diagnostic = Diagnostic.Create(_classNamingConventionRule.Rule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}