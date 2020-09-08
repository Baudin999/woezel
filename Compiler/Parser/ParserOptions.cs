using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Parser
{
    public sealed class ParserOptions
    {
        public static readonly ParserOptions Default = new ParserOptions();
        public static readonly ParserOptions OptionalSemicolons = new ParserOptions { EnforceSemicolons = false };

        public bool AllowRootStatements { get; set; }

        public bool EnforceSemicolons { get; set; }

        public ParserOptions()
        {
            EnforceSemicolons = true;
            AllowRootStatements = true;
        }
    }
}
