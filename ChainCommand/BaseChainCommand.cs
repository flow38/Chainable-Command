using System;
using System.Collections.Generic;

namespace ChainCommand
{
    public abstract class BaseChainCommand : IChainCommand
    {
        protected IChainCommand chainedCommand;

        protected bool cancellable = false;

        protected bool hasBeenExecuted = false;

        protected bool inProgress = false;

        protected List<Action> _onExecuteDone = new List<Action>();

        private IChainCommand _previousCommand;

        public IChainCommand PreviousCommand
        {
            get { return _previousCommand; }
            set { _previousCommand = value; }
        }


        /// <summary>
        /// Concrete class implementations must always call base implementation at first line
        /// in order to mark instance as currently processing (isDone flag) or  benefit of below
        /// isDone value checking. 
        /// </summary>
        public virtual void Execute()
        {
            if(hasBeenExecuted)
                throw new Exception("You try to execute a command which is already have been executed !!  Do you have Clear() command instance before execute it?");
            if(inProgress)
                throw new Exception("You try to execute a command which is currently in progress !! ");
            inProgress = true;
        }

        /// <summary>
        /// Recursive chaining
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public IChainCommand Chain(IChainCommand cmd)
        {
            if(chainedCommand == null)
            {
                chainedCommand = cmd;
                cmd.PreviousCommand = this;
            }
            else
            {
                chainedCommand.Chain(cmd);
            }

            return cmd;
        }

        public void UnChain()
        {
            if(chainedCommand != null)
            {
                (chainedCommand as BaseChainCommand)._previousCommand = null;
                chainedCommand = null;
            }
        }

        /// <summary>
        /// Prepare instance for GC collection
        /// </summary>
        public virtual void Clear()
        {
            hasBeenExecuted = false;
            inProgress = false;
            chainedCommand?.Clear();
            _previousCommand = null;
            _onExecuteDone.Clear();
        }

        public bool IsCancellable()
        {
            return cancellable;
        }

        public bool HasBeenExecuted()
        {
            return hasBeenExecuted;
        }

        public void FlagAsExecuted()
        {
            hasBeenExecuted = true;
        }

        public bool IsInProgress()
        {
            return inProgress;
        }
        
        public IChainCommand NextCommand()
        {
            return chainedCommand;
        }

        public IChainCommand LastCommand()
        {
            if(chainedCommand == null)
                return this;
            else
                return chainedCommand.LastCommand();
        }

        public virtual void OnExecuteDone(Action callback)
        {
            _onExecuteDone.Add(callback);
        }


        /// <summary>
        /// Concrete classes must call this method to trigger chained class execution
        /// and/or end execution callback invokation.
        /// 
        /// If concrete classes override this method, they MUST call at the end of their implementation 
        /// base.done().
        /// </summary>
        protected virtual void done()
        {
            hasBeenExecuted = true;
            inProgress = false;
            if(chainedCommand != null && !chainedCommand.HasBeenExecuted())
            {
                //We listen an chained command executin done event to execute our invokeOnExecuteDone method
                chainedCommand.OnExecuteDone(invokeOnExecuteDone);

                //Execute chained command 
                chainedCommand.Execute();
            }
            else
            {
                //Direct execution done callback invokation
                invokeOnExecuteDone();
            }
        }

        private void invokeOnExecuteDone()
        {
            int length = _onExecuteDone.Count;
            for(int i = 0; i < length; i++)
            {
                _onExecuteDone[i].Invoke();
            }
        }

    }
}
