public class ExecutionContext
{
    public PlayerController player;
    public GridManager grid;

    public ExecutionContext(PlayerController p, GridManager g)
    {
        player = p;
        grid = g;
    }
}