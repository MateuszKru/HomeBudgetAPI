namespace HomeBudget.Service.ModelsDTO.UserModels
{
    public class NewUserDTO : UserDTO
    {
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}