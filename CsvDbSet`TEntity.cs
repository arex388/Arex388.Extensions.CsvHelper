using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Arex388.Extensions.CsvHelper {
    public class CsvDbSet<TEntity> :
        List<TEntity>,
        ICsvDbSet {
        private readonly CsvMap<TEntity> _csvMap;
        private readonly string _path;

        public CsvDbSet(
            CsvDbContextOptions options,
            IEnumerable<Type> csvMaps) {
            _csvMap = GetCsvMap(csvMaps);
            _path = $"{options.Path}\\{_csvMap.FileName}.csv";

            using var reader = new StreamReader(_path);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            csv.Context.RegisterClassMap(_csvMap);

            var records = csv.GetRecords<TEntity>();

            AddRange(records);
        }

        public async Task SaveAsync(
            CancellationToken cancellationToken) {
            using var writer = new StreamWriter(_path);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            csv.Context.RegisterClassMap(_csvMap);

            await csv.WriteRecordsAsync(this, cancellationToken).ConfigureAwait(false);
        }

        //  ========================================================================
        //  Utilities
        //  ========================================================================

        private static CsvMap<TEntity> GetCsvMap(
            IEnumerable<Type> csvMaps) {
            var csvMap = csvMaps.SingleOrDefault(
                _ => _.BaseType == typeof(CsvMap<TEntity>));

            if (csvMap is null) {
                throw new NullReferenceException($"No mapping class inheriting from CsvMap<{typeof(TEntity).Name}> was found for this entity.");
            }

            return (CsvMap<TEntity>)Activator.CreateInstance(csvMap);
        }
    }
}