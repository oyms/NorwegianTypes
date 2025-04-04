namespace Skaar.Contracts;

public interface IIdNumberFactory
{
    Fodselsnummer CreateRandomFodselsnummer(DateOnly date, Gender gender);
    DNummer CreateRandomDNummer(DateOnly date, Gender gender);
    DufNummer CreateRandomDufNummer(DateOnly date);
}