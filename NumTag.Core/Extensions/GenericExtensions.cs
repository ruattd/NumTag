namespace NumTag.Core.Extensions;

public static class GenericExtensions
{
    extension<T>(T generic)
    {
        public void Let(Action<T> func) => func(generic);
        public TReturn Let<TReturn>(Func<T, TReturn> func) => func(generic);

        public T Also(Action<T> func)
        {
            func(generic);
            return generic;
        }
    }
}
