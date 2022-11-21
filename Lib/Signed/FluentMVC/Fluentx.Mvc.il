
//  Microsoft (R) .NET Framework IL Disassembler.  Version 4.0.30319.17929
//  Copyright (c) Microsoft Corporation.  All rights reserved.


// ----- DOS Header:
// Magic:                      0x5a4d
// Bytes on last page:         0x0090
// Pages in file:              0x0003
// Relocations:                0x0000
// Size of header (paragraphs):0x0004
// Min extra paragraphs:       0x0000
// Max extra paragraphs:       0xffff
// Initial (relative) SS:      0x0000
// Initial SP:                 0x00b8
// Checksum:                   0x0000
// Initial IP:                 0x0000
// Initial (relative) CS:      0x0000
// File addr. of reloc table:  0x0040
// Overlay number:             0x0000
// OEM identifier:             0x0000
// OEM info:                   0x0000
// File addr. of COFF header:  0x0080
// ----- COFF/PE Headers:
// Signature:                  0x00004550
// ----- COFF Header:
// Machine:                    0x014c
// Number of sections:         0x0003
// Time-date stamp:            0x54d8de66
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2102
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x0b
// Minor linker version:           0x00
// Size of code:                   0x00001800
// Size of init.data:              0x00000600
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x0000373e
// Base of code:                   0x00002000
// Base of data:                   0x00004000
// Image base:                     0x10000000
// Section alignment:              0x00002000
// File alignment:                 0x00000200
// Major OS version:               0x0004
// Minor OS version:               0x0000
// Major image version:            0x0000
// Minor image version:            0x0000
// Major subsystem version:        0x0006
// Minor subsystem version:        0x0000
// Size of image:                  0x00008000
// Size of headers:                0x00000200
// Checksum:                       0x00000000
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x000036e8 [0x00000053] address [size] of Import Directory:          
// 0x00004000 [0x00000310] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00000000 [0x00000000] address [size] of Security Directory:        
// 0x00006000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x000035b0 [0x0000001c] address [size] of Debug Directory:           
// 0x00000000 [0x00000000] address [size] of Architecture Specific:     
// 0x00000000 [0x00000000] address [size] of Global Pointer:            
// 0x00000000 [0x00000000] address [size] of TLS Directory:             
// 0x00000000 [0x00000000] address [size] of Load Config Directory:     
// 0x00000000 [0x00000000] address [size] of Bound Import Directory:    
// 0x00002000 [0x00000008] address [size] of Import Address Table:      
// 0x00000000 [0x00000000] address [size] of Delay Load IAT:            
// 0x00002008 [0x00000048] address [size] of CLR Header:                


// Image sections:
//              .text
//              0x00001744 Virtual Size
//              0x00002000 Virtual Address
//              0x00001800 Size of Raw Data
//              0x00000200 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x60000020 Characteristics
//                         CNT_CODE
//                         MEM_EXECUTE
//                         MEM_READ

//              .rsrc
//              0x00000310 Virtual Size
//              0x00004000 Virtual Address
//              0x00000400 Size of Raw Data
//              0x00001a00 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x40000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_READ

//              .reloc
//              0x0000000c Virtual Size
//              0x00006000 Virtual Address
//              0x00000200 Size of Raw Data
//              0x00001e00 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x42000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_DISCARDABLE
//                         MEM_READ

// Base Relocation Table
//              0x00003000 Page RVA
//              0x0000000c Block Size
//              0x00000002 Number of Entries
//              Entry 1: Type 0x3 Offset 0x00000740
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x0000372e Import Name Table
//              0          Time Date Stamp
//              0          Index of First Forwarder Reference
//
//              0x0000  _CorDllMain

// Delay Load Import Address Table
// No data.

// Entry point code:
//FF 25 00 20 00 10 


// ----- CLR Header:
// Header size:                        0x00000048
// Major runtime version:              0x0002
// Minor runtime version:              0x0005
// 0x00002308 [0x000012a8] address [size] of Metadata Directory:        
// Flags:                              0x00000001
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x00000000 [0x00000000] address [size] of Strong Name Signature:     
// 0x00000000 [0x00000000] address [size] of CodeManager Table:         
// 0x00000000 [0x00000000] address [size] of VTableFixups Directory:    
// 0x00000000 [0x00000000] address [size] of Export Address Table:      
// 0x00000000 [0x00000000] address [size] of Precompile Header:         

// Metadata Header
//    Storage Signature:
//              0x424a5342 Signature
//                  0x0001 Major Version
//                  0x0001 Minor Version
//              0x00000000 Extra Data Offset
//              0x0000000c Version String Length
//              'v4.0.30319' Version String
//    Storage Header:
//                    0x00 Flags
//                  0x0005 Number of Streams
//    Stream 1:
//              0x0000006c Offset
//              0x000005ec Size
//              '#~' Name
//    Stream 2:
//              0x00000658 Offset
//              0x000007e4 Size
//              '#Strings' Name
//    Stream 3:
//              0x00000e3c Offset
//              0x00000190 Size
//              '#US' Name
//    Stream 4:
//              0x00000fcc Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00000fdc Offset
//              0x000002cc Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x0000020909a21557 MaskValid
//      0x000016003325fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 8192
// PE header size       : 512 (496 used)    ( 6.25%)
// PE additional info   : 987               (12.05%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.88%)
// CLR meta-data size  : 4776               (58.30%)
// CLR additional info : 0                  ( 0.00%)
// CLR method headers  : 97                 ( 1.18%)
// Managed code         : 590               ( 7.20%)
// Data                 : 1536              (18.75%)
// Unaccounted          : -378              (-4.61%)

// Num.of PE sections   : 3
//   .text    - 6144
//   .rsrc    - 1024
//   .reloc   - 512

