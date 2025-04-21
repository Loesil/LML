using LML.Core.Models;

namespace LML.Core.Filters
{
    /// <summary>
    /// Filter that checks if a string property equals a specific value.
    /// </summary>
    public class Filter_String_equal : Filter_Property
    {
        private readonly string _value;
        private readonly bool _caseSensitive;

        /// <summary>
        /// Creates a new string equality filter.
        /// </summary>
        /// <param name="property">The property to filter on</param>
        /// <param name="value">The value to match against</param>
        /// <param name="caseSensitive">Whether the comparison should be case sensitive</param>
        public Filter_String_equal(MediaProperty property, string value, bool caseSensitive = false)
            : base(property, MediaPropertyType.String)
        {
            _value = value;
            _caseSensitive = caseSensitive;
        }

        /// <inheritdoc/>
        public override bool Apply(MediaFile mediaFile)
        {
            var propertyValue = GetPropertyValue(mediaFile)?.ToString();
            if (propertyValue == null) return false;

            return _caseSensitive
                ? propertyValue == _value
                : propertyValue.ToLower() == _value.ToLower();
        }

        /// <inheritdoc/>
        public override FilterType GetFilterType()
        {
            return FilterType.StringEqual;
        }

        /// <inheritdoc/>
        public override string GetFilterDescription(IFilter? parentFilter = null)
        {
            var caseSensitivity = _caseSensitive ? "\"" : "'";
            return $"{PropertyToName(Property)!} == {caseSensitivity}{_value}{caseSensitivity}";
        }
    }
}