namespace EAappEmulater.Models;

public class PlayerListRespVo
{
    [JsonPropertyName("code")]
    public int? Code { get; set; }

    [JsonPropertyName("msg")]
    public string Msg { get; set; }
    
    [JsonPropertyName("data")]
    public Dictionary<string, List<long>> Data { get; set; }
}