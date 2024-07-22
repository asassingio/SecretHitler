using PlayerComponents;
using UnityEngine;

public abstract class View : MonoBehaviour
{
    public bool IsInitialized { get; private set; }
    protected static MyPlayerController OwnerPlayerController { get; private set; }
	
    public virtual void Initialize()
    {
        IsInitialized = true;
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }
	
    public virtual void Hide()
    {
        ResetAllVar();
        gameObject.SetActive(false);
    }
    
    protected virtual void ResetAllVar()
    {
        
    }
	
    public static void SetOwnerPlayer(MyPlayerController ownerPlayerController)
    {
        OwnerPlayerController = ownerPlayerController;
    }
}