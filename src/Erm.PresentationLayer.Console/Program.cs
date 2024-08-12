// using System.Data;
// using Erm.BusinessLayer;
// using Erm.DataAccess;


// internal class Program
// {
//     internal static async Task Main()
//     {
//         IRiskProfileService riskProfileService = new RiskProfileService();
//         IRiskService riskService = new RiskService();
//         IAnalysis analysis = new Analysis();

//         string cmd = string.Empty;

//         while (!cmd.Equals(CommandHelper.ExitCommand))
//         {
//             try
//             {
//                 Console.ForegroundColor = ConsoleColor.DarkGray; // Reset console foreground color to default.
//                 Console.Write(CommandHelper.InputSymbol);
//                 cmd = Console.ReadLine();

//                 switch (cmd)
//                 {
//                     case CommandHelper.CreateRiskProfileCommand:
//                         Console.WriteLine("Введите имя");
//                         string rpName = Console.ReadLine(); // required
//                         Console.WriteLine("Введите описание");
//                         string rpDescription = Console.ReadLine(); // required
//                         Console.WriteLine("Введите бизнес процесс");
//                         string rpBusinessProcess = Console.ReadLine(); // required
//                         if(string.IsNullOrEmpty(rpName) ||  string.IsNullOrEmpty(rpBusinessProcess) || string.IsNullOrEmpty(rpBusinessProcess))
//                         {
//                             throw new ArgumentNullException();
//                         }

//                         int rpOccurrenceProbability;
//                         int rpPotentialBusinessImpact;
//                         bool rpOccurrenceProbabilityParse = int.TryParse(Console.ReadLine(), out rpOccurrenceProbability); // (1-10)
//                         Console.WriteLine("Введите вероятность возникновения (1-10)");
//                         bool rpPotentialBusinessImpactParse = int.TryParse(Console.ReadLine(), out rpPotentialBusinessImpact); // (1-10)
//                         Console.WriteLine("Введите потенцианльное влияние на бизнес (1-10)");
//                         if(rpOccurrenceProbability > 10 || rpOccurrenceProbability < 1 ||
//                             rpPotentialBusinessImpact > 10 || rpPotentialBusinessImpact < 1) 
//                         {
//                             throw new ArgumentOutOfRangeException();
//                         }
//                         RiskProfileInfo riskProfileInfo = new(
//                             rpName, rpDescription, rpBusinessProcess,
//                             rpOccurrenceProbability, rpPotentialBusinessImpact
//                         );
//                         await riskProfileService.CreateAsync(riskProfileInfo);
                        
//                         // TODO: Use int.TryParse() instead of int.Parse().
//                         // TODO: Implement validation.
//                         break;
//                     case CommandHelper.DeleteRiskProfileCommand:
//                         Console.WriteLine("Введите имя");
//                         int rpDeleteName = int.Parse(Console.ReadLine());
//                         await riskProfileService.DeleteAsync(rpDeleteName);
//                         break;
//                     case CommandHelper.CreateRiskCommand:
//                         Console.WriteLine("Введите тип");
//                         string rType = Console.ReadLine();
//                         Console.WriteLine("Введите описание");
//                         string rDescription = Console.ReadLine();
//                         if(string.IsNullOrEmpty(rType) || string.IsNullOrEmpty(rDescription))
//                         {
//                             throw new ArgumentNullException();
//                         }
//                         int rProbability;
//                         int rBusinessImpact;
//                         bool rProbabilityParse = int.TryParse(Console.ReadLine(), out rProbability);
//                         Console.WriteLine("Введите вероятность возникновения (1-10)");
//                         bool rBusinessImpactParse = int.TryParse(Console.ReadLine(), out rBusinessImpact);
//                         Console.WriteLine("Введите потенцианльное влияние на бизнес (1-10)");
//                         if(rProbability > 10 || rProbability < 1 ||
//                            rBusinessImpact > 10 || rBusinessImpact < 1)
//                            throw new ArgumentOutOfRangeException();
//                         Console.WriteLine("Введите дату в таком виде 'dd.MM.yyyy'");
//                         string rOccurenceData = Console.ReadLine();
//                         if(string.IsNullOrEmpty(rOccurenceData))
//                             throw new ArgumentNullException();
                        
