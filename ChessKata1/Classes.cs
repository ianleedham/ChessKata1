namespace ChessKata1;


public class Board
{
    private readonly List<Piece> _pieces = new();

    public void Add(Piece piece)
    {
        _pieces.Add(piece);
    }

    public IEnumerable<string> GetValidMoves(Piece piece)
    {
        return piece.ValidMoves.Where(x => !IsBlocked(piece.Location, 
            new Location(Location.ToNumberFormat(x))));
    }

    bool IsBlocked(Location startLocation, Location endLocation)
    {
        // y = mx + c
        var dy = endLocation.Y - startLocation.Y;
        var dx = endLocation.X - startLocation.X;
        var m = dx != 0 ? dy / dx : 0;
        var c = startLocation.Y - m * startLocation.X;
        
        var blockingLocations = _pieces.Select(x => x.Location)
            .Where(x => !x.Equals(startLocation)).ToList();
        foreach (var location in blockingLocations)
        {
            if (dy == 0 && location.Y == startLocation.Y)
                return true;
            if (dx == 0 && location.X == startLocation.X)
                return true;
            if (m != 0 && location.Y == m * location.X + c)
                return true;
        }

        return false;
    }
}

public abstract class Piece
{
    public Location Location { get; }
    public abstract IEnumerable<string> ValidMoves { get; }
    protected Piece(Location location)
    {
        Location = location;
    }
    
    protected IEnumerable<Location> GetValidMoves(Func<Location, Location> getNextSquare)
    {
        List<Location> moves = new();
        const int boardUpperBound = 8;
        const int boardLowerBound = 1;

        bool IsWithinBoardBounds(Location location)
        {
            return location.X <= boardUpperBound 
                   && location.Y <= boardUpperBound 
                   && location.X >= boardLowerBound 
                   && location.Y >= boardLowerBound;
        }

        var nextSquare = getNextSquare(Location);
        while (IsWithinBoardBounds(nextSquare))
        { 
            moves.Add(nextSquare);
            nextSquare = getNextSquare(nextSquare);
        }
        return moves;
    }

}

public class Rook : Piece
{
    
    public Rook(Location location): base(location)
    {
    }

    public override IEnumerable<string> ValidMoves
    {
        get
        {
            List<Location> moves = new();
            moves.AddRange(GetValidMoves(Location.NextColumnFunc));
            moves.AddRange(GetValidMoves(Location.NextRowFunc));
            moves.AddRange(GetValidMoves(Location.PreviousColumnFunc));
            moves.AddRange(GetValidMoves(Location.PreviousRowFunc));
            return moves.Select(x => x.ToString());
        }
    }
}

public class Bishop: Piece
{

    public Bishop(Location location, Colour colour = Colour.Black): base(location)
    { }
    
    public override IEnumerable<string> ValidMoves
    {
        get
        {
            List<Location> moves = new();
            moves.AddRange(GetValidMoves(Location.NextDiagonalFunc));
            moves.AddRange(GetValidMoves(Location.PreviousDiagonalFunc));
            moves.AddRange(GetValidMoves(Location.NextInverseDiagonalFunc));
            moves.AddRange(GetValidMoves(Location.PreviousInverseDiagonalFunc));

            return moves.Select(x => x.ToString());
        }
    }
}

public class Location : IEquatable<Location>
{
    private static readonly string[] Letters = { "A", "B", "C", "D", "E", "F", "G", "H"};

    public Location(string xy)
    {
        X = int.Parse(xy[0].ToString());
        Y = int.Parse(xy[1].ToString());
    }

    public static string ToNumberFormat(string xy)
    {
        return (Array.IndexOf(Letters, xy[0].ToString()) + 1).ToString() + xy[1];
    }

    private Location(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X { get; }
    public int Y { get; }
    
    public readonly Func<Location, Location> NextDiagonalFunc = 
        location => new Location(location.X + 1, location.Y + 1);
    public readonly Func<Location, Location> NextInverseDiagonalFunc = 
        location => new Location(location.X - 1, location.Y + 1);
    public readonly Func<Location, Location> PreviousInverseDiagonalFunc = 
        location => new Location(location.X + 1, location.Y - 1);
    public readonly Func<Location, Location> PreviousDiagonalFunc = 
        location => new Location(location.X - 1, location.Y - 1);
    public readonly Func<Location, Location> NextColumnFunc = 
        location => new Location(location.X + 1, location.Y);
    public readonly Func<Location, Location> NextRowFunc = 
        location => new Location(location.X, location.Y + 1);
    public readonly Func<Location, Location> PreviousColumnFunc = 
        location => new Location(location.X - 1, location.Y);
    public readonly Func<Location, Location> PreviousRowFunc = 
        location => new Location(location.X, location.Y - 1);
    

    public override string ToString()
    {
        return Letters[X - 1] + Y;
    }

    public bool Equals(Location? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return X == other.X && Y == other.Y;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Location)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
}

public enum Colour
{
    Black,
    White
}