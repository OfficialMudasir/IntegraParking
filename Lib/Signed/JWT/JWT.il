
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
// Time-date stamp:            0x5555fefc
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2102
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x0b
// Minor linker version:           0x00
// Size of code:                   0x00001a00
// Size of init.data:              0x00000600
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x000039de
// Base of code:                   0x00002000
// Base of data:                   0x00004000
// Image base:                     0x10000000
// Section alignment:              0x00002000
// File alignment:                 0x00000200
// Major OS version:               0x0004
// Minor OS version:               0x0000
// Major image version:            0x0000
// Minor image version:            0x0000
// Major subsystem version:        0x0004
// Minor subsystem version:        0x0000
// Size of image:                  0x00008000
// Size of headers:                0x00000200
// Checksum:                       0x00000000
// Subsystem:                      0x0003
// DLL characteristics:            0x8540
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x00003984 [0x00000057] address [size] of Import Directory:          
// 0x00004000 [0x00000310] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00000000 [0x00000000] address [size] of Security Directory:        
// 0x00006000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x0000384c [0x0000001c] address [size] of Debug Directory:           
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
//              0x000019e4 Virtual Size
//              0x00002000 Virtual Address
//              0x00001a00 Size of Raw Data
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
//              0x00001c00 Pointer to Raw Data
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
//              0x00002000 Pointer to Raw Data
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
//              Entry 1: Type 0x3 Offset 0x000009e0
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x000039ce Import Name Table
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
// 0x000025dc [0x00001270] address [size] of Metadata Directory:        
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
//              'v2.0.50727' Version String
//    Storage Header:
//                    0x00 Flags
//                  0x0005 Number of Streams
//    Stream 1:
//              0x0000006c Offset
//              0x00000698 Size
//              '#~' Name
//    Stream 2:
//              0x00000704 Offset
//              0x0000069c Size
//              '#Strings' Name
//    Stream 3:
//              0x00000da0 Offset
//              0x000001d4 Size
//              '#US' Name
//    Stream 4:
//              0x00000f74 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00000f84 Offset
//              0x000002ec Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x00000c0908021f57 MaskValid
//      0x000016003325fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 8704
// PE header size       : 512 (496 used)    ( 5.88%)
// PE additional info   : 991               (11.39%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.83%)
// CLR meta-data size  : 4720               (54.23%)
// CLR additional info : 0                  ( 0.00%)
// CLR method headers  : 216                ( 2.48%)
// Managed code         : 1191              (13.68%)
// Data                 : 1536              (17.65%)
// Unaccounted          : -534              (-6.14%)

// Num.of PE sections   : 3
//   .text    - 6656
//   .rsrc    - 1024
//   .reloc   - 512

// CLR meta-data size  : 4720
//   Module        -    1 (10 bytes)
//   TypeDef       -    6 (84 bytes)      1 interfaces, 0 explicit layout
//   TypeRef       -   39 (234 bytes)
//   MethodDef     -   21 (294 bytes)     2 abstract, 0 native, 19 bodies
//   FieldDef      -   10 (60 bytes)      0 constant
//   MemberRef     -   61 (366 bytes)
//   ParamDef      -   40 (240 bytes)
//   Constant      -    7 (42 bytes)
//   CustomAttribute-   19 (114 bytes)
//   StandAloneSig -   11 (22 bytes)
//   InterfaceImpl -    1 (4 bytes)
//   TypeSpec      -    4 (8 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    3 (60 bytes)
//   GenericParam  -    3 (24 bytes)
//   MethodSpec    -    3 (12 bytes)
//   Strings       -  1692 bytes
//   Blobs         -   748 bytes
//   UserStrings   -   468 bytes
//   Guids         -    16 bytes
//   Uncategorized -   200 bytes

// CLR method headers : 216
//   Num.of method bodies  - 19
//   Num.of fat headers    - 11
//   Num.of tiny headers   - 8
//   Num.of fat sections   - 0
//   Num.of small sections - 4

// Managed code : 1191
//   Ave method size - 62
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Interface IJsonSerializer                (public) (abstract) (auto) (ansi) 
// Class DefaultJsonSerializer          (public) (auto) (ansi) 
// Class JwtHashAlgorithm               (public) (auto) (ansi) (sealed) 
// Class JsonWebToken                   (public) (abstract) (auto) (ansi) (sealed) 
// Class SignatureVerificationException (public) (auto) (ansi) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v2.0.50727
.assembly extern /*23000001*/ mscorlib
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 2:0:0:0
}
.assembly extern /*23000002*/ System.Web.Extensions
{
  .publickeytoken = (31 BF 38 56 AD 36 4E 35 )                         // 1.8V.6N5
  .ver 3:5:0:0
}
.assembly extern /*23000003*/ System.Core
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 3:5:0:0
}
.assembly /*20000001*/ JWT
{
  .custom /*0C000001:0A000004*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000B*/::.ctor(string) /* 0A000004 */ = ( 01 00 0D 50 75 62 6C 69 63 20 44 6F 6D 61 69 6E   // ...Public Domain
                                                                                                                                                             00 00 ) 
  .custom /*0C000002:0A00000F*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute/*01000017*/::.ctor() /* 0A00000F */ = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
                                                                                                                                                                          63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )       // ceptionThrows.
  .custom /*0C000003:0A000001*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000008*/::.ctor(string) /* 0A000001 */ = ( 01 00 03 4A 57 54 00 00 )                         // ...JWT..
  .custom /*0C000004:0A000002*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*01000009*/::.ctor(string) /* 0A000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000005:0A000003*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyConfigurationAttribute/*0100000A*/::.ctor(string) /* 0A000003 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000006:0A000005*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyProductAttribute/*0100000C*/::.ctor(string) /* 0A000005 */ = ( 01 00 03 4A 57 54 00 00 )                         // ...JWT..
  .custom /*0C000007:0A000006*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000D*/::.ctor(string) /* 0A000006 */ = ( 01 00 0D 50 75 62 6C 69 63 20 44 6F 6D 61 69 6E   // ...Public Domain
                                                                                                                                                               00 00 ) 
  .custom /*0C000008:0A000007*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyTrademarkAttribute/*0100000E*/::.ctor(string) /* 0A000007 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000009:0A000009*/ instance void [mscorlib/*23000001*/]System.Runtime.InteropServices.ComVisibleAttribute/*01000010*/::.ctor(bool) /* 0A000009 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00000A:0A00000A*/ instance void [mscorlib/*23000001*/]System.Runtime.InteropServices.GuidAttribute/*01000011*/::.ctor(string) /* 0A00000A */ = ( 01 00 24 37 34 35 65 36 34 39 65 2D 35 38 38 64   // ..$745e649e-588d
                                                                                                                                                               2D 34 65 36 64 2D 38 31 63 65 2D 31 64 32 63 33   // -4e6d-81ce-1d2c3
                                                                                                                                                               39 63 32 34 62 30 61 00 00 )                      // 9c24b0a..
  .custom /*0C00000B:0A00000C*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*01000013*/::.ctor(string) /* 0A00000C */ = ( 01 00 07 31 2E 33 2E 33 2E 30 00 00 )             // ...1.3.3.0..

  // --- The following custom attribute is added automatically, do not uncomment -------
  //  .custom /*0C00000C:0A00000D*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000014*/::.ctor(valuetype [mscorlib/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000014*//DebuggingModes/*01000015*/) /* 0A00000D */ = ( 01 00 02 00 00 00 00 00 ) 

  .custom /*0C00000D:0A00000E*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilationRelaxationsAttribute/*01000016*/::.ctor(int32) /* 0A00000E */ = ( 01 00 08 00 00 00 00 00 ) 
  .hash algorithm 0x00008004
  .ver 1:3:3:0
}
.module JWT.dll
// MVID: {E0565098-0E4D-48A4-8355-E7360858C918}
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000001    //  ILONLY
// Image base: 0x00DA0000


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ interface public abstract auto ansi JWT.IJsonSerializer
{
  .method /*06000001*/ public hidebysig newslot abstract virtual 
          instance string  Serialize(object obj) cil managed
  // SIG: 20 01 0E 1C
  {
    // Method begins at RVA 0x0
  } // end of method IJsonSerializer::Serialize

  .method /*06000002*/ public hidebysig newslot abstract virtual 
          instance !!T  Deserialize<T>(string json) cil managed
  // SIG: 30 01 01 1E 00 0E
  {
    // Method begins at RVA 0x0
  } // end of method IJsonSerializer::Deserialize

} // end of class JWT.IJsonSerializer

