namespace FluentCRUD.Abstractions;

public interface IGenerationStep<TModel>
{
	string GenerateCode(TModel model);
}