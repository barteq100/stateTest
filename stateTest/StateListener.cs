using System;
using System.Collections.Generic;
using System.Text;

namespace stateTest
{
    public class StateListener
    {
        public FSM<MyState> StateMachine;

        public StateListener()
        {
            StateMachine = new FSM<MyState>();
            StateMachine.OnTransition<MyState.InitState>(OnInit);
            StateMachine.OnTransition<MyState.EndState>(OnEndState);
            StateMachine.Transition(new MyState.InitState());
            StateMachine.Transition(new MyState.EndState());
        }

        public void OnEndState(MyState.EndState state)
        {
            Console.WriteLine($"EndState A: {state.A}");
        } 

        public void OnInit(MyState.InitState state)
        {
            Console.WriteLine("StartState");
        }
    }
}
