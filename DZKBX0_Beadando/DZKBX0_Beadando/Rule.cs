namespace DZKBX0_Beadando
{
    internal class Rule
    {
        string inputState;
        string returnState;
        byte ruleID;

        public string InputState { get { return inputState; } }
        public string ReturnState { get { return returnState; } }
        public byte RuleID { get { return ruleID; } }
        
        public Rule(string inputState, string returnState, byte id)
        {
            this.inputState = inputState;
            this.returnState = returnState;
            this.ruleID = id;
        }
    }
}