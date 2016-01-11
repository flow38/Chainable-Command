using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ChainCommand
{
    public interface IChainCommand
    {
        /// <summary>
        /// Execute command
        /// </summary>
        void Execute();

        void OnExecuteDone(Action callback);
        
        /// <summary>
        /// Chain command execution/cancellation to another IChainCommand
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns>The chained command</returns>
        IChainCommand Chain(IChainCommand cmd);

        /// <summary>
        /// Clean instance reference, callbacks and all state relative stuff
        /// Feet perfectly in a pool pattern context....
        /// </summary>
        void Clear();

        Boolean IsCancellable();
    }
}