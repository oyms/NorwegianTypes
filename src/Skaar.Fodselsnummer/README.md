Fødselsnummer 🇳🇴
===

This package is an implementation of the
[Norwegian identity number](https://en.wikipedia.org/wiki/National_identity_number_(Norway)).

The type `Skaar.Fodselsnummer` has validation logic to parse
a string containing a Norwegian [id-number](https://en.wikipedia.org/wiki/National_identity_number_(Norway)).

The type `Skaar.DNummer` represents a [d-number](https://www.skatteetaten.no/en/person/national-registry/identitetsnummer/d-nummer/).

The type `Skaar.DufNummer` represents a [DUF-number](https://www.udi.no/en/word-definitions/duf-number/).

The type `Skaar.IdNumber` may represent any of the numbers.

- With JsonConverter and type converter
- Parsing includes validation logic
- Equality and comparison
- Can create new random valid values (for testing purposes).

`Skaar.Utils.IdNumberFactory` can be used to create new random values. 


**Usage:**
```C#
var rawInput = "23072388054";
var number = Fodselsnummer.CreateNew(rawInput); //alt. use Parse or TryParse
var validity = number.IsValid;
var birthDate = number.BirthDate;
var result = number.ToString();
```

![Icon](https://raw.githubusercontent.com/oyms/NorwegianTypes/refs/heads/main/.idea/.idea.Skaar.NorwegianTypes/.idea/icon.svg)