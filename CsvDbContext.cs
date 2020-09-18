using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Arex388.Extensions.CsvHelper {
    public abstract class CsvDbContext {
        private const string SaveMethod = "Save";

        private readonly CsvDbContextOptions _options;

        private Type _csvDbSetType;
        private Type CsvDbSetType => _csvDbSetType ??= typeof(ICsvDbSet);
        private Type _classMapType;
        private Type ClassMapType => _classMapType ??= typeof(ClassMap);

        private IList<PropertyInfo> _csvDbSets;
        private IList<PropertyInfo> CsvDbSets => _csvDbSets ??= GetType().GetProperties().Where(
            p => CsvDbSetType.IsAssignableFrom(p.PropertyType)).ToList();
        private IList<Type> _classMaps;
        private IList<Type> ClassMaps => _classMaps ??= GetType().Assembly.GetTypes().Where(
            t => t.BaseType?.BaseType == ClassMapType).ToList();

        protected CsvDbContext(
            CsvDbContextOptions options) {
            _options = options;

            Init();
        }

        public abstract void Relate();

        public void Save() {
            foreach (var csvDbSet in CsvDbSets) {
                var instance = csvDbSet.GetValue(this, null);
                var saveMethod = csvDbSet.PropertyType.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly).Single(
                    m => m.Name == SaveMethod);

                saveMethod.Invoke(instance, null);
            }

            Relate();
        }

        //  ========================================================================
        //  Utilities
        //  ========================================================================

        private void Init() {
            foreach (var csvDbSet in CsvDbSets) {
                var instance = Activator.CreateInstance(csvDbSet.PropertyType, _options, ClassMaps);

                csvDbSet.SetValue(this, instance);
            }

            Relate();
        }
    }
}