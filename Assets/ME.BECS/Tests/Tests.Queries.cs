using NUnit.Framework;
using Unity.Jobs;
using ME.BECS.Jobs;

namespace ME.BECS.Tests {
    
    public struct Aspect1 : IAspect {
        
        public Ent ent { get; set; }
        
        [QueryWith]
        public AspectDataPtr<Test1Component> t1Value;
        [QueryWith]
        public AspectDataPtr<Test2Component> t2Value;

        public ref Test1Component t1 => ref this.t1Value.Get(this.ent);
        public ref Test2Component t2 => ref this.t2Value.Get(this.ent);

    }

    public struct Aspect2 : IAspect {
        
        public Ent ent { get; set; }

        [QueryWith]
        public AspectDataPtr<Test3Component> t1Value;
        [QueryWith]
        public AspectDataPtr<Test4Component> t2Value;

        public ref Test3Component t1 => ref this.t1Value.Get(this.ent);
        public ref Test4Component t2 => ref this.t2Value.Get(this.ent);

    }

    public unsafe class Tests_Queries {

        [Unity.Burst.BurstCompileAttribute]
        private struct JobComponents : IJobParallelForComponents<TestComponent> {

            public void Execute(ref TestComponent component) {

                component.data += 1;

            }

        }
        
        [Test]
        public void WithJobComponents() {
            
            {
                var world = World.Create();
                Ent ent1;
                Ent ent2;
                Ent ent3;
                {
                    var ent = Ent.New();
                    ent.Set(new TestComponent() {
                        data = 1,
                    });
                    ent1 = ent;
                }
                {
                    var ent = Ent.New();
                    ent.Set(new TestComponent() {
                        data = 1,
                    });
                    ent2 = ent;
                }
                {
                    var ent = Ent.New();
                    ent.Set(new TestComponent() {
                        data = 1,
                    });
                    ent.Set(new Test2Component() {
                        data = 1,
                    });
                    ent3 = ent;
                }

                var job = API.Query(world).Without<Test2Component>().ScheduleParallelFor<JobComponents, TestComponent>(new JobComponents());
                job.Complete();
                
                Assert.AreEqual(2, ent1.Read<TestComponent>().data);
                Assert.AreEqual(2, ent2.Read<TestComponent>().data);
                Assert.AreEqual(1, ent3.Read<TestComponent>().data);
                
                world.Dispose();

            }
            
        }

        [Test]
        public void WithAspect() {
            
            {
                var world = World.Create();
                var emptyEnt = Ent.New();
                
                var ent = Ent.New();
                ent.Set<Aspect1>();
                ent.Set<Aspect2>();

                var ent2 = Ent.New();
                ent2.Set<Aspect2>();

                var result = new System.Collections.Generic.List<Ent>();
                API.Query(world)
                   .WithAspect<Aspect1>()
                   .WithAspect<Aspect2>()
                   .ForEach((in CommandBufferJob commandBuffer) => {
                       result.Add(commandBuffer.ent);
                   });

                Assert.AreEqual(1, result.Count);
                Assert.IsFalse(result.Contains(emptyEnt));
                Assert.IsTrue(result.Contains(ent));
                Assert.IsFalse(result.Contains(ent2));
                
                world.Dispose();
            }
            {
                var world = World.Create();
                var emptyEnt = Ent.New();
                
                var ent = Ent.New();
                ent.Set(new Test1Component());
                ent.Set(new Test2Component());
                ent.Set(new Test3Component());
                ent.Set(new Test4Component());

                var ent2 = Ent.New();
                ent2.Set(new Test3Component());
                ent2.Set(new Test4Component());

                var result = new System.Collections.Generic.List<Ent>();
                API.Query(world)
                   .WithAspect<Aspect2>()
                   .ForEach((in CommandBufferJob commandBuffer) => {
                       result.Add(commandBuffer.ent);
                   });

                Assert.AreEqual(2, result.Count);
                Assert.IsFalse(result.Contains(emptyEnt));
                Assert.IsTrue(result.Contains(ent));
                Assert.IsTrue(result.Contains(ent2));
                
                world.Dispose();
            }
            
        }

