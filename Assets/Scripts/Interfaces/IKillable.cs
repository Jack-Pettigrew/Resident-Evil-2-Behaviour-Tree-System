using System.Collections;

public interface IKillable
{
    bool isDead {get;}
    void Die();

    /// <summary>
    /// Handles the gameobject after it has been killed. This is perfect for destroying the object after sometime or providing some more involved logic.
    /// </summary>
    IEnumerator CleanUp();
}
