using Skaar.Contracts;

namespace Skaar.Utils;

public class IdNumberFactory : IIdNumberFactory
{
    public Fodselsnummer CreateRandomFodselsnummer(DateOnly date, Gender gender) => 
        Fodselsnummer.CreateNew(ValueFactory.CreateNew(NummerType.Fodselsnummer, date, gender));

    public DNummer CreateRandomDNummer(DateOnly date, Gender gender)=> 
        DNummer.CreateNew(ValueFactory.CreateNew(NummerType.DNummer, date, gender));

    public DufNummer CreateRandomDufNummer(DateOnly date)=> 
        DufNummer.CreateNew(ValueFactory.CreateNew(NummerType.DufNummer, date, Gender.Undefined));
}