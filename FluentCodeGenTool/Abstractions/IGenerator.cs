namespace FluentCodeGenTool.Abstractions;

public interface IGenerator
{
	string Generate(Type targetType);
}