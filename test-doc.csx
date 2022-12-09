using System;
using System.Reflection;

if (Args.Count != 1)
{
    ShowUsage();
    Environment.Exit(1);
}

var pathToUnitTestAssembly = Path.GetFullPath(Args[0]);

void ShowUsage()
{
    WriteLine("test-docker.csx <path to .NET unit tests assembly (DLL file)>");
}

AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(MyResolveEventHandler);

Assembly MyResolveEventHandler(object sender, ResolveEventArgs args)
{
    var binDir = Path.GetDirectoryName(pathToUnitTestAssembly);
    var assemblyName = args.Name.Split(',')[0];
    var pathToAssembly = Path.Combine(binDir, assemblyName + ".dll");
    return Assembly.LoadFile(pathToAssembly);
}

bool HasAttributeLike(ICustomAttributeProvider t, string attributeName)
{
    var customAttributes = t.GetCustomAttributes(true);

    return customAttributes.Any(a => a.GetType().Name.Contains(attributeName));
}

(IEnumerable<T> activeTests, IEnumerable<T> ignoredTests) PartitionBy<T>(IEnumerable<T> items, Func<T, bool> predicate)
{
        IEnumerable<T> selected = null;
        IEnumerable<T> notSelected = null;
            
        foreach (var g in items.GroupBy(predicate))
        {
            if (g.Key)
            {
                selected = g;
            }
            else
            {
                notSelected = g;
            }
        }

        return (selected ?? Enumerable.Empty<T>(), 
                        notSelected ?? Enumerable.Empty<T>());
}

try
{
    var assembly = Assembly
        .LoadFile(pathToUnitTestAssembly);

    var testClasses = assembly.GetTypes()
        .Where(c => HasAttributeLike(c, "TestFixture"));

    WriteLine($"# Tests in {assembly.GetName().Name}");
    WriteLine();

    foreach (var testClass in testClasses)
    {
        WriteLine($"## Test class {testClass.Name}");

        var methods = testClass.GetMethods().Where(m => HasAttributeLike(m, "Test"));

        var (testMethods, ignoredTestMethods) = PartitionBy(methods, m => !HasAttributeLike(m, "Ignore"));

        foreach(var method in testMethods)
        {
            WriteLine($"- {method.Name}");
        }

        foreach(var method in ignoredTestMethods)
        {
            WriteLine($"- [Ignored] {method.Name}");
        }
        WriteLine();
    }
}
catch (System.Reflection.ReflectionTypeLoadException rex)
{
    WriteLine(rex);
    WriteLine($"{rex.LoaderExceptions.Count()} Loader exceptions");
    foreach(var lex in rex.LoaderExceptions.Take(3))
    {
        WriteLine(lex);
    }
}
catch (Exception ex)
{
    WriteLine(ex);
}
