namespace James.Shared.Model
{
    public interface ITieredValue<T>
    {
        public int Minimum { get; set; }
        public int? Maximum { get; set; }
        /// <summary>
        /// This value for the tier
        /// </summary>
        /// <remarks>This is typically a reference to an existing property.
        /// If the concrete object is an entity object used in GraphQL, the
        /// object will need a Value property added in a partial class file
        /// that is decorated with a [GraphQLIgnore] and [NotMapped] attributes
        /// to cause it to be ignored by Entities Framework Core and HotChocolate
        /// respectively.</remarks>
        public T Value { get; set; }
    }
}
