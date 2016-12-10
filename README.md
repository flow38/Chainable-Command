# Chainable-Command#
A command pattern implementation where commands can be chained to create "macro" command.

## IChainCommand API
```

    public interface IChainCommand
    {
        /// <summary>
        /// Execute command
        /// </summary>
        void Execute();

        void OnExecuteDone(Action callback);

        /// <summary>
        /// Chain command execution/cancellation to another IChainCommand
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns>The chained command</returns>
        IChainCommand Chain(IChainCommand cmd);

        /// <summary>
        /// Clean internal command stuff in order to do a new Execute call
        /// 
        /// Feet perfectly in a pool pattern context....
        /// </summary>
        void Clear();

        /// <summary>
        /// Prepare instance for garbage collection
        /// </summary>
        void Destroy();

        bool IsCancellable();

        bool IsDone();
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

...to continue
