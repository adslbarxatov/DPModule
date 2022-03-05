_Changes for v 13.2.1_:
- Adjusted notifications appearance;
- Updated “App about” interface;
- Adjusted packages downloading method;
- Exit and deploy actions now have different buttons;
- Implemented the protocol processing for ```dp://``` alias. F.e., the call ```dp://TextToKKT``` will start downloading of the corresponding package. Association for this alias will be added to OS settings automatically;
- Implemented the ability to download non-deployable files (i.e., the ability to skip deployment): ```.exe```, ```.apk```, etc.;
- App will not show requirements window on second, third and subsequent deployments, if “close on success” flag is set;
- App now able to display alerts if they described in the packages list;
- Removed self-update function from the “About the app” interface
