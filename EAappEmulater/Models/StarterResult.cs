namespace EAappEmulater.Models;

public class StarterResult
{

    [JsonPropertyName("code")]
    public int? Code { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }
    
    [JsonPropertyName("data")]
    public StarterCommand Data { get; set; }

}