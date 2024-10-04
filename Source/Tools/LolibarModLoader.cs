using System.Reflection;

namespace LolibarApp.Source.Tools;

public class LolibarModLoader
{
    public static List<LolibarMod> LocalMods { get; private set; } = new List<LolibarMod>();

    static Type[] GetTypesInNamespace(Assembly assembly, string nameSpace)
    {
        return
          assembly.GetTypes()
                  .Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal))
                  .ToArray();
    }
    static object GetTypeInstance(string TypeName)
    {
        return Activator.CreateInstance(Type.GetType(TypeName));
    }
    public static void LoadMods()
    {
        var modtypes = GetTypesInNamespace(Assembly.GetExecutingAssembly(), "LolibarApp.Mods");
        foreach (var modtype in modtypes)
        {
            LocalMods.Add((LolibarMod)GetTypeInstance(modtype.FullName));
        }
    }
    public static void PreInitializeMods()
    {
        foreach (var mod in LocalMods)
        {
            mod.PreInitialize();
        };
    }
    public static void InitializeMods()
    {
        foreach (var mod in LocalMods)
        {
            mod.Initialize();
        };
    }
    public static void UpdateMods()
    {
        foreach (var mod in LocalMods)
        {
            mod.Update();
        };
    }
}