using Microsoft.Data.Analysis;

namespace DataFlowAPI.CSVParsers
{
    public interface ICsvReader
    {
        public Task<List<DataFrame>> ParseCsvFilesFromDirectory(string filepath, char delimeter);
    }
}