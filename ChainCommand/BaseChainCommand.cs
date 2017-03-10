using System;
using System.Collections.Generic;

namespace ChainCommand
{
    public abstract class BaseChainCommand : IChainCommand
    {
        protected IChainCommand chainedCommand;

        protected bool cancellable = false;

        protected bool isDone = false;

        private List<Action> _onExecuteDone = new List<Action>();

        protected IChainCommand _previousCommand;

        /// <summary>
        /// Concrete class implementations must always call base implementation at first line
        /// in order to mark instance as currently processing (isDone flag) or  benefit of below
        /// isDone value checking. 
        /// </summary>
        public virtual void Execute()
        {
            if(isDone)
                throw new Exception("You try to execute a command which is already is executed !!  Do you have Clear() command instance before execute it?");
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
                (cmd as BaseChainCommand)._previousCommand = this;
            }
            else
            {
                chainedCommand.Chain(cmd);
            }

            return cmd;
        }
        /// <summary>
        /// Prepare instance for GC collection
        /// </summary>
        public virtual void Clear()
        {
            isDone = false;
            chainedCommand?.Clear();
            _previousCommand = null;
            _onExecuteDone.Clear();
        }

        public bool IsCancellable()
        {
            return cancellable;
        }

        public bool IsDone()
        {
            return isDone;
        }

        public IChainCommand PreviousCommand()
        {
            return _previousCommand;
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

        public void OnExecuteDone(Action callback)
        {
            _onExecuteDone.Add(callback);
        }

        public void FlagAsDone()
        {
            isDone = true;
        }

        /// <summary>
        /// Concrete classes must call this method to trigger chained class execution
        /// and/or end execution callback invokation.
        /// 
        /// If concrete classes override this method, they MUST call at the end of their implementation 
        /// base.done().
        /// </summary>
        protected void done()
        {
            isDone = true;
            if(chainedCommand != null && !chainedCommand.IsDone())
            {

                //We listen an chained command executin done event to execute our invokeOnExecuteDone method
                chainedCommand.OnExecuteDone(delegate () {
                    invokeOnExecuteDone();
                });

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
