using NUnit.Framework;
using ME.BECS.Jobs;

namespace ME.BECS.Tests {

    public class Tests_Queries_Static {

        [Test]
        public void Create() {

            var world = World.Create();

            var dt = 0.01f;
            {
                var systemGroup = SystemGroup.Create();
                systemGroup.Add<TestSystem1>();
                world.AssignRootSystemGroup(systemGroup);
                {
                    var ent = Ent.New(world);
                    ent.Set(new TestComponent() { data = 1 });
                }
                {
                    var ent = Ent.New(world);
                    ent.Set(new Test2Component() { data = 3 });
                }

                world.Awake();
                
                {
                    var ent = Ent.New(world);
                    ent.Set(new TestComponent() { data = 2 });
                }
                {
                    var ent = Ent.New(world);
                    ent.Set(new Test2Component() { data = 4 });
                }
                // Tick world
                world.Tick(dt).Complete();
            }

            world.Dispose();

        }

        [Test]
        public void CreateArchetypeAfterQuery() {

            var world = World.Create();

            var dt = 0.01f;
            {
                var systemGroup = SystemGroup.Create();
                systemGroup.Add<TestSystemDefer1>();
                world.AssignRootSystemGroup(systemGroup);
                world.Awake();
                {
                    var ent = Ent.New(world);
                    ent.Set(new TestComponent() { data = 2 });
                }
                {
                    var ent = Ent.New(world);
                    ent.Set(new Test2Component() { data = 4 });
                }
                // Tick world
                world.Tick(dt).Complete();
            }

            world.Dispose();

        }

        [Test]
        public void QueryId() {

            var world = World.Create();

            var dt = 0.01f;
            {
                var systemGroup = SystemGroup.Create();
                systemGroup.Add<TestSystem2>();
                world.AssignRootSystemGroup(systemGroup);
                
                world.Awake();
                // Tick world
                world.Tick(dt).Complete();
            }

            world.Dispose();

        }

        [Test]
        public void QueryEquals() {

            var world = World.Create();

            var dt = 0.01f;
            {
                var systemGroup = SystemGroup.Create();
                systemGroup.Add<TestSystem3>();
                world.AssignRootSystemGroup(systemGroup);
                
                world.Awake();
                // Tick world
                world.Tick(dt).Complete();
            }

            world.Dispose();

        }

        [Test]
        public void Schedule() {

            var world = World.Create();

            var dt = 0.01f;
            {
                var systemGroup = SystemGroup.Create();
                systemGroup.Add<TestSystem4>();
                world.AssignRootSystemGroup(systemGroup);
                {
                    var ent = Ent.New(world);
                    ent.Set(new TestComponent() { data = 2 });
                }
                {
                    var ent = Ent.New(world);
                    ent.Set(new Test2Component() { data = 4 });
                }
                world.Awake();
                // Tick world
                world.Tick(dt).Complete();
            }

            world.Dispose();

        }

        public struct TestSystemDefer1 : IAwake, IUpdate, IDestroy {

            public Query query;
            private int sum;
            
            public void OnAwake(ref SystemContext context) {
                this.query = Query.With<TestComponent>(in context).Build();
                Assert.AreEqual(0, this.sum);
            }

            public void OnUpdate(ref SystemContext context) {
                
                Assert.AreEqual(0, this.sum);
                foreach (var ent in this.query.ForEach(context)) {
                    this.sum += ent.Read<TestComponent>().data;
                }
                Assert.AreEqual(2, this.sum);
                
            }

            public void OnDestroy(ref SystemContext context) {
                Assert.AreEqual(2, this.sum);
            }

        }

        public struct TestSystem1 : IAwake, IUpdate, IDestroy {

            public Query query;
            private int sum;
            
            public void OnAwake(ref SystemContext context) {
                this.query = Query.With<TestComponent>(in context).Build();
                Assert.AreEqual(0, this.sum);
            }

            public void OnUpdate(ref SystemContext context) {
                
                Assert.AreEqual(0, this.sum);
                foreach (var ent in this.query.ForEach(context)) {
                    this.sum += ent.Read<TestComponent>().data;
                }
                Assert.AreEqual(3, this.sum);
                
            }

            public void OnDestroy(ref SystemContext context) {
                Assert.AreEqual(3, this.sum);
            }

        }

        public struct TestSystem2 : IAwake {

            public void OnAwake(ref SystemContext context) {
                {
                    var query = Query.With<TestComponent>(in context).Build();
                    Assert.AreEqual(1, query.id);
                }
                {
                    var query = Query.With<Test2Component>(in context).Build();
                    Assert.AreEqual(2, query.id);
                }
                {
                    var query = Query.With<TestComponent>(in context).Build();
                    Assert.AreEqual(1, query.id);
                }
            }

        }

        public struct TestSystem3 : IAwake {

            public void OnAwake(ref SystemContext context) {
                {
                    var query = Query.With<TestComponent>(in context).WithAll<Test2Component, Test3Component>().WithAny<Test4Component, Test5Component>().Build();
                    Assert.AreEqual(1, query.id);
                }
                {
                    var query = Query.With<Test2Component>(in context).Build();
                    Assert.AreEqual(2, query.id);
                }
                {
                    var query = Query.With<TestComponent>(in context).WithAll<Test2Component, Test3Component>().WithAny<Test4Component, Test5Component>().Build();
                    Assert.AreEqual(1, query.id);
                }
            }

        }

        public struct TestSystem4 : IAwake, IUpdate {

            [Unity.Burst.BurstCompileAttribute]
            public struct Job : IJobCommandBuffer {

                public Unity.Collections.NativeReference<int> sum;

                public void Execute(in CommandBufferJob commandBuffer) {
                    this.sum.Value += commandBuffer.ent.Read<TestComponent>().data;
                }

            }

            [Unity.Burst.BurstCompileAttribute]
            public struct JobComponents : IJobComponents<TestComponent> {

                public Unity.Collections.NativeReference<int> sum;

                public void Execute(ref TestComponent comp) {
                    this.sum.Value += comp.data;
                }

            }

            public Query query;
            
            public void OnAwake(ref SystemContext context) {
                this.query = Query.With<TestComponent>(in context).Build();
            }

            public void OnUpdate(ref SystemContext context) {

                {
                    var job = new Job() {
                        sum = new Unity.Collections.NativeReference<int>(0, Unity.Collections.Allocator.TempJob),
                    };
                    var handle = this.query.Schedule(job, context);
                    context.SetDependency(handle);
                    handle.Complete();
                    var val = job.sum.Value;
                    job.sum.Dispose();
                    Assert.AreEqual(2, val);
                }
                {
                    var job = new Job() {
                        sum = new Unity.Collections.NativeReference<int>(0, Unity.Collections.Allocator.TempJob),
                    };
                    var handle = this.query.Schedule(job, context);
                    context.SetDependency(handle);
                    handle.Complete();
                    var val = job.sum.Value;
                    job.sum.Dispose();
                    Assert.AreEqual(2, val);
                }
                {
                    var job = new JobComponents() {
                        sum = new Unity.Collections.NativeReference<int>(0, Unity.Collections.Allocator.TempJob),
                    };
                    var handle = this.query.Schedule<JobComponents, TestComponent>(job, context);
                    context.SetDependency(handle);
                    handle.Complete();
                    var val = job.sum.Value;
                    job.sum.Dispose();
                    Assert.AreEqual(2, val);
                }

            }

        }

    }

}