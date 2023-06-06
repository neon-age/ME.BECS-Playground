namespace ME.BECS.FeaturesGraph.Nodes {

    using Extensions.GraphProcessor;
    using g = System.Collections.Generic;
    
    [System.Serializable]
    public class StartNode : FeaturesGraphNode {

        public SystemHandle rootDependsOn;
        
        private struct RootSystem : ISystem {}

        [Output(name = "Out", allowMultiple = true)]
        public g::List<SystemHandle> output;

        public override string name => "START";
        public override bool isLocked => false;
        public override bool deletable => false;
        public override bool isCollapsable => false;
        public override UnityEngine.Color color => new UnityEngine.Color(0.06f, 0.3f, 0.14f);
        public override string style => "start-node";

        protected override void Process() {
            //UnityEngine.Debug.Log("ROOT NODE PLAY");
            this.runtimeHandle = this.runtimeSystemGroup.Add<RootSystem>(this.rootDependsOn);
        }

    }

}