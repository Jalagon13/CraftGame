using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettlerStateFactory
{
    private SettlerStateMachine _context;

    public SettlerStateFactory(SettlerStateMachine currentContext)
    {
        _context = currentContext;
    }

    public SettlerBaseState Idle()
    {
        return new SettlerIdleState(_context, this);
    }
    public SettlerBaseState Move()
    {
        return new SettlerMoveState(_context, this);
    }
    public SettlerBaseState Grounded()
    {
        return new SettlerGroundedState(_context, this);
    }
}
