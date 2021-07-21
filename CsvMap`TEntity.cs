using CsvHelper.Configuration;

namespace Arex388.Extensions.CsvHelper {
    public abstract class CsvMap<TEntity> :
        ClassMap<TEntity>,
        ICsvMap {
        public virtual string FileName => $"{typeof(TEntity).Name}s";
    }
}