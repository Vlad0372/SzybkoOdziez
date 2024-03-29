﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SzybkoOdziez.Services
{
    public interface IDataStore<T, TKey>
    {
        Task<bool> AddItemAsync(T item);
        Task<bool> UpdateItemAsync(T item);
        Task<bool> DeleteItemAsync(T item);
        Task<bool> DeleteItemAsync(TKey id);
        Task<T> GetItemAsync(TKey id);
        Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);
    }

}
