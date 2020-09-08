using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
{
    public enum TokenCatagory
    {
        Unknown,
        WhiteSpace,
        Comment,

        Constant,
        Identifier,
        Grouping,
        Punctuation,
        Operator,

        Invalid,
    }
}
