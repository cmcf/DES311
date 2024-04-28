using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPurchaseListener
{
    public bool HandlePurchase(Object newPurchase);
}
public class Credits : MonoBehaviour
{
    [SerializeField] int credits;
    [SerializeField] Component[] purchaseListeners;

    List<IPurchaseListener> purchaseListenersList = new List<IPurchaseListener>();

    void Start()
    {
        GetPurchaseListeners();
    }

    void GetPurchaseListeners()
    {
        foreach(Component listener in purchaseListeners)
        {
            IPurchaseListener listenerInterface = listener as IPurchaseListener;

            if (listenerInterface != null)
            {
                purchaseListenersList.Add(listenerInterface);
            }
        }
    }

    void BroadcastPurchase(Object item) 
    {
        foreach (IPurchaseListener purchaseListener in purchaseListenersList)
        {
            if (purchaseListener.HandlePurchase(item))
            {
                return;
            }
        }
    }
    public int Credit
    {
        get { return credits; }
    }

    public delegate void OnCreditChanged(int newCredit);
    public event OnCreditChanged onCreditChanged;

    public bool Purchase(int price, Object item)
    {
        // Check if player has enough credits
        if (GameManager.Instance.gameData.totalCredits < price)
            return false;

        // Subtract price from current credits
        GameManager.Instance.gameData.totalCredits -= price;

        // Broadcast purchase event
        BroadcastPurchase(item);

        return true;
    }
}
