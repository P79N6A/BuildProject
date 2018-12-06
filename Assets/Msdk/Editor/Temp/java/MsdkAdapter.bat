C:
cd "C:\Users\jinas\Desktop\Projects\ProjectTest\Assets/Msdk/Editor/Temp/java"
mkdir classes
javac -source 1.6 -target 1.6 -nowarn -encoding utf8 -cp "C:\Users\jinas\Desktop\Projects\ProjectTest\Assets\Msdk\Editor\Librarys\Android3.2/MSDKLibrary/libs\MSDK_Android_3.2.8a_92312.jar;C:/Users/jinas/AppData/Local/Android/Sdk/platforms\android-28/android.jar;C:\Users\jinas\Desktop\Projects\ProjectTest\Assets\Msdk\Editor\Librarys\Android3.2/UnityClasses.jar" -d classes ./src/com/example/wegame/*.java ./src/com/example/wegame/wxapi/*.java ./src/com/tencent/msdk/adapter/*.java
cd classes
jar cvf msdk_unity_adapter_3.2.10u.jar com
exit
