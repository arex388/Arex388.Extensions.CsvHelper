namespace Arex388.Extensions.CsvHelper {
    public sealed class CsvDbContextOptions {
        public string Path { get; }

        public CsvDbContextOptions(
            string path) => Path = path;
    }
}