public enum CommandType
{
    Move,
    Turn,
    Wait
}

public enum Facing
{
    Up,
    Right,
    Down,
    Left
}

[System.Serializable]
public class Instruction
{
    public CommandType command;
    public int parameter;

    public Instruction(CommandType cmd, int param = 0)
    {
        this.command = cmd;
        this.parameter = param;
    }
}