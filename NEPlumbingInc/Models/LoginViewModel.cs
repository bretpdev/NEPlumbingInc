namespace NEPlumbingInc.Models;

public class LoginViewModel
 {
     [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your username")]
     public string UserName { get; set; } = string.Empty;
 
     [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your password")]
     public string Password { get; set; } = string.Empty;
 }