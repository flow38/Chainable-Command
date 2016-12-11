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
        /// Register a callback for execution done "event"
        /// 
        /// Note 1 : You can register as many callback as you want.
        /// Note 2 : Registers callbacks will only be invoke when chained command (if exist ) is done.
        /// </summary>
        /// <param name="callback"></param>
        void OnExecuteDone(Action callback);


        /// <summary>
        /// Clean internal command stuff in order to do a new Execute call : 
        /// - Clear chained command if exist one
        /// - Set isDone flag to true
        /// - Remove all  callbacks registered via OnExecuteDone API.
        ///  
        /// Note : could perfectly fit in a pool pattern context
        /// </summary>
        void Clear();

        bool IsCancellable();

        bool IsDone();
    }
}