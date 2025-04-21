using Sprache;
using System.Linq.Expressions;
using LML.Core.Models;

namespace LML.Core.Filters
{
    public static class FilterParser
    {
        #region Parser Components
        #region Literals
        // String literal (single or double quoted)
        private static readonly Parser<string> StringLiteral =
            from open in Parse.Char('"').Or(Parse.Char('\''))
            from content in Parse.CharExcept(open).Many().Text()
            from close in Parse.Char(open)
            select open + content + open; // include quotes

        // Number literal
        private static readonly Parser<int> NumberLiteral =
            Parse.Number.Select(int.Parse);

        // Value: either string or number
        private static readonly Parser<object> Value =
            (
                StringLiteral.Select(x => (object)x)
                .Or(NumberLiteral.Select(x => (object)x))
            )
            .Token();
        #endregion

        #region Identifiers and Properties
        // Identifier (letters, digits, underscores)
        private static readonly Parser<string> Identifier =
            from first in Parse.Letter.Or(Parse.Char('_'))
            from rest in Parse.LetterOrDigit.Or(Parse.Char('_')).Many().Text()
            select first + rest;

        // Property
        private static readonly Parser<MediaProperty> Property =
            Identifier.Select(CreateMediaProperty).Token();
        #endregion

        #region Operators and Comparisons
        // Comparison operator
        private static readonly Parser<ExpressionType> Operator =
            (
                Parse.String("<=").Return(ExpressionType.LessThanOrEqual)
                .Or(Parse.String(">=").Return(ExpressionType.GreaterThanOrEqual))
                .Or(Parse.String("==").Return(ExpressionType.Equal))
                .Or(Parse.String("!=").Return(ExpressionType.NotEqual))
                .Or(Parse.String("<").Return(ExpressionType.LessThan))
                .Or(Parse.String(">").Return(ExpressionType.GreaterThan))
            ).Token();

        // Property comparison (property operator value)
        private static readonly Parser<IFilter> PropertyValueComparison =
            from prop in Property
            from op in Operator
            from val in Value
            select CreateComparisonFilter(prop, op, val);
        #endregion

        #region Parentheses, Not, Atoms
        // Forward declaration for recursion
        private static readonly Parser<IFilter> Parentheses =
            from l in Parse.Char('(').Token()
            from expr in Parse.Ref(() => Term)
            from r in Parse.Char(')').Token()
            select expr;

        // Not (!)
        private static readonly Parser<IFilter> Not =
            from not in Parse.Char('!').Token()
            from expr in Parse.Ref(() => Atom)
            select new Filter_not(expr);

        // Atom = base building block
        private static readonly Parser<IFilter> Atom =
            PropertyValueComparison
            .Or(Identifier.Select(CreatePropertyFilter));

        // Factor = negation, parentheses, or atom
        private static readonly Parser<IFilter> Factor =
            Parentheses
            .Or(Not)
            .Or(Atom);
        #endregion

        #region Logical Expressions
        private static readonly Parser<ExpressionType> AndOperator =
            Parse.String(Filter_and.Operator).Token().Return(ExpressionType.AndAlso);

        private static readonly Parser<ExpressionType> OrOperator =
            Parse.String(Filter_or.Operator).Token().Return(ExpressionType.OrElse);

        // Logical AND
        private static readonly Parser<IFilter> LogicalAnd =
            Parse.ChainOperator(AndOperator, Factor, CreateLogicalFilter);

        // Logical OR
        private static readonly Parser<IFilter> LogicalOr =
            Parse.ChainOperator(OrOperator, LogicalAnd, CreateLogicalFilter);
        #endregion

        private static readonly Parser<IFilter> Term =
            Parentheses
            .Or(LogicalOr)
            .Or(Atom);

        #region Entry Point
        // Entry point parser (consumes entire input)
        private static readonly Parser<IFilter> Filter = Term.End();
        #endregion
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
            uint? val_uint = type == MediaPropertyType.Uint ? (uint)(int)right : null;
            string? val_string = type == MediaPropertyType.String || type == MediaPropertyType.StringList ? (string)right : null;
            string? val_stringText = val_string?.Substring(1, val_string.Length - 2);
            bool caseSensitive = val_string != null ? val_string[0] == '"' : false;

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
                ExpressionType.AndAlso => new Filter_and(left, right),
                ExpressionType.OrElse => new Filter_or(left, right),
                _ => throw new ArgumentException($"Unsupported logical operator: {op}")
            };
        }
        #endregion

        public static IFilter ParseFilter(string filterString)
        {
            try
            {
                var ret = Filter.TryParse(filterString);
                if (!ret.WasSuccessful)
                {
                    throw new ArgumentException(ret.Message);
                }
                else if (!ret.Remainder.AtEnd)
                {
                    throw new ArgumentException($"Only parsed: {ret.Remainder.Source.Substring(0, ret.Remainder.Position)}");
                }
                return ret.Value;
            }
            catch (ParseException ex)
            {
                throw new ArgumentException($"Invalid filter expression: {ex.Message}", ex);
            }
        }
    }
}