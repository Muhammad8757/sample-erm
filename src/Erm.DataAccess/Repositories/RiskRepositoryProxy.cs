using System.Text.Json;
using System.Text.Json.Serialization;

using StackExchange.Redis;

namespace Erm.DataAccess;

public sealed class RiskRepositoryProxy : IRiskRepository
{
    private const string RedisHost = "127.0.0.1:6379";

    private readonly IDatabase _redisDb = null!;

    private readonly RiskRepository _originalRepository = null!;
    
    public RiskRepositoryProxy(RiskRepository originalRepository)
    {
        _originalRepository = originalRepository;

        ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(RedisHost);
        _redisDb = connectionMultiplexer.GetDatabase();
    }

    

    public async Task CreateAsync (Risk entity, CancellationToken token = default)
    {
        await _originalRepository.CreateAsync(entity, token);
    }

    public async Task DeleteAsync(string type, CancellationToken token = default)
    {
        // Удаляем объект из Redis
        await _redisDb.KeyDeleteAsync(type);

        // Удаляем объект из оригинального репозитория
        await _originalRepository.DeleteAsync(type, token);
    }

    public async Task<Risk> GetAsync(string type, CancellationToken token = default)
    {
        try
        {
            RedisValue redisValue = await _redisDb.StringGetAsync(type);
            if (redisValue.IsNullOrEmpty)
            {
                // Объект не найден в кэше, запрашиваем его из оригинального репозитория
                Risk profileFromDb = await _originalRepository.GetAsync(type, token);

                // Сохраняем полученный объект в кэше Redis, только если он не null
                if (profileFromDb != null)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve
                    };
                    string redisProfileJson = JsonSerializer.Serialize(profileFromDb, options);
                    await _redisDb.StringSetAsync(type, redisProfileJson);
                }

                return profileFromDb ?? throw new InvalidOperationException();
            }

            string redisProfileJsonStr = redisValue.ToString();

            // Объект найден в кэше, десериализуем его и возвращаем
            Risk profile = JsonSerializer.Deserialize<Risk>(redisProfileJsonStr)
                ?? throw new InvalidOperationException();
            return profile;
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Ошибка десериализации JSON: {ex.Message}");
            return await _originalRepository.GetAsync(type, token);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
            throw;
        }
    }



    public int[] ROccurrenceProbability()
        => _originalRepository.RPotentialBusinessImpact();

   

    public int[] RPotentialBusinessImpact()
        => _originalRepository.RPotentialBusinessImpact();

    public async Task UpdateAsync(string name, Risk risk, CancellationToken token = default)
    {
        // Обновляем объект в оригинальном репозитории
        await _originalRepository.UpdateAsync(name, risk, token);

        // Обновляем данные объекта в кэше Redis
        JsonSerializerOptions options = new()
        {
            ReferenceHandler = ReferenceHandler.Preserve
        };
        string redisProfileJson = JsonSerializer.Serialize(risk, options);
        await _redisDb.StringSetAsync(name, redisProfileJson);
    }
}
