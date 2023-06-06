namespace ME.BECS.Editor {

    public static class MainMenu {
        
        [UnityEditor.MenuItem("ME.BECS/Worlds Viewer...")]
        public static void ShowWorldsViewer() {
            
            WorldGraphEditorWindow.ShowWindow();
            
        }

        [UnityEditor.MenuItem("ME.BECS/Regenerate Assemblies")]
        public static void CodeGeneratorRegenerateAsms() {

            CodeGenerator.RegenerateBurstAOT();

        }

        #if ME_BECS_EDITOR_INTERNAL
        [UnityEditor.MenuItem("ME.BECS/Internal/GenerateComponentsParallelFor")]
        public static void CodeGenInternalGenerateComponentsParallelFor() {
            
            CodeGenerator.GenerateComponentsParallelFor();
            
        }
        #endif

    }

}