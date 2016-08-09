namespace ChainCommand.test.fixture
{
    public class BasicChainCommand : BaseChainCommand
    {
        public override void Execute()
        {
            base.Execute();
            done();
        }
    }
}