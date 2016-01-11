using System;
using System.Collections.Generic;
using ChainCommand;

namespace ChainCommand
{
    public class BaseChainCommand : IChainCommand
    {
        protected IChainCommand _chainedCommand;

        protected bool Cancellable = false;

        private List<Action> _onExecuteDone = new List<Action>();
        
        public virtual void Execute()
        {
            done();
        }

        
        public IChainCommand Chain(IChainCommand cmd)
        {
            _chainedCommand = cmd;

            return cmd;
        }

        public virtual void Clear()
        {
            _chainedCommand?.Clear();
            _onExecuteDone.Clear();
        }

        public bool IsCancellable()
        {
            return Cancellable;
        }

        public void OnExecuteDone(Action callback)
        {
            _onExecuteDone.Add(callback);
        }

        protected void done()
        {
            if (_chainedCommand != null) {
                //If a executeion done callback exist we listen for chained command end execution
                if (_onExecuteDone != null) {
                    _chainedCommand.OnExecuteDone(delegate()
                    {
                        invokeOnExecuteDone();
                    });
                }
                //Execute chained command 
                _chainedCommand.Execute();
            } else {
                //Direct execution done callback invokation
                invokeOnExecuteDone();
            }
        }

        private void invokeOnExecuteDone()
        {
            int length = _onExecuteDone.Count;
            for (int i = 0; i < length; i++)
            {
                _onExecuteDone[i].Invoke();
            }
        }
    }
}
