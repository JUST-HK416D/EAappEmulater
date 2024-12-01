using EAappEmulater.Helper;

namespace EAappEmulater.Models;

public class StarterConfig
{
    /**
     * 配置文件路径
     */
    private string ConfigFilePath { get; set; }
    
    /**
     * 当前client编号
     */
    public string ClientNo { get; private set; }
    
    /**
     * 游戏状态更新间隔
     */
    public int StateUpdateInterval { get; private set; }
    
    /**
     * 数据上传间隔
     */
    public int DataUploadInterval { get; private set; }
    
    /**
     * AFK间隔
     */
    public int AfkInterval { get; private set; }
    
    /**
     * 账号更新间隔
     */
    public int AccountUpdateInterval { get; private set; }
    
    public StarterConfig()
    {
        this.EnsureConfig();
        this.ReadConfig();
    }
    
    #region 确保配置文件
    private void EnsureConfig()
    {
        // 获取配置文件
        var dirPath = Path.Combine("./config");
        FileHelper.CreateDirectory(dirPath);
        this.ConfigFilePath = Path.Combine(dirPath, "Config.ini");
    }
    #endregion

    #region 读取配置文件
    private void ReadConfig()
    {
        this.ReadClientNo();
        this.ReadRuntimeParameters();
    }
    #endregion
    
    #region 写入配置
    public void Write(string section, string key, string value)
    {
        IniHelper.WriteString(section, key, value, ConfigFilePath);
        this.ReadConfig();
    }
    #endregion
    
    #region 读取ClientNo
    private void ReadClientNo()
    {
        ClientNo = IniHelper.ReadString("Starter", "ClientNo", ConfigFilePath);
        if (!string.IsNullOrWhiteSpace(ClientNo))
        {
            return;
        }
        var random = new Random();
        ClientNo = random.NextInt64(10000000, 99999999).ToString();
        IniHelper.WriteString("Starter", "ClientNo", ClientNo, ConfigFilePath);
    }
    #endregion
    
    #region 读取运行时参数
    private void ReadRuntimeParameters()
    {
        this.ReadStateUpdateInterval();
        this.ReadDataUploadInterval();
        this.ReadAfkInterval();
        this.ReadAccountUpdateInterval();
    }
    #endregion

    #region 读取状态更新间隔
    private void ReadStateUpdateInterval()
    {
        var stateUpdateIntervalStr = IniHelper.ReadString("Runtime", "StateUpdateInterval", ConfigFilePath);
        if (string.IsNullOrWhiteSpace(stateUpdateIntervalStr))
        {
            stateUpdateIntervalStr = "3";
            IniHelper.WriteString("Runtime", "StateUpdateInterval", stateUpdateIntervalStr, ConfigFilePath);
        }
        StateUpdateInterval = int.Parse(stateUpdateIntervalStr);
    }
    #endregion

    #region 读取数据上传间隔
    private void ReadDataUploadInterval()
    {
        var dataUploadIntervalStr = IniHelper.ReadString("Runtime", "DataUploadInterval", ConfigFilePath);
        if (string.IsNullOrWhiteSpace(dataUploadIntervalStr))
        {
            dataUploadIntervalStr = "5";
            IniHelper.WriteString("Runtime", "DataUploadInterval", dataUploadIntervalStr, ConfigFilePath);
        }
        DataUploadInterval = int.Parse(dataUploadIntervalStr);
    }
    #endregion

    #region 读取AFK间隔
    private void ReadAfkInterval()
    {
        var afkIntervalStr = IniHelper.ReadString("Runtime", "AfkInterval", ConfigFilePath);
        if (string.IsNullOrWhiteSpace(afkIntervalStr))
        {
            afkIntervalStr = "60";
            IniHelper.WriteString("Runtime", "AfkInterval", afkIntervalStr, ConfigFilePath);
        }
        AfkInterval = int.Parse(afkIntervalStr);
    }
    #endregion

    #region 读取账号更新间隔
    private void ReadAccountUpdateInterval()
    {
        var accountUpdateIntervalStr = IniHelper.ReadString("Runtime", "AccountUpdateInterval", ConfigFilePath);
        if (string.IsNullOrWhiteSpace(accountUpdateIntervalStr))
        {
            accountUpdateIntervalStr = "300";
            IniHelper.WriteString("Runtime", "AccountUpdateInterval", accountUpdateIntervalStr, ConfigFilePath);
        }
        AccountUpdateInterval = int.Parse(accountUpdateIntervalStr);
    }
    #endregion

}