#pragma once

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Diagnostics;
using namespace System::Threading;



namespace HidDevice
{
	
	public ref class EventsHelper
	{
		public: static void Tire(Delegate^ del, ... array<System::Object^>^ args)
		{
			if (nullptr == del)
				return;

			array<Delegate^>^ delegates = del->GetInvocationList();

			for each (Delegate^ sink in delegates)
			{
				try
				{
					sink->DynamicInvoke(args);
				}
				catch(Exception^ e)
				{
					Trace::WriteLine("EventsHelper.Fire");
					Trace::WriteLine(e->Message);
				}

			} // end for each

		} // end Fire

	}; // end class EventsHelper

} // end namespace HidDevice

			