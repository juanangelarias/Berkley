namespace James.Shared.Model
{
    public interface ITieredValue<T>
    {
        public int Minimum { get; set; }
        public int? Maximum { get; set; }
        public T Value { get; set; }
    }
}
