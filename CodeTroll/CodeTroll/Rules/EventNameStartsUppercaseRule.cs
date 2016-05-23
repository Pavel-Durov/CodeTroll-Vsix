using CodeTroll.Infra;
using Microsoft.CodeAnalysis;
using System;
using CodeTroll.Extentions;

namespace CodeTroll.Rules
{
    class EventNameStartsUppercaseRule
    {
        public static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.UppercaseEventNameTitle), Resources.ResourceManager, typeof(Resources));

        public static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.UppercaseEventNameContent), Resources.ResourceManager, typeof(Resources));

        public static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.UppercaseEventNameMessageFormat), Resources.ResourceManager, typeof(Resources));

        public readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(Const.DIAGNOSTIC_ID, Title, MessageFormat, Const.NAMING_CATEGORY, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        internal bool Match(IEventSymbol @event)
        {
            return !String.IsNullOrEmpty(@event.Name) && @event.Name.StartsWithUpperCase();
        }
    }
}
