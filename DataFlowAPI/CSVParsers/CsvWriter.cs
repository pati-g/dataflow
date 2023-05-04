using Microsoft.Data.Analysis;
using System.Globalization;

namespace DataFlowAPI.CSVParsers
{
    public class CsvWriter : ICsvWriter
    {
        // Save the merged dataframe as csv file and return the filepath
        public async Task<string> SaveDataFrameAsCsv(string filepath, char delimeter, DataFrame dfMerged)
        {
            var fileName = $"{Path.GetFileNameWithoutExtension(filepath)}-combined.csv";
            var outputPath = Path.Combine(filepath, fileName);
            await Task.Run(() => DataFrame.SaveCsv(dfMerged, outputPath, delimeter, true, null, CultureInfo.InvariantCulture));
            return outputPath;
        }
    }
}
