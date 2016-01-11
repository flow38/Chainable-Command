using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChainCommand;

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
            Cancellable = true;
        }

        public void OnCancel(Action callback)
        {
            _onCancelDone.Add(callback);
        }

        public void Cancel()
        {
            if (!Cancellable)
                throw new Exception("You try to cancel a command which is not cancelable !!");

            if (_chainedCommand != null && _chainedCommand.IsCancellable()) {
                ICancellableChainCommand cancellableChainCmd = _chainedCommand as ICancellableChainCommand;
                cancellableChainCmd.OnCancel(DoCancel);
                cancellableChainCmd.Cancel();
            } else {
                DoCancel();
            }
        }

        /// <summary>
        ///     Do all your stuff to revert back to pre execute() state
        ///     Developper must override this method to do all cancellation stuff and
        ///     Override implementation should always end by a call to base.DoCancel() (or chained cancellation feature will be
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
            int length = _onCancelDone.Count;
            for (int i = 0; i < length; i++)
            {
                _onCancelDone[i].Invoke();
            }
        }
    }
}
