namespace Point_Adjust_Robot.Core.Model
{
    public class Authetication
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string scope { get; set; }
        public string language { get; set; }
        public string jti { get; set; }
        public int expires_in { get; set; }
        public int user_account_id { get; set; }
    }
}
