namespace ME.BECS {

    using INLINE = System.Runtime.CompilerServices.MethodImplAttribute;
    
    public interface ISystem {

    }

    public interface IAwake : ISystem {

        void OnAwake(ref SystemContext context);

    }

    public interface IDestroy : ISystem {

        void OnDestroy(ref SystemContext context);

    }

    public interface IUpdate : ISystem {

        void OnUpdate(ref SystemContext context);

    }

}