namespace Board.Domain;
/// <summary>
/// Implement the dispose pattern
/// https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose
/// </summary>
public abstract class BaseDisposable : IDisposable
{
    // To detect redundant calls
    private bool disposed = false;
    protected abstract void DisposeManagedResources();

    ~BaseDisposable() => Dispose(false);

    // Public implementation of Dispose pattern callable by consumers.
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    // Protected implementation of Dispose pattern.
    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                // dispose managed state (managed objects)
                DisposeManagedResources();
            }
            // free unmanaged resources (unmanaged objects) and override a finalizer below
            disposed = true;
        }
    }
}
