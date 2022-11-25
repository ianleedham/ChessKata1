using System.Linq.Expressions;

namespace ChessKata1;

[TestClass]
public class UnitTest1
{

    // Place a single white bishop anywhere on a chess board
    // Determine the set of squares to which the bishop can move (e.g. bishop, b6 -> {a5,c7,d8,a7,c5,d4,e3,f2,g1})
    // board size is from A1 to H8 
    // Add a white castle to the board and determine the squares to which it can move.
    // Include a test where the bishop obstructs the castle’s movement
    // Now add a black bishop to the board, and determine the squares to which the white castle can move.
    // Include a test where the castle is able to take the black bishop
    // Now add a white queen and determine the squares to which she can move
    // Do the same for a white knight
    //     Now add a white pawn and allow it to move forward one square (for now, ignore the two-square advance for a pawn’s first move). Include a test to ensure it is blocked by a black piece immediately in front of it
    //     Allow the white pawn to capture a black piece by moving diagonally forward one square
    // Now allow a pawn to move either one or two spaces forward for its first move
    //     Now allow a pawn to capture a piece en passant
    //     Add a king and the ability to castle

    [TestMethod]
    public void AddABlackBishopToTheBoard()
    {
        var board = new Board();
        var bishop = new Bishop(new Location(Location.ToNumberFormat("C4")), Colour.Black);
        board.Add(bishop);
        
        Assert.AreEqual("C4", bishop.Location.ToString());
    }
    
    [TestMethod]
    public void PlaceASingleWhiteBishopAtC4()
    {
        var board = new Board();
        var bishop = new Bishop(new Location(Location.ToNumberFormat("C4")));
        board.Add(bishop);
        
        Assert.AreEqual("C4", bishop.Location.ToString());
    }

    [TestMethod]
    [DataRow("C4", "D5",
        DisplayName = "Bishop at C4 should be able to move to D5")]
    [DataRow("C4", "G8",
        DisplayName = "Bishop at C4 should be able to move to G8")]
    [DataRow("C5", "D6",
        DisplayName = "Bishop at C5 should be able to move to D6")]
    [DataRow("C5", "B4",
        DisplayName = "Bishop at C5 should be able to move to B4")]
    [DataRow("C5", "A3",
        DisplayName = "Bishop at C5 should be able to move to A3")]
    [DataRow("C5", "F8",
        DisplayName = "Bishop at C5 should be able to move to F8")]
    [DataRow("C5", "A7",
        DisplayName = "Bishop at C5 should be able to move to A7")]
    [DataRow("C5", "G1",
        DisplayName = "Bishop at C5 should be able to move to G1")]
    [DataRow("A1", "H8",
        DisplayName = "Bishop at A1 should be able to move to H8")]
    [DataRow("H1", "A8",
        DisplayName = "Bishop at H1 should be able to move to A8")]
    public void GivenABishopAtLocation_GetValidMoves_ContainsValidMove(string location, string validMove)
    {
        var board = new Board();
        var bishop = new Bishop(new Location(Location.ToNumberFormat(location)));
        board.Add(bishop);
        
        IEnumerable<string> validMoves = board.GetValidMoves(bishop);
        
        Assert.IsTrue(validMoves.Contains(validMove));
        
    }
    
    [TestMethod]
    public void PlaceASingleWhiteRookOnTheBoardAtC4()
    {
        var board = new Board();
        var rook = new Rook(new Location(Location.ToNumberFormat("C4")));
        board.Add(rook);
        
        Assert.AreEqual("C4", rook.Location.ToString());
    }
    
    [TestMethod]
    [DataRow("C3", "D3",
        DisplayName = "Rook at C3 should be able to move to D3")]
    [DataRow("C3", "H3",
        DisplayName = "Rook at C3 should be able to move to H3")]
    [DataRow("C3", "C8",
        DisplayName = "Rook at C3 should be able to move to C8")]
    [DataRow("C1", "C8",
        DisplayName = "Rook at C1 should be able to move to C8")]
    [DataRow("A1", "H1",
        DisplayName = "Rook at A1 should be able to move to H1")]
    [DataRow("A1", "A8",
        DisplayName = "Rook at A1 should be able to move to A8")]
    [DataRow("H1", "A1",
        DisplayName = "Rook at H1 should be able to move to A1")]
    [DataRow("A8", "A1",
        DisplayName = "Rook at A8 should be able to move to A1")]
    public void GivenARookAtLocation_GetValidMoves_ContainsValidMove(string location, string validMove)
    {
        var board = new Board();
        var rook = new Rook(new Location(Location.ToNumberFormat(location)));
        board.Add(rook);
        IEnumerable<string> validMoves = board.GetValidMoves(rook);
        
        Assert.IsTrue(validMoves.Contains(validMove));
    }
    
    [TestMethod]
    [DataRow("A1", "B1", "C1", 
        DisplayName = "Rook at A1 should be blocked by bishop at B1 so can't reach C1")]
    [DataRow("A2", "B2", "C2",
        DisplayName = "Rook at A2 should be blocked by bishop at B2 so can't reach C2")]
    [DataRow("C1", "B1", "A1",
        DisplayName = "Rook at C1 should be blocked by bishop at B1 so can't reach A1")]
    [DataRow("C1", "C2", "C6",
        DisplayName = "Rook at C1 should be blocked by bishop at C2 so can't reach C6")]
    public void GivenAWhiteRookAtPosition1_AndAWhiteBishopAtPosition2_Position3IsAInvalidPositionForTheRook(
        string rookLocation, string bishopLocation, string invalidMove)
    {
        var board = new Board();
        var rook = new Rook(new Location(Location.ToNumberFormat(rookLocation)));
        var bishop = new Bishop(new Location(Location.ToNumberFormat(bishopLocation)));
        board.Add(rook);
        board.Add(bishop);
        
        IEnumerable<string> validMoves = board.GetValidMoves(rook);
        
        Assert.IsTrue(!validMoves.Contains(invalidMove));
    }
    
    [TestMethod]
    [DataRow("C1", "D2", "E3",
        DisplayName = "Bishop at C1 should be blocked by Rook at D2 so can't reach E3")]
    [DataRow("C3", "B2", "A1",
        DisplayName = "Bishop at C3 should be blocked by Rook at B2 so can't reach A1")]
    [DataRow("A8", "D5", "H1",
        DisplayName = "Bishop at A8 should be blocked by Rook at B2 so can't reach A1")]
    [DataRow("H1", "D5", "A8",
        DisplayName = "Bishop at H1 should be blocked by Rook at B2 so can't reach A8")]
    public void GivenAWhitBishopAtPosition1_AndAWhiteRookAtPosition2_Position3IsAInvalidPositionForTheBishop(
        string bishopLocation, string rookLocation , string invalidMove)
    {
        var board = new Board();
        var rook = new Rook(new Location(Location.ToNumberFormat(rookLocation)));
        var bishop = new Bishop(new Location(Location.ToNumberFormat(bishopLocation)));
        board.Add(rook);
        board.Add(bishop);
        
        IEnumerable<string> validMoves = board.GetValidMoves(bishop);
        
        Assert.IsTrue(!validMoves.Contains(invalidMove));
    }
}
