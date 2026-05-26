# Hochbeet Planer

**Schülerin:** Chrissi | **Klasse:** 4AAIFT | **HTL Mödling** | **Fach:** POS1


## Projektbeschreibung

Der Hochbeet Planer ist eine WPF-Desktopanwendung in C#. Man kann die Größe eines Hochbeets eingeben, Pflanzen per Klick platzieren, das Beet zufällig befüllen lassen und Beete in einer SQLite-Datenbank speichern, laden und löschen.


## Umgesetzte Funktionen

- Beetgröße in cm eingeben, daraus wird ein dynamisches Raster generiert
- Pflanzen per Klick im Beet platzieren
- Pflanzengröße und **Mindestabstand** werden berücksichtigt
- Bereits belegte Felder können nicht überschrieben werden
- Zufallsbefüllung des Beets
- Speichern, Laden und Löschen von Beeten in **SQLite**
- Tooltip mit Pflanzeninfos beim Darüberfahren mit der Maus
- Startbildschirm mit Start-Button
- 8 Pflanzen zur Auswahl


## Verfügbare Pflanzen

| Pflanze | Größe (BxL) | Erntezeit | Wasser | Sonne |
|---------|-------------|-----------|--------|-------|
| 🍅 Paradaiser | 2x2 | Juli–Oktober | Viel | Vollsonne |
| 🥒 Gurke | 3x2 | Juni–September | Viel | Vollsonne |
| 🌶️ Chilli | 1x1 | August–Oktober | Mittel | Vollsonne |
| 🥕 Karotte | 1x1 | Juli–Oktober | Mittel | Vollsonne |
| 🥬 Salat | 2x2 | April–Oktober | Viel | Halbschatten |
| 🫑 Paprika | 2x2 | August–Oktober | Mittel | Vollsonne |
| 🍆 Aubergine | 2x2 | August–Oktober | Viel | Vollsonne |
| 🥦 Zucchini | 3x3 | Juni–September | Viel | Vollsonne |


## Datenbankstruktur

Zwei Tabellen in SQLite:

Tabelle `Beete`: Id, Name, Breite, Laenge

Tabelle `BeetBelegung`: Id, BeetId, Zeile, Spalte, PflanzenName

`BeetBelegung` referenziert auf `Beete` über die BeetId (1:n Beziehung).


## Projektstruktur

*(handgezeichnete Skizze folgt)*


## Installation und Start

1. Repository klonen oder ZIP herunterladen
2. Projekt in Visual Studio öffnen (`Hochbeet_Planer.sln`)
3. NuGet Pakete werden automatisch wiederhergestellt
4. F5 drücken oder "Starten" klicken


## Bedienungsanleitung

1. App starten → Startbildschirm erscheint → **START** drücken
2. Beetgröße in cm eingeben (Breite und Länge) → **"Beet generieren"** klicken
3. Pflanze in der Pflanzenliste auswählen
4. Auf eine Zelle im Beet klicken → Pflanze wird platziert
5. Oder **"Zufallsbeet"** klicken → Beet wird automatisch befüllt
6. Beetname eingeben → **"Speichern"** klicken
7. Gespeichertes Beet in der Liste auswählen → **"Laden"** klicken
8. Nicht mehr benötigtes Beet auswählen → **"Löschen"** klicken

*Tipp: Mit der Maus über eine Pflanze fahren zeigt Infos zur Erntezeit, Wasser und Sonne!*


## Screenshots

*(Screenshots folgen)*


## Verwendete Technologien

- C# / WPF (.NET)
- SQLite über `System.Data.SQLite`
- NuGet Pakete: `System.Data.SQLite`, `SQLitePCLRaw.bundle_e_sqlite3`


## Was noch geplant war

- Freund/Feind-Kompatibilität zwischen Pflanzen
- Pflanzen aus Datenbank laden statt hardcoded
- RadioButtons automatisch aus Datenbank generieren
- Maßband am Raster
- Täglicher Pflege-Reminder


## Quellen

**Video:**
- WPF Tutorial Reihe: https://www.youtube.com/watch?v=szfMIkAV6Bw&list=PL_pqkvxZ6ho0zf5P3PYgrm7VbKW6CiUXH

**Dokumentation und Tutorials:**
- https://learn.microsoft.com/en-us/docs/
- https://wpf-tutorial.com/de/23/panels/einfuhrung-in-wpf-panels/
- https://www.lernmoment.de/alle/wpf-grid-panel-xaml-grundlagen/
- https://stackoverflow.com/questions
- https://www.c-sharpcorner.com/UploadFile/mahesh/border-in-wpf/
- https://mycsharp.de/forum/threads/58298/botton-background-im-csharp-code-aendern

**Layout Inspiration:**
- https://github.com/Carlos487/awesome-wpf
- https://wpfui.lepo.co/#check-out-the-gallery
