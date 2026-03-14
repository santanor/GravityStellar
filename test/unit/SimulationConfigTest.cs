using GdUnit4;

namespace GravityStellar.Tests.Physics;

// NOTE: SimulationConfig extends Godot.Node. These tests require the Godot
// runtime (provided by the GdUnit4 test runner). Each test creates and frees
// the node explicitly to avoid memory leaks.

[TestSuite]
public class SimulationConfigTest
{
    [TestCase]
    public void DefaultGravitationalConstant_IsSensibleValue()
    {
        var config = new SimulationConfig();

        Assertions.AssertThat(config.GravitationalConstant).IsEqual(6.674f);

        config.Free();
    }

    [TestCase]
    public void DefaultSofteningParameter_IsPositive()
    {
        var config = new SimulationConfig();

        Assertions.AssertThat(config.SofteningParameter).IsEqual(10.0f);
        Assertions.AssertThat(config.SofteningParameter).IsGreater(0f);

        config.Free();
    }

    [TestCase]
    public void DefaultFixedTimestep_Is60Fps()
    {
        var config = new SimulationConfig();

        float expected = 1.0f / 60.0f;
        Assertions.AssertThat(config.FixedTimestep).IsEqual(expected);

        config.Free();
    }

    [TestCase]
    public void DefaultMaxSimulationSpeed_IsPositive()
    {
        var config = new SimulationConfig();

        Assertions.AssertThat(config.MaxSimulationSpeed).IsEqual(3.0f);
        Assertions.AssertThat(config.MaxSimulationSpeed).IsGreater(0f);

        config.Free();
    }

    [TestCase]
    public void Properties_AreSettable()
    {
        var config = new SimulationConfig();

        config.GravitationalConstant = 100f;
        config.SofteningParameter = 5f;
        config.FixedTimestep = 1.0f / 30.0f;
        config.MaxSimulationSpeed = 10f;

        Assertions.AssertThat(config.GravitationalConstant).IsEqual(100f);
        Assertions.AssertThat(config.SofteningParameter).IsEqual(5f);
        Assertions.AssertThat(config.FixedTimestep).IsEqual(1.0f / 30.0f);
        Assertions.AssertThat(config.MaxSimulationSpeed).IsEqual(10f);

        config.Free();
    }
}
