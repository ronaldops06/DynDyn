# Add project specific ProGuard rules here.
# By default, the flags in this file are appended to flags specified
# in /usr/local/Cellar/android-sdk/24.3.3/tools/proguard/proguard-android.txt
# You can edit the include path and order by changing the proguardFiles
# directive in build.gradle.
#
# For more details, see
#   http://developer.android.com/guide/developing/tools/proguard.html

# Add any project specific keep options here:

# Corrige problema com com.google.errorprone.annotations.Immutable
-keep class com.google.errorprone.annotations.Immutable { *; }

# (Opcional) Garante que tudo da biblioteca do Tink seja mantido
-keep class com.google.crypto.tink.** { *; }
-dontwarn com.google.errorprone.**
