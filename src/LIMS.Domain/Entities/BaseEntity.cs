using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Domain.Entities
{
    public interface IBaseEntity<TKey>
    {
        TKey Id { get; }
    }
    public abstract class BaseEntity<TKey> : IBaseEntity<TKey>
    {
        public TKey Id { get; private set; }
    }
    public class BaseEntity : BaseEntity<long>
    {
        
    }
}
