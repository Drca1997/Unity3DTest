# Unity3DTest

A simple AR application where the user can select a 3D model to place in the 3D world
The first time that each model is required by the user, the application sends a HTTPRequest to a server to download the .glb file of the model.
The following times, since the model is in cache, the application loads the .glb file directly from the directory, in order to diminish loading time.
