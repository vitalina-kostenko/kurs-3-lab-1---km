using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Lab1_Addressing
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("=== Лабораторна робота №1 ===");
            Console.WriteLine("Адресація в сучасних комп'ютерних мережах");
            Console.WriteLine("Варіант 2\n");

            Task1();
            Task2();
            Task3();
            Task4();
            Task5();

            string reportPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Звіт_ЛР1.docx");
            reportPath = Path.GetFullPath(reportPath);
            ReportGenerator.Generate(reportPath);
            Console.WriteLine(new string('=', 60));
            Console.WriteLine($"Звіт збережено: {reportPath}");
        }

        #region Task 1 — MAC address analysis

        static void Task1()
        {
            Console.WriteLine(new string('=', 60));
            Console.WriteLine("ЗАВДАННЯ 1: Аналіз MAC-адрес");
            Console.WriteLine(new string('=', 60));

            string mac1 = "0180C2000000";
            string mac2 = "3413E8114585";

            AnalyzeMac(mac1);
            Console.WriteLine();
            AnalyzeMac(mac2);
            Console.WriteLine();
        }

        static void AnalyzeMac(string macRaw)
        {
            string formatted = FormatMac(macRaw);
            Console.WriteLine($"MAC-адреса: {formatted}");

            byte firstByte = Convert.ToByte(macRaw[..2], 16);
            string binary = Convert.ToString(firstByte, 2).PadLeft(8, '0');
            Console.WriteLine($"Старший байт у двійковій формі: {binary}");

            int igBit = firstByte & 0x01;
            int glBit = (firstByte >> 1) & 0x01;

            Console.WriteLine($"Біт I/G (b0) = {igBit}");
            Console.WriteLine($"Біт G/L (b1) = {glBit}");

            if (macRaw.ToUpper() == "FFFFFFFFFFFF")
            {
                Console.WriteLine("Тип: Широкомовна (Broadcast) MAC-адреса");
                Console.WriteLine("Застосування: лише як адреса отримувача");
            }
            else if (igBit == 1)
            {
                Console.WriteLine("Тип: Групова (Multicast) MAC-адреса");
                Console.WriteLine("Застосування: лише як адреса отримувача");
            }
            else
            {
                Console.WriteLine("Тип: Унікальна (Unicast) MAC-адреса");
                Console.WriteLine("Застосування: як адреса відправника та адреса отримувача");
            }

            Console.WriteLine(glBit == 0
                ? "Адреса: Глобальна (Global)"
                : "Адреса: Локальна (Local)");

            string oui = macRaw[..6].ToUpper();
            Console.WriteLine($"OUI: {oui[..2]}-{oui[2..4]}-{oui[4..6]}");

            string protocol = LookupOuiOrProtocol(macRaw.ToUpper());
            if (protocol != null)
                Console.WriteLine($"Протокол/Виробник: {protocol}");

            string ouiRange = $"{oui[..2]}-{oui[2..4]}-{oui[4..6]}-00-00-00 — {oui[..2]}-{oui[2..4]}-{oui[4..6]}-FF-FF-FF";
            Console.WriteLine($"Діапазон адрес OUI: {ouiRange}");
        }

        static string FormatMac(string raw)
        {
            raw = raw.ToUpper();
            var sb = new StringBuilder();
            for (int i = 0; i < raw.Length; i++)
            {
                if (i > 0 && i % 2 == 0) sb.Append('-');
                sb.Append(raw[i]);
            }
            return sb.ToString();
        }

        static string LookupOuiOrProtocol(string mac)
        {
            var knownMacs = new Dictionary<string, string>
            {
                ["01000CCCCCCC"] = "CDP, VTP, UDLD, DTP, PAgP (Cisco)",
                ["01000CCCCCCD"] = "VSTP (Cisco)",
                ["0180C2000000"] = "STP, RSTP, MSTP (IEEE 802.1D/802.1w/802.1s)",
                ["0180C2000001"] = "Pause (Flow Control, MAC-Control)",
                ["0180C2000002"] = "LACP, LAMP, Link OAM",
                ["0180C2000003"] = "Port Authentication 802.1x",
                ["0180C2000007"] = "E-LMI",
                ["0180C2000008"] = "Provider MSTP",
                ["0180C200000D"] = "Provider MMRP",
                ["0180C200000E"] = "LLDP",
                ["0180C2000020"] = "MMRP / GARP",
                ["0180C2000021"] = "MVRP",
                ["011B19000000"] = "PTP version 2 over Ethernet",
                ["FFFFFFFFFFFF"] = "Широкомовна MAC-адреса (Broadcast)",
            };

            if (knownMacs.TryGetValue(mac, out var result))
                return result;

            var ouiVendors = new Dictionary<string, string>
            {
                ["0180C2"] = "IEEE 802.1 (зарезервовано)",
                ["01000C"] = "Cisco Systems",
                ["01005E"] = "IPv4-Multicast",
                ["333300"] = "IPv6-Multicast",
                ["011B19"] = "PTP (IEEE 1588)",
                ["3413E8"] = "Intel Corporate",
                ["000C41"] = "Cisco-Linksys",
                ["0005FF"] = "SNP Technologies",
                ["000088"] = "Brocade Communications",
                ["00A0C0"] = "Digital Link Corp",
                ["14ABC5"] = "Intel Corporate",
                ["4C8093"] = "Intel Corporate",
                ["000585"] = "Juniper Networks",
                ["00058D"] = "Juniper Networks",
                ["18D11F"] = "Wuhan Huagong Genuine Optics",
                ["00E0FC"] = "Huawei Technologies",
                ["001E10"] = "Entorian Technologies",
                ["88A2E5"] = "Juniper Networks",
                ["000AEB"] = "TP-LINK Technologies",
                ["00040D"] = "Avaya",
                ["2CB05D"] = "Netgear",
                ["F41563"] = "Hewlett Packard Enterprise",
                ["040A83"] = "Unknown",
                ["0005851"] = "Juniper Networks",
            };

            string oui = mac[..6];
            if (ouiVendors.TryGetValue(oui, out var vendor))
                return vendor;

            return null;
        }

        #endregion

        #region Task 2 — Classful IP addressing

        static void Task2()
        {
            Console.WriteLine(new string('=', 60));
            Console.WriteLine("ЗАВДАННЯ 2: Класова IP-адресація");
            Console.WriteLine(new string('=', 60));

            string ip = "132.93.233.8";
            byte[] octets = ParseIp(ip);
            int firstOctet = octets[0];

            string ipClass;
            int prefix;
            if (firstOctet <= 127) { ipClass = "A"; prefix = 8; }
            else if (firstOctet <= 191) { ipClass = "B"; prefix = 16; }
            else if (firstOctet <= 223) { ipClass = "C"; prefix = 24; }
            else if (firstOctet <= 239) { ipClass = "D"; prefix = 0; }
            else { ipClass = "E"; prefix = 0; }

            Console.WriteLine($"IP-адреса: {ip}");
            Console.WriteLine($"Перший октет: {firstOctet} → Клас {ipClass}");

            uint mask = prefix > 0 ? (0xFFFFFFFF << (32 - prefix)) : 0;
            uint inverseMask = ~mask;
            uint ipNum = IpToUint(octets);
            uint networkAddr = ipNum & mask;
            uint hostAddr = ipNum & inverseMask;
            uint broadcastAddr = networkAddr | inverseMask;
            uint minHost = networkAddr + 1;
            uint maxHost = broadcastAddr - 1;
            long hostCount = (long)Math.Pow(2, 32 - prefix) - 2;

            Console.WriteLine($"Класова маска: {UintToIp(mask)}");
            Console.WriteLine($"Інверсна класова маска: {UintToIp(inverseMask)}");
            Console.WriteLine($"Класовий префікс: /{prefix}");
            Console.WriteLine($"IP-адреса мережі: {UintToIp(networkAddr)}");
            Console.WriteLine($"IP-адреса вузла: {UintToIp(hostAddr)}");
            Console.WriteLine($"Мінімальна IP-адреса вузла: {UintToIp(minHost)}");
            Console.WriteLine($"Максимальна IP-адреса вузла: {UintToIp(maxHost)}");
            Console.WriteLine($"Широкомовна IP-адреса: {UintToIp(broadcastAddr)}");
            Console.WriteLine($"Кількість вузлів: 2^{32 - prefix} - 2 = {hostCount}");
            Console.WriteLine();
        }

        #endregion

        #region Task 3 — Classful approach for N hosts

        static void Task3()
        {
            Console.WriteLine(new string('=', 60));
            Console.WriteLine("ЗАВДАННЯ 3: Класовий підхід — кількість вузлів: 8191");
            Console.WriteLine(new string('=', 60));

            int hosts = 8191;
            long x = hosts + 2 - 1; // = 8192

            Console.WriteLine($"Задана кількість вузлів: {hosts}");
            Console.WriteLine($"X = K + 2 - 1 = {hosts} + 2 - 1 = {x}");

            string chosenClass;
            int prefix;
            string mask;
            string inverseMask;
            long maxHosts;

            if (x <= 254) { chosenClass = "C"; prefix = 24; mask = "255.255.255.0"; inverseMask = "0.0.0.255"; maxHosts = 254; }
            else if (x <= 65534) { chosenClass = "B"; prefix = 16; mask = "255.255.0.0"; inverseMask = "0.0.255.255"; maxHosts = 65534; }
            else { chosenClass = "A"; prefix = 8; mask = "255.0.0.0"; inverseMask = "0.255.255.255"; maxHosts = 16777214; }

            Console.WriteLine($"Оптимальний клас: {chosenClass}");
            Console.WriteLine($"Класова маска: {mask}");
            Console.WriteLine($"Інверсна класова маска: {inverseMask}");
            Console.WriteLine($"Класовий префікс: /{prefix}");

            string networkIp, minIp, maxIp, broadcastIp;
            switch (chosenClass)
            {
                case "B":
                    networkIp = "180.1.0.0";
                    minIp = "180.1.0.1";
                    maxIp = "180.1.255.254";
                    broadcastIp = "180.1.255.255";
                    break;
                case "A":
                    networkIp = "10.0.0.0";
                    minIp = "10.0.0.1";
                    maxIp = "10.255.255.254";
                    broadcastIp = "10.255.255.255";
                    break;
                default:
                    networkIp = "195.10.1.0";
                    minIp = "195.10.1.1";
                    maxIp = "195.10.1.254";
                    broadcastIp = "195.10.1.255";
                    break;
            }

            Console.WriteLine($"Обрана IP-адреса мережі: {networkIp}");
            Console.WriteLine($"Мінімальна IP-адреса вузла: {minIp}");
            Console.WriteLine($"Максимальна IP-адреса вузла: {maxIp}");
            Console.WriteLine($"Широкомовна IP-адреса: {broadcastIp}");
            Console.WriteLine($"Максимальна кількість вузлів: 2^{32 - prefix} - 2 = {maxHosts}");
            Console.WriteLine($"Використано: {hosts}, не використано: {maxHosts - hosts}");
            Console.WriteLine();
        }

        #endregion

        #region Task 4 — Classless IP addressing

        static void Task4()
        {
            Console.WriteLine(new string('=', 60));
            Console.WriteLine("ЗАВДАННЯ 4: Безкласова IP-адресація");
            Console.WriteLine(new string('=', 60));

            string ip = "132.93.233.8";
            int prefix = 19;

            byte[] octets = ParseIp(ip);
            uint ipNum = IpToUint(octets);

            string ipBinary = UintToBinaryDotted(ipNum);
            Console.WriteLine($"IP-адреса: {ip}");
            Console.WriteLine($"IP-адреса (двійкова): {ipBinary}");
            Console.WriteLine($"Префікс: /{prefix}");

            uint mask = (0xFFFFFFFF << (32 - prefix));
            uint inverseMask = ~mask;

            Console.WriteLine($"\nМаска мережі (двійкова): {UintToBinaryDotted(mask)}");
            Console.WriteLine($"Маска мережі: {UintToIp(mask)}");

            Console.WriteLine($"\nІнверсна маска (двійкова): {UintToBinaryDotted(inverseMask)}");
            Console.WriteLine($"Інверсна маска: {UintToIp(inverseMask)}");

            uint networkAddr = ipNum & mask;
            Console.WriteLine($"\nIP-адреса AND Маска:");
            Console.WriteLine($"  {ipBinary}");
            Console.WriteLine($"  {UintToBinaryDotted(mask)}");
            Console.WriteLine($"  {UintToBinaryDotted(networkAddr)}");
            Console.WriteLine($"IP-адреса мережі: {UintToIp(networkAddr)}");

            uint hostAddr = ipNum & inverseMask;
            Console.WriteLine($"\nIP-адреса AND Інверсна маска:");
            Console.WriteLine($"  {ipBinary}");
            Console.WriteLine($"  {UintToBinaryDotted(inverseMask)}");
            Console.WriteLine($"  {UintToBinaryDotted(hostAddr)}");
            Console.WriteLine($"IP-адреса вузла: {UintToIp(hostAddr)}");

            uint broadcastAddr = networkAddr | inverseMask;
            uint minHost = networkAddr + 1;
            uint maxHost = broadcastAddr - 1;
            long hostCount = (long)Math.Pow(2, 32 - prefix) - 2;

            Console.WriteLine($"\nМінімальна IP-адреса вузла (двійкова): {UintToBinaryDotted(minHost)}");
            Console.WriteLine($"Мінімальна IP-адреса вузла: {UintToIp(minHost)}");
            Console.WriteLine($"Максимальна IP-адреса вузла (двійкова): {UintToBinaryDotted(maxHost)}");
            Console.WriteLine($"Максимальна IP-адреса вузла: {UintToIp(maxHost)}");
            Console.WriteLine($"Широкомовна IP-адреса (двійкова): {UintToBinaryDotted(broadcastAddr)}");
            Console.WriteLine($"Широкомовна IP-адреса: {UintToIp(broadcastAddr)}");
            Console.WriteLine($"Кількість вузлів: 2^(32-{prefix}) - 2 = 2^{32 - prefix} - 2 = {hostCount}");
            Console.WriteLine();
        }

        #endregion

        #region Task 5 — Classless approach for N hosts

        static void Task5()
        {
            Console.WriteLine(new string('=', 60));
            Console.WriteLine("ЗАВДАННЯ 5: Безкласовий підхід — кількість вузлів: 252");
            Console.WriteLine(new string('=', 60));

            int hosts = 252;
            long x = hosts + 2 - 1; // = 253

            Console.WriteLine($"Задана кількість вузлів: {hosts}");
            Console.WriteLine($"X = K + 2 - 1 = {hosts} + 2 - 1 = {x}");

            int h = 0;
            long temp = x;
            while ((1L << h) <= temp) h++;

            string xBinary = Convert.ToString(x, 2);
            Console.WriteLine($"X у двійковій формі: {xBinary}");
            Console.WriteLine($"H (кількість бітів для вузлів) = {h}");

            int prefix = 32 - h;
            Console.WriteLine($"Префікс P = 32 - {h} = {prefix}");

            uint mask = (0xFFFFFFFF << (32 - prefix));
            uint inverseMask = ~mask;

            Console.WriteLine($"\nМаска мережі (двійкова): {UintToBinaryDotted(mask)}");
            Console.WriteLine($"Маска мережі: {UintToIp(mask)}");
            Console.WriteLine($"Інверсна маска: {UintToIp(inverseMask)}");
            Console.WriteLine($"Префікс: /{prefix}");

            string networkIp = "195.10.1.0";
            byte[] netOctets = ParseIp(networkIp);
            uint netNum = IpToUint(netOctets);

            uint broadcastAddr = netNum | inverseMask;
            uint minHost = netNum + 1;
            uint maxHost = broadcastAddr - 1;
            long hostCount = (1L << (32 - prefix)) - 2;

            Console.WriteLine($"\nОбрана IP-адреса мережі: {networkIp}/{prefix}");
            Console.WriteLine($"Узагальнений запис: {networkIp}  {UintToIp(mask)}  або  {networkIp}/{prefix}");
            Console.WriteLine($"Мінімальна IP-адреса вузла: {UintToIp(minHost)}");
            Console.WriteLine($"Максимальна IP-адреса вузла: {UintToIp(maxHost)}");
            Console.WriteLine($"Широкомовна IP-адреса: {UintToIp(broadcastAddr)}");
            Console.WriteLine($"Кількість вузлів: 2^(32-{prefix}) - 2 = 2^{32 - prefix} - 2 = {hostCount}");
            Console.WriteLine($"Використано: {hosts}, не використано: {hostCount - hosts}");
            Console.WriteLine();
        }

        #endregion

        #region Helpers

        static byte[] ParseIp(string ip)
        {
            string[] parts = ip.Split('.');
            return new byte[]
            {
                byte.Parse(parts[0]),
                byte.Parse(parts[1]),
                byte.Parse(parts[2]),
                byte.Parse(parts[3])
            };
        }

        static uint IpToUint(byte[] octets)
        {
            return ((uint)octets[0] << 24) |
                   ((uint)octets[1] << 16) |
                   ((uint)octets[2] << 8) |
                   octets[3];
        }

        static string UintToIp(uint val)
        {
            return $"{(val >> 24) & 0xFF}.{(val >> 16) & 0xFF}.{(val >> 8) & 0xFF}.{val & 0xFF}";
        }

        static string UintToBinaryDotted(uint val)
        {
            return $"{Convert.ToString((int)((val >> 24) & 0xFF), 2).PadLeft(8, '0')}." +
                   $"{Convert.ToString((int)((val >> 16) & 0xFF), 2).PadLeft(8, '0')}." +
                   $"{Convert.ToString((int)((val >> 8) & 0xFF), 2).PadLeft(8, '0')}." +
                   $"{Convert.ToString((int)(val & 0xFF), 2).PadLeft(8, '0')}";
        }

        #endregion
    }
}
