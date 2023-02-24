namespace SystemServiceAPICore3.Dto.AdminConfigDto
{
    public class AdminConfigDto
    {
        public string query { get; set; }
    }

    public class ConfigPriceRequest
    {
        public int ConfigID { get; set; }

        public int Postage { get; set; }
    }
}
