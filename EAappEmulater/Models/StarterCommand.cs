namespace EAappEmulater.Models;

public class StarterCommand
{
    
    [JsonPropertyName("command")]
    public int? Command { get; set; }
    
    [JsonPropertyName("argument1")]
    public string Argument1 { get; set; }
    
    [JsonPropertyName("argument2")]
    public string Argument2 { get; set; }
    
    [JsonPropertyName("argument3")]
    public string Argument3 { get; set; }
    
}