using CodeTroll.CodeFix;
using CodeTroll.Infra;
using Microsoft.CodeAnalysis;

namespace CodeTroll.Rules
{
    class InterfaceExplicitNamingConventionRule
    {
        public static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.InterfaceExplicitNamingConventionTitle), Resources.ResourceManager, typeof(Resources));

        public static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.InterfaceExplicitNamingConventionMessageFormat), Resources.ResourceManager, typeof(Resources));

        public static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.InterfaceExplicitNamingConventionContent), Resources.ResourceManager, typeof(Resources));

        public readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(Const.DIAGNOSTIC_ID, Title, MessageFormat, Const.NAMING_CATEGORY, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);


  
        public bool Match(INamedTypeSymbol namedTypeSymbol)
        {
            bool result = false;

            var lower = namedTypeSymbol.Name.ToLower();

            if (!string.IsNullOrEmpty(lower) && !lower.Contains(InterfaceExplicitNamingConventionCodeFix.INTERFACE_NAMING_CONVENTION_CODE_FIX_POSTFIX.ToLower()))
            {
                result = true;
            }

            return result && namedTypeSymbol.TypeKind == TypeKind.Interface;
        }

    }
}
