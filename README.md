# Eliza VR Unity Engine

This is a virtual reality instance of the classic "Eliza" AI therapist engine written with [Unity](http://unity.com) to run on either Google Cardboard or GearVR.

## Plugins Used

### [The Watson Unity SDK](https://github.com/watson-developer-cloud/unity-sdk) 

Used to speak and hear via the IBM Watson service.  For this reason, you must set up a Watson account as defined in the watson unity sdk documentation in order to use this project.

### [Salsa Text-to-Speech Engine](http://crazyminnowstudio.com/unity-3d/lip-sync-salsa/)

Responsible for making the character's lips move when they speak.  This must be installed from the Unity Asset store for the project to work.

### [Realistic Eye Movements](https://www.assetstore.unity3d.com/en/#!/content/29168)

Adds realistic eye movements to the project.  This must be installed from the unity asset store to work.

### [RiveScript-Csharp](https://github.com/fabioravila/rivescript-csharp)

This was ported to 2.0 and included in this project to act as the AI.

### [MsgPack](https://github.com/msgpack/msgpack-cli)

Used to cache a compiled copy of the rive script on the client's machine (for use with larger rive scripts)

## Steps to build:

1. If you do not have a bluemix account (at http://bluemix.net), go and create one.

2. Complete the following steps to configure speech-to-text and text-to-speech. 
    1. Log in to Bluemix at https://bluemix.net.
    2. Navigate to the **Dashboard** on your Bluemix account.
    3. Click the "Create Service" button.
    3. Click the **tile** for Speech to Text, and then click "Create."  You should be picking from that list from [here](https://console.ng.bluemix.net/catalog/?category=watson&taxonomyNavigation=watson).
    4. Click **Service Credentials**. Note: If your browser window is too narrow, the service options may be collapsed. Click on the upward facing double arrow next to "Back to Dashboard..." on the upper left to expand the sidebar.
    The [Watson dashboard](https://console.ng.bluemix.net/dashboard/watson) is available under Watson, and should list the Watson parameters.
    5. Copy the content in the **Service Credentials** field, and paste it in the credentials field in the Config Editor (**Watson -> Config Editor**) in Unity.
    6. Click **Apply Credentials**.
    7. Repeat steps 1 - 6 for Text to Speech.

This will create a configuration file at Assets/StreamingAssets/Config.json that will be used by Watson to keep track of your credentials.

4. Install the "Realistic Eye Movements" plugin from the Unity Asset Store

5. Install the "Salsa Text-to-Speech" plugin from the Unity Asset Store.

6. There are two cameras in the app - one for building Cardboard builds named "Cardboard" and one named "Gear" for the GearVR build.  In the Unity Inspector, show the camera that matches the build output you wish to build for, and deactivate the other.  Make sure that only one camera is active at a time.

7.  If you wish to build for Gear VR, make sure that "Virtual Reality Support" is checked under the Player Settings.  If not, make sure that it is not checked.
