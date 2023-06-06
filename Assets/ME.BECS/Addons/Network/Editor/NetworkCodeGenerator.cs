
namespace ME.BECS.Network.Editor {

    using ME.BECS.Editor;
    
    public class NetworkCodeGenerator : CustomCodeGenerator {
        
        public override CodeGenerator.MethodDefinition AddMethod(System.Collections.Generic.List<System.Type> references) {
            
            var content = new System.Collections.Generic.List<string>();
            var methods = UnityEditor.TypeCache.GetMethodsWithAttribute<NetworkMethodAttribute>();
            references.Add(typeof(UnsafeNetworkModule));
            foreach (var method in methods) {

                if (method.IsStatic == false) continue;
                if (method.DeclaringType.IsVisible == false) continue;
                
                if (this.IsValidTypeForAssembly(method.DeclaringType) == false) continue;

                var str = $"methods.Add({method.DeclaringType.FullName}.{method.Name});";
                content.Add(str);

            }
            
            var def = new CodeGenerator.MethodDefinition() {
                methodName = "NetworkLoad",
                type = "ME.BECS.Network.UnsafeNetworkModule.MethodsStorage",
                definition = "ref ME.BECS.Network.UnsafeNetworkModule.MethodsStorage methods",
                content = string.Join("\n", content.ToArray()),
            };
            return def;
            
        }

    }

}