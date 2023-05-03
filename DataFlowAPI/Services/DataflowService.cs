using Microsoft.Data.Analysis;
using System.Globalization;
using System.Text.RegularExpressions;

namespace DataFlowAPI.Services
{
    public class DataflowService : IDataflowService
    {
        private const string IdColumnPattern = "^ide*n*t*i*f*i*e*r*c*a*t*o*r*$";

        public async Task<string?> ProcessDataAsync(string filepath, char delimeter)
        {
            ValidateParams(filepath, delimeter);

            var dfList = new List<DataFrame>();

            // Create a dataframe from the file
            foreach (var file in Directory.EnumerateFiles(filepath, "*.csv"))
            {
                LoadCsvFileToList(delimeter, dfList, file);
            }

            if (dfList.Count == 0)
            {
                return null;
            }
            var dfMerged = MergeDataFrames(dfList);

            // Save the merged dataframe as csv file and return the filepath
            var fileName = $"{filepath}-combined.csv";
            var savePath = Path.Combine(filepath, fileName);
            DataFrame.SaveCsv(dfMerged, savePath, delimeter, true, null, CultureInfo.InvariantCulture);
            return savePath;
        }

        private static void LoadCsvFileToList(char delimeter, List<DataFrame> dfList, string file)
        {
            var dataFrame = DataFrame.LoadCsv(file, delimeter);
            dfList.Add(dataFrame);
        }

        private static DataFrame MergeDataFrames(List<DataFrame> dfList)
        {
            var dfMerged = dfList[0];
            var key = dfMerged.Columns.FirstOrDefault(c => Regex.IsMatch(c.Name, IdColumnPattern)) ?? throw new ArgumentException("No appropriate ID column has been found");
            if (dfList.Count > 1)
            {
                for (int i = 1; i < dfList.Count; i++)
                {
                    var df = dfList[i];
                    dfMerged = dfMerged.Merge(df, new string[] { key.Name }, new string[] { key.Name });
                }
            }
            return dfMerged;
        }

        // Validate the filepath and the delimeter
        private static void ValidateParams(string filepath, char delimeter)
        {
            if (delimeter == default(char))
            {
                throw new ArgumentException($"Argument {nameof(delimeter)} has not been provided");
            }
            if (string.IsNullOrEmpty(filepath) || !Directory.Exists(filepath))
            {
                throw new ArgumentException($"No valid argument for {nameof(filepath)} has been provided");
            }
        }
    }
}