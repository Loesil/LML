using LML.Core.Models;

namespace LML.Core.Filters
{
    /// <summary>
    /// Filter that checks if a string property is equal to a specific value.
    /// </summary>
    public class Filter_StringList_equal : Filter_Property
    {
        private readonly string _value;
        private readonly bool _caseSensitive;

        /// <summary>
        /// Creates a new string equals filter.
        /// </summary>
        /// <param name="property">The property to filter on</param>
        /// <param name="value">The value to match against</param>
        /// <param name="caseSensitive">Whether the comparison should be case sensitive</param>
        public Filter_StringList_equal(MediaProperty property, string value, bool caseSensitive = false)
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

            List<string> values = _value.Replace(";", ",").Split(",").ToList().ConvertAll(v => v.Trim()).ConvertAll(v => _caseSensitive ? v : v.ToLower());
            if (values.Count != propertyValue.Count) return false;
            foreach (string value in values)
            {
                if (!propertyValue.Contains(value)) return false;
            }

            return true;
        }

        /// <inheritdoc/>
        public override FilterType GetFilterType()
        {
            return FilterType.StringListEqual;
        }

        /// <inheritdoc/>
        public override string GetFilterDescription()
        {
            var caseSensitivity = _caseSensitive ? "\"" : "'";
            return $"{PropertyToName(Property)!} == {caseSensitivity}{_value}{caseSensitivity}";
        }
    }
}