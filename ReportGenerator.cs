using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Lab1_Addressing
{
    public static class ReportGenerator
    {
        public static void Generate(string filePath)
        {
            using var doc = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document);
            var mainPart = doc.AddMainDocumentPart();
            mainPart.Document = new Document();
            var body = new Body();

            AddTitle(body, "ЗВІТ");
            AddTitle(body, "Лабораторна робота №1");
            AddTitle(body, "АДРЕСАЦІЯ В СУЧАСНИХ КОМП'ЮТЕРНИХ МЕРЕЖАХ");
            AddCentered(body, "Варіант 2");
            AddEmptyLine(body);

            AddHeading(body, "Мета роботи");
            AddParagraph(body, "Ознайомитися із загальними принципами адресації у сучасних комп'ютерних мережах; ознайомитися із структурою, видами та застосуванням MAC-адрес; ознайомитися із структурою, видами та застосуванням IP-адрес версій 4 та 6; отримати практичні навички аналізу та визначення параметрів MAC-адрес; отримати практичні навички аналізу, визначення та розрахунку параметрів IP-адрес версії 4.");
            AddEmptyLine(body);

            // ===== TASK 1 =====
            AddHeading(body, "Завдання 1. Аналіз MAC-адрес");
            AddParagraph(body, "Вхідні дані: MAC-адреса 1: 01-80-C2-00-00-00, MAC-адреса 2: 34-13-E8-11-45-85");
            AddEmptyLine(body);

            AddSubHeading(body, "MAC-адреса 1: 01-80-C2-00-00-00");
            AddParagraph(body, "Старший байт 01 у двійковій системі числення: 00000001");
            AddParagraph(body, "Біт I/G (b0) = 1 — адреса є груповою");
            AddParagraph(body, "Біт G/L (b1) = 0 — адреса є глобальною");
            AddBoldParagraph(body, "Висновок: Задана MAC-адреса є груповою (Multicast) глобальною адресою. Може застосовуватися лише як адреса отримувача кадру.");
            AddParagraph(body, "OUI: 01-80-C2 — зарезервовано IEEE 802.1");
            AddParagraph(body, "Протокол: Дана адреса використовується протоколами STP (Spanning Tree Protocol), RSTP (Rapid STP), MSTP (Multiple STP).");
            AddParagraph(body, "Діапазон адрес OUI: 01-80-C2-00-00-00 — 01-80-C2-FF-FF-FF");
            AddEmptyLine(body);

            AddSubHeading(body, "MAC-адреса 2: 34-13-E8-11-45-85");
            AddParagraph(body, "Старший байт 34 у двійковій системі числення: 00110100");
            AddParagraph(body, "Біт I/G (b0) = 0 — адреса є унікальною");
            AddParagraph(body, "Біт G/L (b1) = 0 — адреса є глобальною");
            AddBoldParagraph(body, "Висновок: Задана MAC-адреса є унікальною (Unicast) глобальною адресою. Може застосовуватися як адреса відправника, так і як адреса отримувача кадру.");
            AddParagraph(body, "OUI: 34-13-E8 — виділено для Intel Corporate.");
            AddParagraph(body, "Діапазон адрес OUI: 34-13-E8-00-00-00 — 34-13-E8-FF-FF-FF");
            AddEmptyLine(body);

            // ===== TASK 2 =====
            AddHeading(body, "Завдання 2. Класова IP-адресація");
            AddParagraph(body, "Вхідні дані: IP-адреса мережного адаптера/інтерфейсу вузла: 132.93.233.8");
            AddEmptyLine(body);

            AddSubHeading(body, "Розв'язання");
            AddParagraph(body, "Перший октет IP-адреси: 132. Оскільки 128 ≤ 132 ≤ 191, IP-адреса належить до класу B.");
            AddEmptyLine(body);

            AddTable(body, new string[,]
            {
                { "Параметр", "Значення" },
                { "Клас IP-адреси", "B" },
                { "Пряма класова маска", "255.255.0.0" },
                { "Інверсна класова маска", "0.0.255.255" },
                { "Класовий префікс", "/16" },
                { "IP-адреса мережі", "132.93.0.0" },
                { "IP-адреса вузла", "0.0.233.8" },
                { "Мінімальна IP-адреса вузла", "132.93.0.1" },
                { "Максимальна IP-адреса вузла", "132.93.255.254" },
                { "Широкомовна IP-адреса", "132.93.255.255" },
                { "Кількість вузлів", "2^16 - 2 = 65534" },
            });
            AddEmptyLine(body);

            // ===== TASK 3 =====
            AddHeading(body, "Завдання 3. Класовий підхід — визначення мережі для 8191 вузлів");
            AddParagraph(body, "Вхідні дані: Кількість вузлів: 8191");
            AddEmptyLine(body);

            AddSubHeading(body, "Розв'язання");
            AddParagraph(body, "Загальна кількість IP-адрес: X = K + 2 - 1 = 8191 + 2 - 1 = 8192");
            AddParagraph(body, "За таблицею класів:");
            AddParagraph(body, "• Клас C: max 254 вузлів — недостатньо");
            AddParagraph(body, "• Клас B: max 65534 вузлів — достатньо (оптимальний)");
            AddParagraph(body, "• Клас A: max 16777214 вузлів — достатньо, але неекономно");
            AddBoldParagraph(body, "Оптимальний клас: B");
            AddEmptyLine(body);

            AddTable(body, new string[,]
            {
                { "Параметр", "Значення" },
                { "Класова маска", "255.255.0.0" },
                { "Інверсна класова маска", "0.0.255.255" },
                { "Класовий префікс", "/16" },
                { "Обрана IP-адреса мережі", "180.1.0.0" },
                { "Мінімальна IP-адреса вузла", "180.1.0.1" },
                { "Максимальна IP-адреса вузла", "180.1.255.254" },
                { "Широкомовна IP-адреса", "180.1.255.255" },
                { "Максимальна кількість вузлів", "2^16 - 2 = 65534" },
                { "Використано", "8191" },
                { "Не використано", "57343" },
            });
            AddEmptyLine(body);

            // ===== TASK 4 =====
            AddHeading(body, "Завдання 4. Безкласова IP-адресація");
            AddParagraph(body, "Вхідні дані: IP-адреса: 132.93.233.8, Префікс: /19");
            AddEmptyLine(body);

            AddSubHeading(body, "Розв'язання");
            AddParagraph(body, "IP-адреса у двійковій системі числення:");
            AddParagraph(body, "132.93.233.8 → 10000100.01011101.11101001.00001000");
            AddEmptyLine(body);

            AddParagraph(body, "Маска мережі (19 одиниць, 13 нулів):");
            AddParagraph(body, "11111111.11111111.11100000.00000000 → 255.255.224.0");
            AddEmptyLine(body);

            AddParagraph(body, "Інверсна маска (логічне NOT від прямої маски):");
            AddParagraph(body, "00000000.00000000.00011111.11111111 → 0.0.31.255");
            AddEmptyLine(body);

            AddParagraph(body, "IP-адреса мережі (IP AND Маска):");
            AddParagraph(body, "  10000100.01011101.11101001.00001000  (132.93.233.8)");
            AddParagraph(body, "  11111111.11111111.11100000.00000000  (255.255.224.0)");
            AddParagraph(body, "  10000100.01011101.11100000.00000000  → 132.93.224.0");
            AddEmptyLine(body);

            AddParagraph(body, "IP-адреса вузла (IP AND Інверсна маска):");
            AddParagraph(body, "  10000100.01011101.11101001.00001000  (132.93.233.8)");
            AddParagraph(body, "  00000000.00000000.00011111.11111111  (0.0.31.255)");
            AddParagraph(body, "  00000000.00000000.00001001.00001000  → 0.0.9.8");
            AddEmptyLine(body);

            AddTable(body, new string[,]
            {
                { "Параметр", "Значення" },
                { "Маска мережі", "255.255.224.0" },
                { "Інверсна маска", "0.0.31.255" },
                { "IP-адреса мережі", "132.93.224.0" },
                { "IP-адреса вузла", "0.0.9.8" },
                { "Мінімальна IP-адреса вузла", "132.93.224.1" },
                { "Максимальна IP-адреса вузла", "132.93.255.254" },
                { "Широкомовна IP-адреса", "132.93.255.255" },
                { "Кількість вузлів", "2^(32-19) - 2 = 2^13 - 2 = 8190" },
            });
            AddEmptyLine(body);

            // ===== TASK 5 =====
            AddHeading(body, "Завдання 5. Безкласовий підхід — визначення мережі для 252 вузлів");
            AddParagraph(body, "Вхідні дані: Кількість вузлів: 252");
            AddEmptyLine(body);

            AddSubHeading(body, "Розв'язання");
            AddParagraph(body, "Загальна кількість IP-адрес: X = K + 2 - 1 = 252 + 2 - 1 = 253");
            AddParagraph(body, "X у двійковій системі: 253 = 11111101");
            AddParagraph(body, "Кількість бітів: H = 8");
            AddParagraph(body, "Префікс: P = 32 - H = 32 - 8 = 24");
            AddEmptyLine(body);

            AddParagraph(body, "Маска мережі у двійковій системі (24 одиниці, 8 нулів):");
            AddParagraph(body, "11111111.11111111.11111111.00000000 → 255.255.255.0");
            AddEmptyLine(body);

            AddTable(body, new string[,]
            {
                { "Параметр", "Значення" },
                { "Маска мережі", "255.255.255.0" },
                { "Інверсна маска", "0.0.0.255" },
                { "Префікс", "/24" },
                { "Обрана IP-адреса мережі", "195.10.1.0/24" },
                { "Мінімальна IP-адреса вузла", "195.10.1.1" },
                { "Максимальна IP-адреса вузла", "195.10.1.254" },
                { "Широкомовна IP-адреса", "195.10.1.255" },
                { "Максимальна кількість вузлів", "2^8 - 2 = 254" },
                { "Використано", "252" },
                { "Не використано", "2" },
            });
            AddEmptyLine(body);

            // ===== ВИСНОВКИ =====
            AddHeading(body, "Висновки");
            AddParagraph(body, "У ході виконання лабораторної роботи було:");
            AddEmptyLine(body);
            AddParagraph(body, "1. Проаналізовано дві MAC-адреси та визначено їх типи: групова (01-80-C2-00-00-00 — використовується протоколами STP/RSTP/MSTP) та унікальна (34-13-E8-11-45-85 — виробник Intel Corporate).");
            AddEmptyLine(body);
            AddParagraph(body, "2. Для IP-адреси 132.93.233.8 із застосуванням класового підходу визначено належність до класу B та розраховано всі параметри адресації (маска 255.255.0.0, префікс /16, мережа 132.93.0.0).");
            AddEmptyLine(body);
            AddParagraph(body, "3. Для мережі з 8191 вузлами за класовим підходом обрано оптимальний клас B з маскою 255.255.0.0 та префіксом /16. Продемонстровано неефективність класового підходу: не використовується 57343 адреси.");
            AddEmptyLine(body);
            AddParagraph(body, "4. Для IP-адреси 132.93.233.8 із префіксом /19 за безкласовим підходом визначено мережу 132.93.224.0 з маскою 255.255.224.0 та кількістю вузлів 8190.");
            AddEmptyLine(body);
            AddParagraph(body, "5. Для мережі з 252 вузлами за безкласовим підходом визначено оптимальний префікс /24 з маскою 255.255.255.0 та кількістю доступних адрес 254, з яких не використовується лише 2.");
            AddEmptyLine(body);
            AddParagraph(body, "Розроблено програму мовою C#, яка автоматично розраховує всі необхідні параметри адресації для заданого варіанта.");

            mainPart.Document.Append(body);
            mainPart.Document.Save();
        }

        static void AddTitle(Body body, string text)
        {
            var p = new Paragraph(
                new ParagraphProperties(
                    new Justification { Val = JustificationValues.Center },
                    new SpacingBetweenLines { After = "0" }),
                new Run(
                    new RunProperties(
                        new Bold(),
                        new FontSize { Val = "32" },
                        new RunFonts { Ascii = "Times New Roman", HighAnsi = "Times New Roman" }),
                    new Text(text)));
            body.Append(p);
        }

        static void AddCentered(Body body, string text)
        {
            var p = new Paragraph(
                new ParagraphProperties(
                    new Justification { Val = JustificationValues.Center },
                    new SpacingBetweenLines { After = "0" }),
                new Run(
                    new RunProperties(
                        new FontSize { Val = "28" },
                        new RunFonts { Ascii = "Times New Roman", HighAnsi = "Times New Roman" }),
                    new Text(text)));
            body.Append(p);
        }

        static void AddHeading(Body body, string text)
        {
            var p = new Paragraph(
                new ParagraphProperties(
                    new SpacingBetweenLines { Before = "240", After = "120" }),
                new Run(
                    new RunProperties(
                        new Bold(),
                        new FontSize { Val = "28" },
                        new RunFonts { Ascii = "Times New Roman", HighAnsi = "Times New Roman" }),
                    new Text(text)));
            body.Append(p);
        }

        static void AddSubHeading(Body body, string text)
        {
            var p = new Paragraph(
                new ParagraphProperties(
                    new SpacingBetweenLines { Before = "120", After = "60" }),
                new Run(
                    new RunProperties(
                        new Bold(),
                        new Italic(),
                        new FontSize { Val = "24" },
                        new RunFonts { Ascii = "Times New Roman", HighAnsi = "Times New Roman" }),
                    new Text(text)));
            body.Append(p);
        }

        static void AddParagraph(Body body, string text)
        {
            var p = new Paragraph(
                new ParagraphProperties(
                    new SpacingBetweenLines { After = "0" }),
                new Run(
                    new RunProperties(
                        new FontSize { Val = "24" },
                        new RunFonts { Ascii = "Times New Roman", HighAnsi = "Times New Roman" }),
                    new Text(text) { Space = SpaceProcessingModeValues.Preserve }));
            body.Append(p);
        }

        static void AddBoldParagraph(Body body, string text)
        {
            var p = new Paragraph(
                new ParagraphProperties(
                    new SpacingBetweenLines { After = "0" }),
                new Run(
                    new RunProperties(
                        new Bold(),
                        new FontSize { Val = "24" },
                        new RunFonts { Ascii = "Times New Roman", HighAnsi = "Times New Roman" }),
                    new Text(text) { Space = SpaceProcessingModeValues.Preserve }));
            body.Append(p);
        }

        static void AddEmptyLine(Body body)
        {
            body.Append(new Paragraph(
                new ParagraphProperties(
                    new SpacingBetweenLines { After = "0" })));
        }

        static void AddTable(Body body, string[,] data)
        {
            var table = new Table();

            var tblProps = new TableProperties(
                new TableBorders(
                    new TopBorder { Val = BorderValues.Single, Size = 4 },
                    new BottomBorder { Val = BorderValues.Single, Size = 4 },
                    new LeftBorder { Val = BorderValues.Single, Size = 4 },
                    new RightBorder { Val = BorderValues.Single, Size = 4 },
                    new InsideHorizontalBorder { Val = BorderValues.Single, Size = 4 },
                    new InsideVerticalBorder { Val = BorderValues.Single, Size = 4 }),
                new TableWidth { Width = "5000", Type = TableWidthUnitValues.Pct });
            table.Append(tblProps);

            int rows = data.GetLength(0);
            int cols = data.GetLength(1);

            for (int r = 0; r < rows; r++)
            {
                var row = new TableRow();
                for (int c = 0; c < cols; c++)
                {
                    var cell = new TableCell();
                    var rp = new RunProperties(
                        new FontSize { Val = "22" },
                        new RunFonts { Ascii = "Times New Roman", HighAnsi = "Times New Roman" });
                    if (r == 0) rp.Append(new Bold());

                    cell.Append(
                        new TableCellProperties(
                            new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center }),
                        new Paragraph(
                            new ParagraphProperties(new SpacingBetweenLines { After = "0" }),
                            new Run(rp, new Text(data[r, c]))));
                    row.Append(cell);
                }
                table.Append(row);
            }

            body.Append(table);
        }
    }
}
