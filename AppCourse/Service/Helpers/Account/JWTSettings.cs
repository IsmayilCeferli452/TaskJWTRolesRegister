namespace Service.Helpers.Account
{
    public class JWTSettings
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public int ExpireDays { get; set; }
    }
}
