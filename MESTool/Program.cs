﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace MESTool
{
    public class Program
    {
        private enum Platform
        {
            PS3,
            X360,
            PC,
            WiiU
        }

        static Platform Plat = Platform.PS3;

        const string TimeFormat = @"mm\:ss\.ffff";
        static string[] Table = new string[0x10000];

        static void Main(string[] args)
        {
            string[] TblEntries = Properties.Resources.CharTable.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            foreach (string Entry in TblEntries)
            {
                string[] Parameters = Entry.Split(Convert.ToChar("="));
                Table[int.Parse(Parameters[0], NumberStyles.HexNumber)] = Parameters[1];
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Bayonetta *.mes Text Dumper/Creator by gdkchan");
            Console.WriteLine("Version 0.5.0");
            Console.WriteLine(string.Empty);
            Console.ResetColor();

            if (args.Length == 0)
                PrintUsage();
            else
            {
                string Operation = args[0];
                string FileName = null;

                if (args.Length == 2)
                {
                    FileName = args[1];
                }
                else if (args.Length == 3)
                {
                    switch (args[1])
                    {
                        case "-ps3":  Plat = Platform.PS3;  break;
                        case "-x360": Plat = Platform.X360; break;
                        case "-pc":   Plat = Platform.PC;   break;
                        case "-raw":  Plat = Platform.WiiU; break;
                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Error: Invalid platform specified!");
                            Console.WriteLine(string.Empty);
                            PrintUsage();
                            return;
                    }
                    FileName = args[2];
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: Invalid number of arguments!");
                    Console.WriteLine(string.Empty);
                    PrintUsage();
                    return;
                }

                switch (Operation)
                {
                    case "-d":
                        if (FileName == "-all")
                        {
                            string[] Files = Directory.GetFiles(Environment.CurrentDirectory);
                            foreach (string File in Files) if (Path.GetExtension(File).ToLower() == ".mes") Dump(File);
                        }
                        else
                            Dump(FileName);

                        break;
                    case "-c":
                        if (FileName == "-all")
                        {
                            string[] Folders = Directory.GetDirectories(Environment.CurrentDirectory);
                            foreach (string Folder in Folders) Create(Folder);
                        }
                        else
                            Create(FileName);

                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Error: Invalid operation specified!");
                        Console.WriteLine(string.Empty);
                        PrintUsage();
                        break;
                }
            }
        }

        static void PrintUsage()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Usage:");
            Console.ForegroundColor = ConsoleColor.Gray;

            Console.WriteLine("MESTool.exe [operation] [platform] [file|-all]");
            Console.WriteLine(string.Empty);

            Console.WriteLine("[operation]");
            Console.WriteLine("-d  Dumps a *.mes file to a folder");
            Console.WriteLine("-c  Creates a *.mes file from a folder");
            Console.WriteLine(string.Empty);

            Console.WriteLine("[platform]");
            Console.WriteLine("-ps3  For the PS3 version of the game (default)");
            Console.WriteLine("-x360  For the Xbox 360 version of the game");
            Console.WriteLine("-pc  For the PC version of the game");
            Console.WriteLine("-wiiu  For the Wii U version (partial)");
            Console.WriteLine(string.Empty);

            Console.WriteLine("-all  Manipulate all the files on the work directory");
            Console.WriteLine(string.Empty);

            Console.WriteLine("Example:");
            Console.WriteLine("MESTool -d file.mes");
            Console.WriteLine("MESTool -d -all");
            Console.WriteLine("MESTool -c folder");
            Console.WriteLine("MESTool -c -all");
            Console.ResetColor();
        }

        private struct SectEntry
        {
            public uint Index;
            public uint Offset;
            public uint Length;
        }

        public class TextureMapUV
        {
            [XmlAttribute]
            public ushort Id;
            public float StartX;
            public float StartY;
            public float EndX;
            public float EndY;
        }

        public class TextureMapSize
        {
            [XmlAttribute]
            public ushort Id;
            public float Width;
            public float Height;
        }

        public class TextureMapUVSection
        {
            [XmlAttribute]
            public uint Count;
            public List<TextureMapUV> Entries;

            public TextureMapUVSection()
            {
                Entries = new List<TextureMapUV>();
            }
        }

        [XmlRootAttribute("TextureUV", Namespace = "gdkchan/MESTool")]
        public class TUV
        {
            public uint Count;
            public ushort Value;

            public List<TextureMapUV> Entries;

            public TUV()
            {
                Entries = new List<TextureMapUV>();
            }
        }

        public class TextureMapSizeSection
        {
            [XmlAttribute]
            public uint Count;
            public uint CharSizeX;
            public uint CharSizeY;
            public List<TextureMapSize> Entries;

            public TextureMapSizeSection()
            {
                Entries = new List<TextureMapSize>();
            }
        }

        [XmlRootAttribute("TextureMap", Namespace = "gdkchan/MESTool")]
        public class TextureMapInfo
        {
            public TextureMapUVSection UVTable;
            public TextureMapSizeSection SizeTable;

            public TextureMapInfo()
            {
                UVTable = new TextureMapUVSection();
                SizeTable = new TextureMapSizeSection();
            }
        }

        private static void Dump(string FileName)
        {
            FileStream Input = new FileStream(FileName, FileMode.Open);
            EndianBinaryReader Reader = new EndianBinaryReader(Input, Plat == Platform.PC
                    ? EndianBinary.Endian.Little
                    : EndianBinary.Endian.Big);

            string OutDir = Path.GetFileNameWithoutExtension(FileName);
            Directory.CreateDirectory(OutDir);

            uint TextureSectionOffset = Reader.ReadUInt32();
            uint TextCount = Reader.ReadUInt32();
            int TutorialDialogsCount = Reader.ReadInt32();
            if (TutorialDialogsCount > -1) File.WriteAllText(Path.Combine(OutDir, "DialogSettings.txt"), "TutorialDialogsCount=" + TutorialDialogsCount);

            /*
             * Texts
             */
            List<SectEntry> TextList = new List<SectEntry>();
            for (int i = 0; i < TextCount; i++)
            {
                SectEntry Entry = new SectEntry();

                Entry.Index = Reader.ReadUInt32();
                Entry.Offset = Reader.ReadUInt32() + 4;
                Entry.Length = Reader.ReadUInt32();

                TextList.Add(Entry);
            }

            StringBuilder Texts = new StringBuilder();
            foreach (SectEntry Entry in TextList)
            {
                Input.Seek(Entry.Offset, SeekOrigin.Begin);

                for (int i = 0; i < Entry.Length; i += 2)
                {
                    ushort Value = Reader.ReadUInt16();
                    string Character = Table[Value];

                    if (Character != null)
                        Texts.Append(Character);
                    else if (Value == 0x8000)
                        Texts.Append(Environment.NewLine);
                    else if (Value == 0x8f00)
                    {
                        Texts.Append("[id=" + Reader.ReadUInt16().ToString() + "]");
                        i += 2;
                    }
                    else if (Value == 0x8f01)
                    {
                        float Start = Reader.ReadUInt16() / 64f;
                        float End = Reader.ReadUInt16() / 64f;
                        TimeSpan StartPos = TimeSpan.FromSeconds(Start);
                        TimeSpan EndPos = TimeSpan.FromSeconds(End);
                        Texts.Append("[time=" + StartPos.ToString(TimeFormat) + "/" + EndPos.ToString(TimeFormat) + "]");
                        i += 4;
                    }
                    else
                        Texts.Append("[0x" + Value.ToString("X4") + "]");

                    if (i > Entry.Length) break;
                }

                Texts.Append(Environment.NewLine + Environment.NewLine);
            }

            File.WriteAllText(Path.Combine(OutDir, "Texts.txt"), Texts.ToString().TrimEnd());

            /*
             * Texture stuff
             */
            Input.Seek(TextureSectionOffset, SeekOrigin.Begin);
            uint Section2Offset = Reader.ReadUInt32() + TextureSectionOffset;
            uint WTBOffset = Reader.ReadUInt32() + TextureSectionOffset;

            TextureMapInfo TexInfo = new TextureMapInfo();
            uint Section1Count = Reader.ReadUInt32();
            TexInfo.UVTable.Count = Section1Count;
            for (int i = 0; i < Section1Count; i++)
            {
                TextureMapUV Entry = new TextureMapUV();
                Entry.Id = Reader.ReadUInt16();
                Reader.ReadUInt16();
                Entry.StartX = Reader.ReadSingle();
                Entry.StartY = Reader.ReadSingle();
                Entry.EndX = Reader.ReadSingle();
                Entry.EndY = Reader.ReadSingle();
                TexInfo.UVTable.Entries.Add(Entry);
            }

            Input.Seek(Section2Offset, SeekOrigin.Begin);
            uint Section2Count = Reader.ReadUInt32();
            TexInfo.SizeTable.Count = Section2Count;
            TexInfo.SizeTable.CharSizeX = Reader.ReadUInt32();
            TexInfo.SizeTable.CharSizeY = Reader.ReadUInt32();
            Reader.ReadUInt32();
            for (int i = 0; i < Section2Count; i++)
            {
                TextureMapSize Entry = new TextureMapSize();
                Entry.Id = Reader.ReadUInt16();
                Reader.ReadUInt16();
                Entry.Width = Reader.ReadSingle();
                Entry.Height = Reader.ReadSingle();
                TexInfo.SizeTable.Entries.Add(Entry);
                Reader.ReadUInt32();
            }

            FileStream TexInfoOut = new FileStream(Path.Combine(OutDir, "TextureMap.xml"), FileMode.Create);
            XmlSerializer Serializer = new XmlSerializer(typeof(TextureMapInfo));
            Serializer.Serialize(TexInfoOut, TexInfo);
            TexInfoOut.Close();

            Input.Seek(WTBOffset, SeekOrigin.Begin);
            if (Plat == Platform.WiiU || Plat == Platform.PC)
            {
                byte[] Buffer = new byte[Input.Length - WTBOffset];
                Input.Read(Buffer, 0, Buffer.Length);
                File.WriteAllBytes(Path.Combine(OutDir, "Texture.wtb"), Buffer);
            }
            else
                DumpWTB(Reader, Path.Combine(OutDir, "Texture.dds"));

            Input.Close();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Dumped file " + Path.GetFileName(FileName) + "!");
            Console.ResetColor();
        }

        private static void DumpWTB(EndianBinaryReader Reader, string OutFileName)
        {
            FileStream DDSOut = new FileStream(OutFileName, FileMode.Create);
            BinaryWriter DDS = new BinaryWriter(DDSOut);

            uint WTBOffset = (uint)Reader.BaseStream.Position;
            uint WTBSignature = Reader.ReadUInt32();
            Reader.ReadUInt32();
            uint TextureCount = Reader.ReadUInt32();
            uint TexturePointerOffset = Reader.ReadUInt32() + WTBOffset;
            uint SectionLengthOffset = Reader.ReadUInt32() + WTBOffset;
            uint UnknowDataOffset = Reader.ReadUInt32() + WTBOffset;

            Reader.Seek(SectionLengthOffset, SeekOrigin.Begin);
            uint SectionLength = Reader.ReadUInt32();

            Reader.Seek(TexturePointerOffset, SeekOrigin.Begin);
            uint TextureOffset = Reader.ReadUInt32();
            Reader.Seek(TextureOffset + WTBOffset, SeekOrigin.Begin);

            uint Signature = Reader.ReadUInt32();

            if (!(Signature == 3)) //PS3
            {
                uint GTFLength = Reader.ReadUInt32();
                uint GTFTextureCount = Reader.ReadUInt32();
                uint GTFId = Reader.ReadUInt32();
                uint GTFTextureDataOffset = Reader.ReadUInt32();
                uint GTFTextureDataLength = Reader.ReadUInt32();
                byte GTFTextureFormat = Reader.ReadByte();
                byte GTFMipmaps = Reader.ReadByte();
                byte GTFDimension = Reader.ReadByte();
                byte GTFCubemaps = Reader.ReadByte();
                uint GTFRemap = Reader.ReadUInt32();
                ushort GTFTextureWidth = Reader.ReadUInt16();
                ushort GTFTextureHeight = Reader.ReadUInt16();
                ushort GTFDepth = Reader.ReadUInt16();
                ushort GTFPitch = Reader.ReadUInt16();
                ushort GTFLocation = Reader.ReadUInt16();
                ushort GTFTextureOffset = Reader.ReadUInt16();
                Reader.Seek(8, SeekOrigin.Current);

                bool isSwizzle = (GTFTextureFormat & 0x20) == 0;
                bool isNormalized = (GTFTextureFormat & 0x40) == 0;
                GTFTextureFormat = (byte)(GTFTextureFormat & ~0x60);

                byte[] TextureData = new byte[GTFTextureDataLength];
                Reader.Read(TextureData, 0, TextureData.Length);

                DDS.Write(0x20534444); //DDS Signature
                DDS.Write((uint)0x7c); //Header size (without the signature)
                DDS.Write((uint)0x00021007); //DDS Flags
                DDS.Write((uint)GTFTextureHeight);
                DDS.Write((uint)GTFTextureWidth);
                DDS.Write((uint)GTFPitch);
                DDS.Write((uint)GTFDepth);
                DDS.Write((uint)GTFMipmaps);
                DDSOut.Seek(0x2c, SeekOrigin.Current); //Reserved space for future use
                DDS.Write((uint)0x20); //PixelFormat structure size (32 bytes)

                uint PixelFlags = 0;
                if (GTFTextureFormat >= 0x86 && GTFTextureFormat <= 0x88) PixelFlags = 4; //Is DXT Compressed
                else PixelFlags = 0x40; //Isn't compressed
                DDS.Write(PixelFlags);

                switch (GTFTextureFormat)
                {
                    case 0x86: DDS.Write(Encoding.ASCII.GetBytes("DXT1")); break;
                    case 0x87: DDS.Write(Encoding.ASCII.GetBytes("DXT3")); break;
                    case 0x88: DDS.Write(Encoding.ASCII.GetBytes("DXT5")); break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Warning: Unsupported texture format! Use the -raw option!");
                        Console.ResetColor();
                        DDSOut.Close();
                        File.Delete(OutFileName);
                        return;
                }
                DDSOut.Seek(20, SeekOrigin.Current);

                DDS.Write((uint)0x400000); //Caps 1
                if (GTFCubemaps > 0) DDS.Write((uint)0x200); else DDS.Write((uint)0); //Caps 2
                DDS.Write((uint)0); //Unused stuff
                DDS.Write((uint)0);
                DDS.Write((uint)0);
                DDS.Write(TextureData);
            }
            else //Xbox 360
            {
                uint Count = Reader.ReadUInt32();
                Reader.Seek(0xc, SeekOrigin.Current);
                Reader.ReadUInt32(); //0xFFFF0000
                Reader.ReadUInt32(); //0xFFFF0000
                Reader.ReadUInt32(); //0x81000002
                uint TextureFormat = Reader.ReadUInt32();
                uint TextureDescriptor = Reader.ReadUInt32();
                Reader.ReadUInt32(); //0xD10
                uint Mipmaps = ((Reader.ReadUInt32() >> 6) & 7) + 1;
                uint OriginalLength = Reader.ReadUInt32();

                uint Width = (TextureDescriptor & 0x1fff) + 1;
                uint Height = ((TextureDescriptor >> 13) & 0x1fff) + 1;

                StringBuilder TexDescriptor = new StringBuilder();
                TexDescriptor.AppendLine("Width=" + Width.ToString());
                TexDescriptor.AppendLine("Height=" + Height.ToString());
                TexDescriptor.AppendLine("UnpaddedLength=" + OriginalLength.ToString());
                File.WriteAllText(Path.Combine(Path.GetDirectoryName(OutFileName), "TextureSettings.txt"), TexDescriptor.ToString());

                Width = Math.Max(Width, 128);
                Height = Math.Max(Height, 128);

                byte[] TextureData = new byte[SectionLength - (TextureOffset + 0x34)];
                Reader.Read(TextureData, 0, TextureData.Length);
                TextureData = XEndian16(TextureData);
                TextureData = XTextureScramble(TextureData, Width, Height, (DXTType)TextureFormat);

                DDS.Write(0x20534444); //DDS Signature
                DDS.Write((uint)0x7c); //Header size (without the signature)
                DDS.Write((uint)0x00021007); //DDS Flags
                DDS.Write(Height);
                DDS.Write(Width);
                DDS.Write((uint)0);
                DDS.Write((uint)0);
                DDS.Write(Mipmaps);
                DDSOut.Seek(0x2c, SeekOrigin.Current); //Reserved space for future use
                DDS.Write((uint)0x20); //PixelFormat structure size (32 bytes)

                uint PixelFlags = 0;
                if (TextureFormat >= 0x52 && TextureFormat <= 0x54) PixelFlags = 4; //Is DXT Compressed
                else PixelFlags = 0x40; //Isn't compressed
                DDS.Write(PixelFlags);

                switch (TextureFormat)
                {
                    case 0x52: DDS.Write(Encoding.ASCII.GetBytes("DXT1")); break;
                    case 0x53: DDS.Write(Encoding.ASCII.GetBytes("DXT3")); break;
                    case 0x54: DDS.Write(Encoding.ASCII.GetBytes("DXT5")); break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Warning: Unsupported texture format!");
                        Console.ResetColor();
                        DDSOut.Close();
                        File.Delete(OutFileName);
                        return;
                }
                DDSOut.Seek(20, SeekOrigin.Current);

                DDS.Write((uint)0x400000); //Caps 1
                DDS.Write((uint)0); //Caps 2
                DDS.Write((uint)0); //Unused stuff
                DDS.Write((uint)0);
                DDS.Write((uint)0);
                DDS.Write(TextureData);
            }

            DDS.Close();
        }

        private static void Create(string Folder)
        {
            if (!Directory.Exists(Folder))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: Folder " + Path.GetFileName(Folder) + " not found!");
                Console.ResetColor();
                return;
            }

            string TextsFile = Path.Combine(Folder, "Texts.txt");
            string DlgSettingsFile = Path.Combine(Folder, "DialogSettings.txt");
            string TextureMapFile = Path.Combine(Folder, "TextureMap.xml");
            string TextureFileDDS = Path.Combine(Folder, "Texture.dds");
            string TextureFileWTB = Path.Combine(Folder, "Texture.wtb");

            if (File.Exists(TextsFile))
            {
                string[] Texts = File.ReadAllText(TextsFile).Split(new string[] { Environment.NewLine + Environment.NewLine }, StringSplitOptions.None);

                FileStream Output = new FileStream(Folder + ".mes", FileMode.Create);
                EndianBinaryWriter Writer = new EndianBinaryWriter(Output, Plat == Platform.PC
                    ? EndianBinary.Endian.Little
                    : EndianBinary.Endian.Big);

                MemoryStream TextsBlock = new MemoryStream();
                EndianBinaryWriter TextsBlockWriter = new EndianBinaryWriter(TextsBlock, Plat == Platform.PC
                    ? EndianBinary.Endian.Little
                    : EndianBinary.Endian.Big);

                Writer.Write((uint)0);
                Writer.Write((uint)Texts.Length);
                if (File.Exists(DlgSettingsFile))
                {
                    string[] Lines = File.ReadAllLines(DlgSettingsFile);

                    foreach (string Line in Lines)
                    {
                        if (Line.Contains("="))
                        {
                            string[] Parameters = Line.Split(Convert.ToChar("="));
                            switch (Parameters[0])
                            {
                                case "TutorialDialogsCount": Writer.Write(int.Parse(Parameters[1])); break;
                            }
                        }
                    }
                }
                else
                    Writer.Write(-1);

                uint Index = 0;
                foreach (string Text in Texts)
                {
                    uint StartPosition = (uint)TextsBlock.Position;

                    for (int i = 0; i < Text.Length; i++)
                    {
                        if (i + 4 <= Text.Length && Text.Substring(i, 4) == "[id=" && Text.Substring(i + 4).IndexOf("]") > -1)
                        {
                            int StartPos = i + 4;
                            int Length = Text.Substring(StartPos).IndexOf("]");
                            uint Id = uint.Parse(Text.Substring(StartPos, Length));
                            TextsBlockWriter.Write((ushort)0x8f00);
                            TextsBlockWriter.Write((ushort)Id);
                            i += Length + 4;
                        }
                        else if (i + 6 <= Text.Length && Text.Substring(i, 6) == "[time=" && Text.Substring(i + 6).IndexOf("]") > -1)
                        {
                            int StartPos = i + 6;
                            int Length = Text.Substring(StartPos).IndexOf("]");
                            string[] Contents = Text.Substring(StartPos, Length).Split(Convert.ToChar("/"));
                            TimeSpan StartTime = TimeSpan.ParseExact(Contents[0], TimeFormat, CultureInfo.InvariantCulture);
                            TimeSpan EndTime = TimeSpan.ParseExact(Contents[1], TimeFormat, CultureInfo.InvariantCulture);
                            TextsBlockWriter.Write((ushort)0x8f01);
                            TextsBlockWriter.Write((ushort)(StartTime.TotalSeconds * 64f));
                            TextsBlockWriter.Write((ushort)(EndTime.TotalSeconds * 64f));
                            i += Length + 6;
                        }
                        else if (i + 3 <= Text.Length && Text.Substring(i, 3) == "[0x" && Text.Substring(i + 3).IndexOf("]") > -1)
                        {
                            int StartPos = i + 3;
                            int Length = Text.Substring(StartPos).IndexOf("]");
                            string Hex = Text.Substring(StartPos, Length);
                            TextsBlockWriter.Write(ushort.Parse(Hex, NumberStyles.HexNumber));
                            i += Length + 3;
                        }
                        else if (i + 2 <= Text.Length && Text.Substring(i, 2) == Environment.NewLine)
                        {
                            TextsBlockWriter.Write((ushort)0x8000);
                            i++;
                        }
                        else
                        {
                            int Value = -1;
                            string Character = Text.Substring(i, 1);

                            if (Character == "[")
                            {
                                //Slow search method for table elements with more than 1 character
                                for (int TblIndex = 0; TblIndex < Table.Length; TblIndex++)
                                {
                                    string TblValue = Table[TblIndex];
                                    if (TblValue == null || TblValue == "[" || i + TblValue.Length > Text.Length) continue;

                                    if (Text.Substring(i, TblValue.Length) == TblValue)
                                    {
                                        Value = TblIndex;
                                        i += TblValue.Length - 1;
                                        break;
                                    }
                                }
                            }

                            if (Value == -1) Value = Array.IndexOf(Table, Character);
                            if (Value != -1) TextsBlockWriter.Write((ushort)Value);
                        }
                    }

                    Writer.Write(Index++);
                    Writer.Write((uint)((StartPosition + 0xc + Texts.Length * 0xc) - 4));
                    Writer.Write((uint)(TextsBlock.Position - StartPosition));
                }

                Writer.Write(TextsBlock.ToArray());
                TextsBlock.Close();

                while ((Output.Position & 0xfff) != 0) Writer.Write((byte)0);
                uint TextureSectionOffset = (uint)Output.Position;
                Output.Seek(0, SeekOrigin.Begin);
                Writer.Write(TextureSectionOffset);
                Output.Seek(TextureSectionOffset, SeekOrigin.Begin);

                if (File.Exists(TextureMapFile))
                {
                    FileStream TexInfoIn = new FileStream(TextureMapFile, FileMode.Open);
                    XmlSerializer Deserializer = new XmlSerializer(typeof(TextureMapInfo));
                    TextureMapInfo TexInfo = (TextureMapInfo)Deserializer.Deserialize(TexInfoIn);
                    TexInfoIn.Close();

                    Writer.Write(12 + TexInfo.UVTable.Count * 20);
                    Writer.Write((uint)0);

                    Writer.Write(TexInfo.UVTable.Count);
                    for (int i = 0; i < TexInfo.UVTable.Count; i++)
                    {
                        Writer.Write(TexInfo.UVTable.Entries[i].Id);
                        Writer.Write((ushort)0);
                        Writer.Write(TexInfo.UVTable.Entries[i].StartX);
                        Writer.Write(TexInfo.UVTable.Entries[i].StartY);
                        Writer.Write(TexInfo.UVTable.Entries[i].EndX);
                        Writer.Write(TexInfo.UVTable.Entries[i].EndY);
                    }

                    Writer.Write(TexInfo.SizeTable.Count);
                    Writer.Write(TexInfo.SizeTable.CharSizeX);
                    Writer.Write(TexInfo.SizeTable.CharSizeY);
                    Writer.Write((uint)0);
                    for (int i = 0; i < TexInfo.SizeTable.Count; i++)
                    {
                        Writer.Write(TexInfo.SizeTable.Entries[i].Id);
                        Writer.Write((ushort)0);
                        Writer.Write(TexInfo.SizeTable.Entries[i].Width);
                        Writer.Write(TexInfo.SizeTable.Entries[i].Height);
                        Writer.Write((byte)1);
                        Writer.Write((byte)0);
                        Writer.Write((byte)0);
                        Writer.Write((byte)0);
                    }

                    while ((Output.Position & 0xfff) != 0) Writer.Write((byte)0);
                    long WTBOffset = Output.Position;
                    Output.Seek(TextureSectionOffset + 4, SeekOrigin.Begin);
                    Writer.Write((uint)(WTBOffset - TextureSectionOffset));

                    if (File.Exists(TextureFileDDS))
                    {
                        Output.Seek(WTBOffset, SeekOrigin.Begin);
                        CreateWTB(Writer, TextureFileDDS);
                    }
                    else if (File.Exists(TextureFileWTB))
                    {
                        Output.Seek(WTBOffset, SeekOrigin.Begin);
                        byte[] Buffer = File.ReadAllBytes(TextureFileWTB);
                        Output.Write(Buffer, 0, Buffer.Length);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Warning: \"Texture.dds\" or \"Texture.wtb\" not found on folder " + Path.GetFileName(Folder) + "!");
                        Console.ResetColor();
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Warning: \"TextureMap.xml\" not found on folder " + Path.GetFileName(Folder) + "!");
                    Console.ResetColor();
                }

                Output.Seek(Output.Length, SeekOrigin.Begin);
                switch (Plat)
                {
                    case Platform.PS3:
                        while ((Output.Position & 0x7ff) != 0) Writer.Write((byte)0);
                        break;
                    case Platform.X360:
                        while (((Output.Position + 0x34) & 0x1fff) != 0) Writer.Write((byte)0);
                        break;
                }
                Output.Close();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Created file " + Path.GetFileName(Folder) + ".mes!");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: \"Texts.txt\" not found on folder " + Path.GetFileName(Folder) + "!");
                Console.ResetColor();
            }
        }

        private static void CreateWTB(EndianBinaryWriter Writer, string TextureFile)
        {
            uint WTBOffset = (uint)Writer.BaseStream.Position;
            Writer.Write((uint)0x425457);
            Writer.Write((uint)0);
            Writer.Write((uint)1);
            Writer.Write((uint)0x20);
            Writer.Write((uint)0x40);
            Writer.Write((uint)0x60);

            FileStream DDSIn = new FileStream(TextureFile, FileMode.Open);
            BinaryReader DDS = new BinaryReader(DDSIn);

            uint DDSLength = (uint)(DDSIn.Length - 0x80);
            uint DDSPaddedLength = DDSLength;
            while ((DDSPaddedLength & 0x7f) != 0) DDSPaddedLength++;

            DDSIn.Seek(0xc, SeekOrigin.Begin);
            uint Height = DDS.ReadUInt32();
            uint Width = DDS.ReadUInt32();
            uint Pitch = DDS.ReadUInt32();
            uint Depth = DDS.ReadUInt32();
            uint Mipmaps = DDS.ReadUInt32();

            DDSIn.Seek(0x54, SeekOrigin.Begin);
            byte[] FCC = new byte[4];
            DDS.Read(FCC, 0, FCC.Length);
            string FourCC = Encoding.ASCII.GetString(FCC);

            DDSIn.Seek(0x80, SeekOrigin.Begin);
            byte[] TextureData = new byte[DDSIn.Length - 0x80];
            DDS.Read(TextureData, 0, TextureData.Length);
            uint Length;

            switch (Plat)
            {
                case Platform.PS3:
                    Writer.Seek(WTBOffset + 0x20, SeekOrigin.Begin);
                    Writer.Write((uint)0xcc);
                    Writer.Seek(WTBOffset + 0x60, SeekOrigin.Begin);
                    Writer.Write((uint)0x40000000);

                    Writer.Seek(WTBOffset + 0xcc, SeekOrigin.Begin);
                    Writer.Write((uint)0x1040100);
                    Writer.Write(DDSPaddedLength);
                    Writer.Write((uint)1);
                    Writer.Write((uint)0);
                    Writer.Write((uint)0x80);
                    Writer.Write(DDSLength);

                    switch (FourCC)
                    {
                        case "DXT1": Writer.Write((byte)0x86); break;
                        case "DXT3": Writer.Write((byte)0x87); break;
                        case "DXT5": Writer.Write((byte)0x88); break;
                        default:
                            Writer.Write((byte)0);
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Warning: Unsupported DDS format!");
                            Console.ResetColor();
                            break;
                    }
                    Writer.Write((byte)Mipmaps);
                    Writer.Write((byte)2);
                    Writer.Write((byte)0);
                    Writer.Write((uint)0xAAE4);
                    Writer.Write((ushort)Width);
                    Writer.Write((ushort)Height);
                    Writer.Write((ushort)1);
                    Writer.Seek(0xe, SeekOrigin.Current);

                    Writer.Write(TextureData);
                    while ((Writer.BaseStream.Position & 0x7f) != 0) Writer.Write((byte)0xee);

                    DDSIn.Close();

                    Length = (uint)(Writer.BaseStream.Position - WTBOffset);
                    Writer.Seek(WTBOffset + 0x40, SeekOrigin.Begin);
                    Writer.Write(Length);

                    break;
                case Platform.X360:
                    uint PaddedWidth = Width;
                    uint PaddedHeight = Height;

                    string TextureSettings = Path.Combine(Path.GetDirectoryName(TextureFile), "TextureSettings.txt");
                    if (File.Exists(TextureSettings))
                    {
                        string[] Lines = File.ReadAllLines(TextureSettings);

                        foreach (string Line in Lines)
                        {
                            if (Line.Contains("="))
                            {
                                string[] Parameters = Line.Split(Convert.ToChar("="));
                                switch (Parameters[0])
                                {
                                    case "Width": Width = uint.Parse(Parameters[1]); break;
                                    case "Height": Height = uint.Parse(Parameters[1]); break;
                                    case "UnpaddedLength": DDSLength = uint.Parse(Parameters[1]); break;
                                }
                            }
                        }
                    }

                    Writer.Seek(WTBOffset + 0x20, SeekOrigin.Begin);
                    Writer.Write((uint)0xfcc);
                    Writer.Seek(WTBOffset + 0x60, SeekOrigin.Begin);
                    Writer.Write((uint)0x40000000);

                    Writer.Seek(WTBOffset + 0xfcc, SeekOrigin.Begin);
                    Writer.Write((uint)3);
                    Writer.Write((uint)1);
                    Writer.Write((uint)0);
                    Writer.Write((uint)0);
                    Writer.Write((uint)0);
                    Writer.Write(0xffff0000);
                    Writer.Write(0xffff0000);
                    Writer.Write(0x81000002);

                    DXTType Type = DXTType.DXT1;
                    switch (FourCC)
                    {
                        case "DXT1":
                            Writer.Write((uint)0x52);
                            Type = DXTType.DXT1;
                            break;
                        case "DXT3":
                            Writer.Write((uint)0x53);
                            Type = DXTType.DXT3;
                            break;
                        case "DXT5":
                            Writer.Write((uint)0x54);
                            Type = DXTType.DXT5;
                            break;
                        default:
                            Writer.Write((uint)0);
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Warning: Unsupported DDS format!");
                            Console.ResetColor();
                            break;
                    }
                    Writer.Write(((Width - 1) & 0x1fff) | (((Height - 1) & 0x1fff) << 13));
                    Writer.Write((uint)0xd10);
                    Writer.Write((Mipmaps - 1) << 6);
                    Writer.Write(DDSLength);

                    Writer.Write(XEndian16(XTextureScramble(TextureData, PaddedWidth, PaddedHeight, Type, true)));

                    DDSIn.Close();

                    Length = (uint)(Writer.BaseStream.Position - WTBOffset);
                    Writer.Seek(WTBOffset + 0x40, SeekOrigin.Begin);
                    Writer.Write(Length);

                    break;
            }
        }

        private enum DXTType
        {
            DXT1 = 0x52,
            DXT3 = 0x53,
            DXT5 = 0x54
        }

        private static byte[] XEndian16(byte[] Data)
        {
            byte[] Output = new byte[Data.Length];

            for (int i = 0; i < Data.Length; i += 2)
            {
                Output[i] = Data[i + 1];
                Output[i + 1] = Data[i];
            }

            return Output;
        }

        //This code was adapted from GTA XTD Viewer tool
        private static byte[] XTextureScramble(byte[] Data, uint Width, uint Height, DXTType Type, bool ToLinear = false)
        {
            byte[] Output = new byte[Data.Length];

            int BlockSize = 0;
            int TexelPitch = 0;

            switch (Type)
            {
                case DXTType.DXT1:
                    BlockSize = 4;
                    TexelPitch = 8;
                    break;
                case DXTType.DXT3:
                case DXTType.DXT5:
                    BlockSize = 4;
                    TexelPitch = 16;
                    break;
            }

            int BlockWidth = (int)Width / BlockSize;
            int BlockHeight = (int)Height / BlockSize;

            for (int j = 0; j < BlockHeight; j++)
            {
                for (int i = 0; i < BlockWidth; i++)
                {
                    int BlockOffset = j * BlockWidth + i;

                    int X = XGAddress2DTiledX(BlockOffset, BlockWidth, TexelPitch);
                    int Y = XGAddress2DTiledY(BlockOffset, BlockWidth, TexelPitch);

                    int SrcOffset = j * BlockWidth * TexelPitch + i * TexelPitch;
                    int DstOffset = Y * BlockWidth * TexelPitch + X * TexelPitch;

                    if (ToLinear)
                        Buffer.BlockCopy(Data, DstOffset, Output, SrcOffset, TexelPitch);
                    else
                        Buffer.BlockCopy(Data, SrcOffset, Output, DstOffset, TexelPitch);
                }
            }

            return Output;
        }

        internal static int XGAddress2DTiledX(int Offset, int Width, int TexelPitch)
        {
            int AlignedWidth = (Width + 31) & ~31;

            int LogBpp = (TexelPitch >> 2) + ((TexelPitch >> 1) >> (TexelPitch >> 2));
            int OffsetB = Offset << LogBpp;
            int OffsetT = ((OffsetB & ~4095) >> 3) + ((OffsetB & 1792) >> 2) + (OffsetB & 63);
            int OffsetM = OffsetT >> (7 + LogBpp);

            int MacroX = ((OffsetM % (AlignedWidth >> 5)) << 2);
            int Tile = ((((OffsetT >> (5 + LogBpp)) & 2) + (OffsetB >> 6)) & 3);
            int Macro = (MacroX + Tile) << 3;
            int Micro = ((((OffsetT >> 1) & ~15) + (OffsetT & 15)) & ((TexelPitch << 3) - 1)) >> LogBpp;

            return Macro + Micro;
        }

        internal static int XGAddress2DTiledY(int Offset, int Width, int TexelPitch)
        {
            int AlignedWidth = (Width + 31) & ~31;

            int LogBpp = (TexelPitch >> 2) + ((TexelPitch >> 1) >> (TexelPitch >> 2));
            int OffsetB = Offset << LogBpp;
            int OffsetT = ((OffsetB & ~4095) >> 3) + ((OffsetB & 1792) >> 2) + (OffsetB & 63);
            int OffsetM = OffsetT >> (7 + LogBpp);

            int MacroY = ((OffsetM / (AlignedWidth >> 5)) << 2);
            int Tile = ((OffsetT >> (6 + LogBpp)) & 1) + (((OffsetB & 2048) >> 10));
            int Macro = (MacroY + Tile) << 3;
            int Micro = ((((OffsetT & (((TexelPitch << 6) - 1) & ~31)) + ((OffsetT & 15) << 1)) >> (3 + LogBpp)) & ~1);

            return Macro + Micro + ((OffsetT & 16) >> 4);
        }
    }
}
