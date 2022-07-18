namespace RepositoryFramework.UnitTest.Repository.Models
{
    public class Auto
    {
        public int Identificativo { get; set; }
        public string? Targa { get; set; }
        public Guidatore? Guidatore { get; set; }
        public int NumeroRuote { get; set; }
    }
    public class Guidatore
    {
        public string? Nome { get; set; }
    }
}
