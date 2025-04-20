public class LoginViewModel
 {
     [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your username")]
     public string? UserName { get; set; }
 
     [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your password")]
     public string? Password { get; set; }
 }