# Firebird Metadata Tool

## Opis
Aplikacja s³u¿y do stworzenia bazy danych Firebird oraz utworzenia w niej obiektów tabel, domen i procedur dla wskazanego pliku metadanych (w formacie .sql).
Zawiera tak¿e mo¿liwoœæ eksportu bazy danych do skryptu

## Wymagania
- .NET 8.0
- Firebird 5.0
- IBExpert (opcjonalnie)

## Przyk³ad u¿ycia

1. Otwórz projekt w Visual Studio 2022
2. W pliku DatabaseConsts ustaw odpowiednie dane w Connection String (standardowo SYSDBA/masterkey)
3. Zbuduj projekt
4. Uruchom projekt z wiersza poleceñ np
   * utworzenie bazy na podstawie sryptu:
	 <sciezka do projektu>\bin\Debug\net8.0\Firebird build-db --db-dir "<folder dla tworzonej bazy>" --scripts-dir "<skrypt metadanych>"
   * import metadanych bazdy do skryptu
	 <sciezka do projektu>\bin\Debug\net8.0\Firebird export-scripts --connection-string "<connection string do eksportowanego pliku>" --output-dir "<folder dla wyeksportowanego skryptu>"
   * aktualizacja bazy
     <sciezka do projektu>\bin\Debug\net8.0\Firebird update-db --connection-string "<connection string>" --scripts-dir "<folder ze skryptami do aktualizacji>"
5. Strukture bazy mozesz obejzec za pomoca isql 
   np. isql -user SYSDBA -password masterkey <sciezka do pliku bazy danych>

## Przyk³ady
Przyk³adowy plik metadanych: `examples/test.sql`