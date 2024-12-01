using EAappEmulater.Enums;
using EAappEmulater.Helper;
using EAappEmulater.Models;
using System.Net.Http;

namespace EAappEmulater.Core;

public static class StartupParameterStarterApp
{
    /**
     * 数据上传定时器
     */
    private static Timer _dataUploadTimer;
    
    /**
     * 游戏状态更新定时器
     */
    private static Timer _stateUpdateTimer;

    /**
     * 账号信息更新定时器
     */
    private static Timer _accountUpdateTimer;

    /**
     * 防止afk踢出定时器
     */
    private static Timer _preventAfkKickTimer;

    /**
     * 首次校验游戏中定时器
     */
    private static Timer _firstCheckInGameTimer;
    
    /**
     * 最终校验游戏中定时器
     */
    private static Timer _finalCheckInGameTimer;
    
    //////////////////////////////////////////////////////////////

    /**
     * 当前服务器gameId
     */
    private static long? _gameId;
    
    //////////////////////////////////////////////////////////////
    
    /**
     * 游戏状态
     */
    private static GameState _gameState = GameState.IDLE;
    
    //////////////////////////////////////////////////////////////

    /**
     * 当前账号名称
     */
    private static string _personaName;

    /**
     * 当前账号pid
     */
    private static long? _personaId;

    /**
     * 当前账号uid
     */
    private static long? _userId;
    
    //////////////////////////////////////////////////////////////
    
    /**
     * 上传地址
     */
    private const string DataUploadUrl = "https://starter.battlefield.tools/starter/consume";
    
    /**
     * 校验地址
     */
    private const string DataGetterUrl = "https://data.battlefield.tools/api/player-list";

    /**
     * 请求客户端
     */
    private static readonly HttpClient HttpClient;
    
    /**
     * 暖服机配置
     */
    private static StarterConfig _starterConfig;
    
    /**
     * 暖服机数据
     */
    private static readonly StarterData StarterData;

    /**
     * 玩家列表请求类
     */
    private static readonly PlayerListReqVo PlayerListReqVo;
    
    /**
     * 随机类
     */
    private static readonly Random Random = new();
    
    #region 静态构造器
    static StartupParameterStarterApp()
    {
        HttpClient = new HttpClient();
        HttpClient.Timeout = TimeSpan.FromSeconds(5);
        
        StarterData = new StarterData();
        PlayerListReqVo = new PlayerListReqVo { GameIds = new List<long>() };
    }
    #endregion

    #region 启动程序
    public static void Run()
    {
        _starterConfig = new StarterConfig();
        
        LoggerHelper.Info("正在启动暖服机进程...");
        
        _preventAfkKickTimer = new Timer(PreventAfkKick, null, TimeSpan.Zero, TimeSpan.FromSeconds(_starterConfig.AfkInterval));
        LoggerHelper.Info("防止AFK踢出任务已启动...");
        
        _accountUpdateTimer = new Timer(AccountUpdate, null, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(_starterConfig.AccountUpdateInterval));
        LoggerHelper.Info("账号信息更新任务已启动...");
        
        _stateUpdateTimer = new Timer(StateUpdate, null, TimeSpan.FromSeconds(6), TimeSpan.FromSeconds(_starterConfig.StateUpdateInterval));
        LoggerHelper.Info("游戏状态更新任务已启动...");
        
        _dataUploadTimer = new Timer(DataUpload, null, TimeSpan.FromSeconds(8), TimeSpan.FromSeconds(_starterConfig.DataUploadInterval));
        LoggerHelper.Info("数据上传任务已启动...");
    }
    #endregion
    
    #region 防AFK定时任务
    [DllImport("zddll.dll", EntryPoint = "Keyboard_Click", CharSet = CharSet.Ansi)]
    private static extern void KeyboardClick(int ascii);
    
