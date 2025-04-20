using LML.Core.Models;

namespace LML.Core.Filters
{
    /// <summary>
    /// Filter that checks a bool property.
    /// </summary>
    public class Filter_Bool : Filter_Property
    {
        /// <summary>
        /// Creates a new bool filter.
        /// </summary>
        /// <param name="property">The property to filter on</param>
        /// <param name="value">The value to match against</param>
        public Filter_Bool(MediaProperty property)
            : base(property, MediaPropertyType.Boolean)
        {
        }

        /// <inheritdoc/>
        public override bool Apply(MediaFile mediaFile)
        {
            var propertyValue = GetPropertyValue(mediaFile);
            if (propertyValue == null) return false;

            return (bool)propertyValue;
        }

        /// <inheritdoc/>
        public override FilterType GetFilterType()
        {
            return FilterType.Bool;
        }

        /// <inheritdoc/>
        public override string GetFilterDescription()
        {
            return PropertyToName(Property)!;
        }
    }
}