//                         DateTime? rOccurenceDataTime = null;
//                         if (DateTime.TryParseExact(rOccurenceData, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime result))
//                         {
//                             rOccurenceDataTime = result;
//                             if (rOccurenceDataTime >= DateTime.Now)
//                             {
//                                 throw new DataException("Дата должна быть меньше текущей даты.");
//                             }
//                         }

                        
//                         RiskDTO riskDTO = new(
//                             rType, rDescription, rProbability, rBusinessImpact, rOccurenceDataTime
//                         );
                        
//                         await riskService.CreateRiskAsync(riskDTO);
//                         break;
//                     case CommandHelper.TimeSeriesAnalysisCommand: // анализ временных рядов
//                         double[] AnalysisValueArray = [];

//                         Console.WriteLine("Введите сколько значений вы хотите задать анализу:");
//                         int AnanlysisValue;
//                         bool rpAnanlysisValueParse = int.TryParse(Console.ReadLine(), out AnanlysisValue);

//                         if (!rpAnanlysisValueParse || AnanlysisValue < 1 || AnanlysisValue > 10)
//                         {
//                             throw new ArgumentOutOfRangeException("Недопустимое количество значений для анализа.");
//                         }

//                         for (int i = 0; i < AnanlysisValue; i++)
//                         {
//                             bool isValidInput = false;
//                             do
//                             {
//                                 Console.WriteLine($"Введите значение {i + 1}:");
//                                 string input = Console.ReadLine();
//                                 double number;

//                                 if (double.TryParse(input, out number))
//                                 {
//                                     if (!(number > 10 || number < 0))
//                                     {
//                                         Array.Resize(ref AnalysisValueArray, AnalysisValueArray.Length + 1);
//                                         AnalysisValueArray[AnalysisValueArray.Length - 1] = number;
//                                         isValidInput = true; // Устанавливаем флаг в true, если ввод правильный
//                                     }
//                                     else
//                                     {
//                                         Console.WriteLine("Число должно быть в диапазоне от 0 до 10.");
//                                     }
//                                 }
//                                 else
//                                 {
//                                     Console.WriteLine("Неверный формат числа. Пожалуйста, введите целое число.");
//                                 } 
//                             } while (!isValidInput); // Повторяем ввод, пока ввод неверный
//                         }


//                         analysis.TimeSeriesAnalys(AnalysisValueArray);
//                         break;
//                     case CommandHelper.ClusterAnalysisCommand:
//                     //cluster analysis
//                     // TODO: cluster analysis
//                     break;
//                     case CommandHelper.CorrelationAnalyzerCommand:
//                     double[] CorrelationAnalyzerXArr = [];
//                     double[] CorrelationAnalyzerYArr = [];
//                     double CorrelationAnalyzerX;
//                     double CorrelationAnalyzerY;
//                     Console.WriteLine("Размер длины X и Y должны быть равны");
//                     Console.WriteLine("Введите сколько значений вы хотите задать анализу X:");
//                     bool CorrelationAnalyzerXValue = double.TryParse(Console.ReadLine(), out CorrelationAnalyzerX);
//                     Console.WriteLine("Введите сколько значений вы хотите задать анализу Y:");
//                     bool CorrelationAnalyzerYValue = double.TryParse(Console.ReadLine(), out CorrelationAnalyzerY);

//                         if (!(CorrelationAnalyzerXValue || CorrelationAnalyzerYValue) || 
//                             CorrelationAnalyzerX < 1 || CorrelationAnalyzerX > 10 ||
//                             CorrelationAnalyzerY < 1 || CorrelationAnalyzerY > 10
//                             || CorrelationAnalyzerXArr.Length != CorrelationAnalyzerYArr.Length)
//                         {
//                             throw new ArgumentOutOfRangeException("Недопустимое количество значений для анализа.");
//                         }

//                         for (int i = 0; i < CorrelationAnalyzerX; i++)
//                         {
//                             bool isValidInput = false;
//                             do
//                             {
//                                 Console.WriteLine($"Введите значение X : {i + 1}:");
//                                 string input = Console.ReadLine();
//                                 double number;

//                                 if (double.TryParse(input, out number))
//                                 {
//                                     if (!(number > 10 || number < 0))
//                                     {
//                                         Array.Resize(ref CorrelationAnalyzerXArr, CorrelationAnalyzerXArr.Length + 1);
//                                         CorrelationAnalyzerXArr[CorrelationAnalyzerXArr.Length - 1] = number;
//                                         isValidInput = true; // Устанавливаем флаг в true, если ввод правильный
//                                     }
//                                     else
//                                     {
//                                         Console.WriteLine("Число должно быть в диапазоне от 0 до 10.");
//                                     }
//                                 }
//                                 else
//                                 {
//                                     Console.WriteLine("Неверный формат числа. Пожалуйста, введите целое число.");
//                                 } 
//                             } while (!isValidInput); // Повторяем ввод, пока ввод неверный
//                         }

