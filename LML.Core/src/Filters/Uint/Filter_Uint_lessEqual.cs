using LML.Core.Models;

namespace LML.Core.Filters
{
    /// <summary>
    /// Filter that checks if a uint property is less than or equal to a specific value.
    /// </summary>
    public class Filter_Uint_lessEqual : Filter_Property
    {
        private readonly uint _value;

        /// <summary>
        /// Creates a new uint less than or equal filter.
        /// </summary>
        /// <param name="property">The property to filter on</param>
        /// <param name="value">The value to compare against</param>
        public Filter_Uint_lessEqual(MediaProperty property, uint value)
            : base(property, MediaPropertyType.Uint)
        {
            _value = value;
        }

        /// <inheritdoc/>
        public override bool Apply(MediaFile mediaFile)
        {
            var propertyValue = GetPropertyValue(mediaFile);
            if (propertyValue == null) return false;

            return (uint)propertyValue <= _value;
        }

        /// <inheritdoc/>
        public override FilterType GetFilterType()
        {
            return FilterType.UintLessEqual;
        }

        /// <inheritdoc/>
        public override string GetFilterDescription(IFilter? parentFilter = null)
        {
            return $"{PropertyToName(Property)!} <= {_value}";
        }
    }
}