        [Test]
        public void All() {
            
            {
                var world = World.Create();
                var emptyEnt = Ent.New();
                
                var ent = Ent.New();
                ent.Set(new TestComponent() {
                    data = 1,
                });

                var ent2 = Ent.New();
                ent2.Set(new Test2Component() {
                    data = 1,
                });

                var result = new System.Collections.Generic.List<Ent>();
                API.Query(world)
                   .ForEach((in CommandBufferJob commandBuffer) => {
                       result.Add(commandBuffer.ent);
                   });

                Assert.AreEqual(3, result.Count);
                Assert.IsTrue(result.Contains(emptyEnt));
                Assert.IsTrue(result.Contains(ent));
                Assert.IsTrue(result.Contains(ent2));
                
                world.Dispose();

            }
            
        }
        
        [Test]
        public void With() {

            {
                var world = World.Create();
                var emptyEnt = Ent.New();
                
                var ent = Ent.New();
                ent.Set(new TestComponent() {
                    data = 1,
                });

                var ent2 = Ent.New();
                ent2.Set(new Test2Component() {
                    data = 1,
                });

                var ent3 = Ent.New();
                ent3.Set(new Test2Component() {
                    data = 1,
                });

                var result = new System.Collections.Generic.List<Ent>();
                API.Query(world)
                   .With<TestComponent>()
                   .ForEach((in CommandBufferJob commandBuffer) => { result.Add(commandBuffer.ent); });

                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(ent, result[0]);
                
                world.Dispose();

            }
            {
                var world = World.Create();
                var emptyEnt = Ent.New();
                
                var ent = Ent.New();
                ent.Set(new TestComponent() {
                    data = 1,
                });

                var ent2 = Ent.New();
                ent2.Set(new TestComponent() {
                    data = 1,
                });

                var result = new System.Collections.Generic.List<Ent>();
                API.Query(world)
                   .With<TestComponent>()
                   .ForEach((in CommandBufferJob commandBuffer) => { result.Add(commandBuffer.ent); });

                Assert.AreEqual(2, result.Count);
                Assert.IsTrue(result.Contains(ent));
                Assert.IsTrue(result.Contains(ent2));
                
                world.Dispose();

            }
            {
                var world = World.Create();
                var emptyEnt = Ent.New();
                
                var ent = Ent.New();
                ent.Set(new TestComponent() {
                    data = 1,
                });

                var ent2 = Ent.New();
                ent2.Set(new TestComponent() {
                    data = 1,
                });

                var result = new System.Collections.Generic.List<Ent>();
                API.Query(world)
                   .With<Test2Component>()
                   .ForEach((in CommandBufferJob commandBuffer) => { result.Add(commandBuffer.ent); });

                Assert.AreEqual(0, result.Count);
                Assert.IsFalse(result.Contains(emptyEnt));
                Assert.IsFalse(result.Contains(ent));
                Assert.IsFalse(result.Contains(ent2));
                
                world.Dispose();

            }

        }

        [Test]
        public void Without() {

            {
                var world = World.Create();
                var emptyEnt = Ent.New();
                
                var ent = Ent.New();
                ent.Set(new TestComponent() {
                    data = 1,
                });

                var ent2 = Ent.New();
                ent2.Set(new Test2Component() {
                    data = 1,
                });

                var result = new System.Collections.Generic.List<Ent>();
                API.Query(world)
                   .With<TestComponent>()
                   .Without<Test3Component>()
                   .ForEach((in CommandBufferJob commandBuffer) => { result.Add(commandBuffer.ent); });

                Assert.AreEqual(1, result.Count);
                Assert.IsTrue(result.Contains(ent));
                
                world.Dispose();

            }
            {
                var world = World.Create();
                var emptyEnt = Ent.New();
                
                var ent = Ent.New();
                ent.Set(new TestComponent() {
                    data = 1,
                });

                var ent2 = Ent.New();
                ent2.Set(new Test2Component() {
                    data = 1,
                });

                var result = new System.Collections.Generic.List<Ent>();
                API.Query(world)
                   .Without<Test2Component>()
                   .ForEach((in CommandBufferJob commandBuffer) => { result.Add(commandBuffer.ent); });

                Assert.AreEqual(2, result.Count);
                Assert.IsTrue(result.Contains(emptyEnt));
                Assert.IsTrue(result.Contains(ent));
                
                world.Dispose();

            }
            {
                var world = World.Create();
                var emptyEnt = Ent.New();
                
                var ent = Ent.New();
                ent.Set(new TestComponent() {
                    data = 1,
                });

                var ent2 = Ent.New();
                ent2.Set(new TestComponent() {
                    data = 1,
                });

                var result = new System.Collections.Generic.List<Ent>();
                API.Query(world)
                   .Without<Test3Component>()
                   .ForEach((in CommandBufferJob commandBuffer) => { result.Add(commandBuffer.ent); });

                Assert.AreEqual(3, result.Count);
                Assert.IsTrue(result.Contains(emptyEnt));
                Assert.IsTrue(result.Contains(ent));
                Assert.IsTrue(result.Contains(ent2));
                
                world.Dispose();

            }

        }

