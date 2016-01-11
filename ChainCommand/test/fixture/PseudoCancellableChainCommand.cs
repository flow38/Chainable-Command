using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChainCommand.test.fixture
{
    class PseudoCancellableChainCommand : CancellableChainCommand
    {
        public int Counter
        {
            get; set;
        } = 0;

        public override void Execute()
        {
            Counter++;
            base.Execute();
        }

        protected override void DoCancel()
        {
            Counter--;
            base.DoCancel();
        }
    }
}
