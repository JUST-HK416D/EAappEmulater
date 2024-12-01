using EAappEmulater.Enums;

namespace EAappEmulater.Models;

public class StarterData
{
    [JsonPropertyName("clientNo")]
    public string ClientNo { get; set; }
    
    [JsonPropertyName("type")]
    public int? Type { get; set; }
    
    [JsonPropertyName("subType")]
    public int? SubType { get; set; }
    
    [JsonPropertyName("personaName")]
    public string PersonaName { get; set; }
    
    [JsonPropertyName("personaId")]
    public long? PersonaId { get; set; }
    
    [JsonPropertyName("userId")]
    public long? UserId { get; set; }

    [JsonPropertyName("gameState")]
    public GameState GameState { get; set; }
    
    [JsonPropertyName("gameId")]
    public long? GameId { get; set; }

    [JsonPropertyName("timestamp")]
    public long Timestamp { get; set; }
    
}