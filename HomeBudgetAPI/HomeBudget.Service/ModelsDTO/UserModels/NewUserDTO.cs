namespace HomeBudget.Service.ModelsDTO.UserModels
{
    public class NewUserDTO : AppUserDTO
    {
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}