// CLR meta-data size  : 4776
//   Module        -    1 (10 bytes)
//   TypeDef       -    6 (84 bytes)      0 interfaces, 0 explicit layout
//   TypeRef       -   53 (318 bytes)
//   MethodDef     -   15 (210 bytes)     0 abstract, 0 native, 15 bodies
//   FieldDef      -    3 (18 bytes)      0 constant
//   MemberRef     -   61 (366 bytes)
//   ParamDef      -   14 (84 bytes)
//   CustomAttribute-   25 (150 bytes)
//   StandAloneSig -    3 (6 bytes)
//   PropertyMap   -    1 (4 bytes)
//   Property      -    2 (12 bytes)
//   MethodSemantic-    4 (24 bytes)
//   TypeSpec      -    6 (12 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    5 (100 bytes)
//   NestedClass   -    1 (4 bytes)
//   Strings       -  2018 bytes
//   Blobs         -   716 bytes
//   UserStrings   -   400 bytes
//   Guids         -    16 bytes
//   Uncategorized -   202 bytes

// CLR method headers : 97
//   Num.of method bodies  - 15
//   Num.of fat headers    - 3
//   Num.of tiny headers   - 12
//   Num.of fat sections   - 0
//   Num.of small sections - 2

// Managed code : 590
//   Ave method size - 39
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class Extensions                     (public) (abstract) (auto) (ansi) (sealed) 
// Class JsonBinderAttribute            (public) (auto) (ansi) 
// Class JsonModelBinder                (public) (auto) (ansi) 
// Class RedirectAndPostActionResult    (public) (auto) (ansi) 
// Class <>c__DisplayClass1             (auto) (ansi) (sealed) (nested private) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ mscorlib
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 4:0:0:0
}
.assembly extern /*23000002*/ System.Web.Mvc
{
  .publickeytoken = (31 BF 38 56 AD 36 4E 35 )                         // 1.8V.6N5
  .ver 5:2:2:0
}
.assembly extern /*23000003*/ System.Web
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 4:0:0:0
}
.assembly extern /*23000004*/ System
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 4:0:0:0
}
.assembly extern /*23000005*/ System.Web.Extensions
{
  .publickeytoken = (31 BF 38 56 AD 36 4E 35 )                         // 1.8V.6N5
  .ver 4:0:0:0
}
.assembly /*20000001*/ Fluentx.Mvc
{
  .custom /*0C000003:0A000003*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000C*/::.ctor(string) /* 0A000003 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000004:0A000004*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyConfigurationAttribute/*0100000D*/::.ctor(string) /* 0A000004 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000005:0A000005*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000E*/::.ctor(string) /* 0A000005 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000006:0A000011*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*0100001B*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .custom /*0C000007:0A000007*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*01000010*/::.ctor(string) /* 0A000007 */ = ( 01 00 12 43 6F 70 79 72 69 67 68 74 20 C2 A9 20   // ...Copyright .. 
                                                                                                                                                               20 32 30 31 35 00 00 )                            //  2015..
  .custom /*0C000008:0A000008*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyTrademarkAttribute/*01000011*/::.ctor(string) /* 0A000008 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000009:0A000010*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute/*0100001A*/::.ctor() /* 0A000010 */ = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
                                                                                                                                                                          63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )       // ceptionThrows.
  .custom /*0C00000A:0A000002*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyTitleAttribute/*0100000B*/::.ctor(string) /* 0A000002 */ = ( 01 00 0B 46 6C 75 65 6E 74 78 2E 4D 76 63 00 00 ) // ...Fluentx.Mvc..
  .custom /*0C00000B:0A000006*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyProductAttribute/*0100000F*/::.ctor(string) /* 0A000006 */ = ( 01 00 0B 46 6C 75 65 6E 74 78 2E 4D 76 63 00 00 ) // ...Fluentx.Mvc..
  .custom /*0C00000C:0A000001*/ instance void [mscorlib/*23000001*/]System.Runtime.Versioning.TargetFrameworkAttribute/*0100000A*/::.ctor(string) /* 0A000001 */ = ( 01 00 1A 2E 4E 45 54 46 72 61 6D 65 77 6F 72 6B   // ....NETFramework
                                                                                                                                                                     2C 56 65 72 73 69 6F 6E 3D 76 34 2E 35 01 00 54   // ,Version=v4.5..T
                                                                                                                                                                     0E 14 46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C   // ..FrameworkDispl
                                                                                                                                                                     61 79 4E 61 6D 65 12 2E 4E 45 54 20 46 72 61 6D   // ayName..NET Fram
                                                                                                                                                                     65 77 6F 72 6B 20 34 2E 35 )                      // ework 4.5
  .custom /*0C00000D:0A00000A*/ instance void [mscorlib/*23000001*/]System.Runtime.InteropServices.ComVisibleAttribute/*01000013*/::.ctor(bool) /* 0A00000A */ = ( 01 00 00 00 00 ) 
  .custom /*0C00000E:0A00000B*/ instance void [mscorlib/*23000001*/]System.Runtime.InteropServices.GuidAttribute/*01000014*/::.ctor(string) /* 0A00000B */ = ( 01 00 24 63 32 38 38 39 34 30 33 2D 32 34 30 62   // ..$c2889403-240b
                                                                                                                                                               2D 34 65 66 31 2D 39 61 37 62 2D 30 63 39 66 36   // -4ef1-9a7b-0c9f6
                                                                                                                                                               32 65 66 36 37 62 30 00 00 )                      // 2ef67b0..
  .custom /*0C00000F:0A00000D*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*01000016*/::.ctor(string) /* 0A00000D */ = ( 01 00 05 31 2E 30 2E 34 00 00 )                   // ...1.0.4..

  // --- The following custom attribute is added automatically, do not uncomment -------
  //  .custom /*0C000010:0A00000E*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000017*/::.ctor(valuetype [mscorlib/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000017*//DebuggingModes/*01000018*/) /* 0A00000E */ = ( 01 00 02 00 00 00 00 00 ) 

  .custom /*0C000011:0A00000F*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilationRelaxationsAttribute/*01000019*/::.ctor(int32) /* 0A00000F */ = ( 01 00 08 00 00 00 00 00 ) 
  .hash algorithm 0x00008004
  .ver 1:0:4:0
}
.module Fluentx.Mvc.dll
// MVID: {9AFC9FEF-8DC3-4197-90AD-D4AFF9054F8F}
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000001    //  ILONLY
// Image base: 0x00BA0000


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ public abstract auto ansi sealed beforefieldinit Fluentx.Mvc.Extensions
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C000013:0A000011*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*0100001B*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .method /*06000001*/ public hidebysig static 
          class Fluentx.Mvc.RedirectAndPostActionResult/*02000005*/ 
          RedirectAndPost(class [System.Web.Mvc/*23000002*/]System.Web.Mvc.Controller/*01000005*/ controller,
                          string url,
                          class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000006*/<string,object> postData) cil managed
  // SIG: 00 03 12 14 12 15 0E 15 12 19 02 0E 1C
  {
    .custom /*0C000001:0A000011*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*0100001B*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2050
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 04   |                  */ ldarg.2
    IL_0002:  /* 73   | (06)00000A       */ newobj     instance void Fluentx.Mvc.RedirectAndPostActionResult/*02000005*/::.ctor(string,
                                                                                                                                class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000006*/<string,object>) /* 0600000A */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method Extensions::RedirectAndPost

} // end of class Fluentx.Mvc.Extensions

