namespace TheChamberOfDesign;

public class Player(string name)
{
    public string Name { get; } = name;
    public int Wins { get; set; }
    public int Losses { get; set; }
    public int Draws { get; set; }
    public float WinRate => (float)Wins / (Wins + Losses + Draws);
}
