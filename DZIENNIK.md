# StockFlow — dziennik sesji

Postępy z kolejnych sesji nauki. Najnowsza sesja na górze.
Plan i zasady współpracy: [`HARMONOGRAM.md`](HARMONOGRAM.md).

---

## ▶ Następnym razem zaczynamy od

**Etap 0 — Środowisko i repozytorium:** instalacja .NET 10 SDK i Visual Studio 2026 Community, `git init`, `dotnet new gitignore`
(+ dopisanie `.playwright-mcp/`), repozytorium na GitHubie, pierwszy commit.

---

## Sesja 1 — 2026-10-16

**Zrobione:**
- Przeczytany wątek z ChatGPT „MVC i API w .NET” (architektura 4 projektów, nazwa StockFlow, plan ChatGPT).
- Omówiony screen `stock-view.png` i zakres: MVP = Products, Collections, Inventory, Orders, Customers.
- Powstał `HARMONOGRAM.md`: etapy 0–16, uwagi do kolejności z ChatGPT (najpierw jeden prosty produkt przez wszystkie
  warstwy, potem rozbudowa), decyzje domenowe do omówienia.
- Spisane zasady współpracy (sekcja 2 harmonogramu).

**Stan kursu:** Moduł 7 — Bazy danych (w trakcie). Testowanie pominięte, Web API jeszcze nie.

**Stan projektu:** w folderze jest tylko dokumentacja i screen — kodu jeszcze nie ma, Git niezainicjalizowany.

**Ustalenia:**
- Solution tworzymy z terminala (.NET CLI), a na co dzień pracujemy w Visual Studio.
- Zainstalowane jest VS Community 2022 17.14, które nie obsługuje `net10.0`, więc trzeba doinstalować VS 2026 Community.

**Otwarte sprawy:**
- Zadanie `VariantAttribute` z wątku ChatGPT przeniesione do etapu 6.
- Decyzje domenowe (cena na produkcie/wariancie, waluta, wariant domyślny) — do omówienia przed etapami 2 i 6.
