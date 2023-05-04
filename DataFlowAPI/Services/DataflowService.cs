using DataFlowAPI.CSVParsers;
using DataFlowAPI.Utils;
using Microsoft.Data.Analysis;
using System.Text.RegularExpressions;

namespace DataFlowAPI.Services
{
    public class DataflowService : IDataflowService
    {
        private const string _idColumnPattern = "^ide*n*t*i*f*i*e*r*c*a*t*o*r*$";
        private readonly ICsvReader _reader;
        private readonly ICsvWriter _writer;
        private readonly ILogger<DataflowService> _logger;

        public DataflowService(ICsvReader reader, ICsvWriter writer, ILogger<DataflowService> logger)
        {
            _reader = reader;
            _writer = writer;
            _logger = logger;
        }

        public async Task<string?> ProcessDataAsync(string filepath, char delimeter)
        {
            Validator.ValidateParams(filepath, delimeter);

            var dfList = await _reader.ParseCsvFilesFromDirectory(filepath, delimeter);

            if (dfList.Count == 0)
            {
                _logger.LogWarning("CSV reader {reader} returned an empty list, no CSV files found", nameof(_reader));
                return null;
            }
            var dfMerged = MergeDataFrames(dfList);

            try
            {
                return await _writer.SaveDataFrameAsCsv(filepath, delimeter, dfMerged);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("CSV writer {wr} failed to save the output CSV file. \n Exception message: {msg}", nameof(_writer), ex.Message);
                return null;
            }
        }

        private static DataFrame MergeDataFrames(List<DataFrame> dfList)
        {
            var dfMerged = dfList[0];
            var key = dfMerged.Columns.FirstOrDefault(c => Regex.IsMatch(c.Name, _idColumnPattern)) ?? throw new ArgumentException("No appropriate ID column has been found");
            if (dfList.Count > 1)
            {
                for (int i = 1; i < dfList.Count; i++)
                {
                    var df = dfList[i];
                    if (df.Columns.Select(c => c.Name).Contains(key.Name))
                    {
                        dfMerged = dfMerged.Merge(df, new string[] { key.Name }, new string[] { key.Name });
                    }
                }
            }
            return dfMerged;
        }
    }
}