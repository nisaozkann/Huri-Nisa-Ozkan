namespace UzayOyunu.Core
{
    public interface IEntity
    {
        int X { get; set; }
        int Y { get; set; }
        bool IsUzayli { get; set; }
        char Sembol { get; }
    }
}
