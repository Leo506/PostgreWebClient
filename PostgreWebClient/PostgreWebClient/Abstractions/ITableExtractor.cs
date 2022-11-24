using System.Data;
using PostgreWebClient.Models;

namespace PostgreWebClient.Abstractions;

public interface ITableExtractor
{
    Table ExtractTable(IDataReader reader);
}