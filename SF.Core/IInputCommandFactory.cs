namespace SF.Core;

public interface IInputCommandFactory
{
    List<IInputAction> GetAllActions();
}