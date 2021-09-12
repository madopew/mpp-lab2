using System;
using System.Collections.Generic;
using System.Reflection;
using Core.FakerLib.Exceptions;
using Core.FakerLib.Implementations;
using NUnit.Framework;

namespace CoreTests
{
    public class FakerTests
    {
        [SetUp]
        public void Setup()
        {
            Assembly.LoadFile(@"C:\Users\Madi\RiderProjects\Faker\DoubleGenerator\bin\Debug\net5.0\DoubleGenerator.dll");
            Assembly.LoadFile(@"C:\Users\Madi\RiderProjects\Faker\StringGenerator\bin\Debug\net5.0\StringGenerator.dll");
        }
        
        [TestCase(5)]
        [TestCase(5d)]
        [TestCase("hello")]
        public void Create_SimpleTypes_DoesntThrow<T>(T _)
        {
            Assert.DoesNotThrow(() => Faker.Create<T>());
        }

        [Test]
        public void Create_DateTime_DoesntThrow()
        {
            Assert.DoesNotThrow(() => Faker.Create<DateTime>());
        }
        
        [Test]
        public void Create_List_DoesntThrow()
        {
            Assert.DoesNotThrow(() => Faker.Create<List<int>>());
        }

        [TestCase(5f)]
        [TestCase(5L)]
        [TestCase(5u)]
        public void Create_NoGenerator_ThrowsInsufficient<T>(T _)
        {
            Assert.Throws<InsufficientDependencyException>(() => Faker.Create<T>());
        }

        private class A
        {
            public int x;
            public B b;

            public A(int x, B b)
            {
                this.x = x;
                this.b = b;
            }
        }

        private class B
        {
            public double x { get; set; }

            public B()
            {
                
            }

            public B(float x)
            {
                
            }
        }

        [Test]
        public void Create_Class_ReturnsFake()
        {
            A obj = Faker.Create<A>();
            Assert.Multiple(() =>
            {
                Assert.NotZero(obj.x);
                Assert.NotZero(obj.b.x);
            });
        }

        private class C
        {
            public C(D d)
            {
            }
        }

        private class D
        {
            public D(E e)
            {
            }
        }

        private class E
        {
            public E(F f)
            {
            }
        }

        private class F
        {
            public F(D d)
            {
            }
        }

        [Test]
        public void Create_CyclicDependency_ThrowsException()
        {
            Assert.Throws<CyclicDependencyException>(() => Faker.Create<C>());
        }
    }
}