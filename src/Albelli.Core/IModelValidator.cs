namespace Albelli.Core;

public interface IModelValidator
{
    Task ValidateAndThrowAsync<T>(T model);
}