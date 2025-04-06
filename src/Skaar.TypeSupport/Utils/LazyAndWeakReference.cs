namespace Skaar.TypeSupport.Utils;

/// <summary>
/// Combines <see cref="Lazy{T}"/> and <see cref="WeakReference{T}"/>
/// to allow deferred instanciation and eager garbage collection of <see cref="Value"/>
/// </summary>
/// <param name="initializeTarget">A method to create and recreate <see cref="Value"/>.</param>
/// <typeparam name="T">A potential memory costly resource.</typeparam>
public class LazyAndWeakReference<T>(Func<T> initializeTarget) where T : class
{
    private readonly Lazy<WeakReference<T>> _weakReference = new(() => new WeakReference<T>(initializeTarget()));

    public T Value
    {
        get
        {
            if (!_weakReference.Value.TryGetTarget(out var target))
            {
                target = initializeTarget();
                _weakReference.Value.SetTarget(target);
            }
            return target;
        }
    }
}