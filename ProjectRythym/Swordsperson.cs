using System;

namespace ProjectRythym
{
    public enum SwordsPersonState { Still, Attack, Hurt, Death }

    public class Swordsperson
    {
        protected SwordsPersonState currentState;
        public SwordsPersonState CurrentState
        {
            get { return this.currentState; }
            set
            {
                if (this.currentState != value)
                {
                    this.currentState = value;
                }
            }
        }

        public Swordsperson()
        {
            this.CurrentState = SwordsPersonState.Still;
        }
    }
}