    private static void PreventAfkKick(object o)
    {
        try
        {
            IntPtr hWnd = Win32.FindWindow(null, "Battlefield™ 1");
            if (hWnd == IntPtr.Zero)
            {
                return;
            }
            // 显示窗口
            Win32.ShowWindow(hWnd, 9);
            Thread.Sleep(Random.Next(250, 350));
            // 前置窗口
            Win32.SetForegroundWindow(hWnd);
            Thread.Sleep(Random.Next(250, 350));
            // 按下tab
            KeyboardClick(9);
            // 最小化窗口
            Thread.Sleep(Random.Next(250, 350));
            Win32.ShowWindow(hWnd, 6);   
        }
        catch (Exception e)
        {
            LoggerHelper.Error("防AFK异常", e);
        }
    }
    #endregion

    #region 账号信息更新定时任务
    private static void AccountUpdate(object o)
    {
        try
        {
            _personaName = !string.IsNullOrWhiteSpace(Account.PlayerName) ? Account.PlayerName : null;
            _personaId = !string.IsNullOrWhiteSpace(Account.PersonaId) ? long.Parse(Account.PersonaId) : null;
            _userId = !string.IsNullOrWhiteSpace(Account.UserId) ? long.Parse(Account.UserId) : null;
            SkipAllModeVideo();
        }
        catch (Exception e)
        {
            LoggerHelper.Error("更新账号信息异常", e);
        }
    }
    #endregion

    #region 跳过模式动画
    private static void SkipAllModeVideo()
    {
        string myDocumentsFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string battlefield1FolderPath = Path.Combine(myDocumentsFolderPath, "Battlefield 1");
        string twinkleFolderPath = Path.Combine(battlefield1FolderPath, "twinkle");
        string kvstoreFolderPath = Path.Combine(twinkleFolderPath, "kvstore");
        FileHelper.CreateDirectory(kvstoreFolderPath);
        SkipModeVideo(kvstoreFolderPath, "kv_gameMode-AA");
        SkipModeVideo(kvstoreFolderPath, "kv_gameMode-BT");
        SkipModeVideo(kvstoreFolderPath, "kv_gameMode-BTL");
        SkipModeVideo(kvstoreFolderPath, "kv_gameMode-CQ");
        SkipModeVideo(kvstoreFolderPath, "kv_gameMode-CQ_assault");
        SkipModeVideo(kvstoreFolderPath, "kv_gameMode-DOM");
        SkipModeVideo(kvstoreFolderPath, "kv_gameMode-POS");
        SkipModeVideo(kvstoreFolderPath, "kv_gameMode-R");
        SkipModeVideo(kvstoreFolderPath, "kv_gameMode-TDM");
        SkipModeVideo(kvstoreFolderPath, "kv_gameMode-TOW");
        SkipModeVideo(kvstoreFolderPath, "kv_gameMode-ZC");
        SkipModeVideo(kvstoreFolderPath, "kv_legal_companion");
    }
    
    private static void SkipModeVideo(string folderPath, string fileName)
    {
        long? snapshotUserId = _userId;
        if (snapshotUserId == null)
        {
            return;
        }
        string filePath = FileHelper.CreateFile(folderPath, fileName);
        string fileStr = File.ReadAllText(filePath);
        Dictionary<string, bool> file;
        if (string.IsNullOrWhiteSpace(fileStr))
        {
            file = new Dictionary<string, bool>();
            if (fileName.Equals("kv_legal_companion"))
            {
                file["1"] = true;
            }
        }
        else
        {
            file = JsonSerializer.Deserialize<Dictionary<string, bool>>(fileStr);
        }
        if (file.ContainsKey(snapshotUserId.ToString()) && file[snapshotUserId.ToString()])
        {
            return;
        }
        file[snapshotUserId.ToString()] = true;
        fileStr = JsonSerializer.Serialize(file);
        File.WriteAllText(filePath, fileStr);
    }
    #endregion
    