.class /*02000003*/ public auto ansi beforefieldinit JWT.DefaultJsonSerializer
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
       implements JWT.IJsonSerializer/*02000002*/
{
  .field /*04000001*/ private initonly class [System.Web.Extensions/*23000002*/]System.Web.Script.Serialization.JavaScriptSerializer/*01000004*/ serializer
  .method /*06000003*/ public hidebysig newslot virtual final 
          instance string  Serialize(object obj) cil managed
  // SIG: 20 01 0E 1C
  {
    // Method begins at RVA 0x2050
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (04)000001       */ ldfld      class [System.Web.Extensions/*23000002*/]System.Web.Script.Serialization.JavaScriptSerializer/*01000004*/ JWT.DefaultJsonSerializer/*02000003*/::serializer /* 04000001 */
    IL_0006:  /* 03   |                  */ ldarg.1
    IL_0007:  /* 6F   | (0A)000010       */ callvirt   instance string [System.Web.Extensions/*23000002*/]System.Web.Script.Serialization.JavaScriptSerializer/*01000004*/::Serialize(object) /* 0A000010 */
    IL_000c:  /* 2A   |                  */ ret
  } // end of method DefaultJsonSerializer::Serialize

  .method /*06000004*/ public hidebysig newslot virtual final 
          instance !!T  Deserialize<T>(string json) cil managed
  // SIG: 30 01 01 1E 00 0E
  {
    // Method begins at RVA 0x205e
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (04)000001       */ ldfld      class [System.Web.Extensions/*23000002*/]System.Web.Script.Serialization.JavaScriptSerializer/*01000004*/ JWT.DefaultJsonSerializer/*02000003*/::serializer /* 04000001 */
    IL_0006:  /* 03   |                  */ ldarg.1
    IL_0007:  /* 6F   | (2B)000001       */ callvirt   instance !!0 [System.Web.Extensions/*23000002*/]System.Web.Script.Serialization.JavaScriptSerializer/*01000004*/::Deserialize<!!0>(string) /* 2B000001 */
    IL_000c:  /* 2A   |                  */ ret
  } // end of method DefaultJsonSerializer::Deserialize

  .method /*06000005*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x206c
    // Code size       18 (0x12)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 73   | (0A)000012       */ newobj     instance void [System.Web.Extensions/*23000002*/]System.Web.Script.Serialization.JavaScriptSerializer/*01000004*/::.ctor() /* 0A000012 */
    IL_0006:  /* 7D   | (04)000001       */ stfld      class [System.Web.Extensions/*23000002*/]System.Web.Script.Serialization.JavaScriptSerializer/*01000004*/ JWT.DefaultJsonSerializer/*02000003*/::serializer /* 04000001 */
    IL_000b:  /* 02   |                  */ ldarg.0
    IL_000c:  /* 28   | (0A)000013       */ call       instance void [mscorlib/*23000001*/]System.Object/*01000001*/::.ctor() /* 0A000013 */
    IL_0011:  /* 2A   |                  */ ret
  } // end of method DefaultJsonSerializer::.ctor

} // end of class JWT.DefaultJsonSerializer

.class /*02000004*/ public auto ansi sealed JWT.JwtHashAlgorithm
       extends [mscorlib/*23000001*/]System.Enum/*01000002*/
{
  .field /*04000002*/ public specialname rtspecialname int32 value__
  .field /*04000003*/ public static literal valuetype JWT.JwtHashAlgorithm/*02000004*/ HS256 = int32(0x00000000)
  .field /*04000004*/ public static literal valuetype JWT.JwtHashAlgorithm/*02000004*/ HS384 = int32(0x00000001)
  .field /*04000005*/ public static literal valuetype JWT.JwtHashAlgorithm/*02000004*/ HS512 = int32(0x00000002)
} // end of class JWT.JwtHashAlgorithm

