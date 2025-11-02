using System;
using UnityEngine;

namespace Game
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class ReadOnly : PropertyAttribute { }
}