    #region 游戏状态更新定时任务
    private static void StateUpdate(object o)
    {
        try
        {
            // 游戏启动了是加载中
            if (ProcessHelper.IsAppRun("bf1"))
            {
                // 如果此时是游戏中，就跳过
                if (_gameState == GameState.IN_GAME)
                {
                    return;
                }
                _gameState = GameState.JOIN_LOADING;
            }
            // eaac启动了是加载中
            else if (ProcessHelper.IsAppRun("EAAntiCheat.GameServiceLauncher"))
            {
                _gameState = GameState.JOIN_LOADING;
            }
            // 其他情况下就是闲置
            else
            {
                _gameState = GameState.IDLE;
                _gameId = null;
            }
        }
        catch (Exception e)
        {
            LoggerHelper.Error("状态更新异常信息：" + e.Message);
            LoggerHelper.Error("状态更新异常堆栈：" + e.StackTrace);
        }
    }
    #endregion

    #region 数据上传定时任务
    private static void DataUpload(object o)
    {
        string snapshotPersonaName = _personaName;
        if (string.IsNullOrWhiteSpace(snapshotPersonaName))
        {
            LoggerHelper.Info("上传数据由于无法获取personaName中止");
            return;
        }
        long? snapshotPersonaId = _personaId;
        if (snapshotPersonaId == null)
        {
            LoggerHelper.Info("上传数据由于无法获取personaId中止");
            return;
        }
        long? snapshotUserId = _userId;
        if (snapshotUserId == null)
        {
            LoggerHelper.Info("上传数据由于无法获取userId中止");
            return;
        }
        try
        {
            StarterData.ClientNo = _starterConfig.ClientNo;
            StarterData.Type = 1;
            StarterData.SubType = 0;
            StarterData.PersonaName = snapshotPersonaName;
            StarterData.PersonaId = snapshotPersonaId;
            StarterData.UserId = snapshotUserId;
            StarterData.GameState = _gameState;
            StarterData.GameId = _gameId;
            StarterData.Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            // 发起http请求
            var content = new StringContent(JsonSerializer.Serialize(StarterData), Encoding.UTF8, "application/json");
            var response = HttpClient.PostAsync(DataUploadUrl, content).Result;
            response.EnsureSuccessStatusCode();
            var responseBody = response.Content.ReadAsStringAsync().Result;
            // 反序列化回复类
            var starterResult = JsonSerializer.Deserialize<StarterResult>(responseBody);
            CommandHandle(starterResult);
        }
        catch (Exception e)
        {
            LoggerHelper.Error("数据上传异常信息：" + e.Message);
            LoggerHelper.Error("数据上传异常堆栈：" + e.StackTrace);
        }
    }
    #endregion

    #region 指令处理
    private static void CommandHandle(StarterResult starterResult)
    {
        // 没有指令就结束
        if (starterResult.Data?.Command == null)
        {
            return;
        }
        // 转换指令
        var starterCommand = (Command) starterResult.Data.Command.Value;
        // 调用指令
        switch (starterCommand)
        {
            case Command.START:
                Task.Run(StartGame);
                break;
            case Command.JOIN:
                Task.Run(() => JoinGame(long.Parse(starterResult.Data.Argument2)));
                break;
            case Command.LEAVE:
                Task.Run(LeaveGame);
                break;
            case Command.UPDATE:
                Task.Run(() => UpdateRuntimeParameters(starterResult.Data.Argument1, starterResult.Data.Argument2));
                break;
            case Command.DEPLOY:
                Task.Run(Deploy);
                break;
            default:
                return;
        }
    }
    #endregion
    
    #region 启动游戏
    private static void StartGame()
    {
        ProcessHelper.CloseProcess("bf1");
        ProcessHelper.CloseProcess("EAAntiCheat.GameService");
        ProcessHelper.CloseProcess("EAAntiCheat.GameServiceLauncher");
        SkipAllModeVideo();
    }
    #endregion

