namespace Skaar;

public enum KontonummerFormatting
{
    /// <summary>
    /// No spacing
    /// <example>00000000000</example>
    /// </summary>
    /// <seealso cref="KontonummerFormats.General"/>
    None = 0,
    /// <summary>
    /// With (non-breaking) spaces
    /// <example>0000 00 00000</example>
    /// </summary>
    /// <seealso cref="KontonummerFormats.NonBreakingSpaces"/>
    Spaces,
    /// <summary>
    /// With periods
    /// <example>0000.00.00000</example>
    /// </summary>
    /// <seealso cref="KontonummerFormats.Periods"/>
    Periods,
    /// <summary>
    /// IBAN Electronic format
    /// <example>NO0000000000000</example>
    /// </summary>
    /// <seealso cref="KontonummerFormats.IbanScreen"/>
    IbanScreen,
    /// <summary>
    /// IBAN print format
    /// <example>NO00 0000 0000 000</example>
    /// </summary>
    /// <seealso cref="KontonummerFormats.IbanPrint"/>
    IbanPrint
}