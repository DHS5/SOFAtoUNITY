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
* exporting the result in .fbx format with camera animation


## What you can do

### Animation

You can play/pause the animation, change the animation speed from fast to slow-mo and reverse and finally scroll through the animation thanks to a slider.

https://user-images.githubusercontent.com/94963203/178690164-7d177498-6d9a-445d-85a5-54532cf46a3a.mp4

### View

You can turn around the object easily with mouse and keyboard shortcuts, zoom in and out, go up and down.

https://user-images.githubusercontent.com/94963203/178688597-51f13de4-e2e9-4646-bb92-a2096252549b.mp4

### Lighting

You have 9 lights at your disposal, 3 directionnal lights, 3 point lights and 3 spot lights. You can configure their orientation, intensity, color and more. You can save all your changes into presets automatically saved between sessions.

https://user-images.githubusercontent.com/94963203/178689747-6f72e453-b778-4d03-b2b2-5f88053d8486.mp4

### Rendering

You can choose to enable or not the different sub-objects of any object you are displaying and change the material of the sub-objects independently.
You can also apply a wireframe shader on the sub-objects independently to visualize the meshes during the animation.
Of course the wireframe shader and all materials are configurable at runtime.

https://user-images.githubusercontent.com/94963203/178695522-981778d2-b13b-425e-87a6-eca198c5f096.mp4


## Project configurations

By default, all 3D models in the folder 'Assets > Models' having the .blend extension will be "prepared", saved as assets, stored in a ScriptableObject and instantiated when entering play-mode.
* The "preparation" step consist of component adding to the different sub-objects and the main object contained in the model.
* The saving step consist of saving the model as a prefab and saving the animation and animatorOverrideController as prefabs too, all in a folder named after the model
* The storing step is just adding the model's prefab to a prefab list. All prefabs in this list will be instantiated at runtime.
To be precise, the "preparation" and saving steps happens to every model which name doesn't match a folder name in the folder 'Assets > AnimatedModels'. The storing step happens to every prefab having a folder but being absent of the prefab list.
You can add other format extensions to the AssetManager 'Formats' list so that models with other formats will be automatically saved as well.
The AssetManager script is situated on the MainManager GameObject in the scene.
You can add other 3D model formats but you are responsible of making sure that the content of the model corresponds to the expectations.
The expectations being a 3D model with at least one object with a mesh/skinned mesh renderer and only one animation.

To add new materials to the list of runtime' available materials, simply add your new material to the materials list of the 'TextureContainer' ScriptableObject in 'Assets > ScriptableObjects' folder.

You can modify some script's values in the Editor given the result you want.
For example : the max speed for an animation is x16 but if you want more or less, you can modify the value in the AnimatorManager component of the MainManager in the scene.
All useful scripts are components of the MainManager GameObject in the scene, I recommend not changing anything in the scene that isn't a value in those scripts.

Of course, you can add any GameObject in the scene to use as a background or anything you want as long as it doesn't require to interact with the original system.


## Limits

Currently, the project doesn't support models with several animations.
The objects with several sub-objects must have a single animation which animate the whole object and not only a single sub-object.
The perfect usecase is a .blend file with a 'Scene' animation that was baked in Blender and contains the animation of all the sub-objects.

