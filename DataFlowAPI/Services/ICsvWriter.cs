using Microsoft.Data.Analysis;

namespace DataFlowAPI.Services
{
    public interface ICsvWriter
    {
        public Task<string> SaveDataFrameAsCsv(string outputPath, char delimeter, DataFrame dfMerged);
    }
}