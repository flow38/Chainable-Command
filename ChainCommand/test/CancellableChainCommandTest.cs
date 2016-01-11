using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using NFluent;

namespace ChainCommand.test
{
    [TestFixture]
    public class CancellableChainCommandTest
    {
        [Test]
        public void TestOnCancelDone()
        {
            bool myFlag = false;
            CancellableChainCommand cmd = new CancellableChainCommand();
            cmd.OnCancel(delegate () {
                myFlag = true;
            });
            cmd.Cancel();
            Assert.IsTrue((myFlag));
        }

        [Test]
        public void TestChainedCancel()
        {
            CancellableChainCommand cmd1 = new CancellableChainCommand();
            bool myFlag = false;
            cmd1.OnCancel(delegate () {
                myFlag = true;
            });

            CancellableChainCommand cmd2 = new CancellableChainCommand();
            Mock<CancellableChainCommand> cmd3 = new Mock<CancellableChainCommand>() { CallBase = true };
            cmd3.CallBase = true;
            bool myFlag2 = false;
            cmd3.Object.OnCancel(delegate () {
                myFlag2 = true;
            });

            cmd1.Chain(cmd2).Chain(cmd3.Object);

            cmd1.Cancel();

            Check.That(myFlag).IsTrue();
            Check.That(myFlag2).IsTrue();
        }

        [Test]
        public void TestChainedCancelDebug()
        {
            CancellableChainCommand cmd1 = new CancellableChainCommand();
            bool myFlag = false;
            cmd1.OnCancel(delegate () {
                myFlag = true;
            });

            CancellableChainCommand cmd2 = new CancellableChainCommand();
            CancellableChainCommand cmd3 = new CancellableChainCommand();
            bool myFlag2 = false;
            cmd3.OnCancel(delegate () {
                myFlag2 = true;
            });

            cmd1.Chain(cmd2).Chain(cmd3);

            cmd1.Cancel();

            Check.That(myFlag).IsTrue();
            Check.That(myFlag2).IsTrue();
        }

        [Test]
        public void TestDestroy()
        {
            bool onCancelFlag = false;
            CancellableChainCommand cmd = new CancellableChainCommand();
            cmd.OnCancel(delegate () {
                onCancelFlag = true;
            });


            cmd.Clear();
            cmd.Cancel();
            Check.That(onCancelFlag).IsFalse();
        }
    }
}
