using Sprache;
using System.Linq.Expressions;
using LML.Core.Models;

namespace LML.Core.Filters
{
    public static class FilterParser
    {
        #region Parser Components
        // identifier (all words)
        private static readonly Parser<string> Identifier =
            from first in Parse.Letter.Or(Parse.Char('_'))
            from rest in Parse.LetterOrDigit.Or(Parse.Char('_')).Many().Text()
            select first + rest;

        // string literal (with " or ')
        private static readonly Parser<string> StringLiteral =
            from open in Parse.Char('"').Or(Parse.Char('\''))
            from content in Parse.CharExcept(open).Many().Text()
            from close in Parse.Char(open)
            select open + content + open;

        // whitespace (space, tab, newline, carriage return)
        private static readonly Parser<char> WhitespaceChar =
            Parse.Char(' ').Or(Parse.Char('\t')).Or(Parse.Char('\n')).Or(Parse.Char('\r'));

        // optional whitespace (zero or more whitespace characters)
        private static readonly Parser<string> OptionalWhitespace =
            WhitespaceChar.Many().Text();

        // operator (==, !=, <=, >=, <, >)
        private static readonly Parser<ExpressionType> Operator =
            Parse.String("<=").Return(ExpressionType.LessThanOrEqual)
            .Or(Parse.String(">=").Return(ExpressionType.GreaterThanOrEqual))
            .Or(Parse.String("==").Return(ExpressionType.Equal))
            .Or(Parse.String("!=").Return(ExpressionType.NotEqual))
            .Or(Parse.String("<").Return(ExpressionType.LessThan))
            .Or(Parse.String(">").Return(ExpressionType.GreaterThan));

        // logical operator (&, |)
        private static readonly Parser<ExpressionType> LogicalOperator =
            Parse.String("&").Return(ExpressionType.AndAlso)
            .Or(Parse.String("|").Return(ExpressionType.OrElse));

        // property (identifier)
        private static readonly Parser<MediaProperty> Property =
            from identifier in Identifier
            select CreateMediaProperty(identifier);

        // value (string literal or number)
        private static readonly Parser<object> Value =
            StringLiteral.Select(str => (object)str)
            .Or(Parse.Number.Select(num => (object)int.Parse(num)));

        // comparison (property operator value)
        private static readonly Parser<IFilter> Comparison =
            from left in Property
            from ws1 in OptionalWhitespace
            from op in Operator
            from ws2 in OptionalWhitespace
            from right in Value
            select CreateComparisonFilter(left, op, right);

        // factor (comparison or not)
        private static readonly Parser<IFilter> Factor =
            Parse.Char('(').Then(_ => Parse.Ref(() => Filter)).Then(e => Parse.Char(')').Return(e))
            .Or(Parse.Char('!').Then(_ => Parse.Ref(() => Factor)).Select(e => new Filter_not(e)))
            .Or(Comparison)
            .Or(Identifier.Select(id => CreatePropertyFilter(id)));

        // term (logical operator factor)
        private static readonly Parser<IFilter> Term =
            Parse.ChainOperator(LogicalOperator, Factor, CreateLogicalFilter);

        // filter (term)
        private static readonly Parser<IFilter> Filter = Term;
        #endregion

        #region Helper Methods
        private static MediaProperty CreateMediaProperty(string propertyName)
        {
            var property = Filter_Property.NameToProperty(propertyName);
            if (property == null) throw new ArgumentException($"Unknown property: {propertyName}");
            return (MediaProperty)property!;
        }

        private static IFilter CreatePropertyFilter(string propertyName)
        {
            var property = Filter_Property.NameToProperty(propertyName);
            if (property == null) throw new ArgumentException($"Unknown property: {propertyName}");

            return property switch
            {
                // bool
                MediaProperty.Exists => new Filter_Bool(MediaProperty.Exists),
                MediaProperty.Local => new Filter_Bool(MediaProperty.Local),
                MediaProperty.Audio => new Filter_Bool(MediaProperty.Audio),
                MediaProperty.Video => new Filter_Bool(MediaProperty.Video),

                _ => throw new ArgumentException($"Unsupported property for filter: {propertyName}")
            };
        }

        private static IFilter CreateComparisonFilter(MediaProperty property, ExpressionType op, object right)
        {
            // get type and values
            MediaPropertyType? type = Filter_Property.PropertyToPropertyType(property);
            uint? val_uint = (type == MediaPropertyType.Uint ? (uint)(int)right : null);
            string? val_string = (type == MediaPropertyType.String || type == MediaPropertyType.StringList ? (string)right : null);
            string? val_stringText = val_string?.Substring(1, val_string.Length - 2);
            bool caseSensitive = (val_string != null ? val_string[0] == '\'' : false);

            return type switch
            {
                // string
                MediaPropertyType.String => op switch
                {
                    ExpressionType.Equal => new Filter_String_equal(property, val_stringText!, caseSensitive),
                    ExpressionType.NotEqual => new Filter_not(new Filter_String_equal(property, val_stringText!, caseSensitive)),
                    _ => throw new ArgumentException($"Unsupported operator for string: {op}")
                },

                // string list
                MediaPropertyType.StringList => op switch
                {
                    ExpressionType.Equal => new Filter_StringList_equal(property, val_stringText!, caseSensitive),
                    ExpressionType.LessThanOrEqual => new Filter_StringList_contains(property, val_stringText!, caseSensitive),
                    ExpressionType.LessThan => new Filter_StringList_containsPart(property, val_stringText!, caseSensitive),
                    _ => throw new ArgumentException($"Unsupported operator for string list: {op}")
                },

                // uint
                MediaPropertyType.Uint => op switch
                {
                    ExpressionType.Equal => new Filter_Uint_equal(property, (uint)val_uint!),
                    ExpressionType.NotEqual => new Filter_Uint_notEqual(property, (uint)val_uint!),
                    ExpressionType.LessThan => new Filter_Uint_less(property, (uint)val_uint!),
                    ExpressionType.LessThanOrEqual => new Filter_Uint_lessEqual(property, (uint)val_uint!),
                    ExpressionType.GreaterThan => new Filter_Uint_greater(property, (uint)val_uint!),
                    ExpressionType.GreaterThanOrEqual => new Filter_Uint_greaterEqual(property, (uint)val_uint!),
                    _ => throw new ArgumentException($"Unsupported operator for uint: {op}")
                },

                _ => throw new ArgumentException($"Unsupported property type: {type}")
            };
        }

        private static IFilter CreateLogicalFilter(ExpressionType op, IFilter left, IFilter right)
        {
            return op switch
            {
                ExpressionType.AndAlso => new Filter_and(new[] { left, right }),
                ExpressionType.OrElse => new Filter_or(new[] { left, right }),
                _ => throw new ArgumentException($"Unsupported logical operator: {op}")
            };
        }
        #endregion

        public static IFilter ParseFilter(string filterString)
        {
            try
            {
                return Filter.Parse(filterString);
            }
            catch (ParseException ex)
            {
                throw new ArgumentException($"Invalid filter expression: {ex.Message}", ex);
            }
        }
    }
}