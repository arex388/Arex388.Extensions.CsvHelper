﻿using CsvHelper;
using CsvHelper.Configuration;
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
        private readonly Type _classMap;
        private readonly string _path;

        public CsvDbSet(
            CsvDbContextOptions options,
            IEnumerable<Type> classMaps) {
            _classMap = classMaps.SingleOrDefault(
                cm => cm.BaseType == typeof(ClassMap<TEntity>));
            _path = $"{options.Path}\\{typeof(TEntity).Name}s.csv";

            using var reader = new StreamReader(_path);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            csv.Context.RegisterClassMap(_classMap);

            var records = csv.GetRecords<TEntity>();

            AddRange(records);
        }

        public async Task SaveAsync(
            CancellationToken cancellationToken) {
            using var writer = new StreamWriter(_path);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            csv.Context.RegisterClassMap(_classMap);

            await csv.WriteRecordsAsync(this, cancellationToken).ConfigureAwait(false);
        }
    }
}