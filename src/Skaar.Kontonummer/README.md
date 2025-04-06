Kontonummer 🇳🇴
===

This package is an implementation of the
[Norwegian bank account number](https://no.wikipedia.org/wiki/Kontonummer).

The type `Skaar.Kontonummer` has validation logic to parse
a string containing an account number.

- With JsonConverter and type converter
- Parsing includes validation logic
- Equality and comparison
- IBAN number
- IBAN bank lookup
- Account type

**Usage:**
```C#
var rawInput = "32445395551";
var number = Kontonummer.CreateNew(rawInput); //alt. use Parse or TryParse
var validity = number.IsValid;
var iban = number.Iban;
var bank = number.Bank;
var result = number.ToString();
```

<img alt="icon" style="width: 200px;" src="https://raw.githubusercontent.com/oyms/NorwegianTypes/refs/heads/main/.idea/.idea.Skaar.NorwegianTypes/.idea/icon.svg" />
