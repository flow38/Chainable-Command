using System;
using System.Collections.Generic;

namespace ChainCommand
{
    public class BaseChainCommand : IChainCommand
    {
        protected IChainCommand chainedCommand;

        protected bool cancellable = false;

        protected bool isDone = true;

        private List<Action> _onExecuteDone = new List<Action>();


        public virtual void Execute()
        {
            if (!isDone)
                throw new Exception("You try to execute a command which is already is executed !!  Do you have Clear() command instance before execute it?");
            isDone = false;
        }


        public IChainCommand Chain(IChainCommand cmd)
        {
            chainedCommand = cmd;

            return cmd;
        }

        public virtual void Clear()
        {
            isDone = true;
            chainedCommand?.Clear();
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

        public void OnExecuteDone(Action callback)
        {
            _onExecuteDone.Add(callback);
        }

        protected void done()
        {
            if (chainedCommand != null)
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
            isDone = true;
            int length = _onExecuteDone.Count;
            for (int i = 0; i < length; i++)
            {
                _onExecuteDone[i].Invoke();
            }
        }
    }
}
