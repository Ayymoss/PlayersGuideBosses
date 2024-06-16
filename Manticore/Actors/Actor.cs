namespace Manticore.Actors;

public class Actor(int health)
{
    public int InitialHealth { get; } = health;
    public int Health { get; private set; } = health;
    public void Damage(int amount) => Health -= amount;
}
