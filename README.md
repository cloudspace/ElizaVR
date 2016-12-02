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

2. If you have configured the Text to Speech and Speech services already, go to step 3.  Otherwise, complete the following steps
    1. Log in to Bluemix at https://bluemix.net.
    2. Navigate to the **Dashboard** on your Bluemix account.
    3. Click the **tile** for Speech to Text.
    4. Click **Service Credentials**. Note: If your browser window is too narrow, the service options may be collapsed. Click on the upward facing double arrow next to "Back to Dashboard..." on the upper left to expand the sidebar.
    5. Copy the content in the **Service Credentials** field, and paste it in the credentials field in the Config Editor (**Watson -> Config Editor**) in Unity.
    6. Click **Apply Credentials**.
    7. Repeat steps 1 - 5 for Text to Speech.

3. If you need to configure the services that you want to use, complete the following steps.
    1. In the Config Editor (**Watson -> Config Editor**), click the **Configure** button beside the service to register. The service window is displayed.
    2. Under **Add Service**, type a unique name for the service instance in the Service name field. For example, type 'speech-to-text'. Leave the default values for the other options.
    3. Click **Create**.
    4. Click **Service Credentials**. Note: If your browser window is too narrow, the service options may be collapsed. Click on the upward facing double arrow next to "Back to Dashboard..." on the upper left to expand the sidebar.
    5. Copy the content in the **Service Credentials** field, and paste it in the empty credentials field in the **Config Editor** in Unity.
    6. Click **Apply Credentials**.
    7. Repeat steps 1 - 5 for Text to Speech.

Once you have created them, configure them within Unity by following step #3 - selecting the aforementioned services.
This will create a configuration file at Assets/StreamingAssets/Config.json that will be used by Watson to keep track of your credentials.

4. Install the "Realistic Eye Movements" plugin from the Unity Asset Store

5. Install the "Salsa Text-to-Speech" plugin from the Asset Store.

6. There are two cameras in the app - one for building Cardboard builds named "Cardboard" and one named "Gear" for the GearVR build.  In the Unity Inspector, show the camera that matches the build output you wish to build for, and deactivate the other.  Make sure that only one camera is active at a time.

7.  If you wish to build for Gear VR, make sure that "Virtual Reality Support" is checked under the Player Settings.  If not, make sure that it is not checked.