        [Test]
        public void WithWithout() {

            {
                var world = World.Create();
                var emptyEnt = Ent.New();

                for (int i = 0; i < 2000; ++i) {

                    var ent = Ent.New();
                    ent.Set(new TestComponent() {
                        data = 1,
                    });

                }

                var ent2 = Ent.New();
                ent2.Set(new Test2Component() {
                    data = 1,
                });

                var result = new System.Collections.Generic.List<Ent>();
                API.Query(world)
                   .With<TestComponent>()
                   .Without<Test3Component>()
                   .ForEach((in CommandBufferJob commandBuffer) => { result.Add(commandBuffer.ent); });

                Assert.AreEqual(2000, result.Count);
                
                world.Dispose();

            }

        }

        [Test]
        public void WithAll() {

            {
                var world = World.Create();
                var emptyEnt = Ent.New();

                var ent = Ent.New();
                ent.Set(new TestComponent() {
                    data = 1,
                });
                
                var ent2 = Ent.New();
                ent2.Set(new Test2Component() {
                    data = 1,
                });

                var result = new System.Collections.Generic.List<Ent>();
                API.Query(world)
                   .WithAll<TestComponent, Test2Component>()
                   .ForEach((in CommandBufferJob commandBuffer) => { result.Add(commandBuffer.ent); });

                Assert.AreEqual(0, result.Count);
                
                world.Dispose();

            }

            {
                var world = World.Create();
                var emptyEnt = Ent.New();

                var ent = Ent.New();
                ent.Set(new TestComponent() {
                    data = 1,
                });
                ent.Set(new Test2Component() {
                    data = 1,
                });
                
                var ent2 = Ent.New();
                ent2.Set(new Test2Component() {
                    data = 1,
                });

                var result = new System.Collections.Generic.List<Ent>();
                API.Query(world)
                   .WithAll<TestComponent, Test2Component>()
                   .ForEach((in CommandBufferJob commandBuffer) => { result.Add(commandBuffer.ent); });

                Assert.AreEqual(1, result.Count);
                Assert.IsTrue(result.Contains(ent));
                
                world.Dispose();

            }

        }

        [Test]
        public void WithAny() {

            {
                var world = World.Create();
                var emptyEnt = Ent.New();

                var ent = Ent.New();
                ent.Set(new TestComponent() {
                    data = 1,
                });
                
                var ent2 = Ent.New();
                ent2.Set(new Test2Component() {
                    data = 1,
                });

                var ent3 = Ent.New();
                ent3.Set(new Test3Component() {
                    data = 1,
                });

                var result = new System.Collections.Generic.List<Ent>();
                API.Query(world)
                   .WithAny<TestComponent, Test2Component>()
                   .ForEach((in CommandBufferJob commandBuffer) => { result.Add(commandBuffer.ent); });

                Assert.AreEqual(2, result.Count);
                Assert.IsTrue(result.Contains(ent));
                Assert.IsTrue(result.Contains(ent2));
                
                world.Dispose();

            }

            {
                var world = World.Create();
                var emptyEnt = Ent.New();

                var ent = Ent.New();
                ent.Set(new TestComponent() {
                    data = 1,
                });
                ent.Set(new Test2Component() {
                    data = 1,
                });
                
                var ent2 = Ent.New();
                ent2.Set(new Test2Component() {
                    data = 1,
                });

                var result = new System.Collections.Generic.List<Ent>();
                API.Query(world)
                   .WithAny<TestComponent, Test2Component>()
                   .ForEach((in CommandBufferJob commandBuffer) => { result.Add(commandBuffer.ent); });

                Assert.AreEqual(2, result.Count);
                Assert.IsTrue(result.Contains(ent));
                Assert.IsTrue(result.Contains(ent2));
                
                world.Dispose();

            }

        }

