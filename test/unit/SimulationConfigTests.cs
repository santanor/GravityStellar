using GdUnit4;
using static GdUnit4.Assertions;
using System;
using GravityStellar.Stubs;

namespace GravityStellar.Tests.Unit;

[TestSuite]
public class SimulationConfigTests
{
    private SimulationConfig _config = null!;

    [BeforeTest]
    public void Setup()
    {
        _config = new SimulationConfig();
    }

    // --- Default Values ---

    [TestCase]
    public void DefaultGravitationalConstant_IsPositiveGameScale()
    {
        AssertThat(_config.GravitationalConstant).IsEqual(6.674f);
    }

    [TestCase]
    public void DefaultSofteningParameter_IsPositive()
    {
        AssertThat(_config.SofteningParameter).IsEqual(10.0f);
    }

    [TestCase]
    public void DefaultFixedTimestep_IsSixtiethOfSecond()
    {
        AssertThat(_config.FixedTimestep).IsEqual(1.0f / 60.0f);
    }

    [TestCase]
    public void DefaultMaxSimulationSpeed_IsThree()
    {
        AssertThat(_config.MaxSimulationSpeed).IsEqual(3.0f);
    }

    // --- GravitationalConstant Validation ---

    [TestCase]
    public void GravitationalConstant_SetPositive_Succeeds()
    {
        _config.GravitationalConstant = 100f;
        AssertThat(_config.GravitationalConstant).IsEqual(100f);
    }

    [TestCase]
    public void GravitationalConstant_SetZero_ThrowsException()
    {
        AssertThrown(() =>
        {
            _config.GravitationalConstant = 0f;
        }).IsInstanceOf<ArgumentOutOfRangeException>();
    }

    [TestCase]
    public void GravitationalConstant_SetNegative_ThrowsException()
    {
        AssertThrown(() =>
        {
            _config.GravitationalConstant = -1f;
        }).IsInstanceOf<ArgumentOutOfRangeException>();
    }

    // --- SofteningParameter Validation ---

    [TestCase]
    public void SofteningParameter_SetZero_Succeeds()
    {
        _config.SofteningParameter = 0f;
        AssertThat(_config.SofteningParameter).IsEqual(0f);
    }

    [TestCase]
    public void SofteningParameter_SetPositive_Succeeds()
    {
        _config.SofteningParameter = 50f;
        AssertThat(_config.SofteningParameter).IsEqual(50f);
    }

    [TestCase]
    public void SofteningParameter_SetNegative_ThrowsException()
    {
        AssertThrown(() =>
        {
            _config.SofteningParameter = -1f;
        }).IsInstanceOf<ArgumentOutOfRangeException>();
    }

    // --- FixedTimestep Validation ---

    [TestCase]
    public void FixedTimestep_SetValid_Succeeds()
    {
        _config.FixedTimestep = 1.0f / 30.0f;
        AssertThat(_config.FixedTimestep).IsEqual(1.0f / 30.0f);
    }

    [TestCase]
    public void FixedTimestep_SetZero_ThrowsException()
    {
        AssertThrown(() =>
        {
            _config.FixedTimestep = 0f;
        }).IsInstanceOf<ArgumentOutOfRangeException>();
    }

    [TestCase]
    public void FixedTimestep_SetNegative_ThrowsException()
    {
        AssertThrown(() =>
        {
            _config.FixedTimestep = -0.016f;
        }).IsInstanceOf<ArgumentOutOfRangeException>();
    }

    [TestCase]
    public void FixedTimestep_SetTooLarge_ThrowsException()
    {
        AssertThrown(() =>
        {
            _config.FixedTimestep = 2.0f;
        }).IsInstanceOf<ArgumentOutOfRangeException>();
    }

    // --- MaxSimulationSpeed Validation ---

    [TestCase]
    public void MaxSimulationSpeed_SetPositive_Succeeds()
    {
        _config.MaxSimulationSpeed = 10f;
        AssertThat(_config.MaxSimulationSpeed).IsEqual(10f);
    }

    [TestCase]
    public void MaxSimulationSpeed_SetZero_ThrowsException()
    {
        AssertThrown(() =>
        {
            _config.MaxSimulationSpeed = 0f;
        }).IsInstanceOf<ArgumentOutOfRangeException>();
    }

    [TestCase]
    public void MaxSimulationSpeed_SetNegative_ThrowsException()
    {
        AssertThrown(() =>
        {
            _config.MaxSimulationSpeed = -5f;
        }).IsInstanceOf<ArgumentOutOfRangeException>();
    }

    // --- Boundary Edge Cases ---

    [TestCase]
    public void GravitationalConstant_VerySmallPositive_Succeeds()
    {
        _config.GravitationalConstant = float.Epsilon;
        AssertThat(_config.GravitationalConstant).IsEqual(float.Epsilon);
    }

    [TestCase]
    public void GravitationalConstant_VeryLarge_Succeeds()
    {
        _config.GravitationalConstant = 1e6f;
        AssertThat(_config.GravitationalConstant).IsEqual(1e6f);
    }

    [TestCase]
    public void FixedTimestep_ExactlyOne_Succeeds()
    {
        _config.FixedTimestep = 1.0f;
        AssertThat(_config.FixedTimestep).IsEqual(1.0f);
    }
}
