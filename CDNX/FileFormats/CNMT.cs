﻿using System;
using System.Collections.Generic;
using System.IO;

namespace CDNNX {
    internal class CNMT {

		public ulong TitleId;
		public uint TitleVersion;
		public string Type;
		public List<ContEntry> contEntries;

		private enum Types{
			SystemPrograms=1,
			SystemDataArchives=2,
			SystemUpdate=3,
			FirmwarePackageA=4,
			FirmwarePackageB=5,
			RegularApplication=0x80,
			UpdateTitle=0x81,
			AddonContent=0x82,
			DeltaTitle=0x83
		};

		public CNMT(BinaryReader br) {
			TitleId = br.ReadUInt64();
			TitleVersion = br.ReadUInt32();
			Type = ((Types)br.ReadByte()).ToString();
			br.ReadByte();
			ushort offset = br.ReadUInt16();
			ushort contCnt = br.ReadUInt16();
			ushort metaCnt = br.ReadUInt16();
			br.ReadBytes(12+offset);
			contEntries = new List<ContEntry>();
			for (int i = 0; i < contCnt; i++) {
				ContEntry entry = new ContEntry(br);
				contEntries.Add(entry);
			}
			br.Close();
		}
	}

    internal class ContEntry {

		public string Hash;
		public string NcaId;
		public uint Size;
		public string Type;

		private string[] types = { "Meta", "Program", "Data", "Control", "Offline-Manual", "Manual" };

		public ContEntry(BinaryReader br) {
			Hash = BitConverter.ToString(br.ReadBytes(32)).Replace("-", string.Empty);
			NcaId = BitConverter.ToString(br.ReadBytes(16)).Replace("-", string.Empty);
			Size = BitConverter.ToUInt32(br.ReadBytes(6), 0);
			Type = types[br.ReadByte()];
			br.ReadByte();
		}
	}
}
