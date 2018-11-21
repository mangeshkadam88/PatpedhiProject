using System;
using System.Collections.Generic;
using System.Text;

namespace PatPedhi.Core.Entities
{
    // This can easily be modified to be BaseEntity<T> and public T Id to support different key types.
    // Using non-generic integer types for simplicity and to ease caching logic
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime added_date { get; set; }
        public DateTime? modified_date { get; set; }
    }
}
