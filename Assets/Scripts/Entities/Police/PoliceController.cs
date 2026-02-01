using UnityEngine;

public enum PoliceState { Idle, RunFrom, RunTo, Fire }
public class PoliceController : StateMachine<PoliceController>
{
    private void Awake()
    {
        //stateDictionary.Add();
    }
}