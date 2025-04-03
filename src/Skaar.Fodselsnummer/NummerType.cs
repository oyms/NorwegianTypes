namespace Skaar;

public enum NummerType
{
    /// <summary>
    /// Not a valid known number type.
    /// </summary>
    Invalid = 0,
    /// <summary>
    /// Fødselsnummer
    /// <seealso href="https://www.skatteetaten.no/en/person/national-registry/identitetsnummer/fodselsnummer/"/>
    /// </summary>
    Fodselsnummer = 1,
    /// <summary>
    /// D-nummer
    /// <seealso href="https://www.skatteetaten.no/en/person/national-registry/identitetsnummer/d-nummer/"/>
    /// </summary>
    DNummer = 2
}