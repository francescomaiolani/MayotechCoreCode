using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mayotech.Behaviour
{
    public abstract class Behaviour : MonoBehaviour
    { }

    public abstract class ActiveBehaviour : Behaviour
    {
        public abstract void ExecuteBehaviour();
    }
    
    public abstract class ActiveBehaviour<T> : Behaviour
    {
        public abstract void ExecuteBehaviour(T data);
    }

    public abstract class PassiveBehaviour : Behaviour
    {
        
    }
    
}

namespace Mayotech.Timer
{ }

