# SOFAtoUNITY
### Unity project allowing the user to display an animated object (format .blend, .fbx ...) with ready-to-use and configurable textures, lights, wireframe shader etc...


## Presentation

This project was initially created to display an animated (or not) object coming from SOFA with multiple rendering, lighting, animating and texturing options.
It supports a wide range of 3D model formats and is completely customizable.


## Usecase

The main usecase is the observation of an animated object under different angles, different lightings and with different textures/materials.

It can be useful for many reasons :
* observing precisely and direclty in Unity the animations of a character or object
* observing the behaviour of the vertices of an animated object during the animation
* testing different textures on an object and see the result with different lighting settings
* exporting the result in .fbx format with camera and lights animation

## How to use

Download the zip file, unzip it and add the folder to the Unity Hub as a project.

Once in the project, you can direclty enter play-mode to test the project on some examples of animated models coming from SOFA.

By default, all 3D models in the folder 'Assets > Models' having the .blend extension will be "prepared", saved as assets, stored in a ScriptableObject and instantiated when entering play-mode.
* The "preparation" step consist of component adding to the different sub-objects and the main object contained in the model.
* The saving step consist of saving the model as a prefab and saving the animation and animatorOverrideController as prefabs too, all in a folder named after the model
* The storing step is just adding the model's prefab to a prefab list. All prefabs in this list will be instantiated at runtime.
To be precise, the "preparation" and saving steps happens to every model which name doesn't match a folder name in the folder 'Assets > AnimatedModels'. The storing step happens to every prefab having a folder but being absent of the prefab list.
You can add other format extensions to the AssetManager 'Formats' list so that models with other formats will be automatically saved as well.
The AssetManager script is situated on the MainManager GameObject in the scene.
You can add other 3D model formats but you are responsible of making sure that the content of the model corresponds to the expectations.
The expectations being a 3D model with at least one object with a mesh/skinned mesh renderer and only one animation.

To make sure that a model coming from SOFA is ready to be implemented in the project, follow these steps :
* If the model is a .obj with .mdd for the animation, do as follow :
- import your .obj with Geometry > Keep verts order selected
- select the model in your scene and import your .mdd
* Once you imported your animated model (from .mdd or other), select it and enter edit-mode (Tab)
* Select the whole mesh (A)
* Go to meshs > Normals > Recalculate Outside
* Delete the camera and the light from the scene
* Select the model and go to Object > Animation > Bake Action

If everything went fine, you can now go to the Unity project and import your .blend in the 'Models' folder. The imported .blend should contain a single animation clip named 'Scene', the meshes and the sub objects.

Now you just have to press play and the model should be added to the 'Object' dropdown in the Settings during play-mode. You also should find a new folder with the object's name in 'Assets > AnimatedModels'.


## What you can do

### Animation

You can play/pause the animation, change the animation speed from fast to slow-mo and reverse and finally scroll through the animation thanks to a slider (only when paused).


https://user-images.githubusercontent.com/94963203/181253439-5768f985-5da1-48f2-9180-d7a71df451ff.mp4



### View

You can turn around the object easily with mouse and keyboard shortcuts, zoom in and out, go up and down.



https://user-images.githubusercontent.com/94963203/181253753-fd243138-8eb2-4098-8623-fc58da6e9801.mp4



### Lighting

You have 9 lights at your disposal, 3 directionnal lights, 3 point lights and 3 spot lights. You can configure their orientation, intensity, color and more. You can save all your changes into presets or direclty create new presets that are automatically saved between sessions.



https://user-images.githubusercontent.com/94963203/181254567-6c2ccde5-f48e-416f-94fb-8a4f754a97d7.mp4



### Rendering

You can choose to enable or not the different sub-objects of any object you are displaying and change the material of the sub-objects independently.
You can also apply a wireframe shader on the sub-objects independently to visualize the meshes during the animation.
Of course the wireframe shader and all materials are configurable at runtime.



https://user-images.githubusercontent.com/94963203/181255196-823ca8e7-baf4-4d5a-b8cb-8de7d7b1965f.mp4



### Background

The background object helps visualizing the object on a solid color, curvy background. You can activate/desactivate the background, change its color, distance and altitude.



https://user-images.githubusercontent.com/94963203/181259772-e1508154-4ea0-42f6-a7a7-e7ff80c5dc89.mp4



### Depth of field

You can simulate depth of field thanks to the 'Camera View' settings panel. You can activate/desactivate depth of field, change the focal length, focus distance and aperture.


https://user-images.githubusercontent.com/94963203/181260367-8b2ada4b-d1db-44e2-ba15-9c1faf66ca41.mp4




### FBX Scene recording

You can record the scene into a prefab that then can be exported as an FBX file.
To do so you can use the 'Recording window' present in play-mode.
It will create a new prefab that contains the object active during recording, the camera and the lights active during recording. Along with this prefab will be saved an animation clip containing the informations on the objects cited earlier ; and the materials used during the recording.

This prefab can be exported as an FBX file simply by selectionning the prefab then going to 'GameObject > export to FBX' and export it your chosen location. Beware, the animations being made by blendshapes, make the FBX files thus created relatively heavy (~ > 100 MB).

IMPORTANT : do NOT change the active object during recording, nor the object's material(s). Only the objects and materials active at the end of the recording can be saved in the prefab. You can however change the materials, lights, objects etc... properties and transforms.



https://user-images.githubusercontent.com/94963203/181260887-bf356d0a-a957-468c-81c9-f6d10fb56467.mp4

### VR

If you want to experience this project in VR, go check the SOFAtoUNITY_VRviewer : https://github.com/DHS5/SOFAtoUNITY_VRviewer


## Project configurations

To add new materials to the list of runtime' available materials, simply add your new material to the materials list of the 'TextureContainer' ScriptableObject in 'Assets > ScriptableObjects' folder.

You can modify some script's values in the Editor given the result you want.
For example : the max speed for an animation is x32 but if you want more or less, you can modify the value in the AnimatorManager component of the MainManager in the scene.
All useful scripts are components of the MainManager GameObject in the scene, I recommend not changing anything in the scene that isn't a value in those scripts.

Of course, you can add any GameObject in the scene to use as a background or anything you want as long as it doesn't require to interact with the original system.


## Limits

Currently, the project doesn't support models with several animations.
The objects with several sub-objects must have a single animation which animate the whole object and not only a single sub-object.
The perfect usecase is a .blend file with a 'Scene' animation that was baked in Blender and contains the animation of all the sub-objects.

## Help

Of course, a help window is present at runtime for the shortcuts etc...
If you encounter any bugs or have a hard time getting the result you wanted, create a new issue on this GitHub project, I'll see what I can do.
