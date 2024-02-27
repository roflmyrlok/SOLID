namespace SF.Core;

[AttributeUsage(AttributeTargets.Class)]
public class InputActionAttribute : Attribute
{
	public string[] Type { get; }

	public InputActionAttribute(string[] type)
	{
		Type = type;
	}
}