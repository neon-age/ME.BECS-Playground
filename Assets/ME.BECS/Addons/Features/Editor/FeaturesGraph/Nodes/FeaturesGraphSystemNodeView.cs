using System.Reflection;

namespace ME.BECS.Editor.FeaturesGraph.Nodes {
    
    using UnityEditor;
    
    [ME.BECS.Extensions.GraphProcessor.NodeCustomEditor(typeof(ME.BECS.FeaturesGraph.Nodes.SystemNode))]
    public class FeaturesGraphSystemNodeView : FeaturesGraphNodeView {

        protected override void DrawDefaultInspector(bool fromInspector = false) {

            if (fromInspector == false) {

                if (this.nodeTarget is ME.BECS.FeaturesGraph.Nodes.SystemNode systemNode &&
                    systemNode.system != null) {

                    var type = systemNode.system.GetType();
                    var tooltip = type.GetCustomAttribute<UnityEngine.TooltipAttribute>();
                    if (tooltip != null) {

                        var typeStr = EditorUtils.GetComponentName(type);
                        var label = new UnityEngine.UIElements.Label($"<b>{typeStr}</b>\n{tooltip.tooltip}");
                        label.AddToClassList("node-tooltip");
                        this.Add(label);

                    }

                }

            }
            
            base.DrawDefaultInspector(fromInspector);
            
        }

        private static void AddLabel(UnityEngine.UIElements.VisualElement container, string label, string tooltip, bool burst) {
            
            var burstLabel = new UnityEngine.UIElements.Label(label);
            burstLabel.tooltip = tooltip;
            burstLabel.AddToClassList(burst == true ? "burst-label" : "no-burst-label");
            container.Add(burstLabel);
                        
        }

        private static bool IsBurstMethod(ISystem system, System.Type interfaceType, bool systemBurst, out bool hasInterface) {
            var isBurst = systemBurst;
            hasInterface = false;
            if (interfaceType.IsInstanceOfType(system) == true) {
                hasInterface = true;
                var map = system.GetType().GetInterfaceMap(interfaceType);
                var method = map.TargetMethods[0];
                {
                    var withBurst = method.GetCustomAttribute<Unity.Burst.BurstCompileAttribute>() != null;
                    if (withBurst == true) {
                        isBurst = true;
                    } else {
                        var noBurst = method.GetCustomAttribute<WithoutBurstAttribute>() != null;
                        if (noBurst == true) {
                            isBurst = false;
                        }
                    }
                }
            }

            return isBurst;
        }

        protected override void CreateLabels(UnityEngine.UIElements.VisualElement container) {

            var node = this.nodeTarget as ME.BECS.FeaturesGraph.Nodes.SystemNode;
            if (node?.system != null) {
                var isBurst = node.system.GetType().GetCustomAttribute<Unity.Burst.BurstCompileAttribute>() != null;
                var awakeBurst = IsBurstMethod(node.system, typeof(IAwake), isBurst, out var hasAwake);
                var updateBurst = IsBurstMethod(node.system, typeof(IUpdate), isBurst, out var hasUpdate);
                var disposeBurst = IsBurstMethod(node.system, typeof(IDestroy), isBurst, out var hasDestroy);
                
                if (awakeBurst == true && updateBurst == true && disposeBurst == true && isBurst == true) {
                    AddLabel(container, "Burst", "System run with Burst Compiler", true);
                } else if (isBurst == false && awakeBurst == false && updateBurst == false && disposeBurst == false) {
                    AddLabel(container, "No Burst", "System run without Burst Compiler", false);
                } else {
                    if (hasAwake == true) {
                        if (awakeBurst == true) {
                            AddLabel(container, "Awake Burst", "Method run with Burst Compiler", true);
                        } else {
                            AddLabel(container, "Awake No Burst", "Method run without Burst Compiler", false);
                        }
                    }

                    if (hasUpdate == true) {
                        if (updateBurst == true) {
                            AddLabel(container, "Update Burst", "Method run with Burst Compiler", true);
                        } else {
                            AddLabel(container, "Update No Burst", "Method run without Burst Compiler", false);
                        }
                    }

                    if (hasDestroy == true) {
                        if (disposeBurst == true) {
                            AddLabel(container, "Destroy Burst", "Method run with Burst Compiler", true);
                        } else {
                            AddLabel(container, "Destroy No Burst", "Method run without Burst Compiler", false);
                        }
                    }
                }
            }
            
        }

        public override void BuildContextualMenu(UnityEngine.UIElements.ContextualMenuPopulateEvent evt) {
            
            var system = (this.nodeTarget as ME.BECS.FeaturesGraph.Nodes.SystemNode).system;
            if (system != null) {
                evt.menu.AppendAction($"Open Script {system.GetType().Name}...", (e) => OpenScript(), this.OpenScriptStatus);
            }
            
        }

        private UnityEngine.UIElements.DropdownMenuAction.Status OpenScriptStatus(UnityEngine.UIElements.DropdownMenuAction arg) {

            var system = (this.nodeTarget as ME.BECS.FeaturesGraph.Nodes.SystemNode).system;
            if (system != null) {
                var script = FindScriptFromClassName(system.GetType().Name);
                if (script != null) return UnityEngine.UIElements.DropdownMenuAction.Status.Normal;
            }
            
            return UnityEngine.UIElements.DropdownMenuAction.Status.Disabled;

        }

        private void OpenScript() {
            
            var system = (this.nodeTarget as ME.BECS.FeaturesGraph.Nodes.SystemNode).system;
            if (system != null) {
                var script = FindScriptFromClassName(system.GetType().Name);
                if (script != null) AssetDatabase.OpenAsset(script.GetInstanceID(), 0, 0);
            }
            
        }
        
        static MonoScript FindScriptFromClassName(string className)
        {
            var scriptGUIDs = AssetDatabase.FindAssets($"t:script {className}");

            if (scriptGUIDs.Length == 0)
                return null;

            foreach (var scriptGUID in scriptGUIDs)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(scriptGUID);
                var script = AssetDatabase.LoadAssetAtPath<MonoScript>(assetPath);

                if (script != null && string.Equals(className, System.IO.Path.GetFileNameWithoutExtension(assetPath), System.StringComparison.OrdinalIgnoreCase))
                    return script;
            }

            return null;
        }

    }

}