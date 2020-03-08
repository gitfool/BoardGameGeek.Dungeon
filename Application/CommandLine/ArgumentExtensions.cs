using System;
using System.CommandLine;
using System.Globalization;
using System.Linq;

namespace BoardGameGeek.Dungeon.CommandLine
{
    public static class ArgumentExtensions
    {
        public static Argument<DateTime> WithFormat(this Argument<DateTime> argument, string format)
        {
            argument.AddValidator(symbol => symbol.Tokens
                .Select(token => token.Value)
                .Where(value => !DateTime.TryParseExact(value, format, null, DateTimeStyles.None, out _))
                .Select(_ => $@"Date must use format ""{format}""")
                .FirstOrDefault());

            return argument;
        }
    }
}
