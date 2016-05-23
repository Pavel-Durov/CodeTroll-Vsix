using CodeTroll.CodeFix;
using CodeTroll.Infra;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTroll.Rules
{
    class InterfaceNamingConventionRule
    {
        public static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.InterfaceNamingConventionTitle), Resources.ResourceManager, typeof(Resources));

        public static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.InterfaceNamingConventionMessageFormat), Resources.ResourceManager, typeof(Resources));

        public static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.InterfaceNamingConventionContent), Resources.ResourceManager, typeof(Resources));

        public readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(Const.DIAGNOSTIC_ID, Title, MessageFormat, Const.NAMING_CATEGORY, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public bool Match(INamedTypeSymbol namedTypeSymbol)
        {
            bool result = false;

            var lower = namedTypeSymbol.Name.ToLower();

            if (!string.IsNullOrEmpty(lower) && (!lower.StartsWith("i") && !lower.Contains(InterfaceExplicitNamingConventionCodeFix.INTERFACE_NAMING_CONVENTION_CODE_FIX_POSTFIX.ToLower())))
            {
                result = true;
            }

            return result && namedTypeSymbol.TypeKind == TypeKind.Interface;
        }
    }
}
