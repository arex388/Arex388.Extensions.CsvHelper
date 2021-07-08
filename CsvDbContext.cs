using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Arex388.Extensions.CsvHelper {
    public abstract class CsvDbContext {
        private readonly IList<PropertyInfo> _csvDbSets;

        protected CsvDbContext(
            string path) :
            this(new CsvDbContextOptions(path)) {
        }

        protected CsvDbContext(
            CsvDbContextOptions options) {
            _csvDbSets = GetType().GetProperties().Where(
                p => typeof(ICsvDbSet).IsAssignableFrom(p.PropertyType)).ToList();

            var classMaps = GetType().Assembly.GetTypes().Where(
                t => t.BaseType?.BaseType == typeof(ClassMap)).ToList();

            foreach (var csvDbSet in _csvDbSets) {
                var instance = Activator.CreateInstance(csvDbSet.PropertyType, options, classMaps);

                csvDbSet.SetValue(this, instance);
            }
        }

        protected abstract void Relate();

        public async Task SaveAsync(
            CancellationToken cancellationToken = default) {
            foreach (var csvDbSet in _csvDbSets) {
                var instance = (ICsvDbSet)csvDbSet.GetValue(this, null);

                await instance.SaveAsync(cancellationToken).ConfigureAwait(false);
            }
        }
    }
}