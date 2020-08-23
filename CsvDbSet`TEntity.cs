﻿using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Arex388.Extensions.CsvHelper {
    public class CsvDbSet<TEntity> :
        List<TEntity>,
        ICsvDbSet {
        private readonly IList<Type> _classMaps;
        private readonly string _path;

        private Type _classMapType;
        private Type ClassMapType => _classMapType ??= typeof(ClassMap<TEntity>);

        private Type _classMap;
        private Type ClassMap => _classMap ??= _classMaps.SingleOrDefault(
            cm => cm.BaseType == ClassMapType);

        public CsvDbSet(
            CsvDbContextOptions options,
            IList<Type> classMaps) {
            _classMaps = classMaps;
            _path = $"{options.Path}\\{typeof(TEntity).Name}s.csv";

            Load();
        }

        //  ========================================================================
        //  Utilities
        //  ========================================================================

        private void Load() {
            using var reader = new StreamReader(_path);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            csv.Configuration.RegisterClassMap(ClassMap);

            var records = csv.GetRecords<TEntity>();

            AddRange(records);
        }

        private void Save() {
            using var writer = new StreamWriter(_path);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            csv.Configuration.RegisterClassMap(ClassMap);

            csv.WriteRecords(this);
        }
    }
}