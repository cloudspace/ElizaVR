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

1) Register for your service credentials, as described [by Watson's documentation](https://github.com/watson-developer-cloud/unity-sdk#configuring-your-service-credentials)

The services we are using are "Speech to Text" and "Text$ to Speech", so create these two services as described by step #2 of "configuring your service credentials."

Once you have created them, configure them within Unity by following step #3 - selecting the aforementioned services.
This will create a configuration file at Assets/StreamingAssets/Config.json that will be used by Watson to keep track of your credentials.

2) Install the "Realistic Eye Movements" plugin from the Unity Asset Store

3) Install the "Salsa Text-to-Speech" plugin from the Asset Store.

4) There are two cameras in the app - one for building Cardboard builds named "Cardboard" and one named "Gear" for the GearVR build.  In the Unity Inspector, show the camera that matches the build output you wish to build for, and deactivate the other.  Make sure that only one camera is active at a time.

5)  If you wish to build for Gear VR, make sure that "Virtual Reality Support" is checked under the Player Settings.  If not, make sure that it is not checked.
