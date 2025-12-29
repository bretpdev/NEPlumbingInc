namespace NEPlumbingInc.Services;

public class ThemeService
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
                OnChange?.Invoke();
            }
        }
    }

    public event Action? OnChange;

    public void SetDarkMode(bool isDark)
    {
        IsDarkMode = isDark;
    }

    public void Initialize(bool isDark)
    {
        _isDarkMode = isDark;
    }
}