.class /*02000005*/ public abstract auto ansi sealed JWT.JsonWebToken
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .field /*04000006*/ private static class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000005*/<valuetype JWT.JwtHashAlgorithm/*02000004*/,class [System.Core/*23000003*/]System.Func`3/*01000006*/<uint8[],uint8[],uint8[]>> HashAlgorithms
  .field /*04000007*/ public static class JWT.IJsonSerializer/*02000002*/ JsonSerializer
  .field /*04000008*/ private static class [System.Core/*23000003*/]System.Func`3/*01000006*/<uint8[],uint8[],uint8[]> 'CS$<>9__CachedAnonymousMethodDelegate4'
  .custom /*0C00000E:0A000014*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000018*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
  .field /*04000009*/ private static class [System.Core/*23000003*/]System.Func`3/*01000006*/<uint8[],uint8[],uint8[]> 'CS$<>9__CachedAnonymousMethodDelegate5'
  .custom /*0C00000F:0A000014*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000018*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
  .field /*0400000A*/ private static class [System.Core/*23000003*/]System.Func`3/*01000006*/<uint8[],uint8[],uint8[]> 'CS$<>9__CachedAnonymousMethodDelegate6'
  .custom /*0C000010:0A000014*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000018*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
  .method /*06000006*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x2134
    // Code size       131 (0x83)
    .maxstack  4
    .locals /*11000004*/ init (class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000005*/<valuetype JWT.JwtHashAlgorithm/*02000004*/,class [System.Core/*23000003*/]System.Func`3/*01000006*/<uint8[],uint8[],uint8[]>> V_0)
    IL_0000:  /* 73   | (06)000005       */ newobj     instance void JWT.DefaultJsonSerializer/*02000003*/::.ctor() /* 06000005 */
    IL_0005:  /* 80   | (04)000007       */ stsfld     class JWT.IJsonSerializer/*02000002*/ JWT.JsonWebToken/*02000005*/::JsonSerializer /* 04000007 */
    IL_000a:  /* 73   | (0A)00001A       */ newobj     instance void class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000005*/<valuetype JWT.JwtHashAlgorithm/*02000004*/,class [System.Core/*23000003*/]System.Func`3/*01000006*/<uint8[],uint8[],uint8[]>>/*1B000001*/::.ctor() /* 0A00001A */
    IL_000f:  /* 0A   |                  */ stloc.0
    IL_0010:  /* 06   |                  */ ldloc.0
    IL_0011:  /* 16   |                  */ ldc.i4.0
    IL_0012:  /* 7E   | (04)000008       */ ldsfld     class [System.Core/*23000003*/]System.Func`3/*01000006*/<uint8[],uint8[],uint8[]> JWT.JsonWebToken/*02000005*/::'CS$<>9__CachedAnonymousMethodDelegate4' /* 04000008 */
    IL_0017:  /* 2D   | 11               */ brtrue.s   IL_002a

    IL_0019:  /* 14   |                  */ ldnull
    IL_001a:  /* FE06 | (06)000012       */ ldftn      uint8[] JWT.JsonWebToken/*02000005*/::'<.cctor>b__1'(uint8[],
                                                                                                            uint8[]) /* 06000012 */
    IL_0020:  /* 73   | (0A)00001B       */ newobj     instance void class [System.Core/*23000003*/]System.Func`3/*01000006*/<uint8[],uint8[],uint8[]>/*1B000002*/::.ctor(object,
                                                                                                                                                                          native int) /* 0A00001B */
    IL_0025:  /* 80   | (04)000008       */ stsfld     class [System.Core/*23000003*/]System.Func`3/*01000006*/<uint8[],uint8[],uint8[]> JWT.JsonWebToken/*02000005*/::'CS$<>9__CachedAnonymousMethodDelegate4' /* 04000008 */
    IL_002a:  /* 7E   | (04)000008       */ ldsfld     class [System.Core/*23000003*/]System.Func`3/*01000006*/<uint8[],uint8[],uint8[]> JWT.JsonWebToken/*02000005*/::'CS$<>9__CachedAnonymousMethodDelegate4' /* 04000008 */
    IL_002f:  /* 6F   | (0A)00001C       */ callvirt   instance void class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000005*/<valuetype JWT.JwtHashAlgorithm/*02000004*/,class [System.Core/*23000003*/]System.Func`3/*01000006*/<uint8[],uint8[],uint8[]>>/*1B000001*/::Add(!0,
                                                                                                                                                                                                                                                                                                    !1) /* 0A00001C */
    IL_0034:  /* 06   |                  */ ldloc.0
    IL_0035:  /* 17   |                  */ ldc.i4.1
    IL_0036:  /* 7E   | (04)000009       */ ldsfld     class [System.Core/*23000003*/]System.Func`3/*01000006*/<uint8[],uint8[],uint8[]> JWT.JsonWebToken/*02000005*/::'CS$<>9__CachedAnonymousMethodDelegate5' /* 04000009 */
    IL_003b:  /* 2D   | 11               */ brtrue.s   IL_004e

    IL_003d:  /* 14   |                  */ ldnull
    IL_003e:  /* FE06 | (06)000013       */ ldftn      uint8[] JWT.JsonWebToken/*02000005*/::'<.cctor>b__2'(uint8[],
                                                                                                            uint8[]) /* 06000013 */
    IL_0044:  /* 73   | (0A)00001B       */ newobj     instance void class [System.Core/*23000003*/]System.Func`3/*01000006*/<uint8[],uint8[],uint8[]>/*1B000002*/::.ctor(object,
                                                                                                                                                                          native int) /* 0A00001B */
    IL_0049:  /* 80   | (04)000009       */ stsfld     class [System.Core/*23000003*/]System.Func`3/*01000006*/<uint8[],uint8[],uint8[]> JWT.JsonWebToken/*02000005*/::'CS$<>9__CachedAnonymousMethodDelegate5' /* 04000009 */
    IL_004e:  /* 7E   | (04)000009       */ ldsfld     class [System.Core/*23000003*/]System.Func`3/*01000006*/<uint8[],uint8[],uint8[]> JWT.JsonWebToken/*02000005*/::'CS$<>9__CachedAnonymousMethodDelegate5' /* 04000009 */
    IL_0053:  /* 6F   | (0A)00001C       */ callvirt   instance void class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000005*/<valuetype JWT.JwtHashAlgorithm/*02000004*/,class [System.Core/*23000003*/]System.Func`3/*01000006*/<uint8[],uint8[],uint8[]>>/*1B000001*/::Add(!0,
                                                                                                                                                                                                                                                                                                    !1) /* 0A00001C */
    IL_0058:  /* 06   |                  */ ldloc.0
    IL_0059:  /* 18   |                  */ ldc.i4.2
    IL_005a:  /* 7E   | (04)00000A       */ ldsfld     class [System.Core/*23000003*/]System.Func`3/*01000006*/<uint8[],uint8[],uint8[]> JWT.JsonWebToken/*02000005*/::'CS$<>9__CachedAnonymousMethodDelegate6' /* 0400000A */
    IL_005f:  /* 2D   | 11               */ brtrue.s   IL_0072

    IL_0061:  /* 14   |                  */ ldnull
    IL_0062:  /* FE06 | (06)000014       */ ldftn      uint8[] JWT.JsonWebToken/*02000005*/::'<.cctor>b__3'(uint8[],
                                                                                                            uint8[]) /* 06000014 */
    IL_0068:  /* 73   | (0A)00001B       */ newobj     instance void class [System.Core/*23000003*/]System.Func`3/*01000006*/<uint8[],uint8[],uint8[]>/*1B000002*/::.ctor(object,
                                                                                                                                                                          native int) /* 0A00001B */
    IL_006d:  /* 80   | (04)00000A       */ stsfld     class [System.Core/*23000003*/]System.Func`3/*01000006*/<uint8[],uint8[],uint8[]> JWT.JsonWebToken/*02000005*/::'CS$<>9__CachedAnonymousMethodDelegate6' /* 0400000A */
    IL_0072:  /* 7E   | (04)00000A       */ ldsfld     class [System.Core/*23000003*/]System.Func`3/*01000006*/<uint8[],uint8[],uint8[]> JWT.JsonWebToken/*02000005*/::'CS$<>9__CachedAnonymousMethodDelegate6' /* 0400000A */
    IL_0077:  /* 6F   | (0A)00001C       */ callvirt   instance void class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000005*/<valuetype JWT.JwtHashAlgorithm/*02000004*/,class [System.Core/*23000003*/]System.Func`3/*01000006*/<uint8[],uint8[],uint8[]>>/*1B000001*/::Add(!0,
                                                                                                                                                                                                                                                                                                    !1) /* 0A00001C */
    IL_007c:  /* 06   |                  */ ldloc.0
    IL_007d:  /* 80   | (04)000006       */ stsfld     class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000005*/<valuetype JWT.JwtHashAlgorithm/*02000004*/,class [System.Core/*23000003*/]System.Func`3/*01000006*/<uint8[],uint8[],uint8[]>> JWT.JsonWebToken/*02000005*/::HashAlgorithms /* 04000006 */
    IL_0082:  /* 2A   |                  */ ret
  } // end of method JsonWebToken::.cctor

  .method /*06000007*/ public hidebysig static 
          string  Encode(class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000007*/<string,object> extraHeaders,
                         object payload,
                         uint8[] key,
                         valuetype JWT.JwtHashAlgorithm/*02000004*/ 'algorithm') cil managed
  // SIG: 00 04 0E 15 12 1D 02 0E 1C 1C 1D 05 11 10
  {
    // Method begins at RVA 0x21c4
    // Code size       208 (0xd0)
    .maxstack  3
    .locals /*11000005*/ init (class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*0100001E*/<string> V_0,
             class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000005*/<string,object> V_1,
             uint8[] V_2,
             uint8[] V_3,
             string V_4,
             uint8[] V_5,
             uint8[] V_6,
             class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000005*/<string,object> V_7)
    IL_0000:  /* 73   | (0A)00001D       */ newobj     instance void class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*0100001E*/<string>/*1B000003*/::.ctor() /* 0A00001D */
    IL_0005:  /* 0A   |                  */ stloc.0
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 73   | (0A)00001E       */ newobj     instance void class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000005*/<string,object>/*1B000004*/::.ctor(class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000007*/<!0,!1>) /* 0A00001E */
    IL_000c:  /* 13   | 07               */ stloc.s    V_7
    IL_000e:  /* 11   | 07               */ ldloc.s    V_7
    IL_0010:  /* 72   | (70)000001       */ ldstr      "typ" /* 70000001 */
    IL_0015:  /* 72   | (70)000009       */ ldstr      "JWT" /* 70000009 */
    IL_001a:  /* 6F   | (0A)00001F       */ callvirt   instance void class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000005*/<string,object>/*1B000004*/::Add(!0,
                                                                                                                                                                                     !1) /* 0A00001F */
    IL_001f:  /* 11   | 07               */ ldloc.s    V_7
    IL_0021:  /* 72   | (70)000011       */ ldstr      "alg" /* 70000011 */
    IL_0026:  /* 05   |                  */ ldarg.3
    IL_0027:  /* 8C   | (02)000004       */ box        JWT.JwtHashAlgorithm/*02000004*/
    IL_002c:  /* 6F   | (0A)000020       */ callvirt   instance string [mscorlib/*23000001*/]System.Object/*01000001*/::ToString() /* 0A000020 */
    IL_0031:  /* 6F   | (0A)00001F       */ callvirt   instance void class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000005*/<string,object>/*1B000004*/::Add(!0,
                                                                                                                                                                                     !1) /* 0A00001F */
    IL_0036:  /* 11   | 07               */ ldloc.s    V_7
    IL_0038:  /* 0B   |                  */ stloc.1
    IL_0039:  /* 28   | (0A)000021       */ call       class [mscorlib/*23000001*/]System.Text.Encoding/*0100001F*/ [mscorlib/*23000001*/]System.Text.Encoding/*0100001F*/::get_UTF8() /* 0A000021 */
    IL_003e:  /* 7E   | (04)000007       */ ldsfld     class JWT.IJsonSerializer/*02000002*/ JWT.JsonWebToken/*02000005*/::JsonSerializer /* 04000007 */
    IL_0043:  /* 07   |                  */ ldloc.1
    IL_0044:  /* 6F   | (06)000001       */ callvirt   instance string JWT.IJsonSerializer/*02000002*/::Serialize(object) /* 06000001 */
    IL_0049:  /* 6F   | (0A)000022       */ callvirt   instance uint8[] [mscorlib/*23000001*/]System.Text.Encoding/*0100001F*/::GetBytes(string) /* 0A000022 */
    IL_004e:  /* 0C   |                  */ stloc.2
    IL_004f:  /* 28   | (0A)000021       */ call       class [mscorlib/*23000001*/]System.Text.Encoding/*0100001F*/ [mscorlib/*23000001*/]System.Text.Encoding/*0100001F*/::get_UTF8() /* 0A000021 */
    IL_0054:  /* 7E   | (04)000007       */ ldsfld     class JWT.IJsonSerializer/*02000002*/ JWT.JsonWebToken/*02000005*/::JsonSerializer /* 04000007 */
    IL_0059:  /* 03   |                  */ ldarg.1
    IL_005a:  /* 6F   | (06)000001       */ callvirt   instance string JWT.IJsonSerializer/*02000002*/::Serialize(object) /* 06000001 */
    IL_005f:  /* 6F   | (0A)000022       */ callvirt   instance uint8[] [mscorlib/*23000001*/]System.Text.Encoding/*0100001F*/::GetBytes(string) /* 0A000022 */
    IL_0064:  /* 0D   |                  */ stloc.3
    IL_0065:  /* 06   |                  */ ldloc.0
    IL_0066:  /* 08   |                  */ ldloc.2
    IL_0067:  /* 28   | (06)000010       */ call       string JWT.JsonWebToken/*02000005*/::Base64UrlEncode(uint8[]) /* 06000010 */
    IL_006c:  /* 6F   | (0A)000023       */ callvirt   instance void class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*0100001E*/<string>/*1B000003*/::Add(!0) /* 0A000023 */
    IL_0071:  /* 06   |                  */ ldloc.0
    IL_0072:  /* 09   |                  */ ldloc.3
    IL_0073:  /* 28   | (06)000010       */ call       string JWT.JsonWebToken/*02000005*/::Base64UrlEncode(uint8[]) /* 06000010 */
    IL_0078:  /* 6F   | (0A)000023       */ callvirt   instance void class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*0100001E*/<string>/*1B000003*/::Add(!0) /* 0A000023 */
    IL_007d:  /* 72   | (70)000019       */ ldstr      "." /* 70000019 */
    IL_0082:  /* 06   |                  */ ldloc.0
    IL_0083:  /* 6F   | (0A)000024       */ callvirt   instance !0[] class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*0100001E*/<string>/*1B000003*/::ToArray() /* 0A000024 */
    IL_0088:  /* 28   | (0A)000025       */ call       string [mscorlib/*23000001*/]System.String/*01000020*/::Join(string,
                                                                                                                    string[]) /* 0A000025 */
    IL_008d:  /* 13   | 04               */ stloc.s    V_4
    IL_008f:  /* 28   | (0A)000021       */ call       class [mscorlib/*23000001*/]System.Text.Encoding/*0100001F*/ [mscorlib/*23000001*/]System.Text.Encoding/*0100001F*/::get_UTF8() /* 0A000021 */
    IL_0094:  /* 11   | 04               */ ldloc.s    V_4
    IL_0096:  /* 6F   | (0A)000022       */ callvirt   instance uint8[] [mscorlib/*23000001*/]System.Text.Encoding/*0100001F*/::GetBytes(string) /* 0A000022 */
    IL_009b:  /* 13   | 05               */ stloc.s    V_5
    IL_009d:  /* 7E   | (04)000006       */ ldsfld     class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000005*/<valuetype JWT.JwtHashAlgorithm/*02000004*/,class [System.Core/*23000003*/]System.Func`3/*01000006*/<uint8[],uint8[],uint8[]>> JWT.JsonWebToken/*02000005*/::HashAlgorithms /* 04000006 */
    IL_00a2:  /* 05   |                  */ ldarg.3
    IL_00a3:  /* 6F   | (0A)000026       */ callvirt   instance !1 class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000005*/<valuetype JWT.JwtHashAlgorithm/*02000004*/,class [System.Core/*23000003*/]System.Func`3/*01000006*/<uint8[],uint8[],uint8[]>>/*1B000001*/::get_Item(!0) /* 0A000026 */
    IL_00a8:  /* 04   |                  */ ldarg.2
    IL_00a9:  /* 11   | 05               */ ldloc.s    V_5
    IL_00ab:  /* 6F   | (0A)000027       */ callvirt   instance !2 class [System.Core/*23000003*/]System.Func`3/*01000006*/<uint8[],uint8[],uint8[]>/*1B000002*/::Invoke(!0,
                                                                                                                                                                         !1) /* 0A000027 */
    IL_00b0:  /* 13   | 06               */ stloc.s    V_6
    IL_00b2:  /* 06   |                  */ ldloc.0
    IL_00b3:  /* 11   | 06               */ ldloc.s    V_6
    IL_00b5:  /* 28   | (06)000010       */ call       string JWT.JsonWebToken/*02000005*/::Base64UrlEncode(uint8[]) /* 06000010 */
    IL_00ba:  /* 6F   | (0A)000023       */ callvirt   instance void class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*0100001E*/<string>/*1B000003*/::Add(!0) /* 0A000023 */
    IL_00bf:  /* 72   | (70)000019       */ ldstr      "." /* 70000019 */
    IL_00c4:  /* 06   |                  */ ldloc.0
    IL_00c5:  /* 6F   | (0A)000024       */ callvirt   instance !0[] class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*0100001E*/<string>/*1B000003*/::ToArray() /* 0A000024 */
    IL_00ca:  /* 28   | (0A)000025       */ call       string [mscorlib/*23000001*/]System.String/*01000020*/::Join(string,
                                                                                                                    string[]) /* 0A000025 */
    IL_00cf:  /* 2A   |                  */ ret
  } // end of method JsonWebToken::Encode

  .method /*06000008*/ public hidebysig static 
          string  Encode(object payload,
                         uint8[] key,
                         valuetype JWT.JwtHashAlgorithm/*02000004*/ 'algorithm') cil managed
  // SIG: 00 03 0E 1C 1D 05 11 10
  {
    // Method begins at RVA 0x22a0
    // Code size       14 (0xe)
    .maxstack  8
    IL_0000:  /* 73   | (0A)000028       */ newobj     instance void class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000005*/<string,object>/*1B000004*/::.ctor() /* 0A000028 */
    IL_0005:  /* 02   |                  */ ldarg.0
    IL_0006:  /* 03   |                  */ ldarg.1
    IL_0007:  /* 04   |                  */ ldarg.2
    IL_0008:  /* 28   | (06)000007       */ call       string JWT.JsonWebToken/*02000005*/::Encode(class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000007*/<string,object>,
                                                                                                   object,
                                                                                                   uint8[],
                                                                                                   valuetype JWT.JwtHashAlgorithm/*02000004*/) /* 06000007 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method JsonWebToken::Encode

  .method /*06000009*/ public hidebysig static 
          string  Encode(class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000007*/<string,object> extraHeaders,
                         object payload,
                         string key,
                         valuetype JWT.JwtHashAlgorithm/*02000004*/ 'algorithm') cil managed
  // SIG: 00 04 0E 15 12 1D 02 0E 1C 1C 0E 11 10
  {
    // Method begins at RVA 0x22af
    // Code size       20 (0x14)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 28   | (0A)000021       */ call       class [mscorlib/*23000001*/]System.Text.Encoding/*0100001F*/ [mscorlib/*23000001*/]System.Text.Encoding/*0100001F*/::get_UTF8() /* 0A000021 */
    IL_0007:  /* 04   |                  */ ldarg.2
    IL_0008:  /* 6F   | (0A)000022       */ callvirt   instance uint8[] [mscorlib/*23000001*/]System.Text.Encoding/*0100001F*/::GetBytes(string) /* 0A000022 */
    IL_000d:  /* 05   |                  */ ldarg.3
    IL_000e:  /* 28   | (06)000007       */ call       string JWT.JsonWebToken/*02000005*/::Encode(class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000007*/<string,object>,
                                                                                                   object,
                                                                                                   uint8[],
                                                                                                   valuetype JWT.JwtHashAlgorithm/*02000004*/) /* 06000007 */
    IL_0013:  /* 2A   |                  */ ret
  } // end of method JsonWebToken::Encode

  .method /*0600000A*/ public hidebysig static 
          string  Encode(object payload,
                         string key,
                         valuetype JWT.JwtHashAlgorithm/*02000004*/ 'algorithm') cil managed
  // SIG: 00 03 0E 1C 0E 11 10
  {
    // Method begins at RVA 0x22c4
    // Code size       24 (0x18)
    .maxstack  8
    IL_0000:  /* 73   | (0A)000028       */ newobj     instance void class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000005*/<string,object>/*1B000004*/::.ctor() /* 0A000028 */
    IL_0005:  /* 02   |                  */ ldarg.0
    IL_0006:  /* 28   | (0A)000021       */ call       class [mscorlib/*23000001*/]System.Text.Encoding/*0100001F*/ [mscorlib/*23000001*/]System.Text.Encoding/*0100001F*/::get_UTF8() /* 0A000021 */
    IL_000b:  /* 03   |                  */ ldarg.1
    IL_000c:  /* 6F   | (0A)000022       */ callvirt   instance uint8[] [mscorlib/*23000001*/]System.Text.Encoding/*0100001F*/::GetBytes(string) /* 0A000022 */
    IL_0011:  /* 04   |                  */ ldarg.2
    IL_0012:  /* 28   | (06)000007       */ call       string JWT.JsonWebToken/*02000005*/::Encode(class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000007*/<string,object>,
                                                                                                   object,
                                                                                                   uint8[],
                                                                                                   valuetype JWT.JwtHashAlgorithm/*02000004*/) /* 06000007 */
    IL_0017:  /* 2A   |                  */ ret
  } // end of method JsonWebToken::Encode

  .method /*0600000B*/ public hidebysig static 
          string  Decode(string token,
                         uint8[] key,
                         [opt] bool verify) cil managed
  // SIG: 00 03 0E 0E 1D 05 02
  {
    .param [3]/*08000015*/  = bool(true)
    // Method begins at RVA 0x22e0
    // Code size       369 (0x171)
    .maxstack  8
    .locals /*11000006*/ init (string[] V_0,
             string V_1,
             string V_2,
             uint8[] V_3,
             string V_4,
             string V_5,
             class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000005*/<string,object> V_6,
             uint8[] V_7,
             string V_8,
             uint8[] V_9,
             string V_10,
             string V_11,
             class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000005*/<string,object> V_12,
             int32 V_13,
             float64 V_14,
             char[] V_15,
             valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000026*/ V_16)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 17   |                  */ ldc.i4.1
    IL_0002:  /* 8D   | (01)000021       */ newarr     [mscorlib/*23000001*/]System.Char/*01000021*/
    IL_0007:  /* 13   | 0F               */ stloc.s    V_15
    IL_0009:  /* 11   | 0F               */ ldloc.s    V_15
    IL_000b:  /* 16   |                  */ ldc.i4.0
    IL_000c:  /* 1F   | 2E               */ ldc.i4.s   46
    IL_000e:  /* 9D   |                  */ stelem.i2
    IL_000f:  /* 11   | 0F               */ ldloc.s    V_15
    IL_0011:  /* 6F   | (0A)000029       */ callvirt   instance string[] [mscorlib/*23000001*/]System.String/*01000020*/::Split(char[]) /* 0A000029 */
    IL_0016:  /* 0A   |                  */ stloc.0
    IL_0017:  /* 06   |                  */ ldloc.0
    IL_0018:  /* 8E   |                  */ ldlen
    IL_0019:  /* 69   |                  */ conv.i4
    IL_001a:  /* 19   |                  */ ldc.i4.3
    IL_001b:  /* 2E   | 0B               */ beq.s      IL_0028

    IL_001d:  /* 72   | (70)00001D       */ ldstr      "Token must consist from 3 delimited by dot parts" /* 7000001D */
    IL_0022:  /* 73   | (0A)00002A       */ newobj     instance void [mscorlib/*23000001*/]System.ArgumentException/*01000022*/::.ctor(string) /* 0A00002A */
    IL_0027:  /* 7A   |                  */ throw

    IL_0028:  /* 06   |                  */ ldloc.0
    IL_0029:  /* 16   |                  */ ldc.i4.0
    IL_002a:  /* 9A   |                  */ ldelem.ref
    IL_002b:  /* 0B   |                  */ stloc.1
    IL_002c:  /* 06   |                  */ ldloc.0
    IL_002d:  /* 17   |                  */ ldc.i4.1
    IL_002e:  /* 9A   |                  */ ldelem.ref
    IL_002f:  /* 0C   |                  */ stloc.2
    IL_0030:  /* 06   |                  */ ldloc.0
    IL_0031:  /* 18   |                  */ ldc.i4.2
    IL_0032:  /* 9A   |                  */ ldelem.ref
    IL_0033:  /* 28   | (06)000011       */ call       uint8[] JWT.JsonWebToken/*02000005*/::Base64UrlDecode(string) /* 06000011 */
    IL_0038:  /* 0D   |                  */ stloc.3
    IL_0039:  /* 28   | (0A)000021       */ call       class [mscorlib/*23000001*/]System.Text.Encoding/*0100001F*/ [mscorlib/*23000001*/]System.Text.Encoding/*0100001F*/::get_UTF8() /* 0A000021 */
    IL_003e:  /* 07   |                  */ ldloc.1
    IL_003f:  /* 28   | (06)000011       */ call       uint8[] JWT.JsonWebToken/*02000005*/::Base64UrlDecode(string) /* 06000011 */
    IL_0044:  /* 6F   | (0A)00002B       */ callvirt   instance string [mscorlib/*23000001*/]System.Text.Encoding/*0100001F*/::GetString(uint8[]) /* 0A00002B */
    IL_0049:  /* 13   | 04               */ stloc.s    V_4
    IL_004b:  /* 28   | (0A)000021       */ call       class [mscorlib/*23000001*/]System.Text.Encoding/*0100001F*/ [mscorlib/*23000001*/]System.Text.Encoding/*0100001F*/::get_UTF8() /* 0A000021 */
    IL_0050:  /* 08   |                  */ ldloc.2
    IL_0051:  /* 28   | (06)000011       */ call       uint8[] JWT.JsonWebToken/*02000005*/::Base64UrlDecode(string) /* 06000011 */
    IL_0056:  /* 6F   | (0A)00002B       */ callvirt   instance string [mscorlib/*23000001*/]System.Text.Encoding/*0100001F*/::GetString(uint8[]) /* 0A00002B */
    IL_005b:  /* 13   | 05               */ stloc.s    V_5
    IL_005d:  /* 7E   | (04)000007       */ ldsfld     class JWT.IJsonSerializer/*02000002*/ JWT.JsonWebToken/*02000005*/::JsonSerializer /* 04000007 */
    IL_0062:  /* 11   | 04               */ ldloc.s    V_4
    IL_0064:  /* 6F   | (2B)000002       */ callvirt   instance !!0 JWT.IJsonSerializer/*02000002*/::Deserialize<class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000005*/<string,object>>(string) /* 2B000002 */
    IL_0069:  /* 13   | 06               */ stloc.s    V_6
    IL_006b:  /* 04   |                  */ ldarg.2
    IL_006c:  /* 39   | FD000000         */ brfalse    IL_016e

    IL_0071:  /* 28   | (0A)000021       */ call       class [mscorlib/*23000001*/]System.Text.Encoding/*0100001F*/ [mscorlib/*23000001*/]System.Text.Encoding/*0100001F*/::get_UTF8() /* 0A000021 */
    IL_0076:  /* 07   |                  */ ldloc.1
    IL_0077:  /* 72   | (70)000019       */ ldstr      "." /* 70000019 */
    IL_007c:  /* 08   |                  */ ldloc.2
    IL_007d:  /* 28   | (0A)00002C       */ call       string [mscorlib/*23000001*/]System.String/*01000020*/::Concat(string,
                                                                                                                      string,
                                                                                                                      string) /* 0A00002C */
    IL_0082:  /* 6F   | (0A)000022       */ callvirt   instance uint8[] [mscorlib/*23000001*/]System.Text.Encoding/*0100001F*/::GetBytes(string) /* 0A000022 */
    IL_0087:  /* 13   | 07               */ stloc.s    V_7
    IL_0089:  /* 11   | 06               */ ldloc.s    V_6
    IL_008b:  /* 72   | (70)000011       */ ldstr      "alg" /* 70000011 */
    IL_0090:  /* 6F   | (0A)00002D       */ callvirt   instance !1 class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000005*/<string,object>/*1B000004*/::get_Item(!0) /* 0A00002D */
    IL_0095:  /* 74   | (01)000020       */ castclass  [mscorlib/*23000001*/]System.String/*01000020*/
    IL_009a:  /* 13   | 08               */ stloc.s    V_8
    IL_009c:  /* 7E   | (04)000006       */ ldsfld     class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000005*/<valuetype JWT.JwtHashAlgorithm/*02000004*/,class [System.Core/*23000003*/]System.Func`3/*01000006*/<uint8[],uint8[],uint8[]>> JWT.JsonWebToken/*02000005*/::HashAlgorithms /* 04000006 */
    IL_00a1:  /* 11   | 08               */ ldloc.s    V_8
    IL_00a3:  /* 28   | (06)00000F       */ call       valuetype JWT.JwtHashAlgorithm/*02000004*/ JWT.JsonWebToken/*02000005*/::GetHashAlgorithm(string) /* 0600000F */
    IL_00a8:  /* 6F   | (0A)000026       */ callvirt   instance !1 class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000005*/<valuetype JWT.JwtHashAlgorithm/*02000004*/,class [System.Core/*23000003*/]System.Func`3/*01000006*/<uint8[],uint8[],uint8[]>>/*1B000001*/::get_Item(!0) /* 0A000026 */
    IL_00ad:  /* 03   |                  */ ldarg.1
    IL_00ae:  /* 11   | 07               */ ldloc.s    V_7
    IL_00b0:  /* 6F   | (0A)000027       */ callvirt   instance !2 class [System.Core/*23000003*/]System.Func`3/*01000006*/<uint8[],uint8[],uint8[]>/*1B000002*/::Invoke(!0,
                                                                                                                                                                         !1) /* 0A000027 */
    IL_00b5:  /* 13   | 09               */ stloc.s    V_9
    IL_00b7:  /* 09   |                  */ ldloc.3
    IL_00b8:  /* 28   | (0A)00002E       */ call       string [mscorlib/*23000001*/]System.Convert/*01000023*/::ToBase64String(uint8[]) /* 0A00002E */
    IL_00bd:  /* 13   | 0A               */ stloc.s    V_10
    IL_00bf:  /* 11   | 09               */ ldloc.s    V_9
    IL_00c1:  /* 28   | (0A)00002E       */ call       string [mscorlib/*23000001*/]System.Convert/*01000023*/::ToBase64String(uint8[]) /* 0A00002E */
    IL_00c6:  /* 13   | 0B               */ stloc.s    V_11
    IL_00c8:  /* 11   | 0A               */ ldloc.s    V_10
    IL_00ca:  /* 11   | 0B               */ ldloc.s    V_11
    IL_00cc:  /* 28   | (0A)00002F       */ call       bool [mscorlib/*23000001*/]System.String/*01000020*/::op_Inequality(string,
                                                                                                                           string) /* 0A00002F */
    IL_00d1:  /* 2C   | 14               */ brfalse.s  IL_00e7

    IL_00d3:  /* 72   | (70)00007F       */ ldstr      "Invalid signature. Expected {0} got {1}" /* 7000007F */
    IL_00d8:  /* 11   | 0A               */ ldloc.s    V_10
    IL_00da:  /* 11   | 0B               */ ldloc.s    V_11
    IL_00dc:  /* 28   | (0A)000030       */ call       string [mscorlib/*23000001*/]System.String/*01000020*/::Format(string,
                                                                                                                      object,
                                                                                                                      object) /* 0A000030 */
    IL_00e1:  /* 73   | (06)000015       */ newobj     instance void JWT.SignatureVerificationException/*02000006*/::.ctor(string) /* 06000015 */
    IL_00e6:  /* 7A   |                  */ throw

    IL_00e7:  /* 7E   | (04)000007       */ ldsfld     class JWT.IJsonSerializer/*02000002*/ JWT.JsonWebToken/*02000005*/::JsonSerializer /* 04000007 */
    IL_00ec:  /* 11   | 05               */ ldloc.s    V_5
    IL_00ee:  /* 6F   | (2B)000002       */ callvirt   instance !!0 JWT.IJsonSerializer/*02000002*/::Deserialize<class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000005*/<string,object>>(string) /* 2B000002 */
    IL_00f3:  /* 13   | 0C               */ stloc.s    V_12
    IL_00f5:  /* 11   | 0C               */ ldloc.s    V_12
    IL_00f7:  /* 72   | (70)0000CF       */ ldstr      "exp" /* 700000CF */
    IL_00fc:  /* 6F   | (0A)000031       */ callvirt   instance bool class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000005*/<string,object>/*1B000004*/::ContainsKey(!0) /* 0A000031 */
    IL_0101:  /* 2C   | 6B               */ brfalse.s  IL_016e

    IL_0103:  /* 11   | 0C               */ ldloc.s    V_12
    IL_0105:  /* 72   | (70)0000CF       */ ldstr      "exp" /* 700000CF */
    IL_010a:  /* 6F   | (0A)00002D       */ callvirt   instance !1 class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000005*/<string,object>/*1B000004*/::get_Item(!0) /* 0A00002D */
    IL_010f:  /* 2C   | 5D               */ brfalse.s  IL_016e

    .try
    {
      IL_0111:  /* 11   | 0C               */ ldloc.s    V_12
      IL_0113:  /* 72   | (70)0000CF       */ ldstr      "exp" /* 700000CF */
      IL_0118:  /* 6F   | (0A)00002D       */ callvirt   instance !1 class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000005*/<string,object>/*1B000004*/::get_Item(!0) /* 0A00002D */
      IL_011d:  /* 28   | (0A)000032       */ call       int32 [mscorlib/*23000001*/]System.Convert/*01000023*/::ToInt32(object) /* 0A000032 */
      IL_0122:  /* 13   | 0D               */ stloc.s    V_13
      IL_0124:  /* DE   | 0C               */ leave.s    IL_0132

    }  // end .try
    catch [mscorlib/*23000001*/]System.Exception/*01000003*/ 
    {
      IL_0126:  /* 26   |                  */ pop
      IL_0127:  /* 72   | (70)0000D7       */ ldstr      "Claim 'exp' must be an integer." /* 700000D7 */
      IL_012c:  /* 73   | (06)000015       */ newobj     instance void JWT.SignatureVerificationException/*02000006*/::.ctor(string) /* 06000015 */
      IL_0131:  /* 7A   |                  */ throw

    }  // end handler
    // HEX: 00 00 11 01 15 26 01 0C 03 00 00 01
    IL_0132:  /* 28   | (0A)000033       */ call       valuetype [mscorlib/*23000001*/]System.DateTime/*01000024*/ [mscorlib/*23000001*/]System.DateTime/*01000024*/::get_UtcNow() /* 0A000033 */
    IL_0137:  /* 20   | B2070000         */ ldc.i4     0x7b2
    IL_013c:  /* 17   |                  */ ldc.i4.1
    IL_013d:  /* 17   |                  */ ldc.i4.1
    IL_013e:  /* 16   |                  */ ldc.i4.0
    IL_013f:  /* 16   |                  */ ldc.i4.0
    IL_0140:  /* 16   |                  */ ldc.i4.0
    IL_0141:  /* 17   |                  */ ldc.i4.1
    IL_0142:  /* 73   | (0A)000034       */ newobj     instance void [mscorlib/*23000001*/]System.DateTime/*01000024*/::.ctor(int32,
                                                                                                                              int32,
                                                                                                                              int32,
                                                                                                                              int32,
                                                                                                                              int32,
                                                                                                                              int32,
                                                                                                                              valuetype [mscorlib/*23000001*/]System.DateTimeKind/*01000025*/) /* 0A000034 */
    IL_0147:  /* 28   | (0A)000035       */ call       valuetype [mscorlib/*23000001*/]System.TimeSpan/*01000026*/ [mscorlib/*23000001*/]System.DateTime/*01000024*/::op_Subtraction(valuetype [mscorlib/*23000001*/]System.DateTime/*01000024*/,
                                                                                                                                                                                     valuetype [mscorlib/*23000001*/]System.DateTime/*01000024*/) /* 0A000035 */
    IL_014c:  /* 13   | 10               */ stloc.s    V_16
    IL_014e:  /* 12   | 10               */ ldloca.s   V_16
    IL_0150:  /* 28   | (0A)000036       */ call       instance float64 [mscorlib/*23000001*/]System.TimeSpan/*01000026*/::get_TotalSeconds() /* 0A000036 */
    IL_0155:  /* 28   | (0A)000037       */ call       float64 [mscorlib/*23000001*/]System.Math/*01000027*/::Round(float64) /* 0A000037 */
    IL_015a:  /* 13   | 0E               */ stloc.s    V_14
    IL_015c:  /* 11   | 0E               */ ldloc.s    V_14
    IL_015e:  /* 11   | 0D               */ ldloc.s    V_13
    IL_0160:  /* 6C   |                  */ conv.r8
    IL_0161:  /* 37   | 0B               */ blt.un.s   IL_016e

    IL_0163:  /* 72   | (70)000117       */ ldstr      "Token has expired." /* 70000117 */
    IL_0168:  /* 73   | (06)000015       */ newobj     instance void JWT.SignatureVerificationException/*02000006*/::.ctor(string) /* 06000015 */
    IL_016d:  /* 7A   |                  */ throw

    IL_016e:  /* 11   | 05               */ ldloc.s    V_5
    IL_0170:  /* 2A   |                  */ ret
  } // end of method JsonWebToken::Decode

  .method /*0600000C*/ public hidebysig static 
          string  Decode(string token,
                         string key,
                         [opt] bool verify) cil managed
  // SIG: 00 03 0E 0E 0E 02
  {
    .param [3]/*08000018*/  = bool(true)
    // Method begins at RVA 0x2470
    // Code size       19 (0x13)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000021       */ call       class [mscorlib/*23000001*/]System.Text.Encoding/*0100001F*/ [mscorlib/*23000001*/]System.Text.Encoding/*0100001F*/::get_UTF8() /* 0A000021 */
    IL_0006:  /* 03   |                  */ ldarg.1
    IL_0007:  /* 6F   | (0A)000022       */ callvirt   instance uint8[] [mscorlib/*23000001*/]System.Text.Encoding/*0100001F*/::GetBytes(string) /* 0A000022 */
    IL_000c:  /* 04   |                  */ ldarg.2
    IL_000d:  /* 28   | (06)00000B       */ call       string JWT.JsonWebToken/*02000005*/::Decode(string,
                                                                                                   uint8[],
                                                                                                   bool) /* 0600000B */
    IL_0012:  /* 2A   |                  */ ret
  } // end of method JsonWebToken::Decode

  .method /*0600000D*/ public hidebysig static 
          object  DecodeToObject(string token,
                                 string key,
                                 [opt] bool verify) cil managed
  // SIG: 00 03 1C 0E 0E 02
  {
    .param [3]/*0800001B*/  = bool(true)
    // Method begins at RVA 0x2484
    // Code size       23 (0x17)
    .maxstack  3
    .locals /*11000007*/ init (string V_0,
             class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000005*/<string,object> V_1)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 04   |                  */ ldarg.2
    IL_0003:  /* 28   | (06)00000C       */ call       string JWT.JsonWebToken/*02000005*/::Decode(string,
                                                                                                   string,
                                                                                                   bool) /* 0600000C */
    IL_0008:  /* 0A   |                  */ stloc.0
    IL_0009:  /* 7E   | (04)000007       */ ldsfld     class JWT.IJsonSerializer/*02000002*/ JWT.JsonWebToken/*02000005*/::JsonSerializer /* 04000007 */
    IL_000e:  /* 06   |                  */ ldloc.0
    IL_000f:  /* 6F   | (2B)000002       */ callvirt   instance !!0 JWT.IJsonSerializer/*02000002*/::Deserialize<class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000005*/<string,object>>(string) /* 2B000002 */
    IL_0014:  /* 0B   |                  */ stloc.1
    IL_0015:  /* 07   |                  */ ldloc.1
    IL_0016:  /* 2A   |                  */ ret
  } // end of method JsonWebToken::DecodeToObject

  .method /*0600000E*/ public hidebysig static 
          !!T  DecodeToObject<T>(string token,
                                 string key,
                                 [opt] bool verify) cil managed
  // SIG: 10 01 03 1E 00 0E 0E 02
  {
    .param [3]/*0800001E*/  = bool(true)
    // Method begins at RVA 0x24a8
    // Code size       23 (0x17)
    .maxstack  3
    .locals /*11000008*/ init (string V_0,
             !!T V_1)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 04   |                  */ ldarg.2
    IL_0003:  /* 28   | (06)00000C       */ call       string JWT.JsonWebToken/*02000005*/::Decode(string,
                                                                                                   string,
                                                                                                   bool) /* 0600000C */
    IL_0008:  /* 0A   |                  */ stloc.0
    IL_0009:  /* 7E   | (04)000007       */ ldsfld     class JWT.IJsonSerializer/*02000002*/ JWT.JsonWebToken/*02000005*/::JsonSerializer /* 04000007 */
    IL_000e:  /* 06   |                  */ ldloc.0
    IL_000f:  /* 6F   | (2B)000003       */ callvirt   instance !!0 JWT.IJsonSerializer/*02000002*/::Deserialize<!!0>(string) /* 2B000003 */
    IL_0014:  /* 0B   |                  */ stloc.1
    IL_0015:  /* 07   |                  */ ldloc.1
    IL_0016:  /* 2A   |                  */ ret
  } // end of method JsonWebToken::DecodeToObject

  .method /*0600000F*/ private hidebysig static 
          valuetype JWT.JwtHashAlgorithm/*02000004*/ 
          GetHashAlgorithm(string 'algorithm') cil managed
  // SIG: 00 01 11 10 0E
  {
    // Method begins at RVA 0x24cc
    // Code size       63 (0x3f)
    .maxstack  2
    .locals /*11000009*/ init (string V_0)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 25   |                  */ dup
    IL_0002:  /* 0A   |                  */ stloc.0
    IL_0003:  /* 2C   | 2F               */ brfalse.s  IL_0034

    IL_0005:  /* 06   |                  */ ldloc.0
    IL_0006:  /* 72   | (70)00013D       */ ldstr      "HS256" /* 7000013D */
    IL_000b:  /* 28   | (0A)000038       */ call       bool [mscorlib/*23000001*/]System.String/*01000020*/::op_Equality(string,
                                                                                                                         string) /* 0A000038 */
    IL_0010:  /* 2D   | 1C               */ brtrue.s   IL_002e

    IL_0012:  /* 06   |                  */ ldloc.0
    IL_0013:  /* 72   | (70)000149       */ ldstr      "HS384" /* 70000149 */
    IL_0018:  /* 28   | (0A)000038       */ call       bool [mscorlib/*23000001*/]System.String/*01000020*/::op_Equality(string,
                                                                                                                         string) /* 0A000038 */
    IL_001d:  /* 2D   | 11               */ brtrue.s   IL_0030

    IL_001f:  /* 06   |                  */ ldloc.0
    IL_0020:  /* 72   | (70)000155       */ ldstr      "HS512" /* 70000155 */
    IL_0025:  /* 28   | (0A)000038       */ call       bool [mscorlib/*23000001*/]System.String/*01000020*/::op_Equality(string,
                                                                                                                         string) /* 0A000038 */
    IL_002a:  /* 2D   | 06               */ brtrue.s   IL_0032

    IL_002c:  /* 2B   | 06               */ br.s       IL_0034

    IL_002e:  /* 16   |                  */ ldc.i4.0
    IL_002f:  /* 2A   |                  */ ret

    IL_0030:  /* 17   |                  */ ldc.i4.1
    IL_0031:  /* 2A   |                  */ ret

    IL_0032:  /* 18   |                  */ ldc.i4.2
    IL_0033:  /* 2A   |                  */ ret

    IL_0034:  /* 72   | (70)000161       */ ldstr      "Algorithm not supported." /* 70000161 */
    IL_0039:  /* 73   | (06)000015       */ newobj     instance void JWT.SignatureVerificationException/*02000006*/::.ctor(string) /* 06000015 */
    IL_003e:  /* 7A   |                  */ throw
  } // end of method JsonWebToken::GetHashAlgorithm

  .method /*06000010*/ public hidebysig static 
          string  Base64UrlEncode(uint8[] input) cil managed
  // SIG: 00 01 0E 1D 05
  {
    // Method begins at RVA 0x2518
    // Code size       53 (0x35)
    .maxstack  4
    .locals /*1100000A*/ init (string V_0,
             char[] V_1)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00002E       */ call       string [mscorlib/*23000001*/]System.Convert/*01000023*/::ToBase64String(uint8[]) /* 0A00002E */
    IL_0006:  /* 0A   |                  */ stloc.0
    IL_0007:  /* 06   |                  */ ldloc.0
    IL_0008:  /* 17   |                  */ ldc.i4.1
    IL_0009:  /* 8D   | (01)000021       */ newarr     [mscorlib/*23000001*/]System.Char/*01000021*/
    IL_000e:  /* 0B   |                  */ stloc.1
    IL_000f:  /* 07   |                  */ ldloc.1
    IL_0010:  /* 16   |                  */ ldc.i4.0
    IL_0011:  /* 1F   | 3D               */ ldc.i4.s   61
    IL_0013:  /* 9D   |                  */ stelem.i2
    IL_0014:  /* 07   |                  */ ldloc.1
    IL_0015:  /* 6F   | (0A)000029       */ callvirt   instance string[] [mscorlib/*23000001*/]System.String/*01000020*/::Split(char[]) /* 0A000029 */
    IL_001a:  /* 16   |                  */ ldc.i4.0
    IL_001b:  /* 9A   |                  */ ldelem.ref
    IL_001c:  /* 0A   |                  */ stloc.0
    IL_001d:  /* 06   |                  */ ldloc.0
    IL_001e:  /* 1F   | 2B               */ ldc.i4.s   43
    IL_0020:  /* 1F   | 2D               */ ldc.i4.s   45
    IL_0022:  /* 6F   | (0A)000039       */ callvirt   instance string [mscorlib/*23000001*/]System.String/*01000020*/::Replace(char,
                                                                                                                                char) /* 0A000039 */
    IL_0027:  /* 0A   |                  */ stloc.0
    IL_0028:  /* 06   |                  */ ldloc.0
    IL_0029:  /* 1F   | 2F               */ ldc.i4.s   47
    IL_002b:  /* 1F   | 5F               */ ldc.i4.s   95
    IL_002d:  /* 6F   | (0A)000039       */ callvirt   instance string [mscorlib/*23000001*/]System.String/*01000020*/::Replace(char,
                                                                                                                                char) /* 0A000039 */
    IL_0032:  /* 0A   |                  */ stloc.0
    IL_0033:  /* 06   |                  */ ldloc.0
    IL_0034:  /* 2A   |                  */ ret
  } // end of method JsonWebToken::Base64UrlEncode

  .method /*06000011*/ public hidebysig static 
          uint8[]  Base64UrlDecode(string input) cil managed
  // SIG: 00 01 1D 05 0E
  {
    // Method begins at RVA 0x255c
    // Code size       105 (0x69)
    .maxstack  3
    .locals /*1100000B*/ init (string V_0,
             uint8[] V_1,
             int32 V_2)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 0A   |                  */ stloc.0
    IL_0002:  /* 06   |                  */ ldloc.0
    IL_0003:  /* 1F   | 2D               */ ldc.i4.s   45
    IL_0005:  /* 1F   | 2B               */ ldc.i4.s   43
    IL_0007:  /* 6F   | (0A)000039       */ callvirt   instance string [mscorlib/*23000001*/]System.String/*01000020*/::Replace(char,
                                                                                                                                char) /* 0A000039 */
    IL_000c:  /* 0A   |                  */ stloc.0
    IL_000d:  /* 06   |                  */ ldloc.0
    IL_000e:  /* 1F   | 5F               */ ldc.i4.s   95
    IL_0010:  /* 1F   | 2F               */ ldc.i4.s   47
    IL_0012:  /* 6F   | (0A)000039       */ callvirt   instance string [mscorlib/*23000001*/]System.String/*01000020*/::Replace(char,
                                                                                                                                char) /* 0A000039 */
    IL_0017:  /* 0A   |                  */ stloc.0
    IL_0018:  /* 06   |                  */ ldloc.0
    IL_0019:  /* 6F   | (0A)00003A       */ callvirt   instance int32 [mscorlib/*23000001*/]System.String/*01000020*/::get_Length() /* 0A00003A */
    IL_001e:  /* 1A   |                  */ ldc.i4.4
    IL_001f:  /* 5D   |                  */ rem
    IL_0020:  /* 0C   |                  */ stloc.2
    IL_0021:  /* 08   |                  */ ldloc.2
    IL_0022:  /* 45   | 04000000         */ switch     ( 
              /*      | 29000000         */             IL_0060,
              /*      | 1E000000         */             IL_0055,
              /*      | 02000000         */             IL_0039,
              /*      | 10000000         */             IL_0047)
    IL_0037:  /* 2B   | 1C               */ br.s       IL_0055

    IL_0039:  /* 06   |                  */ ldloc.0
    IL_003a:  /* 72   | (70)000193       */ ldstr      "==" /* 70000193 */
    IL_003f:  /* 28   | (0A)00003B       */ call       string [mscorlib/*23000001*/]System.String/*01000020*/::Concat(string,
                                                                                                                      string) /* 0A00003B */
    IL_0044:  /* 0A   |                  */ stloc.0
    IL_0045:  /* 2B   | 19               */ br.s       IL_0060

    IL_0047:  /* 06   |                  */ ldloc.0
    IL_0048:  /* 72   | (70)000199       */ ldstr      "=" /* 70000199 */
    IL_004d:  /* 28   | (0A)00003B       */ call       string [mscorlib/*23000001*/]System.String/*01000020*/::Concat(string,
                                                                                                                      string) /* 0A00003B */
    IL_0052:  /* 0A   |                  */ stloc.0
    IL_0053:  /* 2B   | 0B               */ br.s       IL_0060

    IL_0055:  /* 72   | (70)00019D       */ ldstr      "Illegal base64url string!" /* 7000019D */
    IL_005a:  /* 73   | (0A)00003C       */ newobj     instance void [mscorlib/*23000001*/]System.Exception/*01000003*/::.ctor(string) /* 0A00003C */
    IL_005f:  /* 7A   |                  */ throw

    IL_0060:  /* 06   |                  */ ldloc.0
    IL_0061:  /* 28   | (0A)00003D       */ call       uint8[] [mscorlib/*23000001*/]System.Convert/*01000023*/::FromBase64String(string) /* 0A00003D */
    IL_0066:  /* 0B   |                  */ stloc.1
    IL_0067:  /* 07   |                  */ ldloc.1
    IL_0068:  /* 2A   |                  */ ret
  } // end of method JsonWebToken::Base64UrlDecode

  .method /*06000012*/ private hidebysig static 
          uint8[]  '<.cctor>b__1'(uint8[] key,
                                  uint8[] 'value') cil managed
  // SIG: 00 02 1D 05 1D 05 1D 05
  {
    .custom /*0C000011:0A000014*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000018*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2080
    // Code size       29 (0x1d)
    .maxstack  2
    .locals /*11000001*/ init (class [mscorlib/*23000001*/]System.Security.Cryptography.HMACSHA256/*01000019*/ V_0,
             uint8[] V_1)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 73   | (0A)000015       */ newobj     instance void [mscorlib/*23000001*/]System.Security.Cryptography.HMACSHA256/*01000019*/::.ctor(uint8[]) /* 0A000015 */
    IL_0006:  /* 0A   |                  */ stloc.0
    .try
    {
      IL_0007:  /* 06   |                  */ ldloc.0
      IL_0008:  /* 03   |                  */ ldarg.1
      IL_0009:  /* 6F   | (0A)000016       */ callvirt   instance uint8[] [mscorlib/*23000001*/]System.Security.Cryptography.HashAlgorithm/*0100001A*/::ComputeHash(uint8[]) /* 0A000016 */
      IL_000e:  /* 0B   |                  */ stloc.1
      IL_000f:  /* DE   | 0A               */ leave.s    IL_001b

    }  // end .try
    finally
    {
      IL_0011:  /* 06   |                  */ ldloc.0
      IL_0012:  /* 2C   | 06               */ brfalse.s  IL_001a

      IL_0014:  /* 06   |                  */ ldloc.0
      IL_0015:  /* 6F   | (0A)000017       */ callvirt   instance void [mscorlib/*23000001*/]System.IDisposable/*0100001B*/::Dispose() /* 0A000017 */
      IL_001a:  /* DC   |                  */ endfinally
    }  // end handler
    // HEX: 02 00 07 00 0A 11 00 0A 00 00 00 00
    IL_001b:  /* 07   |                  */ ldloc.1
    IL_001c:  /* 2A   |                  */ ret
  } // end of method JsonWebToken::'<.cctor>b__1'

  .method /*06000013*/ private hidebysig static 
          uint8[]  '<.cctor>b__2'(uint8[] key,
                                  uint8[] 'value') cil managed
  // SIG: 00 02 1D 05 1D 05 1D 05
  {
    .custom /*0C000012:0A000014*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000018*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20bc
    // Code size       29 (0x1d)
    .maxstack  2
    .locals /*11000002*/ init (class [mscorlib/*23000001*/]System.Security.Cryptography.HMACSHA384/*0100001C*/ V_0,
             uint8[] V_1)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 73   | (0A)000018       */ newobj     instance void [mscorlib/*23000001*/]System.Security.Cryptography.HMACSHA384/*0100001C*/::.ctor(uint8[]) /* 0A000018 */
    IL_0006:  /* 0A   |                  */ stloc.0
    .try
    {
      IL_0007:  /* 06   |                  */ ldloc.0
      IL_0008:  /* 03   |                  */ ldarg.1
      IL_0009:  /* 6F   | (0A)000016       */ callvirt   instance uint8[] [mscorlib/*23000001*/]System.Security.Cryptography.HashAlgorithm/*0100001A*/::ComputeHash(uint8[]) /* 0A000016 */
      IL_000e:  /* 0B   |                  */ stloc.1
      IL_000f:  /* DE   | 0A               */ leave.s    IL_001b

    }  // end .try
    finally
    {
      IL_0011:  /* 06   |                  */ ldloc.0
      IL_0012:  /* 2C   | 06               */ brfalse.s  IL_001a

      IL_0014:  /* 06   |                  */ ldloc.0
      IL_0015:  /* 6F   | (0A)000017       */ callvirt   instance void [mscorlib/*23000001*/]System.IDisposable/*0100001B*/::Dispose() /* 0A000017 */
      IL_001a:  /* DC   |                  */ endfinally
    }  // end handler
    // HEX: 02 00 07 00 0A 11 00 0A 00 00 00 00
    IL_001b:  /* 07   |                  */ ldloc.1
    IL_001c:  /* 2A   |                  */ ret
  } // end of method JsonWebToken::'<.cctor>b__2'

  .method /*06000014*/ private hidebysig static 
          uint8[]  '<.cctor>b__3'(uint8[] key,
                                  uint8[] 'value') cil managed
  // SIG: 00 02 1D 05 1D 05 1D 05
  {
    .custom /*0C000013:0A000014*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000018*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20f8
    // Code size       29 (0x1d)
    .maxstack  2
    .locals /*11000003*/ init (class [mscorlib/*23000001*/]System.Security.Cryptography.HMACSHA512/*0100001D*/ V_0,
             uint8[] V_1)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 73   | (0A)000019       */ newobj     instance void [mscorlib/*23000001*/]System.Security.Cryptography.HMACSHA512/*0100001D*/::.ctor(uint8[]) /* 0A000019 */
    IL_0006:  /* 0A   |                  */ stloc.0
    .try
    {
      IL_0007:  /* 06   |                  */ ldloc.0
      IL_0008:  /* 03   |                  */ ldarg.1
      IL_0009:  /* 6F   | (0A)000016       */ callvirt   instance uint8[] [mscorlib/*23000001*/]System.Security.Cryptography.HashAlgorithm/*0100001A*/::ComputeHash(uint8[]) /* 0A000016 */
      IL_000e:  /* 0B   |                  */ stloc.1
      IL_000f:  /* DE   | 0A               */ leave.s    IL_001b

    }  // end .try
    finally
    {
      IL_0011:  /* 06   |                  */ ldloc.0
      IL_0012:  /* 2C   | 06               */ brfalse.s  IL_001a

      IL_0014:  /* 06   |                  */ ldloc.0
      IL_0015:  /* 6F   | (0A)000017       */ callvirt   instance void [mscorlib/*23000001*/]System.IDisposable/*0100001B*/::Dispose() /* 0A000017 */
      IL_001a:  /* DC   |                  */ endfinally
    }  // end handler
    // HEX: 02 00 07 00 0A 11 00 0A 00 00 00 00
    IL_001b:  /* 07   |                  */ ldloc.1
    IL_001c:  /* 2A   |                  */ ret
  } // end of method JsonWebToken::'<.cctor>b__3'

} // end of class JWT.JsonWebToken

.class /*02000006*/ public auto ansi beforefieldinit JWT.SignatureVerificationException
       extends [mscorlib/*23000001*/]System.Exception/*01000003*/
{
  .method /*06000015*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x25d1
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 28   | (0A)00003C       */ call       instance void [mscorlib/*23000001*/]System.Exception/*01000003*/::.ctor(string) /* 0A00003C */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method SignatureVerificationException::.ctor

} // end of class JWT.SignatureVerificationException


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
// WARNING: Created Win32 resource file JWT.res
