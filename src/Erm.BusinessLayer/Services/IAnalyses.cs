namespace Erm.BusinessLayer;

public interface IAnalysis
{
    Task<double> TimeSeriesAnalysisAsync(int[] analyzeValueArray, CancellationToken token = default);
    Task<double> ClusterAnalysisAsync(int[][] data, int k, CancellationToken token = default);
    Task<double> CorrelationAnalyzerAsync(int[] x, int[] y, CancellationToken token = default);
}