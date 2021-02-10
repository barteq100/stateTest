using System;
using System.Collections.Generic;
using System.Text;

namespace stateTest
{
    public class StateDelegate<T> where T : IStateRoot
    {
        public delegate void StateTransition<K>(K state);
    }
    public interface IStateRoot
    {

    }
    public interface IState { }
    public interface IState<T> : IState
    {

    }

    public class State<T> : IState<T> where T: IStateRoot
    {
        public delegate void StateTransition(IState<T> state);
    }

    public class FSM<T> where T: IStateRoot
    {

        public Dictionary<Type, State<T>.StateTransition> Transitions;
        public IState<T> State { get; set; }
        
        public FSM() { 
            Transitions = new Dictionary<Type, State<T>.StateTransition>();
        }
        public FSM(IState<T> init)
        {
           Transition(init);
        }

        public void Transition<K>(K nextState) where K : IState
        {
            if (Transitions.TryGetValue(typeof(K), out var value))
            {
                value?.Invoke((IState<T>)nextState);
            }
            State = (IState<T>)nextState;
        }

        public void OnTransition<K>(Action<K> action)
        {
            if (Transitions.TryGetValue(typeof(K), out var value))
            {
                value += ParseAction(action);
                return;
            }

            Transitions.Add(typeof(K), ParseAction(action));
        }

        private State<T>.StateTransition ParseAction<K>(Action<K> action)
        {
            void callbackAction(IState<T> state)
            {
                action.Invoke((K)state);
            }
            return callbackAction;
        }

    }

}
