using Microsoft.Data.Analysis;

namespace DataFlowAPI.CSVParsers
{
    public interface ICsvWriter
    {
        public Task<string> SaveDataFrameAsCsv(string outputPath, char delimeter, DataFrame dfMerged);
    }
}