using System.Reflection;

namespace FluentCodeGenTool;

public static class UsingCollector
{
	public static HashSet<string> CollectUsings(IEnumerable<PropertyInfo> properties)
	{
		var namespaces = new HashSet<string>();

		foreach (var prop in properties)
		{
			AddNamespaceRecursively(prop.PropertyType, namespaces);
		}

		// Добавь системные по умолчанию, если нужно
		namespaces.Add("System");

		return namespaces;
	}

	private static void AddNamespaceRecursively(Type type, HashSet<string> nsSet)
	{
		if (type.Namespace != null)
			nsSet.Add(type.Namespace);

		if (type.IsGenericType)
		{
			foreach (var arg in type.GetGenericArguments())
			{
				AddNamespaceRecursively(arg, nsSet);
			}
		}

		if (type.IsArray)
		{
			AddNamespaceRecursively(type.GetElementType()!, nsSet);
		}
	}
}