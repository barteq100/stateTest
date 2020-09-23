using System;
using System.Collections.Generic;
using System.Text;

namespace stateTest
{
    public class MyState : IStateRoot
    {
        public class InitState : State<MyState> { }

        public class EndState : State<MyState> { }

    }
}
