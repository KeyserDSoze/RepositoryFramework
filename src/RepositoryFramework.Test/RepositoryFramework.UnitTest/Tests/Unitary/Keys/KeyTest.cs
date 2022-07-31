using RepositoryFramework.UnitTest.Unitary.Keys.Models;
using System;
using Xunit;

namespace RepositoryFramework.UnitTest.Unitary.Keys
{
    public partial class KeyTest
    {
        [Fact]
        public void KeyAsStringFromClass()
        {
            Xilophone xilophone = new("dasdasd");
            var key = xilophone.AsString();
            Assert.Equal("dasdasd", key);
            key = xilophone.AsStringWithName();
            Assert.Equal("Xilophone_dasdasd", key);
            key = xilophone.AsStringWithNameAndPrefix("customPrefix");
            Assert.Equal("customPrefix_Xilophone_dasdasd", key);
        }
        [Fact]
        public void KeyAsStringFromRecord()
        {
            Xai xai = new("a", "b");
            var key = xai.AsString();
            Assert.Equal("a_b", key);
            key = xai.AsStringWithName();
            Assert.Equal("Xai_a_b", key);
            key = xai.AsStringWithNameAndPrefix("customPrefix");
            Assert.Equal("customPrefix_Xai_a_b", key);
        }
        [Fact]
        public void KeyAsStringFromString()
        {
            string a = "dasdasd";
            var key = a.AsString();
            Assert.Equal("dasdasd", key);
            key = a.AsStringWithName();
            Assert.Equal("String_dasdasd", key);
            key = a.AsStringWithNameAndPrefix("customPrefix");
            Assert.Equal("customPrefix_String_dasdasd", key);
        }
        [Fact]
        public void KeyAsStringFromRange()
        {
            Range a = new(1, 2);
            var key = a.AsString();
            Assert.Equal("1_False_2_False", key);
            key = a.AsStringWithName();
            Assert.Equal("Range_1_False_2_False", key);
            key = a.AsStringWithNameAndPrefix("customPrefix");
            Assert.Equal("customPrefix_Range_1_False_2_False", key);
        }
        [Fact]
        public void KeyAsStringFromIndex()
        {
            Index a = new(1, true);
            var key = a.AsString();
            Assert.Equal("1_True", key);
            key = a.AsStringWithName();
            Assert.Equal("Index_1_True", key);
            key = a.AsStringWithNameAndPrefix("customPrefix");
            Assert.Equal("customPrefix_Index_1_True", key);
        }
    }
}
