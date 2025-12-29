namespace NEPlumbingInc.Services;

public class DarkModeService
{
    private bool _isDarkMode;
    
    public bool IsDarkMode
    {
        get => _isDarkMode;
        set
        {
            if (_isDarkMode != value)
            {
                _isDarkMode = value;
                OnDarkModeChanged?.Invoke(_isDarkMode);
            }
        }
    }

    public event Action<bool>? OnDarkModeChanged;

    public void Initialize(bool isDark)
    {
        _isDarkMode = isDark;
    }
}
