using System;

namespace ChainCommand
{
    public interface ICancellableChainCommand : IChainCommand
    {
        /// <summary>
        /// Cancel command execution 
        /// </summary>
        void Cancel();

        /// <summary>
        /// Register a callback for cancellation done "event"
        /// 
        /// You can register as many callback as you want.
        /// Registers callbacks will only be invoke when chained command (if exist ) cancellation is done.
        /// </summary>
        /// <param name="callback"></param>
        void OnCancel(Action callback);
    }
}
