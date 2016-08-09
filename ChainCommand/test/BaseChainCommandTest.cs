using System.Timers;
using NUnit.Framework;
using ChainCommand.test.fixture;
using Moq;
using NFluent;

namespace ChainCommand.test
{
    [TestFixture]
    class BaseChainCommandTest
    {

        [Test]
        public void TestOnExecuteDone()
        {
            bool myFlag = false;
            IChainCommand cmd = new BasicChainCommand();
            cmd.OnExecuteDone(delegate () {
                myFlag = true;
            });
            cmd.Execute();
            Assert.IsTrue((myFlag));
        }

        [Test]
        public void TestChainedExecute()
        {
            IChainCommand cmd1 = new BasicChainCommand();
            bool myFlag = false;
            cmd1.OnExecuteDone(delegate () {
                myFlag = true;
            });

            IChainCommand cmd2 = new BasicChainCommand();
            Mock<BasicChainCommand> cmd3 = new Mock<BasicChainCommand>() { CallBase = true };
            cmd1.Chain(cmd2).Chain(cmd3.Object);

            cmd1.Execute();

            cmd3.Verify(c => c.Execute(), Times.Once);
            Check.That(myFlag).IsTrue();
        }

        [Test]
        public void TestChainedCancel()
        {
            PseudoCancellableChainCommand cmd1 = new PseudoCancellableChainCommand();
            PseudoCancellableChainCommand cmd2 = new PseudoCancellableChainCommand();
            PseudoCancellableChainCommand cmd3 = new PseudoCancellableChainCommand();

            cmd1.Chain(cmd2).Chain(cmd3);
            cmd1.Cancel();

            Check.That(cmd3.Counter).IsEqualTo(-1);
        }

        [Test]
        public void TestClear()
        {
            bool onCancelFlag = false;
            bool onExecuteFlag = false;
            PseudoCancellableChainCommand cmd = new PseudoCancellableChainCommand();
            cmd.OnCancel(delegate () {
                onCancelFlag = true;
            });

            cmd.OnExecuteDone(delegate () {
                onExecuteFlag = true;
            });


            cmd.Clear();
            cmd.Execute();
            cmd.Cancel();
            Check.That(onExecuteFlag).IsFalse();
            Check.That(onCancelFlag).IsFalse();
        }



    }


}
