namespace EAappEmulater.Enums;

public enum GameState
{
    
    /**
     * 未启动
     */
    NOT_STARTED = 1,
    
    /**
     * 启动中
     */
    STARTING = 2,
    
    /**
     * 启动失败
     */
    START_FAILED = 3,
    
    /**
     * 空闲中
     */
    IDLE = 4,
    
    /**
     * 进服载入中
     */
    JOIN_LOADING = 5,
    
    /**
     * 退服载入中
     */
    LEAVE_LOADING = 6,
    
    /**
     * 排队中
     */
    JOIN_QUEUE = 7,
    
    /**
     * 使用中
     */
    IN_GAME = 8,
    
    /**
     * 已掉线
     */
    DISCONNECT = 9,
    
    /**
     * 连线中
     */
    CONNECTING = 10,
    
    /**
     * Cookie失效
     */
    COOKIE_INVALID = 11,
    
    /**
     * 未知状态
     */
    UNKNOWN = 12,
    
    /**
     * 一般加载中
     */
    COMMON_LOADING = 13,
    
}