//                         for (int i = 0; i < CorrelationAnalyzerY; i++)
//                         {
//                             bool isValidInput = false;
//                             do
//                             {
//                                 Console.WriteLine($"Введите значение Y : {i + 1}:");
//                                 string input = Console.ReadLine();
//                                 double number;

//                                 if (double.TryParse(input, out number))
//                                 {
//                                     if (!(number > 10 || number < 0))
//                                     {
//                                         Array.Resize(ref CorrelationAnalyzerYArr, CorrelationAnalyzerYArr.Length + 1);
//                                         CorrelationAnalyzerYArr[CorrelationAnalyzerYArr.Length - 1] = number;
//                                         isValidInput = true; // Устанавливаем флаг в true, если ввод правильный
//                                     }
//                                     else
//                                     {
//                                         Console.WriteLine("Число должно быть в диапазоне от 0 до 10.");
//                                     }
//                                 }
//                                 else
//                                 {
//                                     Console.WriteLine("Неверный формат числа. Пожалуйста, введите целое число.");
//                                 } 
//                             } while (!isValidInput); // Повторяем ввод, пока ввод неверный
//                         }

//                     Console.WriteLine("Результат корреляционного анализа равен: " +
//                         await analysis.CorrelationAnalyzerAsync(CorrelationAnalyzerXArr, CorrelationAnalyzerYArr));
//                     break;
//                     case CommandHelper.QueryRiskProfileCommand:
//                         Console.WriteLine("Введите запрос");
//                         string query = Console.ReadLine();

//                         IEnumerable<RiskProfileInfo> riskProfileInfos = await riskProfileService.QueryAsync(query);
//                         foreach (var item in riskProfileInfos)
//                         {
//                             Console.WriteLine(item.ToString());
//                         }
//                         break;

//                     case CommandHelper.GetRiskProfileCommand:
//                         Console.WriteLine("Введите имя");
//                         string nameOfGetCommand = Console.ReadLine();
//                         Console.WriteLine(await riskProfileService.GetAsync(nameOfGetCommand));
//                         break;
//                     case CommandHelper.GetRiskCommand:
//                         Console.WriteLine("Введите тип риска");
//                         string getTypeRisk = Console.ReadLine();
//                         Console.WriteLine(await riskService.GetAsync(getTypeRisk));
//                     break;
//                     case CommandHelper.UpdateRisk:
//                     Console.WriteLine("Введите id риска");
//                     int UpdateIdRisk = int.Parse(Console.ReadLine());
//                     Console.WriteLine();
//                     string rTypeUpdate = Console.ReadLine();
//                     string rDescriptionUpdate = Console.ReadLine();
//                     if(string.IsNullOrEmpty(rTypeUpdate) || string.IsNullOrEmpty(rDescriptionUpdate))
//                     {
//                         throw new ArgumentNullException();
//                     }
//                     int rProbabilityUpdate;
//                     int rBusinessImpactUpdate;
//                     bool rProbabilityParseUpdate = int.TryParse(Console.ReadLine(), out rProbabilityUpdate);
//                     bool rBusinessImpactParseUpdate = int.TryParse(Console.ReadLine(), out rBusinessImpactUpdate);
//                     if(rProbabilityUpdate > 10 || rProbabilityUpdate < 1 ||
//                         rBusinessImpactUpdate > 10 || rBusinessImpactUpdate < 1)
//                         throw new ArgumentOutOfRangeException();
//                     string rOccurenceDataUpdate = Console.ReadLine();
//                     if(string.IsNullOrEmpty(rOccurenceDataUpdate))
//                         throw new ArgumentNullException();
                    
//                     DateTime? rOccurenceDataTimeUpdate = null;
//                     if (DateTime.TryParseExact(rOccurenceDataUpdate, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime resultUpdate))
//                     {
//                         rOccurenceDataTime = resultUpdate;
//                         if (rOccurenceDataTime >= DateTime.Now)
//                         {
//                             throw new DataException("Дата должна быть меньше текущей даты.");
//                         }
//                     }

                        
//                     RiskDTO riskDTOUpdate = new(
//                         rTypeUpdate, rDescriptionUpdate, rProbabilityUpdate, rBusinessImpactUpdate, rOccurenceDataTimeUpdate
//                     ); 

