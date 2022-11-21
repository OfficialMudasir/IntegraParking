sn -k keyPairPushSharp.Amazon.snk
ildasm PushSharp.Amazon.dll /out:PushSharp.Amazon.il
ren PushSharp.Amazon.dll PushSharp.Amazon.dll.orig
ilasm PushSharp.Amazon.il /dll /key=keyPairPushSharp.Amazon.snk


sn -k keyPairPushSharp.Apple.snk
ildasm PushSharp.Apple.dll /out:PushSharp.Apple.il
ren PushSharp.Apple.dll PushSharp.Apple.dll.orig
ilasm PushSharp.Apple.il /dll /key=keyPairPushSharp.Apple.snk


sn -k keyPairPushSharp.Blackberry.snk
ildasm PushSharp.Blackberry.dll /out:PushSharp.Blackberry.il
ren PushSharp.Blackberry.dll PushSharp.Blackberry.dll.orig
ilasm PushSharp.Blackberry.il /dll /key=keyPairPushSharp.Blackberry.snk


sn -k keyPairPushSharp.Core.snk
ildasm PushSharp.Core.dll /out:PushSharp.Core.il
ren PushSharp.Core.dll PushSharp.Core.dll.orig
ilasm PushSharp.Core.il /dll /key=keyPairPushSharp.Core.snk


sn -k keyPairPushSharp.Firefox.snk
ildasm PushSharp.Firefox.dll /out:PushSharp.Firefox.il
ren PushSharp.Firefox.dll PushSharp.Firefox.dll.orig
ilasm PushSharp.Firefox.il /dll /key=keyPairPushSharp.Firefox.snk


sn -k keyPairPushSharp.Google.snk
ildasm PushSharp.Google.dll /out:PushSharp.Google.il
ren PushSharp.Google.dll PushSharp.Google.dll.orig
ilasm PushSharp.Google.il /dll /key=keyPairPushSharp.Google.snk


sn -k keyPairPushSharp.Windows.snk
ildasm PushSharp.Windows.dll /out:PushSharp.Windows.il
ren PushSharp.Windows.dll PushSharp.Windows.dll.orig
ilasm PushSharp.Windows.il /dll /key=keyPairPushSharp.Windows.snk


