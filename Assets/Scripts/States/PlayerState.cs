using System;

public class PlayerState : BaseState
{
    public PlayerState(CombatManager manager) : base(manager)
    {
    }
    public override void OnStateEnter()
    {
        Arrow.OnArrowStoppedAction += ManageState;//Subscribe to the event
        Manager.Player.CNCam.enabled = true;
        Manager.Player.CanShoot = true;
    }

    private void ManageState( Arrow arrow )
    {
        if( arrow == null || arrow.Owner != Manager.Player ) return;

        //Leave this state
        Manager.SetState( new EnemyState( Manager) );
    }
    public override void OnExitState()
    {
        Arrow.OnArrowStoppedAction -= ManageState;//Unsubscribe from the event
        Manager.Player.CNCam.enabled = false;
    }
}
