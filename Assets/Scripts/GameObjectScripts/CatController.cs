//THIS WILL BE SCRIPT HELD BY CAT GAMEOBJECT

using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class CatController : MonoBehaviour
{
    Cat _data;
    Transform _mineSpot;
    enum State { Idle, MovingToTask, Task }
    State _state;

    public void Bind(Cat cat, Transform taskSpot) //{store reds, set _state based on data.IsMining}
    {

    }
    public void startTask(CatTask task) //{_state = moving to task}
    {
        void Update()
        {
            //movingtomine: MoveTowards(_mineSpot.position); close enough -> _state = mining
            //mining: tick a timer, every N seconds call currencyManager.Instance.Add
        }
    }
}