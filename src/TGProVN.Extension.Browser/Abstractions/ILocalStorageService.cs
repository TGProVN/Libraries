using TGProVN.Extension.Browser.Events;

namespace TGProVN.Extension.Browser.Abstractions;

public interface ILocalStorageService
{
    /// <summary>
    /// Clears all data from local storage.
    /// </summary>
    void Clear();

    /// <summary>
    /// Retrieve the specified data from local storage as a <typeparamref name="T"/>.
    /// </summary>
    /// <param name="key">A <see cref="string"/> value specifying the name of the local storage slot to use</param>
    /// <returns>The data from the specified <paramref name="key"/> as a <typeparamref name="T"/></returns>
    T? GetItem<T>(string key);

    /// <summary>
    /// Asynchronously retrieve the specified data from local storage and deserialize it to the specified type.
    /// </summary>
    /// <param name="key">A <see cref="string"/> value specifying the name of the local storage slot to use</param>
    /// <param name="cancellationToken">
    /// A cancellation token to signal the cancellation of the operation. Specifying this parameter will override any default cancellations such as due to timeouts
    /// <see cref="P:Microsoft.JSInterop.JSRuntime.DefaultAsyncTimeout" /> from being applied.
    /// </param>
    /// <returns>A <see cref="ValueTask"/> representing the completion of the operation.</returns>
    ValueTask<T?> GetItemAsync<T>(string key, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Return the name of the key at the specified <paramref name="index"/>.
    /// </summary>
    /// <param name="index"></param>
    /// <returns>The name of the key at the specified <paramref name="index"/></returns>
    string? Key(int index);

    /// <summary>
    /// Checks if the <paramref name="key"/> exists in local storage, but does not check its value.
    /// </summary>
    /// <param name="key">A <see cref="string"/> value specifying the name of the storage slot to use</param>
    /// <returns>Boolean indicating if the specified <paramref name="key"/> exists</returns>
    bool ContainKey(string key);

    /// <summary>
    /// The number of items stored in local storage.
    /// </summary>
    /// <returns>The number of items stored in local storage</returns>
    int Length();

    /// <summary>
    /// Get the keys of all items stored in local storage.
    /// </summary>
    /// <returns>The keys of all items stored in local storage</returns>
    IEnumerable<string>? Keys();

    /// <summary>
    /// Remove the data with the specified <paramref name="key"/>.
    /// </summary>
    /// <param name="key">A <see cref="string"/> value specifying the name of the storage slot to remove</param>
    void RemoveItem(string key);

    /// <summary>
    /// Removes a collection of <paramref name="keys"/>.
    /// </summary>
    /// <param name="keys">A IEnumerable collection of strings specifying the name of the storage slot to remove</param>
    void RemoveItems(IEnumerable<string> keys);

    /// <summary>
    /// Sets or updates the <paramref name="data"/> in local storage with the specified <paramref name="key"/>.
    /// </summary>
    /// <param name="key">A <see cref="string"/> value specifying the name of the storage slot to use</param>
    /// <param name="data">The data to be saved</param>
    void SetItem<T>(string key, T data);
    
    /// <summary>
    /// Asynchronously clears all data from local storage.
    /// </summary>
    /// <returns>A <see cref="ValueTask"/> representing the completion of the operation.</returns>
    ValueTask ClearAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously return the name of the key at the specified <paramref name="index"/>.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="cancellationToken">
    /// A cancellation token to signal the cancellation of the operation. Specifying this parameter will override any default cancellations such as due to timeouts
    /// (<see cref="P:Microsoft.JSInterop.JSRuntime.DefaultAsyncTimeout"/>) from being applied.
    /// </param>
    /// <returns>A <see cref="ValueTask"/> representing the completion of the operation.</returns>
    ValueTask<string?> KeyAsync(int index, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously returns a collection of strings representing the names of the keys in the local storage.
    /// </summary>
    /// <param name="cancellationToken">
    /// A cancellation token to signal the cancellation of the operation. Specifying this parameter will override any default cancellations such as due to timeouts
    /// (<see cref="P:Microsoft.JSInterop.JSRuntime.DefaultAsyncTimeout"/>) from being applied.
    /// </param>
    /// <returns>A <see cref="ValueTask"/> representing the completion of the operation.</returns>
    ValueTask<IEnumerable<string>?> KeysAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously checks if the <paramref name="key"/> exists in local storage, but does not check its value.
    /// </summary>
    /// <param name="key">A <see cref="string"/> value specifying the name of the storage slot to use</param>
    /// <param name="cancellationToken">
    /// A cancellation token to signal the cancellation of the operation. Specifying this parameter will override any default cancellations such as due to timeouts
    /// (<see cref="P:Microsoft.JSInterop.JSRuntime.DefaultAsyncTimeout"/>) from being applied.
    /// </param>
    /// <returns>A <see cref="ValueTask"/> representing the completion of the operation.</returns>
    ValueTask<bool> ContainKeyAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously gets the number of items stored in local storage.
    /// </summary>
    /// <returns>A <see cref="ValueTask"/> representing the completion of the operation.</returns>
    ValueTask<int> LengthAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously remove the data with the specified <paramref name="key"/>.
    /// </summary>
    /// <param name="key">A <see cref="string"/> value specifying the name of the storage slot to use</param>
    /// <param name="cancellationToken">
    /// A cancellation token to signal the cancellation of the operation. Specifying this parameter will override any default cancellations such as due to timeouts
    /// (<see cref="P:Microsoft.JSInterop.JSRuntime.DefaultAsyncTimeout"/>) from being applied.
    /// </param>
    /// <returns>A <see cref="ValueTask"/> representing the completion of the operation.</returns>
    ValueTask RemoveItemAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously removes a collection of <paramref name="keys"/>.
    /// </summary>
    /// <param name="keys">A IEnumerable collection of strings specifying the name of the storage slot to remove</param>
    /// <param name="cancellationToken">
    /// A cancellation token to signal the cancellation of the operation. Specifying this parameter will override any default cancellations such as due to timeouts
    /// (<see cref="P:Microsoft.JSInterop.JSRuntime.DefaultAsyncTimeout"/>) from being applied.
    /// </param>
    ValueTask RemoveItemsAsync(IEnumerable<string> keys, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously sets or updates the <paramref name="data"/> in local storage with the specified <paramref name="key"/>.
    /// </summary>
    /// <param name="key">A <see cref="string"/> value specifying the name of the storage slot to use</param>
    /// <param name="data">The data to be saved</param>
    /// <param name="cancellationToken">
    /// A cancellation token to signal the cancellation of the operation. Specifying this parameter will override any default cancellations such as due to timeouts
    /// (<see cref="P:Microsoft.JSInterop.JSRuntime.DefaultAsyncTimeout"/>) from being applied.
    /// </param>
    /// <returns>A <see cref="ValueTask"/> representing the completion of the operation.</returns>
    ValueTask SetItemAsync<T>(string key, T data, CancellationToken cancellationToken = default);

    event EventHandler<ChangingEventArgs> Changing;

    event EventHandler<ChangedEventArgs> Changed;
}