.class /*02000003*/ public auto ansi beforefieldinit Fluentx.Mvc.JsonBinderAttribute
       extends [System.Web.Mvc/*23000002*/]System.Web.Mvc.CustomModelBinderAttribute/*01000002*/
{
  .custom /*0C000014:0A000012*/ instance void [mscorlib/*23000001*/]System.AttributeUsageAttribute/*0100001C*/::.ctor(valuetype [mscorlib/*23000001*/]System.AttributeTargets/*0100001D*/) /* 0A000012 */ = ( 01 00 00 08 00 00 00 00 ) 
  .method /*06000002*/ public hidebysig virtual 
          instance class [System.Web.Mvc/*23000002*/]System.Web.Mvc.IModelBinder/*01000007*/ 
          GetBinder() cil managed
  // SIG: 20 00 12 1D
  {
    // Method begins at RVA 0x2059
    // Code size       6 (0x6)
    .maxstack  8
    IL_0000:  /* 73   | (06)000005       */ newobj     instance void Fluentx.Mvc.JsonModelBinder/*02000004*/::.ctor() /* 06000005 */
    IL_0005:  /* 2A   |                  */ ret
  } // end of method JsonBinderAttribute::GetBinder

  .method /*06000003*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2060
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000013       */ call       instance void [System.Web.Mvc/*23000002*/]System.Web.Mvc.CustomModelBinderAttribute/*01000002*/::.ctor() /* 0A000013 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method JsonBinderAttribute::.ctor

} // end of class Fluentx.Mvc.JsonBinderAttribute

.class /*02000004*/ public auto ansi beforefieldinit Fluentx.Mvc.JsonModelBinder
       extends [System.Web.Mvc/*23000002*/]System.Web.Mvc.DefaultModelBinder/*01000003*/
{
  .class /*02000006*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass1'
         extends [mscorlib/*23000001*/]System.Object/*01000001*/
  {
    .custom /*0C000016:0A00002E*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000031*/::.ctor() /* 0A00002E */ = ( 01 00 00 00 ) 
    .field /*04000003*/ public object model
    .method /*0600000E*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x2068
      // Code size       7 (0x7)
      .maxstack  8
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 28   | (0A)000014       */ call       instance void [mscorlib/*23000001*/]System.Object/*01000001*/::.ctor() /* 0A000014 */
      IL_0006:  /* 2A   |                  */ ret
    } // end of method '<>c__DisplayClass1'::.ctor

    .method /*0600000F*/ public hidebysig 
            instance object  '<BindModel>b__0'() cil managed
    // SIG: 20 00 1C
    {
      // Method begins at RVA 0x2070
      // Code size       7 (0x7)
      .maxstack  8
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 7B   | (04)000003       */ ldfld      object Fluentx.Mvc.JsonModelBinder/*02000004*//'<>c__DisplayClass1'/*02000006*/::model /* 04000003 */
      IL_0006:  /* 2A   |                  */ ret
    } // end of method '<>c__DisplayClass1'::'<BindModel>b__0'

  } // end of class '<>c__DisplayClass1'

  .method /*06000004*/ public hidebysig virtual 
          instance object  BindModel(class [System.Web.Mvc/*23000002*/]System.Web.Mvc.ControllerContext/*01000008*/ controllerContext,
                                     class [System.Web.Mvc/*23000002*/]System.Web.Mvc.ModelBindingContext/*01000009*/ bindingContext) cil managed
  // SIG: 20 02 1C 12 21 12 25
  {
    // Method begins at RVA 0x2078
    // Code size       242 (0xf2)
    .maxstack  4
    .locals /*11000001*/ init (string V_0,
             class [System.Web.Extensions/*23000005*/]System.Web.Script.Serialization.JavaScriptSerializer/*01000022*/ V_1,
             class [System.Web.Mvc/*23000002*/]System.Web.Mvc.ModelMetadata/*01000027*/ V_2,
             class [System.Web.Mvc/*23000002*/]System.Web.Mvc.ModelValidator/*01000028*/ V_3,
             class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000029*/<class [System.Web.Mvc/*23000002*/]System.Web.Mvc.ModelValidationResult/*0100002A*/> V_4,
             class [System.Web.Mvc/*23000002*/]System.Web.Mvc.ModelValidationResult/*0100002A*/ V_5,
             class Fluentx.Mvc.JsonModelBinder/*02000004*//'<>c__DisplayClass1'/*02000006*/ V_6,
             class [mscorlib/*23000001*/]System.Exception/*01000030*/ V_7,
             object V_8,
             class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerator`1/*0100002B*/<class [System.Web.Mvc/*23000002*/]System.Web.Mvc.ModelValidationResult/*0100002A*/> V_9)
    .try
    {
      IL_0000:  /* 03   |                  */ ldarg.1
      IL_0001:  /* 6F   | (0A)000015       */ callvirt   instance class [System.Web/*23000003*/]System.Web.HttpContextBase/*0100001E*/ [System.Web.Mvc/*23000002*/]System.Web.Mvc.ControllerContext/*01000008*/::get_HttpContext() /* 0A000015 */
      IL_0006:  /* 6F   | (0A)000016       */ callvirt   instance class [System.Web/*23000003*/]System.Web.HttpRequestBase/*0100001F*/ [System.Web/*23000003*/]System.Web.HttpContextBase/*0100001E*/::get_Request() /* 0A000016 */
      IL_000b:  /* 6F   | (0A)000017       */ callvirt   instance class [System/*23000004*/]System.Collections.Specialized.NameValueCollection/*01000020*/ [System.Web/*23000003*/]System.Web.HttpRequestBase/*0100001F*/::get_Form() /* 0A000017 */
      IL_0010:  /* 04   |                  */ ldarg.2
      IL_0011:  /* 6F   | (0A)000018       */ callvirt   instance string [System.Web.Mvc/*23000002*/]System.Web.Mvc.ModelBindingContext/*01000009*/::get_ModelName() /* 0A000018 */
      IL_0016:  /* 6F   | (0A)000019       */ callvirt   instance string [System/*23000004*/]System.Collections.Specialized.NameValueCollection/*01000020*/::get_Item(string) /* 0A000019 */
      IL_001b:  /* 0A   |                  */ stloc.0
      IL_001c:  /* 06   |                  */ ldloc.0
      IL_001d:  /* 28   | (0A)00001A       */ call       bool [mscorlib/*23000001*/]System.String/*01000021*/::IsNullOrEmpty(string) /* 0A00001A */
      IL_0022:  /* 2C   | 08               */ brfalse.s  IL_002c

      IL_0024:  /* 14   |                  */ ldnull
      IL_0025:  /* 13   | 08               */ stloc.s    V_8
      IL_0027:  /* DD   | C3000000         */ leave      IL_00ef

      IL_002c:  /* 73   | (06)00000E       */ newobj     instance void Fluentx.Mvc.JsonModelBinder/*02000004*//'<>c__DisplayClass1'/*02000006*/::.ctor() /* 0600000E */
      IL_0031:  /* 13   | 06               */ stloc.s    V_6
      IL_0033:  /* 73   | (0A)00001B       */ newobj     instance void [System.Web.Extensions/*23000005*/]System.Web.Script.Serialization.JavaScriptSerializer/*01000022*/::.ctor() /* 0A00001B */
      IL_0038:  /* 0B   |                  */ stloc.1
      IL_0039:  /* 11   | 06               */ ldloc.s    V_6
      IL_003b:  /* 07   |                  */ ldloc.1
      IL_003c:  /* 06   |                  */ ldloc.0
      IL_003d:  /* 04   |                  */ ldarg.2
      IL_003e:  /* 6F   | (0A)00001C       */ callvirt   instance class [mscorlib/*23000001*/]System.Type/*01000023*/ [System.Web.Mvc/*23000002*/]System.Web.Mvc.ModelBindingContext/*01000009*/::get_ModelType() /* 0A00001C */
      IL_0043:  /* 6F   | (0A)00001D       */ callvirt   instance object [System.Web.Extensions/*23000005*/]System.Web.Script.Serialization.JavaScriptSerializer/*01000022*/::Deserialize(string,
                                                                                                                                                                                          class [mscorlib/*23000001*/]System.Type/*01000023*/) /* 0A00001D */
      IL_0048:  /* 7D   | (04)000003       */ stfld      object Fluentx.Mvc.JsonModelBinder/*02000004*//'<>c__DisplayClass1'/*02000006*/::model /* 04000003 */
      IL_004d:  /* 28   | (0A)00001E       */ call       class [System.Web.Mvc/*23000002*/]System.Web.Mvc.ModelMetadataProvider/*01000025*/ [System.Web.Mvc/*23000002*/]System.Web.Mvc.ModelMetadataProviders/*01000024*/::get_Current() /* 0A00001E */
      IL_0052:  /* 11   | 06               */ ldloc.s    V_6
      IL_0054:  /* FE06 | (06)00000F       */ ldftn      instance object Fluentx.Mvc.JsonModelBinder/*02000004*//'<>c__DisplayClass1'/*02000006*/::'<BindModel>b__0'() /* 0600000F */
      IL_005a:  /* 73   | (0A)00001F       */ newobj     instance void class [mscorlib/*23000001*/]System.Func`1/*01000026*/<object>/*1B000001*/::.ctor(object,
                                                                                                                                                        native int) /* 0A00001F */
      IL_005f:  /* 04   |                  */ ldarg.2
      IL_0060:  /* 6F   | (0A)00001C       */ callvirt   instance class [mscorlib/*23000001*/]System.Type/*01000023*/ [System.Web.Mvc/*23000002*/]System.Web.Mvc.ModelBindingContext/*01000009*/::get_ModelType() /* 0A00001C */
      IL_0065:  /* 6F   | (0A)000020       */ callvirt   instance class [System.Web.Mvc/*23000002*/]System.Web.Mvc.ModelMetadata/*01000027*/ [System.Web.Mvc/*23000002*/]System.Web.Mvc.ModelMetadataProvider/*01000025*/::GetMetadataForType(class [mscorlib/*23000001*/]System.Func`1/*01000026*/<object>,
                                                                                                                                                                                                                                              class [mscorlib/*23000001*/]System.Type/*01000023*/) /* 0A000020 */
      IL_006a:  /* 0C   |                  */ stloc.2
      IL_006b:  /* 08   |                  */ ldloc.2
      IL_006c:  /* 03   |                  */ ldarg.1
      IL_006d:  /* 28   | (0A)000021       */ call       class [System.Web.Mvc/*23000002*/]System.Web.Mvc.ModelValidator/*01000028*/ [System.Web.Mvc/*23000002*/]System.Web.Mvc.ModelValidator/*01000028*/::GetModelValidator(class [System.Web.Mvc/*23000002*/]System.Web.Mvc.ModelMetadata/*01000027*/,
                                                                                                                                                                                                                              class [System.Web.Mvc/*23000002*/]System.Web.Mvc.ControllerContext/*01000008*/) /* 0A000021 */
      IL_0072:  /* 0D   |                  */ stloc.3
      IL_0073:  /* 09   |                  */ ldloc.3
      IL_0074:  /* 14   |                  */ ldnull
      IL_0075:  /* 6F   | (0A)000022       */ callvirt   instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000029*/<class [System.Web.Mvc/*23000002*/]System.Web.Mvc.ModelValidationResult/*0100002A*/> [System.Web.Mvc/*23000002*/]System.Web.Mvc.ModelValidator/*01000028*/::Validate(object) /* 0A000022 */
      IL_007a:  /* 13   | 04               */ stloc.s    V_4
      IL_007c:  /* 11   | 04               */ ldloc.s    V_4
      IL_007e:  /* 6F   | (0A)000023       */ callvirt   instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerator`1/*0100002B*/<!0> class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000029*/<class [System.Web.Mvc/*23000002*/]System.Web.Mvc.ModelValidationResult/*0100002A*/>/*1B000002*/::GetEnumerator() /* 0A000023 */
      IL_0083:  /* 13   | 09               */ stloc.s    V_9
      .try
      {
        IL_0085:  /* 2B   | 22               */ br.s       IL_00a9

        IL_0087:  /* 11   | 09               */ ldloc.s    V_9
        IL_0089:  /* 6F   | (0A)000024       */ callvirt   instance !0 class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerator`1/*0100002B*/<class [System.Web.Mvc/*23000002*/]System.Web.Mvc.ModelValidationResult/*0100002A*/>/*1B000003*/::get_Current() /* 0A000024 */
        IL_008e:  /* 13   | 05               */ stloc.s    V_5
        IL_0090:  /* 04   |                  */ ldarg.2
        IL_0091:  /* 6F   | (0A)000025       */ callvirt   instance class [System.Web.Mvc/*23000002*/]System.Web.Mvc.ModelStateDictionary/*0100002C*/ [System.Web.Mvc/*23000002*/]System.Web.Mvc.ModelBindingContext/*01000009*/::get_ModelState() /* 0A000025 */
        IL_0096:  /* 11   | 05               */ ldloc.s    V_5
        IL_0098:  /* 6F   | (0A)000026       */ callvirt   instance string [System.Web.Mvc/*23000002*/]System.Web.Mvc.ModelValidationResult/*0100002A*/::get_MemberName() /* 0A000026 */
        IL_009d:  /* 11   | 05               */ ldloc.s    V_5
        IL_009f:  /* 6F   | (0A)000027       */ callvirt   instance string [System.Web.Mvc/*23000002*/]System.Web.Mvc.ModelValidationResult/*0100002A*/::get_Message() /* 0A000027 */
        IL_00a4:  /* 6F   | (0A)000028       */ callvirt   instance void [System.Web.Mvc/*23000002*/]System.Web.Mvc.ModelStateDictionary/*0100002C*/::AddModelError(string,
                                                                                                                                                                    string) /* 0A000028 */
        IL_00a9:  /* 11   | 09               */ ldloc.s    V_9
        IL_00ab:  /* 6F   | (0A)000029       */ callvirt   instance bool [mscorlib/*23000001*/]System.Collections.IEnumerator/*0100002D*/::MoveNext() /* 0A000029 */
        IL_00b0:  /* 2D   | D5               */ brtrue.s   IL_0087

        IL_00b2:  /* DE   | 0C               */ leave.s    IL_00c0

      }  // end .try
      finally
      {
        IL_00b4:  /* 11   | 09               */ ldloc.s    V_9
        IL_00b6:  /* 2C   | 07               */ brfalse.s  IL_00bf

        IL_00b8:  /* 11   | 09               */ ldloc.s    V_9
        IL_00ba:  /* 6F   | (0A)00002A       */ callvirt   instance void [mscorlib/*23000001*/]System.IDisposable/*0100002E*/::Dispose() /* 0A00002A */
        IL_00bf:  /* DC   |                  */ endfinally
      }  // end handler
      // HEX: 02 00 85 00 2F B4 00 0C 00 00 00 00
      IL_00c0:  /* 11   | 06               */ ldloc.s    V_6
      IL_00c2:  /* 7B   | (04)000003       */ ldfld      object Fluentx.Mvc.JsonModelBinder/*02000004*//'<>c__DisplayClass1'/*02000006*/::model /* 04000003 */
      IL_00c7:  /* 13   | 08               */ stloc.s    V_8
      IL_00c9:  /* DE   | 24               */ leave.s    IL_00ef

    }  // end .try
    catch [mscorlib/*23000001*/]System.Exception/*01000030*/ 
    {
      IL_00cb:  /* 13   | 07               */ stloc.s    V_7
      IL_00cd:  /* 04   |                  */ ldarg.2
      IL_00ce:  /* 6F   | (0A)000025       */ callvirt   instance class [System.Web.Mvc/*23000002*/]System.Web.Mvc.ModelStateDictionary/*0100002C*/ [System.Web.Mvc/*23000002*/]System.Web.Mvc.ModelBindingContext/*01000009*/::get_ModelState() /* 0A000025 */
      IL_00d3:  /* 04   |                  */ ldarg.2
      IL_00d4:  /* 6F   | (0A)00001C       */ callvirt   instance class [mscorlib/*23000001*/]System.Type/*01000023*/ [System.Web.Mvc/*23000002*/]System.Web.Mvc.ModelBindingContext/*01000009*/::get_ModelType() /* 0A00001C */
      IL_00d9:  /* 6F   | (0A)00002B       */ callvirt   instance string [mscorlib/*23000001*/]System.Reflection.MemberInfo/*0100002F*/::get_Name() /* 0A00002B */
      IL_00de:  /* 11   | 07               */ ldloc.s    V_7
      IL_00e0:  /* 6F   | (0A)00002C       */ callvirt   instance string [mscorlib/*23000001*/]System.Exception/*01000030*/::get_Message() /* 0A00002C */
      IL_00e5:  /* 6F   | (0A)000028       */ callvirt   instance void [System.Web.Mvc/*23000002*/]System.Web.Mvc.ModelStateDictionary/*0100002C*/::AddModelError(string,
                                                                                                                                                                  string) /* 0A000028 */
      IL_00ea:  /* 14   |                  */ ldnull
      IL_00eb:  /* 13   | 08               */ stloc.s    V_8
      IL_00ed:  /* DE   | 00               */ leave.s    IL_00ef

    }  // end handler
    // HEX: 00 00 00 00 CB CB 00 24 30 00 00 01
    IL_00ef:  /* 11   | 08               */ ldloc.s    V_8
    IL_00f1:  /* 2A   |                  */ ret
  } // end of method JsonModelBinder::BindModel

  .method /*06000005*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2194
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00002D       */ call       instance void [System.Web.Mvc/*23000002*/]System.Web.Mvc.DefaultModelBinder/*01000003*/::.ctor() /* 0A00002D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method JsonModelBinder::.ctor

} // end of class Fluentx.Mvc.JsonModelBinder

.class /*02000005*/ public auto ansi beforefieldinit Fluentx.Mvc.RedirectAndPostActionResult
       extends [System.Web.Mvc/*23000002*/]System.Web.Mvc.ActionResult/*01000004*/
{
  .field /*04000001*/ private string '<Url>k__BackingField'
  .custom /*0C000002:0A00002E*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000031*/::.ctor() /* 0A00002E */ = ( 01 00 00 00 ) 
  .field /*04000002*/ private class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000006*/<string,object> '<PostData>k__BackingField'
  .custom /*0C000012:0A00002E*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000031*/::.ctor() /* 0A00002E */ = ( 01 00 00 00 ) 
  .method /*06000006*/ public hidebysig specialname 
          instance string  get_Url() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000015:0A00002E*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000031*/::.ctor() /* 0A00002E */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x219c
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (04)000001       */ ldfld      string Fluentx.Mvc.RedirectAndPostActionResult/*02000005*/::'<Url>k__BackingField' /* 04000001 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method RedirectAndPostActionResult::get_Url

  .method /*06000007*/ public hidebysig specialname 
          instance void  set_Url(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000017:0A00002E*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000031*/::.ctor() /* 0A00002E */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21a4
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (04)000001       */ stfld      string Fluentx.Mvc.RedirectAndPostActionResult/*02000005*/::'<Url>k__BackingField' /* 04000001 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method RedirectAndPostActionResult::set_Url

  .method /*06000008*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000006*/<string,object> 
          get_PostData() cil managed
  // SIG: 20 00 15 12 19 02 0E 1C
  {
    .custom /*0C000018:0A00002E*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000031*/::.ctor() /* 0A00002E */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21ad
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (04)000002       */ ldfld      class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000006*/<string,object> Fluentx.Mvc.RedirectAndPostActionResult/*02000005*/::'<PostData>k__BackingField' /* 04000002 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method RedirectAndPostActionResult::get_PostData

  .method /*06000009*/ public hidebysig specialname 
          instance void  set_PostData(class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000006*/<string,object> 'value') cil managed
  // SIG: 20 01 01 15 12 19 02 0E 1C
  {
    .custom /*0C000019:0A00002E*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000031*/::.ctor() /* 0A00002E */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21b5
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (04)000002       */ stfld      class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000006*/<string,object> Fluentx.Mvc.RedirectAndPostActionResult/*02000005*/::'<PostData>k__BackingField' /* 04000002 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method RedirectAndPostActionResult::set_PostData

  .method /*0600000A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string url,
                               class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000006*/<string,object> postData) cil managed
  // SIG: 20 02 01 0E 15 12 19 02 0E 1C
  {
    // Method begins at RVA 0x21be
    // Code size       30 (0x1e)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00002F       */ call       instance void [System.Web.Mvc/*23000002*/]System.Web.Mvc.ActionResult/*01000004*/::.ctor() /* 0A00002F */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 28   | (06)000007       */ call       instance void Fluentx.Mvc.RedirectAndPostActionResult/*02000005*/::set_Url(string) /* 06000007 */
    IL_000d:  /* 02   |                  */ ldarg.0
    IL_000e:  /* 04   |                  */ ldarg.2
    IL_000f:  /* 25   |                  */ dup
    IL_0010:  /* 2D   | 06               */ brtrue.s   IL_0018

    IL_0012:  /* 26   |                  */ pop
    IL_0013:  /* 73   | (0A)000030       */ newobj     instance void class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000006*/<string,object>/*1B000004*/::.ctor() /* 0A000030 */
    IL_0018:  /* 28   | (06)000009       */ call       instance void Fluentx.Mvc.RedirectAndPostActionResult/*02000005*/::set_PostData(class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000006*/<string,object>) /* 06000009 */
    IL_001d:  /* 2A   |                  */ ret
  } // end of method RedirectAndPostActionResult::.ctor

  .method /*0600000B*/ public hidebysig virtual 
          instance void  ExecuteResult(class [System.Web.Mvc/*23000002*/]System.Web.Mvc.ControllerContext/*01000008*/ context) cil managed
  // SIG: 20 01 01 12 21
  {
    // Method begins at RVA 0x21e0
    // Code size       37 (0x25)
    .maxstack  3
    .locals /*11000002*/ init (string V_0)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 02   |                  */ ldarg.0
    IL_0002:  /* 28   | (06)000006       */ call       instance string Fluentx.Mvc.RedirectAndPostActionResult/*02000005*/::get_Url() /* 06000006 */
    IL_0007:  /* 02   |                  */ ldarg.0
    IL_0008:  /* 28   | (06)000008       */ call       instance class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000006*/<string,object> Fluentx.Mvc.RedirectAndPostActionResult/*02000005*/::get_PostData() /* 06000008 */
    IL_000d:  /* 28   | (06)00000C       */ call       instance string Fluentx.Mvc.RedirectAndPostActionResult/*02000005*/::BuildPostForm(string,
                                                                                                                                          class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000006*/<string,object>) /* 0600000C */
    IL_0012:  /* 0A   |                  */ stloc.0
    IL_0013:  /* 03   |                  */ ldarg.1
    IL_0014:  /* 6F   | (0A)000015       */ callvirt   instance class [System.Web/*23000003*/]System.Web.HttpContextBase/*0100001E*/ [System.Web.Mvc/*23000002*/]System.Web.Mvc.ControllerContext/*01000008*/::get_HttpContext() /* 0A000015 */
    IL_0019:  /* 6F   | (0A)000031       */ callvirt   instance class [System.Web/*23000003*/]System.Web.HttpResponseBase/*01000032*/ [System.Web/*23000003*/]System.Web.HttpContextBase/*0100001E*/::get_Response() /* 0A000031 */
    IL_001e:  /* 06   |                  */ ldloc.0
    IL_001f:  /* 6F   | (0A)000032       */ callvirt   instance void [System.Web/*23000003*/]System.Web.HttpResponseBase/*01000032*/::Write(string) /* 0A000032 */
    IL_0024:  /* 2A   |                  */ ret
  } // end of method RedirectAndPostActionResult::ExecuteResult

  .method /*0600000C*/ private hidebysig 
          instance string  BuildPostForm(string Url,
                                         class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000006*/<string,object> PostData) cil managed
  // SIG: 20 02 0E 0E 15 12 19 02 0E 1C
  {
    // Method begins at RVA 0x2214
    // Code size       201 (0xc9)
    .maxstack  4
    .locals /*11000003*/ init (string V_0,
             class [mscorlib/*23000001*/]System.Text.StringBuilder/*01000033*/ V_1,
             valuetype [mscorlib/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000035*/<string,object> V_2,
             class [mscorlib/*23000001*/]System.Text.StringBuilder/*01000033*/ V_3,
             valuetype [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000006*//Enumerator/*01000034*/<string,object> V_4)
    IL_0000:  /* 72   | (70)000001       */ ldstr      "__PostForm" /* 70000001 */
    IL_0005:  /* 0A   |                  */ stloc.0
    IL_0006:  /* 73   | (0A)000033       */ newobj     instance void [mscorlib/*23000001*/]System.Text.StringBuilder/*01000033*/::.ctor() /* 0A000033 */
    IL_000b:  /* 0B   |                  */ stloc.1
    IL_000c:  /* 07   |                  */ ldloc.1
    IL_000d:  /* 72   | (70)000017       */ ldstr      "<form id=\"{0}\" name=\"{0}\" action=\"{1}\" method=\"POS"
    + "T\">" /* 70000017 */
    IL_0012:  /* 06   |                  */ ldloc.0
    IL_0013:  /* 03   |                  */ ldarg.1
    IL_0014:  /* 28   | (0A)000034       */ call       string [mscorlib/*23000001*/]System.String/*01000021*/::Format(string,
                                                                                                                      object,
                                                                                                                      object) /* 0A000034 */
    IL_0019:  /* 6F   | (0A)000035       */ callvirt   instance class [mscorlib/*23000001*/]System.Text.StringBuilder/*01000033*/ [mscorlib/*23000001*/]System.Text.StringBuilder/*01000033*/::Append(string) /* 0A000035 */
    IL_001e:  /* 26   |                  */ pop
    IL_001f:  /* 04   |                  */ ldarg.2
    IL_0020:  /* 6F   | (0A)000036       */ callvirt   instance valuetype [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000006*//Enumerator/*01000034*/<!0,!1> class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000006*/<string,object>/*1B000004*/::GetEnumerator() /* 0A000036 */
    IL_0025:  /* 13   | 04               */ stloc.s    V_4
    .try
    {
      IL_0027:  /* 2B   | 27               */ br.s       IL_0050

      IL_0029:  /* 12   | 04               */ ldloca.s   V_4
      IL_002b:  /* 28   | (0A)000037       */ call       instance valuetype [mscorlib/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000035*/<!0,!1> valuetype [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000006*//Enumerator/*01000034*/<string,object>/*1B000005*/::get_Current() /* 0A000037 */
      IL_0030:  /* 0C   |                  */ stloc.2
      IL_0031:  /* 07   |                  */ ldloc.1
      IL_0032:  /* 72   | (70)000083       */ ldstr      "<input type=\"hidden\" name=\"{0}\" value=\"{1}\"/>" /* 70000083 */
      IL_0037:  /* 12   | 02               */ ldloca.s   V_2
      IL_0039:  /* 28   | (0A)000038       */ call       instance !0 valuetype [mscorlib/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000035*/<string,object>/*1B000006*/::get_Key() /* 0A000038 */
      IL_003e:  /* 12   | 02               */ ldloca.s   V_2
      IL_0040:  /* 28   | (0A)000039       */ call       instance !1 valuetype [mscorlib/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000035*/<string,object>/*1B000006*/::get_Value() /* 0A000039 */
      IL_0045:  /* 28   | (0A)000034       */ call       string [mscorlib/*23000001*/]System.String/*01000021*/::Format(string,
                                                                                                                        object,
                                                                                                                        object) /* 0A000034 */
      IL_004a:  /* 6F   | (0A)000035       */ callvirt   instance class [mscorlib/*23000001*/]System.Text.StringBuilder/*01000033*/ [mscorlib/*23000001*/]System.Text.StringBuilder/*01000033*/::Append(string) /* 0A000035 */
      IL_004f:  /* 26   |                  */ pop
      IL_0050:  /* 12   | 04               */ ldloca.s   V_4
      IL_0052:  /* 28   | (0A)00003A       */ call       instance bool valuetype [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000006*//Enumerator/*01000034*/<string,object>/*1B000005*/::MoveNext() /* 0A00003A */
      IL_0057:  /* 2D   | D0               */ brtrue.s   IL_0029

      IL_0059:  /* DE   | 0E               */ leave.s    IL_0069

    }  // end .try
    finally
    {
      IL_005b:  /* 12   | 04               */ ldloca.s   V_4
      IL_005d:  /* FE16 | (1B)000005       */ constrained. valuetype [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000006*//Enumerator/*01000034*/<string,object>/*1B000005*/
      IL_0063:  /* 6F   | (0A)00002A       */ callvirt   instance void [mscorlib/*23000001*/]System.IDisposable/*0100002E*/::Dispose() /* 0A00002A */
      IL_0068:  /* DC   |                  */ endfinally
    }  // end handler
    // HEX: 02 00 27 00 34 5B 00 0E 00 00 00 00
    IL_0069:  /* 07   |                  */ ldloc.1
    IL_006a:  /* 72   | (70)0000DF       */ ldstr      "</form>" /* 700000DF */
    IL_006f:  /* 6F   | (0A)000035       */ callvirt   instance class [mscorlib/*23000001*/]System.Text.StringBuilder/*01000033*/ [mscorlib/*23000001*/]System.Text.StringBuilder/*01000033*/::Append(string) /* 0A000035 */
    IL_0074:  /* 26   |                  */ pop
    IL_0075:  /* 73   | (0A)000033       */ newobj     instance void [mscorlib/*23000001*/]System.Text.StringBuilder/*01000033*/::.ctor() /* 0A000033 */
    IL_007a:  /* 0D   |                  */ stloc.3
    IL_007b:  /* 09   |                  */ ldloc.3
    IL_007c:  /* 72   | (70)0000EF       */ ldstr      "<script language=\"javascript\">" /* 700000EF */
    IL_0081:  /* 6F   | (0A)000035       */ callvirt   instance class [mscorlib/*23000001*/]System.Text.StringBuilder/*01000033*/ [mscorlib/*23000001*/]System.Text.StringBuilder/*01000033*/::Append(string) /* 0A000035 */
    IL_0086:  /* 26   |                  */ pop
    IL_0087:  /* 09   |                  */ ldloc.3
    IL_0088:  /* 72   | (70)00012D       */ ldstr      "var v{0}=document.{0};" /* 7000012D */
    IL_008d:  /* 06   |                  */ ldloc.0
    IL_008e:  /* 28   | (0A)00003B       */ call       string [mscorlib/*23000001*/]System.String/*01000021*/::Format(string,
                                                                                                                      object) /* 0A00003B */
    IL_0093:  /* 6F   | (0A)000035       */ callvirt   instance class [mscorlib/*23000001*/]System.Text.StringBuilder/*01000033*/ [mscorlib/*23000001*/]System.Text.StringBuilder/*01000033*/::Append(string) /* 0A000035 */
    IL_0098:  /* 26   |                  */ pop
    IL_0099:  /* 09   |                  */ ldloc.3
    IL_009a:  /* 72   | (70)00015B       */ ldstr      "v{0}.submit();" /* 7000015B */
    IL_009f:  /* 06   |                  */ ldloc.0
    IL_00a0:  /* 28   | (0A)00003B       */ call       string [mscorlib/*23000001*/]System.String/*01000021*/::Format(string,
                                                                                                                      object) /* 0A00003B */
    IL_00a5:  /* 6F   | (0A)000035       */ callvirt   instance class [mscorlib/*23000001*/]System.Text.StringBuilder/*01000033*/ [mscorlib/*23000001*/]System.Text.StringBuilder/*01000033*/::Append(string) /* 0A000035 */
    IL_00aa:  /* 26   |                  */ pop
    IL_00ab:  /* 09   |                  */ ldloc.3
    IL_00ac:  /* 72   | (70)000179       */ ldstr      "</script>" /* 70000179 */
    IL_00b1:  /* 6F   | (0A)000035       */ callvirt   instance class [mscorlib/*23000001*/]System.Text.StringBuilder/*01000033*/ [mscorlib/*23000001*/]System.Text.StringBuilder/*01000033*/::Append(string) /* 0A000035 */
    IL_00b6:  /* 26   |                  */ pop
    IL_00b7:  /* 07   |                  */ ldloc.1
    IL_00b8:  /* 6F   | (0A)00003C       */ callvirt   instance string [mscorlib/*23000001*/]System.Object/*01000001*/::ToString() /* 0A00003C */
    IL_00bd:  /* 09   |                  */ ldloc.3
    IL_00be:  /* 6F   | (0A)00003C       */ callvirt   instance string [mscorlib/*23000001*/]System.Object/*01000001*/::ToString() /* 0A00003C */
    IL_00c3:  /* 28   | (0A)00003D       */ call       string [mscorlib/*23000001*/]System.String/*01000021*/::Concat(string,
                                                                                                                      string) /* 0A00003D */
    IL_00c8:  /* 2A   |                  */ ret
  } // end of method RedirectAndPostActionResult::BuildPostForm

  .method /*0600000D*/ public hidebysig static 
          class Fluentx.Mvc.RedirectAndPostActionResult/*02000005*/ 
          RedirectAndPost(string url,
                          class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000006*/<string,object> postData) cil managed
  // SIG: 00 02 12 14 0E 15 12 19 02 0E 1C
  {
    // Method begins at RVA 0x22fc
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 73   | (06)00000A       */ newobj     instance void Fluentx.Mvc.RedirectAndPostActionResult/*02000005*/::.ctor(string,
                                                                                                                                class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000006*/<string,object>) /* 0600000A */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method RedirectAndPostActionResult::RedirectAndPost

  .property /*17000001*/ instance string Url()
  {
    .get instance string Fluentx.Mvc.RedirectAndPostActionResult/*02000005*/::get_Url() /* 06000006 */
    .set instance void Fluentx.Mvc.RedirectAndPostActionResult/*02000005*/::set_Url(string) /* 06000007 */
  } // end of property RedirectAndPostActionResult::Url
  .property /*17000002*/ instance class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000006*/<string,object>
          PostData()
  {
    .get instance class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000006*/<string,object> Fluentx.Mvc.RedirectAndPostActionResult/*02000005*/::get_PostData() /* 06000008 */
    .set instance void Fluentx.Mvc.RedirectAndPostActionResult/*02000005*/::set_PostData(class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000006*/<string,object>) /* 06000009 */
  } // end of property RedirectAndPostActionResult::PostData
} // end of class Fluentx.Mvc.RedirectAndPostActionResult


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
// WARNING: Created Win32 resource file Fluentx.Mvc.res
