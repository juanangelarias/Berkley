namespace James.Shared.Dto;

public class LazyLoadData<T>
where T : class
{
    public List<T> Data { get; set; } = [];
    public int Count { get; set; }

    public LazyLoadData()
    {
    }

    public LazyLoadData(List<T> data, int count) 
        : this()
    {
        Data = data;
        Count = count;       
    }
}