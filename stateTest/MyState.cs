using System;
using System.Collections.Generic;
using System.Text;

namespace stateTest
{
    public class MyState : StateRoot<MyState>
    {
        public class InitState : State<MyState> { 
        }

        public class EndState : State<MyState> {
            public int A = 5;
        }

    }
}
