using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChainCommand;

namespace ChainCommand
{
    interface ICancellableChainCommand : IChainCommand
    {
        /// <summary>
        /// Cancel command execution 
        /// </summary>
        void Cancel();

        void OnCancel(Action callback);
    }
}
