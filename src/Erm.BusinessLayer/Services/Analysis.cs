namespace Erm.BusinessLayer;

public class Analysis : IAnalysis
{
    public async Task<double> ClusterAnalysisAsync(int[][] data, int k, CancellationToken token = default)
    {
        Random random = new Random();
        int[][] centroids = new int[k][];
        for (int i = 0; i < k; i++)
        {
            centroids[i] = data[random.Next(data.Length)];
        }

        int[] clusters = new int[data.Length];
        bool centroidsUpdated = true;
        int iterations = 0;
        while (centroidsUpdated)
        {
            iterations++;
            centroidsUpdated = false;

            for (int i = 0; i < data.Length; i++)
            {
                int[] point = data[i];
                int minDistance = int.MaxValue;
                int closestCluster = -1;

                List<Task<(int, int)>> distanceTasks = new List<Task<(int, int)>>();
                for (int j = 0; j < centroids.Length; j++)
                {
                    distanceTasks.Add(CalculateDistanceAsync(point, centroids[j], j));
                }

                var results = await Task.WhenAll(distanceTasks);
                foreach (var result in results)
                {
                    if (result.Item2 < minDistance)
                    {
                        minDistance = result.Item2;
                        closestCluster = result.Item1;
                    }
                }

                if (clusters[i] != closestCluster)
                {
                    clusters[i] = closestCluster;
                    centroidsUpdated = true;
                }
            }

            for (int i = 0; i < k; i++)
            {
                List<int[]> clusterPoints = new List<int[]>();
                for (int j = 0; j < clusters.Length; j++)
                {
                    if (clusters[j] == i)
                        clusterPoints.Add(data[j]);
                }
                if (clusterPoints.Count > 0)
                {
                    centroids[i] = await ComputeMeanAsync(clusterPoints.ToArray());
                }
            }
        }

        for (int i = 0; i < data.Length; i++)
        {
            Console.WriteLine($"Data {data[i][0]}, {data[i][1]} is in cluster {clusters[i]}");
        }

        return iterations;
    }

    static async Task<(int, int)> CalculateDistanceAsync(int[] point1, int[] point2, int index)
    {
        int distance = await Task.Run(() => Distance(point1, point2));
        return (index, distance);
    }

    static async Task<int[]> ComputeMeanAsync(int[][] points)
    {
        return await Task.Run(() => ComputeMean(points));
    }

    static int Distance(int[] point1, int[] point2)
    {
        double sum = 0;
        for (int i = 0; i < point1.Length; i++)
        {
            sum += Math.Pow(point1[i] - point2[i], 2);
        }
        return (int)Math.Sqrt(sum);
    }

    static int[] ComputeMean(int[][] points)
    {
        int[] mean = new int[points[0].Length];
        for (int i = 0; i < points[0].Length; i++)
        {
            int sum = 0;
            for (int j = 0; j < points.Length; j++)
            {
                sum += points[j][i];
            }
            mean[i] = sum / points.Length;
        }
        return mean;
    }


// анализ временных рядов
    public async Task<double> TimeSeriesAnalysisAsync(int[] analyzeValueArray, CancellationToken token = default)
    {
        return await Task.Run(() =>
        {
            int result = 0;

            // Анализ тренда
            int sum = 0;
            foreach (var value in analyzeValueArray)
            {
                // Проверка на допустимое значение
                if (value < 0 || value > 10)
                {
                    throw new ArgumentException("Значение временного ряда должно быть в диапазоне от 0 до 10.");
                }
                sum += value;
            }
            foreach (var value in analyzeValueArray)
            {
                sum += value;
            }
            int average = sum / analyzeValueArray.Length;
            result += average; // Среднее значение

            // Анализ сезонности
            for (int i = 1; i < analyzeValueArray.Length; i++)
            {
                int diff = analyzeValueArray[i] - analyzeValueArray[i - 1];
                result += diff; // Сезонное изменение
            }

            // Обнаружение аномалий
            double threshold = 1.5; // Пороговое значение
            for (int i = 0; i < analyzeValueArray.Length; i++)
            {
                if (i > 0 && Math.Abs(analyzeValueArray[i] - analyzeValueArray[i - 1]) > threshold)
                {
                    result += analyzeValueArray[i]; // Обнаруженная аномалия
                }
                else
                {
                    result += 0; // Если нет аномалии, добавляем 0
                }
            }

            return result;
        }, token);
    }

    public async Task<double> CorrelationAnalyzerAsync(int[] x, int[] y, CancellationToken token = default)
    {
        if (x.Length != y.Length || x.Length == 0)
        {
            throw new ArgumentException("Некорректные данные для расчета корреляции");
        }

        double meanX = await Task.Run(() => CalculateMean(x));
        double meanY = await Task.Run(() => CalculateMean(y));

        double numerator = 0;
        double denominatorX = 0;
        double denominatorY = 0;

        for (int i = 0; i < x.Length; i++)
        {
            numerator += (x[i] - meanX) * (y[i] - meanY);
            denominatorX += Math.Pow(x[i] - meanX, 2);
            denominatorY += Math.Pow(y[i] - meanY, 2);
        }

        double correlationCoefficient = numerator / Math.Sqrt(denominatorX * denominatorY);

        return (int)Math.Round(correlationCoefficient * 100); // Приводим коэффициент к целому числу
    }

    private static double CalculateMean(int[] data)
    {
        double sum = 0;

        foreach (var value in data)
        {
            sum += value;
        }

        return sum / data.Length;
    }
}