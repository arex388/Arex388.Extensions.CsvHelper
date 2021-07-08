using System.Threading;
using System.Threading.Tasks;

namespace Arex388.Extensions.CsvHelper {
    internal interface ICsvDbSet {
        Task SaveAsync(
            CancellationToken cancellationToken);
    }
}