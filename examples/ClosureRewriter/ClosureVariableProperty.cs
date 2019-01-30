using System.Management.Automation;

namespace PSSyntaxRewriter
{
    public class ClosureVariableProperty : PSPropertyInfo
    {
        private static readonly string s_objectTypeName =
            Microsoft.PowerShell.ToStringCodeMethods.Type(PSObject.AsPSObject(typeof(object)));

        private readonly PSVariable _variable;

        internal ClosureVariableProperty(string name)
        {
            _variable = new PSVariable(name);
            SetMemberName(name);
        }

        internal ClosureVariableProperty(PSVariable variable)
        {
            _variable = variable;
            SetMemberName(variable.Name);
        }

        public override bool IsGettable => true;

        public override bool IsSettable => true;

        public override PSMemberTypes MemberType => PSMemberTypes.Property;

        public override string TypeNameOfValue => _variable.Value != null
            ? Microsoft.PowerShell.ToStringCodeMethods.Type(PSObject.AsPSObject(_variable.Value.GetType()))
            : s_objectTypeName;

        public override object Value
        {
            get => _variable.Value;
            set => _variable.Value = value;
        }

        public override PSMemberInfo Copy() => new ClosureVariableProperty(_variable);
    }
}
