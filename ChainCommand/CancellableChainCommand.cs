using System;
using System.Collections.Generic;

namespace ChainCommand
{
    /// <summary>
    /// Base class for cancellable command implementation
    /// </summary>
    public class CancellableChainCommand : BaseChainCommand, ICancellableChainCommand
    {

        private List<Action> _onCancelDone = new List<Action>();

        public CancellableChainCommand()
        {
            cancellable = true;
        }

        public void OnCancel(Action callback)
        {
            _onCancelDone.Add(callback);
        }

        public void Cancel()
        {
            if (!cancellable)
                throw new Exception("You try to cancel a command which is not cancelable !!");

            if (!isDone)
                throw new Exception("You try to cancel a command which is currently executing itself !! You have to wait command ending before cancell it !");

            isDone = false;

            if (chainedCommand != null && chainedCommand.IsCancellable())
            {
                ICancellableChainCommand cancellableChainCmd = chainedCommand as ICancellableChainCommand;
                cancellableChainCmd.OnCancel(DoCancel);
                cancellableChainCmd.Cancel();
            }
            else
            {
                DoCancel();
            }
        }

        /// <summary>
        ///     Implementate in concrete classes all cancellation actions.
        ///     Beware : concrete classes implementations MUST always end by a call to base.DoCancel() (or chained cancellation feature will be
        ///     broken)
        /// </summary>
        protected virtual void DoCancel()
        {
            invokeOnCancelDone();
        }

        public override void Clear()
        {
            _onCancelDone.Clear();
            base.Clear();
        }

        private void invokeOnCancelDone()
        {
            isDone = true;
            int length = _onCancelDone.Count;
            for (int i = 0; i < length; i++)
            {
                _onCancelDone[i].Invoke();
            }
        }
    }
}
