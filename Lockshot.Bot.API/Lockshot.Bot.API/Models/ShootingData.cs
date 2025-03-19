namespace Lockshot.Bot.API.Models
{
    public class ShootingData
    {
        public string WeaponType { get; set; }
        public int Score { get; set; }
        public DateTime Timestamp { get; set; }
        public double Distance { get; set; }
        public double Metrics { get; set; }
    }

}
