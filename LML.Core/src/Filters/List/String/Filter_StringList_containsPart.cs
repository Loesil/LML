using LML.Core.Models;

namespace LML.Core.Filters
{
    /// <summary>
    /// Filter that checks if a string property contains a specific value.
    /// </summary>
    public class Filter_StringList_containsPart : Filter_Property
    {
        private readonly string _value;
        private readonly bool _caseSensitive;

        /// <summary>
        /// Creates a new string containsPart filter.
        /// </summary>
        /// <param name="property">The property to filter on</param>
        /// <param name="value">The value to match against</param>
        /// <param name="caseSensitive">Whether the comparison should be case sensitive</param>
        public Filter_StringList_containsPart(MediaProperty property, string value, bool caseSensitive = false)
            : base(property, MediaPropertyType.StringList)
        {
            _value = value;
            _caseSensitive = caseSensitive;
        }

        /// <inheritdoc/>
        public override bool Apply(MediaFile mediaFile)
        {
            var propertyValue = (List<string>?)GetPropertyValue(mediaFile);
            if (propertyValue == null) return false;

            return propertyValue.Find((v) => _caseSensitive ? v.Contains(this._value) : v.ToLower().Contains(this._value.ToLower())) != null;
        }

        /// <inheritdoc/>
        public override FilterType GetFilterType()
        {
            return FilterType.StringListContainsPart;
        }

        /// <inheritdoc/>
        public override string GetFilterDescription()
        {
            var caseSensitivity = _caseSensitive ? "\"" : "'";
            return $"{PropertyToName(Property)!} < {caseSensitivity}{_value}{caseSensitivity}";
        }
    }
}