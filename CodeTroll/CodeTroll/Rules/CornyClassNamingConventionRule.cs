using CodeTroll.Infra;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTroll.Rules
{
    class CornyClassNamingConventionRule
    {
        public static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.CornyClassNamingConventionTitle), Resources.ResourceManager, typeof(Resources));

        public static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.CornyClassNamingConventionMessageformat), Resources.ResourceManager, typeof(Resources));

        public static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.CornyClassNamingConventionContent), Resources.ResourceManager, typeof(Resources));

        public readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(Const.DIAGNOSTIC_ID, Title, MessageFormat, Const.NAMING_CATEGORY, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);


        readonly string[] STRINGS = new string[] { "provider", "service", "hadler", "helper", "dispatcher" };

        public bool Match(INamedTypeSymbol namedTypeSymbol)
        {
            bool result = false;

            var lower = namedTypeSymbol.Name.ToLower();

            foreach (var item in STRINGS)
            {
                if (lower.Contains(item))
                {
                    result = true;
                    break;
                }
            }

            return result && namedTypeSymbol.TypeKind == TypeKind.Class;
        }
    }
}
