using System;
namespace ChainCommand
{
    public interface IChainCommand
    {
        /// <summary>
        /// Execute command
        /// </summary>
        void Execute();

        /// <summary>
        /// Chain command execution/cancellation to another IChainCommand
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns>The chained command</returns>
        IChainCommand Chain(IChainCommand cmd);

        /// <summary>
        /// Break link with NextCommand (which return now NULL)
        /// </summary>
        /// <param name="cmd"></param>
        void UnChain();

        /// <summary>
        /// Register a callback for execution done "event"
        /// 
        /// - You can register as many callback as you want.
        /// - Registers callbacks will only be invoke when chained command (if exist ) is done.
        /// </summary>
        /// <param name="callback"></param>
        void OnExecuteDone(Action callback);


        /// <summary>
        /// Clean internal command stuff in order to do a new Execute call : 
        /// -> Clear chained command if exist one
        /// -> Set isDone flag to true
        /// -> Remove all  callbacks registered via OnExecuteDone API.
        ///  
        /// </summary>
        void Clear();

        bool IsCancellable();

        bool HasBeenExecuted();
        void FlagAsExecuted();

        /// <summary>
        /// Do an "execute" operation is currently running ?
        /// </summary>
        /// <returns></returns>
        bool IsInProgress();

        IChainCommand PreviousCommand { get; set; }
        IChainCommand NextCommand();
        IChainCommand LastCommand();

    }
}