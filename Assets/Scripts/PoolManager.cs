using UnityEngine;
using UnityEngine.Pool;

public class PoolManager<T> where T : class
{
    private readonly ObjectPool<T> pool;

    public PoolManager(
        System.Func<T> createFunc,
        System.Action<T> onGet      = null,
        System.Action<T> onRelease  = null,
        System.Action<T> onDestroy  = null,
        bool collectionCheck        = true,
        int defaultCapacity         = 10,
        int maxSize                 = 50
    )
    {
        pool = new ObjectPool<T>(
            createFunc:   createFunc,
            actionOnGet:  onGet,
            actionOnRelease: onRelease,
            actionOnDestroy: onDestroy,
            collectionCheck: collectionCheck,
            defaultCapacity: defaultCapacity,
            maxSize: maxSize
        );
    }

    /// <summary>
    /// Get an instance from the pool.
    /// </summary>
    public T Get()
    {
        return pool.Get();
    }

    /// <summary>
    /// Return an instance to the pool.
    /// </summary>
    public void Return(T item)
    {
        pool.Release(item);
    }

    /// <summary>
    /// Clean up the pool and destroy all pooled objects.
    /// </summary>
    public void Dispose()
    {
        pool.Dispose();
    }
}
