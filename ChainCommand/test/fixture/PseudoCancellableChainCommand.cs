
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

            base.Execute();
            Counter++;
            done();
        }

        protected override void DoCancel()
        {
            Counter--;
            base.DoCancel();
        }
    }
}
