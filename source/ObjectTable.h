/*
* Copyright (c) 2007-2008 SlimDX Group
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/
#pragma once

#include <unknwn.h>

namespace SlimDX
{
	ref class ComObject;

	ref class ObjectTable
	{
	private:
		value struct ObjectRecord
		{
			ComObject^ Handle;
			System::Diagnostics::StackTrace^ Source;
		};

		static ObjectTable();
		ObjectTable();

		static System::Collections::Generic::Dictionary<System::IntPtr, ObjectRecord>^ m_Table;

		static void OnExit( System::Object^ sender, System::EventArgs^ e );

	internal:
		static ComObject^ Construct( System::IntPtr nativeObject );

	public:
		static void Add( ComObject^ obj );
		static void Remove( ComObject^ obj );

		static System::String^ ReportLeaks();
	};
}