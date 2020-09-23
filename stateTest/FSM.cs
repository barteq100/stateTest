using System;
using System.Collections.Generic;
using System.Text;

namespace stateTest
{
    public interface IStateRoot
    {

    }
    public interface IState<T> where T: IStateRoot
    {

    }

    public abstract class State<T> : IState<T> where T: IStateRoot
    {
       public delegate void StateTransition(State<T> state);
       public StateTransition OnStateChange;

        public void Invoke()
        {
            OnStateChange?.Invoke(this);
        }
    }
    public class FSM<T> where T: IStateRoot
    {
        public Dictionary<Type, State<T>.StateTransition> Transitions;
        public State<T> State { get; set; }
        
        public FSM() { 
            Transitions = new Dictionary<Type, State<T>.StateTransition>();
        }
        public FSM(State<T> init)
        {
            Transition(init);
        }

        public void Transition<K>(K nextState) where K : State<T>
        {
            if (Transitions.TryGetValue(typeof(K), out var value))
            {
                value?.Invoke(nextState);
            }
            State = nextState;
        }

        public void OnTransition<K>(K.StateTransition action) where K : State<T>
        {
            if (Transitions.TryGetValue(typeof(K), out var value))
            {
                value += (State<T>.StateTransition)action;
                return;
            }

            Transitions.Add(typeof(K), (State<T>.StateTransition)action);
        }

    }

}
