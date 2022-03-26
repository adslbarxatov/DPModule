_Changes for v 13.3.1_:
- Some improvements applied to the user interface;
- Implemented the support for reserve list of packages on VK.com (for urgent cases);
- Improved the algorithm of an unzipper selection;
- Adjusted packages downloading method;
- Exit and deploy actions now have different buttons;
- Implemented the protocol processing for ```dp://``` alias. F.e., the call ```dp://TextToKKT``` will start downloading of the corresponding package. Association for this alias will be added to OS settings automatically;
- Implemented the ability to download non-deployable files (i.e., the ability to skip deployment): ```.exe```, ```.apk```, etc.;
- App will not show requirements window on second, third and subsequent deployments, if “close on success” flag is set