    #region 加入服务器
    private static void JoinGame(long gameId)
    {
        ProcessHelper.CloseProcess("bf1");
        ProcessHelper.CloseProcess("EAAntiCheat.GameService");
        ProcessHelper.CloseProcess("EAAntiCheat.GameServiceLauncher");
        SkipAllModeVideo();
        _gameState = GameState.JOIN_LOADING;
        _gameId = gameId;
        Game.RunGame(GameType.BF1, $"-Window.Height 100 -Window.Width 200 -VeniceUI.LoadingMoviesEnabled false -Window.Minimized true -Client.SkipFastLevelLoad true -Online.EnableSnowroller true -VeniceOnline.EnableSnowroller true -Sound.Enable false -Render.NullRendererEnable true -Mesh.LoadingEnabled false -Client.EmittersEnabled false -Core.HardwareProfile Hardware_Low -Client.TerrainEnabled false -Core.HardwareCpuBias -1 -Core.HardwareGpuBias -1 -Texture.RenderTexturesEnabled false -RenderDevice.CreateMinimalWindow true -VeniceUI.UseSparta false -requestState State_ConnectToGameId -gameId \"{gameId}\" -gameMode \"MP\" -role \"soldier\" -asSpectator \"false\" -parentSessinId -joinWithParty \"false\" -RenderDevice.NullDriverEnable true -RenderDevice.MinDriverRequired false");
        // Game.RunGame(GameType.BF1, $"-Window.Width 240 -Window.Height 180 -Window.Minimized true -VeniceUI.LoadingMoviesEnabled false -Sound.Enable false -Render.NullRendererEnable true -Mesh.LoadingEnabled false -Client.EmittersEnabled false -Core.HardwareProfile Hardware_Low -Client.TerrainEnabled false -Core.HardwareCpuBias -1 -Core.HardwareGpuBias -1 -Texture.RenderTexturesEnabled false -VeniceUI.UseSparta false -Client.SkipFastLevelLoad true -Online.EnableSnowroller true -VeniceOnline.EnableSnowroller true -requestState State_ConnectToGameId -gameId \"{gameId}\" -gameMode \"MP\" -role \"soldier\" -asSpectator \"false\" -parentSessinId -joinWithParty \"false\"");
        _firstCheckInGameTimer?.Dispose();
        _firstCheckInGameTimer = new Timer(FirstCheckInGame, null, TimeSpan.FromMinutes(3), Timeout.InfiniteTimeSpan);
        _finalCheckInGameTimer?.Dispose();
        _finalCheckInGameTimer = new Timer(FinalCheckInGame, null, TimeSpan.FromMinutes(5), Timeout.InfiniteTimeSpan);
    }
    #endregion

    #region 校验是否在服务器中
    private static void FirstCheckInGame(object o)
    {
        try
        {
            if (!ProcessHelper.IsAppRun("bf1"))
            {
                return;
            }
            long? snapshotGameId = _gameId;
            if (snapshotGameId == null)
            {
                return;
            }
            long? snapshotPersonaId = _personaId;
            if (snapshotPersonaId == null)
            {
                return;
            }
            PlayerListReqVo.GameIds.Clear();
            PlayerListReqVo.GameIds.Add(snapshotGameId.Value);
            var content = new StringContent(JsonSerializer.Serialize(PlayerListReqVo), Encoding.UTF8, "application/json");
            var response = HttpClient.PostAsync(DataGetterUrl, content).Result;
            response.EnsureSuccessStatusCode();
            var responseBody = response.Content.ReadAsStringAsync().Result;
            // 反序列化回复类
            var playerListRespVo = JsonSerializer.Deserialize<PlayerListRespVo>(responseBody);
            List<long> playerList = playerListRespVo.Data[snapshotGameId.Value.ToString()];
            if (playerList.Contains(snapshotPersonaId.Value))
            {
                LoggerHelper.Info("状态校验成功：游戏中");
                _gameState = GameState.IN_GAME;
            }
        } 
        catch (Exception e)
        {
            LoggerHelper.Error("状态校验异常", e);
        }
    }
    
