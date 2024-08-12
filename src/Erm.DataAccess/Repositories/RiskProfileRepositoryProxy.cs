using System.Text.Json;
using System.Text.Json.Serialization;


using StackExchange.Redis;

namespace Erm.DataAccess;

public sealed class RiskProfileRepositoryProxy : IRiskProfileRepository
{
    private const string RedisHost = "127.0.0.1:6379";

    private readonly IDatabase _redisDb = null!;

    private readonly RiskProfileRepository _originalRepository = null!;

    public RiskProfileRepositoryProxy(RiskProfileRepository originalRepository)
    {
        _originalRepository = originalRepository;

        ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(RedisHost);
        _redisDb = connectionMultiplexer.GetDatabase();
    }

    public async Task CreateAsync(RiskProfile entity, CancellationToken token = default) 
    {
        await _originalRepository.CreateAsync(entity, token);
    }
    
    public async Task DeleteAsync(string name, CancellationToken token = default)
    {
        // Удаляем объект из Redis
        await _redisDb.KeyDeleteAsync(name);

        // Удаляем объект из оригинального репозитория
        await _originalRepository.DeleteAsync(name, token);
    }

    public async Task<RiskProfile> GetAsync(string name, CancellationToken token = default)
    {
        try
        {
            RedisValue redisValue = await _redisDb.StringGetAsync(name);
            if (redisValue.IsNullOrEmpty)
            {
                // Объект не найден в кэше, запрашиваем его из оригинального репозитория
                RiskProfile profileFromDb = await _originalRepository.GetAsync(name, token);

                // Сохраняем полученный объект в кэше Redis, только если он не null
                if (profileFromDb != null)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve
                    };
                    string redisProfileJson = JsonSerializer.Serialize(profileFromDb, options);
                    await _redisDb.StringSetAsync(name, redisProfileJson);
                }

                return profileFromDb ?? throw new InvalidOperationException();
            }

            string redisProfileJsonStr = redisValue.ToString();

            // Объект найден в кэше, десериализуем его и возвращаем
            RiskProfile profile = JsonSerializer.Deserialize<RiskProfile>(redisProfileJsonStr)
                ?? throw new InvalidOperationException();
            return profile;
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Ошибка десериализации JSON: {ex.Message}");
            return await _originalRepository.GetAsync(name, token);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
            throw;
        }
    }



    public Task<IEnumerable<RiskProfile>> QueryAsync(string query, CancellationToken token = default) 
        => _originalRepository.QueryAsync(query, token);

    public int[] RpOccurrenceProbability() 
        => _originalRepository.RpOccurrenceProbability();

    public int[] RpPotentialBusinessImpact()
        => _originalRepository.RpPotentialBusinessImpact();

    public async Task UpdateAsync(string name, RiskProfile riskProfile, CancellationToken token = default)
    {
        // Обновляем объект в оригинальном репозитории
        await _originalRepository.UpdateAsync(name, riskProfile, token);

        // Обновляем данные объекта в кэше Redis
        JsonSerializerOptions options = new()
        {
            ReferenceHandler = ReferenceHandler.Preserve
        };
        string redisProfileJson = JsonSerializer.Serialize(riskProfile, options);
        await _redisDb.StringSetAsync(name, redisProfileJson);
    }

}



