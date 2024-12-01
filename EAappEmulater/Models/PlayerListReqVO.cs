namespace EAappEmulater.Models;

public class PlayerListReqVo
{
    [JsonPropertyName("gameIds")]
    public List<long> GameIds { get; set; }
}