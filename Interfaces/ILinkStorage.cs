using ShortLinkAppV2._0.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShortLinkAppV2._0.Interfaces
{
    public interface ILinkStorage
    {
        void Create(Entry entry);

        Task CreateAsync(Entry entry);

        /// <returns>all entries</returns>
        IList<Entry> Read();

        IList<Entry> Read(EntryField Field, object value);

        /// <returns>all entries</returns>
        Task<IList<Entry>> ReadAsync();

        Task<IList<Entry>> ReadAsync(EntryField Field, object value);

        void Update(EntryField filter, object filterValue, EntryField updateFiled, object updateValue);

        Task UpdateAsync(EntryField filterField, object filterValue, EntryField updateField, object updateValue);

        bool Remove();

        bool Remove(EntryField Field, object value);
    }
}
