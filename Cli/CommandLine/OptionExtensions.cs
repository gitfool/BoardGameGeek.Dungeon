using System;
using System.CommandLine;
using System.Globalization;
using System.Linq;

namespace BoardGameGeek.Dungeon.CommandLine
{
    public static class OptionExtensions
    {
        public static Option<DateTime> WithFormat(this Option<DateTime> option, string format)
        {
            option.Argument.AddValidator(symbol => symbol.Tokens
                .Select(token => token.Value)
                .Where(value => !DateTime.TryParseExact(value, format, null, DateTimeStyles.None, out _))
                .Select(_ => $@"Date must use format ""{format}""")
                .FirstOrDefault());

            return option;
        }
    }
}
