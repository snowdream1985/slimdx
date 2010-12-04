﻿// Copyright (c) 2007-2010 SlimDX Group
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System.Collections.Generic;
using System.Xml.Linq;

namespace Generator.ObjectModel
{
	/// <summary>
	/// Implements a model of a C++ header in memory.
	/// </summary>
	class SourceModel
	{
		List<EnumElement> enums = new List<EnumElement>();
		List<StructElement> structs = new List<StructElement>();
		List<TypedefElement> typedefs = new List<TypedefElement>();
		List<InterfaceElement> interfaces = new List<InterfaceElement>();

		public string Namespace
		{
			get;
			private set;
		}

		public IEnumerable<EnumElement> Enums
		{
			get { return enums; }
		}

		public SourceModel(XElement root, string rootNamespace)
		{
			Namespace = rootNamespace;

			Build(root);
		}

		void Build(XElement element)
		{
			switch (element.Name.LocalName)
			{
				case "Decls":
				case "Extern":
					foreach (var child in element.Elements())
						Build(child);
					break;

				case "Typedef":
					BuildTypedef(element);
					break;

				case "Variable":
					BuildVariable(element);
					break;
			}
		}

		void BuildTypedef(XElement element)
		{
			var type = element.Element("Type");
			var typeBase = type.Element("Base");
			var scalar = type.Element("Scalar");

			string name = null;
			if (typeBase != null)
			{
				name = (string)typeBase.Attribute("Name");
				var enumElement = typeBase.Element("Enum");
				if (enumElement != null)
					enums.Add(new EnumElement(name, enumElement));

				var structElement = typeBase.Element("Struct");
				if (structElement != null)
					structs.Add(new StructElement(name, structElement));
			}
			else if (scalar != null)
				name = scalar.Element("Token").Value;
			else
				name = (string)type.Attribute("Name");

			var typedef = element.Element("Var");
			if (typedef != null && name != null)
			{
				// don't add typedef if it's just redeclaring the type
				var newName = (string)typedef.Attribute("Name");
				if (newName != name)
					typedefs.Add(new TypedefElement(name, newName));
			}
		}

		void BuildVariable(XElement element)
		{
			var type = element.Element("Type");
			var typeBase = type.Element("Base");
			if (typeBase == null)
				return;

			var name = (string)typeBase.Attribute("Name");
			var structElement = typeBase.Element("Struct");
			var inheritance = typeBase.Element("Inheritance");
			var declspec = typeBase.Element("DeclspecOrEmpty");
			if (structElement != null)
				interfaces.Add(new InterfaceElement(name, structElement, inheritance, declspec));
		}
	}
}