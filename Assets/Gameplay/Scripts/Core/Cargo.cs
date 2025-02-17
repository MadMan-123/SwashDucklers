public class Cargo : Item
{
    public Value value;
    public enum Value
    {
        NoValue = -1,
        Low,
        Medium,
        High
    }
}