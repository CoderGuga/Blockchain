# Blockchain — v0.1

## Trumpas aprašymas
- Paprasta vieno mazgo (single-node) blokų grandinė.
- Pagrindinis tikslas: demonstruoti blokų struktūrą, transakcijų kaupimą, kasybą (mining) bei grandinės validaciją.

## Versija
- v0.1 — minimalus prototipas, skirtas konceptų demonstravimui ir tolimesniam plėtojimui.

## Pagrindinės savybės

### Blockchain sistemos parametrai
- Reguliuojamas kasybos sudėtingumas (leading zeros skaičius)
- Konfigūruojamas transakcijų skaičius bloke
- Trys darbo režimai (test, full, custom)

### Transakcijų sistema
- UTXO (Unspent Transaction Output) modelis
- Transakcijų validavimas
- Merkle root skaičiavimas transakcijų verifikavimui

### Blokų struktūra
- Bloko antraštė su ankstesnio bloko hash
- Merkle root transakcijų integralumui
- Proof-of-work kasybos sistema

### Vartotojų valdymas
- Vartotojų registracijos sistema
- Balanso sekimas per UTXO
- Saugi transakcijų validacija

## Naudojimas

Programa siūlo tris veikimo režimus:

1. **Test režimas**
   - 50 vartotojų
   - 500 transakcijų
   - Sudėtingumo lygis: 3
   - 100 transakcijų per bloką

2. **Full režimas**
   - 1000 vartotojų
   - 10000 transakcijų
   - Sudėtingumo lygis: 3
   - 100 transakcijų per bloką

3. **Custom režimas**
   - Galimybė nustatyti savo parametrus:
     - Vartotojų skaičių
     - Transakcijų skaičių
     - Kasybos sudėtingumą
     - Transakcijų skaičių bloke

### Paleidimo instrukcija

1. Paleiskite programą (dotnet run)
2. Pasirinkite veikimo režimą (1-3)
3. Palaukite, kol simuliacija bus baigta
4. Peržiūrėkite blockchain statistiką

## Ekranvaidžiai


## Techninė realizacija

### Bloko struktūra
- Kiekvienas blokas turi:
  - Indeksą
  - Bloko antraštę
  - Transakcijų sąrašą
  - Bloko hash'ą

### Transakcijų apdorojimas
- Transakcijos validuojamos naudojant UTXO modelį
- Kiekviena transakcija turi:
  - Siuntėjo ir gavėjo informaciją
  - Sumą
  - Įėjimo ir išėjimo UTXO
  - Transakcijos ID

### Kasybos procesas
- Proof-of-work sistema su reguliuojamu sudėtingumu
- Merkle root skaičiavimas transakcijų verifikavimui
- Blokų validacija prieš pridedant į grandinę

## Sistemos reikalavimai

- .NET 9.0 arba naujesnė versija

