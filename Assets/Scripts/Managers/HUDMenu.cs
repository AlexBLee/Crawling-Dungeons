using UnityEngine;

public class HUDMenu : MonoBehaviour
{
    public Player player;

    public virtual void Init()
    {

    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}
