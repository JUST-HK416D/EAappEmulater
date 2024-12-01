namespace EAappEmulater.Core;

public static class Win32
{
    [DllImport("user32.dll")]
    public static extern int SetForegroundWindow(IntPtr hwnd);
    
    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
    
    [DllImport("user32.dll")]
    public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
}