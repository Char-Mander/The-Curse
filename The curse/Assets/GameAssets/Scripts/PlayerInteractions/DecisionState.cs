using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionState : MonoBehaviour
{
    //El balance es la variable que decide la decisión final del jugador: salvar o matar a la niña maldita
    int balance = 0;

    public int CheckBalanceState() { return balance; }

    public void AddOrSubtractToBalance(int value)
    {
        balance += value;
        GameManager.instance.SetDecision(balance);
    }
}