        [Test]
        public void Step() {

            {
                var world = World.Create();
                var emptyEnt = Ent.New();

                for (int i = 0; i < 1000; ++i) {

                    var ent = Ent.New();
                    ent.Set(new TestComponent() {
                        data = 1,
                    });

                }

                var result = new System.Collections.Generic.List<Ent>();

                API.Query(world)
                   .With<TestComponent>()
                   .Step(5, 300)
                   .ForEach((in CommandBufferJob commandBuffer) => { result.Add(commandBuffer.ent); });

                Assert.AreEqual(300, result.Count);

                world.Tick(0f).Complete();

                API.Query(world)
                   .With<TestComponent>()
                   .Step(5, 300)
                   .ForEach((in CommandBufferJob commandBuffer) => { result.Add(commandBuffer.ent); });

                Assert.AreEqual(600, result.Count);

                world.Tick(0f).Complete();
                
                API.Query(world)
                   .With<TestComponent>()
                   .Step(5, 300)
                   .ForEach((in CommandBufferJob commandBuffer) => { result.Add(commandBuffer.ent); });

                Assert.AreEqual(900, result.Count);

                world.Tick(0f).Complete();
                
                API.Query(world)
                   .With<TestComponent>()
                   .Step(5, 300)
                   .ForEach((in CommandBufferJob commandBuffer) => { result.Add(commandBuffer.ent); });

                Assert.AreEqual(1000, result.Count);

                world.Tick(0f).Complete();

                API.Query(world)
                   .With<TestComponent>()
                   .Step(5, 300)
                   .ForEach((in CommandBufferJob commandBuffer) => { result.Add(commandBuffer.ent); });

                Assert.AreEqual(1300, result.Count);

                world.Dispose();

            }

        }

        [Test]
        public void AsJob() {

            {
                var world = World.Create();
                var emptyEnt = Ent.New();

                for (int i = 0; i < 1000; ++i) {

                    var ent = Ent.New();
                    ent.Set(new TestComponent() {
                        data = 1,
                    });

                }

                var result = new System.Collections.Generic.List<Ent>();

                var handle = API.Query(world)
                                .With<TestComponent>()
                                .AsJob()
                                .ForEach((in CommandBufferJob commandBuffer) => {
                                    result.Add(commandBuffer.ent);
                                });
                handle.Complete();

                Assert.AreEqual(1000, result.Count);
                
                world.Dispose();

            }

        }

        public struct ScheduleJob : IJobCommandBuffer {

            public void Execute(in CommandBufferJob buffer) {

                buffer.Get<TestComponent>().data = 2;

            }

        }

        public struct ScheduleJob2 : IJobCommandBuffer {

            public void Execute(in CommandBufferJob buffer) {

                buffer.Get<TestComponent>().data = 2;
                buffer.Remove<Test2Component>();

            }

        }

        public struct ScheduleParallelJob : IJobParallelForCommandBuffer {

            public void Execute(in CommandBufferJobParallel buffer) {

                buffer.Get<TestComponent>().data = 2;
            }

        }

        public struct ScheduleParallelJobBatch : IJobParallelForCommandBufferBatch {

            public void Execute(in CommandBufferJobBatch buffer) {

                for (uint i = buffer.fromIndex; i < buffer.toIndex; ++i) {
                    buffer.Get<TestComponent>(i).data = 2;
                }

            }

        }

        [Test]
        public void Schedule() {

            {
                var world = World.Create();
                var emptyEnt = Ent.New();

                var list = new System.Collections.Generic.List<Ent>();
                for (int i = 0; i < 1000; ++i) {

                    var ent = Ent.New();
                    ent.Set(new TestComponent() {
                        data = 1,
                    });
                    list.Add(ent);

                }

                var handle = API.Query(world)
                                .With<TestComponent>()
                                .Schedule<ScheduleJob>();
                handle.Complete();

                foreach (var item in list) {
                    Assert.AreEqual(2, item.Read<TestComponent>().data);
                }

                world.Dispose();

            }

            {
                var world = World.Create();
                var emptyEnt = Ent.New();

                var list = new System.Collections.Generic.List<Ent>();
                for (int i = 0; i < 1000; ++i) {

                    var ent = Ent.New();
                    ent.Set(new TestComponent() {
                        data = 1,
                    });
                    ent.Set(new Test2Component() {
                        data = 1,
                    });
                    list.Add(ent);

                }

                var handle = API.Query(world)
                                .With<TestComponent>()
                                .Schedule<ScheduleJob2>();
                handle.Complete();

                foreach (var item in list) {
                    Assert.AreEqual(2, item.Read<TestComponent>().data);
                    Assert.IsTrue(item.Has<Test2Component>() == false);
                }

                world.Dispose();

            }

        }

