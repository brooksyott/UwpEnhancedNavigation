using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UwpEnhancedNavigation.FiniteStateMachine
{
    public class EnhancedNavigationFSM
    {
        Dictionary<States, State> _states = new Dictionary<States, State>();
        States _currentState = States.UNKNOWN;
        public States CurrentState
        {
            get { return _currentState; }
        }

        public EnhancedNavigationFSM(States startupState)
        {
            _currentState = startupState;
        }

        /// <summary>
        /// Adds a state to the statemachine
        /// </summary>
        /// <param name="stateName"></param>
        /// <param name="state"></param>
        public State Configure(States state)
        {
            State newState = State.Configure(state);
            _states[state] = newState;
            return newState;
        }


        /// <summary>
        /// Fires the state, and sets a new state
        /// </summary>
        /// <param name="trigger"></param>
        public Boolean Fire(Triggers trigger)
        {
            Debug.WriteLine("Fire Start: State {0}, Trigger = {1}", _currentState, trigger);
            Boolean didTransition = TransitionStates(trigger);
            if (didTransition) return true;

            // If it didn't transition, it might be because it's an internal trigger event
            Boolean internalTransition = InternalTransition(trigger);
            return internalTransition;
        }

        private Boolean InternalTransition(Triggers trigger)
        {
            if (!_states.ContainsKey(_currentState))
            {
                return false; // No transition found
            }

            Boolean didInternalTransition = _states[_currentState].InternalTransition(trigger);
            Debug.WriteLine("InternalTransition: State {0}, Trigger = {1}, Found = {2}", 
                _currentState, trigger, didInternalTransition);
            return true;
        }

        private Boolean TransitionStates(Triggers trigger)
        {
            if (!_states.ContainsKey(_currentState))
            {
                return false; // No transition found
            }

            States nextState = _states[_currentState].NextState(trigger);
            if (nextState == States.UNKNOWN)
            {
                return false; // No transition found
            }
            // Check if the new state has been defined
            if (!_states.ContainsKey(nextState))
            {
                return false; // No transition found
            }
            // We have a valid transition, go to that state
            _states[_currentState].ExitingState(trigger);

            // We should have now exited the last state, enter the new one
            _currentState = nextState;

            _states[_currentState].EnteringState(trigger);
            Debug.WriteLine("Fire End: State {0}, Trigger = {1}", _currentState, trigger);
            return true;
        }
    }
}
