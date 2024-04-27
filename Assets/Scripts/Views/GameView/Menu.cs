
using UnityEngine;

public abstract class Menu : View
{
    public override void Hide()
    {
        ResetAllVar();
        base.Hide();
    }

    protected virtual void ResetAllVar()
    {
        
    }
}