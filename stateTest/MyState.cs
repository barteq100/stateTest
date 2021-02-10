using System;
using System.Collections.Generic;
using System.Text;

namespace stateTest
{
    public class MyState : IStateRoot
    {
        public class InitState : IState<MyState> { }

        public class EndState : IState<MyState> {
            public int A = 5;
        }

    }
}
