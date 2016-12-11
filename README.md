# Chainable-Command#
A command pattern implementation where commands can be chained to create "macro" command.

## IChainCommand API
`public interface IChainCommand
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

        bool IsDone();
    }
```
## ICancellableChainCommand API
```
 interface ICancellableChainCommand : IChainCommand
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
```
## Basic Command
```
public class CustomCommandA : BaseChainCommand
    {
        public override void Execute()
        {
            //Keep this base class method call to benefit of isDone flag checking (execute a "not done" command will throw an exception)
            base.Execute();
            
            //
            // Do your stuff here
            //
            
            //This done method call will trigger execution of chained command if exist and finally call all delegate action register throught OnExecuteDone API
            done();
        }
        
    }
```

## Asynchrone Command
```
public class CustomCommandB : BaseChainCommand
    {
        public override void Execute()
        {
            //Keep this base class method call to benefit of isDone flag checking (execute a "not done" command will throw an exception)
            base.Execute();
            
            //
            // Do some asynchrone operation here and use onCustomAsyncTaskDone as "callback"
            //
            
        }
        
        private void onCustomAsyncTaskDone()
        {
            //
            //Some custom post async task operations
            //
            
            done();
        }
        
    }
```
## Cancellable Command
```
public class PseudoCancellableChainCommand : CancellableChainCommand
    {
        public int Counter
        {
            get; set;
        } = 0;

        public override void Execute()
        {
            base.Execute();
            Counter++;
            done();
        }

        protected override void DoCancel()
        {
            Counter--;
            base.DoCancel();
        }
    }
```
## Chained Commands
```
IChainCommand cmdA = new CustomCommandA();
IChainCommand cmdB = new CustomCommandB();
IChainCommand cmdC = new CustomCommandC();
cmdA.Chain(cmdA).Chain(cmdB).chain(cmdC);
cmdA.OnExecuteDone(yourAllCommandDoneCallBack);
cmdA.Execute();

```

## Cancel Commands
```
ICancellableChainCommand cmdA = new CustomCommandA();
ICancellableChainCommand cmdB = new CustomCommandB();
ICancellableChainCommand cmdC = new CustomCommandC();
cmdA.Chain(cmdA).Chain(cmdB).chain(cmdC);
cmdA.OnCancel(yourAllCommandCancelledCallBack);
cmdA.Cancel();

```
