[System.Serializable]
public abstract class BaseState
{
    protected CombatManager Manager;
    public BaseState( CombatManager manager )
    {
        Manager = manager;
    }

    abstract public void OnStateEnter();
    abstract public void OnExitState( );
}
