using System.Text.Json;
using TGProVN.Extension.Browser.Abstractions;
using TGProVN.Extension.Browser.Events;
using TGProVN.Extension.Serialization.Abstractions.Serializers;

namespace TGProVN.Extension.Browser;

public class LocalStorageService(IStorageProvider storageProvider,
                                 IJsonSerializer serializer)
    : ILocalStorageService
{
    public void Clear()
    {
        storageProvider.Clear();
    }

    public T? GetItem<T>(string key)
    {
        var item = storageProvider.GetItem(key);

        return string.IsNullOrWhiteSpace(item) ? default : Deserialize<T>(item);
    }

    public async ValueTask<T?> GetItemAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var item = await storageProvider.GetItemAsync(key, cancellationToken);
        
        return string.IsNullOrWhiteSpace(item) ? default : Deserialize<T>(item);
    }

    public string? Key(int index)
    {
        return storageProvider.Key(index);
    }

    public bool ContainKey(string key)
    {
        return storageProvider.ContainKey(key);
    }

    public int Length()
    {
        return storageProvider.Length();
    }

    public IEnumerable<string>? Keys()
    {
        return storageProvider.Keys();
    }

    public void RemoveItem(string key)
    {
        storageProvider.RemoveItem(key);
    }

    public void RemoveItems(IEnumerable<string> keys)
    {
        storageProvider.RemoveItems(keys);
    }

    public void SetItem<T>(string key, T data)
    {
        var e = RaiseOnChanging(key, data);

        if (e.Cancel) {
            return;
        }

        var serialisedData = serializer.Serialize(data);
        
        storageProvider.SetItem(key, serialisedData);
        
        RaiseOnChanged(key, e.OldValue, data);
    }

    public ValueTask ClearAsync(CancellationToken cancellationToken = default)
    {
        return storageProvider.ClearAsync(cancellationToken);
    }

    public ValueTask<string?> KeyAsync(int index, CancellationToken cancellationToken = default)
    {
        return storageProvider.KeyAsync(index, cancellationToken);
    }

    public ValueTask<IEnumerable<string>?> KeysAsync(CancellationToken cancellationToken = default)
    {
        return storageProvider.KeysAsync(cancellationToken);
    }

    public ValueTask<bool> ContainKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        return storageProvider.ContainKeyAsync(key, cancellationToken);
    }

    public ValueTask<int> LengthAsync(CancellationToken cancellationToken = default)
    {
        return storageProvider.LengthAsync(cancellationToken);
    }

    public ValueTask RemoveItemAsync(string key, CancellationToken cancellationToken = default)
    {
        return storageProvider.RemoveItemAsync(key, cancellationToken);
    }

    public ValueTask RemoveItemsAsync(IEnumerable<string> keys, CancellationToken cancellationToken = default)
    {
        return storageProvider.RemoveItemsAsync(keys, cancellationToken);
    }

    public async ValueTask SetItemAsync<T>(string key, T data, CancellationToken cancellationToken = default)
    {
        var e = await RaiseOnChangingAsync(key, data).ConfigureAwait(false);

        if (e.Cancel) {
            return;
        }

        var serialisedData = serializer.Serialize(data);
        
        await storageProvider.SetItemAsync(key, serialisedData, cancellationToken).ConfigureAwait(false);

        RaiseOnChanged(key, e.OldValue, data);
    }

    private T? Deserialize<T>(string item)
    {
        try
        {
            return serializer.Deserialize<T>(item);
        }
        catch (JsonException)
        {
            return (T)(object)item;
        }
    }

    public event EventHandler<ChangingEventArgs>? Changing;

    private async Task<ChangingEventArgs> RaiseOnChangingAsync(string key, object? data)
    {
        var e = new ChangingEventArgs {
            Key = key,
            OldValue = await GetItemInternalAsync<object>(key).ConfigureAwait(false),
            NewValue = data
        };

        Changing?.Invoke(this, e);

        return e;
    }
    
    private ChangingEventArgs RaiseOnChanging(string key, object? data)
    {
        var e = new ChangingEventArgs
        {
            Key = key,
            OldValue = GetItemInternal(key),
            NewValue = data
        };

        Changing?.Invoke(this, e);

        return e;
    }

    private async Task<T?> GetItemInternalAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(key)) {
            throw new ArgumentNullException(nameof(key));
        }

        var serialisedData = await storageProvider.GetItemAsync(key, cancellationToken).ConfigureAwait(false);

        if (string.IsNullOrWhiteSpace(serialisedData))
            return default;

        try {
            return serializer.Deserialize<T>(serialisedData);
        } catch (JsonException) {
            return (T)(object)serialisedData;
        }
    }

    private object? GetItemInternal(string key)
    {
        if (string.IsNullOrEmpty(key))
            throw new ArgumentNullException(nameof(key));

        var serialisedData = storageProvider.GetItem(key);

        if (string.IsNullOrWhiteSpace(serialisedData))
            return default;

        try {
            return serializer.Deserialize<object>(serialisedData);
        } catch (JsonException) {
            return serialisedData;
        }
    }

    public event EventHandler<ChangedEventArgs>? Changed;
    
    private void RaiseOnChanged(string key, object? oldValue, object? data)
    {
        var e = new ChangedEventArgs
        {
            Key = key,
            OldValue = oldValue,
            NewValue = data
        };

        Changed?.Invoke(this, e);
    }
}