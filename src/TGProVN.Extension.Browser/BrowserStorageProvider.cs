using Microsoft.JSInterop;
using TGProVN.Extension.Browser.Abstractions;
using TGProVN.Extension.Browser.Exceptions;

namespace TGProVN.Extension.Browser;

public class BrowserStorageProvider(IJSRuntime jSRuntime) : IStorageProvider
{
    private const string STORAGE_NOT_AVAILABLE_MESSAGE =
        "Unable to access the browser storage. This is most likely due to the browser settings.";

    private readonly IJSInProcessRuntime? _jSInProcessRuntime = jSRuntime as IJSInProcessRuntime;

    public void Clear()
    {
        ExecVoid(() => _jSInProcessRuntime!.InvokeVoid("localStorage.clear"));
    }

    public async ValueTask ClearAsync(CancellationToken cancellationToken = default)
    {
        await ExecVoidAsync(async () => await jSRuntime.InvokeVoidAsync("localStorage.clear", cancellationToken));
    }

    public bool ContainKey(string key)
    {
        return Exec(() => _jSInProcessRuntime!.Invoke<bool>("localStorage.hasOwnProperty", key));
    }

    public ValueTask<bool> ContainKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        return ExecAsync(
            async () =>
                await jSRuntime.InvokeAsync<bool>("localStorage.hasOwnProperty", cancellationToken, key)
        );
    }

    public string? GetItem(string key)
    {
        return Exec(() => _jSInProcessRuntime!.Invoke<string?>("localStorage.getItem", key));
    }

    public ValueTask<string?> GetItemAsync(string key, CancellationToken cancellationToken = default)
    {
        return ExecAsync(async () =>
                             await jSRuntime.InvokeAsync<string?>("localStorage.getItem", cancellationToken, key)
        );
    }

    public string? Key(int index)
    {
        return Exec(() => _jSInProcessRuntime!.Invoke<string?>("localStorage.key", index));
    }

    public ValueTask<string?> KeyAsync(int index, CancellationToken cancellationToken = default)
    {
        return ExecAsync(async () =>
                             await jSRuntime.InvokeAsync<string?>("localStorage.key", cancellationToken, index)
        );
    }

    public IEnumerable<string>? Keys()
    {
        return Exec(() => _jSInProcessRuntime!.Invoke<IEnumerable<string>?>("eval", "Object.keys(localStorage)"));
    }

    public ValueTask<IEnumerable<string>?> KeysAsync(CancellationToken cancellationToken = default)
    {
        return ExecAsync(
            async () =>
                await jSRuntime.InvokeAsync<IEnumerable<string>?>("eval", cancellationToken,
                                                                  "Object.keys(localStorage)"
                )
        );
    }

    public int Length()
    {
        return Exec(() => _jSInProcessRuntime!.Invoke<int>("eval", "localStorage.length"));
    }

    public ValueTask<int> LengthAsync(CancellationToken cancellationToken = default)
    {
        return ExecAsync(
            async () =>
                await jSRuntime.InvokeAsync<int>("eval", cancellationToken, "localStorage.length")
        );
    }

    public void RemoveItem(string key)
    {
        ExecVoid(() => _jSInProcessRuntime!.InvokeVoid("localStorage.removeItem", key));
    }

    public async ValueTask RemoveItemAsync(string key, CancellationToken cancellationToken = default)
    {
        await ExecVoidAsync(async () =>
                                await jSRuntime.InvokeVoidAsync("localStorage.removeItem", cancellationToken, key));
    }

    public void RemoveItems(IEnumerable<string> keys)
    {
        foreach (var key in keys) {
            RemoveItem(key);
        }
    }

    public async ValueTask RemoveItemsAsync(IEnumerable<string> keys, CancellationToken cancellationToken = default)
    {
        foreach (var key in keys) {
            await RemoveItemAsync(key, cancellationToken);
        }
    }

    public void SetItem(string key, string data)
    {
        ExecVoid(() => _jSInProcessRuntime!.InvokeVoid("localStorage.setItem", key, data));
    }

    public async ValueTask SetItemAsync(string key, string data, CancellationToken cancellationToken = default)
    {
        await ExecVoidAsync(async () =>
                                await jSRuntime.InvokeVoidAsync("localStorage.setItem", cancellationToken, key, data)
        );
    }

    private void ExecVoid(Action action)
    {
        CheckInProcessRuntime();

        try {
            action();
        } catch (Exception exception) {
            if (IsStorageDisabledException(exception)) {
                throw new BrowserStorageDisabledException(STORAGE_NOT_AVAILABLE_MESSAGE, exception);
            }

            throw;
        }
    }

    private async ValueTask ExecVoidAsync(Func<Task> task)
    {
        CheckInProcessRuntime();

        try {
            await task();
        } catch (Exception exception) {
            if (IsStorageDisabledException(exception)) {
                throw new BrowserStorageDisabledException(STORAGE_NOT_AVAILABLE_MESSAGE, exception);
            }

            throw;
        }
    }

    private T? Exec<T>(Func<T?> func)
    {
        CheckInProcessRuntime();

        try {
            return func();
        } catch (Exception exception) {
            if (IsStorageDisabledException(exception)) {
                throw new BrowserStorageDisabledException(STORAGE_NOT_AVAILABLE_MESSAGE, exception);
            }

            throw;
        }
    }

    private async ValueTask<T?> ExecAsync<T>(Func<Task<T?>> task)
    {
        CheckInProcessRuntime();

        try {
            return await task();
        } catch (Exception exception) {
            if (IsStorageDisabledException(exception)) {
                throw new BrowserStorageDisabledException(STORAGE_NOT_AVAILABLE_MESSAGE, exception);
            }

            throw;
        }
    }

    private void CheckInProcessRuntime()
    {
        if (_jSInProcessRuntime == null) {
            throw new InvalidOperationException("IJSInProcessRuntime not available");
        }
    }

    private static bool IsStorageDisabledException(Exception exception)
    {
        return exception.Message.Contains("Failed to read the 'localStorage' property from 'Window'");
    }
}