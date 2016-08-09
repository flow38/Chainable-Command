using System;

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
