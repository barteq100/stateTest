using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace stateTest
{
    public interface IStateRoot
    {
        
    }
    public interface IStateRoot<T> : IStateRoot
    {
    }

    public class StateRoot<T> : IStateRoot<T>
    {
    }

    public interface IState { }
    public interface IState<T> : IState
    {

    }


    public class State<T> : IState<T> where T : StateRoot<T>
    {
        public delegate void StateTransition(IState<T> state);
        public StateTransition OnEnter;
        public StateTransition OnExit;

    }

    public class FSM<T> where T: StateRoot<T>
    {
        private State<T> CurrentState { get; set; }
        private Dictionary<Type, State<T>> States = new Dictionary<Type, State<T>>();
        private Dictionary<Type, State<T>.StateTransition> Transitions = new Dictionary<Type, State<T>.StateTransition>();

        public FSM()
        {
        }

        public FSM(State<T> init)
        {
           Transition(init);
        }


        public void Transition<K>(K nextState) where K : State<T>
        {
            CurrentState?.OnExit?.Invoke(CurrentState);
            State<T> state;
            if (!States.TryGetValue(typeof(K), out state))
            {
                States.Add(typeof(K), Activator.CreateInstance<K>());
            }
            nextState.OnEnter = States[typeof(K)].OnEnter;
            nextState.OnExit = States[typeof(K)].OnExit;
            nextState.OnEnter?.Invoke(nextState);
            CurrentState = nextState;
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

        public void OnEnter<K>(Action<K> action) where K : State<T>
        {
            State<T> state;
            if(!States.TryGetValue(typeof(K), out state))
            {
                States.Add(typeof(K), Activator.CreateInstance<K>());
            }
            States[typeof(K)].OnEnter += ParseAction(action);
        }

        public void OnExit<K>(Action<K> action) where K : State<T>
        {
            State<T> state;
            if (!States.TryGetValue(typeof(K), out state))
            {
                States.Add(typeof(K), Activator.CreateInstance<K>());
            }
            States[typeof(K)].OnExit += ParseAction(action);
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
