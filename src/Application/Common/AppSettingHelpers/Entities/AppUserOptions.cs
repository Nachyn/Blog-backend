namespace Application.Common.AppSettingHelpers.Entities
{
    public class AppUserOptions
    {
        public int EmailMaxLength { get; set; }

        public int UsernameMaxLength { get; set; }

        public int UsernameMinLength { get; set; }

        public string UsernameRegex { get; set; }
    }
}