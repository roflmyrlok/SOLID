namespace SF.Core.ExternalInterfaces;

public interface IInputCommandFactory
{
    List<IInputAction> GetAllActions();
}