#!/usr/bin/python
import os
import sys
import re
import subprocess
from distutils import dir_util
from HZmod_pbxproj import XcodeProject

def edit_pbxproj_file():
    try:
        print "Heyzap: HeyzapPostProcessBuildPlayer started."
        unityProjectTopDirectory = sys.argv[1]
        for xcodeproj in os.listdir(unityProjectTopDirectory):
            if not re.search('\.xcodeproj', xcodeproj):
                continue
            xcodeproj = os.path.join(unityProjectTopDirectory, xcodeproj)
            xcodeVersionString = subprocess.check_output("xcodebuild -version", shell=True)
            print "Heyzap: using xcode version: ", xcodeVersionString

            print "Heyzap: found xcode proj: ", xcodeproj
            for pbxproj in os.listdir(xcodeproj):
                if not re.search('project\.pbxproj', pbxproj):
                    continue
                pbxproj = os.path.join(xcodeproj, pbxproj)
                print "Heyzap: found pbxproj: ", pbxproj
                
                # locate the id of the "Frameworks" group of the pbxproj file so that frameworks will go to that group
                frameworksGroupID = None
                textfile = open(pbxproj, 'r')
                filetext = textfile.read()
                textfile.close()
                matches = re.findall("([0-9A-F]*) /\* Frameworks \*/ = \{\n\s*isa = PBXGroup;", filetext)
                try:
                    frameworksGroupID = matches[0]
                    print "Heyzap: found frameworks group: ", frameworksGroupID
                except:
                    print "Heyzap: did not find frameworks group."
                    pass

                print "Heyzap: loading xcode project."
                project = XcodeProject.Load(pbxproj)
                print "Heyzap: done loading xcode project. Adding libs."

                # the below paths are relative to the SDKROOT, i.e.: `/Applications/Xcode.app/Contents/Developer/Platforms/iPhoneOS.platform/Developer/SDKs/iPhoneOS8.3.sdk/`
                # Add the Frameworks needed

                isXcode6 = "Xcode 6" in xcodeVersionString
                libraryExtension = ".dylib" if isXcode6 else ".tbd"

                project.add_file_if_doesnt_exist('usr/lib/libxml2' + libraryExtension,                       parent=frameworksGroupID, tree='SDKROOT')
                project.add_file_if_doesnt_exist('usr/lib/libsqlite3' + libraryExtension,                    parent=frameworksGroupID, tree='SDKROOT')

                
                project.add_file_if_doesnt_exist('System/Library/Frameworks/WebKit.framework',  parent=frameworksGroupID, tree='SDKROOT')
                project.add_file_if_doesnt_exist('System/Library/Frameworks/MobileCoreServices.framework',  parent=frameworksGroupID, tree='SDKROOT')
                project.add_file_if_doesnt_exist('System/Library/Frameworks/AdSupport.framework',  parent=frameworksGroupID, tree='SDKROOT')
                project.add_file_if_doesnt_exist('System/Library/Frameworks/StoreKit.framework',  parent=frameworksGroupID, tree='SDKROOT')
                project.add_file_if_doesnt_exist('System/Library/Frameworks/CoreTelephony.framework',  parent=frameworksGroupID, tree='SDKROOT')
                project.add_file_if_doesnt_exist('usr/lib/libz' + libraryExtension,                          parent=frameworksGroupID, tree='SDKROOT')

                print "Heyzap: done adding libs, adding flags."
                # Add -ObjC for the benefit of AppLovin/FAN
                project.add_other_ldflags("-ObjC")

                # Enable modules for the benefit of AdMob.
                # (This allows automatic linking for the frameworks they use)
                project.add_other_buildsetting("CLANG_ENABLE_MODULES", "YES")

                project.save()
                print "Heyzap: successfully modified file: ", pbxproj
                return 0
        raise FileExistsError("Could not find the 'project.pbxproj' file to edit")
    except Exception as e:
      print "Heyzap: ERROR modifying 'project.pbxproj', error: ", e
      return 1

sys.exit(edit_pbxproj_file())
