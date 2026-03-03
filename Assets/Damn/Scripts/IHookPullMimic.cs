using System.Collections;

public interface IHookable
{
    IEnumerator OnHook(PlayerController player, GridManager grid);
}

public interface IPullable
{
    IEnumerator OnPull(PlayerController player, GridManager grid);
}

public interface IMimicable
{
    void Bind(PlayerController player);
    void Unbind();
}