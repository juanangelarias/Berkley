namespace James.Shared.Model
{
    public partial class ImagingType
    {
        //Used to simplify comparing types
        
        public override int GetHashCode() => Type.GetHashCode();
        public override bool Equals(object? obj)
        {
            if (obj is string str)
                return str == Type;
            if (obj is ImagingType ty)
                return ty.Type == Type || (ty.Id != Guid.Empty && ty.Id==Id);
            return false;
        }
    }
}