    private static void FinalCheckInGame(object o)
    {
        try
        {
            if (!ProcessHelper.IsAppRun("bf1"))
            {
                return;
            }
            long? snapshotGameId = _gameId;
            if (snapshotGameId == null)
            {
                return;
            }
            long? snapshotPersonaId = _personaId;
            if (snapshotPersonaId == null)
            {
                return;
            }
            PlayerListReqVo.GameIds.Clear();
            PlayerListReqVo.GameIds.Add(snapshotGameId.Value);
            var content = new StringContent(JsonSerializer.Serialize(PlayerListReqVo), Encoding.UTF8, "application/json");
            var response = HttpClient.PostAsync(DataGetterUrl, content).Result;
            response.EnsureSuccessStatusCode();
            var responseBody = response.Content.ReadAsStringAsync().Result;
            // 反序列化回复类
            var playerListRespVo = JsonSerializer.Deserialize<PlayerListRespVo>(responseBody);
            List<long> playerList = playerListRespVo.Data[snapshotGameId.Value.ToString()];
            if (playerList.Contains(snapshotPersonaId.Value))
            {
                LoggerHelper.Info("状态校验成功：游戏中");
                _gameState = GameState.IN_GAME;
            }
            else
            {
                LoggerHelper.Info("状态校验成功：未知");
                ProcessHelper.CloseProcess("bf1");
                ProcessHelper.CloseProcess("EAAntiCheat.GameService");
                ProcessHelper.CloseProcess("EAAntiCheat.GameServiceLauncher");
            }
        } 
        catch (Exception e)
        {
            LoggerHelper.Error("状态校验异常", e);
        }
    }
    #endregion

    #region 离开服务器
    private static void LeaveGame()
    {
        ProcessHelper.CloseProcess("bf1");
        ProcessHelper.CloseProcess("EAAntiCheat.GameService");
        ProcessHelper.CloseProcess("EAAntiCheat.GameServiceLauncher");
        _firstCheckInGameTimer?.Dispose();
        _finalCheckInGameTimer?.Dispose();
        _gameState = GameState.LEAVE_LOADING;
    }
    #endregion

    #region 部署落地
    private static void Deploy()
    {
        try
        {
            IntPtr hWnd = Win32.FindWindow(null, "Battlefield™ 1");
            if (hWnd == IntPtr.Zero)
            {
                return;
            }
            // 显示窗口
            Win32.ShowWindow(hWnd, 9);
            Thread.Sleep(Random.Next(250, 350));
            // 前置窗口
            Win32.SetForegroundWindow(hWnd);
            Thread.Sleep(Random.Next(500, 600));
            // 按下空格
            KeyboardClick(32);
            Thread.Sleep(Random.Next(1000, 1500));
            KeyboardClick(32);
            Thread.Sleep(Random.Next(1000, 1500));
            KeyboardClick(32);
            Thread.Sleep(Random.Next(1000, 1500));
            KeyboardClick(32);
            // 最小化窗口
            Thread.Sleep(Random.Next(500, 600));
            Win32.ShowWindow(hWnd, 6);   
        }
        catch (Exception e)
        {
            LoggerHelper.Error("部署异常", e);
        }
    }
    #endregion

    #region 更新运行时参数
    private static void UpdateRuntimeParameters(string key, string value)
    {
        _starterConfig.Write("Runtime", key, value);
    }
    #endregion

    #region 关闭程序
    public static void Stop()
    {
        _stateUpdateTimer?.Dispose();
        _stateUpdateTimer = null;
        LoggerHelper.Info("游戏状态更新任务已关闭...");
        _dataUploadTimer?.Dispose();
        _dataUploadTimer = null;
        LoggerHelper.Info("数据上传任务已关闭...");
        _preventAfkKickTimer?.Dispose();
        _preventAfkKickTimer = null;
        LoggerHelper.Info("防止AFK踢出任务已关闭...");
        _accountUpdateTimer?.Dispose();
        _accountUpdateTimer = null;
        LoggerHelper.Info("账号信息更新任务已关闭...");
        LoggerHelper.Info("SessionId刷新任务已关闭...");
    }
    #endregion
    
}