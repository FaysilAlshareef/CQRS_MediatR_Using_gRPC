using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Students.Command.Test.Fakers;
public class PrivateFaker<T> : Faker<T> where T : class
{
    public PrivateFaker<T> UsePrivateConstructor()
    {
        return (PrivateFaker<T>)base.CustomInstantiator(f => Activator.CreateInstance(typeof(T), nonPublic: true) as T ?? throw new InvalidOperationException());
    }
}
