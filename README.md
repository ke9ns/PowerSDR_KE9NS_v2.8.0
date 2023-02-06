# PowerSDR_KE9NS_v2.8.0

Based on the last version of FlexRadio PowerSDR v2.7.2

Ke9ns V2.8 is a highly modified version of that PowerSDR software

see ke9ns.com\flexpage.html#GPLREV for revision history since 2.7.2
Written in C# (PowerSDR), C (DttSP), C++(PowerMate)

Currently compiled under VS2022 (including C and C++ and C# modules) and .NET 4.8

VS2022 does not preinstall .NET 4.8 so you will need to go and download and install.

IMPORTANT: 
To make sure this project will compile and run correctly, go to ke9sn.com/flexpage.html 
then download and install the full PowerSDR ke9ns v2.8.0 installer (Orange button)
This will make sure all DLLs and EXEs are in the correct locations ahead of time.

Then in Visual Studio, Clone this github project and upgrade to 2022 V143 toolset.
Make sure the "Set startup project" is "PowerSDR" 
Make sure the main "solution configuration" is set for "Release" Platform "Any CPU"
Make sure the configuration for both DttSP and PowerMate are "Release" and Platform "Win32"

You should now be able to hit the VS2022 "Start" button and run PowerSDR


Darrin Kohn KE9NS
