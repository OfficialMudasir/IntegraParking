
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
// Time-date stamp:            0x4c9e0286
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2102
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x08
// Minor linker version:           0x00
// Size of code:                   0x0000d200
// Size of init.data:              0x00000600
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x0000f09e
// Base of code:                   0x00002000
// Base of data:                   0x00010000
// Image base:                     0x00400000
// Section alignment:              0x00002000
// File alignment:                 0x00000200
// Major OS version:               0x0004
// Minor OS version:               0x0000
// Major image version:            0x0000
// Minor image version:            0x0000
// Major subsystem version:        0x0004
// Minor subsystem version:        0x0000
// Size of image:                  0x00014000
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
// 0x0000f050 [0x0000004b] address [size] of Import Directory:          
// 0x00010000 [0x000003b8] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00000000 [0x00000000] address [size] of Security Directory:        
// 0x00012000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x0000efc4 [0x0000001c] address [size] of Debug Directory:           
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
//              0x0000d0a4 Virtual Size
//              0x00002000 Virtual Address
//              0x0000d200 Size of Raw Data
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
//              0x000003b8 Virtual Size
//              0x00010000 Virtual Address
//              0x00000400 Size of Raw Data
//              0x0000d400 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x40000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_READ

//              .reloc
//              0x0000000c Virtual Size
//              0x00012000 Virtual Address
//              0x00000200 Size of Raw Data
//              0x0000d800 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x42000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_DISCARDABLE
//                         MEM_READ

// Base Relocation Table
//              0x0000f000 Page RVA
//              0x0000000c Block Size
//              0x00000002 Number of Entries
//              Entry 1: Type 0x3 Offset 0x000000a0
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x0000f08e Import Name Table
//              0          Time Date Stamp
//              0          Index of First Forwarder Reference
//
//              0x0000  _CorDllMain

// Delay Load Import Address Table
// No data.

// Entry point code:
//FF 25 00 20 40 00 


// ----- CLR Header:
// Header size:                        0x00000048
// Major runtime version:              0x0002
// Minor runtime version:              0x0005
// 0x000059d4 [0x000095f0] address [size] of Metadata Directory:        
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
//              0x00004824 Size
//              '#~' Name
//    Stream 2:
//              0x00004890 Offset
//              0x00002b90 Size
//              '#Strings' Name
//    Stream 3:
//              0x00007420 Offset
//              0x00000a48 Size
//              '#US' Name
//    Stream 4:
//              0x00007e68 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00007e78 Offset
//              0x00001778 Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x00001e090ba21f57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 55808
// PE header size       : 512 (496 used)    ( 0.92%)
// PE additional info   : 1147              ( 2.06%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.13%)
// CLR meta-data size  : 38384              (68.78%)
// CLR additional info : 0                  ( 0.00%)
// CLR method headers  : 1515               ( 2.71%)
// Managed code         : 13104             (23.48%)
// Data                 : 1536              ( 2.75%)
// Unaccounted          : -462              (-0.83%)

// Num.of PE sections   : 3
//   .text    - 53760
//   .rsrc    - 1024
//   .reloc   - 512

// CLR meta-data size  : 38384
//   Module        -    1 (10 bytes)
//   TypeDef       -   93 (1302 bytes)    13 interfaces, 0 explicit layout
//   TypeRef       -  118 (708 bytes)
//   MethodDef     -  438 (6132 bytes)    29 abstract, 0 native, 409 bodies
//   FieldDef      -  128 (768 bytes)     4 constant
//   MemberRef     -  493 (2958 bytes)
//   ParamDef      -  480 (2880 bytes)
//   MethodImpl    -   21 (126 bytes)
//   Constant      -   39 (234 bytes)
//   CustomAttribute-  149 (894 bytes)
//   StandAloneSig -   51 (102 bytes)
//   InterfaceImpl -   30 (120 bytes)
//   PropertyMap   -   19 (76 bytes)
//   Property      -   46 (276 bytes)
//   MethodSemantic-   63 (378 bytes)
//   TypeSpec      -  132 (264 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    5 (100 bytes)
//   NestedClass   -    8 (32 bytes)
//   GenericParam  -   90 (720 bytes)
//   MethodSpec    -   31 (124 bytes)
//   GenericParamConstraint-   32 (128 bytes)
//   Strings       - 11152 bytes
//   Blobs         -  6008 bytes
//   UserStrings   -  2632 bytes
//   Guids         -    16 bytes
//   Uncategorized -   222 bytes

// CLR method headers : 1515
//   Num.of method bodies  - 409
//   Num.of fat headers    - 80
//   Num.of tiny headers   - 329
//   Num.of fat sections   - 0
//   Num.of small sections - 12

// Managed code : 13104
//   Ave method size - 32
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Interface IBehaviorMarker                (public) (abstract) (auto) (ansi) 
// Interface IBehavior`1                    (public) (abstract) (auto) (ansi) 
// Class ThreadSafeBehavior             (public) (abstract) (auto) (ansi) 
// Class AutoLabelBehavior              (public) (auto) (ansi) 
// Class AutoLabelSettings              (public) (auto) (ansi) 
// Class CacheingBehaviorApplier        (public) (auto) (ansi) 
// Class ThreadSafeDictionary`2         (public) (auto) (ansi) 
// Class TypeToBehaviorMap              (auto) (ansi) (nested private) 
// Class BehaviorMarkerExtensions       (public) (abstract) (auto) (ansi) (sealed) 
// Class BehaviorHelperExtensions       (public) (abstract) (auto) (ansi) (sealed) 
// Class ThreadSafeMemberBehavior       (public) (abstract) (auto) (ansi) 
// Class RequiredCssClassBehavior       (public) (auto) (ansi) 
// Class RequiredCssClassBehavior`1     (public) (auto) (ansi) 
// Class GenericTypeSpec                (public) (auto) (ansi) 
// Interface IOrderedBehavior               (public) (abstract) (auto) (ansi) 
// Interface IOrderedBehavior`1             (public) (abstract) (auto) (ansi) 
// Interface IBehavior                      (public) (abstract) (auto) (ansi) 
// Interface IBehaviorsContainer            (public) (abstract) (auto) (ansi) 
// Interface IModelStateHandler             (public) (abstract) (auto) (ansi) 
// Interface ISupportsMaxLength             (public) (abstract) (auto) (ansi) 
// Interface ISupportsModelState            (public) (abstract) (auto) (ansi) 
// Class MemberBehaviorHelper`1         (public) (auto) (ansi) 
// Class ValidationBehavior             (public) (auto) (ansi) 
// Interface IMemberBehavior                (public) (abstract) (auto) (ansi) 
// Class TypeCssClassBehavior           (public) (auto) (ansi) 
// Interface IElement                       (public) (abstract) (auto) (ansi) 
// Interface IMemberElement                 (public) (abstract) (auto) (ansi) 
// Class Element`1                      (public) (abstract) (auto) (ansi) 
// Class DisableableElement`1           (public) (abstract) (auto) (ansi) 
// Class FormElement`1                  (public) (abstract) (auto) (ansi) 
// Class Input`1                        (public) (abstract) (auto) (ansi) 
// Class ButtonBase`1                   (public) (abstract) (auto) (ansi) 
// Class Button                         (public) (auto) (ansi) 
// Class CheckBoxBase`1                 (public) (abstract) (auto) (ansi) 
// Class CheckBox                       (public) (auto) (ansi) 
// Class OptionsElementBase`1           (public) (abstract) (auto) (ansi) 
// Class CheckBoxListBase`1             (public) (abstract) (auto) (ansi) 
// Class CheckBoxList                   (public) (auto) (ansi) 
// Class LabelBase`1                    (public) (auto) (ansi) 
// Class Label                          (public) (auto) (ansi) 
// Class ResetButtonBase`1              (public) (abstract) (auto) (ansi) 
// Class ResetButton                    (public) (auto) (ansi) 
// Class TextInput`1                    (public) (abstract) (auto) (ansi) 
// Class Hidden                         (public) (auto) (ansi) 
// Class Index                          (public) (auto) (ansi) 
// Class FileUpload                     (public) (auto) (ansi) 
// Class FormLiteralBase`1              (public) (abstract) (auto) (ansi) 
// Class FormLiteral                    (public) (auto) (ansi) 
// Class LiteralBase`1                  (public) (abstract) (auto) (ansi) 
// Class RadioButtonBase`1              (public) (abstract) (auto) (ansi) 
// Class RadioButton                    (public) (auto) (ansi) 
// Class RadioSetBase`1                 (public) (abstract) (auto) (ansi) 
// Class RadioSet                       (public) (auto) (ansi) 
// Class TypeStamp                      (public) (auto) (ansi) 
// Class ModelStateDictionaryExtensions (public) (abstract) (auto) (ansi) (sealed) 
// Class PartialNameConventionService   (public) (abstract) (auto) (ansi) (sealed) 
// Class PartialRenderer`2              (public) (auto) (ansi) 
// Class StringExtensions               (public) (abstract) (auto) (ansi) (sealed) 
// Class SubmitButtonBase`1             (public) (abstract) (auto) (ansi) 
// Class TextAreaBase`1                 (public) (abstract) (auto) (ansi) 
// Class ValidationMessage              (public) (auto) (ansi) 
// Class ExpressionNameVisitor          (public) (auto) (ansi) 
// Class ExpressionNameVisitorResult    (public) (auto) (ansi) 
// Class HtmlEventAttribute             (public) (abstract) (auto) (ansi) (sealed) 
// Class HtmlInputType                  (public) (abstract) (auto) (ansi) (sealed) 
// Class HtmlTag                        (public) (abstract) (auto) (ansi) (sealed) 
// Class Literal                        (public) (auto) (ansi) 
// Class SelectBase`1                   (public) (abstract) (auto) (ansi) 
// Class MultiSelect                    (public) (auto) (ansi) 
// Class MultiSelect`1                  (public) (auto) (ansi) 
// Class Option                         (public) (auto) (ansi) 
// Class Password                       (public) (auto) (ansi) 
// Class Select                         (public) (auto) (ansi) 
// Class Select`1                       (public) (auto) (ansi) 
// Class SubmitButton                   (public) (auto) (ansi) 
// Class TextArea                       (public) (auto) (ansi) 
// Class TextBox                        (public) (auto) (ansi) 
// Class HtmlAttribute                  (public) (abstract) (auto) (ansi) (sealed) 
// Interface IViewModelContainer`1          (public) (abstract) (auto) (ansi) 
// Class ExpressionExtensions           (public) (abstract) (auto) (ansi) (sealed) 
// Class ModelViewMasterPage`1          (public) (auto) (ansi) 
// Class ModelViewPage`1                (public) (auto) (ansi) 
// Class ModelViewUserControl`1         (public) (auto) (ansi) 
// Class ViewDataContainerExtensions    (public) (abstract) (auto) (ansi) (sealed) 
// Class ViewModelContainerExtensions   (public) (abstract) (auto) (ansi) (sealed) 
// Class <>c__DisplayClass3             (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass8             (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClassc             (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClasse             (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClassa`1           (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClasse             (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass1             (auto) (ansi) (sealed) (nested private) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v2.0.50727
.assembly extern /*23000001*/ mscorlib
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 2:0:0:0
}
.assembly extern /*23000002*/ System.Core
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 3:5:0:0
}
.assembly extern /*23000003*/ System.Web.Mvc
{
  .publickeytoken = (31 BF 38 56 AD 36 4E 35 )                         // 1.8V.6N5
  .ver 2:0:0:0
}
.assembly extern /*23000004*/ System
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 2:0:0:0
}
.assembly extern /*23000005*/ System.Web
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 2:0:0:0
}
.assembly /*20000001*/ MvcContrib.FluentHtml
{

  // --- The following custom attribute is added automatically, do not uncomment -------
  //  .custom /*0C000001:0A00000C*/ instance void [mscorlib/*23000001*/]System.Diagnostics.DebuggableAttribute/*0100002C*/::.ctor(valuetype [mscorlib/*23000001*/]System.Diagnostics.DebuggableAttribute/*0100002C*//DebuggingModes/*0100002D*/) /* 0A00000C */ = ( 01 00 02 00 00 00 00 00 ) 

  .custom /*0C000002:0A00000D*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilationRelaxationsAttribute/*0100002E*/::.ctor(int32) /* 0A00000D */ = ( 01 00 08 00 00 00 00 00 ) 
  .custom /*0C000003:0A00000E*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute/*0100002F*/::.ctor() /* 0A00000E */ = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
                                                                                                                                                                          63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )       // ceptionThrows.
  .custom /*0C000004:0A00000B*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100002B*/::.ctor(string) /* 0A00000B */ = ( 01 00 08 32 2E 30 2E 39 36 2E 30 00 00 )          // ...2.0.96.0..
  .custom /*0C000005:0A000003*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000023*/::.ctor(string) /* 0A000003 */ = ( 01 00 15 4D 76 63 43 6F 6E 74 72 69 62 2E 46 6C   // ...MvcContrib.Fl
                                                                                                                                                           75 65 6E 74 48 74 6D 6C 00 00 )                   // uentHtml..
  .custom /*0C000006:0A000004*/ instance void [mscorlib/*23000001*/]System.Runtime.InteropServices.ComVisibleAttribute/*01000024*/::.ctor(bool) /* 0A000004 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000007:0A000009*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*01000029*/::.ctor(string) /* 0A000009 */ = ( 01 00 0B 4D 56 43 20 43 6F 6E 74 72 69 62 00 00 ) // ...MVC Contrib..
  .custom /*0C000008:0A00000A*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyConfigurationAttribute/*0100002A*/::.ctor(string) /* 0A00000A */ = ( 01 00 07 72 65 6C 65 61 73 65 00 00 )             // ...release..
  .custom /*0C000009:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
  .custom /*0C00000A:0A000006*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*01000026*/::.ctor(string) /* 0A000006 */ = ( 01 00 08 32 2E 30 2E 39 36 2E 30 00 00 )          // ...2.0.96.0..
  .custom /*0C00000B:0A000007*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*01000027*/::.ctor(string) /* 0A000007 */ = ( 01 00 22 43 6F 70 79 72 69 67 68 74 20 C2 A9 20   // .."Copyright .. 
                                                                                                                                                               4D 56 43 20 43 6F 6E 74 72 69 62 20 32 30 30 37   // MVC Contrib 2007
                                                                                                                                                               2D 32 30 31 30 00 00 )                            // -2010..
  .custom /*0C00000C:0A000008*/ instance void [mscorlib/*23000001*/]System.Reflection.AssemblyProductAttribute/*01000028*/::.ctor(string) /* 0A000008 */ = ( 01 00 0A 4D 56 43 43 6F 6E 74 72 69 62 00 00 )    // ...MVCContrib..
  .hash algorithm 0x00008004
  .ver 2:0:96:0
}
.module MvcContrib.FluentHtml.dll
// MVID: {A5C9498F-6CA3-476A-A35E-51DFF2BE4D4B}
.imagebase 0x00400000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000001    //  ILONLY
// Image base: 0x03250000


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ interface public abstract auto ansi MvcContrib.FluentHtml.Behaviors.IBehaviorMarker
{
} // end of class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker

.class /*02000003*/ interface public abstract auto ansi MvcContrib.FluentHtml.Behaviors.IBehavior`1<T>
       implements MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/
{
  .method /*06000001*/ public hidebysig newslot abstract virtual 
          instance void  Execute(!T behavee) cil managed
  // SIG: 20 01 01 13 00
  {
    // Method begins at RVA 0x0
  } // end of method IBehavior`1::Execute

} // end of class MvcContrib.FluentHtml.Behaviors.IBehavior`1

.class /*02000004*/ public abstract auto ansi beforefieldinit MvcContrib.FluentHtml.Behaviors.ThreadSafeBehavior
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
       implements class MvcContrib.FluentHtml.Behaviors.IBehavior`1/*02000003*/<class MvcContrib.FluentHtml.Elements.IElement/*0200001B*/>/*1B000001*/,
                  MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/
{
  .field /*04000001*/ private static initonly object objLock
  .method /*06000002*/ public hidebysig newslot virtual final 
          instance void  Execute(class MvcContrib.FluentHtml.Elements.IElement/*0200001B*/ element) cil managed
  // SIG: 20 01 01 12 6C
  {
    // Method begins at RVA 0x2050
    // Code size       29 (0x1d)
    .maxstack  2
    .locals /*11000001*/ init (object V_0)
    IL_0000:  /* 7E   | (04)000001       */ ldsfld     object MvcContrib.FluentHtml.Behaviors.ThreadSafeBehavior/*02000004*/::objLock /* 04000001 */
    IL_0005:  /* 25   |                  */ dup
    IL_0006:  /* 0A   |                  */ stloc.0
    IL_0007:  /* 28   | (0A)000010       */ call       void [mscorlib/*23000001*/]System.Threading.Monitor/*01000031*/::Enter(object) /* 0A000010 */
    .try
    {
      IL_000c:  /* 02   |                  */ ldarg.0
      IL_000d:  /* 03   |                  */ ldarg.1
      IL_000e:  /* 6F   | (06)000003       */ callvirt   instance void MvcContrib.FluentHtml.Behaviors.ThreadSafeBehavior/*02000004*/::DoExecute(class MvcContrib.FluentHtml.Elements.IElement/*0200001B*/) /* 06000003 */
      IL_0013:  /* DE   | 07               */ leave.s    IL_001c

    }  // end .try
    finally
    {
      IL_0015:  /* 06   |                  */ ldloc.0
      IL_0016:  /* 28   | (0A)000011       */ call       void [mscorlib/*23000001*/]System.Threading.Monitor/*01000031*/::Exit(object) /* 0A000011 */
      IL_001b:  /* DC   |                  */ endfinally
    }  // end handler
    // HEX: 02 00 0C 00 09 15 00 07 00 00 00 00
    IL_001c:  /* 2A   |                  */ ret
  } // end of method ThreadSafeBehavior::Execute

  .method /*06000003*/ family hidebysig newslot abstract virtual 
          instance void  DoExecute(class MvcContrib.FluentHtml.Elements.IElement/*0200001B*/ element) cil managed
  // SIG: 20 01 01 12 6C
  {
    // Method begins at RVA 0x0
  } // end of method ThreadSafeBehavior::DoExecute

  .method /*06000004*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2098
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000012       */ call       instance void [mscorlib/*23000001*/]System.Object/*01000001*/::.ctor() /* 0A000012 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ThreadSafeBehavior::.ctor

  .method /*06000005*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x208c
    // Code size       11 (0xb)
    .maxstack  8
    IL_0000:  /* 73   | (0A)000012       */ newobj     instance void [mscorlib/*23000001*/]System.Object/*01000001*/::.ctor() /* 0A000012 */
    IL_0005:  /* 80   | (04)000001       */ stsfld     object MvcContrib.FluentHtml.Behaviors.ThreadSafeBehavior/*02000004*/::objLock /* 04000001 */
    IL_000a:  /* 2A   |                  */ ret
  } // end of method ThreadSafeBehavior::.cctor

} // end of class MvcContrib.FluentHtml.Behaviors.ThreadSafeBehavior

.class /*02000005*/ public auto ansi beforefieldinit MvcContrib.FluentHtml.Behaviors.AutoLabelBehavior
       extends MvcContrib.FluentHtml.Behaviors.ThreadSafeBehavior/*02000004*/
{
  .field /*04000002*/ private initonly class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class MvcContrib.FluentHtml.Elements.IElement/*0200001B*/,bool> isQualifiedFunc
  .field /*04000003*/ private initonly class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class MvcContrib.FluentHtml.Elements.IElement/*0200001B*/,bool> renderLabelAfterFunc
  .field /*04000004*/ private static class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class MvcContrib.FluentHtml.Elements.IElement/*0200001B*/,bool> 'CS$<>9__CachedAnonymousMethodDelegate1'
  .custom /*0C00000D:0A000013*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000032*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
  .method /*06000006*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20a0
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 14   |                  */ ldnull
    IL_0003:  /* 28   | (06)000007       */ call       instance void MvcContrib.FluentHtml.Behaviors.AutoLabelBehavior/*02000005*/::.ctor(class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class MvcContrib.FluentHtml.Elements.IElement/*0200001B*/,bool>,
                                                                                                                                          class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class MvcContrib.FluentHtml.Elements.IElement/*0200001B*/,bool>) /* 06000007 */
    IL_0008:  /* 2A   |                  */ ret
  } // end of method AutoLabelBehavior::.ctor

  .method /*06000007*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class MvcContrib.FluentHtml.Elements.IElement/*0200001B*/,bool> isQualifiedFunc,
                               class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class MvcContrib.FluentHtml.Elements.IElement/*0200001B*/,bool> renderLabelAfterFunc) cil managed
  // SIG: 20 02 01 15 12 2D 02 12 6C 02 15 12 2D 02 12 6C 02
  {
    // Method begins at RVA 0x20ca
    // Code size       54 (0x36)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000004       */ call       instance void MvcContrib.FluentHtml.Behaviors.ThreadSafeBehavior/*02000004*/::.ctor() /* 06000004 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 25   |                  */ dup
    IL_0009:  /* 2D   | 1E               */ brtrue.s   IL_0029

    IL_000b:  /* 26   |                  */ pop
    IL_000c:  /* 7E   | (04)000004       */ ldsfld     class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class MvcContrib.FluentHtml.Elements.IElement/*0200001B*/,bool> MvcContrib.FluentHtml.Behaviors.AutoLabelBehavior/*02000005*/::'CS$<>9__CachedAnonymousMethodDelegate1' /* 04000004 */
    IL_0011:  /* 2D   | 11               */ brtrue.s   IL_0024

    IL_0013:  /* 14   |                  */ ldnull
    IL_0014:  /* FE06 | (06)000009       */ ldftn      bool MvcContrib.FluentHtml.Behaviors.AutoLabelBehavior/*02000005*/::'<.ctor>b__0'(class MvcContrib.FluentHtml.Elements.IElement/*0200001B*/) /* 06000009 */
    IL_001a:  /* 73   | (0A)000014       */ newobj     instance void class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class MvcContrib.FluentHtml.Elements.IElement/*0200001B*/,bool>/*1B000030*/::.ctor(object,
                                                                                                                                                                                                                 native int) /* 0A000014 */
    IL_001f:  /* 80   | (04)000004       */ stsfld     class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class MvcContrib.FluentHtml.Elements.IElement/*0200001B*/,bool> MvcContrib.FluentHtml.Behaviors.AutoLabelBehavior/*02000005*/::'CS$<>9__CachedAnonymousMethodDelegate1' /* 04000004 */
    IL_0024:  /* 7E   | (04)000004       */ ldsfld     class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class MvcContrib.FluentHtml.Elements.IElement/*0200001B*/,bool> MvcContrib.FluentHtml.Behaviors.AutoLabelBehavior/*02000005*/::'CS$<>9__CachedAnonymousMethodDelegate1' /* 04000004 */
    IL_0029:  /* 7D   | (04)000002       */ stfld      class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class MvcContrib.FluentHtml.Elements.IElement/*0200001B*/,bool> MvcContrib.FluentHtml.Behaviors.AutoLabelBehavior/*02000005*/::isQualifiedFunc /* 04000002 */
    IL_002e:  /* 02   |                  */ ldarg.0
    IL_002f:  /* 04   |                  */ ldarg.2
    IL_0030:  /* 7D   | (04)000003       */ stfld      class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class MvcContrib.FluentHtml.Elements.IElement/*0200001B*/,bool> MvcContrib.FluentHtml.Behaviors.AutoLabelBehavior/*02000005*/::renderLabelAfterFunc /* 04000003 */
    IL_0035:  /* 2A   |                  */ ret
  } // end of method AutoLabelBehavior::.ctor

  .method /*06000008*/ family hidebysig virtual 
          instance void  DoExecute(class MvcContrib.FluentHtml.Elements.IElement/*0200001B*/ element) cil managed
  // SIG: 20 01 01 12 6C
  {
    // Method begins at RVA 0x2101
    // Code size       51 (0x33)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (04)000002       */ ldfld      class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class MvcContrib.FluentHtml.Elements.IElement/*0200001B*/,bool> MvcContrib.FluentHtml.Behaviors.AutoLabelBehavior/*02000005*/::isQualifiedFunc /* 04000002 */
    IL_0006:  /* 03   |                  */ ldarg.1
    IL_0007:  /* 6F   | (0A)000015       */ callvirt   instance !1 class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class MvcContrib.FluentHtml.Elements.IElement/*0200001B*/,bool>/*1B000030*/::Invoke(!0) /* 0A000015 */
    IL_000c:  /* 2D   | 01               */ brtrue.s   IL_000f

    IL_000e:  /* 2A   |                  */ ret

    IL_000f:  /* 02   |                  */ ldarg.0
    IL_0010:  /* 7B   | (04)000003       */ ldfld      class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class MvcContrib.FluentHtml.Elements.IElement/*0200001B*/,bool> MvcContrib.FluentHtml.Behaviors.AutoLabelBehavior/*02000005*/::renderLabelAfterFunc /* 04000003 */
    IL_0015:  /* 2C   | 0E               */ brfalse.s  IL_0025

    IL_0017:  /* 02   |                  */ ldarg.0
    IL_0018:  /* 7B   | (04)000003       */ ldfld      class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class MvcContrib.FluentHtml.Elements.IElement/*0200001B*/,bool> MvcContrib.FluentHtml.Behaviors.AutoLabelBehavior/*02000005*/::renderLabelAfterFunc /* 04000003 */
    IL_001d:  /* 03   |                  */ ldarg.1
    IL_001e:  /* 6F   | (0A)000015       */ callvirt   instance !1 class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class MvcContrib.FluentHtml.Elements.IElement/*0200001B*/,bool>/*1B000030*/::Invoke(!0) /* 0A000015 */
    IL_0023:  /* 2D   | 07               */ brtrue.s   IL_002c

    IL_0025:  /* 03   |                  */ ldarg.1
    IL_0026:  /* 6F   | (06)000051       */ callvirt   instance void MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::SetAutoLabel() /* 06000051 */
    IL_002b:  /* 2A   |                  */ ret

    IL_002c:  /* 03   |                  */ ldarg.1
    IL_002d:  /* 6F   | (06)000052       */ callvirt   instance void MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::SetAutoLabelAfter() /* 06000052 */
    IL_0032:  /* 2A   |                  */ ret
  } // end of method AutoLabelBehavior::DoExecute

  .method /*06000009*/ private hidebysig static 
          bool  '<.ctor>b__0'(class MvcContrib.FluentHtml.Elements.IElement/*0200001B*/ e) cil managed
  // SIG: 00 01 02 12 6C
  {
    .custom /*0C000011:0A000013*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000032*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20aa
    // Code size       31 (0x1f)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 75   | (02)00002D       */ isinst     MvcContrib.FluentHtml.Elements.Hidden/*0200002D*/
    IL_0006:  /* 2D   | 15               */ brtrue.s   IL_001d

    IL_0008:  /* 02   |                  */ ldarg.0
    IL_0009:  /* 75   | (02)000022       */ isinst     MvcContrib.FluentHtml.Elements.Button/*02000022*/
    IL_000e:  /* 2D   | 0D               */ brtrue.s   IL_001d

    IL_0010:  /* 02   |                  */ ldarg.0
    IL_0011:  /* 75   | (02)00004C       */ isinst     MvcContrib.FluentHtml.Elements.SubmitButton/*0200004C*/
    IL_0016:  /* 14   |                  */ ldnull
    IL_0017:  /* FE03 |                  */ cgt.un
    IL_0019:  /* 16   |                  */ ldc.i4.0
    IL_001a:  /* FE01 |                  */ ceq
    IL_001c:  /* 2A   |                  */ ret

    IL_001d:  /* 16   |                  */ ldc.i4.0
    IL_001e:  /* 2A   |                  */ ret
  } // end of method AutoLabelBehavior::'<.ctor>b__0'

} // end of class MvcContrib.FluentHtml.Behaviors.AutoLabelBehavior

.class /*02000006*/ public auto ansi beforefieldinit MvcContrib.FluentHtml.Behaviors.AutoLabelSettings
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
       implements MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/
{
  .field /*04000005*/ private bool '<UseFullNameForNestedProperties>k__BackingField'
  .custom /*0C00000E:0A000013*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000032*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
  .field /*04000006*/ private string '<LabelFormat>k__BackingField'
  .custom /*0C00000F:0A000013*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000032*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
  .field /*04000007*/ private string '<LabelCssClass>k__BackingField'
  .custom /*0C000010:0A000013*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000032*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
  .method /*0600000A*/ public hidebysig specialname 
          instance bool  get_UseFullNameForNestedProperties() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000012:0A000013*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000032*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2135
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (04)000005       */ ldfld      bool MvcContrib.FluentHtml.Behaviors.AutoLabelSettings/*02000006*/::'<UseFullNameForNestedProperties>k__BackingField' /* 04000005 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method AutoLabelSettings::get_UseFullNameForNestedProperties

  .method /*0600000B*/ private hidebysig specialname 
          instance void  set_UseFullNameForNestedProperties(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C000015:0A000013*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000032*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x213d
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (04)000005       */ stfld      bool MvcContrib.FluentHtml.Behaviors.AutoLabelSettings/*02000006*/::'<UseFullNameForNestedProperties>k__BackingField' /* 04000005 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method AutoLabelSettings::set_UseFullNameForNestedProperties

  .method /*0600000C*/ public hidebysig specialname 
          instance string  get_LabelFormat() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000017:0A000013*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000032*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2146
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (04)000006       */ ldfld      string MvcContrib.FluentHtml.Behaviors.AutoLabelSettings/*02000006*/::'<LabelFormat>k__BackingField' /* 04000006 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method AutoLabelSettings::get_LabelFormat

  .method /*0600000D*/ private hidebysig specialname 
          instance void  set_LabelFormat(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000018:0A000013*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000032*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x214e
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (04)000006       */ stfld      string MvcContrib.FluentHtml.Behaviors.AutoLabelSettings/*02000006*/::'<LabelFormat>k__BackingField' /* 04000006 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method AutoLabelSettings::set_LabelFormat

  .method /*0600000E*/ public hidebysig specialname 
          instance string  get_LabelCssClass() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000019:0A000013*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000032*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2157
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (04)000007       */ ldfld      string MvcContrib.FluentHtml.Behaviors.AutoLabelSettings/*02000006*/::'<LabelCssClass>k__BackingField' /* 04000007 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method AutoLabelSettings::get_LabelCssClass

  .method /*0600000F*/ private hidebysig specialname 
          instance void  set_LabelCssClass(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C00001A:0A000013*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000032*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x215f
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (04)000007       */ stfld      string MvcContrib.FluentHtml.Behaviors.AutoLabelSettings/*02000006*/::'<LabelCssClass>k__BackingField' /* 04000007 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method AutoLabelSettings::set_LabelCssClass

  .method /*06000010*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2168
    // Code size       10 (0xa)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 16   |                  */ ldc.i4.0
    IL_0002:  /* 14   |                  */ ldnull
    IL_0003:  /* 14   |                  */ ldnull
    IL_0004:  /* 28   | (06)000011       */ call       instance void MvcContrib.FluentHtml.Behaviors.AutoLabelSettings/*02000006*/::.ctor(bool,
                                                                                                                                          string,
                                                                                                                                          string) /* 06000011 */
    IL_0009:  /* 2A   |                  */ ret
  } // end of method AutoLabelSettings::.ctor

  .method /*06000011*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(bool useFullNameForNestedProperties,
                               string labelFormat,
                               string labelCssClass) cil managed
  // SIG: 20 03 01 02 0E 0E
  {
    // Method begins at RVA 0x2173
    // Code size       28 (0x1c)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000012       */ call       instance void [mscorlib/*23000001*/]System.Object/*01000001*/::.ctor() /* 0A000012 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 28   | (06)00000B       */ call       instance void MvcContrib.FluentHtml.Behaviors.AutoLabelSettings/*02000006*/::set_UseFullNameForNestedProperties(bool) /* 0600000B */
    IL_000d:  /* 02   |                  */ ldarg.0
    IL_000e:  /* 04   |                  */ ldarg.2
    IL_000f:  /* 28   | (06)00000D       */ call       instance void MvcContrib.FluentHtml.Behaviors.AutoLabelSettings/*02000006*/::set_LabelFormat(string) /* 0600000D */
    IL_0014:  /* 02   |                  */ ldarg.0
    IL_0015:  /* 05   |                  */ ldarg.3
    IL_0016:  /* 28   | (06)00000F       */ call       instance void MvcContrib.FluentHtml.Behaviors.AutoLabelSettings/*02000006*/::set_LabelCssClass(string) /* 0600000F */
    IL_001b:  /* 2A   |                  */ ret
  } // end of method AutoLabelSettings::.ctor

  .property /*17000001*/ instance bool UseFullNameForNestedProperties()
  {
    .get instance bool MvcContrib.FluentHtml.Behaviors.AutoLabelSettings/*02000006*/::get_UseFullNameForNestedProperties() /* 0600000A */
    .set instance void MvcContrib.FluentHtml.Behaviors.AutoLabelSettings/*02000006*/::set_UseFullNameForNestedProperties(bool) /* 0600000B */
  } // end of property AutoLabelSettings::UseFullNameForNestedProperties
  .property /*17000002*/ instance string LabelFormat()
  {
    .set instance void MvcContrib.FluentHtml.Behaviors.AutoLabelSettings/*02000006*/::set_LabelFormat(string) /* 0600000D */
    .get instance string MvcContrib.FluentHtml.Behaviors.AutoLabelSettings/*02000006*/::get_LabelFormat() /* 0600000C */
  } // end of property AutoLabelSettings::LabelFormat
  .property /*17000003*/ instance string LabelCssClass()
  {
    .get instance string MvcContrib.FluentHtml.Behaviors.AutoLabelSettings/*02000006*/::get_LabelCssClass() /* 0600000E */
    .set instance void MvcContrib.FluentHtml.Behaviors.AutoLabelSettings/*02000006*/::set_LabelCssClass(string) /* 0600000F */
  } // end of property AutoLabelSettings::LabelCssClass
} // end of class MvcContrib.FluentHtml.Behaviors.AutoLabelSettings

.class /*02000007*/ public auto ansi beforefieldinit MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .class /*02000009*/ auto ansi nested private beforefieldinit TypeToBehaviorMap
         extends class MvcContrib.FluentHtml.Behaviors.ThreadSafeDictionary`2/*02000008*/<class [mscorlib/*23000001*/]System.Type/*01000003*/,class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class [System.Core/*23000002*/]System.Action`2/*01000005*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/,object>>>/*1B000003*/
  {
    .method /*0600001C*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x2553
      // Code size       7 (0x7)
      .maxstack  8
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 28   | (0A)000045       */ call       instance void class MvcContrib.FluentHtml.Behaviors.ThreadSafeDictionary`2/*02000008*/<class [mscorlib/*23000001*/]System.Type/*01000003*/,class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class [System.Core/*23000002*/]System.Action`2/*01000005*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/,object>>>/*1B000003*/::.ctor() /* 0A000045 */
      IL_0006:  /* 2A   |                  */ ret
    } // end of method TypeToBehaviorMap::.ctor

  } // end of class TypeToBehaviorMap

  .class /*02000057*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass3'
         extends [mscorlib/*23000001*/]System.Object/*01000001*/
  {
    .custom /*0C00003B:0A000013*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000032*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    .field /*04000074*/ public class [mscorlib/*23000001*/]System.Type/*01000003*/ behaviorType
    .field /*04000075*/ public class [mscorlib/*23000001*/]System.Type/*01000003*/ targetType
    .method /*060001A7*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x21f8
      // Code size       7 (0x7)
      .maxstack  8
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 28   | (0A)000012       */ call       instance void [mscorlib/*23000001*/]System.Object/*01000001*/::.ctor() /* 0A000012 */
      IL_0006:  /* 2A   |                  */ ret
    } // end of method '<>c__DisplayClass3'::.ctor

    .method /*060001A8*/ public hidebysig 
            instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class [System.Core/*23000002*/]System.Action`2/*01000005*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/,object>> 
            '<GetExecuteActions>b__1'() cil managed
    // SIG: 20 00 15 12 11 01 15 12 15 02 12 08 1C
    {
      // Method begins at RVA 0x2207
      // Code size       18 (0x12)
      .maxstack  8
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 7B   | (04)000074       */ ldfld      class [mscorlib/*23000001*/]System.Type/*01000003*/ MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier/*02000007*//'<>c__DisplayClass3'/*02000057*/::behaviorType /* 04000074 */
      IL_0006:  /* 02   |                  */ ldarg.0
      IL_0007:  /* 7B   | (04)000075       */ ldfld      class [mscorlib/*23000001*/]System.Type/*01000003*/ MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier/*02000007*//'<>c__DisplayClass3'/*02000057*/::targetType /* 04000075 */
      IL_000c:  /* 28   | (06)000014       */ call       class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class [System.Core/*23000002*/]System.Action`2/*01000005*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/,object>> MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier/*02000007*/::CreateExecuteActions(class [mscorlib/*23000001*/]System.Type/*01000003*/,
                                                                                                                                                                                                                                                                                                                                                                          class [mscorlib/*23000001*/]System.Type/*01000003*/) /* 06000014 */
      IL_0011:  /* 2A   |                  */ ret
    } // end of method '<>c__DisplayClass3'::'<GetExecuteActions>b__1'

  } // end of class '<>c__DisplayClass3'

  .class /*02000058*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass8'
         extends [mscorlib/*23000001*/]System.Object/*01000001*/
  {
    .custom /*0C00003C:0A000013*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000032*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    .field /*04000076*/ public class MvcContrib.FluentHtml.Behaviors.GenericTypeSpec/*0200000F*/ behaviorTypeSpec
    .field /*04000077*/ public class MvcContrib.FluentHtml.Behaviors.GenericTypeSpec/*0200000F*/ orderedBehaviorTypeSpec
    .method /*060001A9*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x2283
      // Code size       7 (0x7)
      .maxstack  8
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 28   | (0A)000012       */ call       instance void [mscorlib/*23000001*/]System.Object/*01000001*/::.ctor() /* 0A000012 */
      IL_0006:  /* 2A   |                  */ ret
    } // end of method '<>c__DisplayClass8'::.ctor

    .method /*060001AA*/ public hidebysig 
            instance bool  '<CreateExecuteActions>b__7'(class [mscorlib/*23000001*/]System.Type/*01000003*/ x) cil managed
    // SIG: 20 01 02 12 0D
    {
      // Method begins at RVA 0x228b
      // Code size       39 (0x27)
      .maxstack  8
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 7B   | (04)000076       */ ldfld      class MvcContrib.FluentHtml.Behaviors.GenericTypeSpec/*0200000F*/ MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier/*02000007*//'<>c__DisplayClass8'/*02000058*/::behaviorTypeSpec /* 04000076 */
      IL_0006:  /* 6F   | (06)000031       */ callvirt   instance class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class [mscorlib/*23000001*/]System.Type/*01000003*/,bool> MvcContrib.FluentHtml.Behaviors.GenericTypeSpec/*0200000F*/::get_Matcher() /* 06000031 */
      IL_000b:  /* 03   |                  */ ldarg.1
      IL_000c:  /* 6F   | (0A)000020       */ callvirt   instance !1 class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class [mscorlib/*23000001*/]System.Type/*01000003*/,bool>/*1B000038*/::Invoke(!0) /* 0A000020 */
      IL_0011:  /* 2D   | 12               */ brtrue.s   IL_0025

      IL_0013:  /* 02   |                  */ ldarg.0
      IL_0014:  /* 7B   | (04)000077       */ ldfld      class MvcContrib.FluentHtml.Behaviors.GenericTypeSpec/*0200000F*/ MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier/*02000007*//'<>c__DisplayClass8'/*02000058*/::orderedBehaviorTypeSpec /* 04000077 */
      IL_0019:  /* 6F   | (06)000031       */ callvirt   instance class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class [mscorlib/*23000001*/]System.Type/*01000003*/,bool> MvcContrib.FluentHtml.Behaviors.GenericTypeSpec/*0200000F*/::get_Matcher() /* 06000031 */
      IL_001e:  /* 03   |                  */ ldarg.1
      IL_001f:  /* 6F   | (0A)000020       */ callvirt   instance !1 class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class [mscorlib/*23000001*/]System.Type/*01000003*/,bool>/*1B000038*/::Invoke(!0) /* 0A000020 */
      IL_0024:  /* 2A   |                  */ ret

      IL_0025:  /* 17   |                  */ ldc.i4.1
      IL_0026:  /* 2A   |                  */ ret
    } // end of method '<>c__DisplayClass8'::'<CreateExecuteActions>b__7'

  } // end of class '<>c__DisplayClass8'

  .class /*02000059*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClassc'
         extends [mscorlib/*23000001*/]System.Object/*01000001*/
  {
    .custom /*0C00003D:0A000013*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000032*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    .class /*0200005A*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClasse'
           extends [mscorlib/*23000001*/]System.Object/*01000001*/
    {
      .field /*04000079*/ public class MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier/*02000007*//'<>c__DisplayClassc'/*02000059*/ 'CS$<>8__localsd'
      .field /*0400007A*/ public class [mscorlib/*23000001*/]System.Type/*01000003*/ x
      .method /*060001AD*/ public hidebysig specialname rtspecialname 
              instance void  .ctor() cil managed
      // SIG: 20 00 01
      {
        // Method begins at RVA 0x233e
        // Code size       7 (0x7)
        .maxstack  8
        IL_0000:  /* 02   |                  */ ldarg.0
        IL_0001:  /* 28   | (0A)000012       */ call       instance void [mscorlib/*23000001*/]System.Object/*01000001*/::.ctor() /* 0A000012 */
        IL_0006:  /* 2A   |                  */ ret
      } // end of method '<>c__DisplayClasse'::.ctor

      .method /*060001AE*/ public hidebysig 
              instance class [System.Core/*23000002*/]System.Action`2/*01000005*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/,object> 
              '<BehaviorActionSelector>b__b'() cil managed
      // SIG: 20 00 15 12 15 02 12 08 1C
      {
        // Method begins at RVA 0x2346
        // Code size       12 (0xc)
        .maxstack  8
        IL_0000:  /* 02   |                  */ ldarg.0
        IL_0001:  /* 7B   | (04)00007A       */ ldfld      class [mscorlib/*23000001*/]System.Type/*01000003*/ MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier/*02000007*//'<>c__DisplayClassc'/*02000059*//'<>c__DisplayClasse'/*0200005A*/::x /* 0400007A */
        IL_0006:  /* 28   | (06)000016       */ call       class [System.Core/*23000002*/]System.Action`2/*01000005*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/,object> MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier/*02000007*/::CreateExecuteAction(class [mscorlib/*23000001*/]System.Type/*01000003*/) /* 06000016 */
        IL_000b:  /* 2A   |                  */ ret
      } // end of method '<>c__DisplayClasse'::'<BehaviorActionSelector>b__b'

    } // end of class '<>c__DisplayClasse'

    .field /*04000078*/ public class [mscorlib/*23000001*/]System.Type/*01000003*/ behaviorType
    .method /*060001AB*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x2336
      // Code size       7 (0x7)
      .maxstack  8
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 28   | (0A)000012       */ call       instance void [mscorlib/*23000001*/]System.Object/*01000001*/::.ctor() /* 0A000012 */
      IL_0006:  /* 2A   |                  */ ret
    } // end of method '<>c__DisplayClassc'::.ctor

    .method /*060001AC*/ public hidebysig 
            instance class [System.Core/*23000002*/]System.Action`2/*01000005*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/,object> 
            '<BehaviorActionSelector>b__a'(class [mscorlib/*23000001*/]System.Type/*01000003*/ x) cil managed
    // SIG: 20 01 15 12 15 02 12 08 1C 12 0D
    {
      // Method begins at RVA 0x2354
      // Code size       49 (0x31)
      .maxstack  5
      .locals /*11000005*/ init (class MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier/*02000007*//'<>c__DisplayClassc'/*02000059*//'<>c__DisplayClasse'/*0200005A*/ V_0)
      IL_0000:  /* 73   | (06)0001AD       */ newobj     instance void MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier/*02000007*//'<>c__DisplayClassc'/*02000059*//'<>c__DisplayClasse'/*0200005A*/::.ctor() /* 060001AD */
      IL_0005:  /* 0A   |                  */ stloc.0
      IL_0006:  /* 06   |                  */ ldloc.0
      IL_0007:  /* 02   |                  */ ldarg.0
      IL_0008:  /* 7D   | (04)000079       */ stfld      class MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier/*02000007*//'<>c__DisplayClassc'/*02000059*/ MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier/*02000007*//'<>c__DisplayClassc'/*02000059*//'<>c__DisplayClasse'/*0200005A*/::'CS$<>8__localsd' /* 04000079 */
      IL_000d:  /* 06   |                  */ ldloc.0
      IL_000e:  /* 03   |                  */ ldarg.1
      IL_000f:  /* 7D   | (04)00007A       */ stfld      class [mscorlib/*23000001*/]System.Type/*01000003*/ MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier/*02000007*//'<>c__DisplayClassc'/*02000059*//'<>c__DisplayClasse'/*0200005A*/::x /* 0400007A */
      IL_0014:  /* 7E   | (04)000009       */ ldsfld     class MvcContrib.FluentHtml.Behaviors.ThreadSafeDictionary`2/*02000008*/<class [mscorlib/*23000001*/]System.Type/*01000003*/,class [System.Core/*23000002*/]System.Action`2/*01000005*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/,object>> MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier/*02000007*/::cachedBehaviorExecuteActions /* 04000009 */
      IL_0019:  /* 02   |                  */ ldarg.0
      IL_001a:  /* 7B   | (04)000078       */ ldfld      class [mscorlib/*23000001*/]System.Type/*01000003*/ MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier/*02000007*//'<>c__DisplayClassc'/*02000059*/::behaviorType /* 04000078 */
      IL_001f:  /* 06   |                  */ ldloc.0
      IL_0020:  /* FE06 | (06)0001AE       */ ldftn      instance class [System.Core/*23000002*/]System.Action`2/*01000005*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/,object> MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier/*02000007*//'<>c__DisplayClassc'/*02000059*//'<>c__DisplayClasse'/*0200005A*/::'<BehaviorActionSelector>b__b'() /* 060001AE */
      IL_0026:  /* 73   | (0A)000027       */ newobj     instance void class [System.Core/*23000002*/]System.Func`1/*0100000D*/<class [System.Core/*23000002*/]System.Action`2/*01000005*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/,object>>/*1B000039*/::.ctor(object,
                                                                                                                                                                                                                                                                                         native int) /* 0A000027 */
      IL_002b:  /* 6F   | (0A)000028       */ callvirt   instance !1 class MvcContrib.FluentHtml.Behaviors.ThreadSafeDictionary`2/*02000008*/<class [mscorlib/*23000001*/]System.Type/*01000003*/,class [System.Core/*23000002*/]System.Action`2/*01000005*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/,object>>/*1B00003A*/::GetOrAdd(!0,
                                                                                                                                                                                                                                                                                                                                                              class [System.Core/*23000002*/]System.Func`1/*0100000D*/<!1>) /* 0A000028 */
      IL_0030:  /* 2A   |                  */ ret
    } // end of method '<>c__DisplayClassc'::'<BehaviorActionSelector>b__a'

  } // end of class '<>c__DisplayClassc'

  .field /*04000008*/ private static initonly class MvcContrib.FluentHtml.Behaviors.ThreadSafeDictionary`2/*02000008*/<class [mscorlib/*23000001*/]System.Type/*01000003*/,class MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier/*02000007*//TypeToBehaviorMap/*02000009*/> cachedTypeToBehaviorMap
  .field /*04000009*/ private static initonly class MvcContrib.FluentHtml.Behaviors.ThreadSafeDictionary`2/*02000008*/<class [mscorlib/*23000001*/]System.Type/*01000003*/,class [System.Core/*23000002*/]System.Action`2/*01000005*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/,object>> cachedBehaviorExecuteActions
  .field /*0400000A*/ private static class [System.Core/*23000002*/]System.Func`1/*0100000D*/<class MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier/*02000007*//TypeToBehaviorMap/*02000009*/> 'CS$<>9__CachedAnonymousMethodDelegate2'
  .custom /*0C000013:0A000013*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000032*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
  .method /*06000012*/ public hidebysig static 
          void  Apply<(MvcContrib.FluentHtml.Elements.IElement/*0200001B*/) T>(class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/ behavior,
                                                                               !!T target) cil managed
  // SIG: 10 01 02 01 12 08 1E 00
  {
    // Method begins at RVA 0x2190
    // Code size       75 (0x4b)
    .maxstack  3
    .locals /*11000002*/ init (class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class [System.Core/*23000002*/]System.Action`2/*01000005*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/,object>> V_0,
             class [System.Core/*23000002*/]System.Action`2/*01000005*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/,object> V_1,
             class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000033*/<class [System.Core/*23000002*/]System.Action`2/*01000005*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/,object>> V_2)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 6F   | (0A)000016       */ callvirt   instance class [mscorlib/*23000001*/]System.Type/*01000003*/ [mscorlib/*23000001*/]System.Object/*01000001*/::GetType() /* 0A000016 */
    IL_0006:  /* 0F   | 01               */ ldarga.s   target
    IL_0008:  /* FE16 | (1B)000031       */ constrained. !!T/*1B000031*/
    IL_000e:  /* 6F   | (0A)000016       */ callvirt   instance class [mscorlib/*23000001*/]System.Type/*01000003*/ [mscorlib/*23000001*/]System.Object/*01000001*/::GetType() /* 0A000016 */
    IL_0013:  /* 28   | (06)000013       */ call       class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class [System.Core/*23000002*/]System.Action`2/*01000005*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/,object>> MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier/*02000007*/::GetExecuteActions(class [mscorlib/*23000001*/]System.Type/*01000003*/,
                                                                                                                                                                                                                                                                                                                                                                     class [mscorlib/*23000001*/]System.Type/*01000003*/) /* 06000013 */
    IL_0018:  /* 0A   |                  */ stloc.0
    IL_0019:  /* 06   |                  */ ldloc.0
    IL_001a:  /* 6F   | (0A)000017       */ callvirt   instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000033*/<!0> class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class [System.Core/*23000002*/]System.Action`2/*01000005*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/,object>>/*1B000032*/::GetEnumerator() /* 0A000017 */
    IL_001f:  /* 0C   |                  */ stloc.2
    .try
    {
      IL_0020:  /* 2B   | 14               */ br.s       IL_0036

      IL_0022:  /* 08   |                  */ ldloc.2
      IL_0023:  /* 6F   | (0A)000018       */ callvirt   instance !0 class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000033*/<class [System.Core/*23000002*/]System.Action`2/*01000005*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/,object>>/*1B000033*/::get_Current() /* 0A000018 */
      IL_0028:  /* 0B   |                  */ stloc.1
      IL_0029:  /* 07   |                  */ ldloc.1
      IL_002a:  /* 02   |                  */ ldarg.0
      IL_002b:  /* 03   |                  */ ldarg.1
      IL_002c:  /* 8C   | (1B)000031       */ box        !!T/*1B000031*/
      IL_0031:  /* 6F   | (0A)000019       */ callvirt   instance void class [System.Core/*23000002*/]System.Action`2/*01000005*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/,object>/*1B000034*/::Invoke(!0,
                                                                                                                                                                                                                                !1) /* 0A000019 */
      IL_0036:  /* 08   |                  */ ldloc.2
      IL_0037:  /* 6F   | (0A)00001A       */ callvirt   instance bool [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000034*/::MoveNext() /* 0A00001A */
      IL_003c:  /* 2D   | E4               */ brtrue.s   IL_0022

      IL_003e:  /* DE   | 0A               */ leave.s    IL_004a

    }  // end .try
    finally
    {
      IL_0040:  /* 08   |                  */ ldloc.2
      IL_0041:  /* 2C   | 06               */ brfalse.s  IL_0049

      IL_0043:  /* 08   |                  */ ldloc.2
      IL_0044:  /* 6F   | (0A)00001B       */ callvirt   instance void [mscorlib/*23000001*/]System.IDisposable/*01000035*/::Dispose() /* 0A00001B */
      IL_0049:  /* DC   |                  */ endfinally
    }  // end handler
    // HEX: 02 00 20 00 20 40 00 0A 00 00 00 00
    IL_004a:  /* 2A   |                  */ ret
  } // end of method CacheingBehaviorApplier::Apply

  .method /*06000013*/ private hidebysig static 
          class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class [System.Core/*23000002*/]System.Action`2/*01000005*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/,object>> 
          GetExecuteActions(class [mscorlib/*23000001*/]System.Type/*01000003*/ behaviorType,
                            class [mscorlib/*23000001*/]System.Type/*01000003*/ targetType) cil managed
  // SIG: 00 02 15 12 11 01 15 12 15 02 12 08 1C 12 0D 12 0D
  {
    // Method begins at RVA 0x221c
    // Code size       91 (0x5b)
    .maxstack  5
    .locals /*11000003*/ init (class MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier/*02000007*//TypeToBehaviorMap/*02000009*/ V_0,
             class MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier/*02000007*//'<>c__DisplayClass3'/*02000057*/ V_1)
    IL_0000:  /* 73   | (06)0001A7       */ newobj     instance void MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier/*02000007*//'<>c__DisplayClass3'/*02000057*/::.ctor() /* 060001A7 */
    IL_0005:  /* 0B   |                  */ stloc.1
    IL_0006:  /* 07   |                  */ ldloc.1
    IL_0007:  /* 02   |                  */ ldarg.0
    IL_0008:  /* 7D   | (04)000074       */ stfld      class [mscorlib/*23000001*/]System.Type/*01000003*/ MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier/*02000007*//'<>c__DisplayClass3'/*02000057*/::behaviorType /* 04000074 */
    IL_000d:  /* 07   |                  */ ldloc.1
    IL_000e:  /* 03   |                  */ ldarg.1
    IL_000f:  /* 7D   | (04)000075       */ stfld      class [mscorlib/*23000001*/]System.Type/*01000003*/ MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier/*02000007*//'<>c__DisplayClass3'/*02000057*/::targetType /* 04000075 */
    IL_0014:  /* 7E   | (04)000008       */ ldsfld     class MvcContrib.FluentHtml.Behaviors.ThreadSafeDictionary`2/*02000008*/<class [mscorlib/*23000001*/]System.Type/*01000003*/,class MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier/*02000007*//TypeToBehaviorMap/*02000009*/> MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier/*02000007*/::cachedTypeToBehaviorMap /* 04000008 */
    IL_0019:  /* 07   |                  */ ldloc.1
    IL_001a:  /* 7B   | (04)000074       */ ldfld      class [mscorlib/*23000001*/]System.Type/*01000003*/ MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier/*02000007*//'<>c__DisplayClass3'/*02000057*/::behaviorType /* 04000074 */
    IL_001f:  /* 7E   | (04)00000A       */ ldsfld     class [System.Core/*23000002*/]System.Func`1/*0100000D*/<class MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier/*02000007*//TypeToBehaviorMap/*02000009*/> MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier/*02000007*/::'CS$<>9__CachedAnonymousMethodDelegate2' /* 0400000A */
    IL_0024:  /* 2D   | 11               */ brtrue.s   IL_0037

    IL_0026:  /* 14   |                  */ ldnull
    IL_0027:  /* FE06 | (06)000018       */ ldftn      class MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier/*02000007*//TypeToBehaviorMap/*02000009*/ MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier/*02000007*/::'<GetExecuteActions>b__0'() /* 06000018 */
    IL_002d:  /* 73   | (0A)00001C       */ newobj     instance void class [System.Core/*23000002*/]System.Func`1/*0100000D*/<class MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier/*02000007*//TypeToBehaviorMap/*02000009*/>/*1B000035*/::.ctor(object,
                                                                                                                                                                                                                                                          native int) /* 0A00001C */
    IL_0032:  /* 80   | (04)00000A       */ stsfld     class [System.Core/*23000002*/]System.Func`1/*0100000D*/<class MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier/*02000007*//TypeToBehaviorMap/*02000009*/> MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier/*02000007*/::'CS$<>9__CachedAnonymousMethodDelegate2' /* 0400000A */
    IL_0037:  /* 7E   | (04)00000A       */ ldsfld     class [System.Core/*23000002*/]System.Func`1/*0100000D*/<class MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier/*02000007*//TypeToBehaviorMap/*02000009*/> MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier/*02000007*/::'CS$<>9__CachedAnonymousMethodDelegate2' /* 0400000A */
    IL_003c:  /* 6F   | (0A)00001D       */ callvirt   instance !1 class MvcContrib.FluentHtml.Behaviors.ThreadSafeDictionary`2/*02000008*/<class [mscorlib/*23000001*/]System.Type/*01000003*/,class MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier/*02000007*//TypeToBehaviorMap/*02000009*/>/*1B000036*/::GetOrAdd(!0,
                                                                                                                                                                                                                                                                                                                               class [System.Core/*23000002*/]System.Func`1/*0100000D*/<!1>) /* 0A00001D */
    IL_0041:  /* 0A   |                  */ stloc.0
    IL_0042:  /* 06   |                  */ ldloc.0
    IL_0043:  /* 07   |                  */ ldloc.1
    IL_0044:  /* 7B   | (04)000075       */ ldfld      class [mscorlib/*23000001*/]System.Type/*01000003*/ MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier/*02000007*//'<>c__DisplayClass3'/*02000057*/::targetType /* 04000075 */
    IL_0049:  /* 07   |                  */ ldloc.1
    IL_004a:  /* FE06 | (06)0001A8       */ ldftn      instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class [System.Core/*23000002*/]System.Action`2/*01000005*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/,object>> MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier/*02000007*//'<>c__DisplayClass3'/*02000057*/::'<GetExecuteActions>b__1'() /* 060001A8 */
    IL_0050:  /* 73   | (0A)00001E       */ newobj     instance void class [System.Core/*23000002*/]System.Func`1/*0100000D*/<class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class [System.Core/*23000002*/]System.Action`2/*01000005*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/,object>>>/*1B000037*/::.ctor(object,
                                                                                                                                                                                                                                                                                                                                                                         native int) /* 0A00001E */
    IL_0055:  /* 6F   | (0A)00001F       */ callvirt   instance !1 class MvcContrib.FluentHtml.Behaviors.ThreadSafeDictionary`2/*02000008*/<class [mscorlib/*23000001*/]System.Type/*01000003*/,class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class [System.Core/*23000002*/]System.Action`2/*01000005*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/,object>>>/*1B000003*/::GetOrAdd(!0,
                                                                                                                                                                                                                                                                                                                                                                                                                                              class [System.Core/*23000002*/]System.Func`1/*0100000D*/<!1>) /* 0A00001F */
    IL_005a:  /* 2A   |                  */ ret
  } // end of method CacheingBehaviorApplier::GetExecuteActions

  .method /*06000014*/ private hidebysig static 
          class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class [System.Core/*23000002*/]System.Action`2/*01000005*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/,object>> 
          CreateExecuteActions(class [mscorlib/*23000001*/]System.Type/*01000003*/ behaviorType,
                               class [mscorlib/*23000001*/]System.Type/*01000003*/ targetType) cil managed
  // SIG: 00 02 15 12 11 01 15 12 15 02 12 08 1C 12 0D 12 0D
  {
    // Method begins at RVA 0x22b4
    // Code size       118 (0x76)
    .maxstack  4
    .locals /*11000004*/ init (class MvcContrib.FluentHtml.Behaviors.GenericTypeSpec/*0200000F*/ V_0,
             class MvcContrib.FluentHtml.Behaviors.GenericTypeSpec/*0200000F*/ V_1,
             class MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier/*02000007*//'<>c__DisplayClass8'/*02000058*/ V_2)
    IL_0000:  /* 73   | (06)0001A9       */ newobj     instance void MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier/*02000007*//'<>c__DisplayClass8'/*02000058*/::.ctor() /* 060001A9 */
    IL_0005:  /* 0C   |                  */ stloc.2
    IL_0006:  /* 08   |                  */ ldloc.2
    IL_0007:  /* 73   | (06)000032       */ newobj     instance void MvcContrib.FluentHtml.Behaviors.GenericTypeSpec/*0200000F*/::.ctor() /* 06000032 */
    IL_000c:  /* 0A   |                  */ stloc.0
    IL_000d:  /* 06   |                  */ ldloc.0
    IL_000e:  /* D0   | (02)000003       */ ldtoken    MvcContrib.FluentHtml.Behaviors.IBehavior`1/*02000003*/
    IL_0013:  /* 28   | (0A)000021       */ call       class [mscorlib/*23000001*/]System.Type/*01000003*/ [mscorlib/*23000001*/]System.Type/*01000003*/::GetTypeFromHandle(valuetype [mscorlib/*23000001*/]System.RuntimeTypeHandle/*01000036*/) /* 0A000021 */
    IL_0018:  /* 7D   | (04)00000E       */ stfld      class [mscorlib/*23000001*/]System.Type/*01000003*/ MvcContrib.FluentHtml.Behaviors.GenericTypeSpec/*0200000F*/::OpenGenericType /* 0400000E */
    IL_001d:  /* 06   |                  */ ldloc.0
    IL_001e:  /* 03   |                  */ ldarg.1
    IL_001f:  /* 7D   | (04)00000F       */ stfld      class [mscorlib/*23000001*/]System.Type/*01000003*/ MvcContrib.FluentHtml.Behaviors.GenericTypeSpec/*0200000F*/::GenericParameterType /* 0400000F */
    IL_0024:  /* 06   |                  */ ldloc.0
    IL_0025:  /* 7D   | (04)000076       */ stfld      class MvcContrib.FluentHtml.Behaviors.GenericTypeSpec/*0200000F*/ MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier/*02000007*//'<>c__DisplayClass8'/*02000058*/::behaviorTypeSpec /* 04000076 */
    IL_002a:  /* 08   |                  */ ldloc.2
    IL_002b:  /* 73   | (06)000032       */ newobj     instance void MvcContrib.FluentHtml.Behaviors.GenericTypeSpec/*0200000F*/::.ctor() /* 06000032 */
    IL_0030:  /* 0B   |                  */ stloc.1
    IL_0031:  /* 07   |                  */ ldloc.1
    IL_0032:  /* D0   | (02)000011       */ ldtoken    MvcContrib.FluentHtml.Behaviors.IOrderedBehavior`1/*02000011*/
    IL_0037:  /* 28   | (0A)000021       */ call       class [mscorlib/*23000001*/]System.Type/*01000003*/ [mscorlib/*23000001*/]System.Type/*01000003*/::GetTypeFromHandle(valuetype [mscorlib/*23000001*/]System.RuntimeTypeHandle/*01000036*/) /* 0A000021 */
    IL_003c:  /* 7D   | (04)00000E       */ stfld      class [mscorlib/*23000001*/]System.Type/*01000003*/ MvcContrib.FluentHtml.Behaviors.GenericTypeSpec/*0200000F*/::OpenGenericType /* 0400000E */
    IL_0041:  /* 07   |                  */ ldloc.1
    IL_0042:  /* 03   |                  */ ldarg.1
    IL_0043:  /* 7D   | (04)00000F       */ stfld      class [mscorlib/*23000001*/]System.Type/*01000003*/ MvcContrib.FluentHtml.Behaviors.GenericTypeSpec/*0200000F*/::GenericParameterType /* 0400000F */
    IL_0048:  /* 07   |                  */ ldloc.1
    IL_0049:  /* 7D   | (04)000077       */ stfld      class MvcContrib.FluentHtml.Behaviors.GenericTypeSpec/*0200000F*/ MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier/*02000007*//'<>c__DisplayClass8'/*02000058*/::orderedBehaviorTypeSpec /* 04000077 */
    IL_004e:  /* 02   |                  */ ldarg.0
    IL_004f:  /* 6F   | (0A)000022       */ callvirt   instance class [mscorlib/*23000001*/]System.Type/*01000003*/[] [mscorlib/*23000001*/]System.Type/*01000003*/::GetInterfaces() /* 0A000022 */
    IL_0054:  /* 08   |                  */ ldloc.2
    IL_0055:  /* FE06 | (06)0001AA       */ ldftn      instance bool MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier/*02000007*//'<>c__DisplayClass8'/*02000058*/::'<CreateExecuteActions>b__7'(class [mscorlib/*23000001*/]System.Type/*01000003*/) /* 060001AA */
    IL_005b:  /* 73   | (0A)000023       */ newobj     instance void class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class [mscorlib/*23000001*/]System.Type/*01000003*/,bool>/*1B000038*/::.ctor(object,
                                                                                                                                                                                                           native int) /* 0A000023 */
    IL_0060:  /* 28   | (2B)000001       */ call       class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<!!0> [System.Core/*23000002*/]System.Linq.Enumerable/*01000037*/::Where<class [mscorlib/*23000001*/]System.Type/*01000003*/>(class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<!!0>,
                                                                                                                                                                                                                                                                     class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,bool>) /* 2B000001 */
    IL_0065:  /* 02   |                  */ ldarg.0
    IL_0066:  /* 28   | (06)000015       */ call       class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class [mscorlib/*23000001*/]System.Type/*01000003*/,class [System.Core/*23000002*/]System.Action`2/*01000005*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/,object>> MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier/*02000007*/::BehaviorActionSelector(class [mscorlib/*23000001*/]System.Type/*01000003*/) /* 06000015 */
    IL_006b:  /* 28   | (2B)000002       */ call       class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<!!1> [System.Core/*23000002*/]System.Linq.Enumerable/*01000037*/::Select<class [mscorlib/*23000001*/]System.Type/*01000003*/,class [System.Core/*23000002*/]System.Action`2/*01000005*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/,object>>(class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<!!0>,
                                                                                                                                                                                                                                                                                                                                                                                                           class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,!!1>) /* 2B000002 */
    IL_0070:  /* 28   | (2B)000003       */ call       !!0[] [System.Core/*23000002*/]System.Linq.Enumerable/*01000037*/::ToArray<class [System.Core/*23000002*/]System.Action`2/*01000005*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/,object>>(class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<!!0>) /* 2B000003 */
    IL_0075:  /* 2A   |                  */ ret
  } // end of method CacheingBehaviorApplier::CreateExecuteActions

  .method /*06000015*/ private hidebysig static 
          class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class [mscorlib/*23000001*/]System.Type/*01000003*/,class [System.Core/*23000002*/]System.Action`2/*01000005*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/,object>> 
          BehaviorActionSelector(class [mscorlib/*23000001*/]System.Type/*01000003*/ behaviorType) cil managed
  // SIG: 00 01 15 12 2D 02 12 0D 15 12 15 02 12 08 1C 12 0D
  {
    // Method begins at RVA 0x2394
    // Code size       26 (0x1a)
    .maxstack  3
    .locals /*11000006*/ init (class MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier/*02000007*//'<>c__DisplayClassc'/*02000059*/ V_0)
    IL_0000:  /* 73   | (06)0001AB       */ newobj     instance void MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier/*02000007*//'<>c__DisplayClassc'/*02000059*/::.ctor() /* 060001AB */
    IL_0005:  /* 0A   |                  */ stloc.0
    IL_0006:  /* 06   |                  */ ldloc.0
    IL_0007:  /* 02   |                  */ ldarg.0
    IL_0008:  /* 7D   | (04)000078       */ stfld      class [mscorlib/*23000001*/]System.Type/*01000003*/ MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier/*02000007*//'<>c__DisplayClassc'/*02000059*/::behaviorType /* 04000078 */
    IL_000d:  /* 06   |                  */ ldloc.0
    IL_000e:  /* FE06 | (06)0001AC       */ ldftn      instance class [System.Core/*23000002*/]System.Action`2/*01000005*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/,object> MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier/*02000007*//'<>c__DisplayClassc'/*02000059*/::'<BehaviorActionSelector>b__a'(class [mscorlib/*23000001*/]System.Type/*01000003*/) /* 060001AC */
    IL_0014:  /* 73   | (0A)000029       */ newobj     instance void class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class [mscorlib/*23000001*/]System.Type/*01000003*/,class [System.Core/*23000002*/]System.Action`2/*01000005*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/,object>>/*1B00003B*/::.ctor(object,
                                                                                                                                                                                                                                                                                                                                           native int) /* 0A000029 */
    IL_0019:  /* 2A   |                  */ ret
  } // end of method CacheingBehaviorApplier::BehaviorActionSelector

  .method /*06000016*/ private hidebysig static 
          class [System.Core/*23000002*/]System.Action`2/*01000005*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/,object> 
          CreateExecuteAction(class [mscorlib/*23000001*/]System.Type/*01000003*/ behaviorType) cil managed
  // SIG: 00 01 15 12 15 02 12 08 1C 12 0D
  {
    // Method begins at RVA 0x23bc
    // Code size       200 (0xc8)
    .maxstack  5
    .locals /*11000007*/ init (class [mscorlib/*23000001*/]System.Reflection.Emit.DynamicMethod/*01000039*/ V_0,
             class [mscorlib/*23000001*/]System.Reflection.Emit.ILGenerator/*0100003A*/ V_1,
             class [mscorlib/*23000001*/]System.Type/*01000003*/ V_2,
             class [mscorlib/*23000001*/]System.Reflection.MethodInfo/*01000010*/ V_3,
             class [mscorlib/*23000001*/]System.Type/*01000003*/[] V_4,
             class [mscorlib/*23000001*/]System.Type/*01000003*/[] V_5)
    IL_0000:  /* 72   | (70)000001       */ ldstr      "caller" /* 70000001 */
    IL_0005:  /* 14   |                  */ ldnull
    IL_0006:  /* 18   |                  */ ldc.i4.2
    IL_0007:  /* 8D   | (01)000003       */ newarr     [mscorlib/*23000001*/]System.Type/*01000003*/
    IL_000c:  /* 13   | 04               */ stloc.s    V_4
    IL_000e:  /* 11   | 04               */ ldloc.s    V_4
    IL_0010:  /* 16   |                  */ ldc.i4.0
    IL_0011:  /* D0   | (02)000002       */ ldtoken    MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/
    IL_0016:  /* 28   | (0A)000021       */ call       class [mscorlib/*23000001*/]System.Type/*01000003*/ [mscorlib/*23000001*/]System.Type/*01000003*/::GetTypeFromHandle(valuetype [mscorlib/*23000001*/]System.RuntimeTypeHandle/*01000036*/) /* 0A000021 */
    IL_001b:  /* A2   |                  */ stelem.ref
    IL_001c:  /* 11   | 04               */ ldloc.s    V_4
    IL_001e:  /* 17   |                  */ ldc.i4.1
    IL_001f:  /* D0   | (01)000001       */ ldtoken    [mscorlib/*23000001*/]System.Object/*01000001*/
    IL_0024:  /* 28   | (0A)000021       */ call       class [mscorlib/*23000001*/]System.Type/*01000003*/ [mscorlib/*23000001*/]System.Type/*01000003*/::GetTypeFromHandle(valuetype [mscorlib/*23000001*/]System.RuntimeTypeHandle/*01000036*/) /* 0A000021 */
    IL_0029:  /* A2   |                  */ stelem.ref
    IL_002a:  /* 11   | 04               */ ldloc.s    V_4
    IL_002c:  /* D0   | (02)000007       */ ldtoken    MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier/*02000007*/
    IL_0031:  /* 28   | (0A)000021       */ call       class [mscorlib/*23000001*/]System.Type/*01000003*/ [mscorlib/*23000001*/]System.Type/*01000003*/::GetTypeFromHandle(valuetype [mscorlib/*23000001*/]System.RuntimeTypeHandle/*01000036*/) /* 0A000021 */
    IL_0036:  /* 6F   | (0A)00002A       */ callvirt   instance class [mscorlib/*23000001*/]System.Reflection.Module/*01000038*/ [mscorlib/*23000001*/]System.Type/*01000003*/::get_Module() /* 0A00002A */
    IL_003b:  /* 73   | (0A)00002B       */ newobj     instance void [mscorlib/*23000001*/]System.Reflection.Emit.DynamicMethod/*01000039*/::.ctor(string,
                                                                                                                                                   class [mscorlib/*23000001*/]System.Type/*01000003*/,
                                                                                                                                                   class [mscorlib/*23000001*/]System.Type/*01000003*/[],
                                                                                                                                                   class [mscorlib/*23000001*/]System.Reflection.Module/*01000038*/) /* 0A00002B */
    IL_0040:  /* 0A   |                  */ stloc.0
    IL_0041:  /* 06   |                  */ ldloc.0
    IL_0042:  /* 6F   | (0A)00002C       */ callvirt   instance class [mscorlib/*23000001*/]System.Reflection.Emit.ILGenerator/*0100003A*/ [mscorlib/*23000001*/]System.Reflection.Emit.DynamicMethod/*01000039*/::GetILGenerator() /* 0A00002C */
    IL_0047:  /* 0B   |                  */ stloc.1
    IL_0048:  /* 07   |                  */ ldloc.1
    IL_0049:  /* 7E   | (0A)00002D       */ ldsfld     valuetype [mscorlib/*23000001*/]System.Reflection.Emit.OpCode/*0100003C*/ [mscorlib/*23000001*/]System.Reflection.Emit.OpCodes/*0100003B*/::Ldarg_0 /* 0A00002D */
    IL_004e:  /* 6F   | (0A)00002E       */ callvirt   instance void [mscorlib/*23000001*/]System.Reflection.Emit.ILGenerator/*0100003A*/::Emit(valuetype [mscorlib/*23000001*/]System.Reflection.Emit.OpCode/*0100003C*/) /* 0A00002E */
    IL_0053:  /* 07   |                  */ ldloc.1
    IL_0054:  /* 7E   | (0A)00002F       */ ldsfld     valuetype [mscorlib/*23000001*/]System.Reflection.Emit.OpCode/*0100003C*/ [mscorlib/*23000001*/]System.Reflection.Emit.OpCodes/*0100003B*/::Castclass /* 0A00002F */
    IL_0059:  /* 02   |                  */ ldarg.0
    IL_005a:  /* 6F   | (0A)000030       */ callvirt   instance void [mscorlib/*23000001*/]System.Reflection.Emit.ILGenerator/*0100003A*/::Emit(valuetype [mscorlib/*23000001*/]System.Reflection.Emit.OpCode/*0100003C*/,
                                                                                                                                                class [mscorlib/*23000001*/]System.Type/*01000003*/) /* 0A000030 */
    IL_005f:  /* 02   |                  */ ldarg.0
    IL_0060:  /* 6F   | (0A)000031       */ callvirt   instance class [mscorlib/*23000001*/]System.Type/*01000003*/[] [mscorlib/*23000001*/]System.Type/*01000003*/::GetGenericArguments() /* 0A000031 */
    IL_0065:  /* 16   |                  */ ldc.i4.0
    IL_0066:  /* 9A   |                  */ ldelem.ref
    IL_0067:  /* 0C   |                  */ stloc.2
    IL_0068:  /* 07   |                  */ ldloc.1
    IL_0069:  /* 7E   | (0A)000032       */ ldsfld     valuetype [mscorlib/*23000001*/]System.Reflection.Emit.OpCode/*0100003C*/ [mscorlib/*23000001*/]System.Reflection.Emit.OpCodes/*0100003B*/::Ldarg_1 /* 0A000032 */
    IL_006e:  /* 6F   | (0A)00002E       */ callvirt   instance void [mscorlib/*23000001*/]System.Reflection.Emit.ILGenerator/*0100003A*/::Emit(valuetype [mscorlib/*23000001*/]System.Reflection.Emit.OpCode/*0100003C*/) /* 0A00002E */
    IL_0073:  /* 07   |                  */ ldloc.1
    IL_0074:  /* 7E   | (0A)00002F       */ ldsfld     valuetype [mscorlib/*23000001*/]System.Reflection.Emit.OpCode/*0100003C*/ [mscorlib/*23000001*/]System.Reflection.Emit.OpCodes/*0100003B*/::Castclass /* 0A00002F */
    IL_0079:  /* 08   |                  */ ldloc.2
    IL_007a:  /* 6F   | (0A)000030       */ callvirt   instance void [mscorlib/*23000001*/]System.Reflection.Emit.ILGenerator/*0100003A*/::Emit(valuetype [mscorlib/*23000001*/]System.Reflection.Emit.OpCode/*0100003C*/,
                                                                                                                                                class [mscorlib/*23000001*/]System.Type/*01000003*/) /* 0A000030 */
    IL_007f:  /* 02   |                  */ ldarg.0
    IL_0080:  /* 72   | (70)00000F       */ ldstr      "Execute" /* 7000000F */
    IL_0085:  /* 17   |                  */ ldc.i4.1
    IL_0086:  /* 8D   | (01)000003       */ newarr     [mscorlib/*23000001*/]System.Type/*01000003*/
    IL_008b:  /* 13   | 05               */ stloc.s    V_5
    IL_008d:  /* 11   | 05               */ ldloc.s    V_5
    IL_008f:  /* 16   |                  */ ldc.i4.0
    IL_0090:  /* 08   |                  */ ldloc.2
    IL_0091:  /* A2   |                  */ stelem.ref
    IL_0092:  /* 11   | 05               */ ldloc.s    V_5
    IL_0094:  /* 6F   | (0A)000033       */ callvirt   instance class [mscorlib/*23000001*/]System.Reflection.MethodInfo/*01000010*/ [mscorlib/*23000001*/]System.Type/*01000003*/::GetMethod(string,
                                                                                                                                                                                              class [mscorlib/*23000001*/]System.Type/*01000003*/[]) /* 0A000033 */
    IL_0099:  /* 0D   |                  */ stloc.3
    IL_009a:  /* 07   |                  */ ldloc.1
    IL_009b:  /* 7E   | (0A)000034       */ ldsfld     valuetype [mscorlib/*23000001*/]System.Reflection.Emit.OpCode/*0100003C*/ [mscorlib/*23000001*/]System.Reflection.Emit.OpCodes/*0100003B*/::Callvirt /* 0A000034 */
    IL_00a0:  /* 09   |                  */ ldloc.3
    IL_00a1:  /* 14   |                  */ ldnull
    IL_00a2:  /* 6F   | (0A)000035       */ callvirt   instance void [mscorlib/*23000001*/]System.Reflection.Emit.ILGenerator/*0100003A*/::EmitCall(valuetype [mscorlib/*23000001*/]System.Reflection.Emit.OpCode/*0100003C*/,
                                                                                                                                                    class [mscorlib/*23000001*/]System.Reflection.MethodInfo/*01000010*/,
                                                                                                                                                    class [mscorlib/*23000001*/]System.Type/*01000003*/[]) /* 0A000035 */
    IL_00a7:  /* 07   |                  */ ldloc.1
    IL_00a8:  /* 7E   | (0A)000036       */ ldsfld     valuetype [mscorlib/*23000001*/]System.Reflection.Emit.OpCode/*0100003C*/ [mscorlib/*23000001*/]System.Reflection.Emit.OpCodes/*0100003B*/::Ret /* 0A000036 */
    IL_00ad:  /* 6F   | (0A)00002E       */ callvirt   instance void [mscorlib/*23000001*/]System.Reflection.Emit.ILGenerator/*0100003A*/::Emit(valuetype [mscorlib/*23000001*/]System.Reflection.Emit.OpCode/*0100003C*/) /* 0A00002E */
    IL_00b2:  /* 06   |                  */ ldloc.0
    IL_00b3:  /* D0   | (1B)000034       */ ldtoken    class [System.Core/*23000002*/]System.Action`2/*01000005*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/,object>/*1B000034*/
    IL_00b8:  /* 28   | (0A)000021       */ call       class [mscorlib/*23000001*/]System.Type/*01000003*/ [mscorlib/*23000001*/]System.Type/*01000003*/::GetTypeFromHandle(valuetype [mscorlib/*23000001*/]System.RuntimeTypeHandle/*01000036*/) /* 0A000021 */
    IL_00bd:  /* 6F   | (0A)000037       */ callvirt   instance class [mscorlib/*23000001*/]System.Delegate/*0100003D*/ [mscorlib/*23000001*/]System.Reflection.Emit.DynamicMethod/*01000039*/::CreateDelegate(class [mscorlib/*23000001*/]System.Type/*01000003*/) /* 0A000037 */
    IL_00c2:  /* 74   | (1B)000034       */ castclass  class [System.Core/*23000002*/]System.Action`2/*01000005*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/,object>/*1B000034*/
    IL_00c7:  /* 2A   |                  */ ret
  } // end of method CacheingBehaviorApplier::CreateExecuteAction

  .method /*06000017*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x24a6
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000012       */ call       instance void [mscorlib/*23000001*/]System.Object/*01000001*/::.ctor() /* 0A000012 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method CacheingBehaviorApplier::.ctor

  .method /*06000018*/ private hidebysig static 
          class MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier/*02000007*//TypeToBehaviorMap/*02000009*/ 
          '<GetExecuteActions>b__0'() cil managed
  // SIG: 00 00 12 24
  {
    .custom /*0C00001D:0A000013*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000032*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2200
    // Code size       6 (0x6)
    .maxstack  8
    IL_0000:  /* 73   | (06)00001C       */ newobj     instance void MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier/*02000007*//TypeToBehaviorMap/*02000009*/::.ctor() /* 0600001C */
    IL_0005:  /* 2A   |                  */ ret
  } // end of method CacheingBehaviorApplier::'<GetExecuteActions>b__0'

  .method /*06000019*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x2490
    // Code size       21 (0x15)
    .maxstack  8
    IL_0000:  /* 73   | (0A)000038       */ newobj     instance void class MvcContrib.FluentHtml.Behaviors.ThreadSafeDictionary`2/*02000008*/<class [mscorlib/*23000001*/]System.Type/*01000003*/,class MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier/*02000007*//TypeToBehaviorMap/*02000009*/>/*1B000036*/::.ctor() /* 0A000038 */
    IL_0005:  /* 80   | (04)000008       */ stsfld     class MvcContrib.FluentHtml.Behaviors.ThreadSafeDictionary`2/*02000008*/<class [mscorlib/*23000001*/]System.Type/*01000003*/,class MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier/*02000007*//TypeToBehaviorMap/*02000009*/> MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier/*02000007*/::cachedTypeToBehaviorMap /* 04000008 */
    IL_000a:  /* 73   | (0A)000039       */ newobj     instance void class MvcContrib.FluentHtml.Behaviors.ThreadSafeDictionary`2/*02000008*/<class [mscorlib/*23000001*/]System.Type/*01000003*/,class [System.Core/*23000002*/]System.Action`2/*01000005*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/,object>>/*1B00003A*/::.ctor() /* 0A000039 */
    IL_000f:  /* 80   | (04)000009       */ stsfld     class MvcContrib.FluentHtml.Behaviors.ThreadSafeDictionary`2/*02000008*/<class [mscorlib/*23000001*/]System.Type/*01000003*/,class [System.Core/*23000002*/]System.Action`2/*01000005*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/,object>> MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier/*02000007*/::cachedBehaviorExecuteActions /* 04000009 */
    IL_0014:  /* 2A   |                  */ ret
  } // end of method CacheingBehaviorApplier::.cctor

} // end of class MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier

.class /*02000008*/ public auto ansi beforefieldinit MvcContrib.FluentHtml.Behaviors.ThreadSafeDictionary`2<TKey,TValue>
       extends class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000002*/<!TKey,!TValue>/*1B000002*/
{
  .field /*0400000B*/ private class [System.Core/*23000002*/]System.Threading.ReaderWriterLockSlim/*0100000C*/ cacheLock
  .method /*0600001A*/ public hidebysig instance !TValue 
          GetOrAdd(!TKey key,
                   class [System.Core/*23000002*/]System.Func`1/*0100000D*/<!TValue> defaultValueDelegate) cil managed
  // SIG: 20 02 13 01 13 00 15 12 35 01 13 01
  {
    // Method begins at RVA 0x24b0
    // Code size       101 (0x65)
    .maxstack  3
    .locals /*11000008*/ init (!TValue V_0)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (0A)00003A       */ ldfld      class [System.Core/*23000002*/]System.Threading.ReaderWriterLockSlim/*0100000C*/ class MvcContrib.FluentHtml.Behaviors.ThreadSafeDictionary`2/*02000008*/<!TKey,!TValue>/*1B00003C*/::cacheLock /* 0A00003A */
    IL_0006:  /* 6F   | (0A)00003B       */ callvirt   instance void [System.Core/*23000002*/]System.Threading.ReaderWriterLockSlim/*0100000C*/::EnterReadLock() /* 0A00003B */
    .try
    {
      IL_000b:  /* 02   |                  */ ldarg.0
      IL_000c:  /* 03   |                  */ ldarg.1
      IL_000d:  /* 28   | (0A)00003C       */ call       instance bool class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000002*/<!TKey,!TValue>/*1B000002*/::ContainsKey(!0) /* 0A00003C */
      IL_0012:  /* 2C   | 0A               */ brfalse.s  IL_001e

      IL_0014:  /* 02   |                  */ ldarg.0
      IL_0015:  /* 03   |                  */ ldarg.1
      IL_0016:  /* 28   | (0A)00003D       */ call       instance !1 class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000002*/<!TKey,!TValue>/*1B000002*/::get_Item(!0) /* 0A00003D */
      IL_001b:  /* 0A   |                  */ stloc.0
      IL_001c:  /* DE   | 45               */ leave.s    IL_0063

      IL_001e:  /* DE   | 0C               */ leave.s    IL_002c

    }  // end .try
    finally
    {
      IL_0020:  /* 02   |                  */ ldarg.0
      IL_0021:  /* 7B   | (0A)00003A       */ ldfld      class [System.Core/*23000002*/]System.Threading.ReaderWriterLockSlim/*0100000C*/ class MvcContrib.FluentHtml.Behaviors.ThreadSafeDictionary`2/*02000008*/<!TKey,!TValue>/*1B00003C*/::cacheLock /* 0A00003A */
      IL_0026:  /* 6F   | (0A)00003E       */ callvirt   instance void [System.Core/*23000002*/]System.Threading.ReaderWriterLockSlim/*0100000C*/::ExitReadLock() /* 0A00003E */
      IL_002b:  /* DC   |                  */ endfinally
    }  // end handler
    // HEX: 02 00 0B 00 15 20 00 0C 00 00 00 00
    IL_002c:  /* 02   |                  */ ldarg.0
    IL_002d:  /* 7B   | (0A)00003A       */ ldfld      class [System.Core/*23000002*/]System.Threading.ReaderWriterLockSlim/*0100000C*/ class MvcContrib.FluentHtml.Behaviors.ThreadSafeDictionary`2/*02000008*/<!TKey,!TValue>/*1B00003C*/::cacheLock /* 0A00003A */
    IL_0032:  /* 6F   | (0A)00003F       */ callvirt   instance void [System.Core/*23000002*/]System.Threading.ReaderWriterLockSlim/*0100000C*/::EnterWriteLock() /* 0A00003F */
    .try
    {
      IL_0037:  /* 02   |                  */ ldarg.0
      IL_0038:  /* 03   |                  */ ldarg.1
      IL_0039:  /* 28   | (0A)00003C       */ call       instance bool class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000002*/<!TKey,!TValue>/*1B000002*/::ContainsKey(!0) /* 0A00003C */
      IL_003e:  /* 2D   | 0D               */ brtrue.s   IL_004d

      IL_0040:  /* 02   |                  */ ldarg.0
      IL_0041:  /* 03   |                  */ ldarg.1
      IL_0042:  /* 04   |                  */ ldarg.2
      IL_0043:  /* 6F   | (0A)000040       */ callvirt   instance !0 class [System.Core/*23000002*/]System.Func`1/*0100000D*/<!TValue>/*1B00003D*/::Invoke() /* 0A000040 */
      IL_0048:  /* 28   | (0A)000041       */ call       instance void class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000002*/<!TKey,!TValue>/*1B000002*/::Add(!0,
                                                                                                                                                                                       !1) /* 0A000041 */
      IL_004d:  /* 02   |                  */ ldarg.0
      IL_004e:  /* 03   |                  */ ldarg.1
      IL_004f:  /* 28   | (0A)00003D       */ call       instance !1 class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000002*/<!TKey,!TValue>/*1B000002*/::get_Item(!0) /* 0A00003D */
      IL_0054:  /* 0A   |                  */ stloc.0
      IL_0055:  /* DE   | 0C               */ leave.s    IL_0063

    }  // end .try
    finally
    {
      IL_0057:  /* 02   |                  */ ldarg.0
      IL_0058:  /* 7B   | (0A)00003A       */ ldfld      class [System.Core/*23000002*/]System.Threading.ReaderWriterLockSlim/*0100000C*/ class MvcContrib.FluentHtml.Behaviors.ThreadSafeDictionary`2/*02000008*/<!TKey,!TValue>/*1B00003C*/::cacheLock /* 0A00003A */
      IL_005d:  /* 6F   | (0A)000042       */ callvirt   instance void [System.Core/*23000002*/]System.Threading.ReaderWriterLockSlim/*0100000C*/::ExitWriteLock() /* 0A000042 */
      IL_0062:  /* DC   |                  */ endfinally
    }  // end handler
    // HEX: 02 00 37 00 20 57 00 0C 00 00 00 00
    IL_0063:  /* 06   |                  */ ldloc.0
    IL_0064:  /* 2A   |                  */ ret
  } // end of method ThreadSafeDictionary`2::GetOrAdd

  .method /*0600001B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2540
    // Code size       18 (0x12)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 73   | (0A)000043       */ newobj     instance void [System.Core/*23000002*/]System.Threading.ReaderWriterLockSlim/*0100000C*/::.ctor() /* 0A000043 */
    IL_0006:  /* 7D   | (0A)00003A       */ stfld      class [System.Core/*23000002*/]System.Threading.ReaderWriterLockSlim/*0100000C*/ class MvcContrib.FluentHtml.Behaviors.ThreadSafeDictionary`2/*02000008*/<!TKey,!TValue>/*1B00003C*/::cacheLock /* 0A00003A */
    IL_000b:  /* 02   |                  */ ldarg.0
    IL_000c:  /* 28   | (0A)000044       */ call       instance void class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000002*/<!TKey,!TValue>/*1B000002*/::.ctor() /* 0A000044 */
    IL_0011:  /* 2A   |                  */ ret
  } // end of method ThreadSafeDictionary`2::.ctor

} // end of class MvcContrib.FluentHtml.Behaviors.ThreadSafeDictionary`2

.class /*0200000A*/ public abstract auto ansi sealed beforefieldinit MvcContrib.FluentHtml.Behaviors.BehaviorMarkerExtensions
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C000014:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
  .method /*0600001D*/ public hidebysig static 
          class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> 
          ApplyTo<(MvcContrib.FluentHtml.Elements.IElement/*0200001B*/) T>(class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> behaviors,
                                                                           !!T target) cil managed
  // SIG: 10 01 02 15 12 11 01 12 08 15 12 11 01 12 08 1E 00
  {
    .custom /*0C000021:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x255c
    // Code size       46 (0x2e)
    .maxstack  2
    .locals /*11000009*/ init (class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/ V_0,
             class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000033*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> V_1)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 6F   | (0A)000046       */ callvirt   instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000033*/<!0> class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>/*1B00003E*/::GetEnumerator() /* 0A000046 */
    IL_0006:  /* 0B   |                  */ stloc.1
    .try
    {
      IL_0007:  /* 2B   | 0F               */ br.s       IL_0018

      IL_0009:  /* 07   |                  */ ldloc.1
      IL_000a:  /* 6F   | (0A)000047       */ callvirt   instance !0 class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000033*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>/*1B00003F*/::get_Current() /* 0A000047 */
      IL_000f:  /* 0A   |                  */ stloc.0
      IL_0010:  /* 06   |                  */ ldloc.0
      IL_0011:  /* 03   |                  */ ldarg.1
      IL_0012:  /* 28   | (2B)000004       */ call       class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/ MvcContrib.FluentHtml.Behaviors.BehaviorMarkerExtensions/*0200000A*/::ApplyTo<!!0>(class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/,
                                                                                                                                                                                                              !!0) /* 2B000004 */
      IL_0017:  /* 26   |                  */ pop
      IL_0018:  /* 07   |                  */ ldloc.1
      IL_0019:  /* 6F   | (0A)00001A       */ callvirt   instance bool [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000034*/::MoveNext() /* 0A00001A */
      IL_001e:  /* 2D   | E9               */ brtrue.s   IL_0009

      IL_0020:  /* DE   | 0A               */ leave.s    IL_002c

    }  // end .try
    finally
    {
      IL_0022:  /* 07   |                  */ ldloc.1
      IL_0023:  /* 2C   | 06               */ brfalse.s  IL_002b

      IL_0025:  /* 07   |                  */ ldloc.1
      IL_0026:  /* 6F   | (0A)00001B       */ callvirt   instance void [mscorlib/*23000001*/]System.IDisposable/*01000035*/::Dispose() /* 0A00001B */
      IL_002b:  /* DC   |                  */ endfinally
    }  // end handler
    // HEX: 02 00 07 00 1B 22 00 0A 00 00 00 00
    IL_002c:  /* 02   |                  */ ldarg.0
    IL_002d:  /* 2A   |                  */ ret
  } // end of method BehaviorMarkerExtensions::ApplyTo

  .method /*0600001E*/ public hidebysig static 
          class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/ 
          ApplyTo<(MvcContrib.FluentHtml.Elements.IElement/*0200001B*/) T>(class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/ behavior,
                                                                           !!T target) cil managed
  // SIG: 10 01 02 12 08 12 08 1E 00
  {
    .custom /*0C000023:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x25a8
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 28   | (2B)000005       */ call       void MvcContrib.FluentHtml.Behaviors.CacheingBehaviorApplier/*02000007*/::Apply<!!0>(class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/,
                                                                                                                                            !!0) /* 2B000005 */
    IL_0007:  /* 02   |                  */ ldarg.0
    IL_0008:  /* 2A   |                  */ ret
  } // end of method BehaviorMarkerExtensions::ApplyTo

} // end of class MvcContrib.FluentHtml.Behaviors.BehaviorMarkerExtensions

.class /*0200000B*/ public abstract auto ansi sealed beforefieldinit MvcContrib.FluentHtml.Behaviors.BehaviorHelperExtensions
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C000016:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
  .method /*0600001F*/ public hidebysig static 
          !!T  GetAttribute<([mscorlib/*23000001*/]System.Attribute/*01000006*/) T>(class MvcContrib.FluentHtml.Elements.IMemberElement/*0200001C*/ element) cil managed
  // SIG: 10 01 01 1E 00 12 70
  {
    .custom /*0C000025:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x25b2
    // Code size       12 (0xc)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 6F   | (06)000054       */ callvirt   instance class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ MvcContrib.FluentHtml.Elements.IMemberElement/*0200001C*/::get_ForMember() /* 06000054 */
    IL_0006:  /* 28   | (2B)000006       */ call       !!0 MvcContrib.FluentHtml.Behaviors.BehaviorHelperExtensions/*0200000B*/::GetAttribute<!!0>(class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/) /* 2B000006 */
    IL_000b:  /* 2A   |                  */ ret
  } // end of method BehaviorHelperExtensions::GetAttribute

  .method /*06000020*/ public hidebysig static 
          !!T  GetAttribute<([mscorlib/*23000001*/]System.Attribute/*01000006*/) T>(class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ expression) cil managed
  // SIG: 10 01 01 1E 00 12 39
  {
    .custom /*0C000027:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x25c0
    // Code size       63 (0x3f)
    .maxstack  3
    .locals /*1100000A*/ init (object[] V_0,
             !!T V_1,
             !!T V_2)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 2D   | 0A               */ brtrue.s   IL_000d

    IL_0003:  /* 12   | 01               */ ldloca.s   V_1
    IL_0005:  /* FE15 | (1B)000031       */ initobj    !!T/*1B000031*/
    IL_000b:  /* 07   |                  */ ldloc.1
    IL_000c:  /* 2A   |                  */ ret

    IL_000d:  /* 02   |                  */ ldarg.0
    IL_000e:  /* 6F   | (0A)000048       */ callvirt   instance class [mscorlib/*23000001*/]System.Reflection.MemberInfo/*0100003E*/ [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/::get_Member() /* 0A000048 */
    IL_0013:  /* D0   | (1B)000031       */ ldtoken    !!T/*1B000031*/
    IL_0018:  /* 28   | (0A)000021       */ call       class [mscorlib/*23000001*/]System.Type/*01000003*/ [mscorlib/*23000001*/]System.Type/*01000003*/::GetTypeFromHandle(valuetype [mscorlib/*23000001*/]System.RuntimeTypeHandle/*01000036*/) /* 0A000021 */
    IL_001d:  /* 17   |                  */ ldc.i4.1
    IL_001e:  /* 6F   | (0A)000049       */ callvirt   instance object[] [mscorlib/*23000001*/]System.Reflection.MemberInfo/*0100003E*/::GetCustomAttributes(class [mscorlib/*23000001*/]System.Type/*01000003*/,
                                                                                                                                                             bool) /* 0A000049 */
    IL_0023:  /* 0A   |                  */ stloc.0
    IL_0024:  /* 06   |                  */ ldloc.0
    IL_0025:  /* 2C   | 05               */ brfalse.s  IL_002c

    IL_0027:  /* 06   |                  */ ldloc.0
    IL_0028:  /* 8E   |                  */ ldlen
    IL_0029:  /* 69   |                  */ conv.i4
    IL_002a:  /* 2D   | 0A               */ brtrue.s   IL_0036

    IL_002c:  /* 12   | 02               */ ldloca.s   V_2
    IL_002e:  /* FE15 | (1B)000031       */ initobj    !!T/*1B000031*/
    IL_0034:  /* 08   |                  */ ldloc.2
    IL_0035:  /* 2A   |                  */ ret

    IL_0036:  /* 06   |                  */ ldloc.0
    IL_0037:  /* 16   |                  */ ldc.i4.0
    IL_0038:  /* 9A   |                  */ ldelem.ref
    IL_0039:  /* A5   | (1B)000031       */ unbox.any  !!T/*1B000031*/
    IL_003e:  /* 2A   |                  */ ret
  } // end of method BehaviorHelperExtensions::GetAttribute

  .method /*06000021*/ public hidebysig static 
          bool  IsTextElement(class MvcContrib.FluentHtml.Elements.IElement/*0200001B*/ e) cil managed
  // SIG: 00 01 02 12 6C
  {
    .custom /*0C000029:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x260c
    // Code size       64 (0x40)
    .maxstack  2
    .locals /*1100000B*/ init (string V_0,
             string V_1)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 6F   | (06)000045       */ callvirt   instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::get_Builder() /* 06000045 */
    IL_0006:  /* 6F   | (0A)00004A       */ callvirt   instance string [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/::get_TagName() /* 0A00004A */
    IL_000b:  /* 0A   |                  */ stloc.0
    IL_000c:  /* 02   |                  */ ldarg.0
    IL_000d:  /* 72   | (70)00001F       */ ldstr      "type" /* 7000001F */
    IL_0012:  /* 6F   | (06)000048       */ callvirt   instance string MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::GetAttr(string) /* 06000048 */
    IL_0017:  /* 0B   |                  */ stloc.1
    IL_0018:  /* 06   |                  */ ldloc.0
    IL_0019:  /* 72   | (70)000029       */ ldstr      "input" /* 70000029 */
    IL_001e:  /* 28   | (0A)00004B       */ call       bool [mscorlib/*23000001*/]System.String/*0100003F*/::op_Equality(string,
                                                                                                                         string) /* 0A00004B */
    IL_0023:  /* 2C   | 0D               */ brfalse.s  IL_0032

    IL_0025:  /* 07   |                  */ ldloc.1
    IL_0026:  /* 72   | (70)000035       */ ldstr      "text" /* 70000035 */
    IL_002b:  /* 28   | (0A)00004B       */ call       bool [mscorlib/*23000001*/]System.String/*0100003F*/::op_Equality(string,
                                                                                                                         string) /* 0A00004B */
    IL_0030:  /* 2D   | 0C               */ brtrue.s   IL_003e

    IL_0032:  /* 06   |                  */ ldloc.0
    IL_0033:  /* 72   | (70)00003F       */ ldstr      "textarea" /* 7000003F */
    IL_0038:  /* 28   | (0A)00004B       */ call       bool [mscorlib/*23000001*/]System.String/*0100003F*/::op_Equality(string,
                                                                                                                         string) /* 0A00004B */
    IL_003d:  /* 2A   |                  */ ret

    IL_003e:  /* 17   |                  */ ldc.i4.1
    IL_003f:  /* 2A   |                  */ ret
  } // end of method BehaviorHelperExtensions::IsTextElement

  .method /*06000022*/ public hidebysig static 
          class [mscorlib/*23000001*/]System.Type/*01000003*/ 
          GetPropertyOrFieldType(class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ m) cil managed
  // SIG: 00 01 12 0D 12 39
  {
    .custom /*0C00002B:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2658
    // Code size       65 (0x41)
    .maxstack  2
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 6F   | (0A)000048       */ callvirt   instance class [mscorlib/*23000001*/]System.Reflection.MemberInfo/*0100003E*/ [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/::get_Member() /* 0A000048 */
    IL_0006:  /* 6F   | (0A)00004C       */ callvirt   instance valuetype [mscorlib/*23000001*/]System.Reflection.MemberTypes/*01000040*/ [mscorlib/*23000001*/]System.Reflection.MemberInfo/*0100003E*/::get_MemberType() /* 0A00004C */
    IL_000b:  /* 1F   | 10               */ ldc.i4.s   16
    IL_000d:  /* 33   | 11               */ bne.un.s   IL_0020

    IL_000f:  /* 02   |                  */ ldarg.0
    IL_0010:  /* 6F   | (0A)000048       */ callvirt   instance class [mscorlib/*23000001*/]System.Reflection.MemberInfo/*0100003E*/ [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/::get_Member() /* 0A000048 */
    IL_0015:  /* 74   | (01)000041       */ castclass  [mscorlib/*23000001*/]System.Reflection.PropertyInfo/*01000041*/
    IL_001a:  /* 6F   | (0A)00004D       */ callvirt   instance class [mscorlib/*23000001*/]System.Type/*01000003*/ [mscorlib/*23000001*/]System.Reflection.PropertyInfo/*01000041*/::get_PropertyType() /* 0A00004D */
    IL_001f:  /* 2A   |                  */ ret

    IL_0020:  /* 02   |                  */ ldarg.0
    IL_0021:  /* 6F   | (0A)000048       */ callvirt   instance class [mscorlib/*23000001*/]System.Reflection.MemberInfo/*0100003E*/ [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/::get_Member() /* 0A000048 */
    IL_0026:  /* 6F   | (0A)00004C       */ callvirt   instance valuetype [mscorlib/*23000001*/]System.Reflection.MemberTypes/*01000040*/ [mscorlib/*23000001*/]System.Reflection.MemberInfo/*0100003E*/::get_MemberType() /* 0A00004C */
    IL_002b:  /* 1A   |                  */ ldc.i4.4
    IL_002c:  /* 33   | 11               */ bne.un.s   IL_003f

    IL_002e:  /* 02   |                  */ ldarg.0
    IL_002f:  /* 6F   | (0A)000048       */ callvirt   instance class [mscorlib/*23000001*/]System.Reflection.MemberInfo/*0100003E*/ [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/::get_Member() /* 0A000048 */
    IL_0034:  /* 74   | (01)000042       */ castclass  [mscorlib/*23000001*/]System.Reflection.FieldInfo/*01000042*/
    IL_0039:  /* 6F   | (0A)00004E       */ callvirt   instance class [mscorlib/*23000001*/]System.Type/*01000003*/ [mscorlib/*23000001*/]System.Reflection.FieldInfo/*01000042*/::get_FieldType() /* 0A00004E */
    IL_003e:  /* 2A   |                  */ ret

    IL_003f:  /* 14   |                  */ ldnull
    IL_0040:  /* 2A   |                  */ ret
  } // end of method BehaviorHelperExtensions::GetPropertyOrFieldType

  .method /*06000023*/ public hidebysig static 
          bool  IsNumber(class [mscorlib/*23000001*/]System.Type/*01000003*/ t) cil managed
  // SIG: 00 01 02 12 0D
  {
    .custom /*0C00002C:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x26a8
    // Code size       128 (0x80)
    .maxstack  2
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000024       */ call       class [mscorlib/*23000001*/]System.Type/*01000003*/ MvcContrib.FluentHtml.Behaviors.BehaviorHelperExtensions/*0200000B*/::GetTypeWithoutNullability(class [mscorlib/*23000001*/]System.Type/*01000003*/) /* 06000024 */
    IL_0006:  /* 10   | 00               */ starg.s    t
    IL_0008:  /* 02   |                  */ ldarg.0
    IL_0009:  /* D0   | (01)000043       */ ldtoken    [mscorlib/*23000001*/]System.Int16/*01000043*/
    IL_000e:  /* 28   | (0A)000021       */ call       class [mscorlib/*23000001*/]System.Type/*01000003*/ [mscorlib/*23000001*/]System.Type/*01000003*/::GetTypeFromHandle(valuetype [mscorlib/*23000001*/]System.RuntimeTypeHandle/*01000036*/) /* 0A000021 */
    IL_0013:  /* 2E   | 69               */ beq.s      IL_007e

    IL_0015:  /* 02   |                  */ ldarg.0
    IL_0016:  /* D0   | (01)000044       */ ldtoken    [mscorlib/*23000001*/]System.Int32/*01000044*/
    IL_001b:  /* 28   | (0A)000021       */ call       class [mscorlib/*23000001*/]System.Type/*01000003*/ [mscorlib/*23000001*/]System.Type/*01000003*/::GetTypeFromHandle(valuetype [mscorlib/*23000001*/]System.RuntimeTypeHandle/*01000036*/) /* 0A000021 */
    IL_0020:  /* 2E   | 5C               */ beq.s      IL_007e

    IL_0022:  /* 02   |                  */ ldarg.0
    IL_0023:  /* D0   | (01)000045       */ ldtoken    [mscorlib/*23000001*/]System.Int64/*01000045*/
    IL_0028:  /* 28   | (0A)000021       */ call       class [mscorlib/*23000001*/]System.Type/*01000003*/ [mscorlib/*23000001*/]System.Type/*01000003*/::GetTypeFromHandle(valuetype [mscorlib/*23000001*/]System.RuntimeTypeHandle/*01000036*/) /* 0A000021 */
    IL_002d:  /* 2E   | 4F               */ beq.s      IL_007e

    IL_002f:  /* 02   |                  */ ldarg.0
    IL_0030:  /* D0   | (01)000046       */ ldtoken    [mscorlib/*23000001*/]System.UInt16/*01000046*/
    IL_0035:  /* 28   | (0A)000021       */ call       class [mscorlib/*23000001*/]System.Type/*01000003*/ [mscorlib/*23000001*/]System.Type/*01000003*/::GetTypeFromHandle(valuetype [mscorlib/*23000001*/]System.RuntimeTypeHandle/*01000036*/) /* 0A000021 */
    IL_003a:  /* 2E   | 42               */ beq.s      IL_007e

    IL_003c:  /* 02   |                  */ ldarg.0
    IL_003d:  /* D0   | (01)000047       */ ldtoken    [mscorlib/*23000001*/]System.UInt32/*01000047*/
    IL_0042:  /* 28   | (0A)000021       */ call       class [mscorlib/*23000001*/]System.Type/*01000003*/ [mscorlib/*23000001*/]System.Type/*01000003*/::GetTypeFromHandle(valuetype [mscorlib/*23000001*/]System.RuntimeTypeHandle/*01000036*/) /* 0A000021 */
    IL_0047:  /* 2E   | 35               */ beq.s      IL_007e

    IL_0049:  /* 02   |                  */ ldarg.0
    IL_004a:  /* D0   | (01)000048       */ ldtoken    [mscorlib/*23000001*/]System.UInt64/*01000048*/
    IL_004f:  /* 28   | (0A)000021       */ call       class [mscorlib/*23000001*/]System.Type/*01000003*/ [mscorlib/*23000001*/]System.Type/*01000003*/::GetTypeFromHandle(valuetype [mscorlib/*23000001*/]System.RuntimeTypeHandle/*01000036*/) /* 0A000021 */
    IL_0054:  /* 2E   | 28               */ beq.s      IL_007e

    IL_0056:  /* 02   |                  */ ldarg.0
    IL_0057:  /* D0   | (01)000049       */ ldtoken    [mscorlib/*23000001*/]System.Decimal/*01000049*/
    IL_005c:  /* 28   | (0A)000021       */ call       class [mscorlib/*23000001*/]System.Type/*01000003*/ [mscorlib/*23000001*/]System.Type/*01000003*/::GetTypeFromHandle(valuetype [mscorlib/*23000001*/]System.RuntimeTypeHandle/*01000036*/) /* 0A000021 */
    IL_0061:  /* 2E   | 1B               */ beq.s      IL_007e

    IL_0063:  /* 02   |                  */ ldarg.0
    IL_0064:  /* D0   | (01)00004A       */ ldtoken    [mscorlib/*23000001*/]System.Single/*0100004A*/
    IL_0069:  /* 28   | (0A)000021       */ call       class [mscorlib/*23000001*/]System.Type/*01000003*/ [mscorlib/*23000001*/]System.Type/*01000003*/::GetTypeFromHandle(valuetype [mscorlib/*23000001*/]System.RuntimeTypeHandle/*01000036*/) /* 0A000021 */
    IL_006e:  /* 2E   | 0E               */ beq.s      IL_007e

    IL_0070:  /* 02   |                  */ ldarg.0
    IL_0071:  /* D0   | (01)00004B       */ ldtoken    [mscorlib/*23000001*/]System.Double/*0100004B*/
    IL_0076:  /* 28   | (0A)000021       */ call       class [mscorlib/*23000001*/]System.Type/*01000003*/ [mscorlib/*23000001*/]System.Type/*01000003*/::GetTypeFromHandle(valuetype [mscorlib/*23000001*/]System.RuntimeTypeHandle/*01000036*/) /* 0A000021 */
    IL_007b:  /* FE01 |                  */ ceq
    IL_007d:  /* 2A   |                  */ ret

    IL_007e:  /* 17   |                  */ ldc.i4.1
    IL_007f:  /* 2A   |                  */ ret
  } // end of method BehaviorHelperExtensions::IsNumber

  .method /*06000024*/ public hidebysig static 
          class [mscorlib/*23000001*/]System.Type/*01000003*/ 
          GetTypeWithoutNullability(class [mscorlib/*23000001*/]System.Type/*01000003*/ t) cil managed
  // SIG: 00 01 12 0D 12 0D
  {
    .custom /*0C00002D:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2734
    // Code size       22 (0x16)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000025       */ call       bool MvcContrib.FluentHtml.Behaviors.BehaviorHelperExtensions/*0200000B*/::IsNullableType(class [mscorlib/*23000001*/]System.Type/*01000003*/) /* 06000025 */
    IL_0006:  /* 2D   | 02               */ brtrue.s   IL_000a

    IL_0008:  /* 02   |                  */ ldarg.0
    IL_0009:  /* 2A   |                  */ ret

    IL_000a:  /* 02   |                  */ ldarg.0
    IL_000b:  /* 73   | (0A)00004F       */ newobj     instance void [System/*23000004*/]System.ComponentModel.NullableConverter/*0100004C*/::.ctor(class [mscorlib/*23000001*/]System.Type/*01000003*/) /* 0A00004F */
    IL_0010:  /* 28   | (0A)000050       */ call       instance class [mscorlib/*23000001*/]System.Type/*01000003*/ [System/*23000004*/]System.ComponentModel.NullableConverter/*0100004C*/::get_UnderlyingType() /* 0A000050 */
    IL_0015:  /* 2A   |                  */ ret
  } // end of method BehaviorHelperExtensions::GetTypeWithoutNullability

  .method /*06000025*/ public hidebysig static 
          bool  IsNullableType(class [mscorlib/*23000001*/]System.Type/*01000003*/ t) cil managed
  // SIG: 00 01 02 12 0D
  {
    .custom /*0C00002E:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x274b
    // Code size       29 (0x1d)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 6F   | (0A)000051       */ callvirt   instance bool [mscorlib/*23000001*/]System.Type/*01000003*/::get_IsGenericType() /* 0A000051 */
    IL_0006:  /* 2C   | 13               */ brfalse.s  IL_001b

    IL_0008:  /* 02   |                  */ ldarg.0
    IL_0009:  /* 6F   | (0A)000052       */ callvirt   instance class [mscorlib/*23000001*/]System.Type/*01000003*/ [mscorlib/*23000001*/]System.Type/*01000003*/::GetGenericTypeDefinition() /* 0A000052 */
    IL_000e:  /* D0   | (01)00004D       */ ldtoken    [mscorlib/*23000001*/]System.Nullable`1/*0100004D*/
    IL_0013:  /* 28   | (0A)000021       */ call       class [mscorlib/*23000001*/]System.Type/*01000003*/ [mscorlib/*23000001*/]System.Type/*01000003*/::GetTypeFromHandle(valuetype [mscorlib/*23000001*/]System.RuntimeTypeHandle/*01000036*/) /* 0A000021 */
    IL_0018:  /* FE01 |                  */ ceq
    IL_001a:  /* 2A   |                  */ ret

    IL_001b:  /* 16   |                  */ ldc.i4.0
    IL_001c:  /* 2A   |                  */ ret
  } // end of method BehaviorHelperExtensions::IsNullableType

} // end of class MvcContrib.FluentHtml.Behaviors.BehaviorHelperExtensions

.class /*0200000C*/ public abstract auto ansi beforefieldinit MvcContrib.FluentHtml.Behaviors.ThreadSafeMemberBehavior
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
       implements class MvcContrib.FluentHtml.Behaviors.IBehavior`1/*02000003*/<class MvcContrib.FluentHtml.Elements.IMemberElement/*0200001C*/>/*1B000004*/,
                  MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/
{
  .field /*0400000C*/ private static initonly object objLock
  .method /*06000026*/ public hidebysig newslot virtual final 
          instance void  Execute(class MvcContrib.FluentHtml.Elements.IMemberElement/*0200001C*/ element) cil managed
  // SIG: 20 01 01 12 70
  {
    // Method begins at RVA 0x276c
    // Code size       29 (0x1d)
    .maxstack  2
    .locals /*11000001*/ init (object V_0)
    IL_0000:  /* 7E   | (04)00000C       */ ldsfld     object MvcContrib.FluentHtml.Behaviors.ThreadSafeMemberBehavior/*0200000C*/::objLock /* 0400000C */
    IL_0005:  /* 25   |                  */ dup
    IL_0006:  /* 0A   |                  */ stloc.0
    IL_0007:  /* 28   | (0A)000010       */ call       void [mscorlib/*23000001*/]System.Threading.Monitor/*01000031*/::Enter(object) /* 0A000010 */
    .try
    {
      IL_000c:  /* 02   |                  */ ldarg.0
      IL_000d:  /* 03   |                  */ ldarg.1
      IL_000e:  /* 6F   | (06)000027       */ callvirt   instance void MvcContrib.FluentHtml.Behaviors.ThreadSafeMemberBehavior/*0200000C*/::DoExecute(class MvcContrib.FluentHtml.Elements.IMemberElement/*0200001C*/) /* 06000027 */
      IL_0013:  /* DE   | 07               */ leave.s    IL_001c

    }  // end .try
    finally
    {
      IL_0015:  /* 06   |                  */ ldloc.0
      IL_0016:  /* 28   | (0A)000011       */ call       void [mscorlib/*23000001*/]System.Threading.Monitor/*01000031*/::Exit(object) /* 0A000011 */
      IL_001b:  /* DC   |                  */ endfinally
    }  // end handler
    // HEX: 02 00 0C 00 09 15 00 07 00 00 00 00
    IL_001c:  /* 2A   |                  */ ret
  } // end of method ThreadSafeMemberBehavior::Execute

  .method /*06000027*/ family hidebysig newslot abstract virtual 
          instance void  DoExecute(class MvcContrib.FluentHtml.Elements.IMemberElement/*0200001C*/ element) cil managed
  // SIG: 20 01 01 12 70
  {
    // Method begins at RVA 0x0
  } // end of method ThreadSafeMemberBehavior::DoExecute

  .method /*06000028*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x27b4
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000012       */ call       instance void [mscorlib/*23000001*/]System.Object/*01000001*/::.ctor() /* 0A000012 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ThreadSafeMemberBehavior::.ctor

  .method /*06000029*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x27a8
    // Code size       11 (0xb)
    .maxstack  8
    IL_0000:  /* 73   | (0A)000012       */ newobj     instance void [mscorlib/*23000001*/]System.Object/*01000001*/::.ctor() /* 0A000012 */
    IL_0005:  /* 80   | (04)00000C       */ stsfld     object MvcContrib.FluentHtml.Behaviors.ThreadSafeMemberBehavior/*0200000C*/::objLock /* 0400000C */
    IL_000a:  /* 2A   |                  */ ret
  } // end of method ThreadSafeMemberBehavior::.cctor

} // end of class MvcContrib.FluentHtml.Behaviors.ThreadSafeMemberBehavior

.class /*0200000D*/ public auto ansi beforefieldinit MvcContrib.FluentHtml.Behaviors.RequiredCssClassBehavior
       extends MvcContrib.FluentHtml.Behaviors.ThreadSafeMemberBehavior/*0200000C*/
{
  .field /*0400000D*/ private initonly string requiredClass
  .method /*0600002A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x27bc
    // Code size       12 (0xc)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 72   | (70)000051       */ ldstr      "required" /* 70000051 */
    IL_0006:  /* 28   | (06)00002B       */ call       instance void MvcContrib.FluentHtml.Behaviors.RequiredCssClassBehavior/*0200000D*/::.ctor(string) /* 0600002B */
    IL_000b:  /* 2A   |                  */ ret
  } // end of method RequiredCssClassBehavior::.ctor

  .method /*0600002B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string requiredClass) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x27c9
    // Code size       14 (0xe)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000028       */ call       instance void MvcContrib.FluentHtml.Behaviors.ThreadSafeMemberBehavior/*0200000C*/::.ctor() /* 06000028 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)00000D       */ stfld      string MvcContrib.FluentHtml.Behaviors.RequiredCssClassBehavior/*0200000D*/::requiredClass /* 0400000D */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method RequiredCssClassBehavior::.ctor

  .method /*0600002C*/ family hidebysig virtual 
          instance void  DoExecute(class MvcContrib.FluentHtml.Elements.IMemberElement/*0200001C*/ element) cil managed
  // SIG: 20 01 01 12 70
  {
    // Method begins at RVA 0x27d8
    // Code size       22 (0x16)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 6F   | (06)00002D       */ callvirt   instance bool MvcContrib.FluentHtml.Behaviors.RequiredCssClassBehavior/*0200000D*/::IsRequired(class MvcContrib.FluentHtml.Elements.IMemberElement/*0200001C*/) /* 0600002D */
    IL_0007:  /* 2C   | 0C               */ brfalse.s  IL_0015

    IL_0009:  /* 03   |                  */ ldarg.1
    IL_000a:  /* 02   |                  */ ldarg.0
    IL_000b:  /* 7B   | (04)00000D       */ ldfld      string MvcContrib.FluentHtml.Behaviors.RequiredCssClassBehavior/*0200000D*/::requiredClass /* 0400000D */
    IL_0010:  /* 6F   | (06)000053       */ callvirt   instance void MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::AddClass(string) /* 06000053 */
    IL_0015:  /* 2A   |                  */ ret
  } // end of method RequiredCssClassBehavior::DoExecute

  .method /*0600002D*/ family hidebysig newslot virtual 
          instance bool  IsRequired(class MvcContrib.FluentHtml.Elements.IMemberElement/*0200001C*/ element) cil managed
  // SIG: 20 01 02 12 70
  {
    // Method begins at RVA 0x27ef
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 28   | (06)00002E       */ call       instance bool MvcContrib.FluentHtml.Behaviors.RequiredCssClassBehavior/*0200000D*/::IsAutomaticallyRequired(class MvcContrib.FluentHtml.Elements.IMemberElement/*0200001C*/) /* 0600002E */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method RequiredCssClassBehavior::IsRequired

  .method /*0600002E*/ family hidebysig instance bool 
          IsAutomaticallyRequired(class MvcContrib.FluentHtml.Elements.IMemberElement/*0200001C*/ element) cil managed
  // SIG: 20 01 02 12 70
  {
    // Method begins at RVA 0x27f8
    // Code size       55 (0x37)
    .maxstack  2
    .locals /*1100000C*/ init (class [mscorlib/*23000001*/]System.Type/*01000003*/ V_0)
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 6F   | (06)000054       */ callvirt   instance class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ MvcContrib.FluentHtml.Elements.IMemberElement/*0200001C*/::get_ForMember() /* 06000054 */
    IL_0006:  /* 2C   | 08               */ brfalse.s  IL_0010

    IL_0008:  /* 03   |                  */ ldarg.1
    IL_0009:  /* 28   | (06)000021       */ call       bool MvcContrib.FluentHtml.Behaviors.BehaviorHelperExtensions/*0200000B*/::IsTextElement(class MvcContrib.FluentHtml.Elements.IElement/*0200001B*/) /* 06000021 */
    IL_000e:  /* 2D   | 02               */ brtrue.s   IL_0012

    IL_0010:  /* 16   |                  */ ldc.i4.0
    IL_0011:  /* 2A   |                  */ ret

    IL_0012:  /* 03   |                  */ ldarg.1
    IL_0013:  /* 6F   | (06)000054       */ callvirt   instance class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ MvcContrib.FluentHtml.Elements.IMemberElement/*0200001C*/::get_ForMember() /* 06000054 */
    IL_0018:  /* 28   | (06)000022       */ call       class [mscorlib/*23000001*/]System.Type/*01000003*/ MvcContrib.FluentHtml.Behaviors.BehaviorHelperExtensions/*0200000B*/::GetPropertyOrFieldType(class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/) /* 06000022 */
    IL_001d:  /* 0A   |                  */ stloc.0
    IL_001e:  /* 06   |                  */ ldloc.0
    IL_001f:  /* 2D   | 02               */ brtrue.s   IL_0023

    IL_0021:  /* 16   |                  */ ldc.i4.0
    IL_0022:  /* 2A   |                  */ ret

    IL_0023:  /* 06   |                  */ ldloc.0
    IL_0024:  /* 6F   | (0A)000053       */ callvirt   instance bool [mscorlib/*23000001*/]System.Type/*01000003*/::get_IsValueType() /* 0A000053 */
    IL_0029:  /* 2C   | 0A               */ brfalse.s  IL_0035

    IL_002b:  /* 06   |                  */ ldloc.0
    IL_002c:  /* 28   | (06)000025       */ call       bool MvcContrib.FluentHtml.Behaviors.BehaviorHelperExtensions/*0200000B*/::IsNullableType(class [mscorlib/*23000001*/]System.Type/*01000003*/) /* 06000025 */
    IL_0031:  /* 16   |                  */ ldc.i4.0
    IL_0032:  /* FE01 |                  */ ceq
    IL_0034:  /* 2A   |                  */ ret

    IL_0035:  /* 16   |                  */ ldc.i4.0
    IL_0036:  /* 2A   |                  */ ret
  } // end of method RequiredCssClassBehavior::IsAutomaticallyRequired

} // end of class MvcContrib.FluentHtml.Behaviors.RequiredCssClassBehavior

.class /*0200000E*/ public auto ansi beforefieldinit MvcContrib.FluentHtml.Behaviors.RequiredCssClassBehavior`1<([mscorlib/*23000001*/]System.Attribute/*01000006*/) TRequiredAttribute>
       extends MvcContrib.FluentHtml.Behaviors.RequiredCssClassBehavior/*0200000D*/
{
  .method /*0600002F*/ family hidebysig virtual 
          instance bool  IsRequired(class MvcContrib.FluentHtml.Elements.IMemberElement/*0200001C*/ element) cil managed
  // SIG: 20 01 02 12 70
  {
    // Method begins at RVA 0x283b
    // Code size       29 (0x1d)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 28   | (06)00002D       */ call       instance bool MvcContrib.FluentHtml.Behaviors.RequiredCssClassBehavior/*0200000D*/::IsRequired(class MvcContrib.FluentHtml.Elements.IMemberElement/*0200001C*/) /* 0600002D */
    IL_0007:  /* 2D   | 12               */ brtrue.s   IL_001b

    IL_0009:  /* 03   |                  */ ldarg.1
    IL_000a:  /* 28   | (2B)000007       */ call       !!0 MvcContrib.FluentHtml.Behaviors.BehaviorHelperExtensions/*0200000B*/::GetAttribute<!TRequiredAttribute>(class MvcContrib.FluentHtml.Elements.IMemberElement/*0200001C*/) /* 2B000007 */
    IL_000f:  /* 8C   | (1B)000040       */ box        !TRequiredAttribute/*1B000040*/
    IL_0014:  /* 14   |                  */ ldnull
    IL_0015:  /* FE01 |                  */ ceq
    IL_0017:  /* 16   |                  */ ldc.i4.0
    IL_0018:  /* FE01 |                  */ ceq
    IL_001a:  /* 2A   |                  */ ret

    IL_001b:  /* 17   |                  */ ldc.i4.1
    IL_001c:  /* 2A   |                  */ ret
  } // end of method RequiredCssClassBehavior`1::IsRequired

  .method /*06000030*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2859
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00002A       */ call       instance void MvcContrib.FluentHtml.Behaviors.RequiredCssClassBehavior/*0200000D*/::.ctor() /* 0600002A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method RequiredCssClassBehavior`1::.ctor

} // end of class MvcContrib.FluentHtml.Behaviors.RequiredCssClassBehavior`1

.class /*0200000F*/ public auto ansi beforefieldinit MvcContrib.FluentHtml.Behaviors.GenericTypeSpec
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .field /*0400000E*/ public class [mscorlib/*23000001*/]System.Type/*01000003*/ OpenGenericType
  .field /*0400000F*/ public class [mscorlib/*23000001*/]System.Type/*01000003*/ GenericParameterType
  .method /*06000031*/ public hidebysig specialname 
          instance class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class [mscorlib/*23000001*/]System.Type/*01000003*/,bool> 
          get_Matcher() cil managed
  // SIG: 20 00 15 12 2D 02 12 0D 02
  {
    // Method begins at RVA 0x288e
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* FE06 | (06)000033       */ ldftn      instance bool MvcContrib.FluentHtml.Behaviors.GenericTypeSpec/*0200000F*/::'<get_Matcher>b__0'(class [mscorlib/*23000001*/]System.Type/*01000003*/) /* 06000033 */
    IL_0007:  /* 73   | (0A)000023       */ newobj     instance void class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class [mscorlib/*23000001*/]System.Type/*01000003*/,bool>/*1B000038*/::.ctor(object,
                                                                                                                                                                                                           native int) /* 0A000023 */
    IL_000c:  /* 2A   |                  */ ret
  } // end of method GenericTypeSpec::get_Matcher

  .method /*06000032*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x289c
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000012       */ call       instance void [mscorlib/*23000001*/]System.Object/*01000001*/::.ctor() /* 0A000012 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method GenericTypeSpec::.ctor

  .method /*06000033*/ private hidebysig 
          instance bool  '<get_Matcher>b__0'(class [mscorlib/*23000001*/]System.Type/*01000003*/ x) cil managed
  // SIG: 20 01 02 12 0D
  {
    .custom /*0C000031:0A000013*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000032*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2861
    // Code size       44 (0x2c)
    .maxstack  8
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 6F   | (0A)000051       */ callvirt   instance bool [mscorlib/*23000001*/]System.Type/*01000003*/::get_IsGenericType() /* 0A000051 */
    IL_0006:  /* 2C   | 22               */ brfalse.s  IL_002a

    IL_0008:  /* 03   |                  */ ldarg.1
    IL_0009:  /* 6F   | (0A)000052       */ callvirt   instance class [mscorlib/*23000001*/]System.Type/*01000003*/ [mscorlib/*23000001*/]System.Type/*01000003*/::GetGenericTypeDefinition() /* 0A000052 */
    IL_000e:  /* 02   |                  */ ldarg.0
    IL_000f:  /* 7B   | (04)00000E       */ ldfld      class [mscorlib/*23000001*/]System.Type/*01000003*/ MvcContrib.FluentHtml.Behaviors.GenericTypeSpec/*0200000F*/::OpenGenericType /* 0400000E */
    IL_0014:  /* 33   | 14               */ bne.un.s   IL_002a

    IL_0016:  /* 03   |                  */ ldarg.1
    IL_0017:  /* 6F   | (0A)000031       */ callvirt   instance class [mscorlib/*23000001*/]System.Type/*01000003*/[] [mscorlib/*23000001*/]System.Type/*01000003*/::GetGenericArguments() /* 0A000031 */
    IL_001c:  /* 16   |                  */ ldc.i4.0
    IL_001d:  /* 9A   |                  */ ldelem.ref
    IL_001e:  /* 02   |                  */ ldarg.0
    IL_001f:  /* 7B   | (04)00000F       */ ldfld      class [mscorlib/*23000001*/]System.Type/*01000003*/ MvcContrib.FluentHtml.Behaviors.GenericTypeSpec/*0200000F*/::GenericParameterType /* 0400000F */
    IL_0024:  /* 6F   | (0A)000054       */ callvirt   instance bool [mscorlib/*23000001*/]System.Type/*01000003*/::IsAssignableFrom(class [mscorlib/*23000001*/]System.Type/*01000003*/) /* 0A000054 */
    IL_0029:  /* 2A   |                  */ ret

    IL_002a:  /* 16   |                  */ ldc.i4.0
    IL_002b:  /* 2A   |                  */ ret
  } // end of method GenericTypeSpec::'<get_Matcher>b__0'

  .property /*17000004*/ instance class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class [mscorlib/*23000001*/]System.Type/*01000003*/,bool>
          Matcher()
  {
    .get instance class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class [mscorlib/*23000001*/]System.Type/*01000003*/,bool> MvcContrib.FluentHtml.Behaviors.GenericTypeSpec/*0200000F*/::get_Matcher() /* 06000031 */
  } // end of property GenericTypeSpec::Matcher
} // end of class MvcContrib.FluentHtml.Behaviors.GenericTypeSpec

.class /*02000010*/ interface public abstract auto ansi MvcContrib.FluentHtml.Behaviors.IOrderedBehavior
       implements MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/
{
  .method /*06000034*/ public hidebysig newslot specialname abstract virtual 
          instance int32  get_Order() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x0
  } // end of method IOrderedBehavior::get_Order

  .property /*17000005*/ instance int32 Order()
  {
    .get instance int32 MvcContrib.FluentHtml.Behaviors.IOrderedBehavior/*02000010*/::get_Order() /* 06000034 */
  } // end of property IOrderedBehavior::Order
} // end of class MvcContrib.FluentHtml.Behaviors.IOrderedBehavior

.class /*02000011*/ interface public abstract auto ansi MvcContrib.FluentHtml.Behaviors.IOrderedBehavior`1<T>
       implements MvcContrib.FluentHtml.Behaviors.IOrderedBehavior/*02000010*/,
                  MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/
{
  .method /*06000035*/ public hidebysig newslot abstract virtual 
          instance void  Execute(!T behavee) cil managed
  // SIG: 20 01 01 13 00
  {
    // Method begins at RVA 0x0
  } // end of method IOrderedBehavior`1::Execute

} // end of class MvcContrib.FluentHtml.Behaviors.IOrderedBehavior`1

.class /*02000012*/ interface public abstract auto ansi MvcContrib.FluentHtml.Behaviors.IBehavior
       implements class MvcContrib.FluentHtml.Behaviors.IBehavior`1/*02000003*/<class MvcContrib.FluentHtml.Elements.IElement/*0200001B*/>/*1B000001*/,
                  MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/
{
  .custom /*0C00001B:0A000055*/ instance void [mscorlib/*23000001*/]System.ObsoleteAttribute/*0100004E*/::.ctor(string) /* 0A000055 */ = ( 01 00 20 55 73 65 20 49 42 65 68 61 76 69 6F 72   // .. Use IBehavior
                                                                                                                                           3C 49 45 6C 65 6D 65 6E 74 3E 20 69 6E 73 74 65   // <IElement> inste
                                                                                                                                           61 64 2E 00 00 )                                  // ad...
} // end of class MvcContrib.FluentHtml.Behaviors.IBehavior

.class /*02000013*/ interface public abstract auto ansi MvcContrib.FluentHtml.Behaviors.IBehaviorsContainer
{
  .method /*06000036*/ public hidebysig newslot specialname abstract virtual 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> 
          get_Behaviors() cil managed
  // SIG: 20 00 15 12 11 01 12 08
  {
    // Method begins at RVA 0x0
  } // end of method IBehaviorsContainer::get_Behaviors

  .property /*17000006*/ instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>
          Behaviors()
  {
    .get instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> MvcContrib.FluentHtml.Behaviors.IBehaviorsContainer/*02000013*/::get_Behaviors() /* 06000036 */
  } // end of property IBehaviorsContainer::Behaviors
} // end of class MvcContrib.FluentHtml.Behaviors.IBehaviorsContainer

.class /*02000014*/ interface public abstract auto ansi MvcContrib.FluentHtml.Behaviors.IModelStateHandler
{
  .method /*06000037*/ public hidebysig newslot abstract virtual 
          instance bool  Handle(class MvcContrib.FluentHtml.Elements.IElement/*0200001B*/ element,
                                class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelState/*0100000F*/ state) cil managed
  // SIG: 20 02 02 12 6C 12 3D
  {
    // Method begins at RVA 0x0
  } // end of method IModelStateHandler::Handle

} // end of class MvcContrib.FluentHtml.Behaviors.IModelStateHandler

.class /*02000015*/ interface public abstract auto ansi MvcContrib.FluentHtml.Behaviors.ISupportsMaxLength
{
} // end of class MvcContrib.FluentHtml.Behaviors.ISupportsMaxLength

.class /*02000016*/ interface public abstract auto ansi MvcContrib.FluentHtml.Behaviors.ISupportsModelState
{
  .method /*06000038*/ public hidebysig newslot abstract virtual 
          instance void  ApplyModelState(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelState/*0100000F*/ state) cil managed
  // SIG: 20 01 01 12 3D
  {
    // Method begins at RVA 0x0
  } // end of method ISupportsModelState::ApplyModelState

} // end of class MvcContrib.FluentHtml.Behaviors.ISupportsModelState

.class /*02000017*/ public auto ansi beforefieldinit MvcContrib.FluentHtml.Behaviors.MemberBehaviorHelper`1<([mscorlib/*23000001*/]System.Attribute/*01000006*/) T>
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C00001C:0A000055*/ instance void [mscorlib/*23000001*/]System.ObsoleteAttribute/*0100004E*/::.ctor(string) /* 0A000055 */ = ( 01 00 26 55 73 65 20 4D 65 6D 62 65 72 20 45 6C   // ..&Use Member El
                                                                                                                                           65 6D 65 6E 74 20 65 78 74 65 6E 73 69 6F 6E 73   // ement extensions
                                                                                                                                           20 69 6E 73 74 65 61 64 2E 00 00 )                //  instead...
  .method /*06000039*/ public hidebysig instance !T 
          GetAttribute(class MvcContrib.FluentHtml.Elements.IMemberElement/*0200001C*/ element) cil managed
  // SIG: 20 01 13 00 12 70
  {
    // Method begins at RVA 0x28a4
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 6F   | (06)000054       */ callvirt   instance class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ MvcContrib.FluentHtml.Elements.IMemberElement/*0200001C*/::get_ForMember() /* 06000054 */
    IL_0007:  /* 28   | (0A)000056       */ call       instance !0 class MvcContrib.FluentHtml.Behaviors.MemberBehaviorHelper`1/*02000017*/<!T>/*1B000041*/::GetAttribute(class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/) /* 0A000056 */
    IL_000c:  /* 2A   |                  */ ret
  } // end of method MemberBehaviorHelper`1::GetAttribute

  .method /*0600003A*/ public hidebysig instance !T 
          GetAttribute(class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ expression) cil managed
  // SIG: 20 01 13 00 12 39
  {
    // Method begins at RVA 0x28b4
    // Code size       63 (0x3f)
    .maxstack  3
    .locals /*1100000D*/ init (object[] V_0,
             !T V_1,
             !T V_2)
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 2D   | 0A               */ brtrue.s   IL_000d

    IL_0003:  /* 12   | 01               */ ldloca.s   V_1
    IL_0005:  /* FE15 | (1B)000040       */ initobj    !T/*1B000040*/
    IL_000b:  /* 07   |                  */ ldloc.1
    IL_000c:  /* 2A   |                  */ ret

    IL_000d:  /* 03   |                  */ ldarg.1
    IL_000e:  /* 6F   | (0A)000048       */ callvirt   instance class [mscorlib/*23000001*/]System.Reflection.MemberInfo/*0100003E*/ [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/::get_Member() /* 0A000048 */
    IL_0013:  /* D0   | (1B)000040       */ ldtoken    !T/*1B000040*/
    IL_0018:  /* 28   | (0A)000021       */ call       class [mscorlib/*23000001*/]System.Type/*01000003*/ [mscorlib/*23000001*/]System.Type/*01000003*/::GetTypeFromHandle(valuetype [mscorlib/*23000001*/]System.RuntimeTypeHandle/*01000036*/) /* 0A000021 */
    IL_001d:  /* 17   |                  */ ldc.i4.1
    IL_001e:  /* 6F   | (0A)000049       */ callvirt   instance object[] [mscorlib/*23000001*/]System.Reflection.MemberInfo/*0100003E*/::GetCustomAttributes(class [mscorlib/*23000001*/]System.Type/*01000003*/,
                                                                                                                                                             bool) /* 0A000049 */
    IL_0023:  /* 0A   |                  */ stloc.0
    IL_0024:  /* 06   |                  */ ldloc.0
    IL_0025:  /* 2C   | 05               */ brfalse.s  IL_002c

    IL_0027:  /* 06   |                  */ ldloc.0
    IL_0028:  /* 8E   |                  */ ldlen
    IL_0029:  /* 69   |                  */ conv.i4
    IL_002a:  /* 2D   | 0A               */ brtrue.s   IL_0036

    IL_002c:  /* 12   | 02               */ ldloca.s   V_2
    IL_002e:  /* FE15 | (1B)000040       */ initobj    !T/*1B000040*/
    IL_0034:  /* 08   |                  */ ldloc.2
    IL_0035:  /* 2A   |                  */ ret

    IL_0036:  /* 06   |                  */ ldloc.0
    IL_0037:  /* 16   |                  */ ldc.i4.0
    IL_0038:  /* 9A   |                  */ ldelem.ref
    IL_0039:  /* A5   | (1B)000040       */ unbox.any  !T/*1B000040*/
    IL_003e:  /* 2A   |                  */ ret
  } // end of method MemberBehaviorHelper`1::GetAttribute

  .method /*0600003B*/ public hidebysig instance class [mscorlib/*23000001*/]System.Reflection.MethodInfo/*01000010*/ 
          GetMethod(class MvcContrib.FluentHtml.Elements.IMemberElement/*0200001C*/ element,
                    string methodName) cil managed
  // SIG: 20 02 12 41 12 70 0E
  {
    // Method begins at RVA 0x28ff
    // Code size       23 (0x17)
    .maxstack  8
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 6F   | (06)000054       */ callvirt   instance class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ MvcContrib.FluentHtml.Elements.IMemberElement/*0200001C*/::get_ForMember() /* 06000054 */
    IL_0006:  /* 2C   | 0D               */ brfalse.s  IL_0015

    IL_0008:  /* 03   |                  */ ldarg.1
    IL_0009:  /* 6F   | (0A)000016       */ callvirt   instance class [mscorlib/*23000001*/]System.Type/*01000003*/ [mscorlib/*23000001*/]System.Object/*01000001*/::GetType() /* 0A000016 */
    IL_000e:  /* 04   |                  */ ldarg.2
    IL_000f:  /* 6F   | (0A)000057       */ callvirt   instance class [mscorlib/*23000001*/]System.Reflection.MethodInfo/*01000010*/ [mscorlib/*23000001*/]System.Type/*01000003*/::GetMethod(string) /* 0A000057 */
    IL_0014:  /* 2A   |                  */ ret

    IL_0015:  /* 14   |                  */ ldnull
    IL_0016:  /* 2A   |                  */ ret
  } // end of method MemberBehaviorHelper`1::GetMethod

  .method /*0600003C*/ public hidebysig instance void 
          InvokeMethod(class [mscorlib/*23000001*/]System.Reflection.MethodInfo/*01000010*/ 'method',
                       class MvcContrib.FluentHtml.Elements.IMemberElement/*0200001C*/ element,
                       object[] parameters) cil managed
  // SIG: 20 03 01 12 41 12 70 1D 1C
  {
    .param [3]/*08000035*/ 
    .custom /*0C000032:0A000002*/ instance void [mscorlib/*23000001*/]System.ParamArrayAttribute/*01000022*/::.ctor() /* 0A000002 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2917
    // Code size       10 (0xa)
    .maxstack  8
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 04   |                  */ ldarg.2
    IL_0002:  /* 05   |                  */ ldarg.3
    IL_0003:  /* 6F   | (0A)000058       */ callvirt   instance object [mscorlib/*23000001*/]System.Reflection.MethodBase/*0100004F*/::Invoke(object,
                                                                                                                                              object[]) /* 0A000058 */
    IL_0008:  /* 26   |                  */ pop
    IL_0009:  /* 2A   |                  */ ret
  } // end of method MemberBehaviorHelper`1::InvokeMethod

  .method /*0600003D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2922
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000012       */ call       instance void [mscorlib/*23000001*/]System.Object/*01000001*/::.ctor() /* 0A000012 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method MemberBehaviorHelper`1::.ctor

} // end of class MvcContrib.FluentHtml.Behaviors.MemberBehaviorHelper`1

.class /*02000018*/ public auto ansi beforefieldinit MvcContrib.FluentHtml.Behaviors.ValidationBehavior
       extends MvcContrib.FluentHtml.Behaviors.ThreadSafeBehavior/*02000004*/
{
  .field /*04000010*/ private static literal string defaultValidationCssClass = "input-validation-error"
  .field /*04000011*/ private initonly class [System.Core/*23000002*/]System.Func`1/*0100000D*/<class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelStateDictionary/*01000011*/> modelStateDictionaryFunc
  .field /*04000012*/ private initonly string validationErrorCssClass
  .method /*0600003E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Core/*23000002*/]System.Func`1/*0100000D*/<class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelStateDictionary/*01000011*/> modelStateDictionaryFunc) cil managed
  // SIG: 20 01 01 15 12 35 01 12 45
  {
    // Method begins at RVA 0x292a
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 72   | (70)000063       */ ldstr      "input-validation-error" /* 70000063 */
    IL_0007:  /* 28   | (06)00003F       */ call       instance void MvcContrib.FluentHtml.Behaviors.ValidationBehavior/*02000018*/::.ctor(class [System.Core/*23000002*/]System.Func`1/*0100000D*/<class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelStateDictionary/*01000011*/>,
                                                                                                                                           string) /* 0600003F */
    IL_000c:  /* 2A   |                  */ ret
  } // end of method ValidationBehavior::.ctor

  .method /*0600003F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Core/*23000002*/]System.Func`1/*0100000D*/<class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelStateDictionary/*01000011*/> modelStateDictionaryFunc,
                               string validationErrorCssClass) cil managed
  // SIG: 20 02 01 15 12 35 01 12 45 0E
  {
    // Method begins at RVA 0x2938
    // Code size       21 (0x15)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000004       */ call       instance void MvcContrib.FluentHtml.Behaviors.ThreadSafeBehavior/*02000004*/::.ctor() /* 06000004 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000011       */ stfld      class [System.Core/*23000002*/]System.Func`1/*0100000D*/<class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelStateDictionary/*01000011*/> MvcContrib.FluentHtml.Behaviors.ValidationBehavior/*02000018*/::modelStateDictionaryFunc /* 04000011 */
    IL_000d:  /* 02   |                  */ ldarg.0
    IL_000e:  /* 04   |                  */ ldarg.2
    IL_000f:  /* 7D   | (04)000012       */ stfld      string MvcContrib.FluentHtml.Behaviors.ValidationBehavior/*02000018*/::validationErrorCssClass /* 04000012 */
    IL_0014:  /* 2A   |                  */ ret
  } // end of method ValidationBehavior::.ctor

  .method /*06000040*/ family hidebysig virtual 
          instance void  DoExecute(class MvcContrib.FluentHtml.Elements.IElement/*0200001B*/ element) cil managed
  // SIG: 20 01 01 12 6C
  {
    // Method begins at RVA 0x2950
    // Code size       89 (0x59)
    .maxstack  3
    .locals /*1100000E*/ init (string V_0,
             class MvcContrib.FluentHtml.Behaviors.ISupportsModelState/*02000016*/ V_1,
             class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelState/*0100000F*/ V_2)
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 72   | (70)000091       */ ldstr      "name" /* 70000091 */
    IL_0006:  /* 6F   | (06)000048       */ callvirt   instance string MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::GetAttr(string) /* 06000048 */
    IL_000b:  /* 0A   |                  */ stloc.0
    IL_000c:  /* 03   |                  */ ldarg.1
    IL_000d:  /* 75   | (02)000016       */ isinst     MvcContrib.FluentHtml.Behaviors.ISupportsModelState/*02000016*/
    IL_0012:  /* 0B   |                  */ stloc.1
    IL_0013:  /* 06   |                  */ ldloc.0
    IL_0014:  /* 2C   | 03               */ brfalse.s  IL_0019

    IL_0016:  /* 07   |                  */ ldloc.1
    IL_0017:  /* 2D   | 01               */ brtrue.s   IL_001a

    IL_0019:  /* 2A   |                  */ ret

    IL_001a:  /* 02   |                  */ ldarg.0
    IL_001b:  /* 7B   | (04)000011       */ ldfld      class [System.Core/*23000002*/]System.Func`1/*0100000D*/<class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelStateDictionary/*01000011*/> MvcContrib.FluentHtml.Behaviors.ValidationBehavior/*02000018*/::modelStateDictionaryFunc /* 04000011 */
    IL_0020:  /* 6F   | (0A)000059       */ callvirt   instance !0 class [System.Core/*23000002*/]System.Func`1/*0100000D*/<class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelStateDictionary/*01000011*/>/*1B000042*/::Invoke() /* 0A000059 */
    IL_0025:  /* 06   |                  */ ldloc.0
    IL_0026:  /* 12   | 02               */ ldloca.s   V_2
    IL_0028:  /* 6F   | (0A)00005A       */ callvirt   instance bool [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelStateDictionary/*01000011*/::TryGetValue(string,
                                                                                                                                                              class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelState/*0100000F*/&) /* 0A00005A */
    IL_002d:  /* 2C   | 29               */ brfalse.s  IL_0058

    IL_002f:  /* 02   |                  */ ldarg.0
    IL_0030:  /* 08   |                  */ ldloc.2
    IL_0031:  /* 28   | (06)000041       */ call       instance bool MvcContrib.FluentHtml.Behaviors.ValidationBehavior/*02000018*/::HasErrors(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelState/*0100000F*/) /* 06000041 */
    IL_0036:  /* 2C   | 11               */ brfalse.s  IL_0049

    IL_0038:  /* 03   |                  */ ldarg.1
    IL_0039:  /* 6F   | (06)000045       */ callvirt   instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::get_Builder() /* 06000045 */
    IL_003e:  /* 02   |                  */ ldarg.0
    IL_003f:  /* 7B   | (04)000012       */ ldfld      string MvcContrib.FluentHtml.Behaviors.ValidationBehavior/*02000018*/::validationErrorCssClass /* 04000012 */
    IL_0044:  /* 6F   | (0A)00005B       */ callvirt   instance void [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/::AddCssClass(string) /* 0A00005B */
    IL_0049:  /* 08   |                  */ ldloc.2
    IL_004a:  /* 6F   | (0A)00005C       */ callvirt   instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ValueProviderResult/*01000050*/ [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelState/*0100000F*/::get_Value() /* 0A00005C */
    IL_004f:  /* 2C   | 07               */ brfalse.s  IL_0058

    IL_0051:  /* 07   |                  */ ldloc.1
    IL_0052:  /* 08   |                  */ ldloc.2
    IL_0053:  /* 6F   | (06)000038       */ callvirt   instance void MvcContrib.FluentHtml.Behaviors.ISupportsModelState/*02000016*/::ApplyModelState(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelState/*0100000F*/) /* 06000038 */
    IL_0058:  /* 2A   |                  */ ret
  } // end of method ValidationBehavior::DoExecute

  .method /*06000041*/ private hidebysig 
          instance bool  HasErrors(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelState/*0100000F*/ state) cil managed
  // SIG: 20 01 02 12 3D
  {
    // Method begins at RVA 0x29b5
    // Code size       25 (0x19)
    .maxstack  8
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 6F   | (0A)00005D       */ callvirt   instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelErrorCollection/*01000051*/ [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelState/*0100000F*/::get_Errors() /* 0A00005D */
    IL_0006:  /* 2C   | 0F               */ brfalse.s  IL_0017

    IL_0008:  /* 03   |                  */ ldarg.1
    IL_0009:  /* 6F   | (0A)00005D       */ callvirt   instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelErrorCollection/*01000051*/ [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelState/*0100000F*/::get_Errors() /* 0A00005D */
    IL_000e:  /* 6F   | (0A)00005E       */ callvirt   instance int32 class [mscorlib/*23000001*/]System.Collections.ObjectModel.Collection`1/*01000052*/<class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelError/*01000053*/>/*1B000043*/::get_Count() /* 0A00005E */
    IL_0013:  /* 16   |                  */ ldc.i4.0
    IL_0014:  /* FE02 |                  */ cgt
    IL_0016:  /* 2A   |                  */ ret

    IL_0017:  /* 16   |                  */ ldc.i4.0
    IL_0018:  /* 2A   |                  */ ret
  } // end of method ValidationBehavior::HasErrors

} // end of class MvcContrib.FluentHtml.Behaviors.ValidationBehavior

.class /*02000019*/ interface public abstract auto ansi MvcContrib.FluentHtml.Behaviors.IMemberBehavior
       implements class MvcContrib.FluentHtml.Behaviors.IBehavior`1/*02000003*/<class MvcContrib.FluentHtml.Elements.IMemberElement/*0200001C*/>/*1B000004*/,
                  MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/
{
  .custom /*0C00001E:0A000055*/ instance void [mscorlib/*23000001*/]System.ObsoleteAttribute/*0100004E*/::.ctor(string) /* 0A000055 */ = ( 01 00 26 55 73 65 20 49 42 65 68 61 76 69 6F 72   // ..&Use IBehavior
                                                                                                                                           3C 49 4D 65 6D 62 65 72 45 6C 65 6D 65 6E 74 3E   // <IMemberElement>
                                                                                                                                           20 69 6E 73 74 65 61 64 2E 00 00 )                //  instead...
} // end of class MvcContrib.FluentHtml.Behaviors.IMemberBehavior

.class /*0200001A*/ public auto ansi beforefieldinit MvcContrib.FluentHtml.Behaviors.TypeCssClassBehavior
       extends MvcContrib.FluentHtml.Behaviors.ThreadSafeMemberBehavior/*0200000C*/
{
  .field /*04000013*/ private initonly string numberClass
  .field /*04000014*/ private initonly string dateClass
  .method /*06000042*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x29cf
    // Code size       17 (0x11)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 72   | (70)00009B       */ ldstr      "number" /* 7000009B */
    IL_0006:  /* 72   | (70)0000A9       */ ldstr      "date" /* 700000A9 */
    IL_000b:  /* 28   | (06)000043       */ call       instance void MvcContrib.FluentHtml.Behaviors.TypeCssClassBehavior/*0200001A*/::.ctor(string,
                                                                                                                                             string) /* 06000043 */
    IL_0010:  /* 2A   |                  */ ret
  } // end of method TypeCssClassBehavior::.ctor

  .method /*06000043*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string numberClass,
                               string dateClass) cil managed
  // SIG: 20 02 01 0E 0E
  {
    // Method begins at RVA 0x29e1
    // Code size       21 (0x15)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000028       */ call       instance void MvcContrib.FluentHtml.Behaviors.ThreadSafeMemberBehavior/*0200000C*/::.ctor() /* 06000028 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000013       */ stfld      string MvcContrib.FluentHtml.Behaviors.TypeCssClassBehavior/*0200001A*/::numberClass /* 04000013 */
    IL_000d:  /* 02   |                  */ ldarg.0
    IL_000e:  /* 04   |                  */ ldarg.2
    IL_000f:  /* 7D   | (04)000014       */ stfld      string MvcContrib.FluentHtml.Behaviors.TypeCssClassBehavior/*0200001A*/::dateClass /* 04000014 */
    IL_0014:  /* 2A   |                  */ ret
  } // end of method TypeCssClassBehavior::.ctor

  .method /*06000044*/ family hidebysig virtual 
          instance void  DoExecute(class MvcContrib.FluentHtml.Elements.IMemberElement/*0200001C*/ element) cil managed
  // SIG: 20 01 01 12 70
  {
    // Method begins at RVA 0x29f8
    // Code size       90 (0x5a)
    .maxstack  2
    .locals /*1100000C*/ init (class [mscorlib/*23000001*/]System.Type/*01000003*/ V_0)
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 6F   | (06)000054       */ callvirt   instance class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ MvcContrib.FluentHtml.Elements.IMemberElement/*0200001C*/::get_ForMember() /* 06000054 */
    IL_0006:  /* 2D   | 01               */ brtrue.s   IL_0009

    IL_0008:  /* 2A   |                  */ ret

    IL_0009:  /* 03   |                  */ ldarg.1
    IL_000a:  /* 28   | (06)000021       */ call       bool MvcContrib.FluentHtml.Behaviors.BehaviorHelperExtensions/*0200000B*/::IsTextElement(class MvcContrib.FluentHtml.Elements.IElement/*0200001B*/) /* 06000021 */
    IL_000f:  /* 2D   | 01               */ brtrue.s   IL_0012

    IL_0011:  /* 2A   |                  */ ret

    IL_0012:  /* 03   |                  */ ldarg.1
    IL_0013:  /* 6F   | (06)000054       */ callvirt   instance class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ MvcContrib.FluentHtml.Elements.IMemberElement/*0200001C*/::get_ForMember() /* 06000054 */
    IL_0018:  /* 28   | (06)000022       */ call       class [mscorlib/*23000001*/]System.Type/*01000003*/ MvcContrib.FluentHtml.Behaviors.BehaviorHelperExtensions/*0200000B*/::GetPropertyOrFieldType(class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/) /* 06000022 */
    IL_001d:  /* 0A   |                  */ stloc.0
    IL_001e:  /* 06   |                  */ ldloc.0
    IL_001f:  /* 28   | (06)000023       */ call       bool MvcContrib.FluentHtml.Behaviors.BehaviorHelperExtensions/*0200000B*/::IsNumber(class [mscorlib/*23000001*/]System.Type/*01000003*/) /* 06000023 */
    IL_0024:  /* 2C   | 0D               */ brfalse.s  IL_0033

    IL_0026:  /* 03   |                  */ ldarg.1
    IL_0027:  /* 02   |                  */ ldarg.0
    IL_0028:  /* 7B   | (04)000013       */ ldfld      string MvcContrib.FluentHtml.Behaviors.TypeCssClassBehavior/*0200001A*/::numberClass /* 04000013 */
    IL_002d:  /* 6F   | (06)000053       */ callvirt   instance void MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::AddClass(string) /* 06000053 */
    IL_0032:  /* 2A   |                  */ ret

    IL_0033:  /* 06   |                  */ ldloc.0
    IL_0034:  /* D0   | (01)000054       */ ldtoken    [mscorlib/*23000001*/]System.DateTime/*01000054*/
    IL_0039:  /* 28   | (0A)000021       */ call       class [mscorlib/*23000001*/]System.Type/*01000003*/ [mscorlib/*23000001*/]System.Type/*01000003*/::GetTypeFromHandle(valuetype [mscorlib/*23000001*/]System.RuntimeTypeHandle/*01000036*/) /* 0A000021 */
    IL_003e:  /* 2E   | 0D               */ beq.s      IL_004d

    IL_0040:  /* 06   |                  */ ldloc.0
    IL_0041:  /* D0   | (1B)000044       */ ldtoken    valuetype [mscorlib/*23000001*/]System.Nullable`1/*0100004D*/<valuetype [mscorlib/*23000001*/]System.DateTime/*01000054*/>/*1B000044*/
    IL_0046:  /* 28   | (0A)000021       */ call       class [mscorlib/*23000001*/]System.Type/*01000003*/ [mscorlib/*23000001*/]System.Type/*01000003*/::GetTypeFromHandle(valuetype [mscorlib/*23000001*/]System.RuntimeTypeHandle/*01000036*/) /* 0A000021 */
    IL_004b:  /* 33   | 0C               */ bne.un.s   IL_0059

    IL_004d:  /* 03   |                  */ ldarg.1
    IL_004e:  /* 02   |                  */ ldarg.0
    IL_004f:  /* 7B   | (04)000014       */ ldfld      string MvcContrib.FluentHtml.Behaviors.TypeCssClassBehavior/*0200001A*/::dateClass /* 04000014 */
    IL_0054:  /* 6F   | (06)000053       */ callvirt   instance void MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::AddClass(string) /* 06000053 */
    IL_0059:  /* 2A   |                  */ ret
  } // end of method TypeCssClassBehavior::DoExecute

} // end of class MvcContrib.FluentHtml.Behaviors.TypeCssClassBehavior

.class /*0200001B*/ interface public abstract auto ansi MvcContrib.FluentHtml.Elements.IElement
{
  .method /*06000045*/ public hidebysig newslot specialname abstract virtual 
          instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ 
          get_Builder() cil managed
  // SIG: 20 00 12 49
  {
    // Method begins at RVA 0x0
  } // end of method IElement::get_Builder

  .method /*06000046*/ public hidebysig newslot specialname abstract virtual 
          instance valuetype [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagRenderMode/*01000013*/ 
          get_TagRenderMode() cil managed
  // SIG: 20 00 11 4D
  {
    // Method begins at RVA 0x0
  } // end of method IElement::get_TagRenderMode

  .method /*06000047*/ public hidebysig newslot abstract virtual 
          instance void  SetAttr(string name,
                                 object 'value') cil managed
  // SIG: 20 02 01 0E 1C
  {
    // Method begins at RVA 0x0
  } // end of method IElement::SetAttr

  .method /*06000048*/ public hidebysig newslot abstract virtual 
          instance string  GetAttr(string name) cil managed
  // SIG: 20 01 0E 0E
  {
    // Method begins at RVA 0x0
  } // end of method IElement::GetAttr

  .method /*06000049*/ public hidebysig newslot abstract virtual 
          instance void  RemoveAttr(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x0
  } // end of method IElement::RemoveAttr

  .method /*0600004A*/ public hidebysig newslot specialname abstract virtual 
          instance string  get_LabelBeforeText() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x0
  } // end of method IElement::get_LabelBeforeText

  .method /*0600004B*/ public hidebysig newslot specialname abstract virtual 
          instance void  set_LabelBeforeText(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x0
  } // end of method IElement::set_LabelBeforeText

  .method /*0600004C*/ public hidebysig newslot specialname abstract virtual 
          instance string  get_LabelAfterText() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x0
  } // end of method IElement::get_LabelAfterText

  .method /*0600004D*/ public hidebysig newslot specialname abstract virtual 
          instance void  set_LabelAfterText(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x0
  } // end of method IElement::set_LabelAfterText

  .method /*0600004E*/ public hidebysig newslot specialname abstract virtual 
          instance string  get_LabelClass() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x0
  } // end of method IElement::get_LabelClass

  .method /*0600004F*/ public hidebysig newslot specialname abstract virtual 
          instance void  set_LabelClass(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x0
  } // end of method IElement::set_LabelClass

  .method /*06000050*/ public hidebysig newslot specialname abstract virtual 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<object,object> 
          get_Metadata() cil managed
  // SIG: 20 00 15 12 51 02 1C 1C
  {
    // Method begins at RVA 0x0
  } // end of method IElement::get_Metadata

  .method /*06000051*/ public hidebysig newslot abstract virtual 
          instance void  SetAutoLabel() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x0
  } // end of method IElement::SetAutoLabel

  .method /*06000052*/ public hidebysig newslot abstract virtual 
          instance void  SetAutoLabelAfter() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x0
  } // end of method IElement::SetAutoLabelAfter

  .method /*06000053*/ public hidebysig newslot abstract virtual 
          instance void  AddClass(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x0
  } // end of method IElement::AddClass

  .property /*17000007*/ instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/
          Builder()
  {
    .get instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::get_Builder() /* 06000045 */
  } // end of property IElement::Builder
  .property /*17000008*/ instance valuetype [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagRenderMode/*01000013*/
          TagRenderMode()
  {
    .get instance valuetype [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagRenderMode/*01000013*/ MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::get_TagRenderMode() /* 06000046 */
  } // end of property IElement::TagRenderMode
  .property /*17000009*/ instance string LabelBeforeText()
  {
    .get instance string MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::get_LabelBeforeText() /* 0600004A */
    .set instance void MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::set_LabelBeforeText(string) /* 0600004B */
  } // end of property IElement::LabelBeforeText
  .property /*1700000A*/ instance string LabelAfterText()
  {
    .get instance string MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::get_LabelAfterText() /* 0600004C */
    .set instance void MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::set_LabelAfterText(string) /* 0600004D */
  } // end of property IElement::LabelAfterText
  .property /*1700000B*/ instance string LabelClass()
  {
    .get instance string MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::get_LabelClass() /* 0600004E */
    .set instance void MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::set_LabelClass(string) /* 0600004F */
  } // end of property IElement::LabelClass
  .property /*1700000C*/ instance class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<object,object>
          Metadata()
  {
    .get instance class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<object,object> MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::get_Metadata() /* 06000050 */
  } // end of property IElement::Metadata
} // end of class MvcContrib.FluentHtml.Elements.IElement

.class /*0200001C*/ interface public abstract auto ansi MvcContrib.FluentHtml.Elements.IMemberElement
       implements MvcContrib.FluentHtml.Elements.IElement/*0200001B*/
{
  .method /*06000054*/ public hidebysig newslot specialname abstract virtual 
          instance class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ 
          get_ForMember() cil managed
  // SIG: 20 00 12 39
  {
    // Method begins at RVA 0x0
  } // end of method IMemberElement::get_ForMember

  .property /*1700000D*/ instance class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/
          ForMember()
  {
    .get instance class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ MvcContrib.FluentHtml.Elements.IMemberElement/*0200001C*/::get_ForMember() /* 06000054 */
  } // end of property IMemberElement::ForMember
} // end of class MvcContrib.FluentHtml.Elements.IMemberElement

.class /*0200001D*/ public abstract auto ansi beforefieldinit MvcContrib.FluentHtml.Elements.Element`1<(class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/, MvcContrib.FluentHtml.Elements.IElement/*0200001B*/) T>
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
       implements MvcContrib.FluentHtml.Elements.IMemberElement/*0200001C*/,
                  MvcContrib.FluentHtml.Elements.IElement/*0200001B*/
{
  .field /*04000015*/ static family literal string LABEL_FORMAT = "{0}_Label"
  .field /*04000016*/ private bool doNotAutoLabel
  .field /*04000017*/ family initonly class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ builder
  .field /*04000018*/ family class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ forMember
  .field /*04000019*/ family class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> behaviors
  .field /*0400001A*/ family initonly class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<object,object> metadata
  .field /*0400001B*/ private string '<MvcContrib.FluentHtml.Elements.IElement.LabelBeforeText>k__BackingField'
  .custom /*0C00001F:0A000013*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000032*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
  .field /*0400001C*/ private string '<MvcContrib.FluentHtml.Elements.IElement.LabelAfterText>k__BackingField'
  .custom /*0C000020:0A000013*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000032*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
  .field /*0400001D*/ private string '<MvcContrib.FluentHtml.Elements.IElement.LabelClass>k__BackingField'
  .custom /*0C000022:0A000013*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000032*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
  .field /*0400001E*/ private static class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/,bool> 'CS$<>9__CachedAnonymousMethodDelegate1'
  .custom /*0C000024:0A000013*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000032*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
  .field /*0400001F*/ private static class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/,bool> 'CS$<>9__CachedAnonymousMethodDelegate5'
  .custom /*0C000026:0A000013*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000032*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
  .field /*04000020*/ private static class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/,bool> 'CS$<>9__CachedAnonymousMethodDelegate6'
  .custom /*0C000028:0A000013*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000032*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
  .field /*04000021*/ private static class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/,int32> 'CS$<>9__CachedAnonymousMethodDelegate7'
  .custom /*0C00002A:0A000013*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000032*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
  .method /*06000055*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(string tag,
                               class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ forMember,
                               class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> behaviors) cil managed
  // SIG: 20 03 01 0E 12 39 15 12 11 01 12 08
  {
    // Method begins at RVA 0x2a5e
    // Code size       22 (0x16)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 28   | (0A)00005F       */ call       instance void class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::.ctor(string) /* 0A00005F */
    IL_0007:  /* 02   |                  */ ldarg.0
    IL_0008:  /* 04   |                  */ ldarg.2
    IL_0009:  /* 7D   | (0A)000060       */ stfld      class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::forMember /* 0A000060 */
    IL_000e:  /* 02   |                  */ ldarg.0
    IL_000f:  /* 05   |                  */ ldarg.3
    IL_0010:  /* 7D   | (0A)000061       */ stfld      class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::behaviors /* 0A000061 */
    IL_0015:  /* 2A   |                  */ ret
  } // end of method Element`1::.ctor

  .method /*06000056*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(string tag) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2a75
    // Code size       30 (0x1e)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 73   | (0A)000062       */ newobj     instance void class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000002*/<object,object>/*1B000045*/::.ctor() /* 0A000062 */
    IL_0006:  /* 7D   | (0A)000063       */ stfld      class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<object,object> class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::metadata /* 0A000063 */
    IL_000b:  /* 02   |                  */ ldarg.0
    IL_000c:  /* 28   | (0A)000012       */ call       instance void [mscorlib/*23000001*/]System.Object/*01000001*/::.ctor() /* 0A000012 */
    IL_0011:  /* 02   |                  */ ldarg.0
    IL_0012:  /* 03   |                  */ ldarg.1
    IL_0013:  /* 73   | (0A)000064       */ newobj     instance void [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/::.ctor(string) /* 0A000064 */
    IL_0018:  /* 7D   | (0A)000065       */ stfld      class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::builder /* 0A000065 */
    IL_001d:  /* 2A   |                  */ ret
  } // end of method Element`1::.ctor

  .method /*06000057*/ private hidebysig newslot specialname virtual final 
          instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ 
          MvcContrib.FluentHtml.Elements.IElement.get_Builder() cil managed
  // SIG: 20 00 12 49
  {
    .override MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::get_Builder /*0200001B::06000045*/ 
    // Method begins at RVA 0x2a94
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (0A)000065       */ ldfld      class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::builder /* 0A000065 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method Element`1::MvcContrib.FluentHtml.Elements.IElement.get_Builder

  .method /*06000058*/ public hidebysig newslot virtual 
          instance !T  Id(string 'value') cil managed
  // SIG: 20 01 13 00 0E
  {
    // Method begins at RVA 0x2a9c
    // Code size       25 (0x19)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (0A)000065       */ ldfld      class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::builder /* 0A000065 */
    IL_0006:  /* 72   | (70)0000B3       */ ldstr      "id" /* 700000B3 */
    IL_000b:  /* 03   |                  */ ldarg.1
    IL_000c:  /* 17   |                  */ ldc.i4.1
    IL_000d:  /* 6F   | (0A)000066       */ callvirt   instance void [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/::MergeAttribute(string,
                                                                                                                                                       string,
                                                                                                                                                       bool) /* 0A000066 */
    IL_0012:  /* 02   |                  */ ldarg.0
    IL_0013:  /* A5   | (1B)000040       */ unbox.any  !T/*1B000040*/
    IL_0018:  /* 2A   |                  */ ret
  } // end of method Element`1::Id

  .method /*06000059*/ public hidebysig newslot virtual 
          instance !T  Class(string classToAdd) cil managed
  // SIG: 20 01 13 00 0E
  {
    // Method begins at RVA 0x2ab6
    // Code size       19 (0x13)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (0A)000065       */ ldfld      class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::builder /* 0A000065 */
    IL_0006:  /* 03   |                  */ ldarg.1
    IL_0007:  /* 6F   | (0A)00005B       */ callvirt   instance void [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/::AddCssClass(string) /* 0A00005B */
    IL_000c:  /* 02   |                  */ ldarg.0
    IL_000d:  /* A5   | (1B)000040       */ unbox.any  !T/*1B000040*/
    IL_0012:  /* 2A   |                  */ ret
  } // end of method Element`1::Class

  .method /*0600005A*/ public hidebysig newslot virtual 
          instance !T  Title(string 'value') cil managed
  // SIG: 20 01 13 00 0E
  {
    // Method begins at RVA 0x2aca
    // Code size       25 (0x19)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (0A)000065       */ ldfld      class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::builder /* 0A000065 */
    IL_0006:  /* 72   | (70)0000B9       */ ldstr      "title" /* 700000B9 */
    IL_000b:  /* 03   |                  */ ldarg.1
    IL_000c:  /* 17   |                  */ ldc.i4.1
    IL_000d:  /* 6F   | (0A)000066       */ callvirt   instance void [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/::MergeAttribute(string,
                                                                                                                                                       string,
                                                                                                                                                       bool) /* 0A000066 */
    IL_0012:  /* 02   |                  */ ldarg.0
    IL_0013:  /* A5   | (1B)000040       */ unbox.any  !T/*1B000040*/
    IL_0018:  /* 2A   |                  */ ret
  } // end of method Element`1::Title

  .method /*0600005B*/ public hidebysig newslot virtual 
          instance !T  Styles(class [System.Core/*23000002*/]System.Func`2/*0100000B*/<string,string>[] values) cil managed
  // SIG: 20 01 13 00 1D 15 12 2D 02 0E 0E
  {
    .param [1]/*0800004D*/ 
    .custom /*0C000037:0A000002*/ instance void [mscorlib/*23000001*/]System.ParamArrayAttribute/*01000022*/::.ctor() /* 0A000002 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2ae4
    // Code size       101 (0x65)
    .maxstack  5
    .locals /*1100000F*/ init (class [mscorlib/*23000001*/]System.Text.StringBuilder/*01000055*/ V_0,
             class [System.Core/*23000002*/]System.Func`2/*0100000B*/<string,string> V_1,
             class [System.Core/*23000002*/]System.Func`2/*0100000B*/<string,string>[] V_2,
             int32 V_3)
    IL_0000:  /* 73   | (0A)000067       */ newobj     instance void [mscorlib/*23000001*/]System.Text.StringBuilder/*01000055*/::.ctor() /* 0A000067 */
    IL_0005:  /* 0A   |                  */ stloc.0
    IL_0006:  /* 03   |                  */ ldarg.1
    IL_0007:  /* 0C   |                  */ stloc.2
    IL_0008:  /* 16   |                  */ ldc.i4.0
    IL_0009:  /* 0D   |                  */ stloc.3
    IL_000a:  /* 2B   | 36               */ br.s       IL_0042

    IL_000c:  /* 08   |                  */ ldloc.2
    IL_000d:  /* 09   |                  */ ldloc.3
    IL_000e:  /* 9A   |                  */ ldelem.ref
    IL_000f:  /* 0B   |                  */ stloc.1
    IL_0010:  /* 06   |                  */ ldloc.0
    IL_0011:  /* 72   | (70)0000C5       */ ldstr      "{0}:{1};" /* 700000C5 */
    IL_0016:  /* 07   |                  */ ldloc.1
    IL_0017:  /* 6F   | (0A)000068       */ callvirt   instance class [mscorlib/*23000001*/]System.Reflection.MethodInfo/*01000010*/ [mscorlib/*23000001*/]System.Delegate/*0100003D*/::get_Method() /* 0A000068 */
    IL_001c:  /* 6F   | (0A)000069       */ callvirt   instance class [mscorlib/*23000001*/]System.Reflection.ParameterInfo/*01000056*/[] [mscorlib/*23000001*/]System.Reflection.MethodBase/*0100004F*/::GetParameters() /* 0A000069 */
    IL_0021:  /* 16   |                  */ ldc.i4.0
    IL_0022:  /* 9A   |                  */ ldelem.ref
    IL_0023:  /* 6F   | (0A)00006A       */ callvirt   instance string [mscorlib/*23000001*/]System.Reflection.ParameterInfo/*01000056*/::get_Name() /* 0A00006A */
    IL_0028:  /* 1F   | 5F               */ ldc.i4.s   95
    IL_002a:  /* 1F   | 2D               */ ldc.i4.s   45
    IL_002c:  /* 6F   | (0A)00006B       */ callvirt   instance string [mscorlib/*23000001*/]System.String/*0100003F*/::Replace(char,
                                                                                                                                char) /* 0A00006B */
    IL_0031:  /* 07   |                  */ ldloc.1
    IL_0032:  /* 14   |                  */ ldnull
    IL_0033:  /* 6F   | (0A)00006C       */ callvirt   instance !1 class [System.Core/*23000002*/]System.Func`2/*0100000B*/<string,string>/*1B000046*/::Invoke(!0) /* 0A00006C */
    IL_0038:  /* 6F   | (0A)00006D       */ callvirt   instance class [mscorlib/*23000001*/]System.Text.StringBuilder/*01000055*/ [mscorlib/*23000001*/]System.Text.StringBuilder/*01000055*/::AppendFormat(string,
                                                                                                                                                                                                            object,
                                                                                                                                                                                                            object) /* 0A00006D */
    IL_003d:  /* 26   |                  */ pop
    IL_003e:  /* 09   |                  */ ldloc.3
    IL_003f:  /* 17   |                  */ ldc.i4.1
    IL_0040:  /* 58   |                  */ add
    IL_0041:  /* 0D   |                  */ stloc.3
    IL_0042:  /* 09   |                  */ ldloc.3
    IL_0043:  /* 08   |                  */ ldloc.2
    IL_0044:  /* 8E   |                  */ ldlen
    IL_0045:  /* 69   |                  */ conv.i4
    IL_0046:  /* 32   | C4               */ blt.s      IL_000c

    IL_0048:  /* 02   |                  */ ldarg.0
    IL_0049:  /* 7B   | (0A)000065       */ ldfld      class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::builder /* 0A000065 */
    IL_004e:  /* 72   | (70)0000D7       */ ldstr      "style" /* 700000D7 */
    IL_0053:  /* 06   |                  */ ldloc.0
    IL_0054:  /* 6F   | (0A)00006E       */ callvirt   instance string [mscorlib/*23000001*/]System.Object/*01000001*/::ToString() /* 0A00006E */
    IL_0059:  /* 6F   | (0A)00006F       */ callvirt   instance void [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/::MergeAttribute(string,
                                                                                                                                                       string) /* 0A00006F */
    IL_005e:  /* 02   |                  */ ldarg.0
    IL_005f:  /* A5   | (1B)000040       */ unbox.any  !T/*1B000040*/
    IL_0064:  /* 2A   |                  */ ret
  } // end of method Element`1::Styles

  .method /*0600005C*/ public hidebysig newslot virtual 
          instance !T  OnClick(string 'value') cil managed
  // SIG: 20 01 13 00 0E
  {
    // Method begins at RVA 0x2b55
    // Code size       25 (0x19)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (0A)000065       */ ldfld      class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::builder /* 0A000065 */
    IL_0006:  /* 72   | (70)0000E3       */ ldstr      "onclick" /* 700000E3 */
    IL_000b:  /* 03   |                  */ ldarg.1
    IL_000c:  /* 17   |                  */ ldc.i4.1
    IL_000d:  /* 6F   | (0A)000066       */ callvirt   instance void [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/::MergeAttribute(string,
                                                                                                                                                       string,
                                                                                                                                                       bool) /* 0A000066 */
    IL_0012:  /* 02   |                  */ ldarg.0
    IL_0013:  /* A5   | (1B)000040       */ unbox.any  !T/*1B000040*/
    IL_0018:  /* 2A   |                  */ ret
  } // end of method Element`1::OnClick

  .method /*0600005D*/ public hidebysig newslot virtual 
          instance !T  Attr(string name,
                            object 'value') cil managed
  // SIG: 20 02 13 00 0E 1C
  {
    // Method begins at RVA 0x2b6f
    // Code size       15 (0xf)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 04   |                  */ ldarg.2
    IL_0003:  /* 6F   | (06)000047       */ callvirt   instance void MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::SetAttr(string,
                                                                                                                                  object) /* 06000047 */
    IL_0008:  /* 02   |                  */ ldarg.0
    IL_0009:  /* A5   | (1B)000040       */ unbox.any  !T/*1B000040*/
    IL_000e:  /* 2A   |                  */ ret
  } // end of method Element`1::Attr

  .method /*0600005E*/ public hidebysig newslot virtual 
          instance !T  Label(string 'value',
                             string 'class') cil managed
  // SIG: 20 02 13 00 0E 0E
  {
    // Method begins at RVA 0x2b7f
    // Code size       21 (0x15)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 6F   | (06)00004B       */ callvirt   instance void MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::set_LabelBeforeText(string) /* 0600004B */
    IL_0007:  /* 02   |                  */ ldarg.0
    IL_0008:  /* 04   |                  */ ldarg.2
    IL_0009:  /* 6F   | (06)00004F       */ callvirt   instance void MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::set_LabelClass(string) /* 0600004F */
    IL_000e:  /* 02   |                  */ ldarg.0
    IL_000f:  /* A5   | (1B)000040       */ unbox.any  !T/*1B000040*/
    IL_0014:  /* 2A   |                  */ ret
  } // end of method Element`1::Label

  .method /*0600005F*/ public hidebysig newslot virtual 
          instance !T  Label(string 'value') cil managed
  // SIG: 20 01 13 00 0E
  {
    // Method begins at RVA 0x2b95
    // Code size       14 (0xe)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 6F   | (06)00004B       */ callvirt   instance void MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::set_LabelBeforeText(string) /* 0600004B */
    IL_0007:  /* 02   |                  */ ldarg.0
    IL_0008:  /* A5   | (1B)000040       */ unbox.any  !T/*1B000040*/
    IL_000d:  /* 2A   |                  */ ret
  } // end of method Element`1::Label

  .method /*06000060*/ public hidebysig newslot virtual 
          instance !T  LabelAfter(string 'value',
                                  string 'class') cil managed
  // SIG: 20 02 13 00 0E 0E
  {
    // Method begins at RVA 0x2ba4
    // Code size       21 (0x15)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 6F   | (06)00004D       */ callvirt   instance void MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::set_LabelAfterText(string) /* 0600004D */
    IL_0007:  /* 02   |                  */ ldarg.0
    IL_0008:  /* 04   |                  */ ldarg.2
    IL_0009:  /* 6F   | (06)00004F       */ callvirt   instance void MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::set_LabelClass(string) /* 0600004F */
    IL_000e:  /* 02   |                  */ ldarg.0
    IL_000f:  /* A5   | (1B)000040       */ unbox.any  !T/*1B000040*/
    IL_0014:  /* 2A   |                  */ ret
  } // end of method Element`1::LabelAfter

  .method /*06000061*/ public hidebysig newslot virtual 
          instance !T  LabelAfter(string 'value') cil managed
  // SIG: 20 01 13 00 0E
  {
    // Method begins at RVA 0x2bba
    // Code size       14 (0xe)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 6F   | (06)00004D       */ callvirt   instance void MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::set_LabelAfterText(string) /* 0600004D */
    IL_0007:  /* 02   |                  */ ldarg.0
    IL_0008:  /* A5   | (1B)000040       */ unbox.any  !T/*1B000040*/
    IL_000d:  /* 2A   |                  */ ret
  } // end of method Element`1::LabelAfter

  .method /*06000062*/ public hidebysig newslot virtual 
          instance !T  AutoLabel() cil managed
  // SIG: 20 00 13 00
  {
    // Method begins at RVA 0x2bc9
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 6F   | (06)000051       */ callvirt   instance void MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::SetAutoLabel() /* 06000051 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* A5   | (1B)000040       */ unbox.any  !T/*1B000040*/
    IL_000c:  /* 2A   |                  */ ret
  } // end of method Element`1::AutoLabel

  .method /*06000063*/ public hidebysig newslot virtual 
          instance !T  AutoLabelAfter() cil managed
  // SIG: 20 00 13 00
  {
    // Method begins at RVA 0x2bd7
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 6F   | (06)000052       */ callvirt   instance void MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::SetAutoLabelAfter() /* 06000052 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* A5   | (1B)000040       */ unbox.any  !T/*1B000040*/
    IL_000c:  /* 2A   |                  */ ret
  } // end of method Element`1::AutoLabelAfter

  .method /*06000064*/ public hidebysig newslot virtual 
          instance !T  DoNotAutoLabel() cil managed
  // SIG: 20 00 13 00
  {
    // Method begins at RVA 0x2be5
    // Code size       14 (0xe)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 17   |                  */ ldc.i4.1
    IL_0002:  /* 7D   | (0A)000070       */ stfld      bool class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::doNotAutoLabel /* 0A000070 */
    IL_0007:  /* 02   |                  */ ldarg.0
    IL_0008:  /* A5   | (1B)000040       */ unbox.any  !T/*1B000040*/
    IL_000d:  /* 2A   |                  */ ret
  } // end of method Element`1::DoNotAutoLabel

  .method /*06000065*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2bf4
    // Code size       70 (0x46)
    .maxstack  3
    .locals /*11000010*/ init (string V_0)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000071       */ call       instance void class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::ApplyBehaviors() /* 0A000071 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 6F   | (0A)000072       */ callvirt   instance void class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::PreRender() /* 0A000072 */
    IL_000c:  /* 02   |                  */ ldarg.0
    IL_000d:  /* 02   |                  */ ldarg.0
    IL_000e:  /* 6F   | (06)00004A       */ callvirt   instance string MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::get_LabelBeforeText() /* 0600004A */
    IL_0013:  /* 6F   | (0A)000073       */ callvirt   instance string class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::RenderLabel(string) /* 0A000073 */
    IL_0018:  /* 0A   |                  */ stloc.0
    IL_0019:  /* 06   |                  */ ldloc.0
    IL_001a:  /* 02   |                  */ ldarg.0
    IL_001b:  /* 7B   | (0A)000065       */ ldfld      class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::builder /* 0A000065 */
    IL_0020:  /* 02   |                  */ ldarg.0
    IL_0021:  /* 6F   | (06)000046       */ callvirt   instance valuetype [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagRenderMode/*01000013*/ MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::get_TagRenderMode() /* 06000046 */
    IL_0026:  /* 6F   | (0A)000074       */ callvirt   instance string [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/::ToString(valuetype [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagRenderMode/*01000013*/) /* 0A000074 */
    IL_002b:  /* 28   | (0A)000075       */ call       string [mscorlib/*23000001*/]System.String/*0100003F*/::Concat(string,
                                                                                                                      string) /* 0A000075 */
    IL_0030:  /* 0A   |                  */ stloc.0
    IL_0031:  /* 06   |                  */ ldloc.0
    IL_0032:  /* 02   |                  */ ldarg.0
    IL_0033:  /* 02   |                  */ ldarg.0
    IL_0034:  /* 6F   | (06)00004C       */ callvirt   instance string MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::get_LabelAfterText() /* 0600004C */
    IL_0039:  /* 6F   | (0A)000073       */ callvirt   instance string class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::RenderLabel(string) /* 0A000073 */
    IL_003e:  /* 28   | (0A)000075       */ call       string [mscorlib/*23000001*/]System.String/*0100003F*/::Concat(string,
                                                                                                                      string) /* 0A000075 */
    IL_0043:  /* 0A   |                  */ stloc.0
    IL_0044:  /* 06   |                  */ ldloc.0
    IL_0045:  /* 2A   |                  */ ret
  } // end of method Element`1::ToString

  .method /*06000066*/ private hidebysig newslot virtual final 
          instance void  MvcContrib.FluentHtml.Elements.IElement.RemoveAttr(string name) cil managed
  // SIG: 20 01 01 0E
  {
    .override MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::RemoveAttr /*0200001B::06000049*/ 
    // Method begins at RVA 0x2c46
    // Code size       19 (0x13)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (0A)000065       */ ldfld      class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::builder /* 0A000065 */
    IL_0006:  /* 6F   | (0A)000076       */ callvirt   instance class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<string,string> [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/::get_Attributes() /* 0A000076 */
    IL_000b:  /* 03   |                  */ ldarg.1
    IL_000c:  /* 6F   | (0A)000077       */ callvirt   instance bool class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<string,string>/*1B000047*/::Remove(!0) /* 0A000077 */
    IL_0011:  /* 26   |                  */ pop
    IL_0012:  /* 2A   |                  */ ret
  } // end of method Element`1::MvcContrib.FluentHtml.Elements.IElement.RemoveAttr

  .method /*06000067*/ private hidebysig newslot virtual final 
          instance void  MvcContrib.FluentHtml.Elements.IElement.SetAttr(string name,
                                                                         object 'value') cil managed
  // SIG: 20 02 01 0E 1C
  {
    .override MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::SetAttr /*0200001B::06000047*/ 
    // Method begins at RVA 0x2c5c
    // Code size       28 (0x1c)
    .maxstack  4
    .locals /*11000010*/ init (string V_0)
    IL_0000:  /* 04   |                  */ ldarg.2
    IL_0001:  /* 2C   | 08               */ brfalse.s  IL_000b

    IL_0003:  /* 04   |                  */ ldarg.2
    IL_0004:  /* 6F   | (0A)00006E       */ callvirt   instance string [mscorlib/*23000001*/]System.Object/*01000001*/::ToString() /* 0A00006E */
    IL_0009:  /* 2B   | 01               */ br.s       IL_000c

    IL_000b:  /* 14   |                  */ ldnull
    IL_000c:  /* 0A   |                  */ stloc.0
    IL_000d:  /* 02   |                  */ ldarg.0
    IL_000e:  /* 7B   | (0A)000065       */ ldfld      class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::builder /* 0A000065 */
    IL_0013:  /* 03   |                  */ ldarg.1
    IL_0014:  /* 06   |                  */ ldloc.0
    IL_0015:  /* 17   |                  */ ldc.i4.1
    IL_0016:  /* 6F   | (0A)000066       */ callvirt   instance void [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/::MergeAttribute(string,
                                                                                                                                                       string,
                                                                                                                                                       bool) /* 0A000066 */
    IL_001b:  /* 2A   |                  */ ret
  } // end of method Element`1::MvcContrib.FluentHtml.Elements.IElement.SetAttr

  .method /*06000068*/ private hidebysig newslot virtual final 
          instance string  MvcContrib.FluentHtml.Elements.IElement.GetAttr(string name) cil managed
  // SIG: 20 01 0E 0E
  {
    .override MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::GetAttr /*0200001B::06000048*/ 
    // Method begins at RVA 0x2c84
    // Code size       22 (0x16)
    .maxstack  3
    .locals /*11000010*/ init (string V_0)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (0A)000065       */ ldfld      class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::builder /* 0A000065 */
    IL_0006:  /* 6F   | (0A)000076       */ callvirt   instance class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<string,string> [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/::get_Attributes() /* 0A000076 */
    IL_000b:  /* 03   |                  */ ldarg.1
    IL_000c:  /* 12   | 00               */ ldloca.s   V_0
    IL_000e:  /* 6F   | (0A)000078       */ callvirt   instance bool class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<string,string>/*1B000047*/::TryGetValue(!0,
                                                                                                                                                                                              !1&) /* 0A000078 */
    IL_0013:  /* 26   |                  */ pop
    IL_0014:  /* 06   |                  */ ldloc.0
    IL_0015:  /* 2A   |                  */ ret
  } // end of method Element`1::MvcContrib.FluentHtml.Elements.IElement.GetAttr

  .method /*06000069*/ private hidebysig newslot specialname virtual final 
          instance string  MvcContrib.FluentHtml.Elements.IElement.get_LabelBeforeText() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000041:0A000013*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000032*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    .override MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::get_LabelBeforeText /*0200001B::0600004A*/ 
    // Method begins at RVA 0x2ca6
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (0A)000079       */ ldfld      string class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::'<MvcContrib.FluentHtml.Elements.IElement.LabelBeforeText>k__BackingField' /* 0A000079 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method Element`1::MvcContrib.FluentHtml.Elements.IElement.get_LabelBeforeText

  .method /*0600006A*/ private hidebysig newslot specialname virtual final 
          instance void  MvcContrib.FluentHtml.Elements.IElement.set_LabelBeforeText(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000042:0A000013*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000032*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    .override MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::set_LabelBeforeText /*0200001B::0600004B*/ 
    // Method begins at RVA 0x2cae
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (0A)000079       */ stfld      string class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::'<MvcContrib.FluentHtml.Elements.IElement.LabelBeforeText>k__BackingField' /* 0A000079 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method Element`1::MvcContrib.FluentHtml.Elements.IElement.set_LabelBeforeText

  .method /*0600006B*/ private hidebysig newslot specialname virtual final 
          instance string  MvcContrib.FluentHtml.Elements.IElement.get_LabelAfterText() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000043:0A000013*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000032*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    .override MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::get_LabelAfterText /*0200001B::0600004C*/ 
    // Method begins at RVA 0x2cb7
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (0A)00007A       */ ldfld      string class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::'<MvcContrib.FluentHtml.Elements.IElement.LabelAfterText>k__BackingField' /* 0A00007A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method Element`1::MvcContrib.FluentHtml.Elements.IElement.get_LabelAfterText

  .method /*0600006C*/ private hidebysig newslot specialname virtual final 
          instance void  MvcContrib.FluentHtml.Elements.IElement.set_LabelAfterText(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000044:0A000013*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000032*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    .override MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::set_LabelAfterText /*0200001B::0600004D*/ 
    // Method begins at RVA 0x2cbf
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (0A)00007A       */ stfld      string class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::'<MvcContrib.FluentHtml.Elements.IElement.LabelAfterText>k__BackingField' /* 0A00007A */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method Element`1::MvcContrib.FluentHtml.Elements.IElement.set_LabelAfterText

  .method /*0600006D*/ private hidebysig newslot specialname virtual final 
          instance string  MvcContrib.FluentHtml.Elements.IElement.get_LabelClass() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000045:0A000013*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000032*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    .override MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::get_LabelClass /*0200001B::0600004E*/ 
    // Method begins at RVA 0x2cc8
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (0A)00007B       */ ldfld      string class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::'<MvcContrib.FluentHtml.Elements.IElement.LabelClass>k__BackingField' /* 0A00007B */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method Element`1::MvcContrib.FluentHtml.Elements.IElement.get_LabelClass

  .method /*0600006E*/ private hidebysig newslot specialname virtual final 
          instance void  MvcContrib.FluentHtml.Elements.IElement.set_LabelClass(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000046:0A000013*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000032*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    .override MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::set_LabelClass /*0200001B::0600004F*/ 
    // Method begins at RVA 0x2cd0
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (0A)00007B       */ stfld      string class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::'<MvcContrib.FluentHtml.Elements.IElement.LabelClass>k__BackingField' /* 0A00007B */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method Element`1::MvcContrib.FluentHtml.Elements.IElement.set_LabelClass

  .method /*0600006F*/ private hidebysig newslot specialname virtual final 
          instance valuetype [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagRenderMode/*01000013*/ 
          MvcContrib.FluentHtml.Elements.IElement.get_TagRenderMode() cil managed
  // SIG: 20 00 11 4D
  {
    .override MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::get_TagRenderMode /*0200001B::06000046*/ 
    // Method begins at RVA 0x2cd9
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 6F   | (0A)00007C       */ callvirt   instance valuetype [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagRenderMode/*01000013*/ class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::get_TagRenderMode() /* 0A00007C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method Element`1::MvcContrib.FluentHtml.Elements.IElement.get_TagRenderMode

  .method /*06000070*/ private hidebysig newslot specialname virtual final 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<object,object> 
          MvcContrib.FluentHtml.Elements.IElement.get_Metadata() cil managed
  // SIG: 20 00 15 12 51 02 1C 1C
  {
    .override MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::get_Metadata /*0200001B::06000050*/ 
    // Method begins at RVA 0x2ce1
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (0A)000063       */ ldfld      class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<object,object> class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::metadata /* 0A000063 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method Element`1::MvcContrib.FluentHtml.Elements.IElement.get_Metadata

  .method /*06000071*/ private hidebysig newslot virtual final 
          instance void  MvcContrib.FluentHtml.Elements.IElement.SetAutoLabel() cil managed
  // SIG: 20 00 01
  {
    .override MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::SetAutoLabel /*0200001B::06000051*/ 
    // Method begins at RVA 0x2cec
    // Code size       47 (0x2f)
    .maxstack  3
    .locals /*11000011*/ init (class MvcContrib.FluentHtml.Behaviors.AutoLabelSettings/*02000006*/ V_0)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00007D       */ call       instance bool class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::ShouldAutoLabel() /* 0A00007D */
    IL_0006:  /* 2C   | 26               */ brfalse.s  IL_002e

    IL_0008:  /* 02   |                  */ ldarg.0
    IL_0009:  /* 28   | (0A)00007E       */ call       instance class MvcContrib.FluentHtml.Behaviors.AutoLabelSettings/*02000006*/ class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::GetAutoLabelSettings() /* 0A00007E */
    IL_000e:  /* 0A   |                  */ stloc.0
    IL_000f:  /* 02   |                  */ ldarg.0
    IL_0010:  /* 02   |                  */ ldarg.0
    IL_0011:  /* 06   |                  */ ldloc.0
    IL_0012:  /* 28   | (0A)00007F       */ call       instance string class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::GetAutoLabelText(class MvcContrib.FluentHtml.Behaviors.AutoLabelSettings/*02000006*/) /* 0A00007F */
    IL_0017:  /* 6F   | (06)00004B       */ callvirt   instance void MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::set_LabelBeforeText(string) /* 0600004B */
    IL_001c:  /* 02   |                  */ ldarg.0
    IL_001d:  /* 06   |                  */ ldloc.0
    IL_001e:  /* 2C   | 08               */ brfalse.s  IL_0028

    IL_0020:  /* 06   |                  */ ldloc.0
    IL_0021:  /* 6F   | (06)00000E       */ callvirt   instance string MvcContrib.FluentHtml.Behaviors.AutoLabelSettings/*02000006*/::get_LabelCssClass() /* 0600000E */
    IL_0026:  /* 2B   | 01               */ br.s       IL_0029

    IL_0028:  /* 14   |                  */ ldnull
    IL_0029:  /* 6F   | (06)00004F       */ callvirt   instance void MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::set_LabelClass(string) /* 0600004F */
    IL_002e:  /* 2A   |                  */ ret
  } // end of method Element`1::MvcContrib.FluentHtml.Elements.IElement.SetAutoLabel

  .method /*06000072*/ private hidebysig newslot virtual final 
          instance void  MvcContrib.FluentHtml.Elements.IElement.SetAutoLabelAfter() cil managed
  // SIG: 20 00 01
  {
    .override MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::SetAutoLabelAfter /*0200001B::06000052*/ 
    // Method begins at RVA 0x2d28
    // Code size       47 (0x2f)
    .maxstack  3
    .locals /*11000011*/ init (class MvcContrib.FluentHtml.Behaviors.AutoLabelSettings/*02000006*/ V_0)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00007D       */ call       instance bool class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::ShouldAutoLabel() /* 0A00007D */
    IL_0006:  /* 2C   | 26               */ brfalse.s  IL_002e

    IL_0008:  /* 02   |                  */ ldarg.0
    IL_0009:  /* 28   | (0A)00007E       */ call       instance class MvcContrib.FluentHtml.Behaviors.AutoLabelSettings/*02000006*/ class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::GetAutoLabelSettings() /* 0A00007E */
    IL_000e:  /* 0A   |                  */ stloc.0
    IL_000f:  /* 02   |                  */ ldarg.0
    IL_0010:  /* 02   |                  */ ldarg.0
    IL_0011:  /* 06   |                  */ ldloc.0
    IL_0012:  /* 28   | (0A)00007F       */ call       instance string class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::GetAutoLabelText(class MvcContrib.FluentHtml.Behaviors.AutoLabelSettings/*02000006*/) /* 0A00007F */
    IL_0017:  /* 6F   | (06)00004D       */ callvirt   instance void MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::set_LabelAfterText(string) /* 0600004D */
    IL_001c:  /* 02   |                  */ ldarg.0
    IL_001d:  /* 06   |                  */ ldloc.0
    IL_001e:  /* 2C   | 08               */ brfalse.s  IL_0028

    IL_0020:  /* 06   |                  */ ldloc.0
    IL_0021:  /* 6F   | (06)00000E       */ callvirt   instance string MvcContrib.FluentHtml.Behaviors.AutoLabelSettings/*02000006*/::get_LabelCssClass() /* 0600000E */
    IL_0026:  /* 2B   | 01               */ br.s       IL_0029

    IL_0028:  /* 14   |                  */ ldnull
    IL_0029:  /* 6F   | (06)00004F       */ callvirt   instance void MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::set_LabelClass(string) /* 0600004F */
    IL_002e:  /* 2A   |                  */ ret
  } // end of method Element`1::MvcContrib.FluentHtml.Elements.IElement.SetAutoLabelAfter

  .method /*06000073*/ private hidebysig newslot specialname virtual final 
          instance class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ 
          MvcContrib.FluentHtml.Elements.IMemberElement.get_ForMember() cil managed
  // SIG: 20 00 12 39
  {
    .override MvcContrib.FluentHtml.Elements.IMemberElement/*0200001C*/::get_ForMember /*0200001C::06000054*/ 
    // Method begins at RVA 0x2d63
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (0A)000060       */ ldfld      class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::forMember /* 0A000060 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method Element`1::MvcContrib.FluentHtml.Elements.IMemberElement.get_ForMember

  .method /*06000074*/ private hidebysig newslot virtual final 
          instance void  MvcContrib.FluentHtml.Elements.IElement.AddClass(string classToAdd) cil managed
  // SIG: 20 01 01 0E
  {
    .override MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::AddClass /*0200001B::06000053*/ 
    // Method begins at RVA 0x2d6b
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (0A)000065       */ ldfld      class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::builder /* 0A000065 */
    IL_0006:  /* 03   |                  */ ldarg.1
    IL_0007:  /* 6F   | (0A)00005B       */ callvirt   instance void [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/::AddCssClass(string) /* 0A00005B */
    IL_000c:  /* 2A   |                  */ ret
  } // end of method Element`1::MvcContrib.FluentHtml.Elements.IElement.AddClass

  .method /*06000075*/ family hidebysig newslot specialname virtual 
          instance valuetype [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagRenderMode/*01000013*/ 
          get_TagRenderMode() cil managed
  // SIG: 20 00 11 4D
  {
    // Method begins at RVA 0x2d79
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 16   |                  */ ldc.i4.0
    IL_0001:  /* 2A   |                  */ ret
  } // end of method Element`1::get_TagRenderMode

  .method /*06000076*/ family hidebysig instance bool 
          ShouldAutoLabel() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2d7c
    // Code size       28 (0x1c)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 6F   | (06)00004A       */ callvirt   instance string MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::get_LabelBeforeText() /* 0600004A */
    IL_0006:  /* 2D   | 12               */ brtrue.s   IL_001a

    IL_0008:  /* 02   |                  */ ldarg.0
    IL_0009:  /* 6F   | (06)00004C       */ callvirt   instance string MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::get_LabelAfterText() /* 0600004C */
    IL_000e:  /* 2D   | 0A               */ brtrue.s   IL_001a

    IL_0010:  /* 02   |                  */ ldarg.0
    IL_0011:  /* 7B   | (0A)000070       */ ldfld      bool class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::doNotAutoLabel /* 0A000070 */
    IL_0016:  /* 16   |                  */ ldc.i4.0
    IL_0017:  /* FE01 |                  */ ceq
    IL_0019:  /* 2A   |                  */ ret

    IL_001a:  /* 16   |                  */ ldc.i4.0
    IL_001b:  /* 2A   |                  */ ret
  } // end of method Element`1::ShouldAutoLabel

  .method /*06000077*/ family hidebysig instance class MvcContrib.FluentHtml.Behaviors.AutoLabelSettings/*02000006*/ 
          GetAutoLabelSettings() cil managed
  // SIG: 20 00 12 18
  {
    // Method begins at RVA 0x2da4
    // Code size       75 (0x4b)
    .maxstack  4
    .locals /*11000011*/ init (class MvcContrib.FluentHtml.Behaviors.AutoLabelSettings/*02000006*/ V_0)
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 0A   |                  */ stloc.0
    IL_0002:  /* 02   |                  */ ldarg.0
    IL_0003:  /* 7B   | (0A)000061       */ ldfld      class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::behaviors /* 0A000061 */
    IL_0008:  /* 2C   | 33               */ brfalse.s  IL_003d

    IL_000a:  /* 02   |                  */ ldarg.0
    IL_000b:  /* 7B   | (0A)000061       */ ldfld      class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::behaviors /* 0A000061 */
    IL_0010:  /* 7E   | (0A)000080       */ ldsfld     class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/,bool> class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::'CS$<>9__CachedAnonymousMethodDelegate1' /* 0A000080 */
    IL_0015:  /* 2D   | 11               */ brtrue.s   IL_0028

    IL_0017:  /* 14   |                  */ ldnull
    IL_0018:  /* FE06 | (0A)000081       */ ldftn      bool class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::'<GetAutoLabelSettings>b__0'(class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/) /* 0A000081 */
    IL_001e:  /* 73   | (0A)000082       */ newobj     instance void class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/,bool>/*1B000048*/::.ctor(object,
                                                                                                                                                                                                                         native int) /* 0A000082 */
    IL_0023:  /* 80   | (0A)000080       */ stsfld     class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/,bool> class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::'CS$<>9__CachedAnonymousMethodDelegate1' /* 0A000080 */
    IL_0028:  /* 7E   | (0A)000080       */ ldsfld     class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/,bool> class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::'CS$<>9__CachedAnonymousMethodDelegate1' /* 0A000080 */
    IL_002d:  /* 28   | (2B)000008       */ call       class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<!!0> [System.Core/*23000002*/]System.Linq.Enumerable/*01000037*/::Where<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>(class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<!!0>,
                                                                                                                                                                                                                                                                                   class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,bool>) /* 2B000008 */
    IL_0032:  /* 28   | (2B)000009       */ call       !!0 [System.Core/*23000002*/]System.Linq.Enumerable/*01000037*/::FirstOrDefault<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>(class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<!!0>) /* 2B000009 */
    IL_0037:  /* 75   | (02)000006       */ isinst     MvcContrib.FluentHtml.Behaviors.AutoLabelSettings/*02000006*/
    IL_003c:  /* 0A   |                  */ stloc.0
    IL_003d:  /* 06   |                  */ ldloc.0
    IL_003e:  /* 25   |                  */ dup
    IL_003f:  /* 2D   | 09               */ brtrue.s   IL_004a

    IL_0041:  /* 26   |                  */ pop
    IL_0042:  /* 16   |                  */ ldc.i4.0
    IL_0043:  /* 14   |                  */ ldnull
    IL_0044:  /* 14   |                  */ ldnull
    IL_0045:  /* 73   | (06)000011       */ newobj     instance void MvcContrib.FluentHtml.Behaviors.AutoLabelSettings/*02000006*/::.ctor(bool,
                                                                                                                                          string,
                                                                                                                                          string) /* 06000011 */
    IL_004a:  /* 2A   |                  */ ret
  } // end of method Element`1::GetAutoLabelSettings

  .method /*06000078*/ family hidebysig instance string 
          GetAutoLabelText(class MvcContrib.FluentHtml.Behaviors.AutoLabelSettings/*02000006*/ settings) cil managed
  // SIG: 20 01 0E 12 18
  {
    // Method begins at RVA 0x2dfc
    // Code size       105 (0x69)
    .maxstack  3
    .locals /*11000012*/ init (string V_0,
             int32 V_1)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 72   | (70)000091       */ ldstr      "name" /* 70000091 */
    IL_0006:  /* 6F   | (06)000048       */ callvirt   instance string MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::GetAttr(string) /* 06000048 */
    IL_000b:  /* 0A   |                  */ stloc.0
    IL_000c:  /* 06   |                  */ ldloc.0
    IL_000d:  /* 2D   | 02               */ brtrue.s   IL_0011

    IL_000f:  /* 06   |                  */ ldloc.0
    IL_0010:  /* 2A   |                  */ ret

    IL_0011:  /* 03   |                  */ ldarg.1
    IL_0012:  /* 6F   | (06)00000A       */ callvirt   instance bool MvcContrib.FluentHtml.Behaviors.AutoLabelSettings/*02000006*/::get_UseFullNameForNestedProperties() /* 0600000A */
    IL_0017:  /* 2C   | 0D               */ brfalse.s  IL_0026

    IL_0019:  /* 06   |                  */ ldloc.0
    IL_001a:  /* 1F   | 2E               */ ldc.i4.s   46
    IL_001c:  /* 1F   | 20               */ ldc.i4.s   32
    IL_001e:  /* 6F   | (0A)00006B       */ callvirt   instance string [mscorlib/*23000001*/]System.String/*0100003F*/::Replace(char,
                                                                                                                                char) /* 0A00006B */
    IL_0023:  /* 0A   |                  */ stloc.0
    IL_0024:  /* 2B   | 1A               */ br.s       IL_0040

    IL_0026:  /* 06   |                  */ ldloc.0
    IL_0027:  /* 72   | (70)0000F3       */ ldstr      "." /* 700000F3 */
    IL_002c:  /* 6F   | (0A)000084       */ callvirt   instance int32 [mscorlib/*23000001*/]System.String/*0100003F*/::LastIndexOf(string) /* 0A000084 */
    IL_0031:  /* 0B   |                  */ stloc.1
    IL_0032:  /* 07   |                  */ ldloc.1
    IL_0033:  /* 16   |                  */ ldc.i4.0
    IL_0034:  /* 32   | 0A               */ blt.s      IL_0040

    IL_0036:  /* 06   |                  */ ldloc.0
    IL_0037:  /* 07   |                  */ ldloc.1
    IL_0038:  /* 17   |                  */ ldc.i4.1
    IL_0039:  /* 58   |                  */ add
    IL_003a:  /* 6F   | (0A)000085       */ callvirt   instance string [mscorlib/*23000001*/]System.String/*0100003F*/::Substring(int32) /* 0A000085 */
    IL_003f:  /* 0A   |                  */ stloc.0
    IL_0040:  /* 06   |                  */ ldloc.0
    IL_0041:  /* 28   | (06)000110       */ call       string MvcContrib.FluentHtml.StringExtensions/*0200003B*/::PascalCaseToPhrase(string) /* 06000110 */
    IL_0046:  /* 0A   |                  */ stloc.0
    IL_0047:  /* 02   |                  */ ldarg.0
    IL_0048:  /* 06   |                  */ ldloc.0
    IL_0049:  /* 28   | (0A)000086       */ call       instance string class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::RemoveArrayNotationFromPhrase(string) /* 0A000086 */
    IL_004e:  /* 0A   |                  */ stloc.0
    IL_004f:  /* 03   |                  */ ldarg.1
    IL_0050:  /* 6F   | (06)00000C       */ callvirt   instance string MvcContrib.FluentHtml.Behaviors.AutoLabelSettings/*02000006*/::get_LabelFormat() /* 0600000C */
    IL_0055:  /* 2D   | 03               */ brtrue.s   IL_005a

    IL_0057:  /* 06   |                  */ ldloc.0
    IL_0058:  /* 2B   | 0C               */ br.s       IL_0066

    IL_005a:  /* 03   |                  */ ldarg.1
    IL_005b:  /* 6F   | (06)00000C       */ callvirt   instance string MvcContrib.FluentHtml.Behaviors.AutoLabelSettings/*02000006*/::get_LabelFormat() /* 0600000C */
    IL_0060:  /* 06   |                  */ ldloc.0
    IL_0061:  /* 28   | (0A)000087       */ call       string [mscorlib/*23000001*/]System.String/*0100003F*/::Format(string,
                                                                                                                      object) /* 0A000087 */
    IL_0066:  /* 0A   |                  */ stloc.0
    IL_0067:  /* 06   |                  */ ldloc.0
    IL_0068:  /* 2A   |                  */ ret
  } // end of method Element`1::GetAutoLabelText

  .method /*06000079*/ family hidebysig instance string 
          RemoveArrayNotationFromPhrase(string phrase) cil managed
  // SIG: 20 01 0E 0E
  {
    // Method begins at RVA 0x2e74
    // Code size       78 (0x4e)
    .maxstack  4
    .locals /*11000013*/ init (class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000021*/<string> V_0,
             char[] V_1)
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 72   | (70)0000F7       */ ldstr      "[" /* 700000F7 */
    IL_0006:  /* 6F   | (0A)000088       */ callvirt   instance int32 [mscorlib/*23000001*/]System.String/*0100003F*/::IndexOf(string) /* 0A000088 */
    IL_000b:  /* 16   |                  */ ldc.i4.0
    IL_000c:  /* 32   | 3E               */ blt.s      IL_004c

    IL_000e:  /* 03   |                  */ ldarg.1
    IL_000f:  /* 17   |                  */ ldc.i4.1
    IL_0010:  /* 8D   | (01)000057       */ newarr     [mscorlib/*23000001*/]System.Char/*01000057*/
    IL_0015:  /* 0B   |                  */ stloc.1
    IL_0016:  /* 07   |                  */ ldloc.1
    IL_0017:  /* 16   |                  */ ldc.i4.0
    IL_0018:  /* 1F   | 20               */ ldc.i4.s   32
    IL_001a:  /* 9D   |                  */ stelem.i2
    IL_001b:  /* 07   |                  */ ldloc.1
    IL_001c:  /* 6F   | (0A)000089       */ callvirt   instance string[] [mscorlib/*23000001*/]System.String/*0100003F*/::Split(char[]) /* 0A000089 */
    IL_0021:  /* 73   | (0A)00008A       */ newobj     instance void class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000021*/<string>/*1B000049*/::.ctor(class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<!0>) /* 0A00008A */
    IL_0026:  /* 0A   |                  */ stloc.0
    IL_0027:  /* 06   |                  */ ldloc.0
    IL_0028:  /* 02   |                  */ ldarg.0
    IL_0029:  /* FE06 | (0A)00008B       */ ldftn      instance string class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::RemoveArrayNotation(string) /* 0A00008B */
    IL_002f:  /* 73   | (0A)00008C       */ newobj     instance void class [mscorlib/*23000001*/]System.Converter`2/*01000058*/<string,string>/*1B00004A*/::.ctor(object,
                                                                                                                                                                  native int) /* 0A00008C */
    IL_0034:  /* 6F   | (2B)00000A       */ callvirt   instance class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000021*/<!!0> class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000021*/<string>/*1B000049*/::ConvertAll<string>(class [mscorlib/*23000001*/]System.Converter`2/*01000058*/<!0,!!0>) /* 2B00000A */
    IL_0039:  /* 0A   |                  */ stloc.0
    IL_003a:  /* 72   | (70)0000FB       */ ldstr      " " /* 700000FB */
    IL_003f:  /* 06   |                  */ ldloc.0
    IL_0040:  /* 6F   | (0A)00008E       */ callvirt   instance !0[] class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000021*/<string>/*1B000049*/::ToArray() /* 0A00008E */
    IL_0045:  /* 28   | (0A)00008F       */ call       string [mscorlib/*23000001*/]System.String/*0100003F*/::Join(string,
                                                                                                                    string[]) /* 0A00008F */
    IL_004a:  /* 10   | 01               */ starg.s    phrase
    IL_004c:  /* 03   |                  */ ldarg.1
    IL_004d:  /* 2A   |                  */ ret
  } // end of method Element`1::RemoveArrayNotationFromPhrase

  .method /*0600007A*/ family hidebysig instance string 
          RemoveArrayNotation(string s) cil managed
  // SIG: 20 01 0E 0E
  {
    // Method begins at RVA 0x2ed0
    // Code size       23 (0x17)
    .maxstack  3
    .locals /*11000014*/ init (int32 V_0)
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 1F   | 5B               */ ldc.i4.s   91
    IL_0003:  /* 6F   | (0A)000090       */ callvirt   instance int32 [mscorlib/*23000001*/]System.String/*0100003F*/::LastIndexOf(char) /* 0A000090 */
    IL_0008:  /* 0A   |                  */ stloc.0
    IL_0009:  /* 06   |                  */ ldloc.0
    IL_000a:  /* 16   |                  */ ldc.i4.0
    IL_000b:  /* 2F   | 02               */ bge.s      IL_000f

    IL_000d:  /* 03   |                  */ ldarg.1
    IL_000e:  /* 2A   |                  */ ret

    IL_000f:  /* 03   |                  */ ldarg.1
    IL_0010:  /* 06   |                  */ ldloc.0
    IL_0011:  /* 6F   | (0A)000091       */ callvirt   instance string [mscorlib/*23000001*/]System.String/*0100003F*/::Remove(int32) /* 0A000091 */
    IL_0016:  /* 2A   |                  */ ret
  } // end of method Element`1::RemoveArrayNotation

  .method /*0600007B*/ family hidebysig newslot virtual 
          instance string  RenderLabel(string labelText) cil managed
  // SIG: 20 01 0E 0E
  {
    // Method begins at RVA 0x2ef4
    // Code size       26 (0x1a)
    .maxstack  2
    .locals /*11000015*/ init (class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ V_0)
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 2D   | 02               */ brtrue.s   IL_0005

    IL_0003:  /* 14   |                  */ ldnull
    IL_0004:  /* 2A   |                  */ ret

    IL_0005:  /* 02   |                  */ ldarg.0
    IL_0006:  /* 28   | (0A)000092       */ call       instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::GetLabelBuilder() /* 0A000092 */
    IL_000b:  /* 0A   |                  */ stloc.0
    IL_000c:  /* 06   |                  */ ldloc.0
    IL_000d:  /* 03   |                  */ ldarg.1
    IL_000e:  /* 6F   | (0A)000093       */ callvirt   instance void [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/::SetInnerText(string) /* 0A000093 */
    IL_0013:  /* 06   |                  */ ldloc.0
    IL_0014:  /* 6F   | (0A)00006E       */ callvirt   instance string [mscorlib/*23000001*/]System.Object/*01000001*/::ToString() /* 0A00006E */
    IL_0019:  /* 2A   |                  */ ret
  } // end of method Element`1::RenderLabel

  .method /*0600007C*/ family hidebysig instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ 
          GetLabelBuilder() cil managed
  // SIG: 20 00 12 49
  {
    // Method begins at RVA 0x2f1c
    // Code size       122 (0x7a)
    .maxstack  4
    .locals /*11000016*/ init (class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ V_0,
             string V_1)
    IL_0000:  /* 72   | (70)0000FF       */ ldstr      "label" /* 700000FF */
    IL_0005:  /* 73   | (0A)000064       */ newobj     instance void [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/::.ctor(string) /* 0A000064 */
    IL_000a:  /* 0A   |                  */ stloc.0
    IL_000b:  /* 02   |                  */ ldarg.0
    IL_000c:  /* 7B   | (0A)000065       */ ldfld      class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::builder /* 0A000065 */
    IL_0011:  /* 6F   | (0A)000076       */ callvirt   instance class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<string,string> [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/::get_Attributes() /* 0A000076 */
    IL_0016:  /* 72   | (70)0000B3       */ ldstr      "id" /* 700000B3 */
    IL_001b:  /* 6F   | (0A)000094       */ callvirt   instance bool class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<string,string>/*1B000047*/::ContainsKey(!0) /* 0A000094 */
    IL_0020:  /* 2C   | 38               */ brfalse.s  IL_005a

    IL_0022:  /* 02   |                  */ ldarg.0
    IL_0023:  /* 7B   | (0A)000065       */ ldfld      class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::builder /* 0A000065 */
    IL_0028:  /* 6F   | (0A)000076       */ callvirt   instance class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<string,string> [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/::get_Attributes() /* 0A000076 */
    IL_002d:  /* 72   | (70)0000B3       */ ldstr      "id" /* 700000B3 */
    IL_0032:  /* 6F   | (0A)000095       */ callvirt   instance !1 class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<string,string>/*1B000047*/::get_Item(!0) /* 0A000095 */
    IL_0037:  /* 0B   |                  */ stloc.1
    IL_0038:  /* 06   |                  */ ldloc.0
    IL_0039:  /* 72   | (70)00010B       */ ldstr      "for" /* 7000010B */
    IL_003e:  /* 07   |                  */ ldloc.1
    IL_003f:  /* 6F   | (0A)00006F       */ callvirt   instance void [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/::MergeAttribute(string,
                                                                                                                                                       string) /* 0A00006F */
    IL_0044:  /* 06   |                  */ ldloc.0
    IL_0045:  /* 72   | (70)0000B3       */ ldstr      "id" /* 700000B3 */
    IL_004a:  /* 72   | (70)000113       */ ldstr      "{0}_Label" /* 70000113 */
    IL_004f:  /* 07   |                  */ ldloc.1
    IL_0050:  /* 28   | (0A)000087       */ call       string [mscorlib/*23000001*/]System.String/*0100003F*/::Format(string,
                                                                                                                      object) /* 0A000087 */
    IL_0055:  /* 6F   | (0A)00006F       */ callvirt   instance void [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/::MergeAttribute(string,
                                                                                                                                                       string) /* 0A00006F */
    IL_005a:  /* 02   |                  */ ldarg.0
    IL_005b:  /* 6F   | (06)00004E       */ callvirt   instance string MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::get_LabelClass() /* 0600004E */
    IL_0060:  /* 28   | (0A)000096       */ call       bool [mscorlib/*23000001*/]System.String/*0100003F*/::IsNullOrEmpty(string) /* 0A000096 */
    IL_0065:  /* 2D   | 11               */ brtrue.s   IL_0078

    IL_0067:  /* 06   |                  */ ldloc.0
    IL_0068:  /* 72   | (70)000127       */ ldstr      "class" /* 70000127 */
    IL_006d:  /* 02   |                  */ ldarg.0
    IL_006e:  /* 6F   | (06)00004E       */ callvirt   instance string MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::get_LabelClass() /* 0600004E */
    IL_0073:  /* 6F   | (0A)00006F       */ callvirt   instance void [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/::MergeAttribute(string,
                                                                                                                                                       string) /* 0A00006F */
    IL_0078:  /* 06   |                  */ ldloc.0
    IL_0079:  /* 2A   |                  */ ret
  } // end of method Element`1::GetLabelBuilder

  .method /*0600007D*/ family hidebysig instance void 
          ApplyBehaviors() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2fc8
    // Code size       142 (0x8e)
    .maxstack  4
    .locals /*11000017*/ init (class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> V_0,
             class [System.Core/*23000002*/]System.Linq.IOrderedEnumerable`1/*01000059*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> V_1)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (0A)000061       */ ldfld      class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::behaviors /* 0A000061 */
    IL_0006:  /* 2D   | 01               */ brtrue.s   IL_0009

    IL_0008:  /* 2A   |                  */ ret

    IL_0009:  /* 02   |                  */ ldarg.0
    IL_000a:  /* 7B   | (0A)000061       */ ldfld      class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::behaviors /* 0A000061 */
    IL_000f:  /* 7E   | (0A)000097       */ ldsfld     class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/,bool> class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::'CS$<>9__CachedAnonymousMethodDelegate5' /* 0A000097 */
    IL_0014:  /* 2D   | 11               */ brtrue.s   IL_0027

    IL_0016:  /* 14   |                  */ ldnull
    IL_0017:  /* FE06 | (0A)000098       */ ldftn      bool class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::'<ApplyBehaviors>b__2'(class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/) /* 0A000098 */
    IL_001d:  /* 73   | (0A)000082       */ newobj     instance void class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/,bool>/*1B000048*/::.ctor(object,
                                                                                                                                                                                                                         native int) /* 0A000082 */
    IL_0022:  /* 80   | (0A)000097       */ stsfld     class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/,bool> class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::'CS$<>9__CachedAnonymousMethodDelegate5' /* 0A000097 */
    IL_0027:  /* 7E   | (0A)000097       */ ldsfld     class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/,bool> class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::'CS$<>9__CachedAnonymousMethodDelegate5' /* 0A000097 */
    IL_002c:  /* 28   | (2B)000008       */ call       class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<!!0> [System.Core/*23000002*/]System.Linq.Enumerable/*01000037*/::Where<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>(class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<!!0>,
                                                                                                                                                                                                                                                                                   class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,bool>) /* 2B000008 */
    IL_0031:  /* 0A   |                  */ stloc.0
    IL_0032:  /* 06   |                  */ ldloc.0
    IL_0033:  /* 02   |                  */ ldarg.0
    IL_0034:  /* 28   | (2B)00000B       */ call       class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> MvcContrib.FluentHtml.Behaviors.BehaviorMarkerExtensions/*0200000A*/::ApplyTo<class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>>(class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>,
                                                                                                                                                                                                                                                                                                                                                         !!0) /* 2B00000B */
    IL_0039:  /* 26   |                  */ pop
    IL_003a:  /* 02   |                  */ ldarg.0
    IL_003b:  /* 7B   | (0A)000061       */ ldfld      class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::behaviors /* 0A000061 */
    IL_0040:  /* 7E   | (0A)000099       */ ldsfld     class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/,bool> class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::'CS$<>9__CachedAnonymousMethodDelegate6' /* 0A000099 */
    IL_0045:  /* 2D   | 11               */ brtrue.s   IL_0058

    IL_0047:  /* 14   |                  */ ldnull
    IL_0048:  /* FE06 | (0A)00009A       */ ldftn      bool class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::'<ApplyBehaviors>b__3'(class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/) /* 0A00009A */
    IL_004e:  /* 73   | (0A)000082       */ newobj     instance void class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/,bool>/*1B000048*/::.ctor(object,
                                                                                                                                                                                                                         native int) /* 0A000082 */
    IL_0053:  /* 80   | (0A)000099       */ stsfld     class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/,bool> class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::'CS$<>9__CachedAnonymousMethodDelegate6' /* 0A000099 */
    IL_0058:  /* 7E   | (0A)000099       */ ldsfld     class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/,bool> class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::'CS$<>9__CachedAnonymousMethodDelegate6' /* 0A000099 */
    IL_005d:  /* 28   | (2B)000008       */ call       class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<!!0> [System.Core/*23000002*/]System.Linq.Enumerable/*01000037*/::Where<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>(class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<!!0>,
                                                                                                                                                                                                                                                                                   class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,bool>) /* 2B000008 */
    IL_0062:  /* 7E   | (0A)00009B       */ ldsfld     class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/,int32> class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::'CS$<>9__CachedAnonymousMethodDelegate7' /* 0A00009B */
    IL_0067:  /* 2D   | 11               */ brtrue.s   IL_007a

    IL_0069:  /* 14   |                  */ ldnull
    IL_006a:  /* FE06 | (0A)00009C       */ ldftn      int32 class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::'<ApplyBehaviors>b__4'(class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/) /* 0A00009C */
    IL_0070:  /* 73   | (0A)00009D       */ newobj     instance void class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/,int32>/*1B00004B*/::.ctor(object,
                                                                                                                                                                                                                          native int) /* 0A00009D */
    IL_0075:  /* 80   | (0A)00009B       */ stsfld     class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/,int32> class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::'CS$<>9__CachedAnonymousMethodDelegate7' /* 0A00009B */
    IL_007a:  /* 7E   | (0A)00009B       */ ldsfld     class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/,int32> class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::'CS$<>9__CachedAnonymousMethodDelegate7' /* 0A00009B */
    IL_007f:  /* 28   | (2B)00000C       */ call       class [System.Core/*23000002*/]System.Linq.IOrderedEnumerable`1/*01000059*/<!!0> [System.Core/*23000002*/]System.Linq.Enumerable/*01000037*/::OrderBy<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/,int32>(class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<!!0>,
                                                                                                                                                                                                                                                                                      class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,!!1>) /* 2B00000C */
    IL_0084:  /* 0B   |                  */ stloc.1
    IL_0085:  /* 07   |                  */ ldloc.1
    IL_0086:  /* 02   |                  */ ldarg.0
    IL_0087:  /* 28   | (2B)00000B       */ call       class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> MvcContrib.FluentHtml.Behaviors.BehaviorMarkerExtensions/*0200000A*/::ApplyTo<class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>>(class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>,
                                                                                                                                                                                                                                                                                                                                                         !!0) /* 2B00000B */
    IL_008c:  /* 26   |                  */ pop
    IL_008d:  /* 2A   |                  */ ret
  } // end of method Element`1::ApplyBehaviors

  .method /*0600007E*/ family hidebysig newslot virtual 
          instance void  PreRender() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x3062
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Element`1::PreRender

  .method /*0600007F*/ private hidebysig static 
          bool  '<GetAutoLabelSettings>b__0'(class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/ x) cil managed
  // SIG: 00 01 02 12 08
  {
    .custom /*0C000047:0A000013*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000032*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2d99
    // Code size       10 (0xa)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 75   | (02)000006       */ isinst     MvcContrib.FluentHtml.Behaviors.AutoLabelSettings/*02000006*/
    IL_0006:  /* 14   |                  */ ldnull
    IL_0007:  /* FE03 |                  */ cgt.un
    IL_0009:  /* 2A   |                  */ ret
  } // end of method Element`1::'<GetAutoLabelSettings>b__0'

  .method /*06000080*/ private hidebysig static 
          bool  '<ApplyBehaviors>b__2'(class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/ x) cil managed
  // SIG: 00 01 02 12 08
  {
    .custom /*0C000048:0A000013*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000032*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2fa2
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 75   | (02)000010       */ isinst     MvcContrib.FluentHtml.Behaviors.IOrderedBehavior/*02000010*/
    IL_0006:  /* 14   |                  */ ldnull
    IL_0007:  /* FE03 |                  */ cgt.un
    IL_0009:  /* 16   |                  */ ldc.i4.0
    IL_000a:  /* FE01 |                  */ ceq
    IL_000c:  /* 2A   |                  */ ret
  } // end of method Element`1::'<ApplyBehaviors>b__2'

  .method /*06000081*/ private hidebysig static 
          bool  '<ApplyBehaviors>b__3'(class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/ x) cil managed
  // SIG: 00 01 02 12 08
  {
    .custom /*0C000049:0A000013*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000032*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2fb0
    // Code size       10 (0xa)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 75   | (02)000010       */ isinst     MvcContrib.FluentHtml.Behaviors.IOrderedBehavior/*02000010*/
    IL_0006:  /* 14   |                  */ ldnull
    IL_0007:  /* FE03 |                  */ cgt.un
    IL_0009:  /* 2A   |                  */ ret
  } // end of method Element`1::'<ApplyBehaviors>b__3'

  .method /*06000082*/ private hidebysig static 
          int32  '<ApplyBehaviors>b__4'(class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/ x) cil managed
  // SIG: 00 01 08 12 08
  {
    .custom /*0C00004A:0A000013*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000032*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2fbb
    // Code size       12 (0xc)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 74   | (02)000010       */ castclass  MvcContrib.FluentHtml.Behaviors.IOrderedBehavior/*02000010*/
    IL_0006:  /* 6F   | (06)000034       */ callvirt   instance int32 MvcContrib.FluentHtml.Behaviors.IOrderedBehavior/*02000010*/::get_Order() /* 06000034 */
    IL_000b:  /* 2A   |                  */ ret
  } // end of method Element`1::'<ApplyBehaviors>b__4'

  .property /*1700000E*/ instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/
          MvcContrib.FluentHtml.Elements.IElement.Builder()
  {
    .get instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/::MvcContrib.FluentHtml.Elements.IElement.get_Builder() /* 06000057 */
  } // end of property Element`1::MvcContrib.FluentHtml.Elements.IElement.Builder
  .property /*1700000F*/ instance string MvcContrib.FluentHtml.Elements.IElement.LabelBeforeText()
  {
    .get instance string MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/::MvcContrib.FluentHtml.Elements.IElement.get_LabelBeforeText() /* 06000069 */
    .set instance void MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/::MvcContrib.FluentHtml.Elements.IElement.set_LabelBeforeText(string) /* 0600006A */
  } // end of property Element`1::MvcContrib.FluentHtml.Elements.IElement.LabelBeforeText
  .property /*17000010*/ instance string MvcContrib.FluentHtml.Elements.IElement.LabelAfterText()
  {
    .set instance void MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/::MvcContrib.FluentHtml.Elements.IElement.set_LabelAfterText(string) /* 0600006C */
    .get instance string MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/::MvcContrib.FluentHtml.Elements.IElement.get_LabelAfterText() /* 0600006B */
  } // end of property Element`1::MvcContrib.FluentHtml.Elements.IElement.LabelAfterText
  .property /*17000011*/ instance string MvcContrib.FluentHtml.Elements.IElement.LabelClass()
  {
    .get instance string MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/::MvcContrib.FluentHtml.Elements.IElement.get_LabelClass() /* 0600006D */
    .set instance void MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/::MvcContrib.FluentHtml.Elements.IElement.set_LabelClass(string) /* 0600006E */
  } // end of property Element`1::MvcContrib.FluentHtml.Elements.IElement.LabelClass
  .property /*17000012*/ instance valuetype [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagRenderMode/*01000013*/
          MvcContrib.FluentHtml.Elements.IElement.TagRenderMode()
  {
    .get instance valuetype [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagRenderMode/*01000013*/ MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/::MvcContrib.FluentHtml.Elements.IElement.get_TagRenderMode() /* 0600006F */
  } // end of property Element`1::MvcContrib.FluentHtml.Elements.IElement.TagRenderMode
  .property /*17000013*/ instance class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<object,object>
          MvcContrib.FluentHtml.Elements.IElement.Metadata()
  {
    .get instance class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<object,object> MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/::MvcContrib.FluentHtml.Elements.IElement.get_Metadata() /* 06000070 */
  } // end of property Element`1::MvcContrib.FluentHtml.Elements.IElement.Metadata
  .property /*17000014*/ instance class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/
          MvcContrib.FluentHtml.Elements.IMemberElement.ForMember()
  {
    .get instance class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/::MvcContrib.FluentHtml.Elements.IMemberElement.get_ForMember() /* 06000073 */
  } // end of property Element`1::MvcContrib.FluentHtml.Elements.IMemberElement.ForMember
  .property /*17000015*/ instance valuetype [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagRenderMode/*01000013*/
          TagRenderMode()
  {
    .get instance valuetype [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagRenderMode/*01000013*/ MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/::get_TagRenderMode() /* 06000075 */
  } // end of property Element`1::TagRenderMode
} // end of class MvcContrib.FluentHtml.Elements.Element`1

.class /*0200001E*/ public abstract auto ansi beforefieldinit MvcContrib.FluentHtml.Elements.DisableableElement`1<(class MvcContrib.FluentHtml.Elements.DisableableElement`1/*0200001E*/<!T>/*1B000006*/, MvcContrib.FluentHtml.Elements.IElement/*0200001B*/) T>
       extends class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/
{
  .method /*06000083*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(string tag,
                               class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ forMember,
                               class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> behaviors) cil managed
  // SIG: 20 03 01 0E 12 39 15 12 11 01 12 08
  {
    // Method begins at RVA 0x3064
    // Code size       10 (0xa)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 04   |                  */ ldarg.2
    IL_0003:  /* 05   |                  */ ldarg.3
    IL_0004:  /* 28   | (0A)00009F       */ call       instance void class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::.ctor(string,
                                                                                                                                                       class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                                                       class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 0A00009F */
    IL_0009:  /* 2A   |                  */ ret
  } // end of method DisableableElement`1::.ctor

  .method /*06000084*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(string tag) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x306f
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 28   | (0A)00005F       */ call       instance void class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::.ctor(string) /* 0A00005F */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method DisableableElement`1::.ctor

  .method /*06000085*/ public hidebysig instance !T 
          Disabled(bool 'value') cil managed
  // SIG: 20 01 13 00 02
  {
    // Method begins at RVA 0x3078
    // Code size       40 (0x28)
    .maxstack  8
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 2C   | 13               */ brfalse.s  IL_0016

    IL_0003:  /* 02   |                  */ ldarg.0
    IL_0004:  /* 72   | (70)000133       */ ldstr      "disabled" /* 70000133 */
    IL_0009:  /* 72   | (70)000133       */ ldstr      "disabled" /* 70000133 */
    IL_000e:  /* 6F   | (0A)0000A0       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::Attr(string,
                                                                                                                                                    object) /* 0A0000A0 */
    IL_0013:  /* 26   |                  */ pop
    IL_0014:  /* 2B   | 0B               */ br.s       IL_0021

    IL_0016:  /* 02   |                  */ ldarg.0
    IL_0017:  /* 72   | (70)000133       */ ldstr      "disabled" /* 70000133 */
    IL_001c:  /* 6F   | (06)000049       */ callvirt   instance void MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::RemoveAttr(string) /* 06000049 */
    IL_0021:  /* 02   |                  */ ldarg.0
    IL_0022:  /* A5   | (1B)000040       */ unbox.any  !T/*1B000040*/
    IL_0027:  /* 2A   |                  */ ret
  } // end of method DisableableElement`1::Disabled

} // end of class MvcContrib.FluentHtml.Elements.DisableableElement`1

.class /*0200001F*/ public abstract auto ansi beforefieldinit MvcContrib.FluentHtml.Elements.FormElement`1<(class MvcContrib.FluentHtml.Elements.FormElement`1/*0200001F*/<!T>/*1B000007*/, MvcContrib.FluentHtml.Elements.IElement/*0200001B*/) T>
       extends class MvcContrib.FluentHtml.Elements.DisableableElement`1/*0200001E*/<!T>/*1B000006*/
{
  .method /*06000086*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(string tag,
                               string name,
                               class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ forMember,
                               class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> behaviors) cil managed
  // SIG: 20 04 01 0E 0E 12 39 15 12 11 01 12 08
  {
    // Method begins at RVA 0x30a1
    // Code size       18 (0x12)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 05   |                  */ ldarg.3
    IL_0003:  /* 0E   | 04               */ ldarg.s    behaviors
    IL_0005:  /* 28   | (0A)0000A1       */ call       instance void class MvcContrib.FluentHtml.Elements.DisableableElement`1/*0200001E*/<!T>/*1B000006*/::.ctor(string,
                                                                                                                                                                  class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                                                                  class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 0A0000A1 */
    IL_000a:  /* 02   |                  */ ldarg.0
    IL_000b:  /* 04   |                  */ ldarg.2
    IL_000c:  /* 28   | (0A)0000A2       */ call       instance void class MvcContrib.FluentHtml.Elements.FormElement`1/*0200001F*/<!T>/*1B000007*/::SetName(string) /* 0A0000A2 */
    IL_0011:  /* 2A   |                  */ ret
  } // end of method FormElement`1::.ctor

  .method /*06000087*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(string tag,
                               string name) cil managed
  // SIG: 20 02 01 0E 0E
  {
    // Method begins at RVA 0x30b4
    // Code size       15 (0xf)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 28   | (0A)0000A3       */ call       instance void class MvcContrib.FluentHtml.Elements.DisableableElement`1/*0200001E*/<!T>/*1B000006*/::.ctor(string) /* 0A0000A3 */
    IL_0007:  /* 02   |                  */ ldarg.0
    IL_0008:  /* 04   |                  */ ldarg.2
    IL_0009:  /* 28   | (0A)0000A2       */ call       instance void class MvcContrib.FluentHtml.Elements.FormElement`1/*0200001F*/<!T>/*1B000007*/::SetName(string) /* 0A0000A2 */
    IL_000e:  /* 2A   |                  */ ret
  } // end of method FormElement`1::.ctor

  .method /*06000088*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x30c4
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 6F   | (0A)0000A4       */ callvirt   instance void class MvcContrib.FluentHtml.Elements.FormElement`1/*0200001F*/<!T>/*1B000007*/::InferIdFromName() /* 0A0000A4 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 28   | (0A)0000A5       */ call       instance string class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::ToString() /* 0A0000A5 */
    IL_000c:  /* 2A   |                  */ ret
  } // end of method FormElement`1::ToString

  .method /*06000089*/ family hidebysig specialname virtual 
          instance valuetype [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagRenderMode/*01000013*/ 
          get_TagRenderMode() cil managed
  // SIG: 20 00 11 4D
  {
    // Method begins at RVA 0x30d2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 19   |                  */ ldc.i4.3
    IL_0001:  /* 2A   |                  */ ret
  } // end of method FormElement`1::get_TagRenderMode

  .method /*0600008A*/ family hidebysig newslot virtual 
          instance void  InferIdFromName() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x30d5
    // Code size       62 (0x3e)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (0A)000065       */ ldfld      class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::builder /* 0A000065 */
    IL_0006:  /* 6F   | (0A)000076       */ callvirt   instance class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<string,string> [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/::get_Attributes() /* 0A000076 */
    IL_000b:  /* 72   | (70)0000B3       */ ldstr      "id" /* 700000B3 */
    IL_0010:  /* 6F   | (0A)000094       */ callvirt   instance bool class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<string,string>/*1B000047*/::ContainsKey(!0) /* 0A000094 */
    IL_0015:  /* 2D   | 26               */ brtrue.s   IL_003d

    IL_0017:  /* 02   |                  */ ldarg.0
    IL_0018:  /* 72   | (70)0000B3       */ ldstr      "id" /* 700000B3 */
    IL_001d:  /* 02   |                  */ ldarg.0
    IL_001e:  /* 7B   | (0A)000065       */ ldfld      class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::builder /* 0A000065 */
    IL_0023:  /* 6F   | (0A)000076       */ callvirt   instance class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<string,string> [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/::get_Attributes() /* 0A000076 */
    IL_0028:  /* 72   | (70)000091       */ ldstr      "name" /* 70000091 */
    IL_002d:  /* 6F   | (0A)000095       */ callvirt   instance !1 class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<string,string>/*1B000047*/::get_Item(!0) /* 0A000095 */
    IL_0032:  /* 28   | (06)00010E       */ call       string MvcContrib.FluentHtml.StringExtensions/*0200003B*/::FormatAsHtmlId(string) /* 0600010E */
    IL_0037:  /* 6F   | (0A)0000A0       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::Attr(string,
                                                                                                                                                    object) /* 0A0000A0 */
    IL_003c:  /* 26   |                  */ pop
    IL_003d:  /* 2A   |                  */ ret
  } // end of method FormElement`1::InferIdFromName

  .method /*0600008B*/ family hidebysig instance void 
          SetName(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x3114
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 72   | (70)000091       */ ldstr      "name" /* 70000091 */
    IL_0006:  /* 03   |                  */ ldarg.1
    IL_0007:  /* 6F   | (06)000047       */ callvirt   instance void MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::SetAttr(string,
                                                                                                                                  object) /* 06000047 */
    IL_000c:  /* 2A   |                  */ ret
  } // end of method FormElement`1::SetName

  .property /*17000016*/ instance valuetype [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagRenderMode/*01000013*/
          TagRenderMode()
  {
    .get instance valuetype [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagRenderMode/*01000013*/ MvcContrib.FluentHtml.Elements.FormElement`1/*0200001F*/::get_TagRenderMode() /* 06000089 */
  } // end of property FormElement`1::TagRenderMode
} // end of class MvcContrib.FluentHtml.Elements.FormElement`1

.class /*02000020*/ public abstract auto ansi beforefieldinit MvcContrib.FluentHtml.Elements.Input`1<(class MvcContrib.FluentHtml.Elements.Input`1/*02000020*/<!T>/*1B000008*/, MvcContrib.FluentHtml.Elements.IElement/*0200001B*/) T>
       extends class MvcContrib.FluentHtml.Elements.FormElement`1/*0200001F*/<!T>/*1B000007*/
       implements MvcContrib.FluentHtml.Behaviors.ISupportsModelState/*02000016*/
{
  .field /*04000022*/ family object elementValue
  .method /*0600008C*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(string 'type',
                               string name) cil managed
  // SIG: 20 02 01 0E 0E
  {
    // Method begins at RVA 0x3122
    // Code size       31 (0x1f)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 72   | (70)000029       */ ldstr      "input" /* 70000029 */
    IL_0006:  /* 04   |                  */ ldarg.2
    IL_0007:  /* 28   | (0A)0000A6       */ call       instance void class MvcContrib.FluentHtml.Elements.FormElement`1/*0200001F*/<!T>/*1B000007*/::.ctor(string,
                                                                                                                                                           string) /* 0A0000A6 */
    IL_000c:  /* 02   |                  */ ldarg.0
    IL_000d:  /* 7B   | (0A)000065       */ ldfld      class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::builder /* 0A000065 */
    IL_0012:  /* 72   | (70)00001F       */ ldstr      "type" /* 7000001F */
    IL_0017:  /* 03   |                  */ ldarg.1
    IL_0018:  /* 17   |                  */ ldc.i4.1
    IL_0019:  /* 6F   | (0A)000066       */ callvirt   instance void [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/::MergeAttribute(string,
                                                                                                                                                       string,
                                                                                                                                                       bool) /* 0A000066 */
    IL_001e:  /* 2A   |                  */ ret
  } // end of method Input`1::.ctor

  .method /*0600008D*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(string 'type',
                               string name,
                               class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ forMember,
                               class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> behaviors) cil managed
  // SIG: 20 04 01 0E 0E 12 39 15 12 11 01 12 08
  {
    // Method begins at RVA 0x3142
    // Code size       34 (0x22)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 72   | (70)000029       */ ldstr      "input" /* 70000029 */
    IL_0006:  /* 04   |                  */ ldarg.2
    IL_0007:  /* 05   |                  */ ldarg.3
    IL_0008:  /* 0E   | 04               */ ldarg.s    behaviors
    IL_000a:  /* 28   | (0A)0000A7       */ call       instance void class MvcContrib.FluentHtml.Elements.FormElement`1/*0200001F*/<!T>/*1B000007*/::.ctor(string,
                                                                                                                                                           string,
                                                                                                                                                           class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                                                           class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 0A0000A7 */
    IL_000f:  /* 02   |                  */ ldarg.0
    IL_0010:  /* 7B   | (0A)000065       */ ldfld      class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::builder /* 0A000065 */
    IL_0015:  /* 72   | (70)00001F       */ ldstr      "type" /* 7000001F */
    IL_001a:  /* 03   |                  */ ldarg.1
    IL_001b:  /* 17   |                  */ ldc.i4.1
    IL_001c:  /* 6F   | (0A)000066       */ callvirt   instance void [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/::MergeAttribute(string,
                                                                                                                                                       string,
                                                                                                                                                       bool) /* 0A000066 */
    IL_0021:  /* 2A   |                  */ ret
  } // end of method Input`1::.ctor

  .method /*0600008E*/ public hidebysig newslot virtual 
          instance !T  Value(object 'value') cil managed
  // SIG: 20 01 13 00 1C
  {
    // Method begins at RVA 0x3165
    // Code size       14 (0xe)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (0A)0000A8       */ stfld      object class MvcContrib.FluentHtml.Elements.Input`1/*02000020*/<!T>/*1B000008*/::elementValue /* 0A0000A8 */
    IL_0007:  /* 02   |                  */ ldarg.0
    IL_0008:  /* A5   | (1B)000040       */ unbox.any  !T/*1B000040*/
    IL_000d:  /* 2A   |                  */ ret
  } // end of method Input`1::Value

  .method /*0600008F*/ public hidebysig newslot virtual 
          instance !T  Size(int32 'value') cil managed
  // SIG: 20 01 13 00 08
  {
    // Method begins at RVA 0x3174
    // Code size       25 (0x19)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 72   | (70)000145       */ ldstr      "size" /* 70000145 */
    IL_0006:  /* 03   |                  */ ldarg.1
    IL_0007:  /* 8C   | (01)000044       */ box        [mscorlib/*23000001*/]System.Int32/*01000044*/
    IL_000c:  /* 6F   | (0A)0000A0       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::Attr(string,
                                                                                                                                                    object) /* 0A0000A0 */
    IL_0011:  /* 26   |                  */ pop
    IL_0012:  /* 02   |                  */ ldarg.0
    IL_0013:  /* A5   | (1B)000040       */ unbox.any  !T/*1B000040*/
    IL_0018:  /* 2A   |                  */ ret
  } // end of method Input`1::Size

  .method /*06000090*/ family hidebysig virtual 
          instance void  PreRender() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x318e
    // Code size       25 (0x19)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 72   | (70)00014F       */ ldstr      "value" /* 7000014F */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 7B   | (0A)0000A8       */ ldfld      object class MvcContrib.FluentHtml.Elements.Input`1/*02000020*/<!T>/*1B000008*/::elementValue /* 0A0000A8 */
    IL_000c:  /* 6F   | (0A)0000A0       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::Attr(string,
                                                                                                                                                    object) /* 0A0000A0 */
    IL_0011:  /* 26   |                  */ pop
    IL_0012:  /* 02   |                  */ ldarg.0
    IL_0013:  /* 28   | (0A)000072       */ call       instance void class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::PreRender() /* 0A000072 */
    IL_0018:  /* 2A   |                  */ ret
  } // end of method Input`1::PreRender

  .method /*06000091*/ private hidebysig newslot virtual final 
          instance void  MvcContrib.FluentHtml.Behaviors.ISupportsModelState.ApplyModelState(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelState/*0100000F*/ state) cil managed
  // SIG: 20 01 01 12 3D
  {
    .override MvcContrib.FluentHtml.Behaviors.ISupportsModelState/*02000016*/::ApplyModelState /*02000016::06000038*/ 
    // Method begins at RVA 0x31a8
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 6F   | (0A)0000A9       */ callvirt   instance void class MvcContrib.FluentHtml.Elements.Input`1/*02000020*/<!T>/*1B000008*/::ApplyModelState(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelState/*0100000F*/) /* 0A0000A9 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method Input`1::MvcContrib.FluentHtml.Behaviors.ISupportsModelState.ApplyModelState

  .method /*06000092*/ family hidebysig newslot virtual 
          instance void  ApplyModelState(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelState/*0100000F*/ state) cil managed
  // SIG: 20 01 01 12 3D
  {
    // Method begins at RVA 0x31b4
    // Code size       31 (0x1f)
    .maxstack  2
    .locals /*11000001*/ init (object V_0)
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 6F   | (0A)00005C       */ callvirt   instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ValueProviderResult/*01000050*/ [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelState/*0100000F*/::get_Value() /* 0A00005C */
    IL_0006:  /* D0   | (01)00003F       */ ldtoken    [mscorlib/*23000001*/]System.String/*0100003F*/
    IL_000b:  /* 28   | (0A)000021       */ call       class [mscorlib/*23000001*/]System.Type/*01000003*/ [mscorlib/*23000001*/]System.Type/*01000003*/::GetTypeFromHandle(valuetype [mscorlib/*23000001*/]System.RuntimeTypeHandle/*01000036*/) /* 0A000021 */
    IL_0010:  /* 6F   | (0A)0000AA       */ callvirt   instance object [System.Web.Mvc/*23000003*/]System.Web.Mvc.ValueProviderResult/*01000050*/::ConvertTo(class [mscorlib/*23000001*/]System.Type/*01000003*/) /* 0A0000AA */
    IL_0015:  /* 0A   |                  */ stloc.0
    IL_0016:  /* 02   |                  */ ldarg.0
    IL_0017:  /* 06   |                  */ ldloc.0
    IL_0018:  /* 6F   | (0A)0000AB       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.Input`1/*02000020*/<!T>/*1B000008*/::Value(object) /* 0A0000AB */
    IL_001d:  /* 26   |                  */ pop
    IL_001e:  /* 2A   |                  */ ret
  } // end of method Input`1::ApplyModelState

} // end of class MvcContrib.FluentHtml.Elements.Input`1

.class /*02000021*/ public abstract auto ansi beforefieldinit MvcContrib.FluentHtml.Elements.ButtonBase`1<(class MvcContrib.FluentHtml.Elements.ButtonBase`1/*02000021*/<!T>/*1B000009*/) T>
       extends class MvcContrib.FluentHtml.Elements.Input`1/*02000020*/<!T>/*1B000008*/
{
  .method /*06000093*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(string text) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x31df
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 14   |                  */ ldnull
    IL_0003:  /* 28   | (0A)0000AC       */ call       instance void class MvcContrib.FluentHtml.Elements.ButtonBase`1/*02000021*/<!T>/*1B000009*/::.ctor(string,
                                                                                                                                                          class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 0A0000AC */
    IL_0008:  /* 2A   |                  */ ret
  } // end of method ButtonBase`1::.ctor

  .method /*06000094*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(string text,
                               class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> behaviors) cil managed
  // SIG: 20 02 01 0E 15 12 11 01 12 08
  {
    // Method begins at RVA 0x31e9
    // Code size       33 (0x21)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 72   | (70)00015B       */ ldstr      "button" /* 7000015B */
    IL_0006:  /* 03   |                  */ ldarg.1
    IL_0007:  /* 2C   | 08               */ brfalse.s  IL_0011

    IL_0009:  /* 03   |                  */ ldarg.1
    IL_000a:  /* 28   | (06)00010F       */ call       string MvcContrib.FluentHtml.StringExtensions/*0200003B*/::FormatAsHtmlName(string) /* 0600010F */
    IL_000f:  /* 2B   | 01               */ br.s       IL_0012

    IL_0011:  /* 14   |                  */ ldnull
    IL_0012:  /* 14   |                  */ ldnull
    IL_0013:  /* 04   |                  */ ldarg.2
    IL_0014:  /* 28   | (0A)0000AD       */ call       instance void class MvcContrib.FluentHtml.Elements.Input`1/*02000020*/<!T>/*1B000008*/::.ctor(string,
                                                                                                                                                     string,
                                                                                                                                                     class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                                                     class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 0A0000AD */
    IL_0019:  /* 02   |                  */ ldarg.0
    IL_001a:  /* 03   |                  */ ldarg.1
    IL_001b:  /* 7D   | (0A)0000A8       */ stfld      object class MvcContrib.FluentHtml.Elements.Input`1/*02000020*/<!T>/*1B000008*/::elementValue /* 0A0000A8 */
    IL_0020:  /* 2A   |                  */ ret
  } // end of method ButtonBase`1::.ctor

} // end of class MvcContrib.FluentHtml.Elements.ButtonBase`1

.class /*02000022*/ public auto ansi beforefieldinit MvcContrib.FluentHtml.Elements.Button
       extends class MvcContrib.FluentHtml.Elements.ButtonBase`1/*02000021*/<class MvcContrib.FluentHtml.Elements.Button/*02000022*/>/*1B00000A*/
{
  .method /*06000095*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string text) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x320b
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 28   | (0A)0000AE       */ call       instance void class MvcContrib.FluentHtml.Elements.ButtonBase`1/*02000021*/<class MvcContrib.FluentHtml.Elements.Button/*02000022*/>/*1B00000A*/::.ctor(string) /* 0A0000AE */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method Button::.ctor

  .method /*06000096*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string text,
                               class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> behaviors) cil managed
  // SIG: 20 02 01 0E 15 12 11 01 12 08
  {
    // Method begins at RVA 0x3214
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 04   |                  */ ldarg.2
    IL_0003:  /* 28   | (0A)0000AF       */ call       instance void class MvcContrib.FluentHtml.Elements.ButtonBase`1/*02000021*/<class MvcContrib.FluentHtml.Elements.Button/*02000022*/>/*1B00000A*/::.ctor(string,
                                                                                                                                                                                                               class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 0A0000AF */
    IL_0008:  /* 2A   |                  */ ret
  } // end of method Button::.ctor

} // end of class MvcContrib.FluentHtml.Elements.Button

.class /*02000023*/ public abstract auto ansi beforefieldinit MvcContrib.FluentHtml.Elements.CheckBoxBase`1<(class MvcContrib.FluentHtml.Elements.CheckBoxBase`1/*02000023*/<!T>/*1B00000B*/) T>
       extends class MvcContrib.FluentHtml.Elements.Input`1/*02000020*/<!T>/*1B000008*/
{
  .method /*06000097*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x321e
    // Code size       24 (0x18)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 72   | (70)000169       */ ldstr      "checkbox" /* 70000169 */
    IL_0006:  /* 03   |                  */ ldarg.1
    IL_0007:  /* 28   | (0A)0000B0       */ call       instance void class MvcContrib.FluentHtml.Elements.Input`1/*02000020*/<!T>/*1B000008*/::.ctor(string,
                                                                                                                                                     string) /* 0A0000B0 */
    IL_000c:  /* 02   |                  */ ldarg.0
    IL_000d:  /* 72   | (70)00017B       */ ldstr      "true" /* 7000017B */
    IL_0012:  /* 7D   | (0A)0000A8       */ stfld      object class MvcContrib.FluentHtml.Elements.Input`1/*02000020*/<!T>/*1B000008*/::elementValue /* 0A0000A8 */
    IL_0017:  /* 2A   |                  */ ret
  } // end of method CheckBoxBase`1::.ctor

  .method /*06000098*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(string name,
                               class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ forMember,
                               class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> behaviors) cil managed
  // SIG: 20 03 01 0E 12 39 15 12 11 01 12 08
  {
    // Method begins at RVA 0x3237
    // Code size       26 (0x1a)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 72   | (70)000169       */ ldstr      "checkbox" /* 70000169 */
    IL_0006:  /* 03   |                  */ ldarg.1
    IL_0007:  /* 04   |                  */ ldarg.2
    IL_0008:  /* 05   |                  */ ldarg.3
    IL_0009:  /* 28   | (0A)0000AD       */ call       instance void class MvcContrib.FluentHtml.Elements.Input`1/*02000020*/<!T>/*1B000008*/::.ctor(string,
                                                                                                                                                     string,
                                                                                                                                                     class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                                                     class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 0A0000AD */
    IL_000e:  /* 02   |                  */ ldarg.0
    IL_000f:  /* 72   | (70)00017B       */ ldstr      "true" /* 7000017B */
    IL_0014:  /* 7D   | (0A)0000A8       */ stfld      object class MvcContrib.FluentHtml.Elements.Input`1/*02000020*/<!T>/*1B000008*/::elementValue /* 0A0000A8 */
    IL_0019:  /* 2A   |                  */ ret
  } // end of method CheckBoxBase`1::.ctor

  .method /*06000099*/ public hidebysig newslot virtual 
          instance !T  Checked(bool 'value') cil managed
  // SIG: 20 01 13 00 02
  {
    // Method begins at RVA 0x3252
    // Code size       40 (0x28)
    .maxstack  8
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 2C   | 13               */ brfalse.s  IL_0016

    IL_0003:  /* 02   |                  */ ldarg.0
    IL_0004:  /* 72   | (70)000185       */ ldstr      "checked" /* 70000185 */
    IL_0009:  /* 72   | (70)000185       */ ldstr      "checked" /* 70000185 */
    IL_000e:  /* 6F   | (0A)0000A0       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::Attr(string,
                                                                                                                                                    object) /* 0A0000A0 */
    IL_0013:  /* 26   |                  */ pop
    IL_0014:  /* 2B   | 0B               */ br.s       IL_0021

    IL_0016:  /* 02   |                  */ ldarg.0
    IL_0017:  /* 72   | (70)000185       */ ldstr      "checked" /* 70000185 */
    IL_001c:  /* 6F   | (06)000049       */ callvirt   instance void MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::RemoveAttr(string) /* 06000049 */
    IL_0021:  /* 02   |                  */ ldarg.0
    IL_0022:  /* A5   | (1B)000040       */ unbox.any  !T/*1B000040*/
    IL_0027:  /* 2A   |                  */ ret
  } // end of method CheckBoxBase`1::Checked

  .method /*0600009A*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x327c
    // Code size       120 (0x78)
    .maxstack  2
    .locals /*11000018*/ init (string V_0,
             string V_1,
             string V_2)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)0000B1       */ call       instance string class MvcContrib.FluentHtml.Elements.CheckBoxBase`1/*02000023*/<!T>/*1B00000B*/::ToCheckBoxOnlyHtml() /* 0A0000B1 */
    IL_0006:  /* 0A   |                  */ stloc.0
    IL_0007:  /* 72   | (70)000195       */ ldstr      "_Hidden" /* 70000195 */
    IL_000c:  /* 0B   |                  */ stloc.1
    IL_000d:  /* 02   |                  */ ldarg.0
    IL_000e:  /* 6F   | (06)000045       */ callvirt   instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::get_Builder() /* 06000045 */
    IL_0013:  /* 6F   | (0A)000076       */ callvirt   instance class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<string,string> [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/::get_Attributes() /* 0A000076 */
    IL_0018:  /* 72   | (70)0000B3       */ ldstr      "id" /* 700000B3 */
    IL_001d:  /* 6F   | (0A)000094       */ callvirt   instance bool class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<string,string>/*1B000047*/::ContainsKey(!0) /* 0A000094 */
    IL_0022:  /* 2C   | 1C               */ brfalse.s  IL_0040

    IL_0024:  /* 02   |                  */ ldarg.0
    IL_0025:  /* 6F   | (06)000045       */ callvirt   instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::get_Builder() /* 06000045 */
    IL_002a:  /* 6F   | (0A)000076       */ callvirt   instance class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<string,string> [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/::get_Attributes() /* 0A000076 */
    IL_002f:  /* 72   | (70)0000B3       */ ldstr      "id" /* 700000B3 */
    IL_0034:  /* 6F   | (0A)000095       */ callvirt   instance !1 class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<string,string>/*1B000047*/::get_Item(!0) /* 0A000095 */
    IL_0039:  /* 07   |                  */ ldloc.1
    IL_003a:  /* 28   | (0A)000075       */ call       string [mscorlib/*23000001*/]System.String/*0100003F*/::Concat(string,
                                                                                                                      string) /* 0A000075 */
    IL_003f:  /* 0B   |                  */ stloc.1
    IL_0040:  /* 02   |                  */ ldarg.0
    IL_0041:  /* 7B   | (0A)000065       */ ldfld      class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::builder /* 0A000065 */
    IL_0046:  /* 6F   | (0A)000076       */ callvirt   instance class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<string,string> [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/::get_Attributes() /* 0A000076 */
    IL_004b:  /* 72   | (70)000091       */ ldstr      "name" /* 70000091 */
    IL_0050:  /* 6F   | (0A)000095       */ callvirt   instance !1 class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<string,string>/*1B000047*/::get_Item(!0) /* 0A000095 */
    IL_0055:  /* 73   | (06)0000D3       */ newobj     instance void MvcContrib.FluentHtml.Elements.Hidden/*0200002D*/::.ctor(string) /* 060000D3 */
    IL_005a:  /* 07   |                  */ ldloc.1
    IL_005b:  /* 6F   | (0A)0000B2       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<class MvcContrib.FluentHtml.Elements.Hidden/*0200002D*/>/*1B00004C*/::Id(string) /* 0A0000B2 */
    IL_0060:  /* 72   | (70)0001A5       */ ldstr      "false" /* 700001A5 */
    IL_0065:  /* 6F   | (0A)0000B3       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.Input`1/*02000020*/<class MvcContrib.FluentHtml.Elements.Hidden/*0200002D*/>/*1B00004D*/::Value(object) /* 0A0000B3 */
    IL_006a:  /* 6F   | (0A)00006E       */ callvirt   instance string [mscorlib/*23000001*/]System.Object/*01000001*/::ToString() /* 0A00006E */
    IL_006f:  /* 0C   |                  */ stloc.2
    IL_0070:  /* 06   |                  */ ldloc.0
    IL_0071:  /* 08   |                  */ ldloc.2
    IL_0072:  /* 28   | (0A)000075       */ call       string [mscorlib/*23000001*/]System.String/*0100003F*/::Concat(string,
                                                                                                                      string) /* 0A000075 */
    IL_0077:  /* 2A   |                  */ ret
  } // end of method CheckBoxBase`1::ToString

  .method /*0600009B*/ public hidebysig instance string 
          ToCheckBoxOnlyHtml() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x3300
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)0000B4       */ call       instance string class MvcContrib.FluentHtml.Elements.FormElement`1/*0200001F*/<!T>/*1B000007*/::ToString() /* 0A0000B4 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method CheckBoxBase`1::ToCheckBoxOnlyHtml

  .method /*0600009C*/ family hidebysig virtual 
          instance void  ApplyModelState(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelState/*0100000F*/ state) cil managed
  // SIG: 20 01 01 12 3D
  {
    // Method begins at RVA 0x3308
    // Code size       56 (0x38)
    .maxstack  2
    .locals /*11000019*/ init (valuetype [mscorlib/*23000001*/]System.Nullable`1/*0100004D*/<bool> V_0)
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 6F   | (0A)00005C       */ callvirt   instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ValueProviderResult/*01000050*/ [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelState/*0100000F*/::get_Value() /* 0A00005C */
    IL_0006:  /* D0   | (1B)00004E       */ ldtoken    valuetype [mscorlib/*23000001*/]System.Nullable`1/*0100004D*/<bool>/*1B00004E*/
    IL_000b:  /* 28   | (0A)000021       */ call       class [mscorlib/*23000001*/]System.Type/*01000003*/ [mscorlib/*23000001*/]System.Type/*01000003*/::GetTypeFromHandle(valuetype [mscorlib/*23000001*/]System.RuntimeTypeHandle/*01000036*/) /* 0A000021 */
    IL_0010:  /* 6F   | (0A)0000AA       */ callvirt   instance object [System.Web.Mvc/*23000003*/]System.Web.Mvc.ValueProviderResult/*01000050*/::ConvertTo(class [mscorlib/*23000001*/]System.Type/*01000003*/) /* 0A0000AA */
    IL_0015:  /* 75   | (1B)00004E       */ isinst     valuetype [mscorlib/*23000001*/]System.Nullable`1/*0100004D*/<bool>/*1B00004E*/
    IL_001a:  /* A5   | (1B)00004E       */ unbox.any  valuetype [mscorlib/*23000001*/]System.Nullable`1/*0100004D*/<bool>/*1B00004E*/
    IL_001f:  /* 0A   |                  */ stloc.0
    IL_0020:  /* 12   | 00               */ ldloca.s   V_0
    IL_0022:  /* 28   | (0A)0000B5       */ call       instance bool valuetype [mscorlib/*23000001*/]System.Nullable`1/*0100004D*/<bool>/*1B00004E*/::get_HasValue() /* 0A0000B5 */
    IL_0027:  /* 2C   | 0E               */ brfalse.s  IL_0037

    IL_0029:  /* 02   |                  */ ldarg.0
    IL_002a:  /* 12   | 00               */ ldloca.s   V_0
    IL_002c:  /* 28   | (0A)0000B6       */ call       instance !0 valuetype [mscorlib/*23000001*/]System.Nullable`1/*0100004D*/<bool>/*1B00004E*/::get_Value() /* 0A0000B6 */
    IL_0031:  /* 6F   | (0A)0000B7       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.CheckBoxBase`1/*02000023*/<!T>/*1B00000B*/::Checked(bool) /* 0A0000B7 */
    IL_0036:  /* 26   |                  */ pop
    IL_0037:  /* 2A   |                  */ ret
  } // end of method CheckBoxBase`1::ApplyModelState

} // end of class MvcContrib.FluentHtml.Elements.CheckBoxBase`1

.class /*02000024*/ public auto ansi beforefieldinit MvcContrib.FluentHtml.Elements.CheckBox
       extends class MvcContrib.FluentHtml.Elements.CheckBoxBase`1/*02000023*/<class MvcContrib.FluentHtml.Elements.CheckBox/*02000024*/>/*1B00000C*/
{
  .method /*0600009D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x334c
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 28   | (0A)0000B8       */ call       instance void class MvcContrib.FluentHtml.Elements.CheckBoxBase`1/*02000023*/<class MvcContrib.FluentHtml.Elements.CheckBox/*02000024*/>/*1B00000C*/::.ctor(string) /* 0A0000B8 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method CheckBox::.ctor

  .method /*0600009E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name,
                               class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ forMember,
                               class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> behaviors) cil managed
  // SIG: 20 03 01 0E 12 39 15 12 11 01 12 08
  {
    // Method begins at RVA 0x3355
    // Code size       10 (0xa)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 04   |                  */ ldarg.2
    IL_0003:  /* 05   |                  */ ldarg.3
    IL_0004:  /* 28   | (0A)0000B9       */ call       instance void class MvcContrib.FluentHtml.Elements.CheckBoxBase`1/*02000023*/<class MvcContrib.FluentHtml.Elements.CheckBox/*02000024*/>/*1B00000C*/::.ctor(string,
                                                                                                                                                                                                                   class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                                                                                                                   class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 0A0000B9 */
    IL_0009:  /* 2A   |                  */ ret
  } // end of method CheckBox::.ctor

} // end of class MvcContrib.FluentHtml.Elements.CheckBox

.class /*02000025*/ public abstract auto ansi beforefieldinit MvcContrib.FluentHtml.Elements.OptionsElementBase`1<(class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/) T>
       extends class MvcContrib.FluentHtml.Elements.FormElement`1/*0200001F*/<!T>/*1B000007*/
{
  .class /*0200005B*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClassa`1'<(class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/) T,TDataSource>
         extends [mscorlib/*23000001*/]System.Object/*01000001*/
  {
    .custom /*0C00003E:0A000013*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000032*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    .field /*0400007B*/ public class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!TDataSource,object> valueFieldSelector
    .field /*0400007C*/ public class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!TDataSource,object> textFieldSelector
    .method /*060001AF*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x351f
      // Code size       7 (0x7)
      .maxstack  8
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 28   | (0A)000012       */ call       instance void [mscorlib/*23000001*/]System.Object/*01000001*/::.ctor() /* 0A000012 */
      IL_0006:  /* 2A   |                  */ ret
    } // end of method '<>c__DisplayClassa`1'::.ctor

    .method /*060001B0*/ public hidebysig 
            instance object  '<Options>b__8'(object x) cil managed
    // SIG: 20 01 1C 1C
    {
      // Method begins at RVA 0x3527
      // Code size       18 (0x12)
      .maxstack  8
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 7B   | (0A)0000D5       */ ldfld      class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!1,object> class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*//'<>c__DisplayClassa`1'/*0200005B*/<!T,!TDataSource>/*1B000053*/::textFieldSelector /* 0A0000D5 */
      IL_0006:  /* 03   |                  */ ldarg.1
      IL_0007:  /* A5   | (1B)000054       */ unbox.any  !TDataSource/*1B000054*/
      IL_000c:  /* 6F   | (0A)0000D6       */ callvirt   instance !1 class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!TDataSource,object>/*1B000055*/::Invoke(!0) /* 0A0000D6 */
      IL_0011:  /* 2A   |                  */ ret
    } // end of method '<>c__DisplayClassa`1'::'<Options>b__8'

    .method /*060001B1*/ public hidebysig 
            instance object  '<Options>b__9'(object x) cil managed
    // SIG: 20 01 1C 1C
    {
      // Method begins at RVA 0x353a
      // Code size       18 (0x12)
      .maxstack  8
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 7B   | (0A)0000D7       */ ldfld      class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!1,object> class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*//'<>c__DisplayClassa`1'/*0200005B*/<!T,!TDataSource>/*1B000053*/::valueFieldSelector /* 0A0000D7 */
      IL_0006:  /* 03   |                  */ ldarg.1
      IL_0007:  /* A5   | (1B)000054       */ unbox.any  !TDataSource/*1B000054*/
      IL_000c:  /* 6F   | (0A)0000D6       */ callvirt   instance !1 class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!TDataSource,object>/*1B000055*/::Invoke(!0) /* 0A0000D6 */
      IL_0011:  /* 2A   |                  */ ret
    } // end of method '<>c__DisplayClassa`1'::'<Options>b__9'

  } // end of class '<>c__DisplayClassa`1'

  .class /*0200005C*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClasse'<(class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/) T>
         extends [mscorlib/*23000001*/]System.Object/*01000001*/
  {
    .custom /*0C00003F:0A000013*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000032*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    .field /*0400007D*/ public class [mscorlib/*23000001*/]System.Reflection.PropertyInfo/*01000041*/ valueProp
    .field /*0400007E*/ public class [mscorlib/*23000001*/]System.Reflection.PropertyInfo/*01000041*/ textProp
    .method /*060001B2*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x366c
      // Code size       7 (0x7)
      .maxstack  8
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 28   | (0A)000012       */ call       instance void [mscorlib/*23000001*/]System.Object/*01000001*/::.ctor() /* 0A000012 */
      IL_0006:  /* 2A   |                  */ ret
    } // end of method '<>c__DisplayClasse'::.ctor

    .method /*060001B3*/ public hidebysig 
            instance object  '<SetFieldExpressions>b__c'(object x) cil managed
    // SIG: 20 01 1C 1C
    {
      // Method begins at RVA 0x3674
      // Code size       14 (0xe)
      .maxstack  8
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 7B   | (0A)0000E7       */ ldfld      class [mscorlib/*23000001*/]System.Reflection.PropertyInfo/*01000041*/ class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*//'<>c__DisplayClasse'/*0200005C*/<!T>/*1B000059*/::textProp /* 0A0000E7 */
      IL_0006:  /* 03   |                  */ ldarg.1
      IL_0007:  /* 14   |                  */ ldnull
      IL_0008:  /* 6F   | (0A)0000E8       */ callvirt   instance object [mscorlib/*23000001*/]System.Reflection.PropertyInfo/*01000041*/::GetValue(object,
                                                                                                                                                    object[]) /* 0A0000E8 */
      IL_000d:  /* 2A   |                  */ ret
    } // end of method '<>c__DisplayClasse'::'<SetFieldExpressions>b__c'

    .method /*060001B4*/ public hidebysig 
            instance object  '<SetFieldExpressions>b__d'(object x) cil managed
    // SIG: 20 01 1C 1C
    {
      // Method begins at RVA 0x3683
      // Code size       14 (0xe)
      .maxstack  8
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 7B   | (0A)0000E9       */ ldfld      class [mscorlib/*23000001*/]System.Reflection.PropertyInfo/*01000041*/ class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*//'<>c__DisplayClasse'/*0200005C*/<!T>/*1B000059*/::valueProp /* 0A0000E9 */
      IL_0006:  /* 03   |                  */ ldarg.1
      IL_0007:  /* 14   |                  */ ldnull
      IL_0008:  /* 6F   | (0A)0000E8       */ callvirt   instance object [mscorlib/*23000001*/]System.Reflection.PropertyInfo/*01000041*/::GetValue(object,
                                                                                                                                                    object[]) /* 0A0000E8 */
      IL_000d:  /* 2A   |                  */ ret
    } // end of method '<>c__DisplayClasse'::'<SetFieldExpressions>b__d'

  } // end of class '<>c__DisplayClasse'

  .field /*04000023*/ family class [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000015*/ _options
  .field /*04000024*/ family class [System.Core/*23000002*/]System.Func`2/*0100000B*/<object,object> _textFieldSelector
  .field /*04000025*/ family class [System.Core/*23000002*/]System.Func`2/*0100000B*/<object,object> _valueFieldSelector
  .field /*04000026*/ family class [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000015*/ _selectedValues
  .field /*04000027*/ private static class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class [System.Web.Mvc/*23000003*/]System.Web.Mvc.SelectListItem/*01000017*/,bool> 'CS$<>9__CachedAnonymousMethodDelegate2'
  .custom /*0C00002F:0A000013*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000032*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
  .field /*04000028*/ private static class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class [System.Web.Mvc/*23000003*/]System.Web.Mvc.SelectListItem/*01000017*/,string> 'CS$<>9__CachedAnonymousMethodDelegate3'
  .custom /*0C000030:0A000013*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000032*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
  .method /*0600009F*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(string tag,
                               string name,
                               class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ forMember,
                               class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> behaviors) cil managed
  // SIG: 20 04 01 0E 0E 12 39 15 12 11 01 12 08
  {
    // Method begins at RVA 0x3360
    // Code size       12 (0xc)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 04   |                  */ ldarg.2
    IL_0003:  /* 05   |                  */ ldarg.3
    IL_0004:  /* 0E   | 04               */ ldarg.s    behaviors
    IL_0006:  /* 28   | (0A)0000A7       */ call       instance void class MvcContrib.FluentHtml.Elements.FormElement`1/*0200001F*/<!T>/*1B000007*/::.ctor(string,
                                                                                                                                                           string,
                                                                                                                                                           class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                                                           class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 0A0000A7 */
    IL_000b:  /* 2A   |                  */ ret
  } // end of method OptionsElementBase`1::.ctor

  .method /*060000A0*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(string tag,
                               string name) cil managed
  // SIG: 20 02 01 0E 0E
  {
    // Method begins at RVA 0x336d
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 04   |                  */ ldarg.2
    IL_0003:  /* 28   | (0A)0000A6       */ call       instance void class MvcContrib.FluentHtml.Elements.FormElement`1/*0200001F*/<!T>/*1B000007*/::.ctor(string,
                                                                                                                                                           string) /* 0A0000A6 */
    IL_0008:  /* 2A   |                  */ ret
  } // end of method OptionsElementBase`1::.ctor

  .method /*060000A1*/ public hidebysig specialname 
          instance class [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000015*/ 
          get_SelectedValues() cil managed
  // SIG: 20 00 12 55
  {
    // Method begins at RVA 0x3377
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (0A)0000BA       */ ldfld      class [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000015*/ class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/::_selectedValues /* 0A0000BA */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method OptionsElementBase`1::get_SelectedValues

  .method /*060000A2*/ public hidebysig newslot virtual 
          instance !T  Options(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.MultiSelectList/*01000016*/ 'value') cil managed
  // SIG: 20 01 13 00 12 59
  {
    // Method begins at RVA 0x337f
    // Code size       60 (0x3c)
    .maxstack  8
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 2C   | 32               */ brfalse.s  IL_0035

    IL_0003:  /* 02   |                  */ ldarg.0
    IL_0004:  /* 03   |                  */ ldarg.1
    IL_0005:  /* 6F   | (0A)0000BB       */ callvirt   instance class [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000015*/ [System.Web.Mvc/*23000003*/]System.Web.Mvc.MultiSelectList/*01000016*/::get_Items() /* 0A0000BB */
    IL_000a:  /* 7D   | (0A)0000BC       */ stfld      class [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000015*/ class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/::_options /* 0A0000BC */
    IL_000f:  /* 02   |                  */ ldarg.0
    IL_0010:  /* 03   |                  */ ldarg.1
    IL_0011:  /* 6F   | (0A)0000BD       */ callvirt   instance string [System.Web.Mvc/*23000003*/]System.Web.Mvc.MultiSelectList/*01000016*/::get_DataValueField() /* 0A0000BD */
    IL_0016:  /* 03   |                  */ ldarg.1
    IL_0017:  /* 6F   | (0A)0000BE       */ callvirt   instance string [System.Web.Mvc/*23000003*/]System.Web.Mvc.MultiSelectList/*01000016*/::get_DataTextField() /* 0A0000BE */
    IL_001c:  /* 28   | (0A)0000BF       */ call       instance void class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/::SetFieldExpressions(string,
                                                                                                                                                                                string) /* 0A0000BF */
    IL_0021:  /* 03   |                  */ ldarg.1
    IL_0022:  /* 6F   | (0A)0000C0       */ callvirt   instance class [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000015*/ [System.Web.Mvc/*23000003*/]System.Web.Mvc.MultiSelectList/*01000016*/::get_SelectedValues() /* 0A0000C0 */
    IL_0027:  /* 2C   | 0C               */ brfalse.s  IL_0035

    IL_0029:  /* 02   |                  */ ldarg.0
    IL_002a:  /* 03   |                  */ ldarg.1
    IL_002b:  /* 6F   | (0A)0000C0       */ callvirt   instance class [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000015*/ [System.Web.Mvc/*23000003*/]System.Web.Mvc.MultiSelectList/*01000016*/::get_SelectedValues() /* 0A0000C0 */
    IL_0030:  /* 7D   | (0A)0000BA       */ stfld      class [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000015*/ class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/::_selectedValues /* 0A0000BA */
    IL_0035:  /* 02   |                  */ ldarg.0
    IL_0036:  /* A5   | (1B)000040       */ unbox.any  !T/*1B000040*/
    IL_003b:  /* 2A   |                  */ ret
  } // end of method OptionsElementBase`1::Options

  .method /*060000A3*/ public hidebysig newslot virtual 
          instance !T  Options<TKey,TValue>(class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<!!TKey,!!TValue> 'value') cil managed
  // SIG: 30 02 01 13 00 15 12 51 02 1E 00 1E 01
  {
    // Method begins at RVA 0x33bc
    // Code size       30 (0x1e)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (0A)0000BC       */ stfld      class [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000015*/ class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/::_options /* 0A0000BC */
    IL_0007:  /* 02   |                  */ ldarg.0
    IL_0008:  /* 72   | (70)0001B1       */ ldstr      "Key" /* 700001B1 */
    IL_000d:  /* 72   | (70)0001B9       */ ldstr      "Value" /* 700001B9 */
    IL_0012:  /* 28   | (0A)0000BF       */ call       instance void class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/::SetFieldExpressions(string,
                                                                                                                                                                                string) /* 0A0000BF */
    IL_0017:  /* 02   |                  */ ldarg.0
    IL_0018:  /* A5   | (1B)000040       */ unbox.any  !T/*1B000040*/
    IL_001d:  /* 2A   |                  */ ret
  } // end of method OptionsElementBase`1::Options

  .method /*060000A4*/ public hidebysig newslot virtual 
          instance !T  Options(class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class [System.Web.Mvc/*23000003*/]System.Web.Mvc.SelectListItem/*01000017*/> 'value') cil managed
  // SIG: 20 01 13 00 15 12 11 01 12 5D
  {
    // Method begins at RVA 0x33ec
    // Code size       113 (0x71)
    .maxstack  5
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (0A)0000BC       */ stfld      class [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000015*/ class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/::_options /* 0A0000BC */
    IL_0007:  /* 02   |                  */ ldarg.0
    IL_0008:  /* 72   | (70)0001B9       */ ldstr      "Value" /* 700001B9 */
    IL_000d:  /* 72   | (70)0001C5       */ ldstr      "Text" /* 700001C5 */
    IL_0012:  /* 28   | (0A)0000BF       */ call       instance void class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/::SetFieldExpressions(string,
                                                                                                                                                                                string) /* 0A0000BF */
    IL_0017:  /* 03   |                  */ ldarg.1
    IL_0018:  /* 2C   | 50               */ brfalse.s  IL_006a

    IL_001a:  /* 02   |                  */ ldarg.0
    IL_001b:  /* 03   |                  */ ldarg.1
    IL_001c:  /* 7E   | (0A)0000C3       */ ldsfld     class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class [System.Web.Mvc/*23000003*/]System.Web.Mvc.SelectListItem/*01000017*/,bool> class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/::'CS$<>9__CachedAnonymousMethodDelegate2' /* 0A0000C3 */
    IL_0021:  /* 2D   | 11               */ brtrue.s   IL_0034

    IL_0023:  /* 14   |                  */ ldnull
    IL_0024:  /* FE06 | (0A)0000C4       */ ldftn      bool class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/::'<Options>b__0'(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.SelectListItem/*01000017*/) /* 0A0000C4 */
    IL_002a:  /* 73   | (0A)0000C5       */ newobj     instance void class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class [System.Web.Mvc/*23000003*/]System.Web.Mvc.SelectListItem/*01000017*/,bool>/*1B00004F*/::.ctor(object,
                                                                                                                                                                                                                                   native int) /* 0A0000C5 */
    IL_002f:  /* 80   | (0A)0000C3       */ stsfld     class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class [System.Web.Mvc/*23000003*/]System.Web.Mvc.SelectListItem/*01000017*/,bool> class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/::'CS$<>9__CachedAnonymousMethodDelegate2' /* 0A0000C3 */
    IL_0034:  /* 7E   | (0A)0000C3       */ ldsfld     class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class [System.Web.Mvc/*23000003*/]System.Web.Mvc.SelectListItem/*01000017*/,bool> class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/::'CS$<>9__CachedAnonymousMethodDelegate2' /* 0A0000C3 */
    IL_0039:  /* 28   | (2B)00000D       */ call       class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<!!0> [System.Core/*23000002*/]System.Linq.Enumerable/*01000037*/::Where<class [System.Web.Mvc/*23000003*/]System.Web.Mvc.SelectListItem/*01000017*/>(class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<!!0>,
                                                                                                                                                                                                                                                                                             class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,bool>) /* 2B00000D */
    IL_003e:  /* 7E   | (0A)0000C6       */ ldsfld     class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class [System.Web.Mvc/*23000003*/]System.Web.Mvc.SelectListItem/*01000017*/,string> class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/::'CS$<>9__CachedAnonymousMethodDelegate3' /* 0A0000C6 */
    IL_0043:  /* 2D   | 11               */ brtrue.s   IL_0056

    IL_0045:  /* 14   |                  */ ldnull
    IL_0046:  /* FE06 | (0A)0000C7       */ ldftn      string class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/::'<Options>b__1'(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.SelectListItem/*01000017*/) /* 0A0000C7 */
    IL_004c:  /* 73   | (0A)0000C8       */ newobj     instance void class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class [System.Web.Mvc/*23000003*/]System.Web.Mvc.SelectListItem/*01000017*/,string>/*1B000050*/::.ctor(object,
                                                                                                                                                                                                                                     native int) /* 0A0000C8 */
    IL_0051:  /* 80   | (0A)0000C6       */ stsfld     class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class [System.Web.Mvc/*23000003*/]System.Web.Mvc.SelectListItem/*01000017*/,string> class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/::'CS$<>9__CachedAnonymousMethodDelegate3' /* 0A0000C6 */
    IL_0056:  /* 7E   | (0A)0000C6       */ ldsfld     class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class [System.Web.Mvc/*23000003*/]System.Web.Mvc.SelectListItem/*01000017*/,string> class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/::'CS$<>9__CachedAnonymousMethodDelegate3' /* 0A0000C6 */
    IL_005b:  /* 28   | (2B)00000E       */ call       class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<!!1> [System.Core/*23000002*/]System.Linq.Enumerable/*01000037*/::Select<class [System.Web.Mvc/*23000003*/]System.Web.Mvc.SelectListItem/*01000017*/,string>(class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<!!0>,
                                                                                                                                                                                                                                                                                                     class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,!!1>) /* 2B00000E */
    IL_0060:  /* 28   | (2B)00000F       */ call       class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000021*/<!!0> [System.Core/*23000002*/]System.Linq.Enumerable/*01000037*/::ToList<string>(class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<!!0>) /* 2B00000F */
    IL_0065:  /* 7D   | (0A)0000BA       */ stfld      class [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000015*/ class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/::_selectedValues /* 0A0000BA */
    IL_006a:  /* 02   |                  */ ldarg.0
    IL_006b:  /* A5   | (1B)000040       */ unbox.any  !T/*1B000040*/
    IL_0070:  /* 2A   |                  */ ret
  } // end of method OptionsElementBase`1::Options

  .method /*060000A5*/ public hidebysig newslot virtual 
          instance !T  Options(class [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000015*/ values,
                               string valueField,
                               string textField) cil managed
  // SIG: 20 03 13 00 12 55 0E 0E
  {
    // Method begins at RVA 0x3469
    // Code size       22 (0x16)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (0A)0000BC       */ stfld      class [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000015*/ class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/::_options /* 0A0000BC */
    IL_0007:  /* 02   |                  */ ldarg.0
    IL_0008:  /* 04   |                  */ ldarg.2
    IL_0009:  /* 05   |                  */ ldarg.3
    IL_000a:  /* 28   | (0A)0000BF       */ call       instance void class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/::SetFieldExpressions(string,
                                                                                                                                                                                string) /* 0A0000BF */
    IL_000f:  /* 02   |                  */ ldarg.0
    IL_0010:  /* A5   | (1B)000040       */ unbox.any  !T/*1B000040*/
    IL_0015:  /* 2A   |                  */ ret
  } // end of method OptionsElementBase`1::Options

  .method /*060000A6*/ public hidebysig newslot virtual 
          instance !T  Options<TValue>(class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<!!TValue> 'value') cil managed
  // SIG: 30 01 01 13 00 15 12 11 01 1E 00
  {
    // Method begins at RVA 0x34b8
    // Code size       91 (0x5b)
    .maxstack  7
    IL_0000:  /* D0   | (1B)000031       */ ldtoken    !!TValue/*1B000031*/
    IL_0005:  /* 28   | (0A)000021       */ call       class [mscorlib/*23000001*/]System.Type/*01000003*/ [mscorlib/*23000001*/]System.Type/*01000003*/::GetTypeFromHandle(valuetype [mscorlib/*23000001*/]System.RuntimeTypeHandle/*01000036*/) /* 0A000021 */
    IL_000a:  /* 28   | (0A)0000CC       */ call       instance bool [mscorlib/*23000001*/]System.Type/*01000003*/::get_IsEnum() /* 0A0000CC */
    IL_000f:  /* 2D   | 25               */ brtrue.s   IL_0036

    IL_0011:  /* 02   |                  */ ldarg.0
    IL_0012:  /* 03   |                  */ ldarg.1
    IL_0013:  /* 14   |                  */ ldnull
    IL_0014:  /* FE06 | (2B)000010       */ ldftn      !!0 class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/::'<Options>b__6'<!!0>(!!0) /* 2B000010 */
    IL_001a:  /* 73   | (0A)0000CE       */ newobj     instance void class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!TValue,!!TValue>/*1B000051*/::.ctor(object,
                                                                                                                                                                    native int) /* 0A0000CE */
    IL_001f:  /* 14   |                  */ ldnull
    IL_0020:  /* FE06 | (2B)000011       */ ldftn      !!0 class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/::'<Options>b__7'<!!0>(!!0) /* 2B000011 */
    IL_0026:  /* 73   | (0A)0000CE       */ newobj     instance void class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!TValue,!!TValue>/*1B000051*/::.ctor(object,
                                                                                                                                                                    native int) /* 0A0000CE */
    IL_002b:  /* 28   | (2B)000012       */ call       class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000002*/<!!1,!!2> [System.Core/*23000002*/]System.Linq.Enumerable/*01000037*/::ToDictionary<!!0,!!0,!!0>(class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<!!0>,
                                                                                                                                                                                                                                       class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,!!1>,
                                                                                                                                                                                                                                       class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,!!2>) /* 2B000012 */
    IL_0030:  /* 6F   | (2B)000013       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/::Options<!!0,!!0>(class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<!!0,!!1>) /* 2B000013 */
    IL_0035:  /* 2A   |                  */ ret

    IL_0036:  /* 02   |                  */ ldarg.0
    IL_0037:  /* 03   |                  */ ldarg.1
    IL_0038:  /* 14   |                  */ ldnull
    IL_0039:  /* FE06 | (2B)000014       */ ldftn      string class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/::'<Options>b__4'<!!0>(!!0) /* 2B000014 */
    IL_003f:  /* 73   | (0A)0000D3       */ newobj     instance void class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!TValue,string>/*1B000052*/::.ctor(object,
                                                                                                                                                                  native int) /* 0A0000D3 */
    IL_0044:  /* 14   |                  */ ldnull
    IL_0045:  /* FE06 | (2B)000015       */ ldftn      string class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/::'<Options>b__5'<!!0>(!!0) /* 2B000015 */
    IL_004b:  /* 73   | (0A)0000D3       */ newobj     instance void class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!TValue,string>/*1B000052*/::.ctor(object,
                                                                                                                                                                  native int) /* 0A0000D3 */
    IL_0050:  /* 28   | (2B)000016       */ call       class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000002*/<!!1,!!2> [System.Core/*23000002*/]System.Linq.Enumerable/*01000037*/::ToDictionary<!!0,string,string>(class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<!!0>,
                                                                                                                                                                                                                                             class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,!!1>,
                                                                                                                                                                                                                                             class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,!!2>) /* 2B000016 */
    IL_0055:  /* 6F   | (2B)000017       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/::Options<string,string>(class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<!!0,!!1>) /* 2B000017 */
    IL_005a:  /* 2A   |                  */ ret
  } // end of method OptionsElementBase`1::Options

  .method /*060000A7*/ public hidebysig newslot virtual 
          instance !T  Options<TDataSource>(class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<!!TDataSource> values,
                                            class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!TDataSource,object> valueFieldSelector,
                                            class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!TDataSource,object> textFieldSelector) cil managed
  // SIG: 30 01 03 13 00 15 12 11 01 1E 00 15 12 2D 02 1E 00 1C 15 12 2D 02 1E 00 1C
  {
    // Method begins at RVA 0x3550
    // Code size       108 (0x6c)
    .maxstack  4
    .locals /*1100001A*/ init (class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*//'<>c__DisplayClassa`1'/*0200005B*/<!T,!!TDataSource> V_0)
    IL_0000:  /* 73   | (0A)0000D8       */ newobj     instance void class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*//'<>c__DisplayClassa`1'/*0200005B*/<!T,!!TDataSource>/*1B000056*/::.ctor() /* 0A0000D8 */
    IL_0005:  /* 0A   |                  */ stloc.0
    IL_0006:  /* 06   |                  */ ldloc.0
    IL_0007:  /* 04   |                  */ ldarg.2
    IL_0008:  /* 7D   | (0A)0000D9       */ stfld      class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!1,object> class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*//'<>c__DisplayClassa`1'/*0200005B*/<!T,!!TDataSource>/*1B000056*/::valueFieldSelector /* 0A0000D9 */
    IL_000d:  /* 06   |                  */ ldloc.0
    IL_000e:  /* 05   |                  */ ldarg.3
    IL_000f:  /* 7D   | (0A)0000DA       */ stfld      class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!1,object> class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*//'<>c__DisplayClassa`1'/*0200005B*/<!T,!!TDataSource>/*1B000056*/::textFieldSelector /* 0A0000DA */
    IL_0014:  /* 06   |                  */ ldloc.0
    IL_0015:  /* 7B   | (0A)0000D9       */ ldfld      class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!1,object> class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*//'<>c__DisplayClassa`1'/*0200005B*/<!T,!!TDataSource>/*1B000056*/::valueFieldSelector /* 0A0000D9 */
    IL_001a:  /* 2D   | 0B               */ brtrue.s   IL_0027

    IL_001c:  /* 72   | (70)0001CF       */ ldstr      "valueFieldSelector" /* 700001CF */
    IL_0021:  /* 73   | (0A)0000DB       */ newobj     instance void [mscorlib/*23000001*/]System.ArgumentNullException/*0100005B*/::.ctor(string) /* 0A0000DB */
    IL_0026:  /* 7A   |                  */ throw

    IL_0027:  /* 06   |                  */ ldloc.0
    IL_0028:  /* 7B   | (0A)0000DA       */ ldfld      class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!1,object> class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*//'<>c__DisplayClassa`1'/*0200005B*/<!T,!!TDataSource>/*1B000056*/::textFieldSelector /* 0A0000DA */
    IL_002d:  /* 2D   | 0B               */ brtrue.s   IL_003a

    IL_002f:  /* 72   | (70)0001F5       */ ldstr      "textFieldSelector" /* 700001F5 */
    IL_0034:  /* 73   | (0A)0000DB       */ newobj     instance void [mscorlib/*23000001*/]System.ArgumentNullException/*0100005B*/::.ctor(string) /* 0A0000DB */
    IL_0039:  /* 7A   |                  */ throw

    IL_003a:  /* 02   |                  */ ldarg.0
    IL_003b:  /* 03   |                  */ ldarg.1
    IL_003c:  /* 7D   | (0A)0000BC       */ stfld      class [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000015*/ class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/::_options /* 0A0000BC */
    IL_0041:  /* 02   |                  */ ldarg.0
    IL_0042:  /* 06   |                  */ ldloc.0
    IL_0043:  /* FE06 | (0A)0000DC       */ ldftn      instance object class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*//'<>c__DisplayClassa`1'/*0200005B*/<!T,!!TDataSource>/*1B000056*/::'<Options>b__8'(object) /* 0A0000DC */
    IL_0049:  /* 73   | (0A)0000DD       */ newobj     instance void class [System.Core/*23000002*/]System.Func`2/*0100000B*/<object,object>/*1B000057*/::.ctor(object,
                                                                                                                                                                native int) /* 0A0000DD */
    IL_004e:  /* 7D   | (0A)0000DE       */ stfld      class [System.Core/*23000002*/]System.Func`2/*0100000B*/<object,object> class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/::_textFieldSelector /* 0A0000DE */
    IL_0053:  /* 02   |                  */ ldarg.0
    IL_0054:  /* 06   |                  */ ldloc.0
    IL_0055:  /* FE06 | (0A)0000DF       */ ldftn      instance object class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*//'<>c__DisplayClassa`1'/*0200005B*/<!T,!!TDataSource>/*1B000056*/::'<Options>b__9'(object) /* 0A0000DF */
    IL_005b:  /* 73   | (0A)0000DD       */ newobj     instance void class [System.Core/*23000002*/]System.Func`2/*0100000B*/<object,object>/*1B000057*/::.ctor(object,
                                                                                                                                                                native int) /* 0A0000DD */
    IL_0060:  /* 7D   | (0A)0000E0       */ stfld      class [System.Core/*23000002*/]System.Func`2/*0100000B*/<object,object> class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/::_valueFieldSelector /* 0A0000E0 */
    IL_0065:  /* 02   |                  */ ldarg.0
    IL_0066:  /* A5   | (1B)000040       */ unbox.any  !T/*1B000040*/
    IL_006b:  /* 2A   |                  */ ret
  } // end of method OptionsElementBase`1::Options

  .method /*060000A8*/ public hidebysig newslot virtual 
          instance !T  Options<valuetype .ctor ([mscorlib/*23000001*/]System.ValueType/*01000018*/) TEnum>() cil managed
  // SIG: 30 01 00 13 00
  {
    // Method begins at RVA 0x35c8
    // Code size       136 (0x88)
    .maxstack  3
    .locals /*1100001B*/ init (class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000002*/<string,string> V_0,
             class [mscorlib/*23000001*/]System.Array/*0100005E*/ V_1,
             object V_2,
             class [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000034*/ V_3,
             int32 V_4,
             class [mscorlib/*23000001*/]System.IDisposable/*01000035*/ V_5)
    IL_0000:  /* D0   | (1B)000031       */ ldtoken    !!TEnum/*1B000031*/
    IL_0005:  /* 28   | (0A)000021       */ call       class [mscorlib/*23000001*/]System.Type/*01000003*/ [mscorlib/*23000001*/]System.Type/*01000003*/::GetTypeFromHandle(valuetype [mscorlib/*23000001*/]System.RuntimeTypeHandle/*01000036*/) /* 0A000021 */
    IL_000a:  /* 28   | (0A)0000CC       */ call       instance bool [mscorlib/*23000001*/]System.Type/*01000003*/::get_IsEnum() /* 0A0000CC */
    IL_000f:  /* 2D   | 10               */ brtrue.s   IL_0021

    IL_0011:  /* 72   | (70)000219       */ ldstr      "The generic parameter must be an enum" /* 70000219 */
    IL_0016:  /* 72   | (70)000265       */ ldstr      "TEnum" /* 70000265 */
    IL_001b:  /* 73   | (0A)0000E1       */ newobj     instance void [mscorlib/*23000001*/]System.ArgumentException/*0100005C*/::.ctor(string,
                                                                                                                                       string) /* 0A0000E1 */
    IL_0020:  /* 7A   |                  */ throw

    IL_0021:  /* 73   | (0A)0000E2       */ newobj     instance void class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000002*/<string,string>/*1B000058*/::.ctor() /* 0A0000E2 */
    IL_0026:  /* 0A   |                  */ stloc.0
    IL_0027:  /* D0   | (1B)000031       */ ldtoken    !!TEnum/*1B000031*/
    IL_002c:  /* 28   | (0A)000021       */ call       class [mscorlib/*23000001*/]System.Type/*01000003*/ [mscorlib/*23000001*/]System.Type/*01000003*/::GetTypeFromHandle(valuetype [mscorlib/*23000001*/]System.RuntimeTypeHandle/*01000036*/) /* 0A000021 */
    IL_0031:  /* 28   | (0A)0000E3       */ call       class [mscorlib/*23000001*/]System.Array/*0100005E*/ [mscorlib/*23000001*/]System.Enum/*0100005D*/::GetValues(class [mscorlib/*23000001*/]System.Type/*01000003*/) /* 0A0000E3 */
    IL_0036:  /* 0B   |                  */ stloc.1
    IL_0037:  /* 07   |                  */ ldloc.1
    IL_0038:  /* 6F   | (0A)0000E4       */ callvirt   instance class [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000034*/ [mscorlib/*23000001*/]System.Array/*0100005E*/::GetEnumerator() /* 0A0000E4 */
    IL_003d:  /* 0D   |                  */ stloc.3
    .try
    {
      IL_003e:  /* 2B   | 22               */ br.s       IL_0062

      IL_0040:  /* 09   |                  */ ldloc.3
      IL_0041:  /* 6F   | (0A)0000E5       */ callvirt   instance object [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000034*/::get_Current() /* 0A0000E5 */
      IL_0046:  /* 0C   |                  */ stloc.2
      IL_0047:  /* 06   |                  */ ldloc.0
      IL_0048:  /* 08   |                  */ ldloc.2
      IL_0049:  /* 28   | (0A)0000CA       */ call       int32 [mscorlib/*23000001*/]System.Convert/*0100005A*/::ToInt32(object) /* 0A0000CA */
      IL_004e:  /* 13   | 04               */ stloc.s    V_4
      IL_0050:  /* 12   | 04               */ ldloca.s   V_4
      IL_0052:  /* 28   | (0A)0000CB       */ call       instance string [mscorlib/*23000001*/]System.Int32/*01000044*/::ToString() /* 0A0000CB */
      IL_0057:  /* 08   |                  */ ldloc.2
      IL_0058:  /* 6F   | (0A)00006E       */ callvirt   instance string [mscorlib/*23000001*/]System.Object/*01000001*/::ToString() /* 0A00006E */
      IL_005d:  /* 6F   | (0A)0000E6       */ callvirt   instance void class [mscorlib/*23000001*/]System.Collections.Generic.Dictionary`2/*01000002*/<string,string>/*1B000058*/::Add(!0,
                                                                                                                                                                                       !1) /* 0A0000E6 */
      IL_0062:  /* 09   |                  */ ldloc.3
      IL_0063:  /* 6F   | (0A)00001A       */ callvirt   instance bool [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000034*/::MoveNext() /* 0A00001A */
      IL_0068:  /* 2D   | D6               */ brtrue.s   IL_0040

      IL_006a:  /* DE   | 14               */ leave.s    IL_0080

    }  // end .try
    finally
    {
      IL_006c:  /* 09   |                  */ ldloc.3
      IL_006d:  /* 75   | (01)000035       */ isinst     [mscorlib/*23000001*/]System.IDisposable/*01000035*/
      IL_0072:  /* 13   | 05               */ stloc.s    V_5
      IL_0074:  /* 11   | 05               */ ldloc.s    V_5
      IL_0076:  /* 2C   | 07               */ brfalse.s  IL_007f

      IL_0078:  /* 11   | 05               */ ldloc.s    V_5
      IL_007a:  /* 6F   | (0A)00001B       */ callvirt   instance void [mscorlib/*23000001*/]System.IDisposable/*01000035*/::Dispose() /* 0A00001B */
      IL_007f:  /* DC   |                  */ endfinally
    }  // end handler
    // HEX: 02 00 3E 00 2E 6C 00 14 00 00 00 00
    IL_0080:  /* 02   |                  */ ldarg.0
    IL_0081:  /* 06   |                  */ ldloc.0
    IL_0082:  /* 6F   | (2B)000017       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/::Options<string,string>(class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<!!0,!!1>) /* 2B000017 */
    IL_0087:  /* 2A   |                  */ ret
  } // end of method OptionsElementBase`1::Options

  .method /*060000A9*/ family hidebysig instance void 
          SetFieldExpressions(string dataValueField,
                              string dataTextField) cil managed
  // SIG: 20 02 01 0E 0E
  {
    // Method begins at RVA 0x3694
    // Code size       190 (0xbe)
    .maxstack  4
    .locals /*1100001C*/ init (class [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000034*/ V_0,
             class [mscorlib/*23000001*/]System.Type/*01000003*/ V_1,
             class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*//'<>c__DisplayClasse'/*0200005C*/<!T> V_2)
    IL_0000:  /* 73   | (0A)0000EA       */ newobj     instance void class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*//'<>c__DisplayClasse'/*0200005C*/<!T>/*1B000059*/::.ctor() /* 0A0000EA */
    IL_0005:  /* 0C   |                  */ stloc.2
    IL_0006:  /* 03   |                  */ ldarg.1
    IL_0007:  /* 2D   | 0B               */ brtrue.s   IL_0014

    IL_0009:  /* 72   | (70)000271       */ ldstr      "dataValueField" /* 70000271 */
    IL_000e:  /* 73   | (0A)0000DB       */ newobj     instance void [mscorlib/*23000001*/]System.ArgumentNullException/*0100005B*/::.ctor(string) /* 0A0000DB */
    IL_0013:  /* 7A   |                  */ throw

    IL_0014:  /* 04   |                  */ ldarg.2
    IL_0015:  /* 2D   | 0B               */ brtrue.s   IL_0022

    IL_0017:  /* 72   | (70)00028F       */ ldstr      "dataTextField" /* 7000028F */
    IL_001c:  /* 73   | (0A)0000DB       */ newobj     instance void [mscorlib/*23000001*/]System.ArgumentNullException/*0100005B*/::.ctor(string) /* 0A0000DB */
    IL_0021:  /* 7A   |                  */ throw

    IL_0022:  /* 02   |                  */ ldarg.0
    IL_0023:  /* 7B   | (0A)0000BC       */ ldfld      class [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000015*/ class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/::_options /* 0A0000BC */
    IL_0028:  /* 6F   | (0A)0000EB       */ callvirt   instance class [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000034*/ [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000015*/::GetEnumerator() /* 0A0000EB */
    IL_002d:  /* 0A   |                  */ stloc.0
    IL_002e:  /* 06   |                  */ ldloc.0
    IL_002f:  /* 6F   | (0A)00001A       */ callvirt   instance bool [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000034*/::MoveNext() /* 0A00001A */
    IL_0034:  /* 2D   | 01               */ brtrue.s   IL_0037

    IL_0036:  /* 2A   |                  */ ret

    IL_0037:  /* 06   |                  */ ldloc.0
    IL_0038:  /* 6F   | (0A)0000E5       */ callvirt   instance object [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000034*/::get_Current() /* 0A0000E5 */
    IL_003d:  /* 6F   | (0A)000016       */ callvirt   instance class [mscorlib/*23000001*/]System.Type/*01000003*/ [mscorlib/*23000001*/]System.Object/*01000001*/::GetType() /* 0A000016 */
    IL_0042:  /* 0B   |                  */ stloc.1
    IL_0043:  /* 08   |                  */ ldloc.2
    IL_0044:  /* 07   |                  */ ldloc.1
    IL_0045:  /* 03   |                  */ ldarg.1
    IL_0046:  /* 6F   | (0A)0000EC       */ callvirt   instance class [mscorlib/*23000001*/]System.Reflection.PropertyInfo/*01000041*/ [mscorlib/*23000001*/]System.Type/*01000003*/::GetProperty(string) /* 0A0000EC */
    IL_004b:  /* 7D   | (0A)0000E9       */ stfld      class [mscorlib/*23000001*/]System.Reflection.PropertyInfo/*01000041*/ class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*//'<>c__DisplayClasse'/*0200005C*/<!T>/*1B000059*/::valueProp /* 0A0000E9 */
    IL_0050:  /* 08   |                  */ ldloc.2
    IL_0051:  /* 7B   | (0A)0000E9       */ ldfld      class [mscorlib/*23000001*/]System.Reflection.PropertyInfo/*01000041*/ class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*//'<>c__DisplayClasse'/*0200005C*/<!T>/*1B000059*/::valueProp /* 0A0000E9 */
    IL_0056:  /* 2D   | 16               */ brtrue.s   IL_006e

    IL_0058:  /* 72   | (70)0002AB       */ ldstr      "The option list does not contain the specified val"
    + "ue property: {0}" /* 700002AB */
    IL_005d:  /* 03   |                  */ ldarg.1
    IL_005e:  /* 28   | (0A)000087       */ call       string [mscorlib/*23000001*/]System.String/*0100003F*/::Format(string,
                                                                                                                      object) /* 0A000087 */
    IL_0063:  /* 72   | (70)000271       */ ldstr      "dataValueField" /* 70000271 */
    IL_0068:  /* 73   | (0A)0000E1       */ newobj     instance void [mscorlib/*23000001*/]System.ArgumentException/*0100005C*/::.ctor(string,
                                                                                                                                       string) /* 0A0000E1 */
    IL_006d:  /* 7A   |                  */ throw

    IL_006e:  /* 08   |                  */ ldloc.2
    IL_006f:  /* 07   |                  */ ldloc.1
    IL_0070:  /* 04   |                  */ ldarg.2
    IL_0071:  /* 6F   | (0A)0000EC       */ callvirt   instance class [mscorlib/*23000001*/]System.Reflection.PropertyInfo/*01000041*/ [mscorlib/*23000001*/]System.Type/*01000003*/::GetProperty(string) /* 0A0000EC */
    IL_0076:  /* 7D   | (0A)0000E7       */ stfld      class [mscorlib/*23000001*/]System.Reflection.PropertyInfo/*01000041*/ class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*//'<>c__DisplayClasse'/*0200005C*/<!T>/*1B000059*/::textProp /* 0A0000E7 */
    IL_007b:  /* 08   |                  */ ldloc.2
    IL_007c:  /* 7B   | (0A)0000E7       */ ldfld      class [mscorlib/*23000001*/]System.Reflection.PropertyInfo/*01000041*/ class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*//'<>c__DisplayClasse'/*0200005C*/<!T>/*1B000059*/::textProp /* 0A0000E7 */
    IL_0081:  /* 2D   | 16               */ brtrue.s   IL_0099

    IL_0083:  /* 72   | (70)000332       */ ldstr      "The option list does not contain the specified tex"
    + "t property: {0}" /* 70000332 */
    IL_0088:  /* 04   |                  */ ldarg.2
    IL_0089:  /* 28   | (0A)000087       */ call       string [mscorlib/*23000001*/]System.String/*0100003F*/::Format(string,
                                                                                                                      object) /* 0A000087 */
    IL_008e:  /* 72   | (70)00028F       */ ldstr      "dataTextField" /* 7000028F */
    IL_0093:  /* 73   | (0A)0000E1       */ newobj     instance void [mscorlib/*23000001*/]System.ArgumentException/*0100005C*/::.ctor(string,
                                                                                                                                       string) /* 0A0000E1 */
    IL_0098:  /* 7A   |                  */ throw

    IL_0099:  /* 02   |                  */ ldarg.0
    IL_009a:  /* 08   |                  */ ldloc.2
    IL_009b:  /* FE06 | (0A)0000ED       */ ldftn      instance object class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*//'<>c__DisplayClasse'/*0200005C*/<!T>/*1B000059*/::'<SetFieldExpressions>b__c'(object) /* 0A0000ED */
    IL_00a1:  /* 73   | (0A)0000DD       */ newobj     instance void class [System.Core/*23000002*/]System.Func`2/*0100000B*/<object,object>/*1B000057*/::.ctor(object,
                                                                                                                                                                native int) /* 0A0000DD */
    IL_00a6:  /* 7D   | (0A)0000DE       */ stfld      class [System.Core/*23000002*/]System.Func`2/*0100000B*/<object,object> class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/::_textFieldSelector /* 0A0000DE */
    IL_00ab:  /* 02   |                  */ ldarg.0
    IL_00ac:  /* 08   |                  */ ldloc.2
    IL_00ad:  /* FE06 | (0A)0000EE       */ ldftn      instance object class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*//'<>c__DisplayClasse'/*0200005C*/<!T>/*1B000059*/::'<SetFieldExpressions>b__d'(object) /* 0A0000EE */
    IL_00b3:  /* 73   | (0A)0000DD       */ newobj     instance void class [System.Core/*23000002*/]System.Func`2/*0100000B*/<object,object>/*1B000057*/::.ctor(object,
                                                                                                                                                                native int) /* 0A0000DD */
    IL_00b8:  /* 7D   | (0A)0000E0       */ stfld      class [System.Core/*23000002*/]System.Func`2/*0100000B*/<object,object> class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/::_valueFieldSelector /* 0A0000E0 */
    IL_00bd:  /* 2A   |                  */ ret
  } // end of method OptionsElementBase`1::SetFieldExpressions

  .method /*060000AA*/ family hidebysig instance bool 
          IsSelectedValue(object 'value') cil managed
  // SIG: 20 01 02 1C
  {
    // Method begins at RVA 0x3760
    // Code size       126 (0x7e)
    .maxstack  2
    .locals /*1100001D*/ init (string V_0,
             class [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000034*/ V_1,
             string V_2,
             int32 V_3)
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 2C   | 08               */ brfalse.s  IL_000b

    IL_0003:  /* 03   |                  */ ldarg.1
    IL_0004:  /* 6F   | (0A)00006E       */ callvirt   instance string [mscorlib/*23000001*/]System.Object/*01000001*/::ToString() /* 0A00006E */
    IL_0009:  /* 2B   | 05               */ br.s       IL_0010

    IL_000b:  /* 7E   | (0A)0000EF       */ ldsfld     string [mscorlib/*23000001*/]System.String/*0100003F*/::Empty /* 0A0000EF */
    IL_0010:  /* 0A   |                  */ stloc.0
    IL_0011:  /* 02   |                  */ ldarg.0
    IL_0012:  /* 7B   | (0A)0000BA       */ ldfld      class [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000015*/ class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/::_selectedValues /* 0A0000BA */
    IL_0017:  /* 2C   | 63               */ brfalse.s  IL_007c

    IL_0019:  /* 02   |                  */ ldarg.0
    IL_001a:  /* 7B   | (0A)0000BA       */ ldfld      class [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000015*/ class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/::_selectedValues /* 0A0000BA */
    IL_001f:  /* 6F   | (0A)0000EB       */ callvirt   instance class [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000034*/ [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000015*/::GetEnumerator() /* 0A0000EB */
    IL_0024:  /* 0B   |                  */ stloc.1
    IL_0025:  /* 2B   | 4D               */ br.s       IL_0074

    IL_0027:  /* 07   |                  */ ldloc.1
    IL_0028:  /* 6F   | (0A)0000E5       */ callvirt   instance object [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000034*/::get_Current() /* 0A0000E5 */
    IL_002d:  /* 2C   | 34               */ brfalse.s  IL_0063

    IL_002f:  /* 07   |                  */ ldloc.1
    IL_0030:  /* 6F   | (0A)0000E5       */ callvirt   instance object [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000034*/::get_Current() /* 0A0000E5 */
    IL_0035:  /* 6F   | (0A)000016       */ callvirt   instance class [mscorlib/*23000001*/]System.Type/*01000003*/ [mscorlib/*23000001*/]System.Object/*01000001*/::GetType() /* 0A000016 */
    IL_003a:  /* 6F   | (0A)0000CC       */ callvirt   instance bool [mscorlib/*23000001*/]System.Type/*01000003*/::get_IsEnum() /* 0A0000CC */
    IL_003f:  /* 2D   | 0D               */ brtrue.s   IL_004e

    IL_0041:  /* 07   |                  */ ldloc.1
    IL_0042:  /* 6F   | (0A)0000E5       */ callvirt   instance object [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000034*/::get_Current() /* 0A0000E5 */
    IL_0047:  /* 6F   | (0A)00006E       */ callvirt   instance string [mscorlib/*23000001*/]System.Object/*01000001*/::ToString() /* 0A00006E */
    IL_004c:  /* 2B   | 1A               */ br.s       IL_0068

    IL_004e:  /* 07   |                  */ ldloc.1
    IL_004f:  /* 6F   | (0A)0000E5       */ callvirt   instance object [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000034*/::get_Current() /* 0A0000E5 */
    IL_0054:  /* A5   | (01)000044       */ unbox.any  [mscorlib/*23000001*/]System.Int32/*01000044*/
    IL_0059:  /* 0D   |                  */ stloc.3
    IL_005a:  /* 12   | 03               */ ldloca.s   V_3
    IL_005c:  /* 28   | (0A)0000CB       */ call       instance string [mscorlib/*23000001*/]System.Int32/*01000044*/::ToString() /* 0A0000CB */
    IL_0061:  /* 2B   | 05               */ br.s       IL_0068

    IL_0063:  /* 7E   | (0A)0000EF       */ ldsfld     string [mscorlib/*23000001*/]System.String/*0100003F*/::Empty /* 0A0000EF */
    IL_0068:  /* 0C   |                  */ stloc.2
    IL_0069:  /* 06   |                  */ ldloc.0
    IL_006a:  /* 08   |                  */ ldloc.2
    IL_006b:  /* 28   | (0A)00004B       */ call       bool [mscorlib/*23000001*/]System.String/*0100003F*/::op_Equality(string,
                                                                                                                         string) /* 0A00004B */
    IL_0070:  /* 2C   | 02               */ brfalse.s  IL_0074

    IL_0072:  /* 17   |                  */ ldc.i4.1
    IL_0073:  /* 2A   |                  */ ret

    IL_0074:  /* 07   |                  */ ldloc.1
    IL_0075:  /* 6F   | (0A)00001A       */ callvirt   instance bool [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000034*/::MoveNext() /* 0A00001A */
    IL_007a:  /* 2D   | AB               */ brtrue.s   IL_0027

    IL_007c:  /* 16   |                  */ ldc.i4.0
    IL_007d:  /* 2A   |                  */ ret
  } // end of method OptionsElementBase`1::IsSelectedValue

  .method /*060000AB*/ family hidebysig virtual 
          instance void  PreRender() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x37ea
    // Code size       32 (0x20)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (0A)0000BC       */ ldfld      class [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000015*/ class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/::_options /* 0A0000BC */
    IL_0006:  /* 2C   | 11               */ brfalse.s  IL_0019

    IL_0008:  /* 02   |                  */ ldarg.0
    IL_0009:  /* 7B   | (0A)000065       */ ldfld      class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::builder /* 0A000065 */
    IL_000e:  /* 02   |                  */ ldarg.0
    IL_000f:  /* 6F   | (0A)0000F0       */ callvirt   instance string class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/::RenderOptions() /* 0A0000F0 */
    IL_0014:  /* 6F   | (0A)0000F1       */ callvirt   instance void [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/::set_InnerHtml(string) /* 0A0000F1 */
    IL_0019:  /* 02   |                  */ ldarg.0
    IL_001a:  /* 28   | (0A)000072       */ call       instance void class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::PreRender() /* 0A000072 */
    IL_001f:  /* 2A   |                  */ ret
  } // end of method OptionsElementBase`1::PreRender

  .method /*060000AC*/ family hidebysig newslot abstract virtual 
          instance string  RenderOptions() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x0
  } // end of method OptionsElementBase`1::RenderOptions

  .method /*060000AD*/ private hidebysig static 
          bool  '<Options>b__0'(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.SelectListItem/*01000017*/ x) cil managed
  // SIG: 00 01 02 12 5D
  {
    .custom /*0C00004B:0A000013*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000032*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x33db
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 6F   | (0A)0000C1       */ callvirt   instance bool [System.Web.Mvc/*23000003*/]System.Web.Mvc.SelectListItem/*01000017*/::get_Selected() /* 0A0000C1 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method OptionsElementBase`1::'<Options>b__0'

  .method /*060000AE*/ private hidebysig static 
          string  '<Options>b__1'(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.SelectListItem/*01000017*/ x) cil managed
  // SIG: 00 01 0E 12 5D
  {
    .custom /*0C00004C:0A000013*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000032*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x33e3
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 6F   | (0A)0000C2       */ callvirt   instance string [System.Web.Mvc/*23000003*/]System.Web.Mvc.SelectListItem/*01000017*/::get_Value() /* 0A0000C2 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method OptionsElementBase`1::'<Options>b__1'

  .method /*060000AF*/ private hidebysig static 
          string  '<Options>b__4'<TValue>(!!TValue x) cil managed
  // SIG: 10 01 01 0E 1E 00
  {
    .custom /*0C00004D:0A000013*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000032*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x3480
    // Code size       20 (0x14)
    .maxstack  1
    .locals /*11000014*/ init (int32 V_0)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 8C   | (1B)000031       */ box        !!TValue/*1B000031*/
    IL_0006:  /* 28   | (0A)0000CA       */ call       int32 [mscorlib/*23000001*/]System.Convert/*0100005A*/::ToInt32(object) /* 0A0000CA */
    IL_000b:  /* 0A   |                  */ stloc.0
    IL_000c:  /* 12   | 00               */ ldloca.s   V_0
    IL_000e:  /* 28   | (0A)0000CB       */ call       instance string [mscorlib/*23000001*/]System.Int32/*01000044*/::ToString() /* 0A0000CB */
    IL_0013:  /* 2A   |                  */ ret
  } // end of method OptionsElementBase`1::'<Options>b__4'

  .method /*060000B0*/ private hidebysig static 
          string  '<Options>b__5'<TValue>(!!TValue x) cil managed
  // SIG: 10 01 01 0E 1E 00
  {
    .custom /*0C00004E:0A000013*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000032*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x34a0
    // Code size       14 (0xe)
    .maxstack  8
    IL_0000:  /* 0F   | 00               */ ldarga.s   x
    IL_0002:  /* FE16 | (1B)000031       */ constrained. !!TValue/*1B000031*/
    IL_0008:  /* 6F   | (0A)00006E       */ callvirt   instance string [mscorlib/*23000001*/]System.Object/*01000001*/::ToString() /* 0A00006E */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method OptionsElementBase`1::'<Options>b__5'

  .method /*060000B1*/ private hidebysig static 
          !!TValue  '<Options>b__6'<TValue>(!!TValue x) cil managed
  // SIG: 10 01 01 1E 00 1E 00
  {
    .custom /*0C00004F:0A000013*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000032*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x34af
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 2A   |                  */ ret
  } // end of method OptionsElementBase`1::'<Options>b__6'

  .method /*060000B2*/ private hidebysig static 
          !!TValue  '<Options>b__7'<TValue>(!!TValue x) cil managed
  // SIG: 10 01 01 1E 00 1E 00
  {
    .custom /*0C000050:0A000013*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000032*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x34b2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 2A   |                  */ ret
  } // end of method OptionsElementBase`1::'<Options>b__7'

  .property /*17000017*/ instance class [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000015*/
          SelectedValues()
  {
    .get instance class [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000015*/ MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/::get_SelectedValues() /* 060000A1 */
  } // end of property OptionsElementBase`1::SelectedValues
} // end of class MvcContrib.FluentHtml.Elements.OptionsElementBase`1

.class /*02000026*/ public abstract auto ansi beforefieldinit MvcContrib.FluentHtml.Elements.CheckBoxListBase`1<(class MvcContrib.FluentHtml.Elements.CheckBoxListBase`1/*02000026*/<!T>/*1B00000E*/) T>
       extends class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/
{
  .field /*04000029*/ family string _itemFormat
  .field /*0400002A*/ family string _itemClass
  .field /*0400002B*/ family class [System.Core/*23000002*/]System.Action`3/*01000019*/<class MvcContrib.FluentHtml.Elements.CheckBox/*02000024*/,object,int32> _optionModifier
  .method /*060000B3*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(string tag,
                               string name,
                               class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ forMember,
                               class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> behaviors) cil managed
  // SIG: 20 04 01 0E 0E 12 39 15 12 11 01 12 08
  {
    // Method begins at RVA 0x380b
    // Code size       12 (0xc)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 04   |                  */ ldarg.2
    IL_0003:  /* 05   |                  */ ldarg.3
    IL_0004:  /* 0E   | 04               */ ldarg.s    behaviors
    IL_0006:  /* 28   | (0A)0000F2       */ call       instance void class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/::.ctor(string,
                                                                                                                                                                  string,
                                                                                                                                                                  class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                                                                  class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 0A0000F2 */
    IL_000b:  /* 2A   |                  */ ret
  } // end of method CheckBoxListBase`1::.ctor

  .method /*060000B4*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(string tag,
                               string name) cil managed
  // SIG: 20 02 01 0E 0E
  {
    // Method begins at RVA 0x3818
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 04   |                  */ ldarg.2
    IL_0003:  /* 28   | (0A)0000F3       */ call       instance void class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/::.ctor(string,
                                                                                                                                                                  string) /* 0A0000F3 */
    IL_0008:  /* 2A   |                  */ ret
  } // end of method CheckBoxListBase`1::.ctor

  .method /*060000B5*/ public hidebysig newslot virtual 
          instance !T  Selected(class [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000015*/ selectedValues) cil managed
  // SIG: 20 01 13 00 12 55
  {
    // Method begins at RVA 0x3822
    // Code size       14 (0xe)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (0A)0000BA       */ stfld      class [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000015*/ class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/::_selectedValues /* 0A0000BA */
    IL_0007:  /* 02   |                  */ ldarg.0
    IL_0008:  /* A5   | (1B)000040       */ unbox.any  !T/*1B000040*/
    IL_000d:  /* 2A   |                  */ ret
  } // end of method CheckBoxListBase`1::Selected

  .method /*060000B6*/ public hidebysig newslot virtual 
          instance !T  ItemFormat(string 'value') cil managed
  // SIG: 20 01 13 00 0E
  {
    // Method begins at RVA 0x3831
    // Code size       14 (0xe)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (0A)0000F4       */ stfld      string class MvcContrib.FluentHtml.Elements.CheckBoxListBase`1/*02000026*/<!T>/*1B00000E*/::_itemFormat /* 0A0000F4 */
    IL_0007:  /* 02   |                  */ ldarg.0
    IL_0008:  /* A5   | (1B)000040       */ unbox.any  !T/*1B000040*/
    IL_000d:  /* 2A   |                  */ ret
  } // end of method CheckBoxListBase`1::ItemFormat

  .method /*060000B7*/ public hidebysig newslot virtual 
          instance !T  ItemClass(string 'value') cil managed
  // SIG: 20 01 13 00 0E
  {
    // Method begins at RVA 0x3840
    // Code size       14 (0xe)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (0A)0000F5       */ stfld      string class MvcContrib.FluentHtml.Elements.CheckBoxListBase`1/*02000026*/<!T>/*1B00000E*/::_itemClass /* 0A0000F5 */
    IL_0007:  /* 02   |                  */ ldarg.0
    IL_0008:  /* A5   | (1B)000040       */ unbox.any  !T/*1B000040*/
    IL_000d:  /* 2A   |                  */ ret
  } // end of method CheckBoxListBase`1::ItemClass

  .method /*060000B8*/ public hidebysig newslot virtual 
          instance !T  EachOption(class [System.Core/*23000002*/]System.Action`3/*01000019*/<class MvcContrib.FluentHtml.Elements.CheckBox/*02000024*/,object,int32> action) cil managed
  // SIG: 20 01 13 00 15 12 65 03 12 80 90 1C 08
  {
    // Method begins at RVA 0x384f
    // Code size       14 (0xe)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (0A)0000F6       */ stfld      class [System.Core/*23000002*/]System.Action`3/*01000019*/<class MvcContrib.FluentHtml.Elements.CheckBox/*02000024*/,object,int32> class MvcContrib.FluentHtml.Elements.CheckBoxListBase`1/*02000026*/<!T>/*1B00000E*/::_optionModifier /* 0A0000F6 */
    IL_0007:  /* 02   |                  */ ldarg.0
    IL_0008:  /* A5   | (1B)000040       */ unbox.any  !T/*1B000040*/
    IL_000d:  /* 2A   |                  */ ret
  } // end of method CheckBoxListBase`1::EachOption

  .method /*060000B9*/ family hidebysig specialname virtual 
          instance valuetype [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagRenderMode/*01000013*/ 
          get_TagRenderMode() cil managed
  // SIG: 20 00 11 4D
  {
    // Method begins at RVA 0x385e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 16   |                  */ ldc.i4.0
    IL_0001:  /* 2A   |                  */ ret
  } // end of method CheckBoxListBase`1::get_TagRenderMode

  .method /*060000BA*/ family hidebysig virtual 
          instance string  RenderOptions() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x3864
    // Code size       320 (0x140)
    .maxstack  4
    .locals /*1100001E*/ init (string V_0,
             class [mscorlib/*23000001*/]System.Text.StringBuilder/*01000055*/ V_1,
             int32 V_2,
             object V_3,
             object V_4,
             class MvcContrib.FluentHtml.Elements.CheckBox/*02000024*/ V_5,
             class [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000034*/ V_6,
             class [mscorlib/*23000001*/]System.IDisposable/*01000035*/ V_7)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (0A)000065       */ ldfld      class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::builder /* 0A000065 */
    IL_0006:  /* 6F   | (0A)000076       */ callvirt   instance class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<string,string> [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/::get_Attributes() /* 0A000076 */
    IL_000b:  /* 72   | (70)000091       */ ldstr      "name" /* 70000091 */
    IL_0010:  /* 6F   | (0A)000095       */ callvirt   instance !1 class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<string,string>/*1B000047*/::get_Item(!0) /* 0A000095 */
    IL_0015:  /* 0A   |                  */ stloc.0
    IL_0016:  /* 02   |                  */ ldarg.0
    IL_0017:  /* 7B   | (0A)000065       */ ldfld      class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::builder /* 0A000065 */
    IL_001c:  /* 6F   | (0A)000076       */ callvirt   instance class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<string,string> [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/::get_Attributes() /* 0A000076 */
    IL_0021:  /* 72   | (70)000091       */ ldstr      "name" /* 70000091 */
    IL_0026:  /* 6F   | (0A)000077       */ callvirt   instance bool class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<string,string>/*1B000047*/::Remove(!0) /* 0A000077 */
    IL_002b:  /* 26   |                  */ pop
    IL_002c:  /* 73   | (0A)000067       */ newobj     instance void [mscorlib/*23000001*/]System.Text.StringBuilder/*01000055*/::.ctor() /* 0A000067 */
    IL_0031:  /* 0B   |                  */ stloc.1
    IL_0032:  /* 16   |                  */ ldc.i4.0
    IL_0033:  /* 0C   |                  */ stloc.2
    IL_0034:  /* 02   |                  */ ldarg.0
    IL_0035:  /* 7B   | (0A)0000BC       */ ldfld      class [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000015*/ class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/::_options /* 0A0000BC */
    IL_003a:  /* 6F   | (0A)0000EB       */ callvirt   instance class [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000034*/ [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000015*/::GetEnumerator() /* 0A0000EB */
    IL_003f:  /* 13   | 06               */ stloc.s    V_6
    .try
    {
      IL_0041:  /* 38   | D0000000         */ br         IL_0116

      IL_0046:  /* 11   | 06               */ ldloc.s    V_6
      IL_0048:  /* 6F   | (0A)0000E5       */ callvirt   instance object [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000034*/::get_Current() /* 0A0000E5 */
      IL_004d:  /* 0D   |                  */ stloc.3
      IL_004e:  /* 02   |                  */ ldarg.0
      IL_004f:  /* 7B   | (0A)0000E0       */ ldfld      class [System.Core/*23000002*/]System.Func`2/*0100000B*/<object,object> class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/::_valueFieldSelector /* 0A0000E0 */
      IL_0054:  /* 09   |                  */ ldloc.3
      IL_0055:  /* 6F   | (0A)0000F7       */ callvirt   instance !1 class [System.Core/*23000002*/]System.Func`2/*0100000B*/<object,object>/*1B000057*/::Invoke(!0) /* 0A0000F7 */
      IL_005a:  /* 13   | 04               */ stloc.s    V_4
      IL_005c:  /* 06   |                  */ ldloc.0
      IL_005d:  /* 02   |                  */ ldarg.0
      IL_005e:  /* 7B   | (0A)000060       */ ldfld      class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::forMember /* 0A000060 */
      IL_0063:  /* 02   |                  */ ldarg.0
      IL_0064:  /* 7B   | (0A)000061       */ ldfld      class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::behaviors /* 0A000061 */
      IL_0069:  /* 73   | (06)00009E       */ newobj     instance void MvcContrib.FluentHtml.Elements.CheckBox/*02000024*/::.ctor(string,
                                                                                                                                  class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                                  class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 0600009E */
      IL_006e:  /* 72   | (70)0003B7       */ ldstr      "{0}_{1}" /* 700003B7 */
      IL_0073:  /* 06   |                  */ ldloc.0
      IL_0074:  /* 28   | (06)00010E       */ call       string MvcContrib.FluentHtml.StringExtensions/*0200003B*/::FormatAsHtmlId(string) /* 0600010E */
      IL_0079:  /* 08   |                  */ ldloc.2
      IL_007a:  /* 8C   | (01)000044       */ box        [mscorlib/*23000001*/]System.Int32/*01000044*/
      IL_007f:  /* 28   | (0A)0000F8       */ call       string [mscorlib/*23000001*/]System.String/*0100003F*/::Format(string,
                                                                                                                        object,
                                                                                                                        object) /* 0A0000F8 */
      IL_0084:  /* 6F   | (0A)0000F9       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<class MvcContrib.FluentHtml.Elements.CheckBox/*02000024*/>/*1B00005A*/::Id(string) /* 0A0000F9 */
      IL_0089:  /* 11   | 04               */ ldloc.s    V_4
      IL_008b:  /* 6F   | (0A)0000FA       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.Input`1/*02000020*/<class MvcContrib.FluentHtml.Elements.CheckBox/*02000024*/>/*1B00005B*/::Value(object) /* 0A0000FA */
      IL_0090:  /* 02   |                  */ ldarg.0
      IL_0091:  /* 7B   | (0A)0000DE       */ ldfld      class [System.Core/*23000002*/]System.Func`2/*0100000B*/<object,object> class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/::_textFieldSelector /* 0A0000DE */
      IL_0096:  /* 09   |                  */ ldloc.3
      IL_0097:  /* 6F   | (0A)0000F7       */ callvirt   instance !1 class [System.Core/*23000002*/]System.Func`2/*0100000B*/<object,object>/*1B000057*/::Invoke(!0) /* 0A0000F7 */
      IL_009c:  /* 6F   | (0A)00006E       */ callvirt   instance string [mscorlib/*23000001*/]System.Object/*01000001*/::ToString() /* 0A00006E */
      IL_00a1:  /* 02   |                  */ ldarg.0
      IL_00a2:  /* 7B   | (0A)0000F5       */ ldfld      string class MvcContrib.FluentHtml.Elements.CheckBoxListBase`1/*02000026*/<!T>/*1B00000E*/::_itemClass /* 0A0000F5 */
      IL_00a7:  /* 6F   | (0A)0000FB       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<class MvcContrib.FluentHtml.Elements.CheckBox/*02000024*/>/*1B00005A*/::LabelAfter(string,
                                                                                                                                                                                                                   string) /* 0A0000FB */
      IL_00ac:  /* 02   |                  */ ldarg.0
      IL_00ad:  /* 11   | 04               */ ldloc.s    V_4
      IL_00af:  /* 28   | (0A)0000FC       */ call       instance bool class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/::IsSelectedValue(object) /* 0A0000FC */
      IL_00b4:  /* 6F   | (0A)0000FD       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.CheckBoxBase`1/*02000023*/<class MvcContrib.FluentHtml.Elements.CheckBox/*02000024*/>/*1B00000C*/::Checked(bool) /* 0A0000FD */
      IL_00b9:  /* 13   | 05               */ stloc.s    V_5
      IL_00bb:  /* 02   |                  */ ldarg.0
      IL_00bc:  /* 7B   | (0A)0000F5       */ ldfld      string class MvcContrib.FluentHtml.Elements.CheckBoxListBase`1/*02000026*/<!T>/*1B00000E*/::_itemClass /* 0A0000F5 */
      IL_00c1:  /* 2C   | 0E               */ brfalse.s  IL_00d1

      IL_00c3:  /* 11   | 05               */ ldloc.s    V_5
      IL_00c5:  /* 02   |                  */ ldarg.0
      IL_00c6:  /* 7B   | (0A)0000F5       */ ldfld      string class MvcContrib.FluentHtml.Elements.CheckBoxListBase`1/*02000026*/<!T>/*1B00000E*/::_itemClass /* 0A0000F5 */
      IL_00cb:  /* 6F   | (0A)0000FE       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<class MvcContrib.FluentHtml.Elements.CheckBox/*02000024*/>/*1B00005A*/::Class(string) /* 0A0000FE */
      IL_00d0:  /* 26   |                  */ pop
      IL_00d1:  /* 02   |                  */ ldarg.0
      IL_00d2:  /* 7B   | (0A)0000F6       */ ldfld      class [System.Core/*23000002*/]System.Action`3/*01000019*/<class MvcContrib.FluentHtml.Elements.CheckBox/*02000024*/,object,int32> class MvcContrib.FluentHtml.Elements.CheckBoxListBase`1/*02000026*/<!T>/*1B00000E*/::_optionModifier /* 0A0000F6 */
      IL_00d7:  /* 2C   | 0F               */ brfalse.s  IL_00e8

      IL_00d9:  /* 02   |                  */ ldarg.0
      IL_00da:  /* 7B   | (0A)0000F6       */ ldfld      class [System.Core/*23000002*/]System.Action`3/*01000019*/<class MvcContrib.FluentHtml.Elements.CheckBox/*02000024*/,object,int32> class MvcContrib.FluentHtml.Elements.CheckBoxListBase`1/*02000026*/<!T>/*1B00000E*/::_optionModifier /* 0A0000F6 */
      IL_00df:  /* 11   | 05               */ ldloc.s    V_5
      IL_00e1:  /* 09   |                  */ ldloc.3
      IL_00e2:  /* 08   |                  */ ldloc.2
      IL_00e3:  /* 6F   | (0A)0000FF       */ callvirt   instance void class [System.Core/*23000002*/]System.Action`3/*01000019*/<class MvcContrib.FluentHtml.Elements.CheckBox/*02000024*/,object,int32>/*1B00005C*/::Invoke(!0,
                                                                                                                                                                                                                              !1,
                                                                                                                                                                                                                              !2) /* 0A0000FF */
      IL_00e8:  /* 07   |                  */ ldloc.1
      IL_00e9:  /* 02   |                  */ ldarg.0
      IL_00ea:  /* 7B   | (0A)0000F4       */ ldfld      string class MvcContrib.FluentHtml.Elements.CheckBoxListBase`1/*02000026*/<!T>/*1B00000E*/::_itemFormat /* 0A0000F4 */
      IL_00ef:  /* 2C   | 14               */ brfalse.s  IL_0105

      IL_00f1:  /* 02   |                  */ ldarg.0
      IL_00f2:  /* 7B   | (0A)0000F4       */ ldfld      string class MvcContrib.FluentHtml.Elements.CheckBoxListBase`1/*02000026*/<!T>/*1B00000E*/::_itemFormat /* 0A0000F4 */
      IL_00f7:  /* 11   | 05               */ ldloc.s    V_5
      IL_00f9:  /* 6F   | (0A)000100       */ callvirt   instance string class MvcContrib.FluentHtml.Elements.CheckBoxBase`1/*02000023*/<class MvcContrib.FluentHtml.Elements.CheckBox/*02000024*/>/*1B00000C*/::ToCheckBoxOnlyHtml() /* 0A000100 */
      IL_00fe:  /* 28   | (0A)000087       */ call       string [mscorlib/*23000001*/]System.String/*0100003F*/::Format(string,
                                                                                                                        object) /* 0A000087 */
      IL_0103:  /* 2B   | 07               */ br.s       IL_010c

      IL_0105:  /* 11   | 05               */ ldloc.s    V_5
      IL_0107:  /* 6F   | (0A)000100       */ callvirt   instance string class MvcContrib.FluentHtml.Elements.CheckBoxBase`1/*02000023*/<class MvcContrib.FluentHtml.Elements.CheckBox/*02000024*/>/*1B00000C*/::ToCheckBoxOnlyHtml() /* 0A000100 */
      IL_010c:  /* 6F   | (0A)000101       */ callvirt   instance class [mscorlib/*23000001*/]System.Text.StringBuilder/*01000055*/ [mscorlib/*23000001*/]System.Text.StringBuilder/*01000055*/::Append(string) /* 0A000101 */
      IL_0111:  /* 26   |                  */ pop
      IL_0112:  /* 08   |                  */ ldloc.2
      IL_0113:  /* 17   |                  */ ldc.i4.1
      IL_0114:  /* 58   |                  */ add
      IL_0115:  /* 0C   |                  */ stloc.2
      IL_0116:  /* 11   | 06               */ ldloc.s    V_6
      IL_0118:  /* 6F   | (0A)00001A       */ callvirt   instance bool [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000034*/::MoveNext() /* 0A00001A */
      IL_011d:  /* 3A   | 24FFFFFF         */ brtrue     IL_0046

      IL_0122:  /* DE   | 15               */ leave.s    IL_0139

    }  // end .try
    finally
    {
      IL_0124:  /* 11   | 06               */ ldloc.s    V_6
      IL_0126:  /* 75   | (01)000035       */ isinst     [mscorlib/*23000001*/]System.IDisposable/*01000035*/
      IL_012b:  /* 13   | 07               */ stloc.s    V_7
      IL_012d:  /* 11   | 07               */ ldloc.s    V_7
      IL_012f:  /* 2C   | 07               */ brfalse.s  IL_0138

      IL_0131:  /* 11   | 07               */ ldloc.s    V_7
      IL_0133:  /* 6F   | (0A)00001B       */ callvirt   instance void [mscorlib/*23000001*/]System.IDisposable/*01000035*/::Dispose() /* 0A00001B */
      IL_0138:  /* DC   |                  */ endfinally
    }  // end handler
    // HEX: 02 00 41 00 E3 24 01 15 00 00 00 00
    IL_0139:  /* 07   |                  */ ldloc.1
    IL_013a:  /* 6F   | (0A)00006E       */ callvirt   instance string [mscorlib/*23000001*/]System.Object/*01000001*/::ToString() /* 0A00006E */
    IL_013f:  /* 2A   |                  */ ret
  } // end of method CheckBoxListBase`1::RenderOptions

  .property /*17000018*/ instance valuetype [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagRenderMode/*01000013*/
          TagRenderMode()
  {
    .get instance valuetype [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagRenderMode/*01000013*/ MvcContrib.FluentHtml.Elements.CheckBoxListBase`1/*02000026*/::get_TagRenderMode() /* 060000B9 */
  } // end of property CheckBoxListBase`1::TagRenderMode
} // end of class MvcContrib.FluentHtml.Elements.CheckBoxListBase`1

.class /*02000027*/ public auto ansi beforefieldinit MvcContrib.FluentHtml.Elements.CheckBoxList
       extends class MvcContrib.FluentHtml.Elements.CheckBoxListBase`1/*02000026*/<class MvcContrib.FluentHtml.Elements.CheckBoxList/*02000027*/>/*1B00000F*/
{
  .method /*060000BB*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x39c0
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 72   | (70)0003C7       */ ldstr      "div" /* 700003C7 */
    IL_0006:  /* 03   |                  */ ldarg.1
    IL_0007:  /* 28   | (0A)000102       */ call       instance void class MvcContrib.FluentHtml.Elements.CheckBoxListBase`1/*02000026*/<class MvcContrib.FluentHtml.Elements.CheckBoxList/*02000027*/>/*1B00000F*/::.ctor(string,
                                                                                                                                                                                                                           string) /* 0A000102 */
    IL_000c:  /* 2A   |                  */ ret
  } // end of method CheckBoxList::.ctor

  .method /*060000BC*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name,
                               class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ forMember,
                               class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> behaviors) cil managed
  // SIG: 20 03 01 0E 12 39 15 12 11 01 12 08
  {
    // Method begins at RVA 0x39ce
    // Code size       15 (0xf)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 72   | (70)0003C7       */ ldstr      "div" /* 700003C7 */
    IL_0006:  /* 03   |                  */ ldarg.1
    IL_0007:  /* 04   |                  */ ldarg.2
    IL_0008:  /* 05   |                  */ ldarg.3
    IL_0009:  /* 28   | (0A)000103       */ call       instance void class MvcContrib.FluentHtml.Elements.CheckBoxListBase`1/*02000026*/<class MvcContrib.FluentHtml.Elements.CheckBoxList/*02000027*/>/*1B00000F*/::.ctor(string,
                                                                                                                                                                                                                           string,
                                                                                                                                                                                                                           class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                                                                                                                           class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 0A000103 */
    IL_000e:  /* 2A   |                  */ ret
  } // end of method CheckBoxList::.ctor

} // end of class MvcContrib.FluentHtml.Elements.CheckBoxList

.class /*02000028*/ public auto ansi beforefieldinit MvcContrib.FluentHtml.Elements.LabelBase`1<(class MvcContrib.FluentHtml.Elements.LabelBase`1/*02000028*/<!T>/*1B000010*/) T>
       extends class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/
{
  .field /*0400002C*/ static family literal string labelIdSuffix = "_DetachedLabel"
  .field /*0400002D*/ family string forName
  .field /*0400002E*/ family object rawValue
  .field /*0400002F*/ family string format
  .field /*04000030*/ family string overridenId
  .method /*060000BD*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(string forName,
                               class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ forMember,
                               class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> behaviors) cil managed
  // SIG: 20 03 01 0E 12 39 15 12 11 01 12 08
  {
    // Method begins at RVA 0x39de
    // Code size       21 (0x15)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 72   | (70)0000FF       */ ldstr      "label" /* 700000FF */
    IL_0006:  /* 04   |                  */ ldarg.2
    IL_0007:  /* 05   |                  */ ldarg.3
    IL_0008:  /* 28   | (0A)00009F       */ call       instance void class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::.ctor(string,
                                                                                                                                                       class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                                                       class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 0A00009F */
    IL_000d:  /* 02   |                  */ ldarg.0
    IL_000e:  /* 03   |                  */ ldarg.1
    IL_000f:  /* 7D   | (0A)000104       */ stfld      string class MvcContrib.FluentHtml.Elements.LabelBase`1/*02000028*/<!T>/*1B000010*/::forName /* 0A000104 */
    IL_0014:  /* 2A   |                  */ ret
  } // end of method LabelBase`1::.ctor

  .method /*060000BE*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(string forName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x39f4
    // Code size       19 (0x13)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 72   | (70)0000FF       */ ldstr      "label" /* 700000FF */
    IL_0006:  /* 28   | (0A)00005F       */ call       instance void class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::.ctor(string) /* 0A00005F */
    IL_000b:  /* 02   |                  */ ldarg.0
    IL_000c:  /* 03   |                  */ ldarg.1
    IL_000d:  /* 7D   | (0A)000104       */ stfld      string class MvcContrib.FluentHtml.Elements.LabelBase`1/*02000028*/<!T>/*1B000010*/::forName /* 0A000104 */
    IL_0012:  /* 2A   |                  */ ret
  } // end of method LabelBase`1::.ctor

  .method /*060000BF*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x3a08
    // Code size       12 (0xc)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 72   | (70)0000FF       */ ldstr      "label" /* 700000FF */
    IL_0006:  /* 28   | (0A)00005F       */ call       instance void class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::.ctor(string) /* 0A00005F */
    IL_000b:  /* 2A   |                  */ ret
  } // end of method LabelBase`1::.ctor

  .method /*060000C0*/ public hidebysig newslot virtual 
          instance !T  Value(object 'value') cil managed
  // SIG: 20 01 13 00 1C
  {
    // Method begins at RVA 0x3a15
    // Code size       14 (0xe)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (0A)000105       */ stfld      object class MvcContrib.FluentHtml.Elements.LabelBase`1/*02000028*/<!T>/*1B000010*/::rawValue /* 0A000105 */
    IL_0007:  /* 02   |                  */ ldarg.0
    IL_0008:  /* A5   | (1B)000040       */ unbox.any  !T/*1B000040*/
    IL_000d:  /* 2A   |                  */ ret
  } // end of method LabelBase`1::Value

  .method /*060000C1*/ public hidebysig newslot virtual 
          instance !T  Format(string 'value') cil managed
  // SIG: 20 01 13 00 0E
  {
    // Method begins at RVA 0x3a24
    // Code size       14 (0xe)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (0A)000106       */ stfld      string class MvcContrib.FluentHtml.Elements.LabelBase`1/*02000028*/<!T>/*1B000010*/::format /* 0A000106 */
    IL_0007:  /* 02   |                  */ ldarg.0
    IL_0008:  /* A5   | (1B)000040       */ unbox.any  !T/*1B000040*/
    IL_000d:  /* 2A   |                  */ ret
  } // end of method LabelBase`1::Format

  .method /*060000C2*/ public hidebysig virtual 
          instance !T  Id(string 'value') cil managed
  // SIG: 20 01 13 00 0E
  {
    // Method begins at RVA 0x3a33
    // Code size       14 (0xe)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (0A)000107       */ stfld      string class MvcContrib.FluentHtml.Elements.LabelBase`1/*02000028*/<!T>/*1B000010*/::overridenId /* 0A000107 */
    IL_0007:  /* 02   |                  */ ldarg.0
    IL_0008:  /* A5   | (1B)000040       */ unbox.any  !T/*1B000040*/
    IL_000d:  /* 2A   |                  */ ret
  } // end of method LabelBase`1::Id

  .method /*060000C3*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x3a42
    // Code size       63 (0x3f)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000108       */ call       instance void class MvcContrib.FluentHtml.Elements.LabelBase`1/*02000028*/<!T>/*1B000010*/::SetId() /* 0A000108 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 7B   | (0A)000065       */ ldfld      class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::builder /* 0A000065 */
    IL_000c:  /* 72   | (70)00010B       */ ldstr      "for" /* 7000010B */
    IL_0011:  /* 02   |                  */ ldarg.0
    IL_0012:  /* 7B   | (0A)000104       */ ldfld      string class MvcContrib.FluentHtml.Elements.LabelBase`1/*02000028*/<!T>/*1B000010*/::forName /* 0A000104 */
    IL_0017:  /* 28   | (06)00010E       */ call       string MvcContrib.FluentHtml.StringExtensions/*0200003B*/::FormatAsHtmlId(string) /* 0600010E */
    IL_001c:  /* 6F   | (0A)00006F       */ callvirt   instance void [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/::MergeAttribute(string,
                                                                                                                                                       string) /* 0A00006F */
    IL_0021:  /* 02   |                  */ ldarg.0
    IL_0022:  /* 7B   | (0A)000065       */ ldfld      class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::builder /* 0A000065 */
    IL_0027:  /* 02   |                  */ ldarg.0
    IL_0028:  /* 02   |                  */ ldarg.0
    IL_0029:  /* 7B   | (0A)000105       */ ldfld      object class MvcContrib.FluentHtml.Elements.LabelBase`1/*02000028*/<!T>/*1B000010*/::rawValue /* 0A000105 */
    IL_002e:  /* 6F   | (0A)000109       */ callvirt   instance string class MvcContrib.FluentHtml.Elements.LabelBase`1/*02000028*/<!T>/*1B000010*/::FormatValue(object) /* 0A000109 */
    IL_0033:  /* 6F   | (0A)000093       */ callvirt   instance void [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/::SetInnerText(string) /* 0A000093 */
    IL_0038:  /* 02   |                  */ ldarg.0
    IL_0039:  /* 28   | (0A)0000A5       */ call       instance string class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::ToString() /* 0A0000A5 */
    IL_003e:  /* 2A   |                  */ ret
  } // end of method LabelBase`1::ToString

  .method /*060000C4*/ private hidebysig 
          instance void  SetId() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x3a82
    // Code size       56 (0x38)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (0A)000107       */ ldfld      string class MvcContrib.FluentHtml.Elements.LabelBase`1/*02000028*/<!T>/*1B000010*/::overridenId /* 0A000107 */
    IL_0006:  /* 28   | (0A)000096       */ call       bool [mscorlib/*23000001*/]System.String/*0100003F*/::IsNullOrEmpty(string) /* 0A000096 */
    IL_000b:  /* 2D   | 0E               */ brtrue.s   IL_001b

    IL_000d:  /* 02   |                  */ ldarg.0
    IL_000e:  /* 02   |                  */ ldarg.0
    IL_000f:  /* 7B   | (0A)000107       */ ldfld      string class MvcContrib.FluentHtml.Elements.LabelBase`1/*02000028*/<!T>/*1B000010*/::overridenId /* 0A000107 */
    IL_0014:  /* 28   | (0A)00010A       */ call       instance !0 class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::Id(string) /* 0A00010A */
    IL_0019:  /* 26   |                  */ pop
    IL_001a:  /* 2A   |                  */ ret

    IL_001b:  /* 02   |                  */ ldarg.0
    IL_001c:  /* 02   |                  */ ldarg.0
    IL_001d:  /* 7B   | (0A)000104       */ ldfld      string class MvcContrib.FluentHtml.Elements.LabelBase`1/*02000028*/<!T>/*1B000010*/::forName /* 0A000104 */
    IL_0022:  /* 28   | (06)00010E       */ call       string MvcContrib.FluentHtml.StringExtensions/*0200003B*/::FormatAsHtmlId(string) /* 0600010E */
    IL_0027:  /* 72   | (70)0003CF       */ ldstr      "_DetachedLabel" /* 700003CF */
    IL_002c:  /* 28   | (0A)000075       */ call       string [mscorlib/*23000001*/]System.String/*0100003F*/::Concat(string,
                                                                                                                      string) /* 0A000075 */
    IL_0031:  /* 28   | (0A)00010A       */ call       instance !0 class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::Id(string) /* 0A00010A */
    IL_0036:  /* 26   |                  */ pop
    IL_0037:  /* 2A   |                  */ ret
  } // end of method LabelBase`1::SetId

  .method /*060000C5*/ family hidebysig newslot virtual 
          instance string  FormatValue(object 'value') cil managed
  // SIG: 20 01 0E 1C
  {
    // Method begins at RVA 0x3abc
    // Code size       102 (0x66)
    .maxstack  3
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (0A)000106       */ ldfld      string class MvcContrib.FluentHtml.Elements.LabelBase`1/*02000028*/<!T>/*1B000010*/::format /* 0A000106 */
    IL_0006:  /* 28   | (0A)000096       */ call       bool [mscorlib/*23000001*/]System.String/*0100003F*/::IsNullOrEmpty(string) /* 0A000096 */
    IL_000b:  /* 2D   | 4D               */ brtrue.s   IL_005a

    IL_000d:  /* 02   |                  */ ldarg.0
    IL_000e:  /* 7B   | (0A)000106       */ ldfld      string class MvcContrib.FluentHtml.Elements.LabelBase`1/*02000028*/<!T>/*1B000010*/::format /* 0A000106 */
    IL_0013:  /* 72   | (70)0003ED       */ ldstr      "{0" /* 700003ED */
    IL_0018:  /* 6F   | (0A)00010B       */ callvirt   instance bool [mscorlib/*23000001*/]System.String/*0100003F*/::StartsWith(string) /* 0A00010B */
    IL_001d:  /* 2C   | 12               */ brfalse.s  IL_0031

    IL_001f:  /* 02   |                  */ ldarg.0
    IL_0020:  /* 7B   | (0A)000106       */ ldfld      string class MvcContrib.FluentHtml.Elements.LabelBase`1/*02000028*/<!T>/*1B000010*/::format /* 0A000106 */
    IL_0025:  /* 72   | (70)0003F3       */ ldstr      "}" /* 700003F3 */
    IL_002a:  /* 6F   | (0A)00010C       */ callvirt   instance bool [mscorlib/*23000001*/]System.String/*0100003F*/::EndsWith(string) /* 0A00010C */
    IL_002f:  /* 2D   | 1C               */ brtrue.s   IL_004d

    IL_0031:  /* 72   | (70)0003F7       */ ldstr      "{0:" /* 700003F7 */
    IL_0036:  /* 02   |                  */ ldarg.0
    IL_0037:  /* 7B   | (0A)000106       */ ldfld      string class MvcContrib.FluentHtml.Elements.LabelBase`1/*02000028*/<!T>/*1B000010*/::format /* 0A000106 */
    IL_003c:  /* 72   | (70)0003F3       */ ldstr      "}" /* 700003F3 */
    IL_0041:  /* 28   | (0A)00010D       */ call       string [mscorlib/*23000001*/]System.String/*0100003F*/::Concat(string,
                                                                                                                      string,
                                                                                                                      string) /* 0A00010D */
    IL_0046:  /* 03   |                  */ ldarg.1
    IL_0047:  /* 28   | (0A)000087       */ call       string [mscorlib/*23000001*/]System.String/*0100003F*/::Format(string,
                                                                                                                      object) /* 0A000087 */
    IL_004c:  /* 2A   |                  */ ret

    IL_004d:  /* 02   |                  */ ldarg.0
    IL_004e:  /* 7B   | (0A)000106       */ ldfld      string class MvcContrib.FluentHtml.Elements.LabelBase`1/*02000028*/<!T>/*1B000010*/::format /* 0A000106 */
    IL_0053:  /* 03   |                  */ ldarg.1
    IL_0054:  /* 28   | (0A)000087       */ call       string [mscorlib/*23000001*/]System.String/*0100003F*/::Format(string,
                                                                                                                      object) /* 0A000087 */
    IL_0059:  /* 2A   |                  */ ret

    IL_005a:  /* 03   |                  */ ldarg.1
    IL_005b:  /* 2C   | 07               */ brfalse.s  IL_0064

    IL_005d:  /* 03   |                  */ ldarg.1
    IL_005e:  /* 6F   | (0A)00006E       */ callvirt   instance string [mscorlib/*23000001*/]System.Object/*01000001*/::ToString() /* 0A00006E */
    IL_0063:  /* 2A   |                  */ ret

    IL_0064:  /* 14   |                  */ ldnull
    IL_0065:  /* 2A   |                  */ ret
  } // end of method LabelBase`1::FormatValue

} // end of class MvcContrib.FluentHtml.Elements.LabelBase`1

.class /*02000029*/ public auto ansi beforefieldinit MvcContrib.FluentHtml.Elements.Label
       extends class MvcContrib.FluentHtml.Elements.LabelBase`1/*02000028*/<class MvcContrib.FluentHtml.Elements.Label/*02000029*/>/*1B000011*/
{
  .method /*060000C6*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string forElement,
                               class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ forMember,
                               class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> behaviors) cil managed
  // SIG: 20 03 01 0E 12 39 15 12 11 01 12 08
  {
    // Method begins at RVA 0x3b2e
    // Code size       10 (0xa)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 04   |                  */ ldarg.2
    IL_0003:  /* 05   |                  */ ldarg.3
    IL_0004:  /* 28   | (0A)00010E       */ call       instance void class MvcContrib.FluentHtml.Elements.LabelBase`1/*02000028*/<class MvcContrib.FluentHtml.Elements.Label/*02000029*/>/*1B000011*/::.ctor(string,
                                                                                                                                                                                                             class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                                                                                                             class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 0A00010E */
    IL_0009:  /* 2A   |                  */ ret
  } // end of method Label::.ctor

  .method /*060000C7*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string forElement) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x3b39
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 28   | (0A)00010F       */ call       instance void class MvcContrib.FluentHtml.Elements.LabelBase`1/*02000028*/<class MvcContrib.FluentHtml.Elements.Label/*02000029*/>/*1B000011*/::.ctor(string) /* 0A00010F */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method Label::.ctor

  .method /*060000C8*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x3b42
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000110       */ call       instance void class MvcContrib.FluentHtml.Elements.LabelBase`1/*02000028*/<class MvcContrib.FluentHtml.Elements.Label/*02000029*/>/*1B000011*/::.ctor() /* 0A000110 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method Label::.ctor

} // end of class MvcContrib.FluentHtml.Elements.Label

.class /*0200002A*/ public abstract auto ansi beforefieldinit MvcContrib.FluentHtml.Elements.ResetButtonBase`1<(class MvcContrib.FluentHtml.Elements.ResetButtonBase`1/*0200002A*/<!T>/*1B000012*/) T>
       extends class MvcContrib.FluentHtml.Elements.Input`1/*02000020*/<!T>/*1B000008*/
{
  .method /*060000C9*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(string text) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x3b4a
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 14   |                  */ ldnull
    IL_0003:  /* 28   | (0A)000111       */ call       instance void class MvcContrib.FluentHtml.Elements.ResetButtonBase`1/*0200002A*/<!T>/*1B000012*/::.ctor(string,
                                                                                                                                                               class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 0A000111 */
    IL_0008:  /* 2A   |                  */ ret
  } // end of method ResetButtonBase`1::.ctor

  .method /*060000CA*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(string text,
                               class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> behaviors) cil managed
  // SIG: 20 02 01 0E 15 12 11 01 12 08
  {
    // Method begins at RVA 0x3b54
    // Code size       33 (0x21)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 72   | (70)0003FF       */ ldstr      "reset" /* 700003FF */
    IL_0006:  /* 03   |                  */ ldarg.1
    IL_0007:  /* 2C   | 08               */ brfalse.s  IL_0011

    IL_0009:  /* 03   |                  */ ldarg.1
    IL_000a:  /* 28   | (06)00010F       */ call       string MvcContrib.FluentHtml.StringExtensions/*0200003B*/::FormatAsHtmlName(string) /* 0600010F */
    IL_000f:  /* 2B   | 01               */ br.s       IL_0012

    IL_0011:  /* 14   |                  */ ldnull
    IL_0012:  /* 14   |                  */ ldnull
    IL_0013:  /* 04   |                  */ ldarg.2
    IL_0014:  /* 28   | (0A)0000AD       */ call       instance void class MvcContrib.FluentHtml.Elements.Input`1/*02000020*/<!T>/*1B000008*/::.ctor(string,
                                                                                                                                                     string,
                                                                                                                                                     class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                                                     class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 0A0000AD */
    IL_0019:  /* 02   |                  */ ldarg.0
    IL_001a:  /* 03   |                  */ ldarg.1
    IL_001b:  /* 7D   | (0A)0000A8       */ stfld      object class MvcContrib.FluentHtml.Elements.Input`1/*02000020*/<!T>/*1B000008*/::elementValue /* 0A0000A8 */
    IL_0020:  /* 2A   |                  */ ret
  } // end of method ResetButtonBase`1::.ctor

} // end of class MvcContrib.FluentHtml.Elements.ResetButtonBase`1

.class /*0200002B*/ public auto ansi beforefieldinit MvcContrib.FluentHtml.Elements.ResetButton
       extends class MvcContrib.FluentHtml.Elements.ResetButtonBase`1/*0200002A*/<class MvcContrib.FluentHtml.Elements.ResetButton/*0200002B*/>/*1B000013*/
{
  .method /*060000CB*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string text) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x3b76
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 28   | (0A)000112       */ call       instance void class MvcContrib.FluentHtml.Elements.ResetButtonBase`1/*0200002A*/<class MvcContrib.FluentHtml.Elements.ResetButton/*0200002B*/>/*1B000013*/::.ctor(string) /* 0A000112 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method ResetButton::.ctor

  .method /*060000CC*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string text,
                               class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> behaviors) cil managed
  // SIG: 20 02 01 0E 15 12 11 01 12 08
  {
    // Method begins at RVA 0x3b7f
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 04   |                  */ ldarg.2
    IL_0003:  /* 28   | (0A)000113       */ call       instance void class MvcContrib.FluentHtml.Elements.ResetButtonBase`1/*0200002A*/<class MvcContrib.FluentHtml.Elements.ResetButton/*0200002B*/>/*1B000013*/::.ctor(string,
                                                                                                                                                                                                                         class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 0A000113 */
    IL_0008:  /* 2A   |                  */ ret
  } // end of method ResetButton::.ctor

} // end of class MvcContrib.FluentHtml.Elements.ResetButton

.class /*0200002C*/ public abstract auto ansi beforefieldinit MvcContrib.FluentHtml.Elements.TextInput`1<(class MvcContrib.FluentHtml.Elements.TextInput`1/*0200002C*/<!T>/*1B000014*/) T>
       extends class MvcContrib.FluentHtml.Elements.Input`1/*02000020*/<!T>/*1B000008*/
       implements MvcContrib.FluentHtml.Behaviors.ISupportsMaxLength/*02000015*/
{
  .field /*04000031*/ family string _format
  .method /*060000CD*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(string 'type',
                               string name) cil managed
  // SIG: 20 02 01 0E 0E
  {
    // Method begins at RVA 0x3b89
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 04   |                  */ ldarg.2
    IL_0003:  /* 28   | (0A)0000B0       */ call       instance void class MvcContrib.FluentHtml.Elements.Input`1/*02000020*/<!T>/*1B000008*/::.ctor(string,
                                                                                                                                                     string) /* 0A0000B0 */
    IL_0008:  /* 2A   |                  */ ret
  } // end of method TextInput`1::.ctor

  .method /*060000CE*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(string 'type',
                               string name,
                               class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ forMember,
                               class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> behaviors) cil managed
  // SIG: 20 04 01 0E 0E 12 39 15 12 11 01 12 08
  {
    // Method begins at RVA 0x3b93
    // Code size       12 (0xc)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 04   |                  */ ldarg.2
    IL_0003:  /* 05   |                  */ ldarg.3
    IL_0004:  /* 0E   | 04               */ ldarg.s    behaviors
    IL_0006:  /* 28   | (0A)0000AD       */ call       instance void class MvcContrib.FluentHtml.Elements.Input`1/*02000020*/<!T>/*1B000008*/::.ctor(string,
                                                                                                                                                     string,
                                                                                                                                                     class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                                                     class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 0A0000AD */
    IL_000b:  /* 2A   |                  */ ret
  } // end of method TextInput`1::.ctor

  .method /*060000CF*/ public hidebysig newslot virtual 
          instance !T  MaxLength(int32 'value') cil managed
  // SIG: 20 01 13 00 08
  {
    // Method begins at RVA 0x3ba0
    // Code size       25 (0x19)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 72   | (70)00040B       */ ldstr      "maxlength" /* 7000040B */
    IL_0006:  /* 03   |                  */ ldarg.1
    IL_0007:  /* 8C   | (01)000044       */ box        [mscorlib/*23000001*/]System.Int32/*01000044*/
    IL_000c:  /* 6F   | (0A)0000A0       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::Attr(string,
                                                                                                                                                    object) /* 0A0000A0 */
    IL_0011:  /* 26   |                  */ pop
    IL_0012:  /* 02   |                  */ ldarg.0
    IL_0013:  /* A5   | (1B)000040       */ unbox.any  !T/*1B000040*/
    IL_0018:  /* 2A   |                  */ ret
  } // end of method TextInput`1::MaxLength

  .method /*060000D0*/ public hidebysig instance !T 
          Format(string 'value') cil managed
  // SIG: 20 01 13 00 0E
  {
    // Method begins at RVA 0x3bba
    // Code size       14 (0xe)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (0A)000114       */ stfld      string class MvcContrib.FluentHtml.Elements.TextInput`1/*0200002C*/<!T>/*1B000014*/::_format /* 0A000114 */
    IL_0007:  /* 02   |                  */ ldarg.0
    IL_0008:  /* A5   | (1B)000040       */ unbox.any  !T/*1B000040*/
    IL_000d:  /* 2A   |                  */ ret
  } // end of method TextInput`1::Format

  .method /*060000D1*/ family hidebysig virtual 
          instance void  PreRender() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x3bc9
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 6F   | (0A)000115       */ callvirt   instance void class MvcContrib.FluentHtml.Elements.TextInput`1/*0200002C*/<!T>/*1B000014*/::FormatValue() /* 0A000115 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 28   | (0A)000116       */ call       instance void class MvcContrib.FluentHtml.Elements.Input`1/*02000020*/<!T>/*1B000008*/::PreRender() /* 0A000116 */
    IL_000c:  /* 2A   |                  */ ret
  } // end of method TextInput`1::PreRender

  .method /*060000D2*/ family hidebysig newslot virtual 
          instance void  FormatValue() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x3bd8
    // Code size       112 (0x70)
    .maxstack  4
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (0A)000114       */ ldfld      string class MvcContrib.FluentHtml.Elements.TextInput`1/*0200002C*/<!T>/*1B000014*/::_format /* 0A000114 */
    IL_0006:  /* 28   | (0A)000096       */ call       bool [mscorlib/*23000001*/]System.String/*0100003F*/::IsNullOrEmpty(string) /* 0A000096 */
    IL_000b:  /* 2D   | 62               */ brtrue.s   IL_006f

    IL_000d:  /* 02   |                  */ ldarg.0
    IL_000e:  /* 7B   | (0A)000114       */ ldfld      string class MvcContrib.FluentHtml.Elements.TextInput`1/*0200002C*/<!T>/*1B000014*/::_format /* 0A000114 */
    IL_0013:  /* 72   | (70)0003ED       */ ldstr      "{0" /* 700003ED */
    IL_0018:  /* 6F   | (0A)00010B       */ callvirt   instance bool [mscorlib/*23000001*/]System.String/*0100003F*/::StartsWith(string) /* 0A00010B */
    IL_001d:  /* 2C   | 2A               */ brfalse.s  IL_0049

    IL_001f:  /* 02   |                  */ ldarg.0
    IL_0020:  /* 7B   | (0A)000114       */ ldfld      string class MvcContrib.FluentHtml.Elements.TextInput`1/*0200002C*/<!T>/*1B000014*/::_format /* 0A000114 */
    IL_0025:  /* 72   | (70)0003F3       */ ldstr      "}" /* 700003F3 */
    IL_002a:  /* 6F   | (0A)00010C       */ callvirt   instance bool [mscorlib/*23000001*/]System.String/*0100003F*/::EndsWith(string) /* 0A00010C */
    IL_002f:  /* 2C   | 18               */ brfalse.s  IL_0049

    IL_0031:  /* 02   |                  */ ldarg.0
    IL_0032:  /* 02   |                  */ ldarg.0
    IL_0033:  /* 7B   | (0A)000114       */ ldfld      string class MvcContrib.FluentHtml.Elements.TextInput`1/*0200002C*/<!T>/*1B000014*/::_format /* 0A000114 */
    IL_0038:  /* 02   |                  */ ldarg.0
    IL_0039:  /* 7B   | (0A)0000A8       */ ldfld      object class MvcContrib.FluentHtml.Elements.Input`1/*02000020*/<!T>/*1B000008*/::elementValue /* 0A0000A8 */
    IL_003e:  /* 28   | (0A)000087       */ call       string [mscorlib/*23000001*/]System.String/*0100003F*/::Format(string,
                                                                                                                      object) /* 0A000087 */
    IL_0043:  /* 7D   | (0A)0000A8       */ stfld      object class MvcContrib.FluentHtml.Elements.Input`1/*02000020*/<!T>/*1B000008*/::elementValue /* 0A0000A8 */
    IL_0048:  /* 2A   |                  */ ret

    IL_0049:  /* 02   |                  */ ldarg.0
    IL_004a:  /* 72   | (70)0003F7       */ ldstr      "{0:" /* 700003F7 */
    IL_004f:  /* 02   |                  */ ldarg.0
    IL_0050:  /* 7B   | (0A)000114       */ ldfld      string class MvcContrib.FluentHtml.Elements.TextInput`1/*0200002C*/<!T>/*1B000014*/::_format /* 0A000114 */
    IL_0055:  /* 72   | (70)0003F3       */ ldstr      "}" /* 700003F3 */
    IL_005a:  /* 28   | (0A)00010D       */ call       string [mscorlib/*23000001*/]System.String/*0100003F*/::Concat(string,
                                                                                                                      string,
                                                                                                                      string) /* 0A00010D */
    IL_005f:  /* 02   |                  */ ldarg.0
    IL_0060:  /* 7B   | (0A)0000A8       */ ldfld      object class MvcContrib.FluentHtml.Elements.Input`1/*02000020*/<!T>/*1B000008*/::elementValue /* 0A0000A8 */
    IL_0065:  /* 28   | (0A)000087       */ call       string [mscorlib/*23000001*/]System.String/*0100003F*/::Format(string,
                                                                                                                      object) /* 0A000087 */
    IL_006a:  /* 7D   | (0A)0000A8       */ stfld      object class MvcContrib.FluentHtml.Elements.Input`1/*02000020*/<!T>/*1B000008*/::elementValue /* 0A0000A8 */
    IL_006f:  /* 2A   |                  */ ret
  } // end of method TextInput`1::FormatValue

} // end of class MvcContrib.FluentHtml.Elements.TextInput`1

.class /*0200002D*/ public auto ansi beforefieldinit MvcContrib.FluentHtml.Elements.Hidden
       extends class MvcContrib.FluentHtml.Elements.TextInput`1/*0200002C*/<class MvcContrib.FluentHtml.Elements.Hidden/*0200002D*/>/*1B000015*/
{
  .method /*060000D3*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x3c54
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 72   | (70)00041F       */ ldstr      "hidden" /* 7000041F */
    IL_0006:  /* 03   |                  */ ldarg.1
    IL_0007:  /* 28   | (0A)000117       */ call       instance void class MvcContrib.FluentHtml.Elements.TextInput`1/*0200002C*/<class MvcContrib.FluentHtml.Elements.Hidden/*0200002D*/>/*1B000015*/::.ctor(string,
                                                                                                                                                                                                              string) /* 0A000117 */
    IL_000c:  /* 2A   |                  */ ret
  } // end of method Hidden::.ctor

  .method /*060000D4*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name,
                               class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ forMember,
                               class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> behaviors) cil managed
  // SIG: 20 03 01 0E 12 39 15 12 11 01 12 08
  {
    // Method begins at RVA 0x3c62
    // Code size       15 (0xf)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 72   | (70)00041F       */ ldstr      "hidden" /* 7000041F */
    IL_0006:  /* 03   |                  */ ldarg.1
    IL_0007:  /* 04   |                  */ ldarg.2
    IL_0008:  /* 05   |                  */ ldarg.3
    IL_0009:  /* 28   | (0A)000118       */ call       instance void class MvcContrib.FluentHtml.Elements.TextInput`1/*0200002C*/<class MvcContrib.FluentHtml.Elements.Hidden/*0200002D*/>/*1B000015*/::.ctor(string,
                                                                                                                                                                                                              string,
                                                                                                                                                                                                              class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                                                                                                              class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 0A000118 */
    IL_000e:  /* 2A   |                  */ ret
  } // end of method Hidden::.ctor

} // end of class MvcContrib.FluentHtml.Elements.Hidden

.class /*0200002E*/ public auto ansi beforefieldinit MvcContrib.FluentHtml.Elements.Index
       extends MvcContrib.FluentHtml.Elements.Hidden/*0200002D*/
{
  .method /*060000D5*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string baseName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x3c72
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 28   | (06)0000D7       */ call       string MvcContrib.FluentHtml.Elements.Index/*0200002E*/::GetName(string) /* 060000D7 */
    IL_0007:  /* 28   | (06)0000D3       */ call       instance void MvcContrib.FluentHtml.Elements.Hidden/*0200002D*/::.ctor(string) /* 060000D3 */
    IL_000c:  /* 2A   |                  */ ret
  } // end of method Index::.ctor

  .method /*060000D6*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string baseName,
                               class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ forMember,
                               class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> behaviors) cil managed
  // SIG: 20 03 01 0E 12 39 15 12 11 01 12 08
  {
    // Method begins at RVA 0x3c80
    // Code size       15 (0xf)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 28   | (06)0000D7       */ call       string MvcContrib.FluentHtml.Elements.Index/*0200002E*/::GetName(string) /* 060000D7 */
    IL_0007:  /* 04   |                  */ ldarg.2
    IL_0008:  /* 05   |                  */ ldarg.3
    IL_0009:  /* 28   | (06)0000D4       */ call       instance void MvcContrib.FluentHtml.Elements.Hidden/*0200002D*/::.ctor(string,
                                                                                                                              class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                              class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 060000D4 */
    IL_000e:  /* 2A   |                  */ ret
  } // end of method Index::.ctor

  .method /*060000D7*/ public hidebysig static 
          string  GetName(string baseName) cil managed
  // SIG: 00 01 0E 0E
  {
    // Method begins at RVA 0x3c90
    // Code size       12 (0xc)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 72   | (70)00042D       */ ldstr      ".Index" /* 7000042D */
    IL_0006:  /* 28   | (0A)000075       */ call       string [mscorlib/*23000001*/]System.String/*0100003F*/::Concat(string,
                                                                                                                      string) /* 0A000075 */
    IL_000b:  /* 2A   |                  */ ret
  } // end of method Index::GetName

  .method /*060000D8*/ family hidebysig virtual 
          instance void  InferIdFromName() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x3c9d
    // Code size       55 (0x37)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 72   | (70)0000B3       */ ldstr      "id" /* 700000B3 */
    IL_0006:  /* 72   | (70)0003B7       */ ldstr      "{0}_{1}" /* 700003B7 */
    IL_000b:  /* 02   |                  */ ldarg.0
    IL_000c:  /* 7B   | (0A)000119       */ ldfld      class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<class MvcContrib.FluentHtml.Elements.Hidden/*0200002D*/>/*1B00004C*/::builder /* 0A000119 */
    IL_0011:  /* 6F   | (0A)000076       */ callvirt   instance class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<string,string> [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/::get_Attributes() /* 0A000076 */
    IL_0016:  /* 72   | (70)000091       */ ldstr      "name" /* 70000091 */
    IL_001b:  /* 6F   | (0A)000095       */ callvirt   instance !1 class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<string,string>/*1B000047*/::get_Item(!0) /* 0A000095 */
    IL_0020:  /* 28   | (06)00010E       */ call       string MvcContrib.FluentHtml.StringExtensions/*0200003B*/::FormatAsHtmlId(string) /* 0600010E */
    IL_0025:  /* 02   |                  */ ldarg.0
    IL_0026:  /* 7B   | (0A)00011A       */ ldfld      object class MvcContrib.FluentHtml.Elements.Input`1/*02000020*/<class MvcContrib.FluentHtml.Elements.Hidden/*0200002D*/>/*1B00004D*/::elementValue /* 0A00011A */
    IL_002b:  /* 28   | (0A)0000F8       */ call       string [mscorlib/*23000001*/]System.String/*0100003F*/::Format(string,
                                                                                                                      object,
                                                                                                                      object) /* 0A0000F8 */
    IL_0030:  /* 6F   | (0A)00011B       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<class MvcContrib.FluentHtml.Elements.Hidden/*0200002D*/>/*1B00004C*/::Attr(string,
                                                                                                                                                                                                         object) /* 0A00011B */
    IL_0035:  /* 26   |                  */ pop
    IL_0036:  /* 2A   |                  */ ret
  } // end of method Index::InferIdFromName

} // end of class MvcContrib.FluentHtml.Elements.Index

.class /*0200002F*/ public auto ansi beforefieldinit MvcContrib.FluentHtml.Elements.FileUpload
       extends class MvcContrib.FluentHtml.Elements.TextInput`1/*0200002C*/<class MvcContrib.FluentHtml.Elements.FileUpload/*0200002F*/>/*1B000016*/
{
  .method /*060000D9*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x3cd5
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 72   | (70)00043B       */ ldstr      "file" /* 7000043B */
    IL_0006:  /* 03   |                  */ ldarg.1
    IL_0007:  /* 28   | (0A)00011C       */ call       instance void class MvcContrib.FluentHtml.Elements.TextInput`1/*0200002C*/<class MvcContrib.FluentHtml.Elements.FileUpload/*0200002F*/>/*1B000016*/::.ctor(string,
                                                                                                                                                                                                                  string) /* 0A00011C */
    IL_000c:  /* 2A   |                  */ ret
  } // end of method FileUpload::.ctor

  .method /*060000DA*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name,
                               class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> behaviors) cil managed
  // SIG: 20 02 01 0E 15 12 11 01 12 08
  {
    // Method begins at RVA 0x3ce3
    // Code size       15 (0xf)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 72   | (70)00043B       */ ldstr      "file" /* 7000043B */
    IL_0006:  /* 03   |                  */ ldarg.1
    IL_0007:  /* 14   |                  */ ldnull
    IL_0008:  /* 04   |                  */ ldarg.2
    IL_0009:  /* 28   | (0A)00011D       */ call       instance void class MvcContrib.FluentHtml.Elements.TextInput`1/*0200002C*/<class MvcContrib.FluentHtml.Elements.FileUpload/*0200002F*/>/*1B000016*/::.ctor(string,
                                                                                                                                                                                                                  string,
                                                                                                                                                                                                                  class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                                                                                                                  class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 0A00011D */
    IL_000e:  /* 2A   |                  */ ret
  } // end of method FileUpload::.ctor

} // end of class MvcContrib.FluentHtml.Elements.FileUpload

.class /*02000030*/ public abstract auto ansi beforefieldinit MvcContrib.FluentHtml.Elements.FormLiteralBase`1<(class MvcContrib.FluentHtml.Elements.FormLiteralBase`1/*02000030*/<!T>/*1B000017*/) T>
       extends class MvcContrib.FluentHtml.Elements.TextInput`1/*0200002C*/<!T>/*1B000014*/
{
  .field /*04000032*/ private initonly string _name
  .method /*060000DB*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x3cf3
    // Code size       20 (0x14)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 72   | (70)00041F       */ ldstr      "hidden" /* 7000041F */
    IL_0006:  /* 03   |                  */ ldarg.1
    IL_0007:  /* 28   | (0A)00011E       */ call       instance void class MvcContrib.FluentHtml.Elements.TextInput`1/*0200002C*/<!T>/*1B000014*/::.ctor(string,
                                                                                                                                                         string) /* 0A00011E */
    IL_000c:  /* 02   |                  */ ldarg.0
    IL_000d:  /* 03   |                  */ ldarg.1
    IL_000e:  /* 7D   | (0A)00011F       */ stfld      string class MvcContrib.FluentHtml.Elements.FormLiteralBase`1/*02000030*/<!T>/*1B000017*/::_name /* 0A00011F */
    IL_0013:  /* 2A   |                  */ ret
  } // end of method FormLiteralBase`1::.ctor

  .method /*060000DC*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(string name,
                               class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ forMember,
                               class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> behaviors) cil managed
  // SIG: 20 03 01 0E 12 39 15 12 11 01 12 08
  {
    // Method begins at RVA 0x3d08
    // Code size       22 (0x16)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 72   | (70)00041F       */ ldstr      "hidden" /* 7000041F */
    IL_0006:  /* 03   |                  */ ldarg.1
    IL_0007:  /* 04   |                  */ ldarg.2
    IL_0008:  /* 05   |                  */ ldarg.3
    IL_0009:  /* 28   | (0A)000120       */ call       instance void class MvcContrib.FluentHtml.Elements.TextInput`1/*0200002C*/<!T>/*1B000014*/::.ctor(string,
                                                                                                                                                         string,
                                                                                                                                                         class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                                                         class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 0A000120 */
    IL_000e:  /* 02   |                  */ ldarg.0
    IL_000f:  /* 03   |                  */ ldarg.1
    IL_0010:  /* 7D   | (0A)00011F       */ stfld      string class MvcContrib.FluentHtml.Elements.FormLiteralBase`1/*02000030*/<!T>/*1B000017*/::_name /* 0A00011F */
    IL_0015:  /* 2A   |                  */ ret
  } // end of method FormLiteralBase`1::.ctor

  .method /*060000DD*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x3d20
    // Code size       90 (0x5a)
    .maxstack  5
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)0000B4       */ call       instance string class MvcContrib.FluentHtml.Elements.FormElement`1/*0200001F*/<!T>/*1B000007*/::ToString() /* 0A0000B4 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 7B   | (0A)00011F       */ ldfld      string class MvcContrib.FluentHtml.Elements.FormLiteralBase`1/*02000030*/<!T>/*1B000017*/::_name /* 0A00011F */
    IL_000c:  /* 73   | (06)00012D       */ newobj     instance void MvcContrib.FluentHtml.Elements.Literal/*02000044*/::.ctor(string) /* 0600012D */
    IL_0011:  /* 72   | (70)000445       */ ldstr      "{0}_Literal" /* 70000445 */
    IL_0016:  /* 02   |                  */ ldarg.0
    IL_0017:  /* 7B   | (0A)000065       */ ldfld      class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::builder /* 0A000065 */
    IL_001c:  /* 6F   | (0A)000076       */ callvirt   instance class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<string,string> [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/::get_Attributes() /* 0A000076 */
    IL_0021:  /* 72   | (70)000091       */ ldstr      "name" /* 70000091 */
    IL_0026:  /* 6F   | (0A)000095       */ callvirt   instance !1 class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<string,string>/*1B000047*/::get_Item(!0) /* 0A000095 */
    IL_002b:  /* 28   | (06)00010E       */ call       string MvcContrib.FluentHtml.StringExtensions/*0200003B*/::FormatAsHtmlId(string) /* 0600010E */
    IL_0030:  /* 28   | (0A)000087       */ call       string [mscorlib/*23000001*/]System.String/*0100003F*/::Format(string,
                                                                                                                      object) /* 0A000087 */
    IL_0035:  /* 6F   | (0A)000121       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<class MvcContrib.FluentHtml.Elements.Literal/*02000044*/>/*1B00005D*/::Id(string) /* 0A000121 */
    IL_003a:  /* 02   |                  */ ldarg.0
    IL_003b:  /* 7B   | (0A)000065       */ ldfld      class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::builder /* 0A000065 */
    IL_0040:  /* 6F   | (0A)000076       */ callvirt   instance class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<string,string> [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/::get_Attributes() /* 0A000076 */
    IL_0045:  /* 72   | (70)00014F       */ ldstr      "value" /* 7000014F */
    IL_004a:  /* 6F   | (0A)000095       */ callvirt   instance !1 class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<string,string>/*1B000047*/::get_Item(!0) /* 0A000095 */
    IL_004f:  /* 6F   | (0A)000122       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.LiteralBase`1/*02000032*/<class MvcContrib.FluentHtml.Elements.Literal/*02000044*/>/*1B000022*/::Value(object) /* 0A000122 */
    IL_0054:  /* 28   | (0A)000123       */ call       string [mscorlib/*23000001*/]System.String/*0100003F*/::Concat(object,
                                                                                                                      object) /* 0A000123 */
    IL_0059:  /* 2A   |                  */ ret
  } // end of method FormLiteralBase`1::ToString

} // end of class MvcContrib.FluentHtml.Elements.FormLiteralBase`1

.class /*02000031*/ public auto ansi beforefieldinit MvcContrib.FluentHtml.Elements.FormLiteral
       extends class MvcContrib.FluentHtml.Elements.FormLiteralBase`1/*02000030*/<class MvcContrib.FluentHtml.Elements.FormLiteral/*02000031*/>/*1B000018*/
{
  .method /*060000DE*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name,
                               class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ forMember,
                               class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> behaviors) cil managed
  // SIG: 20 03 01 0E 12 39 15 12 11 01 12 08
  {
    // Method begins at RVA 0x3d86
    // Code size       10 (0xa)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 04   |                  */ ldarg.2
    IL_0003:  /* 05   |                  */ ldarg.3
    IL_0004:  /* 28   | (0A)000124       */ call       instance void class MvcContrib.FluentHtml.Elements.FormLiteralBase`1/*02000030*/<class MvcContrib.FluentHtml.Elements.FormLiteral/*02000031*/>/*1B000018*/::.ctor(string,
                                                                                                                                                                                                                         class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                                                                                                                         class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 0A000124 */
    IL_0009:  /* 2A   |                  */ ret
  } // end of method FormLiteral::.ctor

  .method /*060000DF*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x3d91
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 28   | (0A)000125       */ call       instance void class MvcContrib.FluentHtml.Elements.FormLiteralBase`1/*02000030*/<class MvcContrib.FluentHtml.Elements.FormLiteral/*02000031*/>/*1B000018*/::.ctor(string) /* 0A000125 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method FormLiteral::.ctor

} // end of class MvcContrib.FluentHtml.Elements.FormLiteral

.class /*02000032*/ public abstract auto ansi beforefieldinit MvcContrib.FluentHtml.Elements.LiteralBase`1<(class MvcContrib.FluentHtml.Elements.LiteralBase`1/*02000032*/<!T>/*1B000019*/) T>
       extends class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/
{
  .field /*04000033*/ family string format
  .field /*04000034*/ family object rawValue
  .field /*04000035*/ family string html
  .field /*04000036*/ family string overridenId
  .field /*04000037*/ family bool htmlWasSpecified
  .method /*060000E0*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(string name,
                               class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ forMember,
                               class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> behaviors) cil managed
  // SIG: 20 03 01 0E 12 39 15 12 11 01 12 08
  {
    // Method begins at RVA 0x3d9a
    // Code size       21 (0x15)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 72   | (70)00045D       */ ldstr      "span" /* 7000045D */
    IL_0006:  /* 04   |                  */ ldarg.2
    IL_0007:  /* 05   |                  */ ldarg.3
    IL_0008:  /* 28   | (0A)00009F       */ call       instance void class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::.ctor(string,
                                                                                                                                                       class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                                                       class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 0A00009F */
    IL_000d:  /* 02   |                  */ ldarg.0
    IL_000e:  /* 03   |                  */ ldarg.1
    IL_000f:  /* 28   | (0A)000126       */ call       instance void class MvcContrib.FluentHtml.Elements.LiteralBase`1/*02000032*/<!T>/*1B000019*/::SetName(string) /* 0A000126 */
    IL_0014:  /* 2A   |                  */ ret
  } // end of method LiteralBase`1::.ctor

  .method /*060000E1*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x3db0
    // Code size       19 (0x13)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 72   | (70)00045D       */ ldstr      "span" /* 7000045D */
    IL_0006:  /* 28   | (0A)00005F       */ call       instance void class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::.ctor(string) /* 0A00005F */
    IL_000b:  /* 02   |                  */ ldarg.0
    IL_000c:  /* 03   |                  */ ldarg.1
    IL_000d:  /* 28   | (0A)000126       */ call       instance void class MvcContrib.FluentHtml.Elements.LiteralBase`1/*02000032*/<!T>/*1B000019*/::SetName(string) /* 0A000126 */
    IL_0012:  /* 2A   |                  */ ret
  } // end of method LiteralBase`1::.ctor

  .method /*060000E2*/ family hidebysig instance void 
          SetName(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x3dc4
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 72   | (70)000091       */ ldstr      "name" /* 70000091 */
    IL_0006:  /* 03   |                  */ ldarg.1
    IL_0007:  /* 6F   | (06)000047       */ callvirt   instance void MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::SetAttr(string,
                                                                                                                                  object) /* 06000047 */
    IL_000c:  /* 2A   |                  */ ret
  } // end of method LiteralBase`1::SetName

  .method /*060000E3*/ public hidebysig newslot virtual 
          instance !T  Value(object 'value') cil managed
  // SIG: 20 01 13 00 1C
  {
    // Method begins at RVA 0x3dd2
    // Code size       14 (0xe)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (0A)000127       */ stfld      object class MvcContrib.FluentHtml.Elements.LiteralBase`1/*02000032*/<!T>/*1B000019*/::rawValue /* 0A000127 */
    IL_0007:  /* 02   |                  */ ldarg.0
    IL_0008:  /* A5   | (1B)000040       */ unbox.any  !T/*1B000040*/
    IL_000d:  /* 2A   |                  */ ret
  } // end of method LiteralBase`1::Value

  .method /*060000E4*/ public hidebysig newslot virtual 
          instance !T  Html(string 'value') cil managed
  // SIG: 20 01 13 00 0E
  {
    // Method begins at RVA 0x3de1
    // Code size       21 (0x15)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (0A)000128       */ stfld      string class MvcContrib.FluentHtml.Elements.LiteralBase`1/*02000032*/<!T>/*1B000019*/::html /* 0A000128 */
    IL_0007:  /* 02   |                  */ ldarg.0
    IL_0008:  /* 17   |                  */ ldc.i4.1
    IL_0009:  /* 7D   | (0A)000129       */ stfld      bool class MvcContrib.FluentHtml.Elements.LiteralBase`1/*02000032*/<!T>/*1B000019*/::htmlWasSpecified /* 0A000129 */
    IL_000e:  /* 02   |                  */ ldarg.0
    IL_000f:  /* A5   | (1B)000040       */ unbox.any  !T/*1B000040*/
    IL_0014:  /* 2A   |                  */ ret
  } // end of method LiteralBase`1::Html

  .method /*060000E5*/ public hidebysig newslot virtual 
          instance !T  Format(string 'value') cil managed
  // SIG: 20 01 13 00 0E
  {
    // Method begins at RVA 0x3df7
    // Code size       14 (0xe)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (0A)00012A       */ stfld      string class MvcContrib.FluentHtml.Elements.LiteralBase`1/*02000032*/<!T>/*1B000019*/::format /* 0A00012A */
    IL_0007:  /* 02   |                  */ ldarg.0
    IL_0008:  /* A5   | (1B)000040       */ unbox.any  !T/*1B000040*/
    IL_000d:  /* 2A   |                  */ ret
  } // end of method LiteralBase`1::Format

  .method /*060000E6*/ public hidebysig virtual 
          instance !T  Id(string 'value') cil managed
  // SIG: 20 01 13 00 0E
  {
    // Method begins at RVA 0x3e06
    // Code size       14 (0xe)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (0A)00012B       */ stfld      string class MvcContrib.FluentHtml.Elements.LiteralBase`1/*02000032*/<!T>/*1B000019*/::overridenId /* 0A00012B */
    IL_0007:  /* 02   |                  */ ldarg.0
    IL_0008:  /* A5   | (1B)000040       */ unbox.any  !T/*1B000040*/
    IL_000d:  /* 2A   |                  */ ret
  } // end of method LiteralBase`1::Id

  .method /*060000E7*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x3e15
    // Code size       63 (0x3f)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00012C       */ call       instance void class MvcContrib.FluentHtml.Elements.LiteralBase`1/*02000032*/<!T>/*1B000019*/::SetId() /* 0A00012C */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 7B   | (0A)000129       */ ldfld      bool class MvcContrib.FluentHtml.Elements.LiteralBase`1/*02000032*/<!T>/*1B000019*/::htmlWasSpecified /* 0A000129 */
    IL_000c:  /* 2C   | 13               */ brfalse.s  IL_0021

    IL_000e:  /* 02   |                  */ ldarg.0
    IL_000f:  /* 7B   | (0A)000065       */ ldfld      class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::builder /* 0A000065 */
    IL_0014:  /* 02   |                  */ ldarg.0
    IL_0015:  /* 7B   | (0A)000128       */ ldfld      string class MvcContrib.FluentHtml.Elements.LiteralBase`1/*02000032*/<!T>/*1B000019*/::html /* 0A000128 */
    IL_001a:  /* 6F   | (0A)0000F1       */ callvirt   instance void [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/::set_InnerHtml(string) /* 0A0000F1 */
    IL_001f:  /* 2B   | 17               */ br.s       IL_0038

    IL_0021:  /* 02   |                  */ ldarg.0
    IL_0022:  /* 7B   | (0A)000065       */ ldfld      class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::builder /* 0A000065 */
    IL_0027:  /* 02   |                  */ ldarg.0
    IL_0028:  /* 02   |                  */ ldarg.0
    IL_0029:  /* 7B   | (0A)000127       */ ldfld      object class MvcContrib.FluentHtml.Elements.LiteralBase`1/*02000032*/<!T>/*1B000019*/::rawValue /* 0A000127 */
    IL_002e:  /* 6F   | (0A)00012D       */ callvirt   instance string class MvcContrib.FluentHtml.Elements.LiteralBase`1/*02000032*/<!T>/*1B000019*/::FormatValue(object) /* 0A00012D */
    IL_0033:  /* 6F   | (0A)000093       */ callvirt   instance void [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/::SetInnerText(string) /* 0A000093 */
    IL_0038:  /* 02   |                  */ ldarg.0
    IL_0039:  /* 28   | (0A)0000A5       */ call       instance string class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::ToString() /* 0A0000A5 */
    IL_003e:  /* 2A   |                  */ ret
  } // end of method LiteralBase`1::ToString

  .method /*060000E8*/ family hidebysig newslot virtual 
          instance void  InferIdFromName() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x3e55
    // Code size       62 (0x3e)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (0A)000065       */ ldfld      class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::builder /* 0A000065 */
    IL_0006:  /* 6F   | (0A)000076       */ callvirt   instance class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<string,string> [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/::get_Attributes() /* 0A000076 */
    IL_000b:  /* 72   | (70)0000B3       */ ldstr      "id" /* 700000B3 */
    IL_0010:  /* 6F   | (0A)000094       */ callvirt   instance bool class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<string,string>/*1B000047*/::ContainsKey(!0) /* 0A000094 */
    IL_0015:  /* 2D   | 26               */ brtrue.s   IL_003d

    IL_0017:  /* 02   |                  */ ldarg.0
    IL_0018:  /* 72   | (70)0000B3       */ ldstr      "id" /* 700000B3 */
    IL_001d:  /* 02   |                  */ ldarg.0
    IL_001e:  /* 7B   | (0A)000065       */ ldfld      class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::builder /* 0A000065 */
    IL_0023:  /* 6F   | (0A)000076       */ callvirt   instance class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<string,string> [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/::get_Attributes() /* 0A000076 */
    IL_0028:  /* 72   | (70)000091       */ ldstr      "name" /* 70000091 */
    IL_002d:  /* 6F   | (0A)000095       */ callvirt   instance !1 class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<string,string>/*1B000047*/::get_Item(!0) /* 0A000095 */
    IL_0032:  /* 28   | (06)00010E       */ call       string MvcContrib.FluentHtml.StringExtensions/*0200003B*/::FormatAsHtmlId(string) /* 0600010E */
    IL_0037:  /* 6F   | (0A)0000A0       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::Attr(string,
                                                                                                                                                    object) /* 0A0000A0 */
    IL_003c:  /* 26   |                  */ pop
    IL_003d:  /* 2A   |                  */ ret
  } // end of method LiteralBase`1::InferIdFromName

  .method /*060000E9*/ private hidebysig 
          instance void  SetId() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x3e94
    // Code size       34 (0x22)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (0A)00012B       */ ldfld      string class MvcContrib.FluentHtml.Elements.LiteralBase`1/*02000032*/<!T>/*1B000019*/::overridenId /* 0A00012B */
    IL_0006:  /* 28   | (0A)000096       */ call       bool [mscorlib/*23000001*/]System.String/*0100003F*/::IsNullOrEmpty(string) /* 0A000096 */
    IL_000b:  /* 2D   | 0E               */ brtrue.s   IL_001b

    IL_000d:  /* 02   |                  */ ldarg.0
    IL_000e:  /* 02   |                  */ ldarg.0
    IL_000f:  /* 7B   | (0A)00012B       */ ldfld      string class MvcContrib.FluentHtml.Elements.LiteralBase`1/*02000032*/<!T>/*1B000019*/::overridenId /* 0A00012B */
    IL_0014:  /* 28   | (0A)00010A       */ call       instance !0 class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::Id(string) /* 0A00010A */
    IL_0019:  /* 26   |                  */ pop
    IL_001a:  /* 2A   |                  */ ret

    IL_001b:  /* 02   |                  */ ldarg.0
    IL_001c:  /* 6F   | (0A)00012E       */ callvirt   instance void class MvcContrib.FluentHtml.Elements.LiteralBase`1/*02000032*/<!T>/*1B000019*/::InferIdFromName() /* 0A00012E */
    IL_0021:  /* 2A   |                  */ ret
  } // end of method LiteralBase`1::SetId

  .method /*060000EA*/ family hidebysig newslot virtual 
          instance string  FormatValue(object 'value') cil managed
  // SIG: 20 01 0E 1C
  {
    // Method begins at RVA 0x3eb8
    // Code size       102 (0x66)
    .maxstack  3
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (0A)00012A       */ ldfld      string class MvcContrib.FluentHtml.Elements.LiteralBase`1/*02000032*/<!T>/*1B000019*/::format /* 0A00012A */
    IL_0006:  /* 28   | (0A)000096       */ call       bool [mscorlib/*23000001*/]System.String/*0100003F*/::IsNullOrEmpty(string) /* 0A000096 */
    IL_000b:  /* 2D   | 4D               */ brtrue.s   IL_005a

    IL_000d:  /* 02   |                  */ ldarg.0
    IL_000e:  /* 7B   | (0A)00012A       */ ldfld      string class MvcContrib.FluentHtml.Elements.LiteralBase`1/*02000032*/<!T>/*1B000019*/::format /* 0A00012A */
    IL_0013:  /* 72   | (70)0003ED       */ ldstr      "{0" /* 700003ED */
    IL_0018:  /* 6F   | (0A)00010B       */ callvirt   instance bool [mscorlib/*23000001*/]System.String/*0100003F*/::StartsWith(string) /* 0A00010B */
    IL_001d:  /* 2C   | 12               */ brfalse.s  IL_0031

    IL_001f:  /* 02   |                  */ ldarg.0
    IL_0020:  /* 7B   | (0A)00012A       */ ldfld      string class MvcContrib.FluentHtml.Elements.LiteralBase`1/*02000032*/<!T>/*1B000019*/::format /* 0A00012A */
    IL_0025:  /* 72   | (70)0003F3       */ ldstr      "}" /* 700003F3 */
    IL_002a:  /* 6F   | (0A)00010C       */ callvirt   instance bool [mscorlib/*23000001*/]System.String/*0100003F*/::EndsWith(string) /* 0A00010C */
    IL_002f:  /* 2D   | 1C               */ brtrue.s   IL_004d

    IL_0031:  /* 72   | (70)0003F7       */ ldstr      "{0:" /* 700003F7 */
    IL_0036:  /* 02   |                  */ ldarg.0
    IL_0037:  /* 7B   | (0A)00012A       */ ldfld      string class MvcContrib.FluentHtml.Elements.LiteralBase`1/*02000032*/<!T>/*1B000019*/::format /* 0A00012A */
    IL_003c:  /* 72   | (70)0003F3       */ ldstr      "}" /* 700003F3 */
    IL_0041:  /* 28   | (0A)00010D       */ call       string [mscorlib/*23000001*/]System.String/*0100003F*/::Concat(string,
                                                                                                                      string,
                                                                                                                      string) /* 0A00010D */
    IL_0046:  /* 03   |                  */ ldarg.1
    IL_0047:  /* 28   | (0A)000087       */ call       string [mscorlib/*23000001*/]System.String/*0100003F*/::Format(string,
                                                                                                                      object) /* 0A000087 */
    IL_004c:  /* 2A   |                  */ ret

    IL_004d:  /* 02   |                  */ ldarg.0
    IL_004e:  /* 7B   | (0A)00012A       */ ldfld      string class MvcContrib.FluentHtml.Elements.LiteralBase`1/*02000032*/<!T>/*1B000019*/::format /* 0A00012A */
    IL_0053:  /* 03   |                  */ ldarg.1
    IL_0054:  /* 28   | (0A)000087       */ call       string [mscorlib/*23000001*/]System.String/*0100003F*/::Format(string,
                                                                                                                      object) /* 0A000087 */
    IL_0059:  /* 2A   |                  */ ret

    IL_005a:  /* 03   |                  */ ldarg.1
    IL_005b:  /* 2C   | 07               */ brfalse.s  IL_0064

    IL_005d:  /* 03   |                  */ ldarg.1
    IL_005e:  /* 6F   | (0A)00006E       */ callvirt   instance string [mscorlib/*23000001*/]System.Object/*01000001*/::ToString() /* 0A00006E */
    IL_0063:  /* 2A   |                  */ ret

    IL_0064:  /* 14   |                  */ ldnull
    IL_0065:  /* 2A   |                  */ ret
  } // end of method LiteralBase`1::FormatValue

} // end of class MvcContrib.FluentHtml.Elements.LiteralBase`1

.class /*02000033*/ public abstract auto ansi beforefieldinit MvcContrib.FluentHtml.Elements.RadioButtonBase`1<(class MvcContrib.FluentHtml.Elements.RadioButtonBase`1/*02000033*/<!T>/*1B00001A*/) T>
       extends class MvcContrib.FluentHtml.Elements.Input`1/*02000020*/<!T>/*1B000008*/
{
  .field /*04000038*/ private string _format
  .method /*060000EB*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x3f2a
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7E   | (04)00004F       */ ldsfld     string MvcContrib.FluentHtml.Html.HtmlInputType/*02000042*/::Radio /* 0400004F */
    IL_0006:  /* 03   |                  */ ldarg.1
    IL_0007:  /* 28   | (0A)0000B0       */ call       instance void class MvcContrib.FluentHtml.Elements.Input`1/*02000020*/<!T>/*1B000008*/::.ctor(string,
                                                                                                                                                     string) /* 0A0000B0 */
    IL_000c:  /* 2A   |                  */ ret
  } // end of method RadioButtonBase`1::.ctor

  .method /*060000EC*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(string name,
                               class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ forMember,
                               class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> behaviors) cil managed
  // SIG: 20 03 01 0E 12 39 15 12 11 01 12 08
  {
    // Method begins at RVA 0x3f38
    // Code size       15 (0xf)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7E   | (04)00004F       */ ldsfld     string MvcContrib.FluentHtml.Html.HtmlInputType/*02000042*/::Radio /* 0400004F */
    IL_0006:  /* 03   |                  */ ldarg.1
    IL_0007:  /* 04   |                  */ ldarg.2
    IL_0008:  /* 05   |                  */ ldarg.3
    IL_0009:  /* 28   | (0A)0000AD       */ call       instance void class MvcContrib.FluentHtml.Elements.Input`1/*02000020*/<!T>/*1B000008*/::.ctor(string,
                                                                                                                                                     string,
                                                                                                                                                     class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                                                     class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 0A0000AD */
    IL_000e:  /* 2A   |                  */ ret
  } // end of method RadioButtonBase`1::.ctor

  .method /*060000ED*/ public hidebysig newslot virtual 
          instance !T  Checked(bool 'value') cil managed
  // SIG: 20 01 13 00 02
  {
    // Method begins at RVA 0x3f48
    // Code size       40 (0x28)
    .maxstack  8
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 2C   | 13               */ brfalse.s  IL_0016

    IL_0003:  /* 02   |                  */ ldarg.0
    IL_0004:  /* 72   | (70)000185       */ ldstr      "checked" /* 70000185 */
    IL_0009:  /* 72   | (70)000185       */ ldstr      "checked" /* 70000185 */
    IL_000e:  /* 6F   | (0A)0000A0       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::Attr(string,
                                                                                                                                                    object) /* 0A0000A0 */
    IL_0013:  /* 26   |                  */ pop
    IL_0014:  /* 2B   | 0B               */ br.s       IL_0021

    IL_0016:  /* 02   |                  */ ldarg.0
    IL_0017:  /* 72   | (70)000185       */ ldstr      "checked" /* 70000185 */
    IL_001c:  /* 6F   | (06)000049       */ callvirt   instance void MvcContrib.FluentHtml.Elements.IElement/*0200001B*/::RemoveAttr(string) /* 06000049 */
    IL_0021:  /* 02   |                  */ ldarg.0
    IL_0022:  /* A5   | (1B)000040       */ unbox.any  !T/*1B000040*/
    IL_0027:  /* 2A   |                  */ ret
  } // end of method RadioButtonBase`1::Checked

  .method /*060000EE*/ family hidebysig virtual 
          instance void  ApplyModelState(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelState/*0100000F*/ state) cil managed
  // SIG: 20 01 01 12 3D
  {
    // Method begins at RVA 0x3f74
    // Code size       49 (0x31)
    .maxstack  3
    .locals /*1100001F*/ init (object V_0,
             string V_1)
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 6F   | (0A)00005C       */ callvirt   instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ValueProviderResult/*01000050*/ [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelState/*0100000F*/::get_Value() /* 0A00005C */
    IL_0006:  /* D0   | (01)00003F       */ ldtoken    [mscorlib/*23000001*/]System.String/*0100003F*/
    IL_000b:  /* 28   | (0A)000021       */ call       class [mscorlib/*23000001*/]System.Type/*01000003*/ [mscorlib/*23000001*/]System.Type/*01000003*/::GetTypeFromHandle(valuetype [mscorlib/*23000001*/]System.RuntimeTypeHandle/*01000036*/) /* 0A000021 */
    IL_0010:  /* 6F   | (0A)0000AA       */ callvirt   instance object [System.Web.Mvc/*23000003*/]System.Web.Mvc.ValueProviderResult/*01000050*/::ConvertTo(class [mscorlib/*23000001*/]System.Type/*01000003*/) /* 0A0000AA */
    IL_0015:  /* 0A   |                  */ stloc.0
    IL_0016:  /* 02   |                  */ ldarg.0
    IL_0017:  /* 7B   | (0A)0000A8       */ ldfld      object class MvcContrib.FluentHtml.Elements.Input`1/*02000020*/<!T>/*1B000008*/::elementValue /* 0A0000A8 */
    IL_001c:  /* 6F   | (0A)00006E       */ callvirt   instance string [mscorlib/*23000001*/]System.Object/*01000001*/::ToString() /* 0A00006E */
    IL_0021:  /* 0B   |                  */ stloc.1
    IL_0022:  /* 02   |                  */ ldarg.0
    IL_0023:  /* 06   |                  */ ldloc.0
    IL_0024:  /* 07   |                  */ ldloc.1
    IL_0025:  /* 28   | (0A)00012F       */ call       bool [mscorlib/*23000001*/]System.Object/*01000001*/::Equals(object,
                                                                                                                    object) /* 0A00012F */
    IL_002a:  /* 6F   | (0A)000130       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.RadioButtonBase`1/*02000033*/<!T>/*1B00001A*/::Checked(bool) /* 0A000130 */
    IL_002f:  /* 26   |                  */ pop
    IL_0030:  /* 2A   |                  */ ret
  } // end of method RadioButtonBase`1::ApplyModelState

  .method /*060000EF*/ public hidebysig newslot virtual 
          instance !T  Format(string format) cil managed
  // SIG: 20 01 13 00 0E
  {
    // Method begins at RVA 0x3fb1
    // Code size       14 (0xe)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (0A)000131       */ stfld      string class MvcContrib.FluentHtml.Elements.RadioButtonBase`1/*02000033*/<!T>/*1B00001A*/::_format /* 0A000131 */
    IL_0007:  /* 02   |                  */ ldarg.0
    IL_0008:  /* A5   | (1B)000040       */ unbox.any  !T/*1B000040*/
    IL_000d:  /* 2A   |                  */ ret
  } // end of method RadioButtonBase`1::Format

  .method /*060000F0*/ family hidebysig virtual 
          instance void  InferIdFromName() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x3fc0
    // Code size       99 (0x63)
    .maxstack  6
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (0A)000065       */ ldfld      class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::builder /* 0A000065 */
    IL_0006:  /* 6F   | (0A)000076       */ callvirt   instance class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<string,string> [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/::get_Attributes() /* 0A000076 */
    IL_000b:  /* 72   | (70)0000B3       */ ldstr      "id" /* 700000B3 */
    IL_0010:  /* 6F   | (0A)000094       */ callvirt   instance bool class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<string,string>/*1B000047*/::ContainsKey(!0) /* 0A000094 */
    IL_0015:  /* 2D   | 4B               */ brtrue.s   IL_0062

    IL_0017:  /* 02   |                  */ ldarg.0
    IL_0018:  /* 72   | (70)0000B3       */ ldstr      "id" /* 700000B3 */
    IL_001d:  /* 72   | (70)000467       */ ldstr      "{0}{1}" /* 70000467 */
    IL_0022:  /* 02   |                  */ ldarg.0
    IL_0023:  /* 7B   | (0A)000065       */ ldfld      class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::builder /* 0A000065 */
    IL_0028:  /* 6F   | (0A)000076       */ callvirt   instance class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<string,string> [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/::get_Attributes() /* 0A000076 */
    IL_002d:  /* 72   | (70)000091       */ ldstr      "name" /* 70000091 */
    IL_0032:  /* 6F   | (0A)000095       */ callvirt   instance !1 class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<string,string>/*1B000047*/::get_Item(!0) /* 0A000095 */
    IL_0037:  /* 02   |                  */ ldarg.0
    IL_0038:  /* 7B   | (0A)0000A8       */ ldfld      object class MvcContrib.FluentHtml.Elements.Input`1/*02000020*/<!T>/*1B000008*/::elementValue /* 0A0000A8 */
    IL_003d:  /* 2C   | 12               */ brfalse.s  IL_0051

    IL_003f:  /* 72   | (70)000475       */ ldstr      "_{0}" /* 70000475 */
    IL_0044:  /* 02   |                  */ ldarg.0
    IL_0045:  /* 7B   | (0A)0000A8       */ ldfld      object class MvcContrib.FluentHtml.Elements.Input`1/*02000020*/<!T>/*1B000008*/::elementValue /* 0A0000A8 */
    IL_004a:  /* 28   | (0A)000087       */ call       string [mscorlib/*23000001*/]System.String/*0100003F*/::Format(string,
                                                                                                                      object) /* 0A000087 */
    IL_004f:  /* 2B   | 01               */ br.s       IL_0052

    IL_0051:  /* 14   |                  */ ldnull
    IL_0052:  /* 28   | (0A)0000F8       */ call       string [mscorlib/*23000001*/]System.String/*0100003F*/::Format(string,
                                                                                                                      object,
                                                                                                                      object) /* 0A0000F8 */
    IL_0057:  /* 28   | (06)00010E       */ call       string MvcContrib.FluentHtml.StringExtensions/*0200003B*/::FormatAsHtmlId(string) /* 0600010E */
    IL_005c:  /* 6F   | (0A)0000A0       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::Attr(string,
                                                                                                                                                    object) /* 0A0000A0 */
    IL_0061:  /* 26   |                  */ pop
    IL_0062:  /* 2A   |                  */ ret
  } // end of method RadioButtonBase`1::InferIdFromName

  .method /*060000F1*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x402f
    // Code size       33 (0x21)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (0A)000131       */ ldfld      string class MvcContrib.FluentHtml.Elements.RadioButtonBase`1/*02000033*/<!T>/*1B00001A*/::_format /* 0A000131 */
    IL_0006:  /* 2C   | 12               */ brfalse.s  IL_001a

    IL_0008:  /* 02   |                  */ ldarg.0
    IL_0009:  /* 7B   | (0A)000131       */ ldfld      string class MvcContrib.FluentHtml.Elements.RadioButtonBase`1/*02000033*/<!T>/*1B00001A*/::_format /* 0A000131 */
    IL_000e:  /* 02   |                  */ ldarg.0
    IL_000f:  /* 28   | (0A)0000B4       */ call       instance string class MvcContrib.FluentHtml.Elements.FormElement`1/*0200001F*/<!T>/*1B000007*/::ToString() /* 0A0000B4 */
    IL_0014:  /* 28   | (0A)000087       */ call       string [mscorlib/*23000001*/]System.String/*0100003F*/::Format(string,
                                                                                                                      object) /* 0A000087 */
    IL_0019:  /* 2A   |                  */ ret

    IL_001a:  /* 02   |                  */ ldarg.0
    IL_001b:  /* 28   | (0A)0000B4       */ call       instance string class MvcContrib.FluentHtml.Elements.FormElement`1/*0200001F*/<!T>/*1B000007*/::ToString() /* 0A0000B4 */
    IL_0020:  /* 2A   |                  */ ret
  } // end of method RadioButtonBase`1::ToString

} // end of class MvcContrib.FluentHtml.Elements.RadioButtonBase`1

.class /*02000034*/ public auto ansi beforefieldinit MvcContrib.FluentHtml.Elements.RadioButton
       extends class MvcContrib.FluentHtml.Elements.RadioButtonBase`1/*02000033*/<class MvcContrib.FluentHtml.Elements.RadioButton/*02000034*/>/*1B00001B*/
{
  .method /*060000F2*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x4051
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 28   | (0A)000132       */ call       instance void class MvcContrib.FluentHtml.Elements.RadioButtonBase`1/*02000033*/<class MvcContrib.FluentHtml.Elements.RadioButton/*02000034*/>/*1B00001B*/::.ctor(string) /* 0A000132 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method RadioButton::.ctor

  .method /*060000F3*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name,
                               class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ forMember,
                               class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> behaviors) cil managed
  // SIG: 20 03 01 0E 12 39 15 12 11 01 12 08
  {
    // Method begins at RVA 0x405a
    // Code size       10 (0xa)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 04   |                  */ ldarg.2
    IL_0003:  /* 05   |                  */ ldarg.3
    IL_0004:  /* 28   | (0A)000133       */ call       instance void class MvcContrib.FluentHtml.Elements.RadioButtonBase`1/*02000033*/<class MvcContrib.FluentHtml.Elements.RadioButton/*02000034*/>/*1B00001B*/::.ctor(string,
                                                                                                                                                                                                                         class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                                                                                                                         class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 0A000133 */
    IL_0009:  /* 2A   |                  */ ret
  } // end of method RadioButton::.ctor

} // end of class MvcContrib.FluentHtml.Elements.RadioButton

.class /*02000035*/ public abstract auto ansi beforefieldinit MvcContrib.FluentHtml.Elements.RadioSetBase`1<(class MvcContrib.FluentHtml.Elements.RadioSetBase`1/*02000035*/<!T>/*1B00001C*/) T>
       extends class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/
{
  .field /*04000039*/ family string _format
  .field /*0400003A*/ family string _itemClass
  .field /*0400003B*/ family class [System.Core/*23000002*/]System.Action`3/*01000019*/<class MvcContrib.FluentHtml.Elements.RadioButton/*02000034*/,object,int32> _optionModifier
  .method /*060000F4*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(string tag,
                               string name,
                               class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ forMember,
                               class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> behaviors) cil managed
  // SIG: 20 04 01 0E 0E 12 39 15 12 11 01 12 08
  {
    // Method begins at RVA 0x4065
    // Code size       12 (0xc)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 04   |                  */ ldarg.2
    IL_0003:  /* 05   |                  */ ldarg.3
    IL_0004:  /* 0E   | 04               */ ldarg.s    behaviors
    IL_0006:  /* 28   | (0A)0000F2       */ call       instance void class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/::.ctor(string,
                                                                                                                                                                  string,
                                                                                                                                                                  class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                                                                  class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 0A0000F2 */
    IL_000b:  /* 2A   |                  */ ret
  } // end of method RadioSetBase`1::.ctor

  .method /*060000F5*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(string tag,
                               string name) cil managed
  // SIG: 20 02 01 0E 0E
  {
    // Method begins at RVA 0x4072
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 04   |                  */ ldarg.2
    IL_0003:  /* 28   | (0A)0000F3       */ call       instance void class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/::.ctor(string,
                                                                                                                                                                  string) /* 0A0000F3 */
    IL_0008:  /* 2A   |                  */ ret
  } // end of method RadioSetBase`1::.ctor

  .method /*060000F6*/ public hidebysig newslot virtual 
          instance !T  Selected(object selectedValue) cil managed
  // SIG: 20 01 13 00 1C
  {
    // Method begins at RVA 0x407c
    // Code size       27 (0x1b)
    .maxstack  3
    .locals /*11000020*/ init (class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000021*/<object> V_0)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 73   | (0A)000134       */ newobj     instance void class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000021*/<object>/*1B00005E*/::.ctor() /* 0A000134 */
    IL_0006:  /* 0A   |                  */ stloc.0
    IL_0007:  /* 06   |                  */ ldloc.0
    IL_0008:  /* 03   |                  */ ldarg.1
    IL_0009:  /* 6F   | (0A)000135       */ callvirt   instance void class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000021*/<object>/*1B00005E*/::Add(!0) /* 0A000135 */
    IL_000e:  /* 06   |                  */ ldloc.0
    IL_000f:  /* 7D   | (0A)0000BA       */ stfld      class [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000015*/ class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/::_selectedValues /* 0A0000BA */
    IL_0014:  /* 02   |                  */ ldarg.0
    IL_0015:  /* A5   | (1B)000040       */ unbox.any  !T/*1B000040*/
    IL_001a:  /* 2A   |                  */ ret
  } // end of method RadioSetBase`1::Selected

  .method /*060000F7*/ public hidebysig newslot virtual 
          instance !T  ItemFormat(string format) cil managed
  // SIG: 20 01 13 00 0E
  {
    // Method begins at RVA 0x40a3
    // Code size       14 (0xe)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (0A)000136       */ stfld      string class MvcContrib.FluentHtml.Elements.RadioSetBase`1/*02000035*/<!T>/*1B00001C*/::_format /* 0A000136 */
    IL_0007:  /* 02   |                  */ ldarg.0
    IL_0008:  /* A5   | (1B)000040       */ unbox.any  !T/*1B000040*/
    IL_000d:  /* 2A   |                  */ ret
  } // end of method RadioSetBase`1::ItemFormat

  .method /*060000F8*/ public hidebysig newslot virtual 
          instance !T  ItemClass(string 'value') cil managed
  // SIG: 20 01 13 00 0E
  {
    // Method begins at RVA 0x40b2
    // Code size       14 (0xe)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (0A)000137       */ stfld      string class MvcContrib.FluentHtml.Elements.RadioSetBase`1/*02000035*/<!T>/*1B00001C*/::_itemClass /* 0A000137 */
    IL_0007:  /* 02   |                  */ ldarg.0
    IL_0008:  /* A5   | (1B)000040       */ unbox.any  !T/*1B000040*/
    IL_000d:  /* 2A   |                  */ ret
  } // end of method RadioSetBase`1::ItemClass

  .method /*060000F9*/ public hidebysig newslot virtual 
          instance !T  EachOption(class [System.Core/*23000002*/]System.Action`3/*01000019*/<class MvcContrib.FluentHtml.Elements.RadioButton/*02000034*/,object,int32> action) cil managed
  // SIG: 20 01 13 00 15 12 65 03 12 80 D0 1C 08
  {
    // Method begins at RVA 0x40c1
    // Code size       14 (0xe)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (0A)000138       */ stfld      class [System.Core/*23000002*/]System.Action`3/*01000019*/<class MvcContrib.FluentHtml.Elements.RadioButton/*02000034*/,object,int32> class MvcContrib.FluentHtml.Elements.RadioSetBase`1/*02000035*/<!T>/*1B00001C*/::_optionModifier /* 0A000138 */
    IL_0007:  /* 02   |                  */ ldarg.0
    IL_0008:  /* A5   | (1B)000040       */ unbox.any  !T/*1B000040*/
    IL_000d:  /* 2A   |                  */ ret
  } // end of method RadioSetBase`1::EachOption

  .method /*060000FA*/ family hidebysig specialname virtual 
          instance valuetype [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagRenderMode/*01000013*/ 
          get_TagRenderMode() cil managed
  // SIG: 20 00 11 4D
  {
    // Method begins at RVA 0x40d0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 16   |                  */ ldc.i4.0
    IL_0001:  /* 2A   |                  */ ret
  } // end of method RadioSetBase`1::get_TagRenderMode

  .method /*060000FB*/ family hidebysig virtual 
          instance string  RenderOptions() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x40d4
    // Code size       271 (0x10f)
    .maxstack  4
    .locals /*11000021*/ init (string V_0,
             class [mscorlib/*23000001*/]System.Text.StringBuilder/*01000055*/ V_1,
             int32 V_2,
             object V_3,
             object V_4,
             class MvcContrib.FluentHtml.Elements.RadioButton/*02000034*/ V_5,
             class [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000034*/ V_6,
             class [mscorlib/*23000001*/]System.IDisposable/*01000035*/ V_7)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (0A)000065       */ ldfld      class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::builder /* 0A000065 */
    IL_0006:  /* 6F   | (0A)000076       */ callvirt   instance class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<string,string> [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/::get_Attributes() /* 0A000076 */
    IL_000b:  /* 72   | (70)000091       */ ldstr      "name" /* 70000091 */
    IL_0010:  /* 6F   | (0A)000095       */ callvirt   instance !1 class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<string,string>/*1B000047*/::get_Item(!0) /* 0A000095 */
    IL_0015:  /* 0A   |                  */ stloc.0
    IL_0016:  /* 02   |                  */ ldarg.0
    IL_0017:  /* 7B   | (0A)000065       */ ldfld      class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::builder /* 0A000065 */
    IL_001c:  /* 6F   | (0A)000076       */ callvirt   instance class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<string,string> [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/::get_Attributes() /* 0A000076 */
    IL_0021:  /* 72   | (70)000091       */ ldstr      "name" /* 70000091 */
    IL_0026:  /* 6F   | (0A)000077       */ callvirt   instance bool class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<string,string>/*1B000047*/::Remove(!0) /* 0A000077 */
    IL_002b:  /* 26   |                  */ pop
    IL_002c:  /* 73   | (0A)000067       */ newobj     instance void [mscorlib/*23000001*/]System.Text.StringBuilder/*01000055*/::.ctor() /* 0A000067 */
    IL_0031:  /* 0B   |                  */ stloc.1
    IL_0032:  /* 16   |                  */ ldc.i4.0
    IL_0033:  /* 0C   |                  */ stloc.2
    IL_0034:  /* 02   |                  */ ldarg.0
    IL_0035:  /* 7B   | (0A)0000BC       */ ldfld      class [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000015*/ class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/::_options /* 0A0000BC */
    IL_003a:  /* 6F   | (0A)0000EB       */ callvirt   instance class [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000034*/ [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000015*/::GetEnumerator() /* 0A0000EB */
    IL_003f:  /* 13   | 06               */ stloc.s    V_6
    .try
    {
      IL_0041:  /* 38   | 9F000000         */ br         IL_00e5

      IL_0046:  /* 11   | 06               */ ldloc.s    V_6
      IL_0048:  /* 6F   | (0A)0000E5       */ callvirt   instance object [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000034*/::get_Current() /* 0A0000E5 */
      IL_004d:  /* 0D   |                  */ stloc.3
      IL_004e:  /* 02   |                  */ ldarg.0
      IL_004f:  /* 7B   | (0A)0000E0       */ ldfld      class [System.Core/*23000002*/]System.Func`2/*0100000B*/<object,object> class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/::_valueFieldSelector /* 0A0000E0 */
      IL_0054:  /* 09   |                  */ ldloc.3
      IL_0055:  /* 6F   | (0A)0000F7       */ callvirt   instance !1 class [System.Core/*23000002*/]System.Func`2/*0100000B*/<object,object>/*1B000057*/::Invoke(!0) /* 0A0000F7 */
      IL_005a:  /* 13   | 04               */ stloc.s    V_4
      IL_005c:  /* 06   |                  */ ldloc.0
      IL_005d:  /* 02   |                  */ ldarg.0
      IL_005e:  /* 7B   | (0A)000060       */ ldfld      class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::forMember /* 0A000060 */
      IL_0063:  /* 02   |                  */ ldarg.0
      IL_0064:  /* 7B   | (0A)000061       */ ldfld      class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::behaviors /* 0A000061 */
      IL_0069:  /* 73   | (06)0000F3       */ newobj     instance void MvcContrib.FluentHtml.Elements.RadioButton/*02000034*/::.ctor(string,
                                                                                                                                     class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                                     class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 060000F3 */
      IL_006e:  /* 11   | 04               */ ldloc.s    V_4
      IL_0070:  /* 6F   | (0A)000139       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.Input`1/*02000020*/<class MvcContrib.FluentHtml.Elements.RadioButton/*02000034*/>/*1B00005F*/::Value(object) /* 0A000139 */
      IL_0075:  /* 02   |                  */ ldarg.0
      IL_0076:  /* 7B   | (0A)000136       */ ldfld      string class MvcContrib.FluentHtml.Elements.RadioSetBase`1/*02000035*/<!T>/*1B00001C*/::_format /* 0A000136 */
      IL_007b:  /* 6F   | (0A)00013A       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.RadioButtonBase`1/*02000033*/<class MvcContrib.FluentHtml.Elements.RadioButton/*02000034*/>/*1B00001B*/::Format(string) /* 0A00013A */
      IL_0080:  /* 02   |                  */ ldarg.0
      IL_0081:  /* 7B   | (0A)0000DE       */ ldfld      class [System.Core/*23000002*/]System.Func`2/*0100000B*/<object,object> class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/::_textFieldSelector /* 0A0000DE */
      IL_0086:  /* 09   |                  */ ldloc.3
      IL_0087:  /* 6F   | (0A)0000F7       */ callvirt   instance !1 class [System.Core/*23000002*/]System.Func`2/*0100000B*/<object,object>/*1B000057*/::Invoke(!0) /* 0A0000F7 */
      IL_008c:  /* 6F   | (0A)00006E       */ callvirt   instance string [mscorlib/*23000001*/]System.Object/*01000001*/::ToString() /* 0A00006E */
      IL_0091:  /* 02   |                  */ ldarg.0
      IL_0092:  /* 7B   | (0A)000137       */ ldfld      string class MvcContrib.FluentHtml.Elements.RadioSetBase`1/*02000035*/<!T>/*1B00001C*/::_itemClass /* 0A000137 */
      IL_0097:  /* 6F   | (0A)00013B       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<class MvcContrib.FluentHtml.Elements.RadioButton/*02000034*/>/*1B000060*/::LabelAfter(string,
                                                                                                                                                                                                                      string) /* 0A00013B */
      IL_009c:  /* 02   |                  */ ldarg.0
      IL_009d:  /* 11   | 04               */ ldloc.s    V_4
      IL_009f:  /* 28   | (0A)0000FC       */ call       instance bool class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/::IsSelectedValue(object) /* 0A0000FC */
      IL_00a4:  /* 6F   | (0A)00013C       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.RadioButtonBase`1/*02000033*/<class MvcContrib.FluentHtml.Elements.RadioButton/*02000034*/>/*1B00001B*/::Checked(bool) /* 0A00013C */
      IL_00a9:  /* 13   | 05               */ stloc.s    V_5
      IL_00ab:  /* 02   |                  */ ldarg.0
      IL_00ac:  /* 7B   | (0A)000137       */ ldfld      string class MvcContrib.FluentHtml.Elements.RadioSetBase`1/*02000035*/<!T>/*1B00001C*/::_itemClass /* 0A000137 */
      IL_00b1:  /* 2C   | 0E               */ brfalse.s  IL_00c1

      IL_00b3:  /* 11   | 05               */ ldloc.s    V_5
      IL_00b5:  /* 02   |                  */ ldarg.0
      IL_00b6:  /* 7B   | (0A)000137       */ ldfld      string class MvcContrib.FluentHtml.Elements.RadioSetBase`1/*02000035*/<!T>/*1B00001C*/::_itemClass /* 0A000137 */
      IL_00bb:  /* 6F   | (0A)00013D       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<class MvcContrib.FluentHtml.Elements.RadioButton/*02000034*/>/*1B000060*/::Class(string) /* 0A00013D */
      IL_00c0:  /* 26   |                  */ pop
      IL_00c1:  /* 02   |                  */ ldarg.0
      IL_00c2:  /* 7B   | (0A)000138       */ ldfld      class [System.Core/*23000002*/]System.Action`3/*01000019*/<class MvcContrib.FluentHtml.Elements.RadioButton/*02000034*/,object,int32> class MvcContrib.FluentHtml.Elements.RadioSetBase`1/*02000035*/<!T>/*1B00001C*/::_optionModifier /* 0A000138 */
      IL_00c7:  /* 2C   | 0F               */ brfalse.s  IL_00d8

      IL_00c9:  /* 02   |                  */ ldarg.0
      IL_00ca:  /* 7B   | (0A)000138       */ ldfld      class [System.Core/*23000002*/]System.Action`3/*01000019*/<class MvcContrib.FluentHtml.Elements.RadioButton/*02000034*/,object,int32> class MvcContrib.FluentHtml.Elements.RadioSetBase`1/*02000035*/<!T>/*1B00001C*/::_optionModifier /* 0A000138 */
      IL_00cf:  /* 11   | 05               */ ldloc.s    V_5
      IL_00d1:  /* 09   |                  */ ldloc.3
      IL_00d2:  /* 08   |                  */ ldloc.2
      IL_00d3:  /* 6F   | (0A)00013E       */ callvirt   instance void class [System.Core/*23000002*/]System.Action`3/*01000019*/<class MvcContrib.FluentHtml.Elements.RadioButton/*02000034*/,object,int32>/*1B000061*/::Invoke(!0,
                                                                                                                                                                                                                                 !1,
                                                                                                                                                                                                                                 !2) /* 0A00013E */
      IL_00d8:  /* 07   |                  */ ldloc.1
      IL_00d9:  /* 11   | 05               */ ldloc.s    V_5
      IL_00db:  /* 6F   | (0A)00013F       */ callvirt   instance class [mscorlib/*23000001*/]System.Text.StringBuilder/*01000055*/ [mscorlib/*23000001*/]System.Text.StringBuilder/*01000055*/::Append(object) /* 0A00013F */
      IL_00e0:  /* 26   |                  */ pop
      IL_00e1:  /* 08   |                  */ ldloc.2
      IL_00e2:  /* 17   |                  */ ldc.i4.1
      IL_00e3:  /* 58   |                  */ add
      IL_00e4:  /* 0C   |                  */ stloc.2
      IL_00e5:  /* 11   | 06               */ ldloc.s    V_6
      IL_00e7:  /* 6F   | (0A)00001A       */ callvirt   instance bool [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000034*/::MoveNext() /* 0A00001A */
      IL_00ec:  /* 3A   | 55FFFFFF         */ brtrue     IL_0046

      IL_00f1:  /* DE   | 15               */ leave.s    IL_0108

    }  // end .try
    finally
    {
      IL_00f3:  /* 11   | 06               */ ldloc.s    V_6
      IL_00f5:  /* 75   | (01)000035       */ isinst     [mscorlib/*23000001*/]System.IDisposable/*01000035*/
      IL_00fa:  /* 13   | 07               */ stloc.s    V_7
      IL_00fc:  /* 11   | 07               */ ldloc.s    V_7
      IL_00fe:  /* 2C   | 07               */ brfalse.s  IL_0107

      IL_0100:  /* 11   | 07               */ ldloc.s    V_7
      IL_0102:  /* 6F   | (0A)00001B       */ callvirt   instance void [mscorlib/*23000001*/]System.IDisposable/*01000035*/::Dispose() /* 0A00001B */
      IL_0107:  /* DC   |                  */ endfinally
    }  // end handler
    // HEX: 02 00 41 00 B2 F3 00 15 00 00 00 00
    IL_0108:  /* 07   |                  */ ldloc.1
    IL_0109:  /* 6F   | (0A)00006E       */ callvirt   instance string [mscorlib/*23000001*/]System.Object/*01000001*/::ToString() /* 0A00006E */
    IL_010e:  /* 2A   |                  */ ret
  } // end of method RadioSetBase`1::RenderOptions

  .property /*17000019*/ instance valuetype [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagRenderMode/*01000013*/
          TagRenderMode()
  {
    .get instance valuetype [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagRenderMode/*01000013*/ MvcContrib.FluentHtml.Elements.RadioSetBase`1/*02000035*/::get_TagRenderMode() /* 060000FA */
  } // end of property RadioSetBase`1::TagRenderMode
} // end of class MvcContrib.FluentHtml.Elements.RadioSetBase`1

.class /*02000036*/ public auto ansi beforefieldinit MvcContrib.FluentHtml.Elements.RadioSet
       extends class MvcContrib.FluentHtml.Elements.RadioSetBase`1/*02000035*/<class MvcContrib.FluentHtml.Elements.RadioSet/*02000036*/>/*1B00001D*/
{
  .method /*060000FC*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x4200
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 72   | (70)0003C7       */ ldstr      "div" /* 700003C7 */
    IL_0006:  /* 03   |                  */ ldarg.1
    IL_0007:  /* 28   | (0A)000140       */ call       instance void class MvcContrib.FluentHtml.Elements.RadioSetBase`1/*02000035*/<class MvcContrib.FluentHtml.Elements.RadioSet/*02000036*/>/*1B00001D*/::.ctor(string,
                                                                                                                                                                                                                   string) /* 0A000140 */
    IL_000c:  /* 2A   |                  */ ret
  } // end of method RadioSet::.ctor

  .method /*060000FD*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name,
                               class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ forMember,
                               class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> behaviors) cil managed
  // SIG: 20 03 01 0E 12 39 15 12 11 01 12 08
  {
    // Method begins at RVA 0x420e
    // Code size       15 (0xf)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 72   | (70)0003C7       */ ldstr      "div" /* 700003C7 */
    IL_0006:  /* 03   |                  */ ldarg.1
    IL_0007:  /* 04   |                  */ ldarg.2
    IL_0008:  /* 05   |                  */ ldarg.3
    IL_0009:  /* 28   | (0A)000141       */ call       instance void class MvcContrib.FluentHtml.Elements.RadioSetBase`1/*02000035*/<class MvcContrib.FluentHtml.Elements.RadioSet/*02000036*/>/*1B00001D*/::.ctor(string,
                                                                                                                                                                                                                   string,
                                                                                                                                                                                                                   class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                                                                                                                   class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 0A000141 */
    IL_000e:  /* 2A   |                  */ ret
  } // end of method RadioSet::.ctor

} // end of class MvcContrib.FluentHtml.Elements.RadioSet

.class /*02000037*/ public auto ansi beforefieldinit MvcContrib.FluentHtml.Elements.TypeStamp
       extends class MvcContrib.FluentHtml.Elements.TextInput`1/*0200002C*/<class MvcContrib.FluentHtml.Elements.TypeStamp/*02000037*/>/*1B00001E*/
{
  .field /*0400003C*/ public static literal string TypeStampKey = "_xTypeStampx_"
  .method /*060000FE*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name,
                               class [mscorlib/*23000001*/]System.Type/*01000003*/ instanceType) cil managed
  // SIG: 20 02 01 0E 12 0D
  {
    // Method begins at RVA 0x421e
    // Code size       31 (0x1f)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 72   | (70)00041F       */ ldstr      "hidden" /* 7000041F */
    IL_0006:  /* 03   |                  */ ldarg.1
    IL_0007:  /* 28   | (06)0000FF       */ call       string MvcContrib.FluentHtml.Elements.TypeStamp/*02000037*/::GenerateBinderContextProperty(string) /* 060000FF */
    IL_000c:  /* 28   | (0A)000142       */ call       instance void class MvcContrib.FluentHtml.Elements.TextInput`1/*0200002C*/<class MvcContrib.FluentHtml.Elements.TypeStamp/*02000037*/>/*1B00001E*/::.ctor(string,
                                                                                                                                                                                                                 string) /* 0A000142 */
    IL_0011:  /* 02   |                  */ ldarg.0
    IL_0012:  /* 04   |                  */ ldarg.2
    IL_0013:  /* 6F   | (0A)000143       */ callvirt   instance string [mscorlib/*23000001*/]System.Type/*01000003*/::get_FullName() /* 0A000143 */
    IL_0018:  /* 6F   | (0A)000144       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.Input`1/*02000020*/<class MvcContrib.FluentHtml.Elements.TypeStamp/*02000037*/>/*1B000062*/::Value(object) /* 0A000144 */
    IL_001d:  /* 26   |                  */ pop
    IL_001e:  /* 2A   |                  */ ret
  } // end of method TypeStamp::.ctor

  .method /*060000FF*/ private hidebysig static 
          string  GenerateBinderContextProperty(string name) cil managed
  // SIG: 00 01 0E 0E
  {
    // Method begins at RVA 0x423e
    // Code size       39 (0x27)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000096       */ call       bool [mscorlib/*23000001*/]System.String/*0100003F*/::IsNullOrEmpty(string) /* 0A000096 */
    IL_0006:  /* 2D   | 0F               */ brtrue.s   IL_0017

    IL_0008:  /* 02   |                  */ ldarg.0
    IL_0009:  /* 1F   | 2E               */ ldc.i4.s   46
    IL_000b:  /* 8C   | (01)000057       */ box        [mscorlib/*23000001*/]System.Char/*01000057*/
    IL_0010:  /* 28   | (0A)000123       */ call       string [mscorlib/*23000001*/]System.String/*0100003F*/::Concat(object,
                                                                                                                      object) /* 0A000123 */
    IL_0015:  /* 2B   | 05               */ br.s       IL_001c

    IL_0017:  /* 7E   | (0A)0000EF       */ ldsfld     string [mscorlib/*23000001*/]System.String/*0100003F*/::Empty /* 0A0000EF */
    IL_001c:  /* 72   | (70)00047F       */ ldstr      "_xTypeStampx_" /* 7000047F */
    IL_0021:  /* 28   | (0A)000075       */ call       string [mscorlib/*23000001*/]System.String/*0100003F*/::Concat(string,
                                                                                                                      string) /* 0A000075 */
    IL_0026:  /* 2A   |                  */ ret
  } // end of method TypeStamp::GenerateBinderContextProperty

} // end of class MvcContrib.FluentHtml.Elements.TypeStamp

.class /*02000038*/ public abstract auto ansi sealed beforefieldinit MvcContrib.FluentHtml.ModelStateDictionaryExtensions
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C000033:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
  .method /*06000100*/ public hidebysig static 
          void  AddModelError<class T>(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelStateDictionary/*01000011*/ modelState,
                                       class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!T,object>> keyExpression,
                                       string errorMessage) cil managed
  // SIG: 10 01 03 01 12 45 15 12 69 01 15 12 2D 02 1E 00 1C 0E
  {
    .custom /*0C000051:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4266
    // Code size       19 (0x13)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 28   | (2B)000018       */ call       string MvcContrib.FluentHtml.Expressions.ExpressionExtensions/*02000051*/::GetNameFor<!!0,object>(class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,!!1>>) /* 2B000018 */
    IL_0007:  /* 28   | (06)00010F       */ call       string MvcContrib.FluentHtml.StringExtensions/*0200003B*/::FormatAsHtmlName(string) /* 0600010F */
    IL_000c:  /* 04   |                  */ ldarg.2
    IL_000d:  /* 6F   | (0A)000145       */ callvirt   instance void [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelStateDictionary/*01000011*/::AddModelError(string,
                                                                                                                                                                string) /* 0A000145 */
    IL_0012:  /* 2A   |                  */ ret
  } // end of method ModelStateDictionaryExtensions::AddModelError

  .method /*06000101*/ public hidebysig static 
          void  AddModelError<class T>(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelStateDictionary/*01000011*/ modelState,
                                       class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!T,object>> keyExpression,
                                       class [mscorlib/*23000001*/]System.Exception/*0100001B*/ exception) cil managed
  // SIG: 10 01 03 01 12 45 15 12 69 01 15 12 2D 02 1E 00 1C 12 6D
  {
    .custom /*0C000052:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x427a
    // Code size       19 (0x13)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 28   | (2B)000018       */ call       string MvcContrib.FluentHtml.Expressions.ExpressionExtensions/*02000051*/::GetNameFor<!!0,object>(class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,!!1>>) /* 2B000018 */
    IL_0007:  /* 28   | (06)00010F       */ call       string MvcContrib.FluentHtml.StringExtensions/*0200003B*/::FormatAsHtmlName(string) /* 0600010F */
    IL_000c:  /* 04   |                  */ ldarg.2
    IL_000d:  /* 6F   | (0A)000146       */ callvirt   instance void [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelStateDictionary/*01000011*/::AddModelError(string,
                                                                                                                                                                class [mscorlib/*23000001*/]System.Exception/*0100001B*/) /* 0A000146 */
    IL_0012:  /* 2A   |                  */ ret
  } // end of method ModelStateDictionaryExtensions::AddModelError

} // end of class MvcContrib.FluentHtml.ModelStateDictionaryExtensions

.class /*02000039*/ public abstract auto ansi sealed beforefieldinit MvcContrib.FluentHtml.PartialNameConventionService
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .field /*0400003D*/ private static string _partialNameConvention
  .method /*06000102*/ public hidebysig specialname static 
          string  get_PartialNameConvention() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x428e
    // Code size       6 (0x6)
    .maxstack  8
    IL_0000:  /* 7E   | (04)00003D       */ ldsfld     string MvcContrib.FluentHtml.PartialNameConventionService/*02000039*/::_partialNameConvention /* 0400003D */
    IL_0005:  /* 2A   |                  */ ret
  } // end of method PartialNameConventionService::get_PartialNameConvention

  .method /*06000103*/ public hidebysig specialname static 
          void  set_PartialNameConvention(string 'value') cil managed
  // SIG: 00 01 01 0E
  {
    // Method begins at RVA 0x4295
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 80   | (04)00003D       */ stsfld     string MvcContrib.FluentHtml.PartialNameConventionService/*02000039*/::_partialNameConvention /* 0400003D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method PartialNameConventionService::set_PartialNameConvention

  .method /*06000104*/ public hidebysig static 
          string  GeneratePartialName(class [mscorlib/*23000001*/]System.Type/*01000003*/ targetType) cil managed
  // SIG: 00 01 0E 12 0D
  {
    // Method begins at RVA 0x429d
    // Code size       17 (0x11)
    .maxstack  8
    IL_0000:  /* 7E   | (04)00003D       */ ldsfld     string MvcContrib.FluentHtml.PartialNameConventionService/*02000039*/::_partialNameConvention /* 0400003D */
    IL_0005:  /* 02   |                  */ ldarg.0
    IL_0006:  /* 6F   | (0A)000147       */ callvirt   instance string [mscorlib/*23000001*/]System.Reflection.MemberInfo/*0100003E*/::get_Name() /* 0A000147 */
    IL_000b:  /* 28   | (0A)000087       */ call       string [mscorlib/*23000001*/]System.String/*0100003F*/::Format(string,
                                                                                                                      object) /* 0A000087 */
    IL_0010:  /* 2A   |                  */ ret
  } // end of method PartialNameConventionService::GeneratePartialName

  .method /*06000105*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x42af
    // Code size       11 (0xb)
    .maxstack  8
    IL_0000:  /* 72   | (70)00049B       */ ldstr      "{0}TypePartial" /* 7000049B */
    IL_0005:  /* 80   | (04)00003D       */ stsfld     string MvcContrib.FluentHtml.PartialNameConventionService/*02000039*/::_partialNameConvention /* 0400003D */
    IL_000a:  /* 2A   |                  */ ret
  } // end of method PartialNameConventionService::.cctor

  .property /*1700001A*/ string PartialNameConvention()
  {
    .get string MvcContrib.FluentHtml.PartialNameConventionService/*02000039*/::get_PartialNameConvention() /* 06000102 */
    .set void MvcContrib.FluentHtml.PartialNameConventionService/*02000039*/::set_PartialNameConvention(string) /* 06000103 */
  } // end of property PartialNameConventionService::PartialNameConvention
} // end of class MvcContrib.FluentHtml.PartialNameConventionService

.class /*0200003A*/ public auto ansi beforefieldinit MvcContrib.FluentHtml.PartialRenderer`2<class T,class TPartialViewModel>
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .class /*0200005D*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass1'<class T,class TPartialViewModel>
         extends [mscorlib/*23000001*/]System.Object/*01000001*/
  {
    .custom /*0C000040:0A000013*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000032*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    .field /*0400007F*/ public class [mscorlib/*23000001*/]System.Type/*01000003*/ viewModelContainerType
    .field /*04000080*/ public class [mscorlib/*23000001*/]System.Type/*01000003*/ instanceType
    .method /*060001B5*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x445c
      // Code size       7 (0x7)
      .maxstack  8
      IL_0000:  /* 02   |                  */ ldarg.0
      IL_0001:  /* 28   | (0A)000012       */ call       instance void [mscorlib/*23000001*/]System.Object/*01000001*/::.ctor() /* 0A000012 */
      IL_0006:  /* 2A   |                  */ ret
    } // end of method '<>c__DisplayClass1'::.ctor

    .method /*060001B6*/ public hidebysig 
            instance bool  '<RenderTypedPartial>b__0'(class [mscorlib/*23000001*/]System.Type/*01000003*/ p) cil managed
    // SIG: 20 01 02 12 0D
    {
      // Method begins at RVA 0x4464
      // Code size       41 (0x29)
      .maxstack  8
      IL_0000:  /* 03   |                  */ ldarg.1
      IL_0001:  /* 6F   | (0A)000051       */ callvirt   instance bool [mscorlib/*23000001*/]System.Type/*01000003*/::get_IsGenericType() /* 0A000051 */
      IL_0006:  /* 2C   | 1F               */ brfalse.s  IL_0027

      IL_0008:  /* 03   |                  */ ldarg.1
      IL_0009:  /* 6F   | (0A)000052       */ callvirt   instance class [mscorlib/*23000001*/]System.Type/*01000003*/ [mscorlib/*23000001*/]System.Type/*01000003*/::GetGenericTypeDefinition() /* 0A000052 */
      IL_000e:  /* 02   |                  */ ldarg.0
      IL_000f:  /* 7B   | (0A)00015C       */ ldfld      class [mscorlib/*23000001*/]System.Type/*01000003*/ class MvcContrib.FluentHtml.PartialRenderer`2/*0200003A*//'<>c__DisplayClass1'/*0200005D*/<!T,!TPartialViewModel>/*1B000067*/::viewModelContainerType /* 0A00015C */
      IL_0014:  /* 33   | 11               */ bne.un.s   IL_0027

      IL_0016:  /* 03   |                  */ ldarg.1
      IL_0017:  /* 6F   | (0A)000031       */ callvirt   instance class [mscorlib/*23000001*/]System.Type/*01000003*/[] [mscorlib/*23000001*/]System.Type/*01000003*/::GetGenericArguments() /* 0A000031 */
      IL_001c:  /* 16   |                  */ ldc.i4.0
      IL_001d:  /* 9A   |                  */ ldelem.ref
      IL_001e:  /* 02   |                  */ ldarg.0
      IL_001f:  /* 7B   | (0A)00015D       */ ldfld      class [mscorlib/*23000001*/]System.Type/*01000003*/ class MvcContrib.FluentHtml.PartialRenderer`2/*0200003A*//'<>c__DisplayClass1'/*0200005D*/<!T,!TPartialViewModel>/*1B000067*/::instanceType /* 0A00015D */
      IL_0024:  /* FE01 |                  */ ceq
      IL_0026:  /* 2A   |                  */ ret

      IL_0027:  /* 16   |                  */ ldc.i4.0
      IL_0028:  /* 2A   |                  */ ret
    } // end of method '<>c__DisplayClass1'::'<RenderTypedPartial>b__0'

  } // end of class '<>c__DisplayClass1'

  .field /*0400003E*/ private initonly class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!T> view
  .field /*0400003F*/ private initonly string partialViewName
  .field /*04000040*/ private initonly class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!T,!TPartialViewModel>> modelExpression
  .field /*04000041*/ private class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/ viewData
  .method /*06000106*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!T> view,
                               string partialViewName,
                               class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!T,!TPartialViewModel>> modelExpression) cil managed
  // SIG: 20 03 01 15 12 81 40 01 13 00 0E 15 12 69 01 15 12 2D 02 13 00 13 01
  {
    // Method begins at RVA 0x42bb
    // Code size       28 (0x1c)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000012       */ call       instance void [mscorlib/*23000001*/]System.Object/*01000001*/::.ctor() /* 0A000012 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (0A)000148       */ stfld      class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!0> class MvcContrib.FluentHtml.PartialRenderer`2/*0200003A*/<!T,!TPartialViewModel>/*1B000063*/::view /* 0A000148 */
    IL_000d:  /* 02   |                  */ ldarg.0
    IL_000e:  /* 04   |                  */ ldarg.2
    IL_000f:  /* 7D   | (0A)000149       */ stfld      string class MvcContrib.FluentHtml.PartialRenderer`2/*0200003A*/<!T,!TPartialViewModel>/*1B000063*/::partialViewName /* 0A000149 */
    IL_0014:  /* 02   |                  */ ldarg.0
    IL_0015:  /* 05   |                  */ ldarg.3
    IL_0016:  /* 7D   | (0A)00014A       */ stfld      class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!0,!1>> class MvcContrib.FluentHtml.PartialRenderer`2/*0200003A*/<!T,!TPartialViewModel>/*1B000063*/::modelExpression /* 0A00014A */
    IL_001b:  /* 2A   |                  */ ret
  } // end of method PartialRenderer`2::.ctor

  .method /*06000107*/ public hidebysig instance class MvcContrib.FluentHtml.PartialRenderer`2/*0200003A*/<!T,!TPartialViewModel> 
          ViewData(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/ 'value') cil managed
  // SIG: 20 01 15 12 80 E8 02 13 00 13 01 12 71
  {
    // Method begins at RVA 0x42d8
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (0A)00014B       */ stfld      class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/ class MvcContrib.FluentHtml.PartialRenderer`2/*0200003A*/<!T,!TPartialViewModel>/*1B000063*/::viewData /* 0A00014B */
    IL_0007:  /* 02   |                  */ ldarg.0
    IL_0008:  /* 2A   |                  */ ret
  } // end of method PartialRenderer`2::ViewData

  .method /*06000108*/ public hidebysig instance void 
          Render() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x42e4
    // Code size       75 (0x4b)
    .maxstack  2
    .locals /*11000022*/ init (string V_0,
             class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!TPartialViewModel> V_1)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 6F   | (0A)00014C       */ callvirt   instance string class MvcContrib.FluentHtml.PartialRenderer`2/*0200003A*/<!T,!TPartialViewModel>/*1B000063*/::GetPartialPath() /* 0A00014C */
    IL_0006:  /* 0A   |                  */ stloc.0
    IL_0007:  /* 06   |                  */ ldloc.0
    IL_0008:  /* D0   | (1B)000064       */ ldtoken    class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!TPartialViewModel>/*1B000064*/
    IL_000d:  /* 28   | (0A)000021       */ call       class [mscorlib/*23000001*/]System.Type/*01000003*/ [mscorlib/*23000001*/]System.Type/*01000003*/::GetTypeFromHandle(valuetype [mscorlib/*23000001*/]System.RuntimeTypeHandle/*01000036*/) /* 0A000021 */
    IL_0012:  /* 28   | (0A)00014D       */ call       object [System.Web/*23000005*/]System.Web.Compilation.BuildManager/*0100005F*/::CreateInstanceFromVirtualPath(string,
                                                                                                                                                                     class [mscorlib/*23000001*/]System.Type/*01000003*/) /* 0A00014D */
    IL_0017:  /* 75   | (1B)000064       */ isinst     class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!TPartialViewModel>/*1B000064*/
    IL_001c:  /* 0B   |                  */ stloc.1
    IL_001d:  /* 07   |                  */ ldloc.1
    IL_001e:  /* 2D   | 0B               */ brtrue.s   IL_002b

    IL_0020:  /* 72   | (70)0004B9       */ ldstr      "IViewModelContainer<T>.RenderPartial can only be u"
    + "sed to render views that implement IViewModelContainer<T>" /* 700004B9 */
    IL_0025:  /* 73   | (0A)00014E       */ newobj     instance void [mscorlib/*23000001*/]System.InvalidOperationException/*01000060*/::.ctor(string) /* 0A00014E */
    IL_002a:  /* 7A   |                  */ throw

    IL_002b:  /* 07   |                  */ ldloc.1
    IL_002c:  /* 02   |                  */ ldarg.0
    IL_002d:  /* 28   | (0A)00014F       */ call       instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/ class MvcContrib.FluentHtml.PartialRenderer`2/*0200003A*/<!T,!TPartialViewModel>/*1B000063*/::GetViewData() /* 0A00014F */
    IL_0032:  /* 6F   | (0A)000150       */ callvirt   instance void [System.Web.Mvc/*23000003*/]System.Web.Mvc.IViewDataContainer/*01000007*/::set_ViewData(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/) /* 0A000150 */
    IL_0037:  /* 07   |                  */ ldloc.1
    IL_0038:  /* 02   |                  */ ldarg.0
    IL_0039:  /* 28   | (0A)000151       */ call       instance string class MvcContrib.FluentHtml.PartialRenderer`2/*0200003A*/<!T,!TPartialViewModel>/*1B000063*/::GetHtmlNamePrefix() /* 0A000151 */
    IL_003e:  /* 6F   | (0A)000152       */ callvirt   instance void class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!TPartialViewModel>/*1B000064*/::set_HtmlNamePrefix(string) /* 0A000152 */
    IL_0043:  /* 02   |                  */ ldarg.0
    IL_0044:  /* 07   |                  */ ldloc.1
    IL_0045:  /* 28   | (0A)000153       */ call       instance void class MvcContrib.FluentHtml.PartialRenderer`2/*0200003A*/<!T,!TPartialViewModel>/*1B000063*/::RenderPartial(class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!1>) /* 0A000153 */
    IL_004a:  /* 2A   |                  */ ret
  } // end of method PartialRenderer`2::Render

  .method /*06000109*/ family hidebysig newslot virtual 
          instance string  GetPartialPath() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x433c
    // Code size       80 (0x50)
    .maxstack  3
    .locals /*11000023*/ init (class [System.Web.Mvc/*23000003*/]System.Web.Mvc.IView/*01000066*/ V_0)
    IL_0000:  /* 28   | (0A)000154       */ call       class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewEngineCollection/*01000062*/ [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewEngines/*01000061*/::get_Engines() /* 0A000154 */
    IL_0005:  /* 02   |                  */ ldarg.0
    IL_0006:  /* 7B   | (0A)000148       */ ldfld      class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!0> class MvcContrib.FluentHtml.PartialRenderer`2/*0200003A*/<!T,!TPartialViewModel>/*1B000063*/::view /* 0A000148 */
    IL_000b:  /* 6F   | (0A)000001       */ callvirt   instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.HtmlHelper/*01000020*/ class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!T>/*1B00002D*/::get_Html() /* 0A000001 */
    IL_0010:  /* 6F   | (0A)000155       */ callvirt   instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewContext/*01000063*/ [System.Web.Mvc/*23000003*/]System.Web.Mvc.HtmlHelper/*01000020*/::get_ViewContext() /* 0A000155 */
    IL_0015:  /* 02   |                  */ ldarg.0
    IL_0016:  /* 7B   | (0A)000149       */ ldfld      string class MvcContrib.FluentHtml.PartialRenderer`2/*0200003A*/<!T,!TPartialViewModel>/*1B000063*/::partialViewName /* 0A000149 */
    IL_001b:  /* 6F   | (0A)000156       */ callvirt   instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewEngineResult/*01000064*/ [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewEngineCollection/*01000062*/::FindPartialView(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ControllerContext/*01000065*/,
                                                                                                                                                                                                                                           string) /* 0A000156 */
    IL_0020:  /* 6F   | (0A)000157       */ callvirt   instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.IView/*01000066*/ [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewEngineResult/*01000064*/::get_View() /* 0A000157 */
    IL_0025:  /* 0A   |                  */ stloc.0
    IL_0026:  /* 06   |                  */ ldloc.0
    IL_0027:  /* 75   | (01)000067       */ isinst     [System.Web.Mvc/*23000003*/]System.Web.Mvc.WebFormView/*01000067*/
    IL_002c:  /* 2D   | 16               */ brtrue.s   IL_0044

    IL_002e:  /* 72   | (70)000592       */ ldstr      "IViewModelContainer<T>.RenderPartial only supports"
    + " WebFormViewEngine" /* 70000592 */
    IL_0033:  /* 16   |                  */ ldc.i4.0
    IL_0034:  /* 8D   | (01)000001       */ newarr     [mscorlib/*23000001*/]System.Object/*01000001*/
    IL_0039:  /* 28   | (0A)000158       */ call       string [mscorlib/*23000001*/]System.String/*0100003F*/::Format(string,
                                                                                                                      object[]) /* 0A000158 */
    IL_003e:  /* 73   | (0A)00014E       */ newobj     instance void [mscorlib/*23000001*/]System.InvalidOperationException/*01000060*/::.ctor(string) /* 0A00014E */
    IL_0043:  /* 7A   |                  */ throw

    IL_0044:  /* 06   |                  */ ldloc.0
    IL_0045:  /* 74   | (01)000067       */ castclass  [System.Web.Mvc/*23000003*/]System.Web.Mvc.WebFormView/*01000067*/
    IL_004a:  /* 6F   | (0A)000159       */ callvirt   instance string [System.Web.Mvc/*23000003*/]System.Web.Mvc.WebFormView/*01000067*/::get_ViewPath() /* 0A000159 */
    IL_004f:  /* 2A   |                  */ ret
  } // end of method PartialRenderer`2::GetPartialPath

  .method /*0600010A*/ private hidebysig 
          instance void  RenderPartial(class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!TPartialViewModel> partial) cil managed
  // SIG: 20 01 01 15 12 81 40 01 13 01
  {
    // Method begins at RVA 0x4398
    // Code size       155 (0x9b)
    .maxstack  2
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 75   | (1B)000065       */ isinst     class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewPage`1/*01000009*/<!TPartialViewModel>/*1B000065*/
    IL_0006:  /* 2C   | 1C               */ brfalse.s  IL_0024

    IL_0008:  /* 03   |                  */ ldarg.1
    IL_0009:  /* 74   | (1B)000065       */ castclass  class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewPage`1/*01000009*/<!TPartialViewModel>/*1B000065*/
    IL_000e:  /* 02   |                  */ ldarg.0
    IL_000f:  /* 7B   | (0A)000148       */ ldfld      class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!0> class MvcContrib.FluentHtml.PartialRenderer`2/*0200003A*/<!T,!TPartialViewModel>/*1B000063*/::view /* 0A000148 */
    IL_0014:  /* 6F   | (0A)000001       */ callvirt   instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.HtmlHelper/*01000020*/ class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!T>/*1B00002D*/::get_Html() /* 0A000001 */
    IL_0019:  /* 6F   | (0A)000155       */ callvirt   instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewContext/*01000063*/ [System.Web.Mvc/*23000003*/]System.Web.Mvc.HtmlHelper/*01000020*/::get_ViewContext() /* 0A000155 */
    IL_001e:  /* 6F   | (0A)00015A       */ callvirt   instance void [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewPage/*01000068*/::RenderView(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewContext/*01000063*/) /* 0A00015A */
    IL_0023:  /* 2A   |                  */ ret

    IL_0024:  /* 03   |                  */ ldarg.1
    IL_0025:  /* 75   | (1B)000066       */ isinst     class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewUserControl`1/*0100000A*/<!TPartialViewModel>/*1B000066*/
    IL_002a:  /* 2C   | 1C               */ brfalse.s  IL_0048

    IL_002c:  /* 03   |                  */ ldarg.1
    IL_002d:  /* 74   | (1B)000066       */ castclass  class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewUserControl`1/*0100000A*/<!TPartialViewModel>/*1B000066*/
    IL_0032:  /* 02   |                  */ ldarg.0
    IL_0033:  /* 7B   | (0A)000148       */ ldfld      class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!0> class MvcContrib.FluentHtml.PartialRenderer`2/*0200003A*/<!T,!TPartialViewModel>/*1B000063*/::view /* 0A000148 */
    IL_0038:  /* 6F   | (0A)000001       */ callvirt   instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.HtmlHelper/*01000020*/ class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!T>/*1B00002D*/::get_Html() /* 0A000001 */
    IL_003d:  /* 6F   | (0A)000155       */ callvirt   instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewContext/*01000063*/ [System.Web.Mvc/*23000003*/]System.Web.Mvc.HtmlHelper/*01000020*/::get_ViewContext() /* 0A000155 */
    IL_0042:  /* 6F   | (0A)00015B       */ callvirt   instance void [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewUserControl/*01000069*/::RenderView(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewContext/*01000063*/) /* 0A00015B */
    IL_0047:  /* 2A   |                  */ ret

    IL_0048:  /* 03   |                  */ ldarg.1
    IL_0049:  /* 75   | (01)000068       */ isinst     [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewPage/*01000068*/
    IL_004e:  /* 2C   | 1C               */ brfalse.s  IL_006c

    IL_0050:  /* 03   |                  */ ldarg.1
    IL_0051:  /* 74   | (01)000068       */ castclass  [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewPage/*01000068*/
    IL_0056:  /* 02   |                  */ ldarg.0
    IL_0057:  /* 7B   | (0A)000148       */ ldfld      class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!0> class MvcContrib.FluentHtml.PartialRenderer`2/*0200003A*/<!T,!TPartialViewModel>/*1B000063*/::view /* 0A000148 */
    IL_005c:  /* 6F   | (0A)000001       */ callvirt   instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.HtmlHelper/*01000020*/ class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!T>/*1B00002D*/::get_Html() /* 0A000001 */
    IL_0061:  /* 6F   | (0A)000155       */ callvirt   instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewContext/*01000063*/ [System.Web.Mvc/*23000003*/]System.Web.Mvc.HtmlHelper/*01000020*/::get_ViewContext() /* 0A000155 */
    IL_0066:  /* 6F   | (0A)00015A       */ callvirt   instance void [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewPage/*01000068*/::RenderView(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewContext/*01000063*/) /* 0A00015A */
    IL_006b:  /* 2A   |                  */ ret

    IL_006c:  /* 03   |                  */ ldarg.1
    IL_006d:  /* 75   | (01)000069       */ isinst     [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewUserControl/*01000069*/
    IL_0072:  /* 2C   | 1C               */ brfalse.s  IL_0090

    IL_0074:  /* 03   |                  */ ldarg.1
    IL_0075:  /* 74   | (01)000069       */ castclass  [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewUserControl/*01000069*/
    IL_007a:  /* 02   |                  */ ldarg.0
    IL_007b:  /* 7B   | (0A)000148       */ ldfld      class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!0> class MvcContrib.FluentHtml.PartialRenderer`2/*0200003A*/<!T,!TPartialViewModel>/*1B000063*/::view /* 0A000148 */
    IL_0080:  /* 6F   | (0A)000001       */ callvirt   instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.HtmlHelper/*01000020*/ class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!T>/*1B00002D*/::get_Html() /* 0A000001 */
    IL_0085:  /* 6F   | (0A)000155       */ callvirt   instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewContext/*01000063*/ [System.Web.Mvc/*23000003*/]System.Web.Mvc.HtmlHelper/*01000020*/::get_ViewContext() /* 0A000155 */
    IL_008a:  /* 6F   | (0A)00015B       */ callvirt   instance void [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewUserControl/*01000069*/::RenderView(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewContext/*01000063*/) /* 0A00015B */
    IL_008f:  /* 2A   |                  */ ret

    IL_0090:  /* 72   | (70)00061D       */ ldstr      "IViewModelContainer<T>.RenderPartial can only be u"
    + "sed to render views that are ViewPage<T>, ViewUserControl<T>, ViewPage or ViewUserControl." /* 7000061D */
    IL_0095:  /* 73   | (0A)00014E       */ newobj     instance void [mscorlib/*23000001*/]System.InvalidOperationException/*01000060*/::.ctor(string) /* 0A00014E */
    IL_009a:  /* 7A   |                  */ throw
  } // end of method PartialRenderer`2::RenderPartial

  .method /*0600010B*/ private hidebysig 
          instance string  GetHtmlNamePrefix() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x443f
    // Code size       28 (0x1c)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (0A)00014A       */ ldfld      class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!0,!1>> class MvcContrib.FluentHtml.PartialRenderer`2/*0200003A*/<!T,!TPartialViewModel>/*1B000063*/::modelExpression /* 0A00014A */
    IL_0006:  /* 2C   | 12               */ brfalse.s  IL_001a

    IL_0008:  /* 02   |                  */ ldarg.0
    IL_0009:  /* 7B   | (0A)00014A       */ ldfld      class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!0,!1>> class MvcContrib.FluentHtml.PartialRenderer`2/*0200003A*/<!T,!TPartialViewModel>/*1B000063*/::modelExpression /* 0A00014A */
    IL_000e:  /* 02   |                  */ ldarg.0
    IL_000f:  /* 7B   | (0A)000148       */ ldfld      class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!0> class MvcContrib.FluentHtml.PartialRenderer`2/*0200003A*/<!T,!TPartialViewModel>/*1B000063*/::view /* 0A000148 */
    IL_0014:  /* 28   | (2B)000019       */ call       string MvcContrib.FluentHtml.Expressions.ExpressionExtensions/*02000051*/::GetNameFor<!T,!TPartialViewModel>(class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,!!1>>,
                                                                                                                                                                    class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!0>) /* 2B000019 */
    IL_0019:  /* 2A   |                  */ ret

    IL_001a:  /* 14   |                  */ ldnull
    IL_001b:  /* 2A   |                  */ ret
  } // end of method PartialRenderer`2::GetHtmlNamePrefix

  .method /*0600010C*/ public hidebysig instance void 
          RenderTypedPartial(class [mscorlib/*23000001*/]System.Type/*01000003*/ instanceType) cil managed
  // SIG: 20 01 01 12 0D
  {
    // Method begins at RVA 0x4490
    // Code size       340 (0x154)
    .maxstack  4
    .locals /*11000024*/ init (class [mscorlib/*23000001*/]System.Type/*01000003*/[] V_0,
             class [mscorlib/*23000001*/]System.Type/*01000003*/ V_1,
             string V_2,
             class [mscorlib/*23000001*/]System.InvalidOperationException/*01000060*/ V_3,
             object V_4,
             class [mscorlib/*23000001*/]System.Type/*01000003*/ V_5,
             class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewUserControl/*01000069*/ V_6,
             class MvcContrib.FluentHtml.PartialRenderer`2/*0200003A*//'<>c__DisplayClass1'/*0200005D*/<!T,!TPartialViewModel> V_7,
             class [mscorlib/*23000001*/]System.Type/*01000003*/[] V_8)
    IL_0000:  /* 73   | (0A)00015E       */ newobj     instance void class MvcContrib.FluentHtml.PartialRenderer`2/*0200003A*//'<>c__DisplayClass1'/*0200005D*/<!T,!TPartialViewModel>/*1B000067*/::.ctor() /* 0A00015E */
    IL_0005:  /* 13   | 07               */ stloc.s    V_7
    IL_0007:  /* 11   | 07               */ ldloc.s    V_7
    IL_0009:  /* 03   |                  */ ldarg.1
    IL_000a:  /* 7D   | (0A)00015D       */ stfld      class [mscorlib/*23000001*/]System.Type/*01000003*/ class MvcContrib.FluentHtml.PartialRenderer`2/*0200003A*//'<>c__DisplayClass1'/*0200005D*/<!T,!TPartialViewModel>/*1B000067*/::instanceType /* 0A00015D */
    IL_000f:  /* 11   | 07               */ ldloc.s    V_7
    IL_0011:  /* D0   | (02)000050       */ ldtoken    MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/
    IL_0016:  /* 28   | (0A)000021       */ call       class [mscorlib/*23000001*/]System.Type/*01000003*/ [mscorlib/*23000001*/]System.Type/*01000003*/::GetTypeFromHandle(valuetype [mscorlib/*23000001*/]System.RuntimeTypeHandle/*01000036*/) /* 0A000021 */
    IL_001b:  /* 7D   | (0A)00015C       */ stfld      class [mscorlib/*23000001*/]System.Type/*01000003*/ class MvcContrib.FluentHtml.PartialRenderer`2/*0200003A*//'<>c__DisplayClass1'/*0200005D*/<!T,!TPartialViewModel>/*1B000067*/::viewModelContainerType /* 0A00015C */
    IL_0020:  /* 17   |                  */ ldc.i4.1
    IL_0021:  /* 8D   | (01)000003       */ newarr     [mscorlib/*23000001*/]System.Type/*01000003*/
    IL_0026:  /* 13   | 08               */ stloc.s    V_8
    IL_0028:  /* 11   | 08               */ ldloc.s    V_8
    IL_002a:  /* 16   |                  */ ldc.i4.0
    IL_002b:  /* 11   | 07               */ ldloc.s    V_7
    IL_002d:  /* 7B   | (0A)00015D       */ ldfld      class [mscorlib/*23000001*/]System.Type/*01000003*/ class MvcContrib.FluentHtml.PartialRenderer`2/*0200003A*//'<>c__DisplayClass1'/*0200005D*/<!T,!TPartialViewModel>/*1B000067*/::instanceType /* 0A00015D */
    IL_0032:  /* A2   |                  */ stelem.ref
    IL_0033:  /* 11   | 08               */ ldloc.s    V_8
    IL_0035:  /* 0A   |                  */ stloc.0
    IL_0036:  /* 11   | 07               */ ldloc.s    V_7
    IL_0038:  /* 7B   | (0A)00015C       */ ldfld      class [mscorlib/*23000001*/]System.Type/*01000003*/ class MvcContrib.FluentHtml.PartialRenderer`2/*0200003A*//'<>c__DisplayClass1'/*0200005D*/<!T,!TPartialViewModel>/*1B000067*/::viewModelContainerType /* 0A00015C */
    IL_003d:  /* 06   |                  */ ldloc.0
    IL_003e:  /* 6F   | (0A)00015F       */ callvirt   instance class [mscorlib/*23000001*/]System.Type/*01000003*/ [mscorlib/*23000001*/]System.Type/*01000003*/::MakeGenericType(class [mscorlib/*23000001*/]System.Type/*01000003*/[]) /* 0A00015F */
    IL_0043:  /* 0B   |                  */ stloc.1
    IL_0044:  /* 7E   | (0A)0000EF       */ ldsfld     string [mscorlib/*23000001*/]System.String/*0100003F*/::Empty /* 0A0000EF */
    IL_0049:  /* 0C   |                  */ stloc.2
    .try
    {
      IL_004a:  /* 02   |                  */ ldarg.0
      IL_004b:  /* 6F   | (0A)00014C       */ callvirt   instance string class MvcContrib.FluentHtml.PartialRenderer`2/*0200003A*/<!T,!TPartialViewModel>/*1B000063*/::GetPartialPath() /* 0A00014C */
      IL_0050:  /* 0C   |                  */ stloc.2
      IL_0051:  /* DE   | 18               */ leave.s    IL_006b

    }  // end .try
    catch [mscorlib/*23000001*/]System.InvalidOperationException/*01000060*/ 
    {
      IL_0053:  /* 0D   |                  */ stloc.3
      IL_0054:  /* 72   | (70)000738       */ ldstr      "IViewModelContainer<T>.RenderTypedPartial only sup"
      + "ports WebFormViewEngine" /* 70000738 */
      IL_0059:  /* 16   |                  */ ldc.i4.0
      IL_005a:  /* 8D   | (01)000001       */ newarr     [mscorlib/*23000001*/]System.Object/*01000001*/
      IL_005f:  /* 28   | (0A)000158       */ call       string [mscorlib/*23000001*/]System.String/*0100003F*/::Format(string,
                                                                                                                        object[]) /* 0A000158 */
      IL_0064:  /* 09   |                  */ ldloc.3
      IL_0065:  /* 73   | (0A)000160       */ newobj     instance void [mscorlib/*23000001*/]System.InvalidOperationException/*01000060*/::.ctor(string,
                                                                                                                                                 class [mscorlib/*23000001*/]System.Exception/*0100001B*/) /* 0A000160 */
      IL_006a:  /* 7A   |                  */ throw

    }  // end handler
    // HEX: 00 00 4A 00 09 53 00 18 60 00 00 01
    IL_006b:  /* 08   |                  */ ldloc.2
    IL_006c:  /* 07   |                  */ ldloc.1
    IL_006d:  /* 28   | (0A)00014D       */ call       object [System.Web/*23000005*/]System.Web.Compilation.BuildManager/*0100005F*/::CreateInstanceFromVirtualPath(string,
                                                                                                                                                                     class [mscorlib/*23000001*/]System.Type/*01000003*/) /* 0A00014D */
    IL_0072:  /* 13   | 04               */ stloc.s    V_4
    IL_0074:  /* 11   | 04               */ ldloc.s    V_4
    IL_0076:  /* 2D   | 0B               */ brtrue.s   IL_0083

    IL_0078:  /* 72   | (70)0007CD       */ ldstr      "Partial not found and was not caught by MVC" /* 700007CD */
    IL_007d:  /* 73   | (0A)00014E       */ newobj     instance void [mscorlib/*23000001*/]System.InvalidOperationException/*01000060*/::.ctor(string) /* 0A00014E */
    IL_0082:  /* 7A   |                  */ throw

    IL_0083:  /* 11   | 04               */ ldloc.s    V_4
    IL_0085:  /* 6F   | (0A)000016       */ callvirt   instance class [mscorlib/*23000001*/]System.Type/*01000003*/ [mscorlib/*23000001*/]System.Object/*01000001*/::GetType() /* 0A000016 */
    IL_008a:  /* 6F   | (0A)000022       */ callvirt   instance class [mscorlib/*23000001*/]System.Type/*01000003*/[] [mscorlib/*23000001*/]System.Type/*01000003*/::GetInterfaces() /* 0A000022 */
    IL_008f:  /* 11   | 07               */ ldloc.s    V_7
    IL_0091:  /* FE06 | (0A)000161       */ ldftn      instance bool class MvcContrib.FluentHtml.PartialRenderer`2/*0200003A*//'<>c__DisplayClass1'/*0200005D*/<!T,!TPartialViewModel>/*1B000067*/::'<RenderTypedPartial>b__0'(class [mscorlib/*23000001*/]System.Type/*01000003*/) /* 0A000161 */
    IL_0097:  /* 73   | (0A)000023       */ newobj     instance void class [System.Core/*23000002*/]System.Func`2/*0100000B*/<class [mscorlib/*23000001*/]System.Type/*01000003*/,bool>/*1B000038*/::.ctor(object,
                                                                                                                                                                                                           native int) /* 0A000023 */
    IL_009c:  /* 28   | (2B)000001       */ call       class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<!!0> [System.Core/*23000002*/]System.Linq.Enumerable/*01000037*/::Where<class [mscorlib/*23000001*/]System.Type/*01000003*/>(class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<!!0>,
                                                                                                                                                                                                                                                                     class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,bool>) /* 2B000001 */
    IL_00a1:  /* 28   | (2B)00001A       */ call       !!0 [System.Core/*23000002*/]System.Linq.Enumerable/*01000037*/::FirstOrDefault<class [mscorlib/*23000001*/]System.Type/*01000003*/>(class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<!!0>) /* 2B00001A */
    IL_00a6:  /* 13   | 05               */ stloc.s    V_5
    IL_00a8:  /* 11   | 05               */ ldloc.s    V_5
    IL_00aa:  /* 2D   | 0B               */ brtrue.s   IL_00b7

    IL_00ac:  /* 72   | (70)0004B9       */ ldstr      "IViewModelContainer<T>.RenderPartial can only be u"
    + "sed to render views that implement IViewModelContainer<T>" /* 700004B9 */
    IL_00b1:  /* 73   | (0A)00014E       */ newobj     instance void [mscorlib/*23000001*/]System.InvalidOperationException/*01000060*/::.ctor(string) /* 0A00014E */
    IL_00b6:  /* 7A   |                  */ throw

    IL_00b7:  /* 11   | 05               */ ldloc.s    V_5
    IL_00b9:  /* 72   | (70)000825       */ ldstr      "HtmlNamePrefix" /* 70000825 */
    IL_00be:  /* 6F   | (0A)0000EC       */ callvirt   instance class [mscorlib/*23000001*/]System.Reflection.PropertyInfo/*01000041*/ [mscorlib/*23000001*/]System.Type/*01000003*/::GetProperty(string) /* 0A0000EC */
    IL_00c3:  /* 11   | 04               */ ldloc.s    V_4
    IL_00c5:  /* 02   |                  */ ldarg.0
    IL_00c6:  /* 28   | (0A)000151       */ call       instance string class MvcContrib.FluentHtml.PartialRenderer`2/*0200003A*/<!T,!TPartialViewModel>/*1B000063*/::GetHtmlNamePrefix() /* 0A000151 */
    IL_00cb:  /* 14   |                  */ ldnull
    IL_00cc:  /* 6F   | (0A)000162       */ callvirt   instance void [mscorlib/*23000001*/]System.Reflection.PropertyInfo/*01000041*/::SetValue(object,
                                                                                                                                                object,
                                                                                                                                                object[]) /* 0A000162 */
    IL_00d1:  /* D0   | (01)000007       */ ldtoken    [System.Web.Mvc/*23000003*/]System.Web.Mvc.IViewDataContainer/*01000007*/
    IL_00d6:  /* 28   | (0A)000021       */ call       class [mscorlib/*23000001*/]System.Type/*01000003*/ [mscorlib/*23000001*/]System.Type/*01000003*/::GetTypeFromHandle(valuetype [mscorlib/*23000001*/]System.RuntimeTypeHandle/*01000036*/) /* 0A000021 */
    IL_00db:  /* 72   | (70)000843       */ ldstr      "ViewData" /* 70000843 */
    IL_00e0:  /* 28   | (0A)0000EC       */ call       instance class [mscorlib/*23000001*/]System.Reflection.PropertyInfo/*01000041*/ [mscorlib/*23000001*/]System.Type/*01000003*/::GetProperty(string) /* 0A0000EC */
    IL_00e5:  /* 11   | 04               */ ldloc.s    V_4
    IL_00e7:  /* 02   |                  */ ldarg.0
    IL_00e8:  /* 28   | (0A)00014F       */ call       instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/ class MvcContrib.FluentHtml.PartialRenderer`2/*0200003A*/<!T,!TPartialViewModel>/*1B000063*/::GetViewData() /* 0A00014F */
    IL_00ed:  /* 14   |                  */ ldnull
    IL_00ee:  /* 6F   | (0A)000162       */ callvirt   instance void [mscorlib/*23000001*/]System.Reflection.PropertyInfo/*01000041*/::SetValue(object,
                                                                                                                                                object,
                                                                                                                                                object[]) /* 0A000162 */
    IL_00f3:  /* 11   | 04               */ ldloc.s    V_4
    IL_00f5:  /* 75   | (01)000069       */ isinst     [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewUserControl/*01000069*/
    IL_00fa:  /* 13   | 06               */ stloc.s    V_6
    IL_00fc:  /* 11   | 06               */ ldloc.s    V_6
    IL_00fe:  /* 2D   | 0B               */ brtrue.s   IL_010b

    IL_0100:  /* 72   | (70)000855       */ ldstr      "Partials must be derived from ViewUserControl" /* 70000855 */
    IL_0105:  /* 73   | (0A)00014E       */ newobj     instance void [mscorlib/*23000001*/]System.InvalidOperationException/*01000060*/::.ctor(string) /* 0A00014E */
    IL_010a:  /* 7A   |                  */ throw

    IL_010b:  /* 02   |                  */ ldarg.0
    IL_010c:  /* 7B   | (0A)000148       */ ldfld      class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!0> class MvcContrib.FluentHtml.PartialRenderer`2/*0200003A*/<!T,!TPartialViewModel>/*1B000063*/::view /* 0A000148 */
    IL_0111:  /* 6F   | (0A)000001       */ callvirt   instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.HtmlHelper/*01000020*/ class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!T>/*1B00002D*/::get_Html() /* 0A000001 */
    IL_0116:  /* 6F   | (0A)000155       */ callvirt   instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewContext/*01000063*/ [System.Web.Mvc/*23000003*/]System.Web.Mvc.HtmlHelper/*01000020*/::get_ViewContext() /* 0A000155 */
    IL_011b:  /* 6F   | (0A)000163       */ callvirt   instance class [mscorlib/*23000001*/]System.IO.TextWriter/*0100006A*/ [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewContext/*01000063*/::get_Writer() /* 0A000163 */
    IL_0120:  /* 02   |                  */ ldarg.0
    IL_0121:  /* 28   | (0A)000151       */ call       instance string class MvcContrib.FluentHtml.PartialRenderer`2/*0200003A*/<!T,!TPartialViewModel>/*1B000063*/::GetHtmlNamePrefix() /* 0A000151 */
    IL_0126:  /* 11   | 07               */ ldloc.s    V_7
    IL_0128:  /* 7B   | (0A)00015D       */ ldfld      class [mscorlib/*23000001*/]System.Type/*01000003*/ class MvcContrib.FluentHtml.PartialRenderer`2/*0200003A*//'<>c__DisplayClass1'/*0200005D*/<!T,!TPartialViewModel>/*1B000067*/::instanceType /* 0A00015D */
    IL_012d:  /* 73   | (06)0000FE       */ newobj     instance void MvcContrib.FluentHtml.Elements.TypeStamp/*02000037*/::.ctor(string,
                                                                                                                                 class [mscorlib/*23000001*/]System.Type/*01000003*/) /* 060000FE */
    IL_0132:  /* 6F   | (0A)00006E       */ callvirt   instance string [mscorlib/*23000001*/]System.Object/*01000001*/::ToString() /* 0A00006E */
    IL_0137:  /* 6F   | (0A)000164       */ callvirt   instance void [mscorlib/*23000001*/]System.IO.TextWriter/*0100006A*/::Write(string) /* 0A000164 */
    IL_013c:  /* 11   | 06               */ ldloc.s    V_6
    IL_013e:  /* 02   |                  */ ldarg.0
    IL_013f:  /* 7B   | (0A)000148       */ ldfld      class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!0> class MvcContrib.FluentHtml.PartialRenderer`2/*0200003A*/<!T,!TPartialViewModel>/*1B000063*/::view /* 0A000148 */
    IL_0144:  /* 6F   | (0A)000001       */ callvirt   instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.HtmlHelper/*01000020*/ class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!T>/*1B00002D*/::get_Html() /* 0A000001 */
    IL_0149:  /* 6F   | (0A)000155       */ callvirt   instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewContext/*01000063*/ [System.Web.Mvc/*23000003*/]System.Web.Mvc.HtmlHelper/*01000020*/::get_ViewContext() /* 0A000155 */
    IL_014e:  /* 6F   | (0A)00015B       */ callvirt   instance void [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewUserControl/*01000069*/::RenderView(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewContext/*01000063*/) /* 0A00015B */
    IL_0153:  /* 2A   |                  */ ret
  } // end of method PartialRenderer`2::RenderTypedPartial

  .method /*0600010D*/ private hidebysig 
          instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/ 
          GetViewData() cil managed
  // SIG: 20 00 12 71
  {
    // Method begins at RVA 0x4600
    // Code size       135 (0x87)
    .maxstack  4
    .locals /*11000025*/ init (!TPartialViewModel V_0,
             class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/ V_1)
    IL_0000:  /* 12   | 00               */ ldloca.s   V_0
    IL_0002:  /* FE15 | (1B)000054       */ initobj    !TPartialViewModel/*1B000054*/
    IL_0008:  /* 02   |                  */ ldarg.0
    IL_0009:  /* 7B   | (0A)00014A       */ ldfld      class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!0,!1>> class MvcContrib.FluentHtml.PartialRenderer`2/*0200003A*/<!T,!TPartialViewModel>/*1B000063*/::modelExpression /* 0A00014A */
    IL_000e:  /* 2C   | 1C               */ brfalse.s  IL_002c

    IL_0010:  /* 02   |                  */ ldarg.0
    IL_0011:  /* 7B   | (0A)00014A       */ ldfld      class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!0,!1>> class MvcContrib.FluentHtml.PartialRenderer`2/*0200003A*/<!T,!TPartialViewModel>/*1B000063*/::modelExpression /* 0A00014A */
    IL_0016:  /* 6F   | (0A)000165       */ callvirt   instance !0 class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!T,!TPartialViewModel>>/*1B000068*/::Compile() /* 0A000165 */
    IL_001b:  /* 02   |                  */ ldarg.0
    IL_001c:  /* 7B   | (0A)000148       */ ldfld      class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!0> class MvcContrib.FluentHtml.PartialRenderer`2/*0200003A*/<!T,!TPartialViewModel>/*1B000063*/::view /* 0A000148 */
    IL_0021:  /* 6F   | (0A)000166       */ callvirt   instance !0 class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!T>/*1B00002D*/::get_ViewModel() /* 0A000166 */
    IL_0026:  /* 6F   | (0A)000167       */ callvirt   instance !1 class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!T,!TPartialViewModel>/*1B000069*/::Invoke(!0) /* 0A000167 */
    IL_002b:  /* 0A   |                  */ stloc.0
    IL_002c:  /* 06   |                  */ ldloc.0
    IL_002d:  /* 8C   | (1B)000054       */ box        !TPartialViewModel/*1B000054*/
    IL_0032:  /* 2C   | 2E               */ brfalse.s  IL_0062

    IL_0034:  /* 02   |                  */ ldarg.0
    IL_0035:  /* 7B   | (0A)00014B       */ ldfld      class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/ class MvcContrib.FluentHtml.PartialRenderer`2/*0200003A*/<!T,!TPartialViewModel>/*1B000063*/::viewData /* 0A00014B */
    IL_003a:  /* 2C   | 1A               */ brfalse.s  IL_0056

    IL_003c:  /* 02   |                  */ ldarg.0
    IL_003d:  /* 7B   | (0A)00014B       */ ldfld      class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/ class MvcContrib.FluentHtml.PartialRenderer`2/*0200003A*/<!T,!TPartialViewModel>/*1B000063*/::viewData /* 0A00014B */
    IL_0042:  /* 73   | (0A)000168       */ newobj     instance void [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/::.ctor(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/) /* 0A000168 */
    IL_0047:  /* 0B   |                  */ stloc.1
    IL_0048:  /* 07   |                  */ ldloc.1
    IL_0049:  /* 06   |                  */ ldloc.0
    IL_004a:  /* 8C   | (1B)000054       */ box        !TPartialViewModel/*1B000054*/
    IL_004f:  /* 6F   | (0A)000169       */ callvirt   instance void [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/::set_Model(object) /* 0A000169 */
    IL_0054:  /* 07   |                  */ ldloc.1
    IL_0055:  /* 2A   |                  */ ret

    IL_0056:  /* 06   |                  */ ldloc.0
    IL_0057:  /* 8C   | (1B)000054       */ box        !TPartialViewModel/*1B000054*/
    IL_005c:  /* 73   | (0A)00016A       */ newobj     instance void [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/::.ctor(object) /* 0A00016A */
    IL_0061:  /* 2A   |                  */ ret

    IL_0062:  /* 02   |                  */ ldarg.0
    IL_0063:  /* 7B   | (0A)00014B       */ ldfld      class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/ class MvcContrib.FluentHtml.PartialRenderer`2/*0200003A*/<!T,!TPartialViewModel>/*1B000063*/::viewData /* 0A00014B */
    IL_0068:  /* 2C   | 0C               */ brfalse.s  IL_0076

    IL_006a:  /* 02   |                  */ ldarg.0
    IL_006b:  /* 7B   | (0A)00014B       */ ldfld      class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/ class MvcContrib.FluentHtml.PartialRenderer`2/*0200003A*/<!T,!TPartialViewModel>/*1B000063*/::viewData /* 0A00014B */
    IL_0070:  /* 73   | (0A)000168       */ newobj     instance void [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/::.ctor(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/) /* 0A000168 */
    IL_0075:  /* 2A   |                  */ ret

    IL_0076:  /* 02   |                  */ ldarg.0
    IL_0077:  /* 7B   | (0A)000148       */ ldfld      class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!0> class MvcContrib.FluentHtml.PartialRenderer`2/*0200003A*/<!T,!TPartialViewModel>/*1B000063*/::view /* 0A000148 */
    IL_007c:  /* 6F   | (0A)00016B       */ callvirt   instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/ [System.Web.Mvc/*23000003*/]System.Web.Mvc.IViewDataContainer/*01000007*/::get_ViewData() /* 0A00016B */
    IL_0081:  /* 73   | (0A)000168       */ newobj     instance void [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/::.ctor(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/) /* 0A000168 */
    IL_0086:  /* 2A   |                  */ ret
  } // end of method PartialRenderer`2::GetViewData

} // end of class MvcContrib.FluentHtml.PartialRenderer`2

.class /*0200003B*/ public abstract auto ansi sealed beforefieldinit MvcContrib.FluentHtml.StringExtensions
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C000034:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
  .method /*0600010E*/ public hidebysig static 
          string  FormatAsHtmlId(string name) cil managed
  // SIG: 00 01 0E 0E
  {
    .custom /*0C000053:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4693
    // Code size       31 (0x1f)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000096       */ call       bool [mscorlib/*23000001*/]System.String/*0100003F*/::IsNullOrEmpty(string) /* 0A000096 */
    IL_0006:  /* 2D   | 11               */ brtrue.s   IL_0019

    IL_0008:  /* 02   |                  */ ldarg.0
    IL_0009:  /* 72   | (70)0008B1       */ ldstr      "[^a-zA-Z0-9-:]" /* 700008B1 */
    IL_000e:  /* 72   | (70)0008CF       */ ldstr      "_" /* 700008CF */
    IL_0013:  /* 28   | (0A)00016C       */ call       string [System/*23000004*/]System.Text.RegularExpressions.Regex/*0100006B*/::Replace(string,
                                                                                                                                            string,
                                                                                                                                            string) /* 0A00016C */
    IL_0018:  /* 2A   |                  */ ret

    IL_0019:  /* 7E   | (0A)0000EF       */ ldsfld     string [mscorlib/*23000001*/]System.String/*0100003F*/::Empty /* 0A0000EF */
    IL_001e:  /* 2A   |                  */ ret
  } // end of method StringExtensions::FormatAsHtmlId

  .method /*0600010F*/ public hidebysig static 
          string  FormatAsHtmlName(string name) cil managed
  // SIG: 00 01 0E 0E
  {
    .custom /*0C000054:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x46b3
    // Code size       31 (0x1f)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000096       */ call       bool [mscorlib/*23000001*/]System.String/*0100003F*/::IsNullOrEmpty(string) /* 0A000096 */
    IL_0006:  /* 2D   | 11               */ brtrue.s   IL_0019

    IL_0008:  /* 02   |                  */ ldarg.0
    IL_0009:  /* 72   | (70)0008D3       */ ldstr      "[^a-zA-Z0-9.-:]" /* 700008D3 */
    IL_000e:  /* 72   | (70)0008CF       */ ldstr      "_" /* 700008CF */
    IL_0013:  /* 28   | (0A)00016C       */ call       string [System/*23000004*/]System.Text.RegularExpressions.Regex/*0100006B*/::Replace(string,
                                                                                                                                            string,
                                                                                                                                            string) /* 0A00016C */
    IL_0018:  /* 2A   |                  */ ret

    IL_0019:  /* 7E   | (0A)0000EF       */ ldsfld     string [mscorlib/*23000001*/]System.String/*0100003F*/::Empty /* 0A0000EF */
    IL_001e:  /* 2A   |                  */ ret
  } // end of method StringExtensions::FormatAsHtmlName

  .method /*06000110*/ public hidebysig static 
          string  PascalCaseToPhrase(string s) cil managed
  // SIG: 00 01 0E 0E
  {
    .custom /*0C000055:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x46d4
    // Code size       177 (0xb1)
    .maxstack  3
    .locals /*11000026*/ init (class [mscorlib/*23000001*/]System.Text.StringBuilder/*01000055*/ V_0,
             char[] V_1,
             int32 V_2,
             bool V_3,
             bool V_4,
             bool V_5,
             bool V_6,
             bool V_7,
             bool V_8)
    IL_0000:  /* 73   | (0A)000067       */ newobj     instance void [mscorlib/*23000001*/]System.Text.StringBuilder/*01000055*/::.ctor() /* 0A000067 */
    IL_0005:  /* 0A   |                  */ stloc.0
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 6F   | (0A)00016D       */ callvirt   instance char[] [mscorlib/*23000001*/]System.String/*0100003F*/::ToCharArray() /* 0A00016D */
    IL_000c:  /* 0B   |                  */ stloc.1
    IL_000d:  /* 16   |                  */ ldc.i4.0
    IL_000e:  /* 0C   |                  */ stloc.2
    IL_000f:  /* 38   | 8D000000         */ br         IL_00a1

    IL_0014:  /* 08   |                  */ ldloc.2
    IL_0015:  /* 16   |                  */ ldc.i4.0
    IL_0016:  /* FE01 |                  */ ceq
    IL_0018:  /* 0D   |                  */ stloc.3
    IL_0019:  /* 08   |                  */ ldloc.2
    IL_001a:  /* 07   |                  */ ldloc.1
    IL_001b:  /* 8E   |                  */ ldlen
    IL_001c:  /* 69   |                  */ conv.i4
    IL_001d:  /* FE01 |                  */ ceq
    IL_001f:  /* 13   | 04               */ stloc.s    V_4
    IL_0021:  /* 07   |                  */ ldloc.1
    IL_0022:  /* 08   |                  */ ldloc.2
    IL_0023:  /* 93   |                  */ ldelem.u2
    IL_0024:  /* 28   | (0A)00016E       */ call       bool [mscorlib/*23000001*/]System.Char/*01000057*/::IsUpper(char) /* 0A00016E */
    IL_0029:  /* 13   | 05               */ stloc.s    V_5
    IL_002b:  /* 08   |                  */ ldloc.2
    IL_002c:  /* 16   |                  */ ldc.i4.0
    IL_002d:  /* 31   | 0C               */ ble.s      IL_003b

    IL_002f:  /* 07   |                  */ ldloc.1
    IL_0030:  /* 08   |                  */ ldloc.2
    IL_0031:  /* 17   |                  */ ldc.i4.1
    IL_0032:  /* 59   |                  */ sub
    IL_0033:  /* 93   |                  */ ldelem.u2
    IL_0034:  /* 28   | (0A)00016F       */ call       bool [mscorlib/*23000001*/]System.Char/*01000057*/::IsLower(char) /* 0A00016F */
    IL_0039:  /* 2B   | 01               */ br.s       IL_003c

    IL_003b:  /* 16   |                  */ ldc.i4.0
    IL_003c:  /* 13   | 06               */ stloc.s    V_6
    IL_003e:  /* 08   |                  */ ldloc.2
    IL_003f:  /* 17   |                  */ ldc.i4.1
    IL_0040:  /* 58   |                  */ add
    IL_0041:  /* 07   |                  */ ldloc.1
    IL_0042:  /* 8E   |                  */ ldlen
    IL_0043:  /* 69   |                  */ conv.i4
    IL_0044:  /* 2F   | 0C               */ bge.s      IL_0052

    IL_0046:  /* 07   |                  */ ldloc.1
    IL_0047:  /* 08   |                  */ ldloc.2
    IL_0048:  /* 17   |                  */ ldc.i4.1
    IL_0049:  /* 58   |                  */ add
    IL_004a:  /* 93   |                  */ ldelem.u2
    IL_004b:  /* 28   | (0A)00016F       */ call       bool [mscorlib/*23000001*/]System.Char/*01000057*/::IsLower(char) /* 0A00016F */
    IL_0050:  /* 2B   | 01               */ br.s       IL_0053

    IL_0052:  /* 16   |                  */ ldc.i4.0
    IL_0053:  /* 13   | 07               */ stloc.s    V_7
    IL_0055:  /* 08   |                  */ ldloc.2
    IL_0056:  /* 16   |                  */ ldc.i4.0
    IL_0057:  /* 31   | 0B               */ ble.s      IL_0064

    IL_0059:  /* 07   |                  */ ldloc.1
    IL_005a:  /* 08   |                  */ ldloc.2
    IL_005b:  /* 17   |                  */ ldc.i4.1
    IL_005c:  /* 59   |                  */ sub
    IL_005d:  /* 93   |                  */ ldelem.u2
    IL_005e:  /* 1F   | 20               */ ldc.i4.s   32
    IL_0060:  /* FE01 |                  */ ceq
    IL_0062:  /* 2B   | 01               */ br.s       IL_0065

    IL_0064:  /* 16   |                  */ ldc.i4.0
    IL_0065:  /* 13   | 08               */ stloc.s    V_8
    IL_0067:  /* 09   |                  */ ldloc.3
    IL_0068:  /* 2D   | 20               */ brtrue.s   IL_008a

    IL_006a:  /* 11   | 05               */ ldloc.s    V_5
    IL_006c:  /* 2C   | 1C               */ brfalse.s  IL_008a

    IL_006e:  /* 11   | 08               */ ldloc.s    V_8
    IL_0070:  /* 2D   | 18               */ brtrue.s   IL_008a

    IL_0072:  /* 11   | 06               */ ldloc.s    V_6
    IL_0074:  /* 2D   | 08               */ brtrue.s   IL_007e

    IL_0076:  /* 11   | 07               */ ldloc.s    V_7
    IL_0078:  /* 2D   | 04               */ brtrue.s   IL_007e

    IL_007a:  /* 11   | 04               */ ldloc.s    V_4
    IL_007c:  /* 2C   | 0C               */ brfalse.s  IL_008a

    IL_007e:  /* 06   |                  */ ldloc.0
    IL_007f:  /* 72   | (70)0000FB       */ ldstr      " " /* 700000FB */
    IL_0084:  /* 6F   | (0A)000101       */ callvirt   instance class [mscorlib/*23000001*/]System.Text.StringBuilder/*01000055*/ [mscorlib/*23000001*/]System.Text.StringBuilder/*01000055*/::Append(string) /* 0A000101 */
    IL_0089:  /* 26   |                  */ pop
    IL_008a:  /* 06   |                  */ ldloc.0
    IL_008b:  /* 07   |                  */ ldloc.1
    IL_008c:  /* 08   |                  */ ldloc.2
    IL_008d:  /* 8F   | (01)000057       */ ldelema    [mscorlib/*23000001*/]System.Char/*01000057*/
    IL_0092:  /* 28   | (0A)000170       */ call       instance string [mscorlib/*23000001*/]System.Char/*01000057*/::ToString() /* 0A000170 */
    IL_0097:  /* 6F   | (0A)000101       */ callvirt   instance class [mscorlib/*23000001*/]System.Text.StringBuilder/*01000055*/ [mscorlib/*23000001*/]System.Text.StringBuilder/*01000055*/::Append(string) /* 0A000101 */
    IL_009c:  /* 26   |                  */ pop
    IL_009d:  /* 08   |                  */ ldloc.2
    IL_009e:  /* 17   |                  */ ldc.i4.1
    IL_009f:  /* 58   |                  */ add
    IL_00a0:  /* 0C   |                  */ stloc.2
    IL_00a1:  /* 08   |                  */ ldloc.2
    IL_00a2:  /* 07   |                  */ ldloc.1
    IL_00a3:  /* 8E   |                  */ ldlen
    IL_00a4:  /* 69   |                  */ conv.i4
    IL_00a5:  /* 3F   | 6AFFFFFF         */ blt        IL_0014

    IL_00aa:  /* 06   |                  */ ldloc.0
    IL_00ab:  /* 6F   | (0A)00006E       */ callvirt   instance string [mscorlib/*23000001*/]System.Object/*01000001*/::ToString() /* 0A00006E */
    IL_00b0:  /* 2A   |                  */ ret
  } // end of method StringExtensions::PascalCaseToPhrase

} // end of class MvcContrib.FluentHtml.StringExtensions

.class /*0200003C*/ public abstract auto ansi beforefieldinit MvcContrib.FluentHtml.Elements.SubmitButtonBase`1<(class MvcContrib.FluentHtml.Elements.SubmitButtonBase`1/*0200003C*/<!T>/*1B00001F*/) T>
       extends class MvcContrib.FluentHtml.Elements.Input`1/*02000020*/<!T>/*1B000008*/
{
  .method /*06000111*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(string text) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x4791
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 14   |                  */ ldnull
    IL_0003:  /* 28   | (0A)000171       */ call       instance void class MvcContrib.FluentHtml.Elements.SubmitButtonBase`1/*0200003C*/<!T>/*1B00001F*/::.ctor(string,
                                                                                                                                                                class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 0A000171 */
    IL_0008:  /* 2A   |                  */ ret
  } // end of method SubmitButtonBase`1::.ctor

  .method /*06000112*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(string text,
                               class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> behaviors) cil managed
  // SIG: 20 02 01 0E 15 12 11 01 12 08
  {
    // Method begins at RVA 0x479b
    // Code size       33 (0x21)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 72   | (70)0008F3       */ ldstr      "submit" /* 700008F3 */
    IL_0006:  /* 03   |                  */ ldarg.1
    IL_0007:  /* 2C   | 08               */ brfalse.s  IL_0011

    IL_0009:  /* 03   |                  */ ldarg.1
    IL_000a:  /* 28   | (06)00010F       */ call       string MvcContrib.FluentHtml.StringExtensions/*0200003B*/::FormatAsHtmlName(string) /* 0600010F */
    IL_000f:  /* 2B   | 01               */ br.s       IL_0012

    IL_0011:  /* 14   |                  */ ldnull
    IL_0012:  /* 14   |                  */ ldnull
    IL_0013:  /* 04   |                  */ ldarg.2
    IL_0014:  /* 28   | (0A)0000AD       */ call       instance void class MvcContrib.FluentHtml.Elements.Input`1/*02000020*/<!T>/*1B000008*/::.ctor(string,
                                                                                                                                                     string,
                                                                                                                                                     class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                                                     class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 0A0000AD */
    IL_0019:  /* 02   |                  */ ldarg.0
    IL_001a:  /* 03   |                  */ ldarg.1
    IL_001b:  /* 7D   | (0A)0000A8       */ stfld      object class MvcContrib.FluentHtml.Elements.Input`1/*02000020*/<!T>/*1B000008*/::elementValue /* 0A0000A8 */
    IL_0020:  /* 2A   |                  */ ret
  } // end of method SubmitButtonBase`1::.ctor

} // end of class MvcContrib.FluentHtml.Elements.SubmitButtonBase`1

.class /*0200003D*/ public abstract auto ansi beforefieldinit MvcContrib.FluentHtml.Elements.TextAreaBase`1<(class MvcContrib.FluentHtml.Elements.TextAreaBase`1/*0200003D*/<!T>/*1B000020*/) T>
       extends class MvcContrib.FluentHtml.Elements.FormElement`1/*0200001F*/<!T>/*1B000007*/
       implements MvcContrib.FluentHtml.Behaviors.ISupportsModelState/*02000016*/
{
  .field /*04000042*/ family string format
  .field /*04000043*/ family object rawValue
  .method /*06000113*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x47bd
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 72   | (70)00003F       */ ldstr      "textarea" /* 7000003F */
    IL_0006:  /* 03   |                  */ ldarg.1
    IL_0007:  /* 28   | (0A)0000A6       */ call       instance void class MvcContrib.FluentHtml.Elements.FormElement`1/*0200001F*/<!T>/*1B000007*/::.ctor(string,
                                                                                                                                                           string) /* 0A0000A6 */
    IL_000c:  /* 2A   |                  */ ret
  } // end of method TextAreaBase`1::.ctor

  .method /*06000114*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(string name,
                               class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ forMember,
                               class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> behaviors) cil managed
  // SIG: 20 03 01 0E 12 39 15 12 11 01 12 08
  {
    // Method begins at RVA 0x47cb
    // Code size       15 (0xf)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 72   | (70)00003F       */ ldstr      "textarea" /* 7000003F */
    IL_0006:  /* 03   |                  */ ldarg.1
    IL_0007:  /* 04   |                  */ ldarg.2
    IL_0008:  /* 05   |                  */ ldarg.3
    IL_0009:  /* 28   | (0A)0000A7       */ call       instance void class MvcContrib.FluentHtml.Elements.FormElement`1/*0200001F*/<!T>/*1B000007*/::.ctor(string,
                                                                                                                                                           string,
                                                                                                                                                           class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                                                           class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 0A0000A7 */
    IL_000e:  /* 2A   |                  */ ret
  } // end of method TextAreaBase`1::.ctor

  .method /*06000115*/ public hidebysig newslot virtual 
          instance !T  Value(object 'value') cil managed
  // SIG: 20 01 13 00 1C
  {
    // Method begins at RVA 0x47db
    // Code size       14 (0xe)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (0A)000172       */ stfld      object class MvcContrib.FluentHtml.Elements.TextAreaBase`1/*0200003D*/<!T>/*1B000020*/::rawValue /* 0A000172 */
    IL_0007:  /* 02   |                  */ ldarg.0
    IL_0008:  /* A5   | (1B)000040       */ unbox.any  !T/*1B000040*/
    IL_000d:  /* 2A   |                  */ ret
  } // end of method TextAreaBase`1::Value

  .method /*06000116*/ public hidebysig newslot virtual 
          instance !T  Rows(int32 'value') cil managed
  // SIG: 20 01 13 00 08
  {
    // Method begins at RVA 0x47ea
    // Code size       25 (0x19)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 72   | (70)000901       */ ldstr      "rows" /* 70000901 */
    IL_0006:  /* 03   |                  */ ldarg.1
    IL_0007:  /* 8C   | (01)000044       */ box        [mscorlib/*23000001*/]System.Int32/*01000044*/
    IL_000c:  /* 6F   | (0A)0000A0       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::Attr(string,
                                                                                                                                                    object) /* 0A0000A0 */
    IL_0011:  /* 26   |                  */ pop
    IL_0012:  /* 02   |                  */ ldarg.0
    IL_0013:  /* A5   | (1B)000040       */ unbox.any  !T/*1B000040*/
    IL_0018:  /* 2A   |                  */ ret
  } // end of method TextAreaBase`1::Rows

  .method /*06000117*/ public hidebysig newslot virtual 
          instance !T  Columns(int32 'value') cil managed
  // SIG: 20 01 13 00 08
  {
    // Method begins at RVA 0x4804
    // Code size       25 (0x19)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 72   | (70)00090B       */ ldstr      "cols" /* 7000090B */
    IL_0006:  /* 03   |                  */ ldarg.1
    IL_0007:  /* 8C   | (01)000044       */ box        [mscorlib/*23000001*/]System.Int32/*01000044*/
    IL_000c:  /* 6F   | (0A)0000A0       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::Attr(string,
                                                                                                                                                    object) /* 0A0000A0 */
    IL_0011:  /* 26   |                  */ pop
    IL_0012:  /* 02   |                  */ ldarg.0
    IL_0013:  /* A5   | (1B)000040       */ unbox.any  !T/*1B000040*/
    IL_0018:  /* 2A   |                  */ ret
  } // end of method TextAreaBase`1::Columns

  .method /*06000118*/ public hidebysig newslot virtual 
          instance !T  Format(string 'value') cil managed
  // SIG: 20 01 13 00 0E
  {
    // Method begins at RVA 0x481e
    // Code size       14 (0xe)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (0A)000173       */ stfld      string class MvcContrib.FluentHtml.Elements.TextAreaBase`1/*0200003D*/<!T>/*1B000020*/::format /* 0A000173 */
    IL_0007:  /* 02   |                  */ ldarg.0
    IL_0008:  /* A5   | (1B)000040       */ unbox.any  !T/*1B000040*/
    IL_000d:  /* 2A   |                  */ ret
  } // end of method TextAreaBase`1::Format

  .method /*06000119*/ family hidebysig virtual 
          instance void  PreRender() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4830
    // Code size       157 (0x9d)
    .maxstack  3
    .locals /*11000027*/ init (class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000021*/<string> V_0,
             object V_1,
             class [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000034*/ V_2,
             class [mscorlib/*23000001*/]System.IDisposable/*01000035*/ V_3)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (0A)000172       */ ldfld      object class MvcContrib.FluentHtml.Elements.TextAreaBase`1/*0200003D*/<!T>/*1B000020*/::rawValue /* 0A000172 */
    IL_0006:  /* 75   | (01)00003F       */ isinst     [mscorlib/*23000001*/]System.String/*0100003F*/
    IL_000b:  /* 2D   | 72               */ brtrue.s   IL_007f

    IL_000d:  /* 02   |                  */ ldarg.0
    IL_000e:  /* 7B   | (0A)000172       */ ldfld      object class MvcContrib.FluentHtml.Elements.TextAreaBase`1/*0200003D*/<!T>/*1B000020*/::rawValue /* 0A000172 */
    IL_0013:  /* 75   | (01)000015       */ isinst     [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000015*/
    IL_0018:  /* 2C   | 65               */ brfalse.s  IL_007f

    IL_001a:  /* 73   | (0A)000174       */ newobj     instance void class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000021*/<string>/*1B000049*/::.ctor() /* 0A000174 */
    IL_001f:  /* 0A   |                  */ stloc.0
    IL_0020:  /* 02   |                  */ ldarg.0
    IL_0021:  /* 7B   | (0A)000172       */ ldfld      object class MvcContrib.FluentHtml.Elements.TextAreaBase`1/*0200003D*/<!T>/*1B000020*/::rawValue /* 0A000172 */
    IL_0026:  /* 74   | (01)000015       */ castclass  [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000015*/
    IL_002b:  /* 6F   | (0A)0000EB       */ callvirt   instance class [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000034*/ [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000015*/::GetEnumerator() /* 0A0000EB */
    IL_0030:  /* 0C   |                  */ stloc.2
    .try
    {
      IL_0031:  /* 2B   | 14               */ br.s       IL_0047

      IL_0033:  /* 08   |                  */ ldloc.2
      IL_0034:  /* 6F   | (0A)0000E5       */ callvirt   instance object [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000034*/::get_Current() /* 0A0000E5 */
      IL_0039:  /* 0B   |                  */ stloc.1
      IL_003a:  /* 06   |                  */ ldloc.0
      IL_003b:  /* 02   |                  */ ldarg.0
      IL_003c:  /* 07   |                  */ ldloc.1
      IL_003d:  /* 6F   | (0A)000175       */ callvirt   instance string class MvcContrib.FluentHtml.Elements.TextAreaBase`1/*0200003D*/<!T>/*1B000020*/::FormatValue(object) /* 0A000175 */
      IL_0042:  /* 6F   | (0A)000176       */ callvirt   instance void class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000021*/<string>/*1B000049*/::Add(!0) /* 0A000176 */
      IL_0047:  /* 08   |                  */ ldloc.2
      IL_0048:  /* 6F   | (0A)00001A       */ callvirt   instance bool [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000034*/::MoveNext() /* 0A00001A */
      IL_004d:  /* 2D   | E4               */ brtrue.s   IL_0033

      IL_004f:  /* DE   | 11               */ leave.s    IL_0062

    }  // end .try
    finally
    {
      IL_0051:  /* 08   |                  */ ldloc.2
      IL_0052:  /* 75   | (01)000035       */ isinst     [mscorlib/*23000001*/]System.IDisposable/*01000035*/
      IL_0057:  /* 0D   |                  */ stloc.3
      IL_0058:  /* 09   |                  */ ldloc.3
      IL_0059:  /* 2C   | 06               */ brfalse.s  IL_0061

      IL_005b:  /* 09   |                  */ ldloc.3
      IL_005c:  /* 6F   | (0A)00001B       */ callvirt   instance void [mscorlib/*23000001*/]System.IDisposable/*01000035*/::Dispose() /* 0A00001B */
      IL_0061:  /* DC   |                  */ endfinally
    }  // end handler
    // HEX: 02 00 31 00 20 51 00 11 00 00 00 00
    IL_0062:  /* 02   |                  */ ldarg.0
    IL_0063:  /* 7B   | (0A)000065       */ ldfld      class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::builder /* 0A000065 */
    IL_0068:  /* 28   | (0A)000177       */ call       string [mscorlib/*23000001*/]System.Environment/*0100006C*/::get_NewLine() /* 0A000177 */
    IL_006d:  /* 06   |                  */ ldloc.0
    IL_006e:  /* 6F   | (0A)00008E       */ callvirt   instance !0[] class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000021*/<string>/*1B000049*/::ToArray() /* 0A00008E */
    IL_0073:  /* 28   | (0A)00008F       */ call       string [mscorlib/*23000001*/]System.String/*0100003F*/::Join(string,
                                                                                                                    string[]) /* 0A00008F */
    IL_0078:  /* 6F   | (0A)000093       */ callvirt   instance void [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/::SetInnerText(string) /* 0A000093 */
    IL_007d:  /* 2B   | 17               */ br.s       IL_0096

    IL_007f:  /* 02   |                  */ ldarg.0
    IL_0080:  /* 7B   | (0A)000065       */ ldfld      class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::builder /* 0A000065 */
    IL_0085:  /* 02   |                  */ ldarg.0
    IL_0086:  /* 02   |                  */ ldarg.0
    IL_0087:  /* 7B   | (0A)000172       */ ldfld      object class MvcContrib.FluentHtml.Elements.TextAreaBase`1/*0200003D*/<!T>/*1B000020*/::rawValue /* 0A000172 */
    IL_008c:  /* 6F   | (0A)000175       */ callvirt   instance string class MvcContrib.FluentHtml.Elements.TextAreaBase`1/*0200003D*/<!T>/*1B000020*/::FormatValue(object) /* 0A000175 */
    IL_0091:  /* 6F   | (0A)000093       */ callvirt   instance void [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/::SetInnerText(string) /* 0A000093 */
    IL_0096:  /* 02   |                  */ ldarg.0
    IL_0097:  /* 28   | (0A)000072       */ call       instance void class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::PreRender() /* 0A000072 */
    IL_009c:  /* 2A   |                  */ ret
  } // end of method TextAreaBase`1::PreRender

  .method /*0600011A*/ family hidebysig newslot virtual 
          instance string  FormatValue(object 'value') cil managed
  // SIG: 20 01 0E 1C
  {
    // Method begins at RVA 0x48ec
    // Code size       102 (0x66)
    .maxstack  3
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (0A)000173       */ ldfld      string class MvcContrib.FluentHtml.Elements.TextAreaBase`1/*0200003D*/<!T>/*1B000020*/::format /* 0A000173 */
    IL_0006:  /* 28   | (0A)000096       */ call       bool [mscorlib/*23000001*/]System.String/*0100003F*/::IsNullOrEmpty(string) /* 0A000096 */
    IL_000b:  /* 2D   | 4D               */ brtrue.s   IL_005a

    IL_000d:  /* 02   |                  */ ldarg.0
    IL_000e:  /* 7B   | (0A)000173       */ ldfld      string class MvcContrib.FluentHtml.Elements.TextAreaBase`1/*0200003D*/<!T>/*1B000020*/::format /* 0A000173 */
    IL_0013:  /* 72   | (70)0003ED       */ ldstr      "{0" /* 700003ED */
    IL_0018:  /* 6F   | (0A)00010B       */ callvirt   instance bool [mscorlib/*23000001*/]System.String/*0100003F*/::StartsWith(string) /* 0A00010B */
    IL_001d:  /* 2C   | 12               */ brfalse.s  IL_0031

    IL_001f:  /* 02   |                  */ ldarg.0
    IL_0020:  /* 7B   | (0A)000173       */ ldfld      string class MvcContrib.FluentHtml.Elements.TextAreaBase`1/*0200003D*/<!T>/*1B000020*/::format /* 0A000173 */
    IL_0025:  /* 72   | (70)0003F3       */ ldstr      "}" /* 700003F3 */
    IL_002a:  /* 6F   | (0A)00010C       */ callvirt   instance bool [mscorlib/*23000001*/]System.String/*0100003F*/::EndsWith(string) /* 0A00010C */
    IL_002f:  /* 2D   | 1C               */ brtrue.s   IL_004d

    IL_0031:  /* 72   | (70)0003F7       */ ldstr      "{0:" /* 700003F7 */
    IL_0036:  /* 02   |                  */ ldarg.0
    IL_0037:  /* 7B   | (0A)000173       */ ldfld      string class MvcContrib.FluentHtml.Elements.TextAreaBase`1/*0200003D*/<!T>/*1B000020*/::format /* 0A000173 */
    IL_003c:  /* 72   | (70)0003F3       */ ldstr      "}" /* 700003F3 */
    IL_0041:  /* 28   | (0A)00010D       */ call       string [mscorlib/*23000001*/]System.String/*0100003F*/::Concat(string,
                                                                                                                      string,
                                                                                                                      string) /* 0A00010D */
    IL_0046:  /* 03   |                  */ ldarg.1
    IL_0047:  /* 28   | (0A)000087       */ call       string [mscorlib/*23000001*/]System.String/*0100003F*/::Format(string,
                                                                                                                      object) /* 0A000087 */
    IL_004c:  /* 2A   |                  */ ret

    IL_004d:  /* 02   |                  */ ldarg.0
    IL_004e:  /* 7B   | (0A)000173       */ ldfld      string class MvcContrib.FluentHtml.Elements.TextAreaBase`1/*0200003D*/<!T>/*1B000020*/::format /* 0A000173 */
    IL_0053:  /* 03   |                  */ ldarg.1
    IL_0054:  /* 28   | (0A)000087       */ call       string [mscorlib/*23000001*/]System.String/*0100003F*/::Format(string,
                                                                                                                      object) /* 0A000087 */
    IL_0059:  /* 2A   |                  */ ret

    IL_005a:  /* 03   |                  */ ldarg.1
    IL_005b:  /* 2C   | 07               */ brfalse.s  IL_0064

    IL_005d:  /* 03   |                  */ ldarg.1
    IL_005e:  /* 6F   | (0A)00006E       */ callvirt   instance string [mscorlib/*23000001*/]System.Object/*01000001*/::ToString() /* 0A00006E */
    IL_0063:  /* 2A   |                  */ ret

    IL_0064:  /* 14   |                  */ ldnull
    IL_0065:  /* 2A   |                  */ ret
  } // end of method TextAreaBase`1::FormatValue

  .method /*0600011B*/ family hidebysig specialname virtual 
          instance valuetype [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagRenderMode/*01000013*/ 
          get_TagRenderMode() cil managed
  // SIG: 20 00 11 4D
  {
    // Method begins at RVA 0x495e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 16   |                  */ ldc.i4.0
    IL_0001:  /* 2A   |                  */ ret
  } // end of method TextAreaBase`1::get_TagRenderMode

  .method /*0600011C*/ private hidebysig newslot virtual final 
          instance void  MvcContrib.FluentHtml.Behaviors.ISupportsModelState.ApplyModelState(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelState/*0100000F*/ state) cil managed
  // SIG: 20 01 01 12 3D
  {
    .override MvcContrib.FluentHtml.Behaviors.ISupportsModelState/*02000016*/::ApplyModelState /*02000016::06000038*/ 
    // Method begins at RVA 0x4961
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 6F   | (0A)000178       */ callvirt   instance void class MvcContrib.FluentHtml.Elements.TextAreaBase`1/*0200003D*/<!T>/*1B000020*/::ApplyModelState(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelState/*0100000F*/) /* 0A000178 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method TextAreaBase`1::MvcContrib.FluentHtml.Behaviors.ISupportsModelState.ApplyModelState

  .method /*0600011D*/ family hidebysig newslot virtual 
          instance void  ApplyModelState(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelState/*0100000F*/ state) cil managed
  // SIG: 20 01 01 12 3D
  {
    // Method begins at RVA 0x496c
    // Code size       31 (0x1f)
    .maxstack  2
    .locals /*11000001*/ init (object V_0)
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 6F   | (0A)00005C       */ callvirt   instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ValueProviderResult/*01000050*/ [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelState/*0100000F*/::get_Value() /* 0A00005C */
    IL_0006:  /* D0   | (01)00003F       */ ldtoken    [mscorlib/*23000001*/]System.String/*0100003F*/
    IL_000b:  /* 28   | (0A)000021       */ call       class [mscorlib/*23000001*/]System.Type/*01000003*/ [mscorlib/*23000001*/]System.Type/*01000003*/::GetTypeFromHandle(valuetype [mscorlib/*23000001*/]System.RuntimeTypeHandle/*01000036*/) /* 0A000021 */
    IL_0010:  /* 6F   | (0A)0000AA       */ callvirt   instance object [System.Web.Mvc/*23000003*/]System.Web.Mvc.ValueProviderResult/*01000050*/::ConvertTo(class [mscorlib/*23000001*/]System.Type/*01000003*/) /* 0A0000AA */
    IL_0015:  /* 0A   |                  */ stloc.0
    IL_0016:  /* 02   |                  */ ldarg.0
    IL_0017:  /* 06   |                  */ ldloc.0
    IL_0018:  /* 6F   | (0A)000179       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.TextAreaBase`1/*0200003D*/<!T>/*1B000020*/::Value(object) /* 0A000179 */
    IL_001d:  /* 26   |                  */ pop
    IL_001e:  /* 2A   |                  */ ret
  } // end of method TextAreaBase`1::ApplyModelState

  .property /*1700001B*/ instance valuetype [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagRenderMode/*01000013*/
          TagRenderMode()
  {
    .get instance valuetype [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagRenderMode/*01000013*/ MvcContrib.FluentHtml.Elements.TextAreaBase`1/*0200003D*/::get_TagRenderMode() /* 0600011B */
  } // end of property TextAreaBase`1::TagRenderMode
} // end of class MvcContrib.FluentHtml.Elements.TextAreaBase`1

.class /*0200003E*/ public auto ansi beforefieldinit MvcContrib.FluentHtml.Elements.ValidationMessage
       extends class MvcContrib.FluentHtml.Elements.LiteralBase`1/*02000032*/<class MvcContrib.FluentHtml.Elements.ValidationMessage/*0200003E*/>/*1B000021*/
{
  .method /*0600011E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ forMember,
                               class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> behaviors) cil managed
  // SIG: 20 02 01 12 39 15 12 11 01 12 08
  {
    // Method begins at RVA 0x4997
    // Code size       14 (0xe)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 72   | (70)000915       */ ldstr      "" /* 70000915 */
    IL_0006:  /* 03   |                  */ ldarg.1
    IL_0007:  /* 04   |                  */ ldarg.2
    IL_0008:  /* 28   | (0A)00017A       */ call       instance void class MvcContrib.FluentHtml.Elements.LiteralBase`1/*02000032*/<class MvcContrib.FluentHtml.Elements.ValidationMessage/*0200003E*/>/*1B000021*/::.ctor(string,
                                                                                                                                                                                                                           class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                                                                                                                           class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 0A00017A */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method ValidationMessage::.ctor

  .method /*0600011F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x49a6
    // Code size       12 (0xc)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 72   | (70)000915       */ ldstr      "" /* 70000915 */
    IL_0006:  /* 28   | (0A)00017B       */ call       instance void class MvcContrib.FluentHtml.Elements.LiteralBase`1/*02000032*/<class MvcContrib.FluentHtml.Elements.ValidationMessage/*0200003E*/>/*1B000021*/::.ctor(string) /* 0A00017B */
    IL_000b:  /* 2A   |                  */ ret
  } // end of method ValidationMessage::.ctor

  .method /*06000120*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x49b3
    // Code size       52 (0x34)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (0A)00017C       */ ldfld      object class MvcContrib.FluentHtml.Elements.LiteralBase`1/*02000032*/<class MvcContrib.FluentHtml.Elements.ValidationMessage/*0200003E*/>/*1B000021*/::rawValue /* 0A00017C */
    IL_0006:  /* 2D   | 02               */ brtrue.s   IL_000a

    IL_0008:  /* 14   |                  */ ldnull
    IL_0009:  /* 2A   |                  */ ret

    IL_000a:  /* 02   |                  */ ldarg.0
    IL_000b:  /* 7B   | (0A)00017D       */ ldfld      class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<class MvcContrib.FluentHtml.Elements.ValidationMessage/*0200003E*/>/*1B00006A*/::builder /* 0A00017D */
    IL_0010:  /* 6F   | (0A)000076       */ callvirt   instance class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<string,string> [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/::get_Attributes() /* 0A000076 */
    IL_0015:  /* 72   | (70)000127       */ ldstr      "class" /* 70000127 */
    IL_001a:  /* 6F   | (0A)000094       */ callvirt   instance bool class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<string,string>/*1B000047*/::ContainsKey(!0) /* 0A000094 */
    IL_001f:  /* 2D   | 0C               */ brtrue.s   IL_002d

    IL_0021:  /* 02   |                  */ ldarg.0
    IL_0022:  /* 72   | (70)000917       */ ldstr      "field-validation-error" /* 70000917 */
    IL_0027:  /* 6F   | (0A)00017E       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<class MvcContrib.FluentHtml.Elements.ValidationMessage/*0200003E*/>/*1B00006A*/::Class(string) /* 0A00017E */
    IL_002c:  /* 26   |                  */ pop
    IL_002d:  /* 02   |                  */ ldarg.0
    IL_002e:  /* 28   | (0A)00017F       */ call       instance string class MvcContrib.FluentHtml.Elements.LiteralBase`1/*02000032*/<class MvcContrib.FluentHtml.Elements.ValidationMessage/*0200003E*/>/*1B000021*/::ToString() /* 0A00017F */
    IL_0033:  /* 2A   |                  */ ret
  } // end of method ValidationMessage::ToString

} // end of class MvcContrib.FluentHtml.Elements.ValidationMessage

.class /*0200003F*/ public auto ansi beforefieldinit MvcContrib.FluentHtml.Expressions.ExpressionNameVisitor
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .method /*06000121*/ public hidebysig instance string 
          Visit(class [System.Core/*23000002*/]System.Linq.Expressions.Expression/*0100001D*/ expression) cil managed
  // SIG: 20 01 0E 12 75
  {
    // Method begins at RVA 0x49e8
    // Code size       95 (0x5f)
    .maxstack  2
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 75   | (01)00006D       */ isinst     [System.Core/*23000002*/]System.Linq.Expressions.UnaryExpression/*0100006D*/
    IL_0006:  /* 2C   | 0D               */ brfalse.s  IL_0015

    IL_0008:  /* 03   |                  */ ldarg.1
    IL_0009:  /* 74   | (01)00006D       */ castclass  [System.Core/*23000002*/]System.Linq.Expressions.UnaryExpression/*0100006D*/
    IL_000e:  /* 6F   | (0A)000180       */ callvirt   instance class [System.Core/*23000002*/]System.Linq.Expressions.Expression/*0100001D*/ [System.Core/*23000002*/]System.Linq.Expressions.UnaryExpression/*0100006D*/::get_Operand() /* 0A000180 */
    IL_0013:  /* 10   | 01               */ starg.s    expression
    IL_0015:  /* 03   |                  */ ldarg.1
    IL_0016:  /* 75   | (01)00001F       */ isinst     [System.Core/*23000002*/]System.Linq.Expressions.MethodCallExpression/*0100001F*/
    IL_001b:  /* 2C   | 0D               */ brfalse.s  IL_002a

    IL_001d:  /* 02   |                  */ ldarg.0
    IL_001e:  /* 03   |                  */ ldarg.1
    IL_001f:  /* 74   | (01)00001F       */ castclass  [System.Core/*23000002*/]System.Linq.Expressions.MethodCallExpression/*0100001F*/
    IL_0024:  /* 28   | (06)000124       */ call       instance string MvcContrib.FluentHtml.Expressions.ExpressionNameVisitor/*0200003F*/::Visit(class [System.Core/*23000002*/]System.Linq.Expressions.MethodCallExpression/*0100001F*/) /* 06000124 */
    IL_0029:  /* 2A   |                  */ ret

    IL_002a:  /* 03   |                  */ ldarg.1
    IL_002b:  /* 75   | (01)00000E       */ isinst     [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/
    IL_0030:  /* 2C   | 0D               */ brfalse.s  IL_003f

    IL_0032:  /* 02   |                  */ ldarg.0
    IL_0033:  /* 03   |                  */ ldarg.1
    IL_0034:  /* 74   | (01)00000E       */ castclass  [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/
    IL_0039:  /* 28   | (06)000123       */ call       instance string MvcContrib.FluentHtml.Expressions.ExpressionNameVisitor/*0200003F*/::Visit(class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/) /* 06000123 */
    IL_003e:  /* 2A   |                  */ ret

    IL_003f:  /* 03   |                  */ ldarg.1
    IL_0040:  /* 75   | (01)00001E       */ isinst     [System.Core/*23000002*/]System.Linq.Expressions.BinaryExpression/*0100001E*/
    IL_0045:  /* 2C   | 16               */ brfalse.s  IL_005d

    IL_0047:  /* 03   |                  */ ldarg.1
    IL_0048:  /* 6F   | (0A)000181       */ callvirt   instance valuetype [System.Core/*23000002*/]System.Linq.Expressions.ExpressionType/*0100006E*/ [System.Core/*23000002*/]System.Linq.Expressions.Expression/*0100001D*/::get_NodeType() /* 0A000181 */
    IL_004d:  /* 1B   |                  */ ldc.i4.5
    IL_004e:  /* 33   | 0D               */ bne.un.s   IL_005d

    IL_0050:  /* 02   |                  */ ldarg.0
    IL_0051:  /* 03   |                  */ ldarg.1
    IL_0052:  /* 74   | (01)00001E       */ castclass  [System.Core/*23000002*/]System.Linq.Expressions.BinaryExpression/*0100001E*/
    IL_0057:  /* 28   | (06)000122       */ call       instance string MvcContrib.FluentHtml.Expressions.ExpressionNameVisitor/*0200003F*/::Visit(class [System.Core/*23000002*/]System.Linq.Expressions.BinaryExpression/*0100001E*/) /* 06000122 */
    IL_005c:  /* 2A   |                  */ ret

    IL_005d:  /* 14   |                  */ ldnull
    IL_005e:  /* 2A   |                  */ ret
  } // end of method ExpressionNameVisitor::Visit

  .method /*06000122*/ private hidebysig 
          instance string  Visit(class [System.Core/*23000002*/]System.Linq.Expressions.BinaryExpression/*0100001E*/ expression) cil managed
  // SIG: 20 01 0E 12 79
  {
    // Method begins at RVA 0x4a54
    // Code size       85 (0x55)
    .maxstack  3
    .locals /*11000028*/ init (string V_0,
             object V_1)
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 0A   |                  */ stloc.0
    IL_0002:  /* 03   |                  */ ldarg.1
    IL_0003:  /* 6F   | (0A)000182       */ callvirt   instance class [System.Core/*23000002*/]System.Linq.Expressions.Expression/*0100001D*/ [System.Core/*23000002*/]System.Linq.Expressions.BinaryExpression/*0100001E*/::get_Left() /* 0A000182 */
    IL_0008:  /* 75   | (01)00000E       */ isinst     [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/
    IL_000d:  /* 2C   | 12               */ brfalse.s  IL_0021

    IL_000f:  /* 02   |                  */ ldarg.0
    IL_0010:  /* 03   |                  */ ldarg.1
    IL_0011:  /* 6F   | (0A)000182       */ callvirt   instance class [System.Core/*23000002*/]System.Linq.Expressions.Expression/*0100001D*/ [System.Core/*23000002*/]System.Linq.Expressions.BinaryExpression/*0100001E*/::get_Left() /* 0A000182 */
    IL_0016:  /* 74   | (01)00000E       */ castclass  [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/
    IL_001b:  /* 28   | (06)000123       */ call       instance string MvcContrib.FluentHtml.Expressions.ExpressionNameVisitor/*0200003F*/::Visit(class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/) /* 06000123 */
    IL_0020:  /* 0A   |                  */ stloc.0
    IL_0021:  /* 03   |                  */ ldarg.1
    IL_0022:  /* 6F   | (0A)000183       */ callvirt   instance class [System.Core/*23000002*/]System.Linq.Expressions.Expression/*0100001D*/ [System.Core/*23000002*/]System.Linq.Expressions.BinaryExpression/*0100001E*/::get_Right() /* 0A000183 */
    IL_0027:  /* 16   |                  */ ldc.i4.0
    IL_0028:  /* 8D   | (01)00006F       */ newarr     [System.Core/*23000002*/]System.Linq.Expressions.ParameterExpression/*0100006F*/
    IL_002d:  /* 28   | (0A)000184       */ call       class [System.Core/*23000002*/]System.Linq.Expressions.LambdaExpression/*01000070*/ [System.Core/*23000002*/]System.Linq.Expressions.Expression/*0100001D*/::Lambda(class [System.Core/*23000002*/]System.Linq.Expressions.Expression/*0100001D*/,
                                                                                                                                                                                                                           class [System.Core/*23000002*/]System.Linq.Expressions.ParameterExpression/*0100006F*/[]) /* 0A000184 */
    IL_0032:  /* 6F   | (0A)000185       */ callvirt   instance class [mscorlib/*23000001*/]System.Delegate/*0100003D*/ [System.Core/*23000002*/]System.Linq.Expressions.LambdaExpression/*01000070*/::Compile() /* 0A000185 */
    IL_0037:  /* 16   |                  */ ldc.i4.0
    IL_0038:  /* 8D   | (01)000001       */ newarr     [mscorlib/*23000001*/]System.Object/*01000001*/
    IL_003d:  /* 6F   | (0A)000186       */ callvirt   instance object [mscorlib/*23000001*/]System.Delegate/*0100003D*/::DynamicInvoke(object[]) /* 0A000186 */
    IL_0042:  /* 0B   |                  */ stloc.1
    IL_0043:  /* 06   |                  */ ldloc.0
    IL_0044:  /* 72   | (70)000945       */ ldstr      "[{0}]" /* 70000945 */
    IL_0049:  /* 07   |                  */ ldloc.1
    IL_004a:  /* 28   | (0A)000087       */ call       string [mscorlib/*23000001*/]System.String/*0100003F*/::Format(string,
                                                                                                                      object) /* 0A000087 */
    IL_004f:  /* 28   | (0A)000075       */ call       string [mscorlib/*23000001*/]System.String/*0100003F*/::Concat(string,
                                                                                                                      string) /* 0A000075 */
    IL_0054:  /* 2A   |                  */ ret
  } // end of method ExpressionNameVisitor::Visit

  .method /*06000123*/ private hidebysig 
          instance string  Visit(class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ expression) cil managed
  // SIG: 20 01 0E 12 39
  {
    // Method begins at RVA 0x4ab8
    // Code size       43 (0x2b)
    .maxstack  3
    .locals /*1100000B*/ init (string V_0,
             string V_1)
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 6F   | (0A)000048       */ callvirt   instance class [mscorlib/*23000001*/]System.Reflection.MemberInfo/*0100003E*/ [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/::get_Member() /* 0A000048 */
    IL_0006:  /* 6F   | (0A)000147       */ callvirt   instance string [mscorlib/*23000001*/]System.Reflection.MemberInfo/*0100003E*/::get_Name() /* 0A000147 */
    IL_000b:  /* 0A   |                  */ stloc.0
    IL_000c:  /* 02   |                  */ ldarg.0
    IL_000d:  /* 03   |                  */ ldarg.1
    IL_000e:  /* 6F   | (0A)000187       */ callvirt   instance class [System.Core/*23000002*/]System.Linq.Expressions.Expression/*0100001D*/ [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/::get_Expression() /* 0A000187 */
    IL_0013:  /* 28   | (06)000121       */ call       instance string MvcContrib.FluentHtml.Expressions.ExpressionNameVisitor/*0200003F*/::Visit(class [System.Core/*23000002*/]System.Linq.Expressions.Expression/*0100001D*/) /* 06000121 */
    IL_0018:  /* 0B   |                  */ stloc.1
    IL_0019:  /* 07   |                  */ ldloc.1
    IL_001a:  /* 2C   | 0D               */ brfalse.s  IL_0029

    IL_001c:  /* 07   |                  */ ldloc.1
    IL_001d:  /* 72   | (70)0000F3       */ ldstr      "." /* 700000F3 */
    IL_0022:  /* 06   |                  */ ldloc.0
    IL_0023:  /* 28   | (0A)00010D       */ call       string [mscorlib/*23000001*/]System.String/*0100003F*/::Concat(string,
                                                                                                                      string,
                                                                                                                      string) /* 0A00010D */
    IL_0028:  /* 0A   |                  */ stloc.0
    IL_0029:  /* 06   |                  */ ldloc.0
    IL_002a:  /* 2A   |                  */ ret
  } // end of method ExpressionNameVisitor::Visit

  .method /*06000124*/ private hidebysig 
          instance string  Visit(class [System.Core/*23000002*/]System.Linq.Expressions.MethodCallExpression/*0100001F*/ expression) cil managed
  // SIG: 20 01 0E 12 7D
  {
    // Method begins at RVA 0x4af0
    // Code size       130 (0x82)
    .maxstack  3
    .locals /*11000028*/ init (string V_0,
             object V_1)
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 0A   |                  */ stloc.0
    IL_0002:  /* 03   |                  */ ldarg.1
    IL_0003:  /* 6F   | (0A)000188       */ callvirt   instance class [System.Core/*23000002*/]System.Linq.Expressions.Expression/*0100001D*/ [System.Core/*23000002*/]System.Linq.Expressions.MethodCallExpression/*0100001F*/::get_Object() /* 0A000188 */
    IL_0008:  /* 75   | (01)00000E       */ isinst     [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/
    IL_000d:  /* 2C   | 12               */ brfalse.s  IL_0021

    IL_000f:  /* 02   |                  */ ldarg.0
    IL_0010:  /* 03   |                  */ ldarg.1
    IL_0011:  /* 6F   | (0A)000188       */ callvirt   instance class [System.Core/*23000002*/]System.Linq.Expressions.Expression/*0100001D*/ [System.Core/*23000002*/]System.Linq.Expressions.MethodCallExpression/*0100001F*/::get_Object() /* 0A000188 */
    IL_0016:  /* 74   | (01)00000E       */ castclass  [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/
    IL_001b:  /* 28   | (06)000123       */ call       instance string MvcContrib.FluentHtml.Expressions.ExpressionNameVisitor/*0200003F*/::Visit(class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/) /* 06000123 */
    IL_0020:  /* 0A   |                  */ stloc.0
    IL_0021:  /* 03   |                  */ ldarg.1
    IL_0022:  /* 6F   | (0A)000189       */ callvirt   instance class [mscorlib/*23000001*/]System.Reflection.MethodInfo/*01000010*/ [System.Core/*23000002*/]System.Linq.Expressions.MethodCallExpression/*0100001F*/::get_Method() /* 0A000189 */
    IL_0027:  /* 6F   | (0A)000147       */ callvirt   instance string [mscorlib/*23000001*/]System.Reflection.MemberInfo/*0100003E*/::get_Name() /* 0A000147 */
    IL_002c:  /* 72   | (70)000951       */ ldstr      "get_Item" /* 70000951 */
    IL_0031:  /* 28   | (0A)00004B       */ call       bool [mscorlib/*23000001*/]System.String/*0100003F*/::op_Equality(string,
                                                                                                                         string) /* 0A00004B */
    IL_0036:  /* 2C   | 48               */ brfalse.s  IL_0080

    IL_0038:  /* 03   |                  */ ldarg.1
    IL_0039:  /* 6F   | (0A)00018A       */ callvirt   instance class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000071*/<class [System.Core/*23000002*/]System.Linq.Expressions.Expression/*0100001D*/> [System.Core/*23000002*/]System.Linq.Expressions.MethodCallExpression/*0100001F*/::get_Arguments() /* 0A00018A */
    IL_003e:  /* 6F   | (0A)00018B       */ callvirt   instance int32 class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000071*/<class [System.Core/*23000002*/]System.Linq.Expressions.Expression/*0100001D*/>/*1B00006B*/::get_Count() /* 0A00018B */
    IL_0043:  /* 17   |                  */ ldc.i4.1
    IL_0044:  /* 33   | 3A               */ bne.un.s   IL_0080

    IL_0046:  /* 03   |                  */ ldarg.1
    IL_0047:  /* 6F   | (0A)00018A       */ callvirt   instance class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000071*/<class [System.Core/*23000002*/]System.Linq.Expressions.Expression/*0100001D*/> [System.Core/*23000002*/]System.Linq.Expressions.MethodCallExpression/*0100001F*/::get_Arguments() /* 0A00018A */
    IL_004c:  /* 16   |                  */ ldc.i4.0
    IL_004d:  /* 6F   | (0A)00018C       */ callvirt   instance !0 class [mscorlib/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*01000071*/<class [System.Core/*23000002*/]System.Linq.Expressions.Expression/*0100001D*/>/*1B00006B*/::get_Item(int32) /* 0A00018C */
    IL_0052:  /* 16   |                  */ ldc.i4.0
    IL_0053:  /* 8D   | (01)00006F       */ newarr     [System.Core/*23000002*/]System.Linq.Expressions.ParameterExpression/*0100006F*/
    IL_0058:  /* 28   | (0A)000184       */ call       class [System.Core/*23000002*/]System.Linq.Expressions.LambdaExpression/*01000070*/ [System.Core/*23000002*/]System.Linq.Expressions.Expression/*0100001D*/::Lambda(class [System.Core/*23000002*/]System.Linq.Expressions.Expression/*0100001D*/,
                                                                                                                                                                                                                           class [System.Core/*23000002*/]System.Linq.Expressions.ParameterExpression/*0100006F*/[]) /* 0A000184 */
    IL_005d:  /* 6F   | (0A)000185       */ callvirt   instance class [mscorlib/*23000001*/]System.Delegate/*0100003D*/ [System.Core/*23000002*/]System.Linq.Expressions.LambdaExpression/*01000070*/::Compile() /* 0A000185 */
    IL_0062:  /* 16   |                  */ ldc.i4.0
    IL_0063:  /* 8D   | (01)000001       */ newarr     [mscorlib/*23000001*/]System.Object/*01000001*/
    IL_0068:  /* 6F   | (0A)000186       */ callvirt   instance object [mscorlib/*23000001*/]System.Delegate/*0100003D*/::DynamicInvoke(object[]) /* 0A000186 */
    IL_006d:  /* 0B   |                  */ stloc.1
    IL_006e:  /* 06   |                  */ ldloc.0
    IL_006f:  /* 72   | (70)000945       */ ldstr      "[{0}]" /* 70000945 */
    IL_0074:  /* 07   |                  */ ldloc.1
    IL_0075:  /* 28   | (0A)000087       */ call       string [mscorlib/*23000001*/]System.String/*0100003F*/::Format(string,
                                                                                                                      object) /* 0A000087 */
    IL_007a:  /* 28   | (0A)000075       */ call       string [mscorlib/*23000001*/]System.String/*0100003F*/::Concat(string,
                                                                                                                      string) /* 0A000075 */
    IL_007f:  /* 0A   |                  */ stloc.0
    IL_0080:  /* 06   |                  */ ldloc.0
    IL_0081:  /* 2A   |                  */ ret
  } // end of method ExpressionNameVisitor::Visit

  .method /*06000125*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4b7e
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000012       */ call       instance void [mscorlib/*23000001*/]System.Object/*01000001*/::.ctor() /* 0A000012 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ExpressionNameVisitor::.ctor

} // end of class MvcContrib.FluentHtml.Expressions.ExpressionNameVisitor

.class /*02000040*/ public auto ansi beforefieldinit MvcContrib.FluentHtml.Expressions.ExpressionNameVisitorResult
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .field /*04000044*/ private class [System.Core/*23000002*/]System.Linq.Expressions.Expression/*0100001D*/ '<NextExpression>k__BackingField'
  .custom /*0C000035:0A000013*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000032*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
  .field /*04000045*/ private string '<Name>k__BackingField'
  .custom /*0C000036:0A000013*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000032*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
  .method /*06000126*/ public hidebysig specialname 
          instance class [System.Core/*23000002*/]System.Linq.Expressions.Expression/*0100001D*/ 
          get_NextExpression() cil managed
  // SIG: 20 00 12 75
  {
    .custom /*0C000056:0A000013*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000032*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4b86
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (04)000044       */ ldfld      class [System.Core/*23000002*/]System.Linq.Expressions.Expression/*0100001D*/ MvcContrib.FluentHtml.Expressions.ExpressionNameVisitorResult/*02000040*/::'<NextExpression>k__BackingField' /* 04000044 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ExpressionNameVisitorResult::get_NextExpression

  .method /*06000127*/ public hidebysig specialname 
          instance void  set_NextExpression(class [System.Core/*23000002*/]System.Linq.Expressions.Expression/*0100001D*/ 'value') cil managed
  // SIG: 20 01 01 12 75
  {
    .custom /*0C000057:0A000013*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000032*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4b8e
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (04)000044       */ stfld      class [System.Core/*23000002*/]System.Linq.Expressions.Expression/*0100001D*/ MvcContrib.FluentHtml.Expressions.ExpressionNameVisitorResult/*02000040*/::'<NextExpression>k__BackingField' /* 04000044 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method ExpressionNameVisitorResult::set_NextExpression

  .method /*06000128*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000058:0A000013*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000032*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4b97
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (04)000045       */ ldfld      string MvcContrib.FluentHtml.Expressions.ExpressionNameVisitorResult/*02000040*/::'<Name>k__BackingField' /* 04000045 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ExpressionNameVisitorResult::get_Name

  .method /*06000129*/ public hidebysig specialname 
          instance void  set_Name(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000059:0A000013*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000032*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4b9f
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (04)000045       */ stfld      string MvcContrib.FluentHtml.Expressions.ExpressionNameVisitorResult/*02000040*/::'<Name>k__BackingField' /* 04000045 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method ExpressionNameVisitorResult::set_Name

  .method /*0600012A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4ba8
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000012       */ call       instance void [mscorlib/*23000001*/]System.Object/*01000001*/::.ctor() /* 0A000012 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ExpressionNameVisitorResult::.ctor

  .property /*1700001C*/ instance class [System.Core/*23000002*/]System.Linq.Expressions.Expression/*0100001D*/
          NextExpression()
  {
    .set instance void MvcContrib.FluentHtml.Expressions.ExpressionNameVisitorResult/*02000040*/::set_NextExpression(class [System.Core/*23000002*/]System.Linq.Expressions.Expression/*0100001D*/) /* 06000127 */
    .get instance class [System.Core/*23000002*/]System.Linq.Expressions.Expression/*0100001D*/ MvcContrib.FluentHtml.Expressions.ExpressionNameVisitorResult/*02000040*/::get_NextExpression() /* 06000126 */
  } // end of property ExpressionNameVisitorResult::NextExpression
  .property /*1700001D*/ instance string Name()
  {
    .get instance string MvcContrib.FluentHtml.Expressions.ExpressionNameVisitorResult/*02000040*/::get_Name() /* 06000128 */
    .set instance void MvcContrib.FluentHtml.Expressions.ExpressionNameVisitorResult/*02000040*/::set_Name(string) /* 06000129 */
  } // end of property ExpressionNameVisitorResult::Name
} // end of class MvcContrib.FluentHtml.Expressions.ExpressionNameVisitorResult

.class /*02000041*/ public abstract auto ansi sealed beforefieldinit MvcContrib.FluentHtml.Html.HtmlEventAttribute
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .field /*04000046*/ public static literal string OnClick = "onclick"
} // end of class MvcContrib.FluentHtml.Html.HtmlEventAttribute

.class /*02000042*/ public abstract auto ansi sealed beforefieldinit MvcContrib.FluentHtml.Html.HtmlInputType
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .field /*04000047*/ public static literal string Text = "text"
  .field /*04000048*/ public static literal string Checkbox = "checkbox"
  .field /*04000049*/ public static literal string Hidden = "hidden"
  .field /*0400004A*/ public static literal string Password = "password"
  .field /*0400004B*/ public static literal string Submit = "submit"
  .field /*0400004C*/ public static literal string File = "file"
  .field /*0400004D*/ public static literal string Button = "button"
  .field /*0400004E*/ public static literal string Reset = "reset"
  .field /*0400004F*/ public static string Radio
  .method /*0600012B*/ private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x4bb0
    // Code size       11 (0xb)
    .maxstack  8
    IL_0000:  /* 72   | (70)000963       */ ldstr      "radio" /* 70000963 */
    IL_0005:  /* 80   | (04)00004F       */ stsfld     string MvcContrib.FluentHtml.Html.HtmlInputType/*02000042*/::Radio /* 0400004F */
    IL_000a:  /* 2A   |                  */ ret
  } // end of method HtmlInputType::.cctor

} // end of class MvcContrib.FluentHtml.Html.HtmlInputType

.class /*02000043*/ public abstract auto ansi sealed beforefieldinit MvcContrib.FluentHtml.Html.HtmlTag
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .field /*04000050*/ public static literal string Div = "div"
  .field /*04000051*/ public static literal string Input = "input"
  .field /*04000052*/ public static literal string Span = "span"
  .field /*04000053*/ public static literal string Select = "select"
  .field /*04000054*/ public static literal string TextArea = "textarea"
  .field /*04000055*/ public static literal string Option = "option"
  .field /*04000056*/ public static literal string Label = "label"
  .field /*04000057*/ public static literal string UnOrderedList = "ul"
  .field /*04000058*/ public static literal string ListItem = "li"
} // end of class MvcContrib.FluentHtml.Html.HtmlTag

.class /*02000044*/ public auto ansi beforefieldinit MvcContrib.FluentHtml.Elements.Literal
       extends class MvcContrib.FluentHtml.Elements.LiteralBase`1/*02000032*/<class MvcContrib.FluentHtml.Elements.Literal/*02000044*/>/*1B000022*/
{
  .method /*0600012C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name,
                               class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ forMember,
                               class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> behaviors) cil managed
  // SIG: 20 03 01 0E 12 39 15 12 11 01 12 08
  {
    // Method begins at RVA 0x4bbc
    // Code size       10 (0xa)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 04   |                  */ ldarg.2
    IL_0003:  /* 05   |                  */ ldarg.3
    IL_0004:  /* 28   | (0A)00018D       */ call       instance void class MvcContrib.FluentHtml.Elements.LiteralBase`1/*02000032*/<class MvcContrib.FluentHtml.Elements.Literal/*02000044*/>/*1B000022*/::.ctor(string,
                                                                                                                                                                                                                 class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                                                                                                                 class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 0A00018D */
    IL_0009:  /* 2A   |                  */ ret
  } // end of method Literal::.ctor

  .method /*0600012D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x4bc7
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 28   | (0A)00018E       */ call       instance void class MvcContrib.FluentHtml.Elements.LiteralBase`1/*02000032*/<class MvcContrib.FluentHtml.Elements.Literal/*02000044*/>/*1B000022*/::.ctor(string) /* 0A00018E */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method Literal::.ctor

} // end of class MvcContrib.FluentHtml.Elements.Literal

.class /*02000045*/ public abstract auto ansi beforefieldinit MvcContrib.FluentHtml.Elements.SelectBase`1<(class MvcContrib.FluentHtml.Elements.SelectBase`1/*02000045*/<!T>/*1B000023*/) T>
       extends class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/
{
  .field /*04000059*/ family class MvcContrib.FluentHtml.Elements.Option/*02000048*/ _firstOption
  .field /*0400005A*/ family bool _hideFirstOption
  .field /*0400005B*/ family class [System.Core/*23000002*/]System.Action`3/*01000019*/<class MvcContrib.FluentHtml.Elements.Option/*02000048*/,object,int32> _optionModifier
  .method /*0600012E*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x4bd0
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 72   | (70)00096F       */ ldstr      "select" /* 7000096F */
    IL_0006:  /* 03   |                  */ ldarg.1
    IL_0007:  /* 28   | (0A)0000F3       */ call       instance void class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/::.ctor(string,
                                                                                                                                                                  string) /* 0A0000F3 */
    IL_000c:  /* 2A   |                  */ ret
  } // end of method SelectBase`1::.ctor

  .method /*0600012F*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(string name,
                               class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ forMember,
                               class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> behaviors) cil managed
  // SIG: 20 03 01 0E 12 39 15 12 11 01 12 08
  {
    // Method begins at RVA 0x4bde
    // Code size       15 (0xf)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 72   | (70)00096F       */ ldstr      "select" /* 7000096F */
    IL_0006:  /* 03   |                  */ ldarg.1
    IL_0007:  /* 04   |                  */ ldarg.2
    IL_0008:  /* 05   |                  */ ldarg.3
    IL_0009:  /* 28   | (0A)0000F2       */ call       instance void class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/::.ctor(string,
                                                                                                                                                                  string,
                                                                                                                                                                  class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                                                                  class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 0A0000F2 */
    IL_000e:  /* 2A   |                  */ ret
  } // end of method SelectBase`1::.ctor

  .method /*06000130*/ public hidebysig newslot virtual 
          instance !T  Size(int32 'value') cil managed
  // SIG: 20 01 13 00 08
  {
    // Method begins at RVA 0x4bee
    // Code size       25 (0x19)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 72   | (70)000145       */ ldstr      "size" /* 70000145 */
    IL_0006:  /* 03   |                  */ ldarg.1
    IL_0007:  /* 8C   | (01)000044       */ box        [mscorlib/*23000001*/]System.Int32/*01000044*/
    IL_000c:  /* 6F   | (0A)0000A0       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<!T>/*1B000005*/::Attr(string,
                                                                                                                                                    object) /* 0A0000A0 */
    IL_0011:  /* 26   |                  */ pop
    IL_0012:  /* 02   |                  */ ldarg.0
    IL_0013:  /* A5   | (1B)000040       */ unbox.any  !T/*1B000040*/
    IL_0018:  /* 2A   |                  */ ret
  } // end of method SelectBase`1::Size

  .method /*06000131*/ public hidebysig newslot virtual 
          instance !T  FirstOption(class MvcContrib.FluentHtml.Elements.Option/*02000048*/ firstOption) cil managed
  // SIG: 20 01 13 00 12 81 20
  {
    // Method begins at RVA 0x4c08
    // Code size       14 (0xe)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (0A)00018F       */ stfld      class MvcContrib.FluentHtml.Elements.Option/*02000048*/ class MvcContrib.FluentHtml.Elements.SelectBase`1/*02000045*/<!T>/*1B000023*/::_firstOption /* 0A00018F */
    IL_0007:  /* 02   |                  */ ldarg.0
    IL_0008:  /* A5   | (1B)000040       */ unbox.any  !T/*1B000040*/
    IL_000d:  /* 2A   |                  */ ret
  } // end of method SelectBase`1::FirstOption

  .method /*06000132*/ public hidebysig newslot virtual 
          instance !T  FirstOption(string text) cil managed
  // SIG: 20 01 13 00 0E
  {
    // Method begins at RVA 0x4c17
    // Code size       16 (0x10)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 03   |                  */ ldarg.1
    IL_0003:  /* 6F   | (0A)000190       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.SelectBase`1/*02000045*/<!T>/*1B000023*/::FirstOption(string,
                                                                                                                                                              string) /* 0A000190 */
    IL_0008:  /* 26   |                  */ pop
    IL_0009:  /* 02   |                  */ ldarg.0
    IL_000a:  /* A5   | (1B)000040       */ unbox.any  !T/*1B000040*/
    IL_000f:  /* 2A   |                  */ ret
  } // end of method SelectBase`1::FirstOption

  .method /*06000133*/ public hidebysig newslot virtual 
          instance !T  FirstOption(string 'value',
                                   string text) cil managed
  // SIG: 20 02 13 00 0E 0E
  {
    // Method begins at RVA 0x4c28
    // Code size       54 (0x36)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 73   | (06)000141       */ newobj     instance void MvcContrib.FluentHtml.Elements.Option/*02000048*/::.ctor() /* 06000141 */
    IL_0006:  /* 04   |                  */ ldarg.2
    IL_0007:  /* 25   |                  */ dup
    IL_0008:  /* 2D   | 06               */ brtrue.s   IL_0010

    IL_000a:  /* 26   |                  */ pop
    IL_000b:  /* 7E   | (0A)0000EF       */ ldsfld     string [mscorlib/*23000001*/]System.String/*0100003F*/::Empty /* 0A0000EF */
    IL_0010:  /* 6F   | (06)000143       */ callvirt   instance class MvcContrib.FluentHtml.Elements.Option/*02000048*/ MvcContrib.FluentHtml.Elements.Option/*02000048*/::Text(string) /* 06000143 */
    IL_0015:  /* 03   |                  */ ldarg.1
    IL_0016:  /* 25   |                  */ dup
    IL_0017:  /* 2D   | 06               */ brtrue.s   IL_001f

    IL_0019:  /* 26   |                  */ pop
    IL_001a:  /* 7E   | (0A)0000EF       */ ldsfld     string [mscorlib/*23000001*/]System.String/*0100003F*/::Empty /* 0A0000EF */
    IL_001f:  /* 6F   | (06)000142       */ callvirt   instance class MvcContrib.FluentHtml.Elements.Option/*02000048*/ MvcContrib.FluentHtml.Elements.Option/*02000048*/::Value(string) /* 06000142 */
    IL_0024:  /* 16   |                  */ ldc.i4.0
    IL_0025:  /* 6F   | (06)000144       */ callvirt   instance class MvcContrib.FluentHtml.Elements.Option/*02000048*/ MvcContrib.FluentHtml.Elements.Option/*02000048*/::Selected(bool) /* 06000144 */
    IL_002a:  /* 7D   | (0A)00018F       */ stfld      class MvcContrib.FluentHtml.Elements.Option/*02000048*/ class MvcContrib.FluentHtml.Elements.SelectBase`1/*02000045*/<!T>/*1B000023*/::_firstOption /* 0A00018F */
    IL_002f:  /* 02   |                  */ ldarg.0
    IL_0030:  /* A5   | (1B)000040       */ unbox.any  !T/*1B000040*/
    IL_0035:  /* 2A   |                  */ ret
  } // end of method SelectBase`1::FirstOption

  .method /*06000134*/ public hidebysig newslot virtual 
          instance !T  FirstOptionText(string firstOptionText) cil managed
  // SIG: 20 01 13 00 0E
  {
    .custom /*0C00005A:0A000055*/ instance void [mscorlib/*23000001*/]System.ObsoleteAttribute/*0100004E*/::.ctor(string) /* 0A000055 */ = ( 01 00 25 55 73 65 20 74 68 65 20 27 46 69 72 73   // ..%Use the 'Firs
                                                                                                                                             74 4F 70 74 69 6F 6E 27 20 6D 65 74 68 6F 64 20   // tOption' method 
                                                                                                                                             69 6E 73 74 65 61 64 2E 00 00 )                   // instead...
    // Method begins at RVA 0x4c5f
    // Code size       15 (0xf)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 6F   | (0A)000191       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.SelectBase`1/*02000045*/<!T>/*1B000023*/::FirstOption(string) /* 0A000191 */
    IL_0007:  /* 26   |                  */ pop
    IL_0008:  /* 02   |                  */ ldarg.0
    IL_0009:  /* A5   | (1B)000040       */ unbox.any  !T/*1B000040*/
    IL_000e:  /* 2A   |                  */ ret
  } // end of method SelectBase`1::FirstOptionText

  .method /*06000135*/ public hidebysig newslot virtual 
          instance !T  HideFirstOptionWhen(bool hideFirstOption) cil managed
  // SIG: 20 01 13 00 02
  {
    // Method begins at RVA 0x4c6f
    // Code size       14 (0xe)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (0A)000192       */ stfld      bool class MvcContrib.FluentHtml.Elements.SelectBase`1/*02000045*/<!T>/*1B000023*/::_hideFirstOption /* 0A000192 */
    IL_0007:  /* 02   |                  */ ldarg.0
    IL_0008:  /* A5   | (1B)000040       */ unbox.any  !T/*1B000040*/
    IL_000d:  /* 2A   |                  */ ret
  } // end of method SelectBase`1::HideFirstOptionWhen

  .method /*06000136*/ public hidebysig newslot virtual 
          instance !T  EachOption(class [System.Core/*23000002*/]System.Action`3/*01000019*/<class MvcContrib.FluentHtml.Elements.Option/*02000048*/,object,int32> action) cil managed
  // SIG: 20 01 13 00 15 12 65 03 12 81 20 1C 08
  {
    // Method begins at RVA 0x4c7e
    // Code size       14 (0xe)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (0A)000193       */ stfld      class [System.Core/*23000002*/]System.Action`3/*01000019*/<class MvcContrib.FluentHtml.Elements.Option/*02000048*/,object,int32> class MvcContrib.FluentHtml.Elements.SelectBase`1/*02000045*/<!T>/*1B000023*/::_optionModifier /* 0A000193 */
    IL_0007:  /* 02   |                  */ ldarg.0
    IL_0008:  /* A5   | (1B)000040       */ unbox.any  !T/*1B000040*/
    IL_000d:  /* 2A   |                  */ ret
  } // end of method SelectBase`1::EachOption

  .method /*06000137*/ family hidebysig specialname virtual 
          instance valuetype [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagRenderMode/*01000013*/ 
          get_TagRenderMode() cil managed
  // SIG: 20 00 11 4D
  {
    // Method begins at RVA 0x4c8d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 16   |                  */ ldc.i4.0
    IL_0001:  /* 2A   |                  */ ret
  } // end of method SelectBase`1::get_TagRenderMode

  .method /*06000138*/ family hidebysig virtual 
          instance string  RenderOptions() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x4c90
    // Code size       177 (0xb1)
    .maxstack  4
    .locals /*11000029*/ init (class [mscorlib/*23000001*/]System.Text.StringBuilder/*01000055*/ V_0,
             int32 V_1,
             class MvcContrib.FluentHtml.Elements.Option/*02000048*/ V_2,
             object V_3,
             class MvcContrib.FluentHtml.Elements.Option/*02000048*/ V_4,
             class [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000034*/ V_5,
             class [mscorlib/*23000001*/]System.IDisposable/*01000035*/ V_6)
    IL_0000:  /* 73   | (0A)000067       */ newobj     instance void [mscorlib/*23000001*/]System.Text.StringBuilder/*01000055*/::.ctor() /* 0A000067 */
    IL_0005:  /* 0A   |                  */ stloc.0
    IL_0006:  /* 16   |                  */ ldc.i4.0
    IL_0007:  /* 0B   |                  */ stloc.1
    IL_0008:  /* 02   |                  */ ldarg.0
    IL_0009:  /* 7B   | (0A)00018F       */ ldfld      class MvcContrib.FluentHtml.Elements.Option/*02000048*/ class MvcContrib.FluentHtml.Elements.SelectBase`1/*02000045*/<!T>/*1B000023*/::_firstOption /* 0A00018F */
    IL_000e:  /* 2C   | 36               */ brfalse.s  IL_0046

    IL_0010:  /* 02   |                  */ ldarg.0
    IL_0011:  /* 7B   | (0A)000192       */ ldfld      bool class MvcContrib.FluentHtml.Elements.SelectBase`1/*02000045*/<!T>/*1B000023*/::_hideFirstOption /* 0A000192 */
    IL_0016:  /* 2D   | 2E               */ brtrue.s   IL_0046

    IL_0018:  /* 02   |                  */ ldarg.0
    IL_0019:  /* 6F   | (0A)000194       */ callvirt   instance class MvcContrib.FluentHtml.Elements.Option/*02000048*/ class MvcContrib.FluentHtml.Elements.SelectBase`1/*02000045*/<!T>/*1B000023*/::GetFirstOption() /* 0A000194 */
    IL_001e:  /* 0C   |                  */ stloc.2
    IL_001f:  /* 02   |                  */ ldarg.0
    IL_0020:  /* 7B   | (0A)000193       */ ldfld      class [System.Core/*23000002*/]System.Action`3/*01000019*/<class MvcContrib.FluentHtml.Elements.Option/*02000048*/,object,int32> class MvcContrib.FluentHtml.Elements.SelectBase`1/*02000045*/<!T>/*1B000023*/::_optionModifier /* 0A000193 */
    IL_0025:  /* 2C   | 13               */ brfalse.s  IL_003a

    IL_0027:  /* 02   |                  */ ldarg.0
    IL_0028:  /* 7B   | (0A)000193       */ ldfld      class [System.Core/*23000002*/]System.Action`3/*01000019*/<class MvcContrib.FluentHtml.Elements.Option/*02000048*/,object,int32> class MvcContrib.FluentHtml.Elements.SelectBase`1/*02000045*/<!T>/*1B000023*/::_optionModifier /* 0A000193 */
    IL_002d:  /* 08   |                  */ ldloc.2
    IL_002e:  /* 02   |                  */ ldarg.0
    IL_002f:  /* 7B   | (0A)00018F       */ ldfld      class MvcContrib.FluentHtml.Elements.Option/*02000048*/ class MvcContrib.FluentHtml.Elements.SelectBase`1/*02000045*/<!T>/*1B000023*/::_firstOption /* 0A00018F */
    IL_0034:  /* 07   |                  */ ldloc.1
    IL_0035:  /* 6F   | (0A)000195       */ callvirt   instance void class [System.Core/*23000002*/]System.Action`3/*01000019*/<class MvcContrib.FluentHtml.Elements.Option/*02000048*/,object,int32>/*1B00006C*/::Invoke(!0,
                                                                                                                                                                                                                          !1,
                                                                                                                                                                                                                          !2) /* 0A000195 */
    IL_003a:  /* 06   |                  */ ldloc.0
    IL_003b:  /* 08   |                  */ ldloc.2
    IL_003c:  /* 6F   | (0A)00013F       */ callvirt   instance class [mscorlib/*23000001*/]System.Text.StringBuilder/*01000055*/ [mscorlib/*23000001*/]System.Text.StringBuilder/*01000055*/::Append(object) /* 0A00013F */
    IL_0041:  /* 26   |                  */ pop
    IL_0042:  /* 07   |                  */ ldloc.1
    IL_0043:  /* 17   |                  */ ldc.i4.1
    IL_0044:  /* 58   |                  */ add
    IL_0045:  /* 0B   |                  */ stloc.1
    IL_0046:  /* 02   |                  */ ldarg.0
    IL_0047:  /* 7B   | (0A)0000BC       */ ldfld      class [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000015*/ class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/::_options /* 0A0000BC */
    IL_004c:  /* 6F   | (0A)0000EB       */ callvirt   instance class [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000034*/ [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000015*/::GetEnumerator() /* 0A0000EB */
    IL_0051:  /* 13   | 05               */ stloc.s    V_5
    .try
    {
      IL_0053:  /* 2B   | 35               */ br.s       IL_008a

      IL_0055:  /* 11   | 05               */ ldloc.s    V_5
      IL_0057:  /* 6F   | (0A)0000E5       */ callvirt   instance object [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000034*/::get_Current() /* 0A0000E5 */
      IL_005c:  /* 0D   |                  */ stloc.3
      IL_005d:  /* 02   |                  */ ldarg.0
      IL_005e:  /* 09   |                  */ ldloc.3
      IL_005f:  /* 6F   | (0A)000196       */ callvirt   instance class MvcContrib.FluentHtml.Elements.Option/*02000048*/ class MvcContrib.FluentHtml.Elements.SelectBase`1/*02000045*/<!T>/*1B000023*/::GetOption(object) /* 0A000196 */
      IL_0064:  /* 13   | 04               */ stloc.s    V_4
      IL_0066:  /* 02   |                  */ ldarg.0
      IL_0067:  /* 7B   | (0A)000193       */ ldfld      class [System.Core/*23000002*/]System.Action`3/*01000019*/<class MvcContrib.FluentHtml.Elements.Option/*02000048*/,object,int32> class MvcContrib.FluentHtml.Elements.SelectBase`1/*02000045*/<!T>/*1B000023*/::_optionModifier /* 0A000193 */
      IL_006c:  /* 2C   | 0F               */ brfalse.s  IL_007d

      IL_006e:  /* 02   |                  */ ldarg.0
      IL_006f:  /* 7B   | (0A)000193       */ ldfld      class [System.Core/*23000002*/]System.Action`3/*01000019*/<class MvcContrib.FluentHtml.Elements.Option/*02000048*/,object,int32> class MvcContrib.FluentHtml.Elements.SelectBase`1/*02000045*/<!T>/*1B000023*/::_optionModifier /* 0A000193 */
      IL_0074:  /* 11   | 04               */ ldloc.s    V_4
      IL_0076:  /* 09   |                  */ ldloc.3
      IL_0077:  /* 07   |                  */ ldloc.1
      IL_0078:  /* 6F   | (0A)000195       */ callvirt   instance void class [System.Core/*23000002*/]System.Action`3/*01000019*/<class MvcContrib.FluentHtml.Elements.Option/*02000048*/,object,int32>/*1B00006C*/::Invoke(!0,
                                                                                                                                                                                                                            !1,
                                                                                                                                                                                                                            !2) /* 0A000195 */
      IL_007d:  /* 06   |                  */ ldloc.0
      IL_007e:  /* 11   | 04               */ ldloc.s    V_4
      IL_0080:  /* 6F   | (0A)00013F       */ callvirt   instance class [mscorlib/*23000001*/]System.Text.StringBuilder/*01000055*/ [mscorlib/*23000001*/]System.Text.StringBuilder/*01000055*/::Append(object) /* 0A00013F */
      IL_0085:  /* 26   |                  */ pop
      IL_0086:  /* 07   |                  */ ldloc.1
      IL_0087:  /* 17   |                  */ ldc.i4.1
      IL_0088:  /* 58   |                  */ add
      IL_0089:  /* 0B   |                  */ stloc.1
      IL_008a:  /* 11   | 05               */ ldloc.s    V_5
      IL_008c:  /* 6F   | (0A)00001A       */ callvirt   instance bool [mscorlib/*23000001*/]System.Collections.IEnumerator/*01000034*/::MoveNext() /* 0A00001A */
      IL_0091:  /* 2D   | C2               */ brtrue.s   IL_0055

      IL_0093:  /* DE   | 15               */ leave.s    IL_00aa

    }  // end .try
    finally
    {
      IL_0095:  /* 11   | 05               */ ldloc.s    V_5
      IL_0097:  /* 75   | (01)000035       */ isinst     [mscorlib/*23000001*/]System.IDisposable/*01000035*/
      IL_009c:  /* 13   | 06               */ stloc.s    V_6
      IL_009e:  /* 11   | 06               */ ldloc.s    V_6
      IL_00a0:  /* 2C   | 07               */ brfalse.s  IL_00a9

      IL_00a2:  /* 11   | 06               */ ldloc.s    V_6
      IL_00a4:  /* 6F   | (0A)00001B       */ callvirt   instance void [mscorlib/*23000001*/]System.IDisposable/*01000035*/::Dispose() /* 0A00001B */
      IL_00a9:  /* DC   |                  */ endfinally
    }  // end handler
    // HEX: 02 00 53 00 42 95 00 15 00 00 00 00
    IL_00aa:  /* 06   |                  */ ldloc.0
    IL_00ab:  /* 6F   | (0A)00006E       */ callvirt   instance string [mscorlib/*23000001*/]System.Object/*01000001*/::ToString() /* 0A00006E */
    IL_00b0:  /* 2A   |                  */ ret
  } // end of method SelectBase`1::RenderOptions

  .method /*06000139*/ family hidebysig newslot virtual 
          instance class MvcContrib.FluentHtml.Elements.Option/*02000048*/ 
          GetFirstOption() cil managed
  // SIG: 20 00 12 81 20
  {
    // Method begins at RVA 0x4d60
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (0A)00018F       */ ldfld      class MvcContrib.FluentHtml.Elements.Option/*02000048*/ class MvcContrib.FluentHtml.Elements.SelectBase`1/*02000045*/<!T>/*1B000023*/::_firstOption /* 0A00018F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SelectBase`1::GetFirstOption

  .method /*0600013A*/ family hidebysig newslot virtual 
          instance class MvcContrib.FluentHtml.Elements.Option/*02000048*/ 
          GetOption(object option) cil managed
  // SIG: 20 01 12 81 20 1C
  {
    // Method begins at RVA 0x4d68
    // Code size       86 (0x56)
    .maxstack  3
    .locals /*1100002A*/ init (object V_0,
             object V_1)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (0A)0000E0       */ ldfld      class [System.Core/*23000002*/]System.Func`2/*0100000B*/<object,object> class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/::_valueFieldSelector /* 0A0000E0 */
    IL_0006:  /* 03   |                  */ ldarg.1
    IL_0007:  /* 6F   | (0A)0000F7       */ callvirt   instance !1 class [System.Core/*23000002*/]System.Func`2/*0100000B*/<object,object>/*1B000057*/::Invoke(!0) /* 0A0000F7 */
    IL_000c:  /* 0A   |                  */ stloc.0
    IL_000d:  /* 02   |                  */ ldarg.0
    IL_000e:  /* 7B   | (0A)0000DE       */ ldfld      class [System.Core/*23000002*/]System.Func`2/*0100000B*/<object,object> class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/::_textFieldSelector /* 0A0000DE */
    IL_0013:  /* 03   |                  */ ldarg.1
    IL_0014:  /* 6F   | (0A)0000F7       */ callvirt   instance !1 class [System.Core/*23000002*/]System.Func`2/*0100000B*/<object,object>/*1B000057*/::Invoke(!0) /* 0A0000F7 */
    IL_0019:  /* 0B   |                  */ stloc.1
    IL_001a:  /* 73   | (06)000141       */ newobj     instance void MvcContrib.FluentHtml.Elements.Option/*02000048*/::.ctor() /* 06000141 */
    IL_001f:  /* 06   |                  */ ldloc.0
    IL_0020:  /* 2C   | 08               */ brfalse.s  IL_002a

    IL_0022:  /* 06   |                  */ ldloc.0
    IL_0023:  /* 6F   | (0A)00006E       */ callvirt   instance string [mscorlib/*23000001*/]System.Object/*01000001*/::ToString() /* 0A00006E */
    IL_0028:  /* 2B   | 05               */ br.s       IL_002f

    IL_002a:  /* 7E   | (0A)0000EF       */ ldsfld     string [mscorlib/*23000001*/]System.String/*0100003F*/::Empty /* 0A0000EF */
    IL_002f:  /* 28   | (06)000142       */ call       instance class MvcContrib.FluentHtml.Elements.Option/*02000048*/ MvcContrib.FluentHtml.Elements.Option/*02000048*/::Value(string) /* 06000142 */
    IL_0034:  /* 07   |                  */ ldloc.1
    IL_0035:  /* 2C   | 08               */ brfalse.s  IL_003f

    IL_0037:  /* 07   |                  */ ldloc.1
    IL_0038:  /* 6F   | (0A)00006E       */ callvirt   instance string [mscorlib/*23000001*/]System.Object/*01000001*/::ToString() /* 0A00006E */
    IL_003d:  /* 2B   | 05               */ br.s       IL_0044

    IL_003f:  /* 7E   | (0A)0000EF       */ ldsfld     string [mscorlib/*23000001*/]System.String/*0100003F*/::Empty /* 0A0000EF */
    IL_0044:  /* 6F   | (06)000143       */ callvirt   instance class MvcContrib.FluentHtml.Elements.Option/*02000048*/ MvcContrib.FluentHtml.Elements.Option/*02000048*/::Text(string) /* 06000143 */
    IL_0049:  /* 02   |                  */ ldarg.0
    IL_004a:  /* 06   |                  */ ldloc.0
    IL_004b:  /* 28   | (0A)0000FC       */ call       instance bool class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<!T>/*1B00000D*/::IsSelectedValue(object) /* 0A0000FC */
    IL_0050:  /* 6F   | (06)000144       */ callvirt   instance class MvcContrib.FluentHtml.Elements.Option/*02000048*/ MvcContrib.FluentHtml.Elements.Option/*02000048*/::Selected(bool) /* 06000144 */
    IL_0055:  /* 2A   |                  */ ret
  } // end of method SelectBase`1::GetOption

  .property /*1700001E*/ instance valuetype [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagRenderMode/*01000013*/
          TagRenderMode()
  {
    .get instance valuetype [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagRenderMode/*01000013*/ MvcContrib.FluentHtml.Elements.SelectBase`1/*02000045*/::get_TagRenderMode() /* 06000137 */
  } // end of property SelectBase`1::TagRenderMode
} // end of class MvcContrib.FluentHtml.Elements.SelectBase`1

.class /*02000046*/ public auto ansi beforefieldinit MvcContrib.FluentHtml.Elements.MultiSelect
       extends class MvcContrib.FluentHtml.Elements.SelectBase`1/*02000045*/<class MvcContrib.FluentHtml.Elements.MultiSelect/*02000046*/>/*1B000024*/
{
  .method /*0600013B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x4dca
    // Code size       30 (0x1e)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 28   | (0A)000197       */ call       instance void class MvcContrib.FluentHtml.Elements.SelectBase`1/*02000045*/<class MvcContrib.FluentHtml.Elements.MultiSelect/*02000046*/>/*1B000024*/::.ctor(string) /* 0A000197 */
    IL_0007:  /* 02   |                  */ ldarg.0
    IL_0008:  /* 7B   | (0A)000198       */ ldfld      class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<class MvcContrib.FluentHtml.Elements.MultiSelect/*02000046*/>/*1B00006D*/::builder /* 0A000198 */
    IL_000d:  /* 72   | (70)00097D       */ ldstr      "multiple" /* 7000097D */
    IL_0012:  /* 72   | (70)00097D       */ ldstr      "multiple" /* 7000097D */
    IL_0017:  /* 17   |                  */ ldc.i4.1
    IL_0018:  /* 6F   | (0A)000066       */ callvirt   instance void [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/::MergeAttribute(string,
                                                                                                                                                       string,
                                                                                                                                                       bool) /* 0A000066 */
    IL_001d:  /* 2A   |                  */ ret
  } // end of method MultiSelect::.ctor

  .method /*0600013C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name,
                               class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ forMember,
                               class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> behaviors) cil managed
  // SIG: 20 03 01 0E 12 39 15 12 11 01 12 08
  {
    // Method begins at RVA 0x4de9
    // Code size       32 (0x20)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 04   |                  */ ldarg.2
    IL_0003:  /* 05   |                  */ ldarg.3
    IL_0004:  /* 28   | (0A)000199       */ call       instance void class MvcContrib.FluentHtml.Elements.SelectBase`1/*02000045*/<class MvcContrib.FluentHtml.Elements.MultiSelect/*02000046*/>/*1B000024*/::.ctor(string,
                                                                                                                                                                                                                    class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                                                                                                                    class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 0A000199 */
    IL_0009:  /* 02   |                  */ ldarg.0
    IL_000a:  /* 7B   | (0A)000198       */ ldfld      class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<class MvcContrib.FluentHtml.Elements.MultiSelect/*02000046*/>/*1B00006D*/::builder /* 0A000198 */
    IL_000f:  /* 72   | (70)00097D       */ ldstr      "multiple" /* 7000097D */
    IL_0014:  /* 72   | (70)00097D       */ ldstr      "multiple" /* 7000097D */
    IL_0019:  /* 17   |                  */ ldc.i4.1
    IL_001a:  /* 6F   | (0A)000066       */ callvirt   instance void [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/::MergeAttribute(string,
                                                                                                                                                       string,
                                                                                                                                                       bool) /* 0A000066 */
    IL_001f:  /* 2A   |                  */ ret
  } // end of method MultiSelect::.ctor

  .method /*0600013D*/ public hidebysig newslot virtual 
          instance class MvcContrib.FluentHtml.Elements.MultiSelect/*02000046*/ 
          Selected(class [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000015*/ selectedValues) cil managed
  // SIG: 20 01 12 81 18 12 55
  {
    // Method begins at RVA 0x4e0a
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (0A)00019A       */ stfld      class [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000015*/ class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<class MvcContrib.FluentHtml.Elements.MultiSelect/*02000046*/>/*1B00006E*/::_selectedValues /* 0A00019A */
    IL_0007:  /* 02   |                  */ ldarg.0
    IL_0008:  /* 2A   |                  */ ret
  } // end of method MultiSelect::Selected

} // end of class MvcContrib.FluentHtml.Elements.MultiSelect

.class /*02000047*/ public auto ansi beforefieldinit MvcContrib.FluentHtml.Elements.MultiSelect`1<TModel>
       extends class MvcContrib.FluentHtml.Elements.SelectBase`1/*02000045*/<class MvcContrib.FluentHtml.Elements.MultiSelect`1/*02000047*/<!TModel>>/*1B000025*/
{
  .method /*0600013E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x4e14
    // Code size       30 (0x1e)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 28   | (0A)00019B       */ call       instance void class MvcContrib.FluentHtml.Elements.SelectBase`1/*02000045*/<class MvcContrib.FluentHtml.Elements.MultiSelect`1/*02000047*/<!TModel>>/*1B000025*/::.ctor(string) /* 0A00019B */
    IL_0007:  /* 02   |                  */ ldarg.0
    IL_0008:  /* 7B   | (0A)00019C       */ ldfld      class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<class MvcContrib.FluentHtml.Elements.MultiSelect`1/*02000047*/<!TModel>>/*1B00006F*/::builder /* 0A00019C */
    IL_000d:  /* 72   | (70)00097D       */ ldstr      "multiple" /* 7000097D */
    IL_0012:  /* 72   | (70)00097D       */ ldstr      "multiple" /* 7000097D */
    IL_0017:  /* 17   |                  */ ldc.i4.1
    IL_0018:  /* 6F   | (0A)000066       */ callvirt   instance void [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/::MergeAttribute(string,
                                                                                                                                                       string,
                                                                                                                                                       bool) /* 0A000066 */
    IL_001d:  /* 2A   |                  */ ret
  } // end of method MultiSelect`1::.ctor

  .method /*0600013F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name,
                               class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ forMember,
                               class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> behaviors) cil managed
  // SIG: 20 03 01 0E 12 39 15 12 11 01 12 08
  {
    // Method begins at RVA 0x4e33
    // Code size       32 (0x20)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 04   |                  */ ldarg.2
    IL_0003:  /* 05   |                  */ ldarg.3
    IL_0004:  /* 28   | (0A)00019D       */ call       instance void class MvcContrib.FluentHtml.Elements.SelectBase`1/*02000045*/<class MvcContrib.FluentHtml.Elements.MultiSelect`1/*02000047*/<!TModel>>/*1B000025*/::.ctor(string,
                                                                                                                                                                                                                               class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                                                                                                                               class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 0A00019D */
    IL_0009:  /* 02   |                  */ ldarg.0
    IL_000a:  /* 7B   | (0A)00019C       */ ldfld      class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<class MvcContrib.FluentHtml.Elements.MultiSelect`1/*02000047*/<!TModel>>/*1B00006F*/::builder /* 0A00019C */
    IL_000f:  /* 72   | (70)00097D       */ ldstr      "multiple" /* 7000097D */
    IL_0014:  /* 72   | (70)00097D       */ ldstr      "multiple" /* 7000097D */
    IL_0019:  /* 17   |                  */ ldc.i4.1
    IL_001a:  /* 6F   | (0A)000066       */ callvirt   instance void [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/::MergeAttribute(string,
                                                                                                                                                       string,
                                                                                                                                                       bool) /* 0A000066 */
    IL_001f:  /* 2A   |                  */ ret
  } // end of method MultiSelect`1::.ctor

  .method /*06000140*/ public hidebysig newslot virtual 
          instance class MvcContrib.FluentHtml.Elements.MultiSelect`1/*02000047*/<!TModel> 
          Selected(class [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000015*/ selectedValues) cil managed
  // SIG: 20 01 15 12 81 1C 01 13 00 12 55
  {
    // Method begins at RVA 0x4e54
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (0A)00019E       */ stfld      class [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000015*/ class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<class MvcContrib.FluentHtml.Elements.MultiSelect`1/*02000047*/<!TModel>>/*1B000070*/::_selectedValues /* 0A00019E */
    IL_0007:  /* 02   |                  */ ldarg.0
    IL_0008:  /* 2A   |                  */ ret
  } // end of method MultiSelect`1::Selected

} // end of class MvcContrib.FluentHtml.Elements.MultiSelect`1

.class /*02000048*/ public auto ansi beforefieldinit MvcContrib.FluentHtml.Elements.Option
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .field /*0400005C*/ family initonly class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ builder
  .method /*06000141*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4e5e
    // Code size       23 (0x17)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000012       */ call       instance void [mscorlib/*23000001*/]System.Object/*01000001*/::.ctor() /* 0A000012 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 72   | (70)00098F       */ ldstr      "option" /* 7000098F */
    IL_000c:  /* 73   | (0A)000064       */ newobj     instance void [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/::.ctor(string) /* 0A000064 */
    IL_0011:  /* 7D   | (04)00005C       */ stfld      class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ MvcContrib.FluentHtml.Elements.Option/*02000048*/::builder /* 0400005C */
    IL_0016:  /* 2A   |                  */ ret
  } // end of method Option::.ctor

  .method /*06000142*/ public hidebysig instance class MvcContrib.FluentHtml.Elements.Option/*02000048*/ 
          Value(string 'value') cil managed
  // SIG: 20 01 12 81 20 0E
  {
    // Method begins at RVA 0x4e76
    // Code size       20 (0x14)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (04)00005C       */ ldfld      class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ MvcContrib.FluentHtml.Elements.Option/*02000048*/::builder /* 0400005C */
    IL_0006:  /* 72   | (70)00014F       */ ldstr      "value" /* 7000014F */
    IL_000b:  /* 03   |                  */ ldarg.1
    IL_000c:  /* 17   |                  */ ldc.i4.1
    IL_000d:  /* 6F   | (0A)000066       */ callvirt   instance void [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/::MergeAttribute(string,
                                                                                                                                                       string,
                                                                                                                                                       bool) /* 0A000066 */
    IL_0012:  /* 02   |                  */ ldarg.0
    IL_0013:  /* 2A   |                  */ ret
  } // end of method Option::Value

  .method /*06000143*/ public hidebysig newslot virtual 
          instance class MvcContrib.FluentHtml.Elements.Option/*02000048*/ 
          Text(string 'value') cil managed
  // SIG: 20 01 12 81 20 0E
  {
    // Method begins at RVA 0x4e8b
    // Code size       14 (0xe)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (04)00005C       */ ldfld      class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ MvcContrib.FluentHtml.Elements.Option/*02000048*/::builder /* 0400005C */
    IL_0006:  /* 03   |                  */ ldarg.1
    IL_0007:  /* 6F   | (0A)000093       */ callvirt   instance void [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/::SetInnerText(string) /* 0A000093 */
    IL_000c:  /* 02   |                  */ ldarg.0
    IL_000d:  /* 2A   |                  */ ret
  } // end of method Option::Text

  .method /*06000144*/ public hidebysig newslot virtual 
          instance class MvcContrib.FluentHtml.Elements.Option/*02000048*/ 
          Selected(bool 'value') cil managed
  // SIG: 20 01 12 81 20 02
  {
    // Method begins at RVA 0x4e9a
    // Code size       51 (0x33)
    .maxstack  8
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 2C   | 18               */ brfalse.s  IL_001b

    IL_0003:  /* 02   |                  */ ldarg.0
    IL_0004:  /* 7B   | (04)00005C       */ ldfld      class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ MvcContrib.FluentHtml.Elements.Option/*02000048*/::builder /* 0400005C */
    IL_0009:  /* 72   | (70)00099D       */ ldstr      "selected" /* 7000099D */
    IL_000e:  /* 72   | (70)00099D       */ ldstr      "selected" /* 7000099D */
    IL_0013:  /* 17   |                  */ ldc.i4.1
    IL_0014:  /* 6F   | (0A)000066       */ callvirt   instance void [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/::MergeAttribute(string,
                                                                                                                                                       string,
                                                                                                                                                       bool) /* 0A000066 */
    IL_0019:  /* 2B   | 16               */ br.s       IL_0031

    IL_001b:  /* 02   |                  */ ldarg.0
    IL_001c:  /* 7B   | (04)00005C       */ ldfld      class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ MvcContrib.FluentHtml.Elements.Option/*02000048*/::builder /* 0400005C */
    IL_0021:  /* 6F   | (0A)000076       */ callvirt   instance class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<string,string> [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/::get_Attributes() /* 0A000076 */
    IL_0026:  /* 72   | (70)00099D       */ ldstr      "selected" /* 7000099D */
    IL_002b:  /* 6F   | (0A)000077       */ callvirt   instance bool class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<string,string>/*1B000047*/::Remove(!0) /* 0A000077 */
    IL_0030:  /* 26   |                  */ pop
    IL_0031:  /* 02   |                  */ ldarg.0
    IL_0032:  /* 2A   |                  */ ret
  } // end of method Option::Selected

  .method /*06000145*/ public hidebysig newslot virtual 
          instance class MvcContrib.FluentHtml.Elements.Option/*02000048*/ 
          Disabled(bool 'value') cil managed
  // SIG: 20 01 12 81 20 02
  {
    // Method begins at RVA 0x4ece
    // Code size       51 (0x33)
    .maxstack  8
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 2C   | 18               */ brfalse.s  IL_001b

    IL_0003:  /* 02   |                  */ ldarg.0
    IL_0004:  /* 7B   | (04)00005C       */ ldfld      class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ MvcContrib.FluentHtml.Elements.Option/*02000048*/::builder /* 0400005C */
    IL_0009:  /* 72   | (70)000133       */ ldstr      "disabled" /* 70000133 */
    IL_000e:  /* 72   | (70)000133       */ ldstr      "disabled" /* 70000133 */
    IL_0013:  /* 17   |                  */ ldc.i4.1
    IL_0014:  /* 6F   | (0A)000066       */ callvirt   instance void [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/::MergeAttribute(string,
                                                                                                                                                       string,
                                                                                                                                                       bool) /* 0A000066 */
    IL_0019:  /* 2B   | 16               */ br.s       IL_0031

    IL_001b:  /* 02   |                  */ ldarg.0
    IL_001c:  /* 7B   | (04)00005C       */ ldfld      class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ MvcContrib.FluentHtml.Elements.Option/*02000048*/::builder /* 0400005C */
    IL_0021:  /* 6F   | (0A)000076       */ callvirt   instance class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<string,string> [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/::get_Attributes() /* 0A000076 */
    IL_0026:  /* 72   | (70)000133       */ ldstr      "disabled" /* 70000133 */
    IL_002b:  /* 6F   | (0A)000077       */ callvirt   instance bool class [mscorlib/*23000001*/]System.Collections.Generic.IDictionary`2/*01000014*/<string,string>/*1B000047*/::Remove(!0) /* 0A000077 */
    IL_0030:  /* 26   |                  */ pop
    IL_0031:  /* 02   |                  */ ldarg.0
    IL_0032:  /* 2A   |                  */ ret
  } // end of method Option::Disabled

  .method /*06000146*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x4f02
    // Code size       12 (0xc)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (04)00005C       */ ldfld      class [System.Web.Mvc/*23000003*/]System.Web.Mvc.TagBuilder/*01000012*/ MvcContrib.FluentHtml.Elements.Option/*02000048*/::builder /* 0400005C */
    IL_0006:  /* 6F   | (0A)00006E       */ callvirt   instance string [mscorlib/*23000001*/]System.Object/*01000001*/::ToString() /* 0A00006E */
    IL_000b:  /* 2A   |                  */ ret
  } // end of method Option::ToString

} // end of class MvcContrib.FluentHtml.Elements.Option

.class /*02000049*/ public auto ansi beforefieldinit MvcContrib.FluentHtml.Elements.Password
       extends class MvcContrib.FluentHtml.Elements.TextInput`1/*0200002C*/<class MvcContrib.FluentHtml.Elements.Password/*02000049*/>/*1B000026*/
{
  .method /*06000147*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x4f0f
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 72   | (70)0009AF       */ ldstr      "password" /* 700009AF */
    IL_0006:  /* 03   |                  */ ldarg.1
    IL_0007:  /* 28   | (0A)00019F       */ call       instance void class MvcContrib.FluentHtml.Elements.TextInput`1/*0200002C*/<class MvcContrib.FluentHtml.Elements.Password/*02000049*/>/*1B000026*/::.ctor(string,
                                                                                                                                                                                                                string) /* 0A00019F */
    IL_000c:  /* 2A   |                  */ ret
  } // end of method Password::.ctor

  .method /*06000148*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name,
                               class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ forMember,
                               class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> behaviors) cil managed
  // SIG: 20 03 01 0E 12 39 15 12 11 01 12 08
  {
    // Method begins at RVA 0x4f1d
    // Code size       15 (0xf)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 72   | (70)0009AF       */ ldstr      "password" /* 700009AF */
    IL_0006:  /* 03   |                  */ ldarg.1
    IL_0007:  /* 04   |                  */ ldarg.2
    IL_0008:  /* 05   |                  */ ldarg.3
    IL_0009:  /* 28   | (0A)0001A0       */ call       instance void class MvcContrib.FluentHtml.Elements.TextInput`1/*0200002C*/<class MvcContrib.FluentHtml.Elements.Password/*02000049*/>/*1B000026*/::.ctor(string,
                                                                                                                                                                                                                string,
                                                                                                                                                                                                                class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                                                                                                                class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 0A0001A0 */
    IL_000e:  /* 2A   |                  */ ret
  } // end of method Password::.ctor

  .method /*06000149*/ family hidebysig virtual 
          instance void  ApplyModelState(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelState/*0100000F*/ state) cil managed
  // SIG: 20 01 01 12 3D
  {
    // Method begins at RVA 0x4f2d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Password::ApplyModelState

} // end of class MvcContrib.FluentHtml.Elements.Password

.class /*0200004A*/ public auto ansi beforefieldinit MvcContrib.FluentHtml.Elements.Select
       extends class MvcContrib.FluentHtml.Elements.SelectBase`1/*02000045*/<class MvcContrib.FluentHtml.Elements.Select/*0200004A*/>/*1B000027*/
{
  .method /*0600014A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x4f2f
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 28   | (0A)0001A1       */ call       instance void class MvcContrib.FluentHtml.Elements.SelectBase`1/*02000045*/<class MvcContrib.FluentHtml.Elements.Select/*0200004A*/>/*1B000027*/::.ctor(string) /* 0A0001A1 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method Select::.ctor

  .method /*0600014B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name,
                               class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ forMember,
                               class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> behaviors) cil managed
  // SIG: 20 03 01 0E 12 39 15 12 11 01 12 08
  {
    // Method begins at RVA 0x4f38
    // Code size       10 (0xa)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 04   |                  */ ldarg.2
    IL_0003:  /* 05   |                  */ ldarg.3
    IL_0004:  /* 28   | (0A)0001A2       */ call       instance void class MvcContrib.FluentHtml.Elements.SelectBase`1/*02000045*/<class MvcContrib.FluentHtml.Elements.Select/*0200004A*/>/*1B000027*/::.ctor(string,
                                                                                                                                                                                                               class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                                                                                                               class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 0A0001A2 */
    IL_0009:  /* 2A   |                  */ ret
  } // end of method Select::.ctor

  .method /*0600014C*/ public hidebysig newslot virtual 
          instance class MvcContrib.FluentHtml.Elements.Select/*0200004A*/ 
          Selected(object selectedValue) cil managed
  // SIG: 20 01 12 81 28 1C
  {
    // Method begins at RVA 0x4f44
    // Code size       22 (0x16)
    .maxstack  3
    .locals /*11000020*/ init (class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000021*/<object> V_0)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 73   | (0A)000134       */ newobj     instance void class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000021*/<object>/*1B00005E*/::.ctor() /* 0A000134 */
    IL_0006:  /* 0A   |                  */ stloc.0
    IL_0007:  /* 06   |                  */ ldloc.0
    IL_0008:  /* 03   |                  */ ldarg.1
    IL_0009:  /* 6F   | (0A)000135       */ callvirt   instance void class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000021*/<object>/*1B00005E*/::Add(!0) /* 0A000135 */
    IL_000e:  /* 06   |                  */ ldloc.0
    IL_000f:  /* 7D   | (0A)0001A3       */ stfld      class [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000015*/ class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<class MvcContrib.FluentHtml.Elements.Select/*0200004A*/>/*1B000071*/::_selectedValues /* 0A0001A3 */
    IL_0014:  /* 02   |                  */ ldarg.0
    IL_0015:  /* 2A   |                  */ ret
  } // end of method Select::Selected

} // end of class MvcContrib.FluentHtml.Elements.Select

.class /*0200004B*/ public auto ansi beforefieldinit MvcContrib.FluentHtml.Elements.Select`1<TModel>
       extends class MvcContrib.FluentHtml.Elements.SelectBase`1/*02000045*/<class MvcContrib.FluentHtml.Elements.Select`1/*0200004B*/<!TModel>>/*1B000028*/
{
  .method /*0600014D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x4f66
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 28   | (0A)0001A4       */ call       instance void class MvcContrib.FluentHtml.Elements.SelectBase`1/*02000045*/<class MvcContrib.FluentHtml.Elements.Select`1/*0200004B*/<!TModel>>/*1B000028*/::.ctor(string) /* 0A0001A4 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method Select`1::.ctor

  .method /*0600014E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name,
                               class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ forMember,
                               class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> behaviors) cil managed
  // SIG: 20 03 01 0E 12 39 15 12 11 01 12 08
  {
    // Method begins at RVA 0x4f6f
    // Code size       10 (0xa)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 04   |                  */ ldarg.2
    IL_0003:  /* 05   |                  */ ldarg.3
    IL_0004:  /* 28   | (0A)0001A5       */ call       instance void class MvcContrib.FluentHtml.Elements.SelectBase`1/*02000045*/<class MvcContrib.FluentHtml.Elements.Select`1/*0200004B*/<!TModel>>/*1B000028*/::.ctor(string,
                                                                                                                                                                                                                          class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                                                                                                                          class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 0A0001A5 */
    IL_0009:  /* 2A   |                  */ ret
  } // end of method Select`1::.ctor

  .method /*0600014F*/ public hidebysig newslot virtual 
          instance class MvcContrib.FluentHtml.Elements.Select`1/*0200004B*/<!TModel> 
          Selected(object selectedValue) cil managed
  // SIG: 20 01 15 12 81 2C 01 13 00 1C
  {
    // Method begins at RVA 0x4f7c
    // Code size       22 (0x16)
    .maxstack  3
    .locals /*11000020*/ init (class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000021*/<object> V_0)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 73   | (0A)000134       */ newobj     instance void class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000021*/<object>/*1B00005E*/::.ctor() /* 0A000134 */
    IL_0006:  /* 0A   |                  */ stloc.0
    IL_0007:  /* 06   |                  */ ldloc.0
    IL_0008:  /* 03   |                  */ ldarg.1
    IL_0009:  /* 6F   | (0A)000135       */ callvirt   instance void class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000021*/<object>/*1B00005E*/::Add(!0) /* 0A000135 */
    IL_000e:  /* 06   |                  */ ldloc.0
    IL_000f:  /* 7D   | (0A)0001A6       */ stfld      class [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000015*/ class MvcContrib.FluentHtml.Elements.OptionsElementBase`1/*02000025*/<class MvcContrib.FluentHtml.Elements.Select`1/*0200004B*/<!TModel>>/*1B000072*/::_selectedValues /* 0A0001A6 */
    IL_0014:  /* 02   |                  */ ldarg.0
    IL_0015:  /* 2A   |                  */ ret
  } // end of method Select`1::Selected

} // end of class MvcContrib.FluentHtml.Elements.Select`1

.class /*0200004C*/ public auto ansi beforefieldinit MvcContrib.FluentHtml.Elements.SubmitButton
       extends class MvcContrib.FluentHtml.Elements.SubmitButtonBase`1/*0200003C*/<class MvcContrib.FluentHtml.Elements.SubmitButton/*0200004C*/>/*1B000029*/
{
  .method /*06000150*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string text) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x4f9e
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 28   | (0A)0001A7       */ call       instance void class MvcContrib.FluentHtml.Elements.SubmitButtonBase`1/*0200003C*/<class MvcContrib.FluentHtml.Elements.SubmitButton/*0200004C*/>/*1B000029*/::.ctor(string) /* 0A0001A7 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method SubmitButton::.ctor

  .method /*06000151*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string text,
                               class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> behaviors) cil managed
  // SIG: 20 02 01 0E 15 12 11 01 12 08
  {
    // Method begins at RVA 0x4fa7
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 04   |                  */ ldarg.2
    IL_0003:  /* 28   | (0A)0001A8       */ call       instance void class MvcContrib.FluentHtml.Elements.SubmitButtonBase`1/*0200003C*/<class MvcContrib.FluentHtml.Elements.SubmitButton/*0200004C*/>/*1B000029*/::.ctor(string,
                                                                                                                                                                                                                           class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 0A0001A8 */
    IL_0008:  /* 2A   |                  */ ret
  } // end of method SubmitButton::.ctor

} // end of class MvcContrib.FluentHtml.Elements.SubmitButton

.class /*0200004D*/ public auto ansi beforefieldinit MvcContrib.FluentHtml.Elements.TextArea
       extends class MvcContrib.FluentHtml.Elements.TextAreaBase`1/*0200003D*/<class MvcContrib.FluentHtml.Elements.TextArea/*0200004D*/>/*1B00002A*/
{
  .method /*06000152*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x4fb1
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 28   | (0A)0001A9       */ call       instance void class MvcContrib.FluentHtml.Elements.TextAreaBase`1/*0200003D*/<class MvcContrib.FluentHtml.Elements.TextArea/*0200004D*/>/*1B00002A*/::.ctor(string) /* 0A0001A9 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method TextArea::.ctor

  .method /*06000153*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name,
                               class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ forMember,
                               class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> behaviors) cil managed
  // SIG: 20 03 01 0E 12 39 15 12 11 01 12 08
  {
    // Method begins at RVA 0x4fba
    // Code size       10 (0xa)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 04   |                  */ ldarg.2
    IL_0003:  /* 05   |                  */ ldarg.3
    IL_0004:  /* 28   | (0A)0001AA       */ call       instance void class MvcContrib.FluentHtml.Elements.TextAreaBase`1/*0200003D*/<class MvcContrib.FluentHtml.Elements.TextArea/*0200004D*/>/*1B00002A*/::.ctor(string,
                                                                                                                                                                                                                   class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                                                                                                                   class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 0A0001AA */
    IL_0009:  /* 2A   |                  */ ret
  } // end of method TextArea::.ctor

} // end of class MvcContrib.FluentHtml.Elements.TextArea

.class /*0200004E*/ public auto ansi beforefieldinit MvcContrib.FluentHtml.Elements.TextBox
       extends class MvcContrib.FluentHtml.Elements.TextInput`1/*0200002C*/<class MvcContrib.FluentHtml.Elements.TextBox/*0200004E*/>/*1B00002B*/
{
  .method /*06000154*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x4fc5
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 72   | (70)000035       */ ldstr      "text" /* 70000035 */
    IL_0006:  /* 03   |                  */ ldarg.1
    IL_0007:  /* 28   | (0A)0001AB       */ call       instance void class MvcContrib.FluentHtml.Elements.TextInput`1/*0200002C*/<class MvcContrib.FluentHtml.Elements.TextBox/*0200004E*/>/*1B00002B*/::.ctor(string,
                                                                                                                                                                                                               string) /* 0A0001AB */
    IL_000c:  /* 2A   |                  */ ret
  } // end of method TextBox::.ctor

  .method /*06000155*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name,
                               class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ forMember,
                               class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> behaviors) cil managed
  // SIG: 20 03 01 0E 12 39 15 12 11 01 12 08
  {
    // Method begins at RVA 0x4fd3
    // Code size       15 (0xf)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 72   | (70)000035       */ ldstr      "text" /* 70000035 */
    IL_0006:  /* 03   |                  */ ldarg.1
    IL_0007:  /* 04   |                  */ ldarg.2
    IL_0008:  /* 05   |                  */ ldarg.3
    IL_0009:  /* 28   | (0A)0001AC       */ call       instance void class MvcContrib.FluentHtml.Elements.TextInput`1/*0200002C*/<class MvcContrib.FluentHtml.Elements.TextBox/*0200004E*/>/*1B00002B*/::.ctor(string,
                                                                                                                                                                                                               string,
                                                                                                                                                                                                               class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                                                                                                               class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 0A0001AC */
    IL_000e:  /* 2A   |                  */ ret
  } // end of method TextBox::.ctor

} // end of class MvcContrib.FluentHtml.Elements.TextBox

.class /*0200004F*/ public abstract auto ansi sealed beforefieldinit MvcContrib.FluentHtml.Html.HtmlAttribute
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .field /*0400005D*/ public static literal string Id = "id"
  .field /*0400005E*/ public static literal string Disabled = "disabled"
  .field /*0400005F*/ public static literal string Checked = "checked"
  .field /*04000060*/ public static literal string Name = "name"
  .field /*04000061*/ public static literal string Type = "type"
  .field /*04000062*/ public static literal string Size = "size"
  .field /*04000063*/ public static literal string Multiple = "multiple"
  .field /*04000064*/ public static literal string Selected = "selected"
  .field /*04000065*/ public static literal string Rows = "rows"
  .field /*04000066*/ public static literal string Cols = "cols"
  .field /*04000067*/ public static literal string MaxLength = "maxlength"
  .field /*04000068*/ public static literal string Value = "value"
  .field /*04000069*/ public static literal string Title = "title"
  .field /*0400006A*/ public static literal string Style = "style"
  .field /*0400006B*/ public static literal string Class = "class"
  .field /*0400006C*/ public static literal string For = "for"
  .field /*0400006D*/ public static literal string HRef = "href"
} // end of class MvcContrib.FluentHtml.Html.HtmlAttribute

.class /*02000050*/ interface public abstract auto ansi MvcContrib.FluentHtml.IViewModelContainer`1<class T>
       implements [System.Web.Mvc/*23000003*/]System.Web.Mvc.IViewDataContainer/*01000007*/,
                  MvcContrib.FluentHtml.Behaviors.IBehaviorsContainer/*02000013*/
{
  .method /*06000156*/ public hidebysig newslot specialname abstract virtual 
          instance !T  get_ViewModel() cil managed
  // SIG: 20 00 13 00
  {
    // Method begins at RVA 0x0
  } // end of method IViewModelContainer`1::get_ViewModel

  .method /*06000157*/ public hidebysig newslot specialname abstract virtual 
          instance string  get_HtmlNamePrefix() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x0
  } // end of method IViewModelContainer`1::get_HtmlNamePrefix

  .method /*06000158*/ public hidebysig newslot specialname abstract virtual 
          instance void  set_HtmlNamePrefix(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x0
  } // end of method IViewModelContainer`1::set_HtmlNamePrefix

  .method /*06000159*/ public hidebysig newslot specialname abstract virtual 
          instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.HtmlHelper/*01000020*/ 
          get_Html() cil managed
  // SIG: 20 00 12 80 81
  {
    // Method begins at RVA 0x0
  } // end of method IViewModelContainer`1::get_Html

  .property /*1700001F*/ instance !T ViewModel()
  {
    .get instance !T MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/::get_ViewModel() /* 06000156 */
  } // end of property IViewModelContainer`1::ViewModel
  .property /*17000020*/ instance string HtmlNamePrefix()
  {
    .get instance string MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/::get_HtmlNamePrefix() /* 06000157 */
    .set instance void MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/::set_HtmlNamePrefix(string) /* 06000158 */
  } // end of property IViewModelContainer`1::HtmlNamePrefix
  .property /*17000021*/ instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.HtmlHelper/*01000020*/
          Html()
  {
    .get instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.HtmlHelper/*01000020*/ MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/::get_Html() /* 06000159 */
  } // end of property IViewModelContainer`1::Html
} // end of class MvcContrib.FluentHtml.IViewModelContainer`1

.class /*02000051*/ public abstract auto ansi sealed beforefieldinit MvcContrib.FluentHtml.Expressions.ExpressionExtensions
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C000038:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
  .method /*0600015A*/ public hidebysig static 
          string  GetNameFor<class T,TValue>(class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!T,!!TValue>> expression) cil managed
  // SIG: 10 02 01 0E 15 12 69 01 15 12 2D 02 1E 00 1E 01
  {
    .custom /*0C00005B:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4fe3
    // Code size       17 (0x11)
    .maxstack  8
    IL_0000:  /* 73   | (06)000125       */ newobj     instance void MvcContrib.FluentHtml.Expressions.ExpressionNameVisitor/*0200003F*/::.ctor() /* 06000125 */
    IL_0005:  /* 02   |                  */ ldarg.0
    IL_0006:  /* 6F   | (0A)0001AD       */ callvirt   instance class [System.Core/*23000002*/]System.Linq.Expressions.Expression/*0100001D*/ [System.Core/*23000002*/]System.Linq.Expressions.LambdaExpression/*01000070*/::get_Body() /* 0A0001AD */
    IL_000b:  /* 28   | (06)000121       */ call       instance string MvcContrib.FluentHtml.Expressions.ExpressionNameVisitor/*0200003F*/::Visit(class [System.Core/*23000002*/]System.Linq.Expressions.Expression/*0100001D*/) /* 06000121 */
    IL_0010:  /* 2A   |                  */ ret
  } // end of method ExpressionExtensions::GetNameFor

  .method /*0600015B*/ public hidebysig static 
          string  GetNameFor<class T,TValue>(class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!T,!!TValue>> expression,
                                             class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!T> view) cil managed
  // SIG: 10 02 02 0E 15 12 69 01 15 12 2D 02 1E 00 1E 01 15 12 81 40 01 1E 00
  {
    .custom /*0C00005C:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4ff8
    // Code size       67 (0x43)
    .maxstack  4
    .locals /*11000010*/ init (string V_0)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (2B)00001B       */ call       string MvcContrib.FluentHtml.Expressions.ExpressionExtensions/*02000051*/::GetNameFor<!!0,!!1>(class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,!!1>>) /* 2B00001B */
    IL_0006:  /* 0A   |                  */ stloc.0
    IL_0007:  /* 72   | (70)0009C1       */ ldstr      "{0}{1}{2}" /* 700009C1 */
    IL_000c:  /* 03   |                  */ ldarg.1
    IL_000d:  /* 6F   | (0A)0001AE       */ callvirt   instance string class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!T>/*1B000073*/::get_HtmlNamePrefix() /* 0A0001AE */
    IL_0012:  /* 03   |                  */ ldarg.1
    IL_0013:  /* 6F   | (0A)0001AE       */ callvirt   instance string class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!T>/*1B000073*/::get_HtmlNamePrefix() /* 0A0001AE */
    IL_0018:  /* 28   | (0A)000096       */ call       bool [mscorlib/*23000001*/]System.String/*0100003F*/::IsNullOrEmpty(string) /* 0A000096 */
    IL_001d:  /* 2D   | 1C               */ brtrue.s   IL_003b

    IL_001f:  /* 06   |                  */ ldloc.0
    IL_0020:  /* 28   | (0A)000096       */ call       bool [mscorlib/*23000001*/]System.String/*0100003F*/::IsNullOrEmpty(string) /* 0A000096 */
    IL_0025:  /* 2D   | 14               */ brtrue.s   IL_003b

    IL_0027:  /* 06   |                  */ ldloc.0
    IL_0028:  /* 72   | (70)0000F7       */ ldstr      "[" /* 700000F7 */
    IL_002d:  /* 6F   | (0A)00010B       */ callvirt   instance bool [mscorlib/*23000001*/]System.String/*0100003F*/::StartsWith(string) /* 0A00010B */
    IL_0032:  /* 2D   | 07               */ brtrue.s   IL_003b

    IL_0034:  /* 72   | (70)0000F3       */ ldstr      "." /* 700000F3 */
    IL_0039:  /* 2B   | 01               */ br.s       IL_003c

    IL_003b:  /* 14   |                  */ ldnull
    IL_003c:  /* 06   |                  */ ldloc.0
    IL_003d:  /* 28   | (0A)0001AF       */ call       string [mscorlib/*23000001*/]System.String/*0100003F*/::Format(string,
                                                                                                                      object,
                                                                                                                      object,
                                                                                                                      object) /* 0A0001AF */
    IL_0042:  /* 2A   |                  */ ret
  } // end of method ExpressionExtensions::GetNameFor

  .method /*0600015C*/ public hidebysig static 
          !!TValue  GetValueFrom<class T,TValue>(class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!T,!!TValue>> expression,
                                                 !!T viewModel) cil managed
  // SIG: 10 02 02 1E 01 15 12 69 01 15 12 2D 02 1E 00 1E 01 1E 00
  {
    .custom /*0C00005D:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x5048
    // Code size       49 (0x31)
    .maxstack  2
    .locals /*1100002B*/ init (!!TValue V_0,
             !!TValue V_1,
             !!TValue V_2)
    .try
    {
      IL_0000:  /* 03   |                  */ ldarg.1
      IL_0001:  /* 8C   | (1B)000031       */ box        !!T/*1B000031*/
      IL_0006:  /* 2C   | 0E               */ brfalse.s  IL_0016

      IL_0008:  /* 02   |                  */ ldarg.0
      IL_0009:  /* 6F   | (0A)0001B0       */ callvirt   instance !0 class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!T,!!TValue>>/*1B000074*/::Compile() /* 0A0001B0 */
      IL_000e:  /* 03   |                  */ ldarg.1
      IL_000f:  /* 6F   | (0A)0001B1       */ callvirt   instance !1 class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!T,!!TValue>/*1B000075*/::Invoke(!0) /* 0A0001B1 */
      IL_0014:  /* 2B   | 09               */ br.s       IL_001f

      IL_0016:  /* 12   | 01               */ ldloca.s   V_1
      IL_0018:  /* FE15 | (1B)000076       */ initobj    !!TValue/*1B000076*/
      IL_001e:  /* 07   |                  */ ldloc.1
      IL_001f:  /* 0A   |                  */ stloc.0
      IL_0020:  /* DE   | 0D               */ leave.s    IL_002f

    }  // end .try
    catch [mscorlib/*23000001*/]System.Exception/*0100001B*/ 
    {
      IL_0022:  /* 26   |                  */ pop
      IL_0023:  /* 12   | 02               */ ldloca.s   V_2
      IL_0025:  /* FE15 | (1B)000076       */ initobj    !!TValue/*1B000076*/
      IL_002b:  /* 08   |                  */ ldloc.2
      IL_002c:  /* 0A   |                  */ stloc.0
      IL_002d:  /* DE   | 00               */ leave.s    IL_002f

    }  // end handler
    // HEX: 00 00 00 00 22 22 00 0D 1B 00 00 01
    IL_002f:  /* 06   |                  */ ldloc.0
    IL_0030:  /* 2A   |                  */ ret
  } // end of method ExpressionExtensions::GetValueFrom

  .method /*0600015D*/ public hidebysig static 
          class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ 
          GetMemberExpression<class T,TValue>(class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!T,!!TValue>> expression) cil managed
  // SIG: 10 02 01 12 39 15 12 69 01 15 12 2D 02 1E 00 1E 01
  {
    .custom /*0C00005E:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x5098
    // Code size       102 (0x66)
    .maxstack  1
    .locals /*1100002C*/ init (class [System.Core/*23000002*/]System.Linq.Expressions.Expression/*0100001D*/ V_0)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 2D   | 02               */ brtrue.s   IL_0005

    IL_0003:  /* 14   |                  */ ldnull
    IL_0004:  /* 2A   |                  */ ret

    IL_0005:  /* 02   |                  */ ldarg.0
    IL_0006:  /* 6F   | (0A)0001AD       */ callvirt   instance class [System.Core/*23000002*/]System.Linq.Expressions.Expression/*0100001D*/ [System.Core/*23000002*/]System.Linq.Expressions.LambdaExpression/*01000070*/::get_Body() /* 0A0001AD */
    IL_000b:  /* 75   | (01)00000E       */ isinst     [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/
    IL_0010:  /* 2C   | 0C               */ brfalse.s  IL_001e

    IL_0012:  /* 02   |                  */ ldarg.0
    IL_0013:  /* 6F   | (0A)0001AD       */ callvirt   instance class [System.Core/*23000002*/]System.Linq.Expressions.Expression/*0100001D*/ [System.Core/*23000002*/]System.Linq.Expressions.LambdaExpression/*01000070*/::get_Body() /* 0A0001AD */
    IL_0018:  /* 74   | (01)00000E       */ castclass  [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/
    IL_001d:  /* 2A   |                  */ ret

    IL_001e:  /* 02   |                  */ ldarg.0
    IL_001f:  /* 6F   | (0A)0001AD       */ callvirt   instance class [System.Core/*23000002*/]System.Linq.Expressions.Expression/*0100001D*/ [System.Core/*23000002*/]System.Linq.Expressions.LambdaExpression/*01000070*/::get_Body() /* 0A0001AD */
    IL_0024:  /* 75   | (01)00006D       */ isinst     [System.Core/*23000002*/]System.Linq.Expressions.UnaryExpression/*0100006D*/
    IL_0029:  /* 2C   | 39               */ brfalse.s  IL_0064

    IL_002b:  /* 02   |                  */ ldarg.0
    IL_002c:  /* 6F   | (0A)0001AD       */ callvirt   instance class [System.Core/*23000002*/]System.Linq.Expressions.Expression/*0100001D*/ [System.Core/*23000002*/]System.Linq.Expressions.LambdaExpression/*01000070*/::get_Body() /* 0A0001AD */
    IL_0031:  /* 74   | (01)00006D       */ castclass  [System.Core/*23000002*/]System.Linq.Expressions.UnaryExpression/*0100006D*/
    IL_0036:  /* 6F   | (0A)000180       */ callvirt   instance class [System.Core/*23000002*/]System.Linq.Expressions.Expression/*0100001D*/ [System.Core/*23000002*/]System.Linq.Expressions.UnaryExpression/*0100006D*/::get_Operand() /* 0A000180 */
    IL_003b:  /* 0A   |                  */ stloc.0
    IL_003c:  /* 06   |                  */ ldloc.0
    IL_003d:  /* 75   | (01)00000E       */ isinst     [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/
    IL_0042:  /* 2C   | 07               */ brfalse.s  IL_004b

    IL_0044:  /* 06   |                  */ ldloc.0
    IL_0045:  /* 74   | (01)00000E       */ castclass  [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/
    IL_004a:  /* 2A   |                  */ ret

    IL_004b:  /* 06   |                  */ ldloc.0
    IL_004c:  /* 75   | (01)00001F       */ isinst     [System.Core/*23000002*/]System.Linq.Expressions.MethodCallExpression/*0100001F*/
    IL_0051:  /* 2C   | 11               */ brfalse.s  IL_0064

    IL_0053:  /* 06   |                  */ ldloc.0
    IL_0054:  /* 74   | (01)00001F       */ castclass  [System.Core/*23000002*/]System.Linq.Expressions.MethodCallExpression/*0100001F*/
    IL_0059:  /* 6F   | (0A)000188       */ callvirt   instance class [System.Core/*23000002*/]System.Linq.Expressions.Expression/*0100001D*/ [System.Core/*23000002*/]System.Linq.Expressions.MethodCallExpression/*0100001F*/::get_Object() /* 0A000188 */
    IL_005e:  /* 75   | (01)00000E       */ isinst     [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/
    IL_0063:  /* 2A   |                  */ ret

    IL_0064:  /* 14   |                  */ ldnull
    IL_0065:  /* 2A   |                  */ ret
  } // end of method ExpressionExtensions::GetMemberExpression

} // end of class MvcContrib.FluentHtml.Expressions.ExpressionExtensions

.class /*02000052*/ public auto ansi beforefieldinit MvcContrib.FluentHtml.ModelViewMasterPage`1<class T>
       extends class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewMasterPage`1/*01000008*/<!T>/*1B00002C*/
       implements class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!T>/*1B00002D*/,
                  [System.Web.Mvc/*23000003*/]System.Web.Mvc.IViewDataContainer/*01000007*/,
                  MvcContrib.FluentHtml.Behaviors.IBehaviorsContainer/*02000013*/
{
  .field /*0400006E*/ family initonly class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000021*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> behaviors
  .field /*0400006F*/ family string htmlNamePrefix
  .method /*0600015E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x5118
    // Code size       53 (0x35)
    .maxstack  4
    .locals /*1100002D*/ init (class [System.Core/*23000002*/]System.Func`1/*0100000D*/<class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelStateDictionary/*01000011*/> V_0)
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 0A   |                  */ stloc.0
    IL_0002:  /* 02   |                  */ ldarg.0
    IL_0003:  /* 73   | (0A)0001B4       */ newobj     instance void class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000021*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>/*1B000078*/::.ctor() /* 0A0001B4 */
    IL_0008:  /* 7D   | (0A)0001B5       */ stfld      class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000021*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> class MvcContrib.FluentHtml.ModelViewMasterPage`1/*02000052*/<!T>/*1B000077*/::behaviors /* 0A0001B5 */
    IL_000d:  /* 02   |                  */ ldarg.0
    IL_000e:  /* 28   | (0A)0001B6       */ call       instance void class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewMasterPage`1/*01000008*/<!T>/*1B00002C*/::.ctor() /* 0A0001B6 */
    IL_0013:  /* 02   |                  */ ldarg.0
    IL_0014:  /* 7B   | (0A)0001B5       */ ldfld      class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000021*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> class MvcContrib.FluentHtml.ModelViewMasterPage`1/*02000052*/<!T>/*1B000077*/::behaviors /* 0A0001B5 */
    IL_0019:  /* 06   |                  */ ldloc.0
    IL_001a:  /* 2D   | 0D               */ brtrue.s   IL_0029

    IL_001c:  /* 02   |                  */ ldarg.0
    IL_001d:  /* FE06 | (0A)0001B7       */ ldftn      instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelStateDictionary/*01000011*/ class MvcContrib.FluentHtml.ModelViewMasterPage`1/*02000052*/<!T>/*1B000077*/::'<.ctor>b__0'() /* 0A0001B7 */
    IL_0023:  /* 73   | (0A)0001B8       */ newobj     instance void class [System.Core/*23000002*/]System.Func`1/*0100000D*/<class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelStateDictionary/*01000011*/>/*1B000042*/::.ctor(object,
                                                                                                                                                                                                                                    native int) /* 0A0001B8 */
    IL_0028:  /* 0A   |                  */ stloc.0
    IL_0029:  /* 06   |                  */ ldloc.0
    IL_002a:  /* 73   | (06)00003E       */ newobj     instance void MvcContrib.FluentHtml.Behaviors.ValidationBehavior/*02000018*/::.ctor(class [System.Core/*23000002*/]System.Func`1/*0100000D*/<class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelStateDictionary/*01000011*/>) /* 0600003E */
    IL_002f:  /* 6F   | (0A)0001B9       */ callvirt   instance void class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000021*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>/*1B000078*/::Add(!0) /* 0A0001B9 */
    IL_0034:  /* 2A   |                  */ ret
  } // end of method ModelViewMasterPage`1::.ctor

  .method /*0600015F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/[] behaviors) cil managed
  // SIG: 20 01 01 1D 12 08
  {
    .param [1]/*0800016F*/ 
    .custom /*0C000060:0A000002*/ instance void [mscorlib/*23000001*/]System.ParamArrayAttribute/*01000022*/::.ctor() /* 0A000002 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x5159
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 03   |                  */ ldarg.1
    IL_0003:  /* 28   | (0A)0001BA       */ call       instance void class MvcContrib.FluentHtml.ModelViewMasterPage`1/*02000052*/<!T>/*1B000077*/::.ctor(string,
                                                                                                                                                          class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/[]) /* 0A0001BA */
    IL_0008:  /* 2A   |                  */ ret
  } // end of method ModelViewMasterPage`1::.ctor

  .method /*06000160*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string htmlNamePrefix,
                               class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/[] memberBehaviors) cil managed
  // SIG: 20 02 01 0E 1D 12 08
  {
    .param [2]/*08000171*/ 
    .custom /*0C000062:0A000002*/ instance void [mscorlib/*23000001*/]System.ParamArrayAttribute/*01000022*/::.ctor() /* 0A000002 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x5163
    // Code size       29 (0x1d)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)0001BB       */ call       instance void class MvcContrib.FluentHtml.ModelViewMasterPage`1/*02000052*/<!T>/*1B000077*/::.ctor() /* 0A0001BB */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (0A)0001BC       */ stfld      string class MvcContrib.FluentHtml.ModelViewMasterPage`1/*02000052*/<!T>/*1B000077*/::htmlNamePrefix /* 0A0001BC */
    IL_000d:  /* 04   |                  */ ldarg.2
    IL_000e:  /* 2C   | 0C               */ brfalse.s  IL_001c

    IL_0010:  /* 02   |                  */ ldarg.0
    IL_0011:  /* 7B   | (0A)0001B5       */ ldfld      class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000021*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> class MvcContrib.FluentHtml.ModelViewMasterPage`1/*02000052*/<!T>/*1B000077*/::behaviors /* 0A0001B5 */
    IL_0016:  /* 04   |                  */ ldarg.2
    IL_0017:  /* 6F   | (0A)0001BD       */ callvirt   instance void class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000021*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>/*1B000078*/::AddRange(class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<!0>) /* 0A0001BD */
    IL_001c:  /* 2A   |                  */ ret
  } // end of method ModelViewMasterPage`1::.ctor

  .method /*06000161*/ public hidebysig newslot specialname virtual final 
          instance string  get_HtmlNamePrefix() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x5181
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (0A)0001BC       */ ldfld      string class MvcContrib.FluentHtml.ModelViewMasterPage`1/*02000052*/<!T>/*1B000077*/::htmlNamePrefix /* 0A0001BC */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ModelViewMasterPage`1::get_HtmlNamePrefix

  .method /*06000162*/ public hidebysig newslot specialname virtual final 
          instance void  set_HtmlNamePrefix(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x5189
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (0A)0001BC       */ stfld      string class MvcContrib.FluentHtml.ModelViewMasterPage`1/*02000052*/<!T>/*1B000077*/::htmlNamePrefix /* 0A0001BC */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method ModelViewMasterPage`1::set_HtmlNamePrefix

  .method /*06000163*/ public hidebysig newslot specialname virtual final 
          instance !T  get_ViewModel() cil managed
  // SIG: 20 00 13 00
  {
    // Method begins at RVA 0x5192
    // Code size       22 (0x16)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)0001B2       */ call       instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/ class MvcContrib.FluentHtml.ModelViewMasterPage`1/*02000052*/<!T>/*1B000077*/::get_ViewData() /* 0A0001B2 */
    IL_0006:  /* 6F   | (0A)0001BE       */ callvirt   instance object [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/::get_Model() /* 0A0001BE */
    IL_000b:  /* 75   | (1B)000040       */ isinst     !T/*1B000040*/
    IL_0010:  /* A5   | (1B)000040       */ unbox.any  !T/*1B000040*/
    IL_0015:  /* 2A   |                  */ ret
  } // end of method ModelViewMasterPage`1::get_ViewModel

  .method /*06000164*/ public hidebysig newslot specialname virtual final 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> 
          get_Behaviors() cil managed
  // SIG: 20 00 15 12 11 01 12 08
  {
    // Method begins at RVA 0x51a9
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (0A)0001B5       */ ldfld      class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000021*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> class MvcContrib.FluentHtml.ModelViewMasterPage`1/*02000052*/<!T>/*1B000077*/::behaviors /* 0A0001B5 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ModelViewMasterPage`1::get_Behaviors

  .method /*06000165*/ public hidebysig newslot specialname virtual final 
          instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/ 
          get_ViewData() cil managed
  // SIG: 20 00 12 71
  {
    // Method begins at RVA 0x51b1
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)0001BF       */ call       instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary`1/*01000072*/<!0> class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewMasterPage`1/*01000008*/<!T>/*1B00002C*/::get_ViewData() /* 0A0001BF */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ModelViewMasterPage`1::get_ViewData

  .method /*06000166*/ public hidebysig newslot specialname virtual final 
          instance void  set_ViewData(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/ 'value') cil managed
  // SIG: 20 01 01 12 71
  {
    // Method begins at RVA 0x51b9
    // Code size       11 (0xb)
    .maxstack  8
    IL_0000:  /* 72   | (70)0009D5       */ ldstr      "ViewData from base class ViewMasterPage<T> is read"
    + "-only." /* 700009D5 */
    IL_0005:  /* 73   | (0A)0001C0       */ newobj     instance void [mscorlib/*23000001*/]System.NotImplementedException/*01000073*/::.ctor(string) /* 0A0001C0 */
    IL_000a:  /* 7A   |                  */ throw
  } // end of method ModelViewMasterPage`1::set_ViewData

  .method /*06000167*/ private hidebysig newslot specialname virtual final 
          instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.HtmlHelper/*01000020*/ 
          'MvcContrib.FluentHtml.IViewModelContainer<T>.get_Html'() cil managed
  // SIG: 20 00 12 80 81
  {
    .override  method instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.HtmlHelper/*01000020*/ class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!T>/*1B00002D*/::get_Html() /* 0A000001 */
    // Method begins at RVA 0x51c5
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)0001C1       */ call       instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.HtmlHelper`1/*01000074*/<!0> class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewMasterPage`1/*01000008*/<!T>/*1B00002C*/::get_Html() /* 0A0001C1 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ModelViewMasterPage`1::'MvcContrib.FluentHtml.IViewModelContainer<T>.get_Html'

  .method /*06000168*/ private hidebysig 
          instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelStateDictionary/*01000011*/ 
          '<.ctor>b__0'() cil managed
  // SIG: 20 00 12 45
  {
    .custom /*0C00005F:0A000013*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000032*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x510a
    // Code size       12 (0xc)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)0001B2       */ call       instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/ class MvcContrib.FluentHtml.ModelViewMasterPage`1/*02000052*/<!T>/*1B000077*/::get_ViewData() /* 0A0001B2 */
    IL_0006:  /* 6F   | (0A)0001B3       */ callvirt   instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelStateDictionary/*01000011*/ [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/::get_ModelState() /* 0A0001B3 */
    IL_000b:  /* 2A   |                  */ ret
  } // end of method ModelViewMasterPage`1::'<.ctor>b__0'

  .property /*17000022*/ instance string HtmlNamePrefix()
  {
    .set instance void MvcContrib.FluentHtml.ModelViewMasterPage`1/*02000052*/::set_HtmlNamePrefix(string) /* 06000162 */
    .get instance string MvcContrib.FluentHtml.ModelViewMasterPage`1/*02000052*/::get_HtmlNamePrefix() /* 06000161 */
  } // end of property ModelViewMasterPage`1::HtmlNamePrefix
  .property /*17000023*/ instance !T ViewModel()
  {
    .get instance !T MvcContrib.FluentHtml.ModelViewMasterPage`1/*02000052*/::get_ViewModel() /* 06000163 */
  } // end of property ModelViewMasterPage`1::ViewModel
  .property /*17000024*/ instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>
          Behaviors()
  {
    .get instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> MvcContrib.FluentHtml.ModelViewMasterPage`1/*02000052*/::get_Behaviors() /* 06000164 */
  } // end of property ModelViewMasterPage`1::Behaviors
  .property /*17000025*/ instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/
          ViewData()
  {
    .get instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/ MvcContrib.FluentHtml.ModelViewMasterPage`1/*02000052*/::get_ViewData() /* 06000165 */
    .set instance void MvcContrib.FluentHtml.ModelViewMasterPage`1/*02000052*/::set_ViewData(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/) /* 06000166 */
  } // end of property ModelViewMasterPage`1::ViewData
  .property /*17000026*/ instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.HtmlHelper/*01000020*/
          'MvcContrib.FluentHtml.IViewModelContainer<T>.Html'()
  {
    .get instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.HtmlHelper/*01000020*/ MvcContrib.FluentHtml.ModelViewMasterPage`1/*02000052*/::'MvcContrib.FluentHtml.IViewModelContainer<T>.get_Html'() /* 06000167 */
  } // end of property ModelViewMasterPage`1::'MvcContrib.FluentHtml.IViewModelContainer<T>.Html'
} // end of class MvcContrib.FluentHtml.ModelViewMasterPage`1

.class /*02000053*/ public auto ansi beforefieldinit MvcContrib.FluentHtml.ModelViewPage`1<class T>
       extends class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewPage`1/*01000009*/<!T>/*1B00002E*/
       implements class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!T>/*1B00002D*/,
                  [System.Web.Mvc/*23000003*/]System.Web.Mvc.IViewDataContainer/*01000007*/,
                  MvcContrib.FluentHtml.Behaviors.IBehaviorsContainer/*02000013*/
{
  .field /*04000070*/ family initonly class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000021*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> behaviors
  .field /*04000071*/ family string htmlNamePrefix
  .method /*06000169*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x51dc
    // Code size       53 (0x35)
    .maxstack  4
    .locals /*1100002D*/ init (class [System.Core/*23000002*/]System.Func`1/*0100000D*/<class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelStateDictionary/*01000011*/> V_0)
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 0A   |                  */ stloc.0
    IL_0002:  /* 02   |                  */ ldarg.0
    IL_0003:  /* 73   | (0A)0001B4       */ newobj     instance void class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000021*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>/*1B000078*/::.ctor() /* 0A0001B4 */
    IL_0008:  /* 7D   | (0A)0001C3       */ stfld      class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000021*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> class MvcContrib.FluentHtml.ModelViewPage`1/*02000053*/<!T>/*1B000079*/::behaviors /* 0A0001C3 */
    IL_000d:  /* 02   |                  */ ldarg.0
    IL_000e:  /* 28   | (0A)0001C4       */ call       instance void class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewPage`1/*01000009*/<!T>/*1B00002E*/::.ctor() /* 0A0001C4 */
    IL_0013:  /* 02   |                  */ ldarg.0
    IL_0014:  /* 7B   | (0A)0001C3       */ ldfld      class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000021*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> class MvcContrib.FluentHtml.ModelViewPage`1/*02000053*/<!T>/*1B000079*/::behaviors /* 0A0001C3 */
    IL_0019:  /* 06   |                  */ ldloc.0
    IL_001a:  /* 2D   | 0D               */ brtrue.s   IL_0029

    IL_001c:  /* 02   |                  */ ldarg.0
    IL_001d:  /* FE06 | (0A)0001C5       */ ldftn      instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelStateDictionary/*01000011*/ class MvcContrib.FluentHtml.ModelViewPage`1/*02000053*/<!T>/*1B000079*/::'<.ctor>b__0'() /* 0A0001C5 */
    IL_0023:  /* 73   | (0A)0001B8       */ newobj     instance void class [System.Core/*23000002*/]System.Func`1/*0100000D*/<class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelStateDictionary/*01000011*/>/*1B000042*/::.ctor(object,
                                                                                                                                                                                                                                    native int) /* 0A0001B8 */
    IL_0028:  /* 0A   |                  */ stloc.0
    IL_0029:  /* 06   |                  */ ldloc.0
    IL_002a:  /* 73   | (06)00003E       */ newobj     instance void MvcContrib.FluentHtml.Behaviors.ValidationBehavior/*02000018*/::.ctor(class [System.Core/*23000002*/]System.Func`1/*0100000D*/<class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelStateDictionary/*01000011*/>) /* 0600003E */
    IL_002f:  /* 6F   | (0A)0001B9       */ callvirt   instance void class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000021*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>/*1B000078*/::Add(!0) /* 0A0001B9 */
    IL_0034:  /* 2A   |                  */ ret
  } // end of method ModelViewPage`1::.ctor

  .method /*0600016A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/[] behaviors) cil managed
  // SIG: 20 01 01 1D 12 08
  {
    .param [1]/*08000174*/ 
    .custom /*0C000063:0A000002*/ instance void [mscorlib/*23000001*/]System.ParamArrayAttribute/*01000022*/::.ctor() /* 0A000002 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x521d
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 03   |                  */ ldarg.1
    IL_0003:  /* 28   | (0A)0001C6       */ call       instance void class MvcContrib.FluentHtml.ModelViewPage`1/*02000053*/<!T>/*1B000079*/::.ctor(string,
                                                                                                                                                    class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/[]) /* 0A0001C6 */
    IL_0008:  /* 2A   |                  */ ret
  } // end of method ModelViewPage`1::.ctor

  .method /*0600016B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string htmlNamePrefix,
                               class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/[] memberBehaviors) cil managed
  // SIG: 20 02 01 0E 1D 12 08
  {
    .param [2]/*08000176*/ 
    .custom /*0C000064:0A000002*/ instance void [mscorlib/*23000001*/]System.ParamArrayAttribute/*01000022*/::.ctor() /* 0A000002 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x5227
    // Code size       29 (0x1d)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)0001C7       */ call       instance void class MvcContrib.FluentHtml.ModelViewPage`1/*02000053*/<!T>/*1B000079*/::.ctor() /* 0A0001C7 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (0A)0001C8       */ stfld      string class MvcContrib.FluentHtml.ModelViewPage`1/*02000053*/<!T>/*1B000079*/::htmlNamePrefix /* 0A0001C8 */
    IL_000d:  /* 04   |                  */ ldarg.2
    IL_000e:  /* 2C   | 0C               */ brfalse.s  IL_001c

    IL_0010:  /* 02   |                  */ ldarg.0
    IL_0011:  /* 7B   | (0A)0001C3       */ ldfld      class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000021*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> class MvcContrib.FluentHtml.ModelViewPage`1/*02000053*/<!T>/*1B000079*/::behaviors /* 0A0001C3 */
    IL_0016:  /* 04   |                  */ ldarg.2
    IL_0017:  /* 6F   | (0A)0001BD       */ callvirt   instance void class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000021*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>/*1B000078*/::AddRange(class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<!0>) /* 0A0001BD */
    IL_001c:  /* 2A   |                  */ ret
  } // end of method ModelViewPage`1::.ctor

  .method /*0600016C*/ public hidebysig newslot specialname virtual final 
          instance string  get_HtmlNamePrefix() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x5245
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (0A)0001C8       */ ldfld      string class MvcContrib.FluentHtml.ModelViewPage`1/*02000053*/<!T>/*1B000079*/::htmlNamePrefix /* 0A0001C8 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ModelViewPage`1::get_HtmlNamePrefix

  .method /*0600016D*/ public hidebysig newslot specialname virtual final 
          instance void  set_HtmlNamePrefix(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x524d
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (0A)0001C8       */ stfld      string class MvcContrib.FluentHtml.ModelViewPage`1/*02000053*/<!T>/*1B000079*/::htmlNamePrefix /* 0A0001C8 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method ModelViewPage`1::set_HtmlNamePrefix

  .method /*0600016E*/ public hidebysig newslot specialname virtual final 
          instance !T  get_ViewModel() cil managed
  // SIG: 20 00 13 00
  {
    // Method begins at RVA 0x5256
    // Code size       12 (0xc)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)0001C2       */ call       instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary`1/*01000072*/<!0> class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewPage`1/*01000009*/<!T>/*1B00002E*/::get_ViewData() /* 0A0001C2 */
    IL_0006:  /* 6F   | (0A)0001C9       */ callvirt   instance !0 class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary`1/*01000072*/<!T>/*1B00007A*/::get_Model() /* 0A0001C9 */
    IL_000b:  /* 2A   |                  */ ret
  } // end of method ModelViewPage`1::get_ViewModel

  .method /*0600016F*/ public hidebysig newslot specialname virtual final 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> 
          get_Behaviors() cil managed
  // SIG: 20 00 15 12 11 01 12 08
  {
    // Method begins at RVA 0x5263
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (0A)0001C3       */ ldfld      class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000021*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> class MvcContrib.FluentHtml.ModelViewPage`1/*02000053*/<!T>/*1B000079*/::behaviors /* 0A0001C3 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ModelViewPage`1::get_Behaviors

  .method /*06000170*/ private hidebysig newslot specialname virtual final 
          instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.HtmlHelper/*01000020*/ 
          'MvcContrib.FluentHtml.IViewModelContainer<T>.get_Html'() cil managed
  // SIG: 20 00 12 80 81
  {
    .override  method instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.HtmlHelper/*01000020*/ class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!T>/*1B00002D*/::get_Html() /* 0A000001 */
    // Method begins at RVA 0x526b
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)0001CA       */ call       instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.HtmlHelper`1/*01000074*/<!0> class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewPage`1/*01000009*/<!T>/*1B00002E*/::get_Html() /* 0A0001CA */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ModelViewPage`1::'MvcContrib.FluentHtml.IViewModelContainer<T>.get_Html'

  .method /*06000171*/ private hidebysig 
          instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelStateDictionary/*01000011*/ 
          '<.ctor>b__0'() cil managed
  // SIG: 20 00 12 45
  {
    .custom /*0C000061:0A000013*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000032*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x51cd
    // Code size       12 (0xc)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)0001C2       */ call       instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary`1/*01000072*/<!0> class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewPage`1/*01000009*/<!T>/*1B00002E*/::get_ViewData() /* 0A0001C2 */
    IL_0006:  /* 6F   | (0A)0001B3       */ callvirt   instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelStateDictionary/*01000011*/ [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/::get_ModelState() /* 0A0001B3 */
    IL_000b:  /* 2A   |                  */ ret
  } // end of method ModelViewPage`1::'<.ctor>b__0'

  .property /*17000027*/ instance string HtmlNamePrefix()
  {
    .set instance void MvcContrib.FluentHtml.ModelViewPage`1/*02000053*/::set_HtmlNamePrefix(string) /* 0600016D */
    .get instance string MvcContrib.FluentHtml.ModelViewPage`1/*02000053*/::get_HtmlNamePrefix() /* 0600016C */
  } // end of property ModelViewPage`1::HtmlNamePrefix
  .property /*17000028*/ instance !T ViewModel()
  {
    .get instance !T MvcContrib.FluentHtml.ModelViewPage`1/*02000053*/::get_ViewModel() /* 0600016E */
  } // end of property ModelViewPage`1::ViewModel
  .property /*17000029*/ instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>
          Behaviors()
  {
    .get instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> MvcContrib.FluentHtml.ModelViewPage`1/*02000053*/::get_Behaviors() /* 0600016F */
  } // end of property ModelViewPage`1::Behaviors
  .property /*1700002A*/ instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.HtmlHelper/*01000020*/
          'MvcContrib.FluentHtml.IViewModelContainer<T>.Html'()
  {
    .get instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.HtmlHelper/*01000020*/ MvcContrib.FluentHtml.ModelViewPage`1/*02000053*/::'MvcContrib.FluentHtml.IViewModelContainer<T>.get_Html'() /* 06000170 */
  } // end of property ModelViewPage`1::'MvcContrib.FluentHtml.IViewModelContainer<T>.Html'
} // end of class MvcContrib.FluentHtml.ModelViewPage`1

.class /*02000054*/ public auto ansi beforefieldinit MvcContrib.FluentHtml.ModelViewUserControl`1<class T>
       extends class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewUserControl`1/*0100000A*/<!T>/*1B00002F*/
       implements class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!T>/*1B00002D*/,
                  [System.Web.Mvc/*23000003*/]System.Web.Mvc.IViewDataContainer/*01000007*/,
                  MvcContrib.FluentHtml.Behaviors.IBehaviorsContainer/*02000013*/
{
  .field /*04000072*/ family initonly class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000021*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> behaviors
  .field /*04000073*/ family string htmlNamePrefix
  .method /*06000172*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x5280
    // Code size       53 (0x35)
    .maxstack  4
    .locals /*1100002D*/ init (class [System.Core/*23000002*/]System.Func`1/*0100000D*/<class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelStateDictionary/*01000011*/> V_0)
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 0A   |                  */ stloc.0
    IL_0002:  /* 02   |                  */ ldarg.0
    IL_0003:  /* 73   | (0A)0001B4       */ newobj     instance void class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000021*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>/*1B000078*/::.ctor() /* 0A0001B4 */
    IL_0008:  /* 7D   | (0A)0001CC       */ stfld      class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000021*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> class MvcContrib.FluentHtml.ModelViewUserControl`1/*02000054*/<!T>/*1B00007B*/::behaviors /* 0A0001CC */
    IL_000d:  /* 02   |                  */ ldarg.0
    IL_000e:  /* 28   | (0A)0001CD       */ call       instance void class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewUserControl`1/*0100000A*/<!T>/*1B00002F*/::.ctor() /* 0A0001CD */
    IL_0013:  /* 02   |                  */ ldarg.0
    IL_0014:  /* 7B   | (0A)0001CC       */ ldfld      class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000021*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> class MvcContrib.FluentHtml.ModelViewUserControl`1/*02000054*/<!T>/*1B00007B*/::behaviors /* 0A0001CC */
    IL_0019:  /* 06   |                  */ ldloc.0
    IL_001a:  /* 2D   | 0D               */ brtrue.s   IL_0029

    IL_001c:  /* 02   |                  */ ldarg.0
    IL_001d:  /* FE06 | (0A)0001CE       */ ldftn      instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelStateDictionary/*01000011*/ class MvcContrib.FluentHtml.ModelViewUserControl`1/*02000054*/<!T>/*1B00007B*/::'<.ctor>b__0'() /* 0A0001CE */
    IL_0023:  /* 73   | (0A)0001B8       */ newobj     instance void class [System.Core/*23000002*/]System.Func`1/*0100000D*/<class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelStateDictionary/*01000011*/>/*1B000042*/::.ctor(object,
                                                                                                                                                                                                                                    native int) /* 0A0001B8 */
    IL_0028:  /* 0A   |                  */ stloc.0
    IL_0029:  /* 06   |                  */ ldloc.0
    IL_002a:  /* 73   | (06)00003E       */ newobj     instance void MvcContrib.FluentHtml.Behaviors.ValidationBehavior/*02000018*/::.ctor(class [System.Core/*23000002*/]System.Func`1/*0100000D*/<class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelStateDictionary/*01000011*/>) /* 0600003E */
    IL_002f:  /* 6F   | (0A)0001B9       */ callvirt   instance void class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000021*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>/*1B000078*/::Add(!0) /* 0A0001B9 */
    IL_0034:  /* 2A   |                  */ ret
  } // end of method ModelViewUserControl`1::.ctor

  .method /*06000173*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/[] behaviors) cil managed
  // SIG: 20 01 01 1D 12 08
  {
    .param [1]/*08000178*/ 
    .custom /*0C000065:0A000002*/ instance void [mscorlib/*23000001*/]System.ParamArrayAttribute/*01000022*/::.ctor() /* 0A000002 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x52c1
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 03   |                  */ ldarg.1
    IL_0003:  /* 28   | (0A)0001CF       */ call       instance void class MvcContrib.FluentHtml.ModelViewUserControl`1/*02000054*/<!T>/*1B00007B*/::.ctor(string,
                                                                                                                                                           class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/[]) /* 0A0001CF */
    IL_0008:  /* 2A   |                  */ ret
  } // end of method ModelViewUserControl`1::.ctor

  .method /*06000174*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string htmlNamePrefix,
                               class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/[] memberBehaviors) cil managed
  // SIG: 20 02 01 0E 1D 12 08
  {
    .param [2]/*0800017A*/ 
    .custom /*0C000067:0A000002*/ instance void [mscorlib/*23000001*/]System.ParamArrayAttribute/*01000022*/::.ctor() /* 0A000002 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x52cb
    // Code size       29 (0x1d)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)0001D0       */ call       instance void class MvcContrib.FluentHtml.ModelViewUserControl`1/*02000054*/<!T>/*1B00007B*/::.ctor() /* 0A0001D0 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (0A)0001D1       */ stfld      string class MvcContrib.FluentHtml.ModelViewUserControl`1/*02000054*/<!T>/*1B00007B*/::htmlNamePrefix /* 0A0001D1 */
    IL_000d:  /* 04   |                  */ ldarg.2
    IL_000e:  /* 2C   | 0C               */ brfalse.s  IL_001c

    IL_0010:  /* 02   |                  */ ldarg.0
    IL_0011:  /* 7B   | (0A)0001CC       */ ldfld      class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000021*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> class MvcContrib.FluentHtml.ModelViewUserControl`1/*02000054*/<!T>/*1B00007B*/::behaviors /* 0A0001CC */
    IL_0016:  /* 04   |                  */ ldarg.2
    IL_0017:  /* 6F   | (0A)0001BD       */ callvirt   instance void class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000021*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>/*1B000078*/::AddRange(class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<!0>) /* 0A0001BD */
    IL_001c:  /* 2A   |                  */ ret
  } // end of method ModelViewUserControl`1::.ctor

  .method /*06000175*/ public hidebysig newslot specialname virtual final 
          instance string  get_HtmlNamePrefix() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x52e9
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (0A)0001D1       */ ldfld      string class MvcContrib.FluentHtml.ModelViewUserControl`1/*02000054*/<!T>/*1B00007B*/::htmlNamePrefix /* 0A0001D1 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ModelViewUserControl`1::get_HtmlNamePrefix

  .method /*06000176*/ public hidebysig newslot specialname virtual final 
          instance void  set_HtmlNamePrefix(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x52f1
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 7D   | (0A)0001D1       */ stfld      string class MvcContrib.FluentHtml.ModelViewUserControl`1/*02000054*/<!T>/*1B00007B*/::htmlNamePrefix /* 0A0001D1 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method ModelViewUserControl`1::set_HtmlNamePrefix

  .method /*06000177*/ public hidebysig newslot specialname virtual final 
          instance !T  get_ViewModel() cil managed
  // SIG: 20 00 13 00
  {
    // Method begins at RVA 0x52fa
    // Code size       12 (0xc)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)0001CB       */ call       instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary`1/*01000072*/<!0> class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewUserControl`1/*0100000A*/<!T>/*1B00002F*/::get_ViewData() /* 0A0001CB */
    IL_0006:  /* 6F   | (0A)0001C9       */ callvirt   instance !0 class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary`1/*01000072*/<!T>/*1B00007A*/::get_Model() /* 0A0001C9 */
    IL_000b:  /* 2A   |                  */ ret
  } // end of method ModelViewUserControl`1::get_ViewModel

  .method /*06000178*/ public hidebysig newslot specialname virtual final 
          instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> 
          get_Behaviors() cil managed
  // SIG: 20 00 15 12 11 01 12 08
  {
    // Method begins at RVA 0x5307
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (0A)0001CC       */ ldfld      class [mscorlib/*23000001*/]System.Collections.Generic.List`1/*01000021*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> class MvcContrib.FluentHtml.ModelViewUserControl`1/*02000054*/<!T>/*1B00007B*/::behaviors /* 0A0001CC */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ModelViewUserControl`1::get_Behaviors

  .method /*06000179*/ private hidebysig newslot specialname virtual final 
          instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.HtmlHelper/*01000020*/ 
          'MvcContrib.FluentHtml.IViewModelContainer<T>.get_Html'() cil managed
  // SIG: 20 00 12 80 81
  {
    .override  method instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.HtmlHelper/*01000020*/ class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!T>/*1B00002D*/::get_Html() /* 0A000001 */
    // Method begins at RVA 0x530f
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)0001D2       */ call       instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.HtmlHelper`1/*01000074*/<!0> class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewUserControl`1/*0100000A*/<!T>/*1B00002F*/::get_Html() /* 0A0001D2 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ModelViewUserControl`1::'MvcContrib.FluentHtml.IViewModelContainer<T>.get_Html'

  .method /*0600017A*/ private hidebysig 
          instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelStateDictionary/*01000011*/ 
          '<.ctor>b__0'() cil managed
  // SIG: 20 00 12 45
  {
    .custom /*0C000066:0A000013*/ instance void [mscorlib/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000032*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x5273
    // Code size       12 (0xc)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)0001CB       */ call       instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary`1/*01000072*/<!0> class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewUserControl`1/*0100000A*/<!T>/*1B00002F*/::get_ViewData() /* 0A0001CB */
    IL_0006:  /* 6F   | (0A)0001B3       */ callvirt   instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelStateDictionary/*01000011*/ [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/::get_ModelState() /* 0A0001B3 */
    IL_000b:  /* 2A   |                  */ ret
  } // end of method ModelViewUserControl`1::'<.ctor>b__0'

  .property /*1700002B*/ instance string HtmlNamePrefix()
  {
    .get instance string MvcContrib.FluentHtml.ModelViewUserControl`1/*02000054*/::get_HtmlNamePrefix() /* 06000175 */
    .set instance void MvcContrib.FluentHtml.ModelViewUserControl`1/*02000054*/::set_HtmlNamePrefix(string) /* 06000176 */
  } // end of property ModelViewUserControl`1::HtmlNamePrefix
  .property /*1700002C*/ instance !T ViewModel()
  {
    .get instance !T MvcContrib.FluentHtml.ModelViewUserControl`1/*02000054*/::get_ViewModel() /* 06000177 */
  } // end of property ModelViewUserControl`1::ViewModel
  .property /*1700002D*/ instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>
          Behaviors()
  {
    .get instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> MvcContrib.FluentHtml.ModelViewUserControl`1/*02000054*/::get_Behaviors() /* 06000178 */
  } // end of property ModelViewUserControl`1::Behaviors
  .property /*1700002E*/ instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.HtmlHelper/*01000020*/
          'MvcContrib.FluentHtml.IViewModelContainer<T>.Html'()
  {
    .get instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.HtmlHelper/*01000020*/ MvcContrib.FluentHtml.ModelViewUserControl`1/*02000054*/::'MvcContrib.FluentHtml.IViewModelContainer<T>.get_Html'() /* 06000179 */
  } // end of property ModelViewUserControl`1::'MvcContrib.FluentHtml.IViewModelContainer<T>.Html'
} // end of class MvcContrib.FluentHtml.ModelViewUserControl`1

.class /*02000055*/ public abstract auto ansi sealed beforefieldinit MvcContrib.FluentHtml.ViewDataContainerExtensions
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C000039:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
  .method /*0600017B*/ public hidebysig static 
          class MvcContrib.FluentHtml.Elements.TextBox/*0200004E*/ 
          TextBox(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.IViewDataContainer/*01000007*/ view,
                  string name) cil managed
  // SIG: 00 02 12 81 38 12 1D 0E
  {
    .custom /*0C000068:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x5317
    // Code size       31 (0x1f)
    .maxstack  8
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 02   |                  */ ldarg.0
    IL_0003:  /* 28   | (06)00018D       */ call       class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> MvcContrib.FluentHtml.ViewDataContainerExtensions/*02000055*/::GetBehaviors(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.IViewDataContainer/*01000007*/) /* 0600018D */
    IL_0008:  /* 73   | (06)000155       */ newobj     instance void MvcContrib.FluentHtml.Elements.TextBox/*0200004E*/::.ctor(string,
                                                                                                                               class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                               class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 06000155 */
    IL_000d:  /* 02   |                  */ ldarg.0
    IL_000e:  /* 6F   | (0A)00016B       */ callvirt   instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/ [System.Web.Mvc/*23000003*/]System.Web.Mvc.IViewDataContainer/*01000007*/::get_ViewData() /* 0A00016B */
    IL_0013:  /* 03   |                  */ ldarg.1
    IL_0014:  /* 6F   | (0A)0001D3       */ callvirt   instance object [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/::Eval(string) /* 0A0001D3 */
    IL_0019:  /* 6F   | (0A)0001D4       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.Input`1/*02000020*/<class MvcContrib.FluentHtml.Elements.TextBox/*0200004E*/>/*1B00007C*/::Value(object) /* 0A0001D4 */
    IL_001e:  /* 2A   |                  */ ret
  } // end of method ViewDataContainerExtensions::TextBox

  .method /*0600017C*/ public hidebysig static 
          class MvcContrib.FluentHtml.Elements.Password/*02000049*/ 
          Password(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.IViewDataContainer/*01000007*/ view,
                   string name) cil managed
  // SIG: 00 02 12 81 24 12 1D 0E
  {
    .custom /*0C000069:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x5337
    // Code size       31 (0x1f)
    .maxstack  8
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 02   |                  */ ldarg.0
    IL_0003:  /* 28   | (06)00018D       */ call       class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> MvcContrib.FluentHtml.ViewDataContainerExtensions/*02000055*/::GetBehaviors(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.IViewDataContainer/*01000007*/) /* 0600018D */
    IL_0008:  /* 73   | (06)000148       */ newobj     instance void MvcContrib.FluentHtml.Elements.Password/*02000049*/::.ctor(string,
                                                                                                                                class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                                class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 06000148 */
    IL_000d:  /* 02   |                  */ ldarg.0
    IL_000e:  /* 6F   | (0A)00016B       */ callvirt   instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/ [System.Web.Mvc/*23000003*/]System.Web.Mvc.IViewDataContainer/*01000007*/::get_ViewData() /* 0A00016B */
    IL_0013:  /* 03   |                  */ ldarg.1
    IL_0014:  /* 6F   | (0A)0001D3       */ callvirt   instance object [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/::Eval(string) /* 0A0001D3 */
    IL_0019:  /* 6F   | (0A)0001D5       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.Input`1/*02000020*/<class MvcContrib.FluentHtml.Elements.Password/*02000049*/>/*1B00007D*/::Value(object) /* 0A0001D5 */
    IL_001e:  /* 2A   |                  */ ret
  } // end of method ViewDataContainerExtensions::Password

  .method /*0600017D*/ public hidebysig static 
          class MvcContrib.FluentHtml.Elements.Select/*0200004A*/ 
          Select(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.IViewDataContainer/*01000007*/ view,
                 string name) cil managed
  // SIG: 00 02 12 81 28 12 1D 0E
  {
    .custom /*0C00006A:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x5357
    // Code size       31 (0x1f)
    .maxstack  8
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 02   |                  */ ldarg.0
    IL_0003:  /* 28   | (06)00018D       */ call       class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> MvcContrib.FluentHtml.ViewDataContainerExtensions/*02000055*/::GetBehaviors(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.IViewDataContainer/*01000007*/) /* 0600018D */
    IL_0008:  /* 73   | (06)00014B       */ newobj     instance void MvcContrib.FluentHtml.Elements.Select/*0200004A*/::.ctor(string,
                                                                                                                              class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                              class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 0600014B */
    IL_000d:  /* 02   |                  */ ldarg.0
    IL_000e:  /* 6F   | (0A)00016B       */ callvirt   instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/ [System.Web.Mvc/*23000003*/]System.Web.Mvc.IViewDataContainer/*01000007*/::get_ViewData() /* 0A00016B */
    IL_0013:  /* 03   |                  */ ldarg.1
    IL_0014:  /* 6F   | (0A)0001D3       */ callvirt   instance object [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/::Eval(string) /* 0A0001D3 */
    IL_0019:  /* 6F   | (06)00014C       */ callvirt   instance class MvcContrib.FluentHtml.Elements.Select/*0200004A*/ MvcContrib.FluentHtml.Elements.Select/*0200004A*/::Selected(object) /* 0600014C */
    IL_001e:  /* 2A   |                  */ ret
  } // end of method ViewDataContainerExtensions::Select

  .method /*0600017E*/ public hidebysig static 
          class MvcContrib.FluentHtml.Elements.MultiSelect/*02000046*/ 
          MultiSelect(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.IViewDataContainer/*01000007*/ view,
                      string name) cil managed
  // SIG: 00 02 12 81 18 12 1D 0E
  {
    .custom /*0C00006B:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x5377
    // Code size       36 (0x24)
    .maxstack  8
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 02   |                  */ ldarg.0
    IL_0003:  /* 28   | (06)00018D       */ call       class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> MvcContrib.FluentHtml.ViewDataContainerExtensions/*02000055*/::GetBehaviors(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.IViewDataContainer/*01000007*/) /* 0600018D */
    IL_0008:  /* 73   | (06)00013C       */ newobj     instance void MvcContrib.FluentHtml.Elements.MultiSelect/*02000046*/::.ctor(string,
                                                                                                                                   class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                                   class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 0600013C */
    IL_000d:  /* 02   |                  */ ldarg.0
    IL_000e:  /* 6F   | (0A)00016B       */ callvirt   instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/ [System.Web.Mvc/*23000003*/]System.Web.Mvc.IViewDataContainer/*01000007*/::get_ViewData() /* 0A00016B */
    IL_0013:  /* 03   |                  */ ldarg.1
    IL_0014:  /* 6F   | (0A)0001D3       */ callvirt   instance object [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/::Eval(string) /* 0A0001D3 */
    IL_0019:  /* 75   | (01)000015       */ isinst     [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000015*/
    IL_001e:  /* 6F   | (06)00013D       */ callvirt   instance class MvcContrib.FluentHtml.Elements.MultiSelect/*02000046*/ MvcContrib.FluentHtml.Elements.MultiSelect/*02000046*/::Selected(class [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000015*/) /* 0600013D */
    IL_0023:  /* 2A   |                  */ ret
  } // end of method ViewDataContainerExtensions::MultiSelect

  .method /*0600017F*/ public hidebysig static 
          class MvcContrib.FluentHtml.Elements.Hidden/*0200002D*/ 
          Hidden(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.IViewDataContainer/*01000007*/ view,
                 string name) cil managed
  // SIG: 00 02 12 80 B4 12 1D 0E
  {
    .custom /*0C00006C:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x539c
    // Code size       31 (0x1f)
    .maxstack  8
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 02   |                  */ ldarg.0
    IL_0003:  /* 28   | (06)00018D       */ call       class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> MvcContrib.FluentHtml.ViewDataContainerExtensions/*02000055*/::GetBehaviors(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.IViewDataContainer/*01000007*/) /* 0600018D */
    IL_0008:  /* 73   | (06)0000D4       */ newobj     instance void MvcContrib.FluentHtml.Elements.Hidden/*0200002D*/::.ctor(string,
                                                                                                                              class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                              class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 060000D4 */
    IL_000d:  /* 02   |                  */ ldarg.0
    IL_000e:  /* 6F   | (0A)00016B       */ callvirt   instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/ [System.Web.Mvc/*23000003*/]System.Web.Mvc.IViewDataContainer/*01000007*/::get_ViewData() /* 0A00016B */
    IL_0013:  /* 03   |                  */ ldarg.1
    IL_0014:  /* 6F   | (0A)0001D3       */ callvirt   instance object [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/::Eval(string) /* 0A0001D3 */
    IL_0019:  /* 6F   | (0A)0000B3       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.Input`1/*02000020*/<class MvcContrib.FluentHtml.Elements.Hidden/*0200002D*/>/*1B00004D*/::Value(object) /* 0A0000B3 */
    IL_001e:  /* 2A   |                  */ ret
  } // end of method ViewDataContainerExtensions::Hidden

  .method /*06000180*/ public hidebysig static 
          class MvcContrib.FluentHtml.Elements.TextArea/*0200004D*/ 
          TextArea(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.IViewDataContainer/*01000007*/ view,
                   string name) cil managed
  // SIG: 00 02 12 81 34 12 1D 0E
  {
    .custom /*0C00006D:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x53bc
    // Code size       31 (0x1f)
    .maxstack  8
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 02   |                  */ ldarg.0
    IL_0003:  /* 28   | (06)00018D       */ call       class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> MvcContrib.FluentHtml.ViewDataContainerExtensions/*02000055*/::GetBehaviors(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.IViewDataContainer/*01000007*/) /* 0600018D */
    IL_0008:  /* 73   | (06)000153       */ newobj     instance void MvcContrib.FluentHtml.Elements.TextArea/*0200004D*/::.ctor(string,
                                                                                                                                class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                                class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 06000153 */
    IL_000d:  /* 02   |                  */ ldarg.0
    IL_000e:  /* 6F   | (0A)00016B       */ callvirt   instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/ [System.Web.Mvc/*23000003*/]System.Web.Mvc.IViewDataContainer/*01000007*/::get_ViewData() /* 0A00016B */
    IL_0013:  /* 03   |                  */ ldarg.1
    IL_0014:  /* 6F   | (0A)0001D3       */ callvirt   instance object [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/::Eval(string) /* 0A0001D3 */
    IL_0019:  /* 6F   | (0A)0001D6       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.TextAreaBase`1/*0200003D*/<class MvcContrib.FluentHtml.Elements.TextArea/*0200004D*/>/*1B00002A*/::Value(object) /* 0A0001D6 */
    IL_001e:  /* 2A   |                  */ ret
  } // end of method ViewDataContainerExtensions::TextArea

  .method /*06000181*/ public hidebysig static 
          class MvcContrib.FluentHtml.Elements.CheckBox/*02000024*/ 
          CheckBox(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.IViewDataContainer/*01000007*/ view,
                   string name) cil managed
  // SIG: 00 02 12 80 90 12 1D 0E
  {
    .custom /*0C00006E:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x53dc
    // Code size       73 (0x49)
    .maxstack  4
    .locals /*1100002E*/ init (class MvcContrib.FluentHtml.Elements.CheckBox/*02000024*/ V_0,
             valuetype [mscorlib/*23000001*/]System.Nullable`1/*0100004D*/<bool> V_1)
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 02   |                  */ ldarg.0
    IL_0003:  /* 28   | (06)00018D       */ call       class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> MvcContrib.FluentHtml.ViewDataContainerExtensions/*02000055*/::GetBehaviors(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.IViewDataContainer/*01000007*/) /* 0600018D */
    IL_0008:  /* 73   | (06)00009E       */ newobj     instance void MvcContrib.FluentHtml.Elements.CheckBox/*02000024*/::.ctor(string,
                                                                                                                                class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                                class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 0600009E */
    IL_000d:  /* 17   |                  */ ldc.i4.1
    IL_000e:  /* 8C   | (01)000075       */ box        [mscorlib/*23000001*/]System.Boolean/*01000075*/
    IL_0013:  /* 6F   | (0A)0000FA       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.Input`1/*02000020*/<class MvcContrib.FluentHtml.Elements.CheckBox/*02000024*/>/*1B00005B*/::Value(object) /* 0A0000FA */
    IL_0018:  /* 0A   |                  */ stloc.0
    IL_0019:  /* 02   |                  */ ldarg.0
    IL_001a:  /* 6F   | (0A)00016B       */ callvirt   instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/ [System.Web.Mvc/*23000003*/]System.Web.Mvc.IViewDataContainer/*01000007*/::get_ViewData() /* 0A00016B */
    IL_001f:  /* 03   |                  */ ldarg.1
    IL_0020:  /* 6F   | (0A)0001D3       */ callvirt   instance object [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/::Eval(string) /* 0A0001D3 */
    IL_0025:  /* 75   | (1B)00004E       */ isinst     valuetype [mscorlib/*23000001*/]System.Nullable`1/*0100004D*/<bool>/*1B00004E*/
    IL_002a:  /* A5   | (1B)00004E       */ unbox.any  valuetype [mscorlib/*23000001*/]System.Nullable`1/*0100004D*/<bool>/*1B00004E*/
    IL_002f:  /* 0B   |                  */ stloc.1
    IL_0030:  /* 12   | 01               */ ldloca.s   V_1
    IL_0032:  /* 28   | (0A)0000B5       */ call       instance bool valuetype [mscorlib/*23000001*/]System.Nullable`1/*0100004D*/<bool>/*1B00004E*/::get_HasValue() /* 0A0000B5 */
    IL_0037:  /* 2C   | 0E               */ brfalse.s  IL_0047

    IL_0039:  /* 06   |                  */ ldloc.0
    IL_003a:  /* 12   | 01               */ ldloca.s   V_1
    IL_003c:  /* 28   | (0A)0000B6       */ call       instance !0 valuetype [mscorlib/*23000001*/]System.Nullable`1/*0100004D*/<bool>/*1B00004E*/::get_Value() /* 0A0000B6 */
    IL_0041:  /* 6F   | (0A)0000FD       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.CheckBoxBase`1/*02000023*/<class MvcContrib.FluentHtml.Elements.CheckBox/*02000024*/>/*1B00000C*/::Checked(bool) /* 0A0000FD */
    IL_0046:  /* 26   |                  */ pop
    IL_0047:  /* 06   |                  */ ldloc.0
    IL_0048:  /* 2A   |                  */ ret
  } // end of method ViewDataContainerExtensions::CheckBox

  .method /*06000182*/ public hidebysig static 
          class MvcContrib.FluentHtml.Elements.CheckBoxList/*02000027*/ 
          CheckBoxList(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.IViewDataContainer/*01000007*/ view,
                       string name) cil managed
  // SIG: 00 02 12 80 9C 12 1D 0E
  {
    .custom /*0C00006F:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x5431
    // Code size       36 (0x24)
    .maxstack  8
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 02   |                  */ ldarg.0
    IL_0003:  /* 28   | (06)00018D       */ call       class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> MvcContrib.FluentHtml.ViewDataContainerExtensions/*02000055*/::GetBehaviors(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.IViewDataContainer/*01000007*/) /* 0600018D */
    IL_0008:  /* 73   | (06)0000BC       */ newobj     instance void MvcContrib.FluentHtml.Elements.CheckBoxList/*02000027*/::.ctor(string,
                                                                                                                                    class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                                    class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 060000BC */
    IL_000d:  /* 02   |                  */ ldarg.0
    IL_000e:  /* 6F   | (0A)00016B       */ callvirt   instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/ [System.Web.Mvc/*23000003*/]System.Web.Mvc.IViewDataContainer/*01000007*/::get_ViewData() /* 0A00016B */
    IL_0013:  /* 03   |                  */ ldarg.1
    IL_0014:  /* 6F   | (0A)0001D3       */ callvirt   instance object [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/::Eval(string) /* 0A0001D3 */
    IL_0019:  /* 75   | (01)000015       */ isinst     [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000015*/
    IL_001e:  /* 6F   | (0A)0001D7       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.CheckBoxListBase`1/*02000026*/<class MvcContrib.FluentHtml.Elements.CheckBoxList/*02000027*/>/*1B00000F*/::Selected(class [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000015*/) /* 0A0001D7 */
    IL_0023:  /* 2A   |                  */ ret
  } // end of method ViewDataContainerExtensions::CheckBoxList

  .method /*06000183*/ public hidebysig static 
          class MvcContrib.FluentHtml.Elements.SubmitButton/*0200004C*/ 
          SubmitButton(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.IViewDataContainer/*01000007*/ view,
                       string text) cil managed
  // SIG: 00 02 12 81 30 12 1D 0E
  {
    .custom /*0C000070:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x5456
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 02   |                  */ ldarg.0
    IL_0002:  /* 28   | (06)00018D       */ call       class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> MvcContrib.FluentHtml.ViewDataContainerExtensions/*02000055*/::GetBehaviors(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.IViewDataContainer/*01000007*/) /* 0600018D */
    IL_0007:  /* 73   | (06)000151       */ newobj     instance void MvcContrib.FluentHtml.Elements.SubmitButton/*0200004C*/::.ctor(string,
                                                                                                                                    class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 06000151 */
    IL_000c:  /* 2A   |                  */ ret
  } // end of method ViewDataContainerExtensions::SubmitButton

  .method /*06000184*/ public hidebysig static 
          class MvcContrib.FluentHtml.Elements.Button/*02000022*/ 
          Button(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.IViewDataContainer/*01000007*/ view,
                 string text) cil managed
  // SIG: 00 02 12 80 88 12 1D 0E
  {
    .custom /*0C000071:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x5464
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 02   |                  */ ldarg.0
    IL_0002:  /* 28   | (06)00018D       */ call       class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> MvcContrib.FluentHtml.ViewDataContainerExtensions/*02000055*/::GetBehaviors(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.IViewDataContainer/*01000007*/) /* 0600018D */
    IL_0007:  /* 73   | (06)000096       */ newobj     instance void MvcContrib.FluentHtml.Elements.Button/*02000022*/::.ctor(string,
                                                                                                                              class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 06000096 */
    IL_000c:  /* 2A   |                  */ ret
  } // end of method ViewDataContainerExtensions::Button

  .method /*06000185*/ public hidebysig static 
          class MvcContrib.FluentHtml.Elements.ResetButton/*0200002B*/ 
          ResetButton(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.IViewDataContainer/*01000007*/ view,
                      string text) cil managed
  // SIG: 00 02 12 80 AC 12 1D 0E
  {
    .custom /*0C000072:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x5472
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 02   |                  */ ldarg.0
    IL_0002:  /* 28   | (06)00018D       */ call       class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> MvcContrib.FluentHtml.ViewDataContainerExtensions/*02000055*/::GetBehaviors(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.IViewDataContainer/*01000007*/) /* 0600018D */
    IL_0007:  /* 73   | (06)0000CC       */ newobj     instance void MvcContrib.FluentHtml.Elements.ResetButton/*0200002B*/::.ctor(string,
                                                                                                                                   class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 060000CC */
    IL_000c:  /* 2A   |                  */ ret
  } // end of method ViewDataContainerExtensions::ResetButton

  .method /*06000186*/ public hidebysig static 
          class MvcContrib.FluentHtml.Elements.Label/*02000029*/ 
          Label(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.IViewDataContainer/*01000007*/ view,
                string forName) cil managed
  // SIG: 00 02 12 80 A4 12 1D 0E
  {
    .custom /*0C000073:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x5480
    // Code size       31 (0x1f)
    .maxstack  8
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 02   |                  */ ldarg.0
    IL_0003:  /* 28   | (06)00018D       */ call       class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> MvcContrib.FluentHtml.ViewDataContainerExtensions/*02000055*/::GetBehaviors(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.IViewDataContainer/*01000007*/) /* 0600018D */
    IL_0008:  /* 73   | (06)0000C6       */ newobj     instance void MvcContrib.FluentHtml.Elements.Label/*02000029*/::.ctor(string,
                                                                                                                             class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                             class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 060000C6 */
    IL_000d:  /* 02   |                  */ ldarg.0
    IL_000e:  /* 6F   | (0A)00016B       */ callvirt   instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/ [System.Web.Mvc/*23000003*/]System.Web.Mvc.IViewDataContainer/*01000007*/::get_ViewData() /* 0A00016B */
    IL_0013:  /* 03   |                  */ ldarg.1
    IL_0014:  /* 6F   | (0A)0001D3       */ callvirt   instance object [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/::Eval(string) /* 0A0001D3 */
    IL_0019:  /* 6F   | (0A)0001D8       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.LabelBase`1/*02000028*/<class MvcContrib.FluentHtml.Elements.Label/*02000029*/>/*1B000011*/::Value(object) /* 0A0001D8 */
    IL_001e:  /* 2A   |                  */ ret
  } // end of method ViewDataContainerExtensions::Label

  .method /*06000187*/ public hidebysig static 
          class MvcContrib.FluentHtml.Elements.Literal/*02000044*/ 
          Literal(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.IViewDataContainer/*01000007*/ view,
                  object 'value') cil managed
  // SIG: 00 02 12 81 10 12 1D 1C
  {
    .custom /*0C000074:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x54a0
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 72   | (70)000915       */ ldstr      "" /* 70000915 */
    IL_0006:  /* 03   |                  */ ldarg.1
    IL_0007:  /* 28   | (06)000188       */ call       class MvcContrib.FluentHtml.Elements.Literal/*02000044*/ MvcContrib.FluentHtml.ViewDataContainerExtensions/*02000055*/::Literal(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.IViewDataContainer/*01000007*/,
                                                                                                                                                                                       string,
                                                                                                                                                                                       object) /* 06000188 */
    IL_000c:  /* 2A   |                  */ ret
  } // end of method ViewDataContainerExtensions::Literal

  .method /*06000188*/ public hidebysig static 
          class MvcContrib.FluentHtml.Elements.Literal/*02000044*/ 
          Literal(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.IViewDataContainer/*01000007*/ view,
                  string name,
                  object 'value') cil managed
  // SIG: 00 03 12 81 10 12 1D 0E 1C
  {
    .custom /*0C000075:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x54ae
    // Code size       20 (0x14)
    .maxstack  8
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 02   |                  */ ldarg.0
    IL_0003:  /* 28   | (06)00018D       */ call       class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> MvcContrib.FluentHtml.ViewDataContainerExtensions/*02000055*/::GetBehaviors(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.IViewDataContainer/*01000007*/) /* 0600018D */
    IL_0008:  /* 73   | (06)00012C       */ newobj     instance void MvcContrib.FluentHtml.Elements.Literal/*02000044*/::.ctor(string,
                                                                                                                               class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                               class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 0600012C */
    IL_000d:  /* 04   |                  */ ldarg.2
    IL_000e:  /* 6F   | (0A)000122       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.LiteralBase`1/*02000032*/<class MvcContrib.FluentHtml.Elements.Literal/*02000044*/>/*1B000022*/::Value(object) /* 0A000122 */
    IL_0013:  /* 2A   |                  */ ret
  } // end of method ViewDataContainerExtensions::Literal

  .method /*06000189*/ public hidebysig static 
          class MvcContrib.FluentHtml.Elements.FormLiteral/*02000031*/ 
          FormLiteral(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.IViewDataContainer/*01000007*/ view,
                      string name) cil managed
  // SIG: 00 02 12 80 C4 12 1D 0E
  {
    .custom /*0C000076:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x54c3
    // Code size       31 (0x1f)
    .maxstack  8
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 02   |                  */ ldarg.0
    IL_0003:  /* 28   | (06)00018D       */ call       class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> MvcContrib.FluentHtml.ViewDataContainerExtensions/*02000055*/::GetBehaviors(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.IViewDataContainer/*01000007*/) /* 0600018D */
    IL_0008:  /* 73   | (06)0000DE       */ newobj     instance void MvcContrib.FluentHtml.Elements.FormLiteral/*02000031*/::.ctor(string,
                                                                                                                                   class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                                   class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 060000DE */
    IL_000d:  /* 02   |                  */ ldarg.0
    IL_000e:  /* 6F   | (0A)00016B       */ callvirt   instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/ [System.Web.Mvc/*23000003*/]System.Web.Mvc.IViewDataContainer/*01000007*/::get_ViewData() /* 0A00016B */
    IL_0013:  /* 03   |                  */ ldarg.1
    IL_0014:  /* 6F   | (0A)0001D3       */ callvirt   instance object [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/::Eval(string) /* 0A0001D3 */
    IL_0019:  /* 6F   | (0A)0001D9       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.Input`1/*02000020*/<class MvcContrib.FluentHtml.Elements.FormLiteral/*02000031*/>/*1B00007E*/::Value(object) /* 0A0001D9 */
    IL_001e:  /* 2A   |                  */ ret
  } // end of method ViewDataContainerExtensions::FormLiteral

  .method /*0600018A*/ public hidebysig static 
          class MvcContrib.FluentHtml.Elements.FileUpload/*0200002F*/ 
          FileUpload(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.IViewDataContainer/*01000007*/ view,
                     string name) cil managed
  // SIG: 00 02 12 80 BC 12 1D 0E
  {
    .custom /*0C000077:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x54e3
    // Code size       30 (0x1e)
    .maxstack  8
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 02   |                  */ ldarg.0
    IL_0002:  /* 28   | (06)00018D       */ call       class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> MvcContrib.FluentHtml.ViewDataContainerExtensions/*02000055*/::GetBehaviors(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.IViewDataContainer/*01000007*/) /* 0600018D */
    IL_0007:  /* 73   | (06)0000DA       */ newobj     instance void MvcContrib.FluentHtml.Elements.FileUpload/*0200002F*/::.ctor(string,
                                                                                                                                  class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 060000DA */
    IL_000c:  /* 02   |                  */ ldarg.0
    IL_000d:  /* 6F   | (0A)00016B       */ callvirt   instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/ [System.Web.Mvc/*23000003*/]System.Web.Mvc.IViewDataContainer/*01000007*/::get_ViewData() /* 0A00016B */
    IL_0012:  /* 03   |                  */ ldarg.1
    IL_0013:  /* 6F   | (0A)0001D3       */ callvirt   instance object [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/::Eval(string) /* 0A0001D3 */
    IL_0018:  /* 6F   | (0A)0001DA       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.Input`1/*02000020*/<class MvcContrib.FluentHtml.Elements.FileUpload/*0200002F*/>/*1B00007F*/::Value(object) /* 0A0001DA */
    IL_001d:  /* 2A   |                  */ ret
  } // end of method ViewDataContainerExtensions::FileUpload

  .method /*0600018B*/ public hidebysig static 
          class MvcContrib.FluentHtml.Elements.RadioButton/*02000034*/ 
          RadioButton(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.IViewDataContainer/*01000007*/ view,
                      string name) cil managed
  // SIG: 00 02 12 80 D0 12 1D 0E
  {
    .custom /*0C000078:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x5502
    // Code size       14 (0xe)
    .maxstack  8
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 02   |                  */ ldarg.0
    IL_0003:  /* 28   | (06)00018D       */ call       class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> MvcContrib.FluentHtml.ViewDataContainerExtensions/*02000055*/::GetBehaviors(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.IViewDataContainer/*01000007*/) /* 0600018D */
    IL_0008:  /* 73   | (06)0000F3       */ newobj     instance void MvcContrib.FluentHtml.Elements.RadioButton/*02000034*/::.ctor(string,
                                                                                                                                   class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                                   class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 060000F3 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method ViewDataContainerExtensions::RadioButton

  .method /*0600018C*/ public hidebysig static 
          class MvcContrib.FluentHtml.Elements.RadioSet/*02000036*/ 
          RadioSet(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.IViewDataContainer/*01000007*/ view,
                   string name) cil managed
  // SIG: 00 02 12 80 D8 12 1D 0E
  {
    .custom /*0C000079:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x5511
    // Code size       31 (0x1f)
    .maxstack  8
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 02   |                  */ ldarg.0
    IL_0003:  /* 28   | (06)00018D       */ call       class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> MvcContrib.FluentHtml.ViewDataContainerExtensions/*02000055*/::GetBehaviors(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.IViewDataContainer/*01000007*/) /* 0600018D */
    IL_0008:  /* 73   | (06)0000FD       */ newobj     instance void MvcContrib.FluentHtml.Elements.RadioSet/*02000036*/::.ctor(string,
                                                                                                                                class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                                class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 060000FD */
    IL_000d:  /* 02   |                  */ ldarg.0
    IL_000e:  /* 6F   | (0A)00016B       */ callvirt   instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/ [System.Web.Mvc/*23000003*/]System.Web.Mvc.IViewDataContainer/*01000007*/::get_ViewData() /* 0A00016B */
    IL_0013:  /* 03   |                  */ ldarg.1
    IL_0014:  /* 6F   | (0A)0001D3       */ callvirt   instance object [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/::Eval(string) /* 0A0001D3 */
    IL_0019:  /* 6F   | (0A)0001DB       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.RadioSetBase`1/*02000035*/<class MvcContrib.FluentHtml.Elements.RadioSet/*02000036*/>/*1B00001D*/::Selected(object) /* 0A0001DB */
    IL_001e:  /* 2A   |                  */ ret
  } // end of method ViewDataContainerExtensions::RadioSet

  .method /*0600018D*/ public hidebysig static 
          class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> 
          GetBehaviors(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.IViewDataContainer/*01000007*/ view) cil managed
  // SIG: 00 01 15 12 11 01 12 08 12 1D
  {
    .custom /*0C00007A:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x5534
    // Code size       19 (0x13)
    .maxstack  2
    .locals /*1100002F*/ init (class MvcContrib.FluentHtml.Behaviors.IBehaviorsContainer/*02000013*/ V_0)
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 75   | (02)000013       */ isinst     MvcContrib.FluentHtml.Behaviors.IBehaviorsContainer/*02000013*/
    IL_0006:  /* 0A   |                  */ stloc.0
    IL_0007:  /* 06   |                  */ ldloc.0
    IL_0008:  /* 2C   | 07               */ brfalse.s  IL_0011

    IL_000a:  /* 06   |                  */ ldloc.0
    IL_000b:  /* 6F   | (06)000036       */ callvirt   instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> MvcContrib.FluentHtml.Behaviors.IBehaviorsContainer/*02000013*/::get_Behaviors() /* 06000036 */
    IL_0010:  /* 2A   |                  */ ret

    IL_0011:  /* 14   |                  */ ldnull
    IL_0012:  /* 2A   |                  */ ret
  } // end of method ViewDataContainerExtensions::GetBehaviors

} // end of class MvcContrib.FluentHtml.ViewDataContainerExtensions

.class /*02000056*/ public abstract auto ansi sealed beforefieldinit MvcContrib.FluentHtml.ViewModelContainerExtensions
       extends [mscorlib/*23000001*/]System.Object/*01000001*/
{
  .custom /*0C00003A:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
  .method /*0600018E*/ public hidebysig static 
          class MvcContrib.FluentHtml.Elements.TextBox/*0200004E*/ 
          TextBox<class T>(class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!T> view,
                           class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!T,object>> expression) cil managed
  // SIG: 10 01 02 12 81 38 15 12 81 40 01 1E 00 15 12 69 01 15 12 2D 02 1E 00 1C
  {
    .custom /*0C00007B:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x5553
    // Code size       42 (0x2a)
    .maxstack  8
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 02   |                  */ ldarg.0
    IL_0002:  /* 28   | (2B)00001C       */ call       string MvcContrib.FluentHtml.Expressions.ExpressionExtensions/*02000051*/::GetNameFor<!!0,object>(class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,!!1>>,
                                                                                                                                                         class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!0>) /* 2B00001C */
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 28   | (2B)00001D       */ call       class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ MvcContrib.FluentHtml.Expressions.ExpressionExtensions/*02000051*/::GetMemberExpression<!!0,object>(class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,!!1>>) /* 2B00001D */
    IL_000d:  /* 02   |                  */ ldarg.0
    IL_000e:  /* 6F   | (06)000036       */ callvirt   instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> MvcContrib.FluentHtml.Behaviors.IBehaviorsContainer/*02000013*/::get_Behaviors() /* 06000036 */
    IL_0013:  /* 73   | (06)000155       */ newobj     instance void MvcContrib.FluentHtml.Elements.TextBox/*0200004E*/::.ctor(string,
                                                                                                                               class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                               class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 06000155 */
    IL_0018:  /* 03   |                  */ ldarg.1
    IL_0019:  /* 02   |                  */ ldarg.0
    IL_001a:  /* 6F   | (0A)0001DC       */ callvirt   instance !0 class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!T>/*1B000073*/::get_ViewModel() /* 0A0001DC */
    IL_001f:  /* 28   | (2B)00001E       */ call       !!1 MvcContrib.FluentHtml.Expressions.ExpressionExtensions/*02000051*/::GetValueFrom<!!0,object>(class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,!!1>>,
                                                                                                                                                        !!0) /* 2B00001E */
    IL_0024:  /* 6F   | (0A)0001D4       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.Input`1/*02000020*/<class MvcContrib.FluentHtml.Elements.TextBox/*0200004E*/>/*1B00007C*/::Value(object) /* 0A0001D4 */
    IL_0029:  /* 2A   |                  */ ret
  } // end of method ViewModelContainerExtensions::TextBox

  .method /*0600018F*/ public hidebysig static 
          class MvcContrib.FluentHtml.Elements.Password/*02000049*/ 
          Password<class T>(class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!T> view,
                            class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!T,object>> expression) cil managed
  // SIG: 10 01 02 12 81 24 15 12 81 40 01 1E 00 15 12 69 01 15 12 2D 02 1E 00 1C
  {
    .custom /*0C00007C:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x557e
    // Code size       42 (0x2a)
    .maxstack  8
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 02   |                  */ ldarg.0
    IL_0002:  /* 28   | (2B)00001C       */ call       string MvcContrib.FluentHtml.Expressions.ExpressionExtensions/*02000051*/::GetNameFor<!!0,object>(class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,!!1>>,
                                                                                                                                                         class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!0>) /* 2B00001C */
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 28   | (2B)00001D       */ call       class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ MvcContrib.FluentHtml.Expressions.ExpressionExtensions/*02000051*/::GetMemberExpression<!!0,object>(class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,!!1>>) /* 2B00001D */
    IL_000d:  /* 02   |                  */ ldarg.0
    IL_000e:  /* 6F   | (06)000036       */ callvirt   instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> MvcContrib.FluentHtml.Behaviors.IBehaviorsContainer/*02000013*/::get_Behaviors() /* 06000036 */
    IL_0013:  /* 73   | (06)000148       */ newobj     instance void MvcContrib.FluentHtml.Elements.Password/*02000049*/::.ctor(string,
                                                                                                                                class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                                class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 06000148 */
    IL_0018:  /* 03   |                  */ ldarg.1
    IL_0019:  /* 02   |                  */ ldarg.0
    IL_001a:  /* 6F   | (0A)0001DC       */ callvirt   instance !0 class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!T>/*1B000073*/::get_ViewModel() /* 0A0001DC */
    IL_001f:  /* 28   | (2B)00001E       */ call       !!1 MvcContrib.FluentHtml.Expressions.ExpressionExtensions/*02000051*/::GetValueFrom<!!0,object>(class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,!!1>>,
                                                                                                                                                        !!0) /* 2B00001E */
    IL_0024:  /* 6F   | (0A)0001D5       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.Input`1/*02000020*/<class MvcContrib.FluentHtml.Elements.Password/*02000049*/>/*1B00007D*/::Value(object) /* 0A0001D5 */
    IL_0029:  /* 2A   |                  */ ret
  } // end of method ViewModelContainerExtensions::Password

  .method /*06000190*/ public hidebysig static 
          class MvcContrib.FluentHtml.Elements.Hidden/*0200002D*/ 
          Hidden<class T>(class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!T> view,
                          class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!T,object>> expression) cil managed
  // SIG: 10 01 02 12 80 B4 15 12 81 40 01 1E 00 15 12 69 01 15 12 2D 02 1E 00 1C
  {
    .custom /*0C00007D:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x55a9
    // Code size       42 (0x2a)
    .maxstack  8
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 02   |                  */ ldarg.0
    IL_0002:  /* 28   | (2B)00001C       */ call       string MvcContrib.FluentHtml.Expressions.ExpressionExtensions/*02000051*/::GetNameFor<!!0,object>(class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,!!1>>,
                                                                                                                                                         class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!0>) /* 2B00001C */
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 28   | (2B)00001D       */ call       class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ MvcContrib.FluentHtml.Expressions.ExpressionExtensions/*02000051*/::GetMemberExpression<!!0,object>(class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,!!1>>) /* 2B00001D */
    IL_000d:  /* 02   |                  */ ldarg.0
    IL_000e:  /* 6F   | (06)000036       */ callvirt   instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> MvcContrib.FluentHtml.Behaviors.IBehaviorsContainer/*02000013*/::get_Behaviors() /* 06000036 */
    IL_0013:  /* 73   | (06)0000D4       */ newobj     instance void MvcContrib.FluentHtml.Elements.Hidden/*0200002D*/::.ctor(string,
                                                                                                                              class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                              class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 060000D4 */
    IL_0018:  /* 03   |                  */ ldarg.1
    IL_0019:  /* 02   |                  */ ldarg.0
    IL_001a:  /* 6F   | (0A)0001DC       */ callvirt   instance !0 class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!T>/*1B000073*/::get_ViewModel() /* 0A0001DC */
    IL_001f:  /* 28   | (2B)00001E       */ call       !!1 MvcContrib.FluentHtml.Expressions.ExpressionExtensions/*02000051*/::GetValueFrom<!!0,object>(class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,!!1>>,
                                                                                                                                                        !!0) /* 2B00001E */
    IL_0024:  /* 6F   | (0A)0000B3       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.Input`1/*02000020*/<class MvcContrib.FluentHtml.Elements.Hidden/*0200002D*/>/*1B00004D*/::Value(object) /* 0A0000B3 */
    IL_0029:  /* 2A   |                  */ ret
  } // end of method ViewModelContainerExtensions::Hidden

  .method /*06000191*/ public hidebysig static 
          class MvcContrib.FluentHtml.Elements.CheckBox/*02000024*/ 
          CheckBox<class T>(class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!T> view,
                            class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!T,object>> expression) cil managed
  // SIG: 10 01 02 12 80 90 15 12 81 40 01 1E 00 15 12 69 01 15 12 2D 02 1E 00 1C
  {
    .custom /*0C00007E:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x55d4
    // Code size       73 (0x49)
    .maxstack  4
    .locals /*1100002E*/ init (class MvcContrib.FluentHtml.Elements.CheckBox/*02000024*/ V_0,
             valuetype [mscorlib/*23000001*/]System.Nullable`1/*0100004D*/<bool> V_1)
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 02   |                  */ ldarg.0
    IL_0002:  /* 28   | (2B)00001C       */ call       string MvcContrib.FluentHtml.Expressions.ExpressionExtensions/*02000051*/::GetNameFor<!!0,object>(class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,!!1>>,
                                                                                                                                                         class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!0>) /* 2B00001C */
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 28   | (2B)00001D       */ call       class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ MvcContrib.FluentHtml.Expressions.ExpressionExtensions/*02000051*/::GetMemberExpression<!!0,object>(class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,!!1>>) /* 2B00001D */
    IL_000d:  /* 02   |                  */ ldarg.0
    IL_000e:  /* 6F   | (06)000036       */ callvirt   instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> MvcContrib.FluentHtml.Behaviors.IBehaviorsContainer/*02000013*/::get_Behaviors() /* 06000036 */
    IL_0013:  /* 73   | (06)00009E       */ newobj     instance void MvcContrib.FluentHtml.Elements.CheckBox/*02000024*/::.ctor(string,
                                                                                                                                class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                                class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 0600009E */
    IL_0018:  /* 0A   |                  */ stloc.0
    IL_0019:  /* 03   |                  */ ldarg.1
    IL_001a:  /* 02   |                  */ ldarg.0
    IL_001b:  /* 6F   | (0A)0001DC       */ callvirt   instance !0 class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!T>/*1B000073*/::get_ViewModel() /* 0A0001DC */
    IL_0020:  /* 28   | (2B)00001E       */ call       !!1 MvcContrib.FluentHtml.Expressions.ExpressionExtensions/*02000051*/::GetValueFrom<!!0,object>(class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,!!1>>,
                                                                                                                                                        !!0) /* 2B00001E */
    IL_0025:  /* 75   | (1B)00004E       */ isinst     valuetype [mscorlib/*23000001*/]System.Nullable`1/*0100004D*/<bool>/*1B00004E*/
    IL_002a:  /* A5   | (1B)00004E       */ unbox.any  valuetype [mscorlib/*23000001*/]System.Nullable`1/*0100004D*/<bool>/*1B00004E*/
    IL_002f:  /* 0B   |                  */ stloc.1
    IL_0030:  /* 12   | 01               */ ldloca.s   V_1
    IL_0032:  /* 28   | (0A)0000B5       */ call       instance bool valuetype [mscorlib/*23000001*/]System.Nullable`1/*0100004D*/<bool>/*1B00004E*/::get_HasValue() /* 0A0000B5 */
    IL_0037:  /* 2C   | 0E               */ brfalse.s  IL_0047

    IL_0039:  /* 06   |                  */ ldloc.0
    IL_003a:  /* 12   | 01               */ ldloca.s   V_1
    IL_003c:  /* 28   | (0A)0000B6       */ call       instance !0 valuetype [mscorlib/*23000001*/]System.Nullable`1/*0100004D*/<bool>/*1B00004E*/::get_Value() /* 0A0000B6 */
    IL_0041:  /* 6F   | (0A)0000FD       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.CheckBoxBase`1/*02000023*/<class MvcContrib.FluentHtml.Elements.CheckBox/*02000024*/>/*1B00000C*/::Checked(bool) /* 0A0000FD */
    IL_0046:  /* 26   |                  */ pop
    IL_0047:  /* 06   |                  */ ldloc.0
    IL_0048:  /* 2A   |                  */ ret
  } // end of method ViewModelContainerExtensions::CheckBox

  .method /*06000192*/ public hidebysig static 
          class MvcContrib.FluentHtml.Elements.CheckBoxList/*02000027*/ 
          CheckBoxList<class T>(class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!T> view,
                                class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!T,object>> expression) cil managed
  // SIG: 10 01 02 12 80 9C 15 12 81 40 01 1E 00 15 12 69 01 15 12 2D 02 1E 00 1C
  {
    .custom /*0C00007F:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x5629
    // Code size       47 (0x2f)
    .maxstack  8
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 02   |                  */ ldarg.0
    IL_0002:  /* 28   | (2B)00001C       */ call       string MvcContrib.FluentHtml.Expressions.ExpressionExtensions/*02000051*/::GetNameFor<!!0,object>(class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,!!1>>,
                                                                                                                                                         class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!0>) /* 2B00001C */
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 28   | (2B)00001D       */ call       class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ MvcContrib.FluentHtml.Expressions.ExpressionExtensions/*02000051*/::GetMemberExpression<!!0,object>(class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,!!1>>) /* 2B00001D */
    IL_000d:  /* 02   |                  */ ldarg.0
    IL_000e:  /* 6F   | (06)000036       */ callvirt   instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> MvcContrib.FluentHtml.Behaviors.IBehaviorsContainer/*02000013*/::get_Behaviors() /* 06000036 */
    IL_0013:  /* 73   | (06)0000BC       */ newobj     instance void MvcContrib.FluentHtml.Elements.CheckBoxList/*02000027*/::.ctor(string,
                                                                                                                                    class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                                    class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 060000BC */
    IL_0018:  /* 03   |                  */ ldarg.1
    IL_0019:  /* 02   |                  */ ldarg.0
    IL_001a:  /* 6F   | (0A)0001DC       */ callvirt   instance !0 class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!T>/*1B000073*/::get_ViewModel() /* 0A0001DC */
    IL_001f:  /* 28   | (2B)00001E       */ call       !!1 MvcContrib.FluentHtml.Expressions.ExpressionExtensions/*02000051*/::GetValueFrom<!!0,object>(class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,!!1>>,
                                                                                                                                                        !!0) /* 2B00001E */
    IL_0024:  /* 75   | (01)000015       */ isinst     [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000015*/
    IL_0029:  /* 6F   | (0A)0001D7       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.CheckBoxListBase`1/*02000026*/<class MvcContrib.FluentHtml.Elements.CheckBoxList/*02000027*/>/*1B00000F*/::Selected(class [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000015*/) /* 0A0001D7 */
    IL_002e:  /* 2A   |                  */ ret
  } // end of method ViewModelContainerExtensions::CheckBoxList

  .method /*06000193*/ public hidebysig static 
          class MvcContrib.FluentHtml.Elements.TextArea/*0200004D*/ 
          TextArea<class T>(class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!T> view,
                            class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!T,object>> expression) cil managed
  // SIG: 10 01 02 12 81 34 15 12 81 40 01 1E 00 15 12 69 01 15 12 2D 02 1E 00 1C
  {
    .custom /*0C000080:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x5659
    // Code size       42 (0x2a)
    .maxstack  8
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 02   |                  */ ldarg.0
    IL_0002:  /* 28   | (2B)00001C       */ call       string MvcContrib.FluentHtml.Expressions.ExpressionExtensions/*02000051*/::GetNameFor<!!0,object>(class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,!!1>>,
                                                                                                                                                         class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!0>) /* 2B00001C */
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 28   | (2B)00001D       */ call       class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ MvcContrib.FluentHtml.Expressions.ExpressionExtensions/*02000051*/::GetMemberExpression<!!0,object>(class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,!!1>>) /* 2B00001D */
    IL_000d:  /* 02   |                  */ ldarg.0
    IL_000e:  /* 6F   | (06)000036       */ callvirt   instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> MvcContrib.FluentHtml.Behaviors.IBehaviorsContainer/*02000013*/::get_Behaviors() /* 06000036 */
    IL_0013:  /* 73   | (06)000153       */ newobj     instance void MvcContrib.FluentHtml.Elements.TextArea/*0200004D*/::.ctor(string,
                                                                                                                                class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                                class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 06000153 */
    IL_0018:  /* 03   |                  */ ldarg.1
    IL_0019:  /* 02   |                  */ ldarg.0
    IL_001a:  /* 6F   | (0A)0001DC       */ callvirt   instance !0 class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!T>/*1B000073*/::get_ViewModel() /* 0A0001DC */
    IL_001f:  /* 28   | (2B)00001E       */ call       !!1 MvcContrib.FluentHtml.Expressions.ExpressionExtensions/*02000051*/::GetValueFrom<!!0,object>(class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,!!1>>,
                                                                                                                                                        !!0) /* 2B00001E */
    IL_0024:  /* 6F   | (0A)0001D6       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.TextAreaBase`1/*0200003D*/<class MvcContrib.FluentHtml.Elements.TextArea/*0200004D*/>/*1B00002A*/::Value(object) /* 0A0001D6 */
    IL_0029:  /* 2A   |                  */ ret
  } // end of method ViewModelContainerExtensions::TextArea

  .method /*06000194*/ public hidebysig static 
          class MvcContrib.FluentHtml.Elements.Select`1/*0200004B*/<!!T> 
          Select<class T>(class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!T> view,
                          class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!T,object>> expression) cil managed
  // SIG: 10 01 02 15 12 81 2C 01 1E 00 15 12 81 40 01 1E 00 15 12 69 01 15 12 2D 02 1E 00 1C
  {
    .custom /*0C000081:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x5684
    // Code size       42 (0x2a)
    .maxstack  8
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 02   |                  */ ldarg.0
    IL_0002:  /* 28   | (2B)00001C       */ call       string MvcContrib.FluentHtml.Expressions.ExpressionExtensions/*02000051*/::GetNameFor<!!0,object>(class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,!!1>>,
                                                                                                                                                         class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!0>) /* 2B00001C */
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 28   | (2B)00001D       */ call       class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ MvcContrib.FluentHtml.Expressions.ExpressionExtensions/*02000051*/::GetMemberExpression<!!0,object>(class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,!!1>>) /* 2B00001D */
    IL_000d:  /* 02   |                  */ ldarg.0
    IL_000e:  /* 6F   | (06)000036       */ callvirt   instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> MvcContrib.FluentHtml.Behaviors.IBehaviorsContainer/*02000013*/::get_Behaviors() /* 06000036 */
    IL_0013:  /* 73   | (0A)0001DD       */ newobj     instance void class MvcContrib.FluentHtml.Elements.Select`1/*0200004B*/<!!T>/*1B000080*/::.ctor(string,
                                                                                                                                                       class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                                                       class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 0A0001DD */
    IL_0018:  /* 03   |                  */ ldarg.1
    IL_0019:  /* 02   |                  */ ldarg.0
    IL_001a:  /* 6F   | (0A)0001DC       */ callvirt   instance !0 class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!T>/*1B000073*/::get_ViewModel() /* 0A0001DC */
    IL_001f:  /* 28   | (2B)00001E       */ call       !!1 MvcContrib.FluentHtml.Expressions.ExpressionExtensions/*02000051*/::GetValueFrom<!!0,object>(class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,!!1>>,
                                                                                                                                                        !!0) /* 2B00001E */
    IL_0024:  /* 6F   | (0A)0001DE       */ callvirt   instance class MvcContrib.FluentHtml.Elements.Select`1/*0200004B*/<!0> class MvcContrib.FluentHtml.Elements.Select`1/*0200004B*/<!!T>/*1B000080*/::Selected(object) /* 0A0001DE */
    IL_0029:  /* 2A   |                  */ ret
  } // end of method ViewModelContainerExtensions::Select

  .method /*06000195*/ public hidebysig static 
          class MvcContrib.FluentHtml.Elements.MultiSelect`1/*02000047*/<!!T> 
          MultiSelect<class T>(class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!T> view,
                               class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!T,object>> expression) cil managed
  // SIG: 10 01 02 15 12 81 1C 01 1E 00 15 12 81 40 01 1E 00 15 12 69 01 15 12 2D 02 1E 00 1C
  {
    .custom /*0C000082:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x56af
    // Code size       47 (0x2f)
    .maxstack  8
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 02   |                  */ ldarg.0
    IL_0002:  /* 28   | (2B)00001C       */ call       string MvcContrib.FluentHtml.Expressions.ExpressionExtensions/*02000051*/::GetNameFor<!!0,object>(class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,!!1>>,
                                                                                                                                                         class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!0>) /* 2B00001C */
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 28   | (2B)00001D       */ call       class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ MvcContrib.FluentHtml.Expressions.ExpressionExtensions/*02000051*/::GetMemberExpression<!!0,object>(class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,!!1>>) /* 2B00001D */
    IL_000d:  /* 02   |                  */ ldarg.0
    IL_000e:  /* 6F   | (06)000036       */ callvirt   instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> MvcContrib.FluentHtml.Behaviors.IBehaviorsContainer/*02000013*/::get_Behaviors() /* 06000036 */
    IL_0013:  /* 73   | (0A)0001DF       */ newobj     instance void class MvcContrib.FluentHtml.Elements.MultiSelect`1/*02000047*/<!!T>/*1B000081*/::.ctor(string,
                                                                                                                                                            class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                                                            class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 0A0001DF */
    IL_0018:  /* 03   |                  */ ldarg.1
    IL_0019:  /* 02   |                  */ ldarg.0
    IL_001a:  /* 6F   | (0A)0001DC       */ callvirt   instance !0 class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!T>/*1B000073*/::get_ViewModel() /* 0A0001DC */
    IL_001f:  /* 28   | (2B)00001E       */ call       !!1 MvcContrib.FluentHtml.Expressions.ExpressionExtensions/*02000051*/::GetValueFrom<!!0,object>(class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,!!1>>,
                                                                                                                                                        !!0) /* 2B00001E */
    IL_0024:  /* 75   | (01)000015       */ isinst     [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000015*/
    IL_0029:  /* 6F   | (0A)0001E0       */ callvirt   instance class MvcContrib.FluentHtml.Elements.MultiSelect`1/*02000047*/<!0> class MvcContrib.FluentHtml.Elements.MultiSelect`1/*02000047*/<!!T>/*1B000081*/::Selected(class [mscorlib/*23000001*/]System.Collections.IEnumerable/*01000015*/) /* 0A0001E0 */
    IL_002e:  /* 2A   |                  */ ret
  } // end of method ViewModelContainerExtensions::MultiSelect

  .method /*06000196*/ public hidebysig static 
          class MvcContrib.FluentHtml.Elements.Label/*02000029*/ 
          Label<class T>(class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!T> view,
                         class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!T,object>> expression) cil managed
  // SIG: 10 01 02 12 80 A4 15 12 81 40 01 1E 00 15 12 69 01 15 12 2D 02 1E 00 1C
  {
    .custom /*0C000083:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x56df
    // Code size       36 (0x24)
    .maxstack  8
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 02   |                  */ ldarg.0
    IL_0002:  /* 28   | (2B)00001C       */ call       string MvcContrib.FluentHtml.Expressions.ExpressionExtensions/*02000051*/::GetNameFor<!!0,object>(class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,!!1>>,
                                                                                                                                                         class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!0>) /* 2B00001C */
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 28   | (2B)00001D       */ call       class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ MvcContrib.FluentHtml.Expressions.ExpressionExtensions/*02000051*/::GetMemberExpression<!!0,object>(class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,!!1>>) /* 2B00001D */
    IL_000d:  /* 02   |                  */ ldarg.0
    IL_000e:  /* 6F   | (06)000036       */ callvirt   instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> MvcContrib.FluentHtml.Behaviors.IBehaviorsContainer/*02000013*/::get_Behaviors() /* 06000036 */
    IL_0013:  /* 73   | (06)0000C6       */ newobj     instance void MvcContrib.FluentHtml.Elements.Label/*02000029*/::.ctor(string,
                                                                                                                             class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                             class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 060000C6 */
    IL_0018:  /* 03   |                  */ ldarg.1
    IL_0019:  /* 28   | (2B)000018       */ call       string MvcContrib.FluentHtml.Expressions.ExpressionExtensions/*02000051*/::GetNameFor<!!0,object>(class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,!!1>>) /* 2B000018 */
    IL_001e:  /* 6F   | (0A)0001D8       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.LabelBase`1/*02000028*/<class MvcContrib.FluentHtml.Elements.Label/*02000029*/>/*1B000011*/::Value(object) /* 0A0001D8 */
    IL_0023:  /* 2A   |                  */ ret
  } // end of method ViewModelContainerExtensions::Label

  .method /*06000197*/ public hidebysig static 
          class MvcContrib.FluentHtml.Elements.Literal/*02000044*/ 
          Literal<class T>(class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!T> view,
                           class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!T,object>> expression) cil managed
  // SIG: 10 01 02 12 81 10 15 12 81 40 01 1E 00 15 12 69 01 15 12 2D 02 1E 00 1C
  {
    .custom /*0C000084:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x5704
    // Code size       42 (0x2a)
    .maxstack  8
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 02   |                  */ ldarg.0
    IL_0002:  /* 28   | (2B)00001C       */ call       string MvcContrib.FluentHtml.Expressions.ExpressionExtensions/*02000051*/::GetNameFor<!!0,object>(class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,!!1>>,
                                                                                                                                                         class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!0>) /* 2B00001C */
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 28   | (2B)00001D       */ call       class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ MvcContrib.FluentHtml.Expressions.ExpressionExtensions/*02000051*/::GetMemberExpression<!!0,object>(class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,!!1>>) /* 2B00001D */
    IL_000d:  /* 02   |                  */ ldarg.0
    IL_000e:  /* 6F   | (06)000036       */ callvirt   instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> MvcContrib.FluentHtml.Behaviors.IBehaviorsContainer/*02000013*/::get_Behaviors() /* 06000036 */
    IL_0013:  /* 73   | (06)00012C       */ newobj     instance void MvcContrib.FluentHtml.Elements.Literal/*02000044*/::.ctor(string,
                                                                                                                               class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                               class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 0600012C */
    IL_0018:  /* 03   |                  */ ldarg.1
    IL_0019:  /* 02   |                  */ ldarg.0
    IL_001a:  /* 6F   | (0A)0001DC       */ callvirt   instance !0 class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!T>/*1B000073*/::get_ViewModel() /* 0A0001DC */
    IL_001f:  /* 28   | (2B)00001E       */ call       !!1 MvcContrib.FluentHtml.Expressions.ExpressionExtensions/*02000051*/::GetValueFrom<!!0,object>(class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,!!1>>,
                                                                                                                                                        !!0) /* 2B00001E */
    IL_0024:  /* 6F   | (0A)000122       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.LiteralBase`1/*02000032*/<class MvcContrib.FluentHtml.Elements.Literal/*02000044*/>/*1B000022*/::Value(object) /* 0A000122 */
    IL_0029:  /* 2A   |                  */ ret
  } // end of method ViewModelContainerExtensions::Literal

  .method /*06000198*/ public hidebysig static 
          class MvcContrib.FluentHtml.Elements.FormLiteral/*02000031*/ 
          FormLiteral<class T>(class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!T> view,
                               class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!T,object>> expression) cil managed
  // SIG: 10 01 02 12 80 C4 15 12 81 40 01 1E 00 15 12 69 01 15 12 2D 02 1E 00 1C
  {
    .custom /*0C000085:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x572f
    // Code size       42 (0x2a)
    .maxstack  8
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 02   |                  */ ldarg.0
    IL_0002:  /* 28   | (2B)00001C       */ call       string MvcContrib.FluentHtml.Expressions.ExpressionExtensions/*02000051*/::GetNameFor<!!0,object>(class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,!!1>>,
                                                                                                                                                         class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!0>) /* 2B00001C */
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 28   | (2B)00001D       */ call       class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ MvcContrib.FluentHtml.Expressions.ExpressionExtensions/*02000051*/::GetMemberExpression<!!0,object>(class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,!!1>>) /* 2B00001D */
    IL_000d:  /* 02   |                  */ ldarg.0
    IL_000e:  /* 6F   | (06)000036       */ callvirt   instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> MvcContrib.FluentHtml.Behaviors.IBehaviorsContainer/*02000013*/::get_Behaviors() /* 06000036 */
    IL_0013:  /* 73   | (06)0000DE       */ newobj     instance void MvcContrib.FluentHtml.Elements.FormLiteral/*02000031*/::.ctor(string,
                                                                                                                                   class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                                   class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 060000DE */
    IL_0018:  /* 03   |                  */ ldarg.1
    IL_0019:  /* 02   |                  */ ldarg.0
    IL_001a:  /* 6F   | (0A)0001DC       */ callvirt   instance !0 class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!T>/*1B000073*/::get_ViewModel() /* 0A0001DC */
    IL_001f:  /* 28   | (2B)00001E       */ call       !!1 MvcContrib.FluentHtml.Expressions.ExpressionExtensions/*02000051*/::GetValueFrom<!!0,object>(class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,!!1>>,
                                                                                                                                                        !!0) /* 2B00001E */
    IL_0024:  /* 6F   | (0A)0001D9       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.Input`1/*02000020*/<class MvcContrib.FluentHtml.Elements.FormLiteral/*02000031*/>/*1B00007E*/::Value(object) /* 0A0001D9 */
    IL_0029:  /* 2A   |                  */ ret
  } // end of method ViewModelContainerExtensions::FormLiteral

  .method /*06000199*/ public hidebysig static 
          class MvcContrib.FluentHtml.Elements.RadioButton/*02000034*/ 
          RadioButton<class T>(class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!T> view,
                               class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!T,object>> expression) cil managed
  // SIG: 10 01 02 12 80 D0 15 12 81 40 01 1E 00 15 12 69 01 15 12 2D 02 1E 00 1C
  {
    .custom /*0C000086:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x575a
    // Code size       25 (0x19)
    .maxstack  8
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 02   |                  */ ldarg.0
    IL_0002:  /* 28   | (2B)00001C       */ call       string MvcContrib.FluentHtml.Expressions.ExpressionExtensions/*02000051*/::GetNameFor<!!0,object>(class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,!!1>>,
                                                                                                                                                         class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!0>) /* 2B00001C */
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 28   | (2B)00001D       */ call       class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ MvcContrib.FluentHtml.Expressions.ExpressionExtensions/*02000051*/::GetMemberExpression<!!0,object>(class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,!!1>>) /* 2B00001D */
    IL_000d:  /* 02   |                  */ ldarg.0
    IL_000e:  /* 6F   | (06)000036       */ callvirt   instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> MvcContrib.FluentHtml.Behaviors.IBehaviorsContainer/*02000013*/::get_Behaviors() /* 06000036 */
    IL_0013:  /* 73   | (06)0000F3       */ newobj     instance void MvcContrib.FluentHtml.Elements.RadioButton/*02000034*/::.ctor(string,
                                                                                                                                   class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                                   class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 060000F3 */
    IL_0018:  /* 2A   |                  */ ret
  } // end of method ViewModelContainerExtensions::RadioButton

  .method /*0600019A*/ public hidebysig static 
          class MvcContrib.FluentHtml.Elements.RadioSet/*02000036*/ 
          RadioSet<class T>(class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!T> view,
                            class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!T,object>> expression) cil managed
  // SIG: 10 01 02 12 80 D8 15 12 81 40 01 1E 00 15 12 69 01 15 12 2D 02 1E 00 1C
  {
    .custom /*0C000087:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x5774
    // Code size       42 (0x2a)
    .maxstack  8
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 02   |                  */ ldarg.0
    IL_0002:  /* 28   | (2B)00001C       */ call       string MvcContrib.FluentHtml.Expressions.ExpressionExtensions/*02000051*/::GetNameFor<!!0,object>(class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,!!1>>,
                                                                                                                                                         class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!0>) /* 2B00001C */
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 28   | (2B)00001D       */ call       class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ MvcContrib.FluentHtml.Expressions.ExpressionExtensions/*02000051*/::GetMemberExpression<!!0,object>(class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,!!1>>) /* 2B00001D */
    IL_000d:  /* 02   |                  */ ldarg.0
    IL_000e:  /* 6F   | (06)000036       */ callvirt   instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> MvcContrib.FluentHtml.Behaviors.IBehaviorsContainer/*02000013*/::get_Behaviors() /* 06000036 */
    IL_0013:  /* 73   | (06)0000FD       */ newobj     instance void MvcContrib.FluentHtml.Elements.RadioSet/*02000036*/::.ctor(string,
                                                                                                                                class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                                class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 060000FD */
    IL_0018:  /* 03   |                  */ ldarg.1
    IL_0019:  /* 02   |                  */ ldarg.0
    IL_001a:  /* 6F   | (0A)0001DC       */ callvirt   instance !0 class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!T>/*1B000073*/::get_ViewModel() /* 0A0001DC */
    IL_001f:  /* 28   | (2B)00001E       */ call       !!1 MvcContrib.FluentHtml.Expressions.ExpressionExtensions/*02000051*/::GetValueFrom<!!0,object>(class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,!!1>>,
                                                                                                                                                        !!0) /* 2B00001E */
    IL_0024:  /* 6F   | (0A)0001DB       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.RadioSetBase`1/*02000035*/<class MvcContrib.FluentHtml.Elements.RadioSet/*02000036*/>/*1B00001D*/::Selected(object) /* 0A0001DB */
    IL_0029:  /* 2A   |                  */ ret
  } // end of method ViewModelContainerExtensions::RadioSet

  .method /*0600019B*/ public hidebysig static 
          class MvcContrib.FluentHtml.Elements.ValidationMessage/*0200003E*/ 
          ValidationMessage<class T>(class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!T> view,
                                     class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!T,object>> expression) cil managed
  // SIG: 10 01 02 12 80 F8 15 12 81 40 01 1E 00 15 12 69 01 15 12 2D 02 1E 00 1C
  {
    .custom /*0C000088:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x579f
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 14   |                  */ ldnull
    IL_0003:  /* 28   | (2B)00001F       */ call       class MvcContrib.FluentHtml.Elements.ValidationMessage/*0200003E*/ MvcContrib.FluentHtml.ViewModelContainerExtensions/*02000056*/::ValidationMessage<!!0>(class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!0>,
                                                                                                                                                                                                                 class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,object>>,
                                                                                                                                                                                                                 string) /* 2B00001F */
    IL_0008:  /* 2A   |                  */ ret
  } // end of method ViewModelContainerExtensions::ValidationMessage

  .method /*0600019C*/ public hidebysig static 
          class MvcContrib.FluentHtml.Elements.ValidationMessage/*0200003E*/ 
          ValidationMessage<class T>(class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!T> view,
                                     class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!T,object>> expression,
                                     string message) cil managed
  // SIG: 10 01 03 12 80 F8 15 12 81 40 01 1E 00 15 12 69 01 15 12 2D 02 1E 00 1C 0E
  {
    .custom /*0C000089:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x57ac
    // Code size       136 (0x88)
    .maxstack  3
    .locals /*11000030*/ init (string V_0,
             string V_1,
             class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelState/*0100000F*/ V_2)
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 0A   |                  */ stloc.0
    IL_0002:  /* 03   |                  */ ldarg.1
    IL_0003:  /* 02   |                  */ ldarg.0
    IL_0004:  /* 28   | (2B)00001C       */ call       string MvcContrib.FluentHtml.Expressions.ExpressionExtensions/*02000051*/::GetNameFor<!!0,object>(class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,!!1>>,
                                                                                                                                                         class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!0>) /* 2B00001C */
    IL_0009:  /* 0B   |                  */ stloc.1
    IL_000a:  /* 02   |                  */ ldarg.0
    IL_000b:  /* 6F   | (0A)00016B       */ callvirt   instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/ [System.Web.Mvc/*23000003*/]System.Web.Mvc.IViewDataContainer/*01000007*/::get_ViewData() /* 0A00016B */
    IL_0010:  /* 6F   | (0A)0001B3       */ callvirt   instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelStateDictionary/*01000011*/ [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/::get_ModelState() /* 0A0001B3 */
    IL_0015:  /* 07   |                  */ ldloc.1
    IL_0016:  /* 6F   | (0A)0001E1       */ callvirt   instance bool [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelStateDictionary/*01000011*/::ContainsKey(string) /* 0A0001E1 */
    IL_001b:  /* 2C   | 53               */ brfalse.s  IL_0070

    IL_001d:  /* 02   |                  */ ldarg.0
    IL_001e:  /* 6F   | (0A)00016B       */ callvirt   instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/ [System.Web.Mvc/*23000003*/]System.Web.Mvc.IViewDataContainer/*01000007*/::get_ViewData() /* 0A00016B */
    IL_0023:  /* 6F   | (0A)0001B3       */ callvirt   instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelStateDictionary/*01000011*/ [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/::get_ModelState() /* 0A0001B3 */
    IL_0028:  /* 07   |                  */ ldloc.1
    IL_0029:  /* 6F   | (0A)0001E2       */ callvirt   instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelState/*0100000F*/ [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelStateDictionary/*01000011*/::get_Item(string) /* 0A0001E2 */
    IL_002e:  /* 0C   |                  */ stloc.2
    IL_002f:  /* 08   |                  */ ldloc.2
    IL_0030:  /* 2C   | 3E               */ brfalse.s  IL_0070

    IL_0032:  /* 08   |                  */ ldloc.2
    IL_0033:  /* 6F   | (0A)00005D       */ callvirt   instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelErrorCollection/*01000051*/ [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelState/*0100000F*/::get_Errors() /* 0A00005D */
    IL_0038:  /* 2C   | 36               */ brfalse.s  IL_0070

    IL_003a:  /* 08   |                  */ ldloc.2
    IL_003b:  /* 6F   | (0A)00005D       */ callvirt   instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelErrorCollection/*01000051*/ [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelState/*0100000F*/::get_Errors() /* 0A00005D */
    IL_0040:  /* 6F   | (0A)00005E       */ callvirt   instance int32 class [mscorlib/*23000001*/]System.Collections.ObjectModel.Collection`1/*01000052*/<class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelError/*01000053*/>/*1B000043*/::get_Count() /* 0A00005E */
    IL_0045:  /* 16   |                  */ ldc.i4.0
    IL_0046:  /* 31   | 28               */ ble.s      IL_0070

    IL_0048:  /* 08   |                  */ ldloc.2
    IL_0049:  /* 6F   | (0A)00005D       */ callvirt   instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelErrorCollection/*01000051*/ [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelState/*0100000F*/::get_Errors() /* 0A00005D */
    IL_004e:  /* 16   |                  */ ldc.i4.0
    IL_004f:  /* 6F   | (0A)0001E3       */ callvirt   instance !0 class [mscorlib/*23000001*/]System.Collections.ObjectModel.Collection`1/*01000052*/<class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelError/*01000053*/>/*1B000043*/::get_Item(int32) /* 0A0001E3 */
    IL_0054:  /* 2C   | 18               */ brfalse.s  IL_006e

    IL_0056:  /* 04   |                  */ ldarg.2
    IL_0057:  /* 25   |                  */ dup
    IL_0058:  /* 2D   | 15               */ brtrue.s   IL_006f

    IL_005a:  /* 26   |                  */ pop
    IL_005b:  /* 08   |                  */ ldloc.2
    IL_005c:  /* 6F   | (0A)00005D       */ callvirt   instance class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelErrorCollection/*01000051*/ [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelState/*0100000F*/::get_Errors() /* 0A00005D */
    IL_0061:  /* 16   |                  */ ldc.i4.0
    IL_0062:  /* 6F   | (0A)0001E3       */ callvirt   instance !0 class [mscorlib/*23000001*/]System.Collections.ObjectModel.Collection`1/*01000052*/<class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelError/*01000053*/>/*1B000043*/::get_Item(int32) /* 0A0001E3 */
    IL_0067:  /* 6F   | (0A)0001E4       */ callvirt   instance string [System.Web.Mvc/*23000003*/]System.Web.Mvc.ModelError/*01000053*/::get_ErrorMessage() /* 0A0001E4 */
    IL_006c:  /* 2B   | 01               */ br.s       IL_006f

    IL_006e:  /* 14   |                  */ ldnull
    IL_006f:  /* 0A   |                  */ stloc.0
    IL_0070:  /* 03   |                  */ ldarg.1
    IL_0071:  /* 28   | (2B)00001D       */ call       class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ MvcContrib.FluentHtml.Expressions.ExpressionExtensions/*02000051*/::GetMemberExpression<!!0,object>(class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,!!1>>) /* 2B00001D */
    IL_0076:  /* 02   |                  */ ldarg.0
    IL_0077:  /* 6F   | (06)000036       */ callvirt   instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> MvcContrib.FluentHtml.Behaviors.IBehaviorsContainer/*02000013*/::get_Behaviors() /* 06000036 */
    IL_007c:  /* 73   | (06)00011E       */ newobj     instance void MvcContrib.FluentHtml.Elements.ValidationMessage/*0200003E*/::.ctor(class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                                         class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 0600011E */
    IL_0081:  /* 06   |                  */ ldloc.0
    IL_0082:  /* 6F   | (0A)0001E5       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.LiteralBase`1/*02000032*/<class MvcContrib.FluentHtml.Elements.ValidationMessage/*0200003E*/>/*1B000021*/::Value(object) /* 0A0001E5 */
    IL_0087:  /* 2A   |                  */ ret
  } // end of method ViewModelContainerExtensions::ValidationMessage

  .method /*0600019D*/ public hidebysig static 
          class MvcContrib.FluentHtml.Elements.Index/*0200002E*/ 
          Index<class T>(class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!T> view,
                         class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!T,object>> expression) cil managed
  // SIG: 10 01 02 12 80 B8 15 12 81 40 01 1E 00 15 12 69 01 15 12 2D 02 1E 00 1C
  {
    .custom /*0C00008A:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x5840
    // Code size       25 (0x19)
    .maxstack  8
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 02   |                  */ ldarg.0
    IL_0002:  /* 28   | (2B)00001C       */ call       string MvcContrib.FluentHtml.Expressions.ExpressionExtensions/*02000051*/::GetNameFor<!!0,object>(class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,!!1>>,
                                                                                                                                                         class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!0>) /* 2B00001C */
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 28   | (2B)00001D       */ call       class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ MvcContrib.FluentHtml.Expressions.ExpressionExtensions/*02000051*/::GetMemberExpression<!!0,object>(class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,!!1>>) /* 2B00001D */
    IL_000d:  /* 02   |                  */ ldarg.0
    IL_000e:  /* 6F   | (06)000036       */ callvirt   instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> MvcContrib.FluentHtml.Behaviors.IBehaviorsContainer/*02000013*/::get_Behaviors() /* 06000036 */
    IL_0013:  /* 73   | (06)0000D6       */ newobj     instance void MvcContrib.FluentHtml.Elements.Index/*0200002E*/::.ctor(string,
                                                                                                                             class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                             class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 060000D6 */
    IL_0018:  /* 2A   |                  */ ret
  } // end of method ViewModelContainerExtensions::Index

  .method /*0600019E*/ public hidebysig static 
          class MvcContrib.FluentHtml.Elements.Hidden/*0200002D*/ 
          Index<class T>(class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!T> view,
                         class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!T,object>> expression,
                         class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!T,object>> valueExpression) cil managed
  // SIG: 10 01 03 12 80 B4 15 12 81 40 01 1E 00 15 12 69 01 15 12 2D 02 1E 00 1C 15 12 69 01 15 12 2D 02
  //      1E 00 1C
  {
    .custom /*0C00008B:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x585c
    // Code size       103 (0x67)
    .maxstack  4
    .locals /*11000031*/ init (string V_0,
             object V_1,
             string V_2,
             class MvcContrib.FluentHtml.Elements.Hidden/*0200002D*/ V_3)
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 02   |                  */ ldarg.0
    IL_0002:  /* 28   | (2B)00001C       */ call       string MvcContrib.FluentHtml.Expressions.ExpressionExtensions/*02000051*/::GetNameFor<!!0,object>(class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,!!1>>,
                                                                                                                                                         class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!0>) /* 2B00001C */
    IL_0007:  /* 72   | (70)00042D       */ ldstr      ".Index" /* 7000042D */
    IL_000c:  /* 28   | (0A)000075       */ call       string [mscorlib/*23000001*/]System.String/*0100003F*/::Concat(string,
                                                                                                                      string) /* 0A000075 */
    IL_0011:  /* 0A   |                  */ stloc.0
    IL_0012:  /* 04   |                  */ ldarg.2
    IL_0013:  /* 02   |                  */ ldarg.0
    IL_0014:  /* 6F   | (0A)0001DC       */ callvirt   instance !0 class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!T>/*1B000073*/::get_ViewModel() /* 0A0001DC */
    IL_0019:  /* 28   | (2B)00001E       */ call       !!1 MvcContrib.FluentHtml.Expressions.ExpressionExtensions/*02000051*/::GetValueFrom<!!0,object>(class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,!!1>>,
                                                                                                                                                        !!0) /* 2B00001E */
    IL_001e:  /* 0B   |                  */ stloc.1
    IL_001f:  /* 06   |                  */ ldloc.0
    IL_0020:  /* 28   | (06)00010E       */ call       string MvcContrib.FluentHtml.StringExtensions/*0200003B*/::FormatAsHtmlId(string) /* 0600010E */
    IL_0025:  /* 07   |                  */ ldloc.1
    IL_0026:  /* 2C   | 17               */ brfalse.s  IL_003f

    IL_0028:  /* 72   | (70)0008CF       */ ldstr      "_" /* 700008CF */
    IL_002d:  /* 07   |                  */ ldloc.1
    IL_002e:  /* 6F   | (0A)00006E       */ callvirt   instance string [mscorlib/*23000001*/]System.Object/*01000001*/::ToString() /* 0A00006E */
    IL_0033:  /* 28   | (06)00010E       */ call       string MvcContrib.FluentHtml.StringExtensions/*0200003B*/::FormatAsHtmlId(string) /* 0600010E */
    IL_0038:  /* 28   | (0A)000075       */ call       string [mscorlib/*23000001*/]System.String/*0100003F*/::Concat(string,
                                                                                                                      string) /* 0A000075 */
    IL_003d:  /* 2B   | 01               */ br.s       IL_0040

    IL_003f:  /* 14   |                  */ ldnull
    IL_0040:  /* 28   | (0A)000075       */ call       string [mscorlib/*23000001*/]System.String/*0100003F*/::Concat(string,
                                                                                                                      string) /* 0A000075 */
    IL_0045:  /* 0C   |                  */ stloc.2
    IL_0046:  /* 06   |                  */ ldloc.0
    IL_0047:  /* 03   |                  */ ldarg.1
    IL_0048:  /* 28   | (2B)00001D       */ call       class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/ MvcContrib.FluentHtml.Expressions.ExpressionExtensions/*02000051*/::GetMemberExpression<!!0,object>(class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,!!1>>) /* 2B00001D */
    IL_004d:  /* 02   |                  */ ldarg.0
    IL_004e:  /* 6F   | (06)000036       */ callvirt   instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> MvcContrib.FluentHtml.Behaviors.IBehaviorsContainer/*02000013*/::get_Behaviors() /* 06000036 */
    IL_0053:  /* 73   | (06)0000D4       */ newobj     instance void MvcContrib.FluentHtml.Elements.Hidden/*0200002D*/::.ctor(string,
                                                                                                                              class [System.Core/*23000002*/]System.Linq.Expressions.MemberExpression/*0100000E*/,
                                                                                                                              class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 060000D4 */
    IL_0058:  /* 07   |                  */ ldloc.1
    IL_0059:  /* 6F   | (0A)0000B3       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.Input`1/*02000020*/<class MvcContrib.FluentHtml.Elements.Hidden/*0200002D*/>/*1B00004D*/::Value(object) /* 0A0000B3 */
    IL_005e:  /* 08   |                  */ ldloc.2
    IL_005f:  /* 6F   | (0A)0000B2       */ callvirt   instance !0 class MvcContrib.FluentHtml.Elements.Element`1/*0200001D*/<class MvcContrib.FluentHtml.Elements.Hidden/*0200002D*/>/*1B00004C*/::Id(string) /* 0A0000B2 */
    IL_0064:  /* 0D   |                  */ stloc.3
    IL_0065:  /* 09   |                  */ ldloc.3
    IL_0066:  /* 2A   |                  */ ret
  } // end of method ViewModelContainerExtensions::Index

  .method /*0600019F*/ public hidebysig static 
          string  NameFor<class T>(class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!T> view,
                                   class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!T,object>> expression) cil managed
  // SIG: 10 01 02 0E 15 12 81 40 01 1E 00 15 12 69 01 15 12 2D 02 1E 00 1C
  {
    .custom /*0C00008C:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x58cf
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 02   |                  */ ldarg.0
    IL_0002:  /* 28   | (2B)00001C       */ call       string MvcContrib.FluentHtml.Expressions.ExpressionExtensions/*02000051*/::GetNameFor<!!0,object>(class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,!!1>>,
                                                                                                                                                         class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!0>) /* 2B00001C */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method ViewModelContainerExtensions::NameFor

  .method /*060001A0*/ public hidebysig static 
          string  IdFor<class T>(class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!T> view,
                                 class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!T,object>> expression) cil managed
  // SIG: 10 01 02 0E 15 12 81 40 01 1E 00 15 12 69 01 15 12 2D 02 1E 00 1C
  {
    .custom /*0C00008D:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x58d8
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 02   |                  */ ldarg.0
    IL_0002:  /* 28   | (2B)00001C       */ call       string MvcContrib.FluentHtml.Expressions.ExpressionExtensions/*02000051*/::GetNameFor<!!0,object>(class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!0,!!1>>,
                                                                                                                                                         class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!0>) /* 2B00001C */
    IL_0007:  /* 28   | (06)00010E       */ call       string MvcContrib.FluentHtml.StringExtensions/*0200003B*/::FormatAsHtmlId(string) /* 0600010E */
    IL_000c:  /* 2A   |                  */ ret
  } // end of method ViewModelContainerExtensions::IdFor

  .method /*060001A1*/ public hidebysig static 
          class MvcContrib.FluentHtml.Elements.SubmitButton/*0200004C*/ 
          SubmitButton<class T>(class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!T> view,
                                string text) cil managed
  // SIG: 10 01 02 12 81 30 15 12 81 40 01 1E 00 0E
  {
    .custom /*0C00008E:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x58e6
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 02   |                  */ ldarg.0
    IL_0002:  /* 6F   | (06)000036       */ callvirt   instance class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/> MvcContrib.FluentHtml.Behaviors.IBehaviorsContainer/*02000013*/::get_Behaviors() /* 06000036 */
    IL_0007:  /* 73   | (06)000151       */ newobj     instance void MvcContrib.FluentHtml.Elements.SubmitButton/*0200004C*/::.ctor(string,
                                                                                                                                    class [mscorlib/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000004*/<class MvcContrib.FluentHtml.Behaviors.IBehaviorMarker/*02000002*/>) /* 06000151 */
    IL_000c:  /* 2A   |                  */ ret
  } // end of method ViewModelContainerExtensions::SubmitButton

  .method /*060001A2*/ public hidebysig static 
          class MvcContrib.FluentHtml.Elements.TypeStamp/*02000037*/ 
          TypeStamp<class T>(class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!T> view) cil managed
  // SIG: 10 01 01 12 80 DC 15 12 81 40 01 1E 00
  {
    .custom /*0C00008F:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x58f4
    // Code size       22 (0x16)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 6F   | (0A)0001AE       */ callvirt   instance string class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!T>/*1B000073*/::get_HtmlNamePrefix() /* 0A0001AE */
    IL_0006:  /* D0   | (1B)000031       */ ldtoken    !!T/*1B000031*/
    IL_000b:  /* 28   | (0A)000021       */ call       class [mscorlib/*23000001*/]System.Type/*01000003*/ [mscorlib/*23000001*/]System.Type/*01000003*/::GetTypeFromHandle(valuetype [mscorlib/*23000001*/]System.RuntimeTypeHandle/*01000036*/) /* 0A000021 */
    IL_0010:  /* 73   | (06)0000FE       */ newobj     instance void MvcContrib.FluentHtml.Elements.TypeStamp/*02000037*/::.ctor(string,
                                                                                                                                 class [mscorlib/*23000001*/]System.Type/*01000003*/) /* 060000FE */
    IL_0015:  /* 2A   |                  */ ret
  } // end of method ViewModelContainerExtensions::TypeStamp

  .method /*060001A3*/ public hidebysig static 
          void  RenderTypedPartial<class T,class TPartialViewModel>(class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!T> view,
                                                                    class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!T,!!TPartialViewModel>> modelExpression) cil managed
  // SIG: 10 02 02 01 15 12 81 40 01 1E 00 15 12 69 01 15 12 2D 02 1E 00 1E 01
  {
    .custom /*0C000090:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x590c
    // Code size       54 (0x36)
    .maxstack  4
    .locals /*11000032*/ init (class [mscorlib/*23000001*/]System.Type/*01000003*/ V_0,
             string V_1,
             !!TPartialViewModel V_2)
    IL_0000:  /* 03   |                  */ ldarg.1
    IL_0001:  /* 6F   | (0A)0001B0       */ callvirt   instance !0 class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!T,!!TPartialViewModel>>/*1B000074*/::Compile() /* 0A0001B0 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 6F   | (0A)0001DC       */ callvirt   instance !0 class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!T>/*1B000073*/::get_ViewModel() /* 0A0001DC */
    IL_000c:  /* 6F   | (0A)0001B1       */ callvirt   instance !1 class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!T,!!TPartialViewModel>/*1B000075*/::Invoke(!0) /* 0A0001B1 */
    IL_0011:  /* 0C   |                  */ stloc.2
    IL_0012:  /* 12   | 02               */ ldloca.s   V_2
    IL_0014:  /* FE16 | (1B)000076       */ constrained. !!TPartialViewModel/*1B000076*/
    IL_001a:  /* 6F   | (0A)000016       */ callvirt   instance class [mscorlib/*23000001*/]System.Type/*01000003*/ [mscorlib/*23000001*/]System.Object/*01000001*/::GetType() /* 0A000016 */
    IL_001f:  /* 0A   |                  */ stloc.0
    IL_0020:  /* 06   |                  */ ldloc.0
    IL_0021:  /* 28   | (06)000104       */ call       string MvcContrib.FluentHtml.PartialNameConventionService/*02000039*/::GeneratePartialName(class [mscorlib/*23000001*/]System.Type/*01000003*/) /* 06000104 */
    IL_0026:  /* 0B   |                  */ stloc.1
    IL_0027:  /* 02   |                  */ ldarg.0
    IL_0028:  /* 07   |                  */ ldloc.1
    IL_0029:  /* 03   |                  */ ldarg.1
    IL_002a:  /* 73   | (0A)0001E6       */ newobj     instance void class MvcContrib.FluentHtml.PartialRenderer`2/*0200003A*/<!!T,!!TPartialViewModel>/*1B000082*/::.ctor(class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!0>,
                                                                                                                                                                           string,
                                                                                                                                                                           class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!0,!1>>) /* 0A0001E6 */
    IL_002f:  /* 06   |                  */ ldloc.0
    IL_0030:  /* 28   | (0A)0001E7       */ call       instance void class MvcContrib.FluentHtml.PartialRenderer`2/*0200003A*/<!!T,!!TPartialViewModel>/*1B000082*/::RenderTypedPartial(class [mscorlib/*23000001*/]System.Type/*01000003*/) /* 0A0001E7 */
    IL_0035:  /* 2A   |                  */ ret
  } // end of method ViewModelContainerExtensions::RenderTypedPartial

  .method /*060001A4*/ public hidebysig static 
          void  RenderPartial<class T,class TPartialViewModel>(class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!T> view,
                                                               string partialViewName,
                                                               class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!T,!!TPartialViewModel>> modelExpression,
                                                               class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/ viewData) cil managed
  // SIG: 10 02 04 01 15 12 81 40 01 1E 00 0E 15 12 69 01 15 12 2D 02 1E 00 1E 01 12 71
  {
    .custom /*0C000091:0A000055*/ instance void [mscorlib/*23000001*/]System.ObsoleteAttribute/*0100004E*/::.ctor(string) /* 0A000055 */ = ( 01 00 81 6E 55 73 65 20 6F 76 65 72 6C 6F 61 64   // ...nUse overload
                                                                                                                                             20 74 68 61 74 20 74 61 6B 65 73 20 46 75 6E 63   //  that takes Func
                                                                                                                                             3C 73 74 72 69 6E 67 2C 20 6F 62 6A 65 63 74 3E   // <string, object>
                                                                                                                                             5B 5D 20 76 69 65 77 44 61 74 61 49 74 65 6D 73   // [] viewDataItems
                                                                                                                                             20 69 6E 73 74 65 61 64 2E 20 20 54 68 69 73 20   //  instead.  This 
                                                                                                                                             77 69 6C 6C 20 77 69 6C 6C 20 61 76 6F 69 64 20   // will will avoid 
                                                                                                                                             69 6E 61 76 65 72 74 65 6E 74 6C 79 20 70 61 73   // inavertently pas
                                                                                                                                             73 69 6E 67 20 69 6E 20 74 68 65 20 6D 6F 64 65   // sing in the mode
                                                                                                                                             6C 20 74 77 69 63 65 20 2D 2D 20 6F 6E 63 65 20   // l twice -- once 
                                                                                                                                             76 69 61 20 74 68 65 20 6D 6F 64 65 6C 45 78 70   // via the modelExp
                                                                                                                                             72 65 73 73 69 6F 6E 20 70 61 72 61 6D 65 74 65   // ression paramete
                                                                                                                                             72 20 61 6E 64 20 61 67 61 69 6E 20 69 6E 20 74   // r and again in t
                                                                                                                                             68 65 20 76 69 65 77 44 61 74 61 20 70 61 72 61   // he viewData para
                                                                                                                                             6D 65 74 65 72 2E 20 20 54 68 65 20 76 69 65 77   // meter.  The view
                                                                                                                                             44 61 74 61 20 70 61 72 61 6D 65 74 65 72 20 77   // Data parameter w
                                                                                                                                             61 73 20 6D 65 61 6E 74 20 6F 6E 6C 79 20 74 6F   // as meant only to
                                                                                                                                             20 70 72 6F 76 69 64 65 20 61 20 77 61 79 20 74   //  provide a way t
                                                                                                                                             6F 20 70 61 73 73 20 69 6E 20 77 65 61 6B 6C 79   // o pass in weakly
                                                                                                                                             20 74 79 70 65 64 20 76 69 65 77 20 64 61 74 61   //  typed view data
                                                                                                                                             20 77 68 69 63 68 20 69 73 20 6D 6F 72 65 20 61   //  which is more a
                                                                                                                                             70 70 72 6F 72 69 61 74 65 6C 79 20 64 6F 6E 65   // pproriately done
                                                                                                                                             20 77 69 74 68 20 61 6E 20 61 72 72 61 79 20 6F   //  with an array o
                                                                                                                                             66 20 6B 65 79 20 76 61 6C 75 65 20 70 61 69 72   // f key value pair
                                                                                                                                             73 2E 00 00 )                                     // s...
    .custom /*0C000092:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x594e
    // Code size       20 (0x14)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 04   |                  */ ldarg.2
    IL_0003:  /* 73   | (0A)0001E6       */ newobj     instance void class MvcContrib.FluentHtml.PartialRenderer`2/*0200003A*/<!!T,!!TPartialViewModel>/*1B000082*/::.ctor(class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!0>,
                                                                                                                                                                           string,
                                                                                                                                                                           class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!0,!1>>) /* 0A0001E6 */
    IL_0008:  /* 05   |                  */ ldarg.3
    IL_0009:  /* 28   | (0A)0001E8       */ call       instance class MvcContrib.FluentHtml.PartialRenderer`2/*0200003A*/<!0,!1> class MvcContrib.FluentHtml.PartialRenderer`2/*0200003A*/<!!T,!!TPartialViewModel>/*1B000082*/::ViewData(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/) /* 0A0001E8 */
    IL_000e:  /* 6F   | (0A)0001E9       */ callvirt   instance void class MvcContrib.FluentHtml.PartialRenderer`2/*0200003A*/<!!T,!!TPartialViewModel>/*1B000082*/::Render() /* 0A0001E9 */
    IL_0013:  /* 2A   |                  */ ret
  } // end of method ViewModelContainerExtensions::RenderPartial

  .method /*060001A5*/ public hidebysig static 
          void  RenderPartial<class T,class TPartialViewModel>(class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!T> view,
                                                               string partialViewName,
                                                               class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!T,!!TPartialViewModel>> modelExpression) cil managed
  // SIG: 10 02 03 01 15 12 81 40 01 1E 00 0E 15 12 69 01 15 12 2D 02 1E 00 1E 01
  {
    .custom /*0C000093:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x5963
    // Code size       14 (0xe)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 04   |                  */ ldarg.2
    IL_0003:  /* 73   | (0A)0001E6       */ newobj     instance void class MvcContrib.FluentHtml.PartialRenderer`2/*0200003A*/<!!T,!!TPartialViewModel>/*1B000082*/::.ctor(class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!0>,
                                                                                                                                                                           string,
                                                                                                                                                                           class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!0,!1>>) /* 0A0001E6 */
    IL_0008:  /* 28   | (0A)0001E9       */ call       instance void class MvcContrib.FluentHtml.PartialRenderer`2/*0200003A*/<!!T,!!TPartialViewModel>/*1B000082*/::Render() /* 0A0001E9 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method ViewModelContainerExtensions::RenderPartial

  .method /*060001A6*/ public hidebysig static 
          void  RenderPartial<class T,class TPartialViewModel>(class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!!T> view,
                                                               string partialViewName,
                                                               class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!!T,!!TPartialViewModel>> modelExpression,
                                                               class [System.Core/*23000002*/]System.Func`2/*0100000B*/<string,object>[] viewDataItems) cil managed
  // SIG: 10 02 04 01 15 12 81 40 01 1E 00 0E 15 12 69 01 15 12 2D 02 1E 00 1E 01 1D 15 12 2D 02 0E 1C
  {
    .custom /*0C000094:0A00000F*/ instance void [System.Core/*23000002*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000030*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    .param [4]/*080001D9*/ 
    .custom /*0C000095:0A000002*/ instance void [mscorlib/*23000001*/]System.ParamArrayAttribute/*01000022*/::.ctor() /* 0A000002 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x5974
    // Code size       82 (0x52)
    .maxstack  4
    .locals /*11000033*/ init (class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/ V_0,
             class [System.Core/*23000002*/]System.Func`2/*0100000B*/<string,object> V_1,
             class [System.Core/*23000002*/]System.Func`2/*0100000B*/<string,object>[] V_2,
             int32 V_3)
    IL_0000:  /* 73   | (0A)0001EA       */ newobj     instance void [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/::.ctor() /* 0A0001EA */
    IL_0005:  /* 0A   |                  */ stloc.0
    IL_0006:  /* 05   |                  */ ldarg.3
    IL_0007:  /* 0C   |                  */ stloc.2
    IL_0008:  /* 16   |                  */ ldc.i4.0
    IL_0009:  /* 0D   |                  */ stloc.3
    IL_000a:  /* 2B   | 2C               */ br.s       IL_0038

    IL_000c:  /* 08   |                  */ ldloc.2
    IL_000d:  /* 09   |                  */ ldloc.3
    IL_000e:  /* 9A   |                  */ ldelem.ref
    IL_000f:  /* 0B   |                  */ stloc.1
    IL_0010:  /* 06   |                  */ ldloc.0
    IL_0011:  /* 07   |                  */ ldloc.1
    IL_0012:  /* 6F   | (0A)000068       */ callvirt   instance class [mscorlib/*23000001*/]System.Reflection.MethodInfo/*01000010*/ [mscorlib/*23000001*/]System.Delegate/*0100003D*/::get_Method() /* 0A000068 */
    IL_0017:  /* 6F   | (0A)000069       */ callvirt   instance class [mscorlib/*23000001*/]System.Reflection.ParameterInfo/*01000056*/[] [mscorlib/*23000001*/]System.Reflection.MethodBase/*0100004F*/::GetParameters() /* 0A000069 */
    IL_001c:  /* 16   |                  */ ldc.i4.0
    IL_001d:  /* 9A   |                  */ ldelem.ref
    IL_001e:  /* 6F   | (0A)00006A       */ callvirt   instance string [mscorlib/*23000001*/]System.Reflection.ParameterInfo/*01000056*/::get_Name() /* 0A00006A */
    IL_0023:  /* 07   |                  */ ldloc.1
    IL_0024:  /* 14   |                  */ ldnull
    IL_0025:  /* 6F   | (0A)0001EB       */ callvirt   instance !1 class [System.Core/*23000002*/]System.Func`2/*0100000B*/<string,object>/*1B000083*/::Invoke(!0) /* 0A0001EB */
    IL_002a:  /* 73   | (0A)0001EC       */ newobj     instance void valuetype [mscorlib/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000076*/<string,object>/*1B000084*/::.ctor(!0,
                                                                                                                                                                                             !1) /* 0A0001EC */
    IL_002f:  /* 6F   | (0A)0001ED       */ callvirt   instance void [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/::Add(valuetype [mscorlib/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000076*/<string,object>) /* 0A0001ED */
    IL_0034:  /* 09   |                  */ ldloc.3
    IL_0035:  /* 17   |                  */ ldc.i4.1
    IL_0036:  /* 58   |                  */ add
    IL_0037:  /* 0D   |                  */ stloc.3
    IL_0038:  /* 09   |                  */ ldloc.3
    IL_0039:  /* 08   |                  */ ldloc.2
    IL_003a:  /* 8E   |                  */ ldlen
    IL_003b:  /* 69   |                  */ conv.i4
    IL_003c:  /* 32   | CE               */ blt.s      IL_000c

    IL_003e:  /* 02   |                  */ ldarg.0
    IL_003f:  /* 03   |                  */ ldarg.1
    IL_0040:  /* 04   |                  */ ldarg.2
    IL_0041:  /* 73   | (0A)0001E6       */ newobj     instance void class MvcContrib.FluentHtml.PartialRenderer`2/*0200003A*/<!!T,!!TPartialViewModel>/*1B000082*/::.ctor(class MvcContrib.FluentHtml.IViewModelContainer`1/*02000050*/<!0>,
                                                                                                                                                                           string,
                                                                                                                                                                           class [System.Core/*23000002*/]System.Linq.Expressions.Expression`1/*0100001A*/<class [System.Core/*23000002*/]System.Func`2/*0100000B*/<!0,!1>>) /* 0A0001E6 */
    IL_0046:  /* 06   |                  */ ldloc.0
    IL_0047:  /* 28   | (0A)0001E8       */ call       instance class MvcContrib.FluentHtml.PartialRenderer`2/*0200003A*/<!0,!1> class MvcContrib.FluentHtml.PartialRenderer`2/*0200003A*/<!!T,!!TPartialViewModel>/*1B000082*/::ViewData(class [System.Web.Mvc/*23000003*/]System.Web.Mvc.ViewDataDictionary/*0100001C*/) /* 0A0001E8 */
    IL_004c:  /* 6F   | (0A)0001E9       */ callvirt   instance void class MvcContrib.FluentHtml.PartialRenderer`2/*0200003A*/<!!T,!!TPartialViewModel>/*1B000082*/::Render() /* 0A0001E9 */
    IL_0051:  /* 2A   |                  */ ret
  } // end of method ViewModelContainerExtensions::RenderPartial

} // end of class MvcContrib.FluentHtml.ViewModelContainerExtensions


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
// WARNING: Created Win32 resource file MvcContrib.FluentHtml.res
