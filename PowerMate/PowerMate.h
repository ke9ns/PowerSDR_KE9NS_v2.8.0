//======================================================================================================
//======================================================================================================
// ke9ns ======================================================================================================
//======================================================================================================
//======================================================================================================

//Trace.WriteLine(slider.Value.ToString());

#define _CRT_SECURE_NO_WARNINGS

#include <string.h>
#include <stdio.h>
#include <stdlib.h>
#include <math.h>   

  
#pragma once

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Diagnostics;
using namespace System::Threading;


#include "EventsHelper.h"

namespace HidDevice
{ 
    int KBON = 0;										// 1=detect knob, 0=dont



	public ref class PowerMate
	{
		
//=======Enums ===========================================================================================

       #pragma region Enums

		private: enum class HidAttributes			// Griffin PowerMate Knob ID
		{
			VendorID		= 0x077d,
			ProductID		= 0x0410,
			VersionNumber	= 0x0311
		};

		public: enum class RotationalDirection
		{
			Left,
			Right
		};

		public: enum class ButtonState
		{
			Up,
			Down
		};      
		#pragma endregion

//======Data members============================================================================================

		#pragma region Data members

 
		public: 
			delegate void RotationHandler(int value);  // SEND TO FORM:  direction, currentrotationvalue, txsend
			delegate void ButtonHandler(ButtonState bs, int value, int value1, int value2);                               // SEND TO FORM: bs, bandsel

			delegate void SignalHandler(int value, int value1);  // send to form Signal strength : corrected for display, actual -xxxdBm


			// ^ indicates a handle to a routine.
		private:
			SignalHandler^ signalDelegate;
			RotationHandler^ rotationDelegate;				// direction, rot value, txsend, value2, value3
			ButtonHandler^ buttonDelegate;					// button state, bandsel, value1, value2
            HANDLE handleToDevice;							// device handle of the PowerMate Knob
		    Thread^ inputThread;							// inputthread routine 
		    ManualResetEvent^ exitEvent;					//

			HANDLE hPort;									// COM port handle


 //-----------------------------------------------------


			int rotvalue;
			int rotationValue;								// is value from rotationHandler
			int lastrotation;
			int rotationLowerBound;
			int rotationUpperBound;							// rotation value
     		int bandsel;									// tune step
			int bandsel1;									// band just prior to pushing knob
			int xxx;										// used to pass com port value back to this program
			int txsend;										// 1=sending CAT command 0=not
			int txsend1;
			int speed;										// counter
			int speed1;										// tune speed
			int RIT;										// 1=RIT on , 0=VFOA on
			int speed4;										// temp
			int RITFreq;									// RIT Freq value
	
	
			int qqq;										// button		
			int aaa;

			int www;										// rotation 
			int zzz;


            HWND hNWnd;										// Window Handle
 			HWND hTWnd;									    // child window handle
            DWORD threadID;									// Widnow Thread	

			int RXM;									// 1=sent request for RX meter reading, 0=nothing.
			int RXOFF;

			int S2LAST;									// last signal value

			int TIMER_CAT;

			int PTT;									// 1=TX, 0=RX


		#pragma endregion



 //ke9ns ============================================================================================================
		#pragma region Constructor

		public: PowerMate() : handleToDevice(NULL),inputThread(nullptr),exitEvent(nullptr),signalDelegate(nullptr),rotationDelegate(nullptr),buttonDelegate(nullptr), rotationValue(0) 
		{

		}

		#pragma endregion
	
//ke9ns ======Initialize============================================================================================

		#pragma region Initialize

		public: bool Initialize()
		{
			
				TIMER_CAT = 100;

			    bandsel = 2;						 // select 100hz first band
				bandsel1 = 4;
				S2LAST = 0;

				RXM = 0;

	
        //==============PowerMate====================================================
		
         array<Process^>^ powerMateProcess = Process::GetProcessesByName("PowerMate");

			if(powerMateProcess->Length > 0)
			{
				HANDLE hp = OpenProcess(SYNCHRONIZE | PROCESS_TERMINATE,FALSE,powerMateProcess[0]->Id);
				
		
				HWND hWnd = FindWindow(NULL, L"PowerMateWnd");
				
			//	Trace::WriteLine("DETECT IF PowerMate RUNNING");

				if(NULL != hWnd)
				{
				//	Trace::WriteLine("PowerMate is RUNNING");
					
					PostMessage(hWnd, WM_CLOSE,0,0);

					if( WaitForSingleObject(hp, 5000)!= WAIT_OBJECT_0)
					{
					//	Trace::WriteLine("Failure to close PowerMate.exe");
						return false;
					}
					else
					{
						//	Trace::WriteLine("PowerMate.exe has been closed");
					}
				}
			}
	


		//=======================================================================
		//=======================================================================
		//=======================================================================
		//=======================================================================
		// ke9ns INITIALIZE POWERMATE KNOB


			GUID hidClass;
			HidD_GetHidGuid(&hidClass);									// returns the device interface GUID for HIDClass devices.
			
																		// retrieves a device information set that contains all the
																		// devices of a specified class (hint: our HIDClass)
			HDEVINFO hDevInfoSet = SetupDiGetClassDevs(&hidClass,
													   NULL,					// retrieve dev info for all instances
													   NULL,				    // hWnd parent
													   DIGCF_PRESENT |			// only present devices
													   DIGCF_INTERFACEDEVICE);	// that expose hid interface

			
			
			if (INVALID_HANDLE_VALUE == hDevInfoSet)
			{
			//	Trace::WriteLine("SetupDiGetClassDevs");
				return false;
			}

			
			SP_INTERFACE_DEVICE_DATA interfaceData;												 // enumerate devices looking for hid interfaces
			interfaceData.cbSize = sizeof(interfaceData);

																							// poll device manager until no matching devices left
			for(int i = 0; SetupDiEnumDeviceInterfaces(hDevInfoSet,NULL,&hidClass,i,&interfaceData); ++i)
			{
					
					DWORD bufferLength;                                                     // retrieve buffer size for interface detail data

					                                                                         // retrieves detailed information about a specified device interface
					SetupDiGetDeviceInterfaceDetail(hDevInfoSet,&interfaceData,NULL,0,&bufferLength,NULL);

					PSP_INTERFACE_DEVICE_DETAIL_DATA interfaceDetail = (PSP_INTERFACE_DEVICE_DETAIL_DATA)new char[bufferLength];

					interfaceDetail->cbSize = sizeof(SP_INTERFACE_DEVICE_DETAIL_DATA);
			
					                                                                              // now get the interface detail data
					if( !SetupDiGetDeviceInterfaceDetail(hDevInfoSet,&interfaceData,interfaceDetail,bufferLength,NULL,NULL))
					{
						delete interfaceDetail;
						SetupDiDestroyDeviceInfoList(hDevInfoSet);
						break;
					}

         //==================================================================================================
					// - now that we have the device path, open device
					// - use FILE_FLAG_OVERLAPPED for simultaneous read/write
					// - the system does not maintain the file pointer,
					//   therefore you must pass the file position to the
					//   read and write functions in the OVERLAPPED structure
					
					HANDLE hDevice = CreateFile(interfaceDetail->DevicePath,GENERIC_READ | GENERIC_WRITE,FILE_SHARE_READ | FILE_SHARE_WRITE,
							                        NULL,OPEN_EXISTING,FILE_ATTRIBUTE_NORMAL | FILE_FLAG_OVERLAPPED ,NULL);

					if( INVALID_HANDLE_VALUE == hDevice )
					{
						delete interfaceDetail;
						continue; // keep searching
					}
					else
					{
						HIDD_ATTRIBUTES hidAttr;
						BOOLEAN result = HidD_GetAttributes(hDevice,&hidAttr);

						if (result)
						{
									// this is what we are looking for (PowerMate Knob)
							        //	VendorID		= 0x077d,
									// ProductID		= 0x0410,
									// VersionNumber	= 0x0311
									// grab the first matching device we find and return

							if( (int)HidAttributes::ProductID == hidAttr.ProductID && (int)HidAttributes::VendorID  == hidAttr.VendorID )
							{
								//Trace::WriteLine("FOUND GRIFFIN KNOB");
								handleToDevice = hDevice;

								KBON = 1;

								delete interfaceDetail;
								SetupDiDestroyDeviceInfoList(hDevInfoSet);

								inputThread = gcnew Thread( gcnew ThreadStart(this, &PowerMate::InputThread));		// THREAD for InputThread
								inputThread->Priority = ThreadPriority::Normal;
								inputThread->Start();

								return true;
							} // found knob

						} // result

					} // valid handle

				

			} // FOR LOOP

			KBON = 0;	// no knob found


			//delete interfaceDetail;
			SetupDiDestroyDeviceInfoList(hDevInfoSet);

			return false;


		} // end Initialize


		#pragma endregion

//=====Shutdown =================================================================================================

		#pragma region Shutdown
		public: void Shutdown()
		{
			
			KBON = 0; // shut down thread

			//	Trace::WriteLine("ShutDown-1 ");

			if (NULL != handleToDevice)
			{
				inputThread->Join();  // shut down
				CloseHandle(handleToDevice);     // close PowerMate	
			}

		//	MessageBox(NULL, L"MessageBox Text", L"MessageBox caption", MB_OK);

		//	Trace::WriteLine("ShutDown-5 " + KBON);
		} // shutdown


		#pragma endregion




//====LED Brightness==================================================================================================

		#pragma region LedBrightness
		public: property Byte LedBrightness
		{
			
			void set(Byte value)   // when trying to set the LED value
			{
				if( NULL != handleToDevice )
				{
					HANDLE hEvent = CreateEvent(NULL, FALSE, FALSE, NULL);

					OVERLAPPED overLap;							// async I/O sturcture
					ZeroMemory(&overLap, sizeof(overLap));		// 0 overlap
					overLap.hEvent = hEvent; 

					UCHAR reportBuffer[2];
					reportBuffer[0] = 0;		//
					reportBuffer[1] = value;    // LED brightness

					

					DWORD dwBytesTransmitted = 0;
					BOOL result = WriteFile(handleToDevice,reportBuffer,sizeof(reportBuffer),&dwBytesTransmitted,&overLap);  // write data to Knob

					DWORD dw = WaitForSingleObject(hEvent, 40); // wait  40 mSec for signal
								
					switch(dw)
					{
						case WAIT_OBJECT_0:
							break;

						case WAIT_TIMEOUT:
							break;
					}

					CloseHandle(hEvent);
					
				}
			}

			Byte get()   // when trying to read the LED value
			{ 
				UCHAR reportBuffer[7] = { 0 };

				if (NULL != handleToDevice)
				{
					BOOLEAN error = HidD_GetInputReport(handleToDevice, reportBuffer, sizeof(reportBuffer));  // read KNOB

					if (FALSE == error)
					{
						//	Trace::WriteLine("failure to get input report - brightness");	
					}
					
				
				}

				return reportBuffer[4];
			}
		}
		#pragma endregion



//====Events==================================================================================================

		#pragma region Events

		public: event RotationHandler^ RotateEvent
		{
			void add(RotationHandler^ value)
			{
				rotationDelegate += value;

			}

			void remove(RotationHandler^ value)
			{
				rotationDelegate -= value;
			}
		}



		public: event ButtonHandler^ ButtonEvent
		{
			void add(ButtonHandler^ value)
			{
				buttonDelegate += value;
			}

			void remove(ButtonHandler^ value)
			{
				buttonDelegate -= value;
			}
		}
				
	
		#pragma endregion


				
//==================================================================================================================
//==================================================================================================================
//==================================================================================================================
//==================================================================================================================
//==================================================================================================================
//==================================================================================================================
// ke9ns InputThread  is a running thread
//==================================================================================================================
//==================================================================================================================
//==================================================================================================================
//==================================================================================================================


		#pragma region InputThread

		public: void InputThread()
		{
			
			char reportBuffer[8] = {0};
			
			DWORD dwBytesRead = 0;									//
			BOOL result = FALSE;									// 

			HANDLE hEvent = CreateEvent(NULL,FALSE,FALSE,NULL);		// 
			
			OVERLAPPED overLap;										// 
			ZeroMemory( &overLap, sizeof(overLap));					// 
			overLap.hEvent = hEvent;								// 
		
			while(true)             
			{
				
				result = ReadFile(handleToDevice, reportBuffer, sizeof(reportBuffer), &dwBytesRead, &overLap);// async call
			                                                                                
				DWORD dw = WaitForSingleObject(hEvent, TIMER_CAT);  // wait for 200=200mSec (was 2 seconds)
			
				switch(dw)
				{
					case WAIT_OBJECT_0:
					{
						
						if ((reportBuffer[1] == 1) )   // test for pushbutton
						{
				
							ButtonState bs = ButtonState::Down;
							EventsHelper::Tire(buttonDelegate, bs, bandsel, qqq, aaa);  // send button state, and bandsel to form
							
							// Trace::WriteLine("DETECTED DN");

						}
						else if ( reportBuffer[2] == 0 && reportBuffer[1] == 0)  // test for knob UP
						{
							ButtonState bs = ButtonState::Up;
							EventsHelper::Tire(buttonDelegate, bs, bandsel1, qqq, aaa);

							//Trace::WriteLine("DETECTED UP");
		   			    }
		

						rotvalue = reportBuffer[2];

						if ((reportBuffer[2] != 0)  )                    // test for knob rotation
						{
							rotationValue += reportBuffer[2];

						//	Trace::WriteLine("Reportbuffer " + reportBuffer[2]);
                 	
							EventsHelper::Tire(rotationDelegate,rotvalue); // send direction, rotationvalue, txsend
							
						} // knob rotation


					} break;  // case wait_object 0


		//==================================================================
					case WAIT_TIMEOUT:
					{
						
						if( (KBON == 0))  // wait for event to finish(exitEvent->WaitOne(20,false)) ||
						{
							CloseHandle(hEvent);
							//Trace::WriteLine("hEvent closed");
							return;
						}

					} break;

				} // end switch

			} // end while

			
		} // end inputthread

#pragma endregion

//======================================================================================================

	}; // end class PowerMate

} // end namespace