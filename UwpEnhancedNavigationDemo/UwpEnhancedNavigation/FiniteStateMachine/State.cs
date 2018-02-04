using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UwpEnhancedNavigation.FiniteStateMachine
{
    public class Transition
    {
        public Triggers Trigger { get; set; }
        public States State { get; set; }
        public Func<Boolean> Guard;
    }

    public class InternalTransition
    {
        public Triggers Trigger { get; set; }
        public Func<Boolean> Exec;
    }

    public class State
    {
        States _state = States.UNKNOWN;
        public States StateType
        {
            get { return _state; }
        }

        int _numberOfTriggers = 0;
        //Dictionary<Triggers, Func<Triggers, States>> _onEntryAction = new Dictionary<Triggers, Func<Triggers, States>>();
        //Dictionary<Triggers, Func<Triggers, States>> _onExitAction = new Dictionary<Triggers, Func<Triggers, States>>();
        List<Func<Boolean>> _onEntryAction = new List<Func<Boolean>>();
        List<Func<Boolean>> _onExitAction = new List<Func<Boolean>>();
        List<InternalTransition> _onSelfTriggerAction = new List<InternalTransition>();
        List<Transition> _transitions = new List<Transition>();

        private State(States state)
        {
            _state = state;
            _numberOfTriggers = Enum.GetNames(typeof(Triggers)).Length;
        }

        /// <summary>
        /// Throws an exception if the state transition has not been defined
        /// </summary>
        /// <param name="trigger"></param>
        /// <returns></returns>
        private States InvalidTrigger(Triggers trigger)
        {
            throw new StateTransitionException("Invalid Trigger: " + trigger);
        }

        static public State Configure(States state)
        {
            return new State(state);
        }

        /// <summary>
        /// Sets up an action based on a trigger
        /// Returns the pointer to the statemachine as per a Fluent Design (This may not be perfect)
        /// </summary>
        /// <param name="trigger"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public State InternalTrigger(Triggers trigger, Func<Boolean> func)
        {
            InternalTransition tTransition = new InternalTransition();
            tTransition.Trigger = trigger;
            tTransition.Exec = func;

            _onSelfTriggerAction.Add(tTransition);
            return this;
        }

        /// <summary>
        /// Sets up an action based on a trigger
        /// Returns the pointer to the statemachine as per a Fluent Design (This may not be perfect)
        /// </summary>
        /// <param name="trigger"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public State OnEntry(Func<Boolean> func)
        {
            _onEntryAction.Add(func);
            return this;
        }

        /// <summary>
        /// Sets up an action based on a trigger
        /// Returns the pointer to the statemachine as per a Fluent Design (This may not be perfect)
        /// </summary>
        /// <param name="trigger"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public State OnExit(Func<Boolean> func)
        {
            _onExitAction.Add(func);
            return this;
        }

        /// <summary>
        /// Initiates the action and returns the new state
        /// </summary>
        /// <param name="trigger"></param>
        /// <returns></returns>
        public void EnteringState(Triggers trigger)
        {
            foreach (Func<Boolean> func in _onEntryAction)
            {
                func.Invoke();
            }
        }

        /// <summary>
        /// Initiates the action and returns the new state
        /// </summary>
        /// <param name="trigger"></param>
        /// <returns></returns>
        public void ExitingState(Triggers trigger)
        {
            foreach (Func<Boolean> func in _onExitAction)
            {
                func.Invoke();
            }
        }

        public State Permit(Triggers trigger, States newState)
        {
            return PermitIf(trigger, newState, EmptyGuard);
        }

        public State PermitIf(Triggers trigger, States newState, Func<Boolean> guard)
        {
            Transition tTransition = new Transition();
            tTransition.Trigger = trigger;
            tTransition.State = newState;
            tTransition.Guard = guard;

            _transitions.Add(tTransition);
            return this;
        }

        public States NextState(Triggers trigger)
        {
            foreach(Transition trans in _transitions)
            {
                if (trans.Trigger == trigger)
                {
                    // We have a valid, trigger, check the guard
                    if (trans.Guard != null)
                    {
                        Boolean guardPassed = trans.Guard.Invoke();
                        if (guardPassed)
                        {
                            return trans.State;
                        }
                    }
                }
            }

            return States.UNKNOWN;
        }

        public Boolean InternalTransition(Triggers trigger)
        {
            foreach (InternalTransition trans in _onSelfTriggerAction)
            {
                if (trans.Trigger == trigger)
                {
                    // We have a valid, trigger, check the guard
                    if (trans.Exec != null)
                    {
                        return trans.Exec.Invoke();
                    }
                }
            }

            return false;
        }

        private Boolean EmptyGuard()
        {
            return true;
        }
    }
}
