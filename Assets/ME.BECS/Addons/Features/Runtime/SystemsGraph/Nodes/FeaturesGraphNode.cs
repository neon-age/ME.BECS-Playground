
namespace ME.BECS.FeaturesGraph.Nodes {

    using g = System.Collections.Generic;
    using Extensions.GraphProcessor;

    [System.Serializable]
    public abstract class FeaturesGraphNode : BaseNode {

        [UnityEngine.HideInInspector]
        public FeaturesGraph.SystemsGraph customRuntimeSystemRoot;
        public SystemsGraph featuresGraph => this.customRuntimeSystemRoot != null ? this.customRuntimeSystemRoot : this.graph as SystemsGraph;
        
        public SystemHandle runtimeHandle;

        public ref SystemGroup runtimeSystemGroup => ref this.featuresGraph.runtimeRootSystemGroup;

    }

}