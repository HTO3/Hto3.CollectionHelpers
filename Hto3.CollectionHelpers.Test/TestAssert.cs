using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hto3.CollectionHelpers.Test
{
    internal static class TestAssert
    {
        public static TException Throws<TException>(Action action) where TException : Exception
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                if (ex is TException tex)
                    return tex;
                else
                    Assert.Fail($"Expected exception of type {typeof(TException).FullName} but caught {ex.GetType().FullName}.");
            }

            Assert.Fail($"Expected exception of type {typeof(TException).FullName} but no exception was thrown.");
            return null;
        }
    }
}
