using StarKnightsLibrary.GameFlow;
using StarKnightsLibrary.Triggers;
using StarKnightsLibrary.UnitTests.TestDoubles.Fakes;
using StarKnightsLibrary.UnitTests.TestDoubles.Spies;
using Xunit;

namespace StarKnightsTests
{
    public class TriggerTests
    {
        private (ITrigger, EffectSpy<T>, Condition<T>) GetTrigger<T>(Condition<T> condition)
            where T : ConditionResult, new()
        {            
            var effect = new EffectSpy<T>();
            var trigger = new Trigger<T>(condition, effect.Act);
            return (trigger, effect, condition);
        }

        private (ITrigger, EffectSpy<ConditionResult>, Condition<ConditionResult>) GetTrigger(bool conditionPassed = true)
        {
            return GetTrigger(enviroment => new ConditionResult
            {
                ConditionPassed = conditionPassed
            });
        }

        [Fact]
        public void CondtionMatchedTest()
        {
            //Arrange
            (var trigger, var effect, _) = GetTrigger();

            //Act
            trigger.Update(null);

            //Assert
            Assert.True(effect.ActWasCalled);
        }

        [Fact]
        public void ConditionNotMatchedTest()
        {
            //Arrange
            (var trigger, var effect, _) = GetTrigger(false);

            //Act
            trigger.Update(null);

            //Assert
            Assert.False(effect.ActWasCalled);
        }

        [Fact]
        public void GenericTriggerTest()
        {
            //Arrange
            var data = 51;
            (var trigger, var effect, _) = GetTrigger<ConditionResult<int>>(x => new ConditionResult<int>
            {
                ConditionPassed = true,
                Data = data
            });

            //Act
            trigger.Update(null);

            //
            Assert.Equal(data, effect.Result.Data);
            Assert.True(effect.ActWasCalled);
        }

        [Fact]
        public void TriggerManagerUpdateOneTest()
        {
            //Arrange
            var manager = new TriggerManager();
            (_, var effect, var condition) = GetTrigger();
            var space = new Space(0, 0);
            var scene = new FakeSpaceScene(space);
            manager.AddTrigger(condition, effect.Act, false);

            //Act
            manager.Update(scene);

            //Assert
            Assert.True(effect.ActWasCalled);
        }

        [Fact]
        public void TriggerManagerUpdateManyTest()
        {
            var manager = new TriggerManager();
            (_, var effect1, var condition1) = GetTrigger();
            (_, var effect2, var condition2) = GetTrigger();
            var space = new Space(0, 0);
            var scene = new FakeSpaceScene(space);
            manager.AddTrigger(condition1, effect1.Act, false);
            manager.AddTrigger(condition2, effect2.Act, false);

            //Act
            manager.Update(scene);

            //Assert
            Assert.True(effect1.ActWasCalled);
            Assert.True(effect2.ActWasCalled);
        }         

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void TriggerManagerRecurringTest(int updates)
        {
            //Arrange
            var manager = new TriggerManager();
            (_, var effect, var condition) = GetTrigger();
            var space = new Space(0, 0);
            var scene = new FakeSpaceScene(space);
            manager.AddTrigger(condition, effect.Act, true);

            //Act
            for (int i = 0; i < updates; i++)
            {
                manager.Update(scene);
            }            

            //Assert
            Assert.Equal(updates, effect.TimesActCalled);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void TriggerManagerNotRecurringTest(int updates)
        {
            //Arrange
            var manager = new TriggerManager();
            (_, var effect, var condition) = GetTrigger();
            var space = new Space(0, 0);
            var scene = new FakeSpaceScene(space);
            manager.AddTrigger(condition, effect.Act, false);

            //Act
            for (int i = 0; i < updates; i++)
            {
                manager.Update(scene);
            }

            //Assert
            Assert.Equal(1, effect.TimesActCalled);
        }

        [Fact]
        public void TriggerManagerRemoveTest()
        {
            //Arrange
            var manager = new TriggerManager();
            (_, var effect, var condition) = GetTrigger();

            //Act
            uint key = manager.AddTrigger(condition, effect.Act, true);
            manager.RemoveTrigger(key);
            manager.Update(null);

            //Assert
            Assert.False(effect.ActWasCalled);
        }
    }
}