        [Test]
        public void ScheduleParallelFor() {

            {
                var world = World.Create();
                var emptyEnt = Ent.New();

                var list = new System.Collections.Generic.List<Ent>();
                for (int i = 0; i < 1000; ++i) {

                    var ent = Ent.New();
                    ent.Set(new TestComponent() {
                        data = 1,
                    });
                    list.Add(ent);

                }

                var handle = API.Query(world)
                                .With<TestComponent>()
                                .ScheduleParallelFor<ScheduleParallelJob>();
                handle.Complete();

                foreach (var item in list) {
                    Assert.AreEqual(2, item.Read<TestComponent>().data);
                }

                world.Dispose();

            }

            {
                var world = World.Create();
                var emptyEnt = Ent.New();

                var list = new System.Collections.Generic.List<Ent>();
                for (int i = 0; i < 1000; ++i) {

                    var ent = Ent.New();
                    ent.Set(new TestComponent() {
                        data = 1,
                    });
                    ent.Set(new Test2Component() {
                        data = 1,
                    });
                    list.Add(ent);

                }

                var handle = API.Query(world)
                                .With<TestComponent>()
                                .ScheduleParallelForBatch<ScheduleParallelJobBatch>();
                handle.Complete();

                foreach (var item in list) {
                    Assert.AreEqual(2, item.Read<TestComponent>().data);
                }

                world.Dispose();

            }

        }

        [Test]
        public void ParallelFor() {

            {
                var world = World.Create();
                var emptyEnt = Ent.New();

                for (int i = 0; i < 1000; ++i) {

                    var ent = Ent.New();
                    ent.Set(new TestComponent() {
                        data = 1,
                    });

                }

                var result = new System.Collections.Generic.List<Ent>();

                var handle = API.Query(world)
                                .With<TestComponent>()
                                .ParallelFor(300)
                                .ForEach((in CommandBufferJob commandBuffer) => {
                                    lock (result) result.Add(commandBuffer.ent);
                                });
                handle.Complete();

                Assert.AreEqual(1000, result.Count);

                world.Dispose();

            }

        }

        [Test]
        public void ParallelForBurst() {

            {
                var world = World.Create();
                var emptyEnt = Ent.New();

                for (int i = 0; i < 1000; ++i) {

                    var ent = Ent.New();
                    ent.Set(new TestComponent() {
                        data = 1,
                    });

                }

                var result = new System.Collections.Generic.List<Ent>();

                var handle = API.Query(world)
                                .With<TestComponent>()
                                .ParallelFor(300)
                                .WithBurst()
                                .ForEach((in CommandBufferJob commandBuffer) => {
                                    lock (result) result.Add(commandBuffer.ent);
                                });
                handle.Complete();

                Assert.AreEqual(1000, result.Count);

                world.Dispose();

            }

        }

        [Unity.Burst.BurstCompileAttribute]
        public static class Lambdas {

            [Unity.Burst.BurstCompileAttribute]
            public static void ParallelForBurst_Lambda(in CommandBufferJob buffer) {
                buffer.Get<TestComponent>().data = 2;
            }

        }

        [Test]
        public void ParallelForBurstStatic() {

            {
                var world = World.Create();
                var emptyEnt = Ent.New();

                var result = new System.Collections.Generic.List<Ent>();

                for (int i = 0; i < 1000; ++i) {

                    var ent = Ent.New();
                    ent.Set(new TestComponent() {
                        data = 1,
                    });
                    result.Add(ent);

                }

                var handle = API.Query(world)
                                .With<TestComponent>()
                                .ParallelFor(300)
                                .WithBurst()
                                .ForEach(Lambdas.ParallelForBurst_Lambda);
                handle.Complete();

                foreach (var ent in result) {
                    Assert.AreEqual(2, ent.Read<TestComponent>().data);
                }

                world.Dispose();

            }

        }

        [Test]
        public void Complex() {
            
            var world = World.Create();
            var ent = Ent.New();
            ent.Get<TestComponent>().data = 1;

            var count = 0;
            var handle = API.Query(world)
                            .With<TestComponent>()
                            .Without<Test3Component>()
                            .WithAny<TestComponent, Test2Component>()
                            .WithAspect<TestAspect>()
                            .Step(10, 2)
                            .ParallelFor(64)
                            .ForEach((in CommandBufferJob commandBuffer) => { ++count; });

            handle = API.Query(world, handle)
               .WithAll<TestComponent, Test2Component>()
               .WithAny<TestComponent, Test2Component>()
               .With<TestComponent>()
               .Without<Test3Component>()
               .WithAspect<TestAspect>()
               .ParallelFor(64)
               .ForEach((in CommandBufferJob commandBuffer) => {
                   
               });
            handle.Complete();

            Assert.AreEqual(1, count);

            world.Dispose();
            
        }

    }

}