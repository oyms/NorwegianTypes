namespace Skaar;

public enum OrganisasjonsnummerFormatting
{
    /// <summary>
    /// Without spaces
    /// </summary>
    /// <example>968253980</example>
    None = 0,
    /// <summary>
    /// With triples interspaced with non-breaking space
    /// </summary>
    /// <example>894 961 902</example>
    WithSpaces,
    /// <summary>
    /// <see href="https://org-id.guide/list/NO-BRC"/>
    /// </summary>
    /// <example>NO-BRC-972417920</example>
    OrgIdFormat
}