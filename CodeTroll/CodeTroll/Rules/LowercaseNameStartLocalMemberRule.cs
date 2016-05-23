using CodeTroll.Infra;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CodeTroll.Extentions;

namespace CodeTroll.Rules
{
    class LowercaseNameStartLocalMemberRule
    {
        public static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.LowerCaseMemberNameStartNameTitle), Resources.ResourceManager, typeof(Resources));

        public static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.LowerCaseMemberNameStartMessageFormat), Resources.ResourceManager, typeof(Resources));

        public static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.LowerCaseMemberNameStartContent), Resources.ResourceManager, typeof(Resources));

        public readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(Const.DIAGNOSTIC_ID, Title, MessageFormat, Const.NAMING_CATEGORY, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        internal bool Match(VariableDeclaratorSyntax variableDeclaration)
        {
            var varName = variableDeclaration.Identifier.Text;
            return !String.IsNullOrEmpty(varName) && varName.StartsWithLowerCase();
        }
    }
}