//                     Risk riskToUpdate = new()
//                     {
//                         Type = riskDTOUpdate.Type,
//                         Description = riskDTOUpdate.Description,
//                         Probability = riskDTOUpdate.Probability,
//                         BusinessImpact = riskDTOUpdate.BusinessImpact,
//                         OccurenceData = riskDTOUpdate.OccurenceData
//                     };
//                     await riskService.UpdateAsync(UpdateIdRisk,riskToUpdate);

//                     break;
//                     case CommandHelper.HelpCommand:
//                         Console.WriteLine(CommandHelper.InputSymbol + CommandHelper.CreateRiskProfileCommand + " -> " + CommandHelper.CreateRiskProfileDescription);
//                         Console.WriteLine(CommandHelper.InputSymbol + CommandHelper.CreateRiskCommand + " -> " + CommandHelper.CreateRiskDescription);
//                         Console.WriteLine(CommandHelper.InputSymbol + CommandHelper.DeleteRiskProfileCommand + " -> " + CommandHelper.DeleteRiskProfileDescription);
//                         Console.WriteLine(CommandHelper.InputSymbol + CommandHelper.QueryRiskProfileCommand + " -> " + CommandHelper.QueryRiskProfileDescription);
//                         Console.WriteLine(CommandHelper.InputSymbol + CommandHelper.TimeSeriesAnalysisCommand + " -> " + CommandHelper.TimeSeriesAnalysisDescription);
//                         Console.WriteLine(CommandHelper.InputSymbol + CommandHelper.CorrelationAnalyzerCommand + " -> " + CommandHelper.CorrelationAnalyzerDescription);
//                         Console.WriteLine(CommandHelper.InputSymbol + CommandHelper.GetRiskProfileCommand + " -> " + CommandHelper.GetRiskProfileDescription); 
//                         Console.WriteLine(CommandHelper.InputSymbol + CommandHelper.GetRiskCommand + " -> " + CommandHelper.GetRiskDescription); 
                        
//                         // TODO: Add help and exit cmds...
//                         break;
//                     case CommandHelper.ExitCommand:
//                         break;
//                     default:
//                         Console.ForegroundColor = ConsoleColor.Red;
//                         Console.WriteLine(CommandHelper.UknownCommandMessage);
//                         break;
//                 }

//             }
//             catch (Exception ex)
//             {
//                 Console.ForegroundColor = ConsoleColor.Red;
//                 Console.WriteLine(CommandHelper.InputSymbol + ex.Message);
//             }
//         }
//     }
// }

// file static class CommandHelper
// {
//     public const string InputSymbol = "> ";
//     public const string ExitCommand = "exit";
//     public const string HelpCommand = "help";
//     public const string CreateRiskCommand = "create_risk";
//     public const string CreateRiskDescription = "Create Risk.";
//     public const string CreateRiskProfileCommand = "create_profile";
//     public const string DeleteRiskProfileCommand = "delete_profile";
//     public const string DeleteRiskProfileDescription = "Delete Risk Profile";
//     public const string TimeSeriesAnalysisCommand = "time_series_analysis"; // анализ временных рядов
//     public const string TimeSeriesAnalysisDescription = "Perform a time series analysis";
//     public const string ClusterAnalysisCommand = "cluster_analysis"; //кластерный анализ !!!!
//     public const string CorrelationAnalyzerCommand = "correlation_analysis"; //корреляционный анализ
//     public const string CorrelationAnalyzerDescription = "Perform a correlation analysis";
//     public const string QueryRiskProfileCommand = "search_profile";
//     public const string GetRiskProfileCommand = "get_profile";
//     public const string GetRiskProfileDescription = "Get a risk profile";
//     public const string GetRiskCommand = "get_risk";
//     public const string GetRiskDescription = "Get a risk";
//     public const string QueryRiskProfileDescription = "Search a Risk Profile";
//     public const string CreateRiskProfileDescription = "Creates Risk Profile.";
//     public const string UpdateRisk = "update_risk";
//     public const string UpdateRiskDescription = "Update risk";
//     public const string UknownCommandMessage = "Unknown command, use help to see list of available commands.";
// }