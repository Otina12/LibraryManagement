namespace Library.Model.Abstractions
{
    public class Result<Tvalue> : Result
    {
        private readonly Tvalue? _value;

        protected internal Result(Tvalue? value, bool IsSuccess, Error error) : base(IsSuccess, error)
        {
            _value = value;
        }

        public Tvalue Value()
        {
            return IsSuccess ? _value! : throw new InvalidOperationException("The value of a failure result cannot be accessed");
        }

        public static implicit operator Result<Tvalue>(Tvalue? value) => Create(value);
    }
}
