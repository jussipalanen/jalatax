# JalaTax – tiivistelmä ja käyttöohje

<p align="center">
  <img src="assets/jalatax-logo.png" alt="JalaTax – verolaskuri VB.NET:llä" width="420">
</p>

**In English:** [README.md](README.md)

> **Huom:** JalaTax on esimerkkisovellus. **Mikään sen tulos ei ole virallinen verolaskelma.** Oletussäännöt ja kaikki esimerkkitiedot ovat kuvitteellisia. Valinnainen sääntöjoukko käyttää vuoden 2026 valtion tuloveroasteikkoa yksinkertaistettuna (katso [Sääntöjoukot](#sääntöjoukot)). JalaTax ei jäljittele mitään todellista verohallinnon järjestelmää.

## Tiivistelmä

JalaTax on pieni VB.NET-sovellus, joka laskee verotapaukselle veron konfiguroitavien sääntöjen perusteella. Laskennan jokainen vaihe kirjataan, joten jokaisen tuloksen syy voidaan jäljittää.

Sovelluksessa on kaksi käyttöliittymää:

- **Työpöytäsovellus** (Windows Forms) on pääkäyttöliittymä. Siinä syötetään verotapaus, ja se näyttää tuloksen, tarkistusvirheet ja kirjausketjun.
- **Komentorivisovellus** käsittelee esimerkkitapaukset kerralla ja toimii kaikilla alustoilla.

### Mitä projekti osoittaa

| Osa-alue | Miten se näkyy projektissa |
|---|---|
| VB.NET ja .NET-arkkitehtuuri | Nykyaikainen VB.NET (.NET 10), `Option Strict On`, kerrosrakenne: liiketoimintalogiikka `JalaTax.Core`-kirjastossa, käyttöliittymät erillään |
| Konfiguroitavat liiketoimintasäännöt | Veroportaat ja vähennykset JSON-tiedostoissa, ei koodissa. Säännöt ovat pieniä, erikseen testattavia luokkia (`ITaxRule`). |
| Tarkistukset ja virheenkäsittely | Syötteet tarkistetaan ennen laskentaa, ja kaikki virheet näytetään kerralla. Virheelliset asetukset hylätään selkein virheilmoituksin. |
| Jäljitettävyys | Kirjausketju näyttää jokaisen vaiheen: tapauksen lataus, tarkistus, käytetty sääntöjoukko, jokainen veroporras ja lopputulos |
| Testaus | 173 automaattista testiä (MSTest): rajatapaukset, pyöristys, virhetilanteet, käännökset ja julkaistun veroasteikon luvut |
| Laadunvarmistus | Koodityyli ja analysaattorit pakotetaan käännöksessä. GitHub Actions tarkistaa muotoilun, käännöksen ja testit jokaisessa muutoksessa. |
| Hallittu kehitystapa | Jokainen muutos kulkee issue → haara → pull request → katselmointi. Päätökset on kirjattu issueihin ja PR-kuvauksiin. |
| Dokumentointi | Englanninkielinen README, tämä suomenkielinen ohje ja kehitysohjeet (`AGENTS.md`) |
| Kaksikielisyys | Suomi (oletus) ja englanti, myös lukujen muotoilu (`45 000,00` / `45,000.00`) |

## Pikaopas

### Vaatimukset

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- Työpöytäsovellus vaatii Windowsin. Komentorivisovellus ja testit toimivat myös Linuxissa ja macOS:ssä.
- Valinnaisesti Visual Studio 2026 tai Visual Studio Code (C#-laajennus)

### Asennus ja käynnistys

```powershell
git clone https://github.com/jussipalanen/jalatax.git
cd jalatax
dotnet build
dotnet run --project src/JalaTax.WinForms     # työpöytäsovellus
dotnet run --project src/JalaTax.Console      # komentorivisovellus
dotnet test                                   # testit
```

Visual Studiossa avaa `JalaTax.sln`, valitse käynnistysprojektiksi **JalaTax.WinForms** ja paina **F5**. Visual Studio Codessa valitse käynnistysmääritys **JalaTax.WinForms** ja paina **F5**.

## Työpöytäsovelluksen käyttö

![Työpöytäsovellus, tapaus DEMO-001](docs/screenshots/calculation-demo-001-fi.png)

1. **Valitse esimerkkitapaus** (`DEMO-001`–`DEMO-003`) tai syötä tunniste, vuositulot ja vähennykset itse. Summat voi kirjoittaa suomalaisessa muodossa, esimerkiksi `45000,50`.
2. **Paina Laske** (tai Enter).
3. **Lue tulos:**
   - **Tulos** näyttää vuositulot, vähennykset, perusvähennyksen, verotettavan tulon, veron veroportaittain sekä lasketun veron.
   - **Kirjausketju** näyttää jokaisen käsittelyvaiheen kellonaikoineen.
4. **Virheelliset syötteet** merkitään punaisella kuvakkeella kentän viereen, ja virheilmoitus näkyy painikkeiden alla. Esimerkiksi `DEMO-003`:n vähennykset ovat negatiiviset, joten tapaus hylätään eikä veroa lasketa.
5. **Tyhjennä** tyhjentää kentät ja tulokset. **Tietoja** näyttää sovelluksen version.

Tilarivin oikeassa reunassa on kaksi valitsinta:

- **Säännöt:** valitsee sääntöjoukon (kuvitteelliset esimerkkisäännöt tai valtion tuloveroasteikko 2026).
- **Kieli:** vaihtaa kielen (suomi tai englanti).

Kummankin vaihdon jälkeen avoinna oleva tapaus lasketaan uudelleen.

| Hylätty tapaus (DEMO-003) | Tietoja-ikkuna |
|---|---|
| ![DEMO-003](docs/screenshots/validation-demo-003-fi.png) | ![Tietoja](docs/screenshots/about-dialog-fi.png) |

Käynnistysvalitsimet:

```powershell
dotnet run --project src/JalaTax.WinForms -- --lang en                   # englanniksi
dotnet run --project src/JalaTax.WinForms -- --rules rules-fi-2026.json  # vuoden 2026 asteikko valittuna
```

## Komentorivisovelluksen käyttö

```powershell
dotnet run --project src/JalaTax.Console                                     # suomeksi, esimerkkisäännöt
dotnet run --project src/JalaTax.Console -- --rules data/rules-fi-2026.json  # vuoden 2026 asteikko
dotnet run --project src/JalaTax.Console -- --lang en                        # englanniksi
dotnet run --project src/JalaTax.Console -- omat-tapaukset.json              # omat verotapaukset
```

Sovellus käsittelee jokaisen tapauksen ja tulostaa tarkistuksen, sovelletut säännöt ja tuloksen:

```text
JalaTax 1.0.0
----------------------------------------
Sääntöjoukko: Valtion tuloveroasteikko 2026 (yksinkertaistettu)
Esimerkkilaskenta, ei virallinen verolaskelma.

Käsitellään tapausta: DEMO-001

Tarkistus
✓ Tulot ja vähennykset tarkistettu

Säännöt
✓ Vähennykset tehty: verotettava tulo 42 500,00 (tulot 45 000,00 − vähennykset 2 500,00 − perusvähennys 0,00, vähintään 0,00)
✓ Veroporras 0,00–22 000,00, 12,64 %: verotettu 22 000,00, vero 2 780,80
✓ Veroporras 22 000,00–32 600,00, 19 %: verotettu 10 600,00, vero 2 014,00
✓ Veroporras 32 600,00–40 100,00, 30,25 %: verotettu 7 500,00, vero 2 268,75
✓ Veroporras 40 100,00–52 100,00, 33,25 %: verotettu 2 400,00, vero 798,00

Tulos
----------------------------------------
Vuositulot:            45 000,00
Vähennykset:            2 500,00
Perusvähennys:              0,00
Verotettava tulo:      42 500,00
Laskettu vero:          7 861,55
```

Paluukoodit:

| Koodi | Merkitys |
|---|---|
| 0 | Onnistui. Hylätyt tapaukset kuuluvat normaaliin tulostukseen. |
| 1 | Asetusvirhe (sääntötiedosto) |
| 2 | Syötetiedoston virhe |
| 3 | Odottamaton virhe |
| 4 | Virheellinen `--lang`- tai `--rules`-valitsin |

## Laskentasäännöt

Kaikki sääntöjoukot lasketaan samoilla säännöillä:

1. **Verotettava tulo** = vuositulot − tapauksen vähennykset − sääntöjoukon perusvähennys, kuitenkin vähintään 0.
2. **Progressiivinen asteikko:** kunkin veroportaan prosentti koskee vain sitä osaa tulosta, joka osuu portaan sisälle.
3. **Portaan rajat:** alaraja kuuluu portaaseen ja yläraja ei, joten täsmälleen 22 000 € kuuluu seuraavaan portaaseen. Ylimmällä portaalla ei ole ylärajaa.
4. **Pyöristys:** summat ovat `Decimal`-tyyppiä, ja lopullinen vero pyöristetään kahteen desimaaliin (puolikkaat ylöspäin).

## Sääntöjoukot

Jokainen `data/rules*.json`-tiedosto on sääntöjoukko:

| Tiedosto | Nimi | Sisältö |
|---|---|---|
| `rules.json` (oletus) | Kuvitteelliset esimerkkisäännöt | Keksitty perusvähennys (3 000 €) ja kolme veroporrasta (10 %, 20 %, 30 %) |
| `rules-fi-2026.json` | Valtion tuloveroasteikko 2026 (yksinkertaistettu) | Julkaistu vuoden 2026 valtion tuloveroasteikko |

### Valtion tuloveroasteikko 2026

Lähde: *Laki vuoden 2026 tuloveroasteikosta* (1140/2025).

| Verotettava ansiotulo (€) | Vero alarajan kohdalla (€) | Vero ylittävästä tulon osasta |
|---|---:|---:|
| 0 – 22 000 | 0,00 | 12,64 % |
| 22 000 – 32 600 | 2 780,80 | 19,00 % |
| 32 600 – 40 100 | 4 794,80 | 30,25 % |
| 40 100 – 52 100 | 7 063,55 | 33,25 % |
| 52 100 – | 11 053,55 | 37,50 % |

Sääntöjoukko on **tarkoituksella yksinkertaistettu**: se laskee vain valtion tuloveron verotettavasta ansiotulosta.
- **Pois jätetty:** kunnallisvero, kirkollisvero, sairausvakuutusmaksut ja verosta tehtävät vähennykset, kuten työtulovähennys.
- **Vähennykset:** tapauksen vähennykset vähennetään tuloista, eikä perusvähennystä ole.
- **Testit:** testit varmistavat, että laskenta tuottaa jokaisen portaan alarajalla täsmälleen lain taulukon luvun.

| Tapaus | Verotettava tulo | Valtion tulovero |
|---|---:|---:|
| DEMO-001 | 42 500 € | **7 861,55 €** |
| DEMO-002 | 66 800 € | **16 566,05 €** |

![Valtion tuloveroasteikko 2026, tapaus DEMO-002](docs/screenshots/finnish-scale-2026-demo-002-fi.png)

### Oma sääntötiedosto

Tee uusi tiedosto `data/rules-omat.json`. Käännös kopioi `data`-kansion tiedostot ohjelman viereen, joten työpöytäsovellus näyttää sen Säännöt-valikossa, kun sovellus seuraavan kerran käynnistetään komennolla `dotnet run` (tai Visual Studiosta). Komentorivillä sen voi antaa suoraan: `--rules data/rules-omat.json`.

```json
// Kommentit ovat sallittuja.
{
  "names": { "fi": "Omat säännöt", "en": "My rules" },
  "basicDeduction": 1000,
  "taxBrackets": [
    { "min": 0,     "max": 30000, "rate": 0.15 },
    { "min": 30000, "max": null,  "rate": 0.25 }
  ]
}
```

- **Pakolliset kentät:** `basicDeduction`, `taxBrackets` sekä jokaisen portaan `min` ja `rate`. `names` on valinnainen.
- **Rajat:** ensimmäisen portaan on alettava nollasta, ja jokaisen portaan `min` on oltava sama kuin edellisen `max`. Vain viimeiseltä portaalta puuttuu `max` (tai se on `null`).
- **Prosentti** annetaan desimaalina välillä 0–1 (esimerkiksi `0.15` = 15 %).

Virheellinen tiedosto hylätään, ja virheilmoitus luettelee kaikki ongelmat, esimerkiksi *"Veroporras 2: alarajan (min) on oltava sama kuin edellisen portaan yläraja (30 000,00)…"*.

### Omat verotapaukset

Verotapaukset ovat JSON-taulukossa, kuten `data/example-taxpayer.json`:

```json
[
  { "taxpayerId": "DEMO-010", "annualIncome": 52000, "deductions": 750 }
]
```

Käytä vain kuvitteellisia tunnisteita (`DEMO-…`). Älä koskaan käytä oikeita henkilötunnuksia tai asiakastietoja.

## Virheselvitys: miksi tapaus sai tämän tuloksen?

Kirjausketju on tehty juuri tätä varten. Kun tulos näyttää väärältä:

1. **Tarkista sääntöjoukko.** Kirjausketjun rivi *Sääntöjoukko valittu* kertoo, millä säännöillä tulos laskettiin. Moni "väärä" tulos johtuu väärästä sääntöjoukosta.
2. **Tarkista verotettava tulo.** *DeductionRule*-rivi näyttää laskutoimituksen: tulot − vähennykset − perusvähennys. Jos verotettava tulo on 0, vähennykset ovat vähintään yhtä suuret kuin tulot.
3. **Käy veroportaat läpi.** Jokaisella käytetyllä portaalla on oma *TaxBracketRule*-rivinsä: porras, prosentti, verotettu osuus ja vero. Portaiden verojen summa on laskettu vero ennen pyöristystä.
4. **Toista tapaus komentorivillä.** Tallenna tapaus tiedostoon ja aja `dotnet run --project src/JalaTax.Console -- tapaus.json`. Näin saat saman kirjausketjun tekstinä, esimerkiksi liitettäväksi virheilmoitukseen.
5. **Kirjoita testi.** Kun syy löytyy, lisää rajatapaukselle testi (esimerkiksi `TaxBracketRuleTests`- tai `FinnishTaxScale2026Tests`-luokkaan) ennen korjausta. Testi jää estämään saman virheen palaamisen.

Yleisimmät virheilmoitukset:

| Virheilmoitus | Syy | Korjaus |
|---|---|---|
| *Vähennykset eivät saa olla negatiivisia.* | Syötteessä on negatiivinen summa | Korjaa syöte. Tapaus lasketaan vasta, kun se on kunnossa. |
| *Anna luku.* | Summa ei ole luku (esimerkiksi `abc`) | Kirjoita summa numeroina, esimerkiksi `45000` |
| *Asetustiedostoa ei löytynyt: …* | Sääntötiedoston polku on väärä | Tarkista `--rules`-valitsimen polku |
| *Asetustiedoston JSON on virheellinen: …* | JSON-syntaksivirhe tai tuntematon kenttä (esimerkiksi kirjoitusvirhe kentän nimessä) | Korjaa tiedosto virheilmoituksen kohdan mukaan |
| *Veroporras N: …* | Portaissa on aukko, päällekkäisyys tai virheellinen prosentti | Katso [Oma sääntötiedosto](#oma-sääntötiedosto) |

## Testit ja laadunvarmistus

```powershell
dotnet format --verify-no-changes   # koodityylin tarkistus
dotnet build                        # varoitukset ovat virheitä
dotnet test                         # 173 testiä
```

Samat tarkistukset ajetaan GitHub Actionsissa jokaiselle pull requestille.

## Versiot ja julkaisut

JalaTax käyttää semanttista versiointia (`major.minor.patch`). Versio on yhdessä paikassa, `Directory.Build.props`-tiedostossa. Muutokset kirjataan [CHANGELOG.md](CHANGELOG.md)-tiedostoon.

Jokainen julkaisu on Git-tagi `vX.Y.Z` ja GitHub Release, jossa on valmiit zip-paketit työpöytäsovelluksesta ja komentorivisovelluksesta. Uusi versio julkaistaan näin:

1. Päivitä pull requestissa `Directory.Build.props`-tiedoston versio ja lisää CHANGELOG.md:hen uuden version osio.
2. Yhdistä pull request. GitHub Actions huomaa, ettei versiolle ole vielä tagia. Se kääntää ja testaa ratkaisun, luo tagin ja julkaisee Releasen.

Testit varmistavat, että CHANGELOG.md:ssä on osio nykyiselle versiolle, joten ilman muutoskuvausta ei voi julkaista.

## Lisätietoa

- [README.md](README.md): tekninen kuvaus englanniksi (arkkitehtuuri, rakenne, konfiguraatio)
- [AGENTS.md](AGENTS.md): kehitysohjeet ja periaatteet
- [CHANGELOG.md](CHANGELOG.md): versiohistoria
- [Releases](https://github.com/jussipalanen/jalatax/releases): ladattavat versiot
