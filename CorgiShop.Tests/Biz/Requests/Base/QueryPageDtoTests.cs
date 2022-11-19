using CorgiShop.Application.Requests.Base;

namespace CorgiShop.Tests.Biz.Requests.Base;

public class QueryPageDtoTests
{
    private const int TOTAL_MAX_VALUE = 100;

    /*
     * Forward/Back
     */

    [Fact]
    public void CanGoForward_True()
    {
        //Arrange
        int currentLimit = 10;
        int currentOffset = 10;
        //Act
        var uut = GetUutAndRunLogic(currentLimit, currentOffset);
        //Assert
        Assert.True(uut.CanGoForward);
    }

    [Fact]
    public void CanGoForward_False()
    {
        //Arrange
        int currentLimit = 10;
        int currentOffset = 95;
        //Act
        var uut = GetUutAndRunLogic(currentLimit, currentOffset);
        //Assert
        Assert.False(uut.CanGoForward);
    }

    [Fact]
    public void CanGoBackward_True()
    {
        //Arrange
        int currentLimit = 10;
        int currentOffset = 10;
        //Act
        var uut = GetUutAndRunLogic(currentLimit, currentOffset);
        //Assert
        Assert.True(uut.CanGoBackward);
    }

    [Fact]
    public void CanGoBackward_False()
    {
        //Arrange
        int currentLimit = 10;
        int currentOffset = 0;
        //Act
        var uut = GetUutAndRunLogic(currentLimit, currentOffset);
        //Assert
        Assert.False(uut.CanGoBackward);
    }

    /*
     * Next/Previous offset calculations
     */

    [Fact]
    public void NextOffset_LowerClampToZero()
    {
        //Arrange
        int currentLimit = 500;
        int currentOffset = 10;
        //Act
        var uut = GetUutAndRunLogic(currentLimit, currentOffset);
        //Assert
        Assert.Equal(0, uut.NextOffset);
    }

    [Fact]
    public void NextOffset_UpperClampToMax()
    {
        //Arrange
        int currentLimit = 10;
        int currentOffset = 500;
        //Act
        var uut = GetUutAndRunLogic(currentLimit, currentOffset);
        //Assert
        Assert.Equal(90, uut.NextOffset);
    }

    [Fact]
    public void NextOffset_WithinBounds()
    {
        //Arrange
        int currentLimit = 10;
        int currentOffset = 10;
        //Act
        var uut = GetUutAndRunLogic(currentLimit, currentOffset);
        //Assert
        Assert.Equal(20, uut.NextOffset);
    }

    [Fact]
    public void PreviousOffset_LowerClampToZero()
    {
        //Arrange
        int currentLimit = 50;
        int currentOffset = 10;
        //Act
        var uut = GetUutAndRunLogic(currentLimit, currentOffset);
        //Assert
        Assert.Equal(0, uut.PreviousOffset);
    }

    [Fact]
    public void PreviousOffset_WithinBounds()
    {
        //Arrange
        int currentLimit = 5;
        int currentOffset = 20;
        //Act
        var uut = GetUutAndRunLogic(currentLimit, currentOffset);
        //Assert
        Assert.Equal(15, uut.PreviousOffset);
    }

    /*
     * Basic current value assignments
     */

    [Fact]
    public void CurrentLimit_Assigned()
    {
        //Arrange
        int currentLimit = 10;
        int currentOffset = 20;
        //Act
        var uut = GetUutAndRunLogic(currentLimit, currentOffset);
        //Assert
        Assert.Equal(10, uut.CurrentLimit);
    }


    [Fact]
    public void CurrentOffset_Assigned()
    {
        //Arrange
        int currentLimit = 10;
        int currentOffset = 20;
        //Act
        var uut = GetUutAndRunLogic(currentLimit, currentOffset);
        //Assert
        Assert.Equal(20, uut.CurrentOffset);
    }

    private QueryPageDto GetUutAndRunLogic(int currentLimit, int currentOffset) => QueryPageDto.FromCurrentPage(TOTAL_MAX_VALUE, currentLimit, currentOffset);

}
