public class EmailSettings
{
    public string From { get; set; } = "";
    public string To { get; set; } = "";
    public string AppPassword { get; set; } = "";
}

// App password will be populated from the user_secrets set
// When that password is available, in terminal run these
// dotnet user-secrets set "Email:AppPassword" "your_new_app_password_here"
// dotnet user-secrets list

// For production, run
// export Email__AppPassword="your_production_app_password_here"
