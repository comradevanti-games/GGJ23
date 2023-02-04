using System.Collections.Immutable;

namespace TeamShrimp.GGJ23
{
    public record Game(
        string Ip,
        ushort Port,
        IImmutableDictionary<Team, string> PlayerNamesByTeam,
        int MapSize,
        int MapSeed);
}