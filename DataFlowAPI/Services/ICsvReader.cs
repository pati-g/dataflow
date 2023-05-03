using Microsoft.Data.Analysis;

namespace DataFlowAPI.Services
{
    public interface ICsvReader
    {
        public Task<List<DataFrame>> ParseCsvFilesFromDirectory(string filepath, char delimeter